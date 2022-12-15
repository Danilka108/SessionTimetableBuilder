using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.CommonControls.MessageWindow;
using Domain;
using Domain.Project.Models;
using Domain.Project.UseCases.BellTime;
using ReactiveUI;

namespace App.Project.BellTimeEditor;

public class BellTimeEditorViewModel : ViewModelBase, IActivatableViewModel
{
    public delegate BellTimeEditorViewModel Factory(IdentifiedModel<BellTime>? bellTime);

    private readonly ObservableAsPropertyHelper<bool> _canBeSaved;
    private readonly int? _id;
    private readonly ObservableAsPropertyHelper<bool> _isLoading;

    private readonly SaveBellTimeUseCase _saveUseCase;

    private string _hour;
    private string _minute;

    public BellTimeEditorViewModel
    (
        IdentifiedModel<BellTime>? bellTime,
        SaveBellTimeUseCase saveUseCase
    )
    {
        Activator = new ViewModelActivator();
        OpenMessageDialog = new Interaction<MessageWindowViewModel, Unit>();

        _saveUseCase = saveUseCase;

        Hour = bellTime?.Model.Hour.ToString() ?? "";
        Minute = bellTime?.Model.Minute.ToString() ?? "";
        _id = bellTime?.Id;

        Close = ReactiveCommand.Create(() => Unit.Default);

        var canBeSaved = this
            .WhenAny
            (
                vm => vm.Hour,
                vm => vm.Minute,
                ValidateSave
            );

        _canBeSaved = canBeSaved
            .ToProperty(this, vm => vm.CanBeSaved);


        Save = ReactiveCommand.CreateFromTask(TrySave, canBeSaved);

        _isLoading = Save
            .IsExecuting
            .ToProperty(this, vm => vm.IsLoading);

        var savingErrors = Save
            .ThrownExceptions
            .SelectMany(HandleSavingErrors);

        this.WhenActivated
        (
            d =>
            {
                Save
                    .SelectMany(_ => Close.Execute())
                    .Subscribe()
                    .DisposeWith(d);

                savingErrors
                    .Subscribe()
                    .DisposeWith(d);
            }
        );
    }

    public bool CanBeSaved => _canBeSaved.Value;

    public bool IsLoading => _isLoading.Value;

    public ReactiveCommand<Unit, Unit> Close { get; }

    public ReactiveCommand<Unit, Unit> Save { get; }

    public Interaction<MessageWindowViewModel, Unit> OpenMessageDialog { get; }

    public string Hour
    {
        get => _hour;
        set => this.RaiseAndSetIfChanged(ref _hour, value);
    }

    public string Minute
    {
        get => _minute;
        set => this.RaiseAndSetIfChanged(ref _minute, value);
    }

    public ViewModelActivator Activator { get; }

    private async Task<Unit> HandleSavingErrors
    (
        Exception e,
        int _,
        CancellationToken token
    )
    {
        var messageViewModel = new MessageWindowViewModel("Error", e.Message);
        await OpenMessageDialog.Handle(messageViewModel);

        return Unit.Default;
    }

    private async Task TrySave()
    {
        var parsedHour = ParseInt(nameof(Hour), Hour);
        var parsedMinute = ParseInt(nameof(Minute), Minute);
        var model = new BellTime(parsedMinute, parsedHour);

        await _saveUseCase.Handle(model, _id);
    }

    private static int ParseInt(string nameofProperty, string value)
    {
        try
        {
            return int.Parse(value);
        }
        catch (Exception)
        {
            throw new FormatException($"Invalid '{nameofProperty}' value. Expected integer.");
        }
    }

    private static bool ValidateSave
    (
        IObservedChange<BellTimeEditorViewModel, string> hour,
        IObservedChange<BellTimeEditorViewModel, string> minute
    )
    {
        return hour.Value.Length > 0 && minute.Value.Length > 0;
    }
}