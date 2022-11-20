using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using App.CommonControls.MessageWindow;
using Domain;
using Domain.Project.Models;
using Domain.Project.UseCases;
using ReactiveUI;

namespace App.Project.AudienceSpecificityEditor;

public class AudienceSpecificityEditorViewModel : ViewModelBase, IActivatableViewModel
{
    public delegate AudienceSpecificityEditorViewModel Factory
        (IdentifiedModel<AudienceSpecificity>? specificity);

    private string _description;

    private readonly ObservableAsPropertyHelper<bool> _canBeSaved;

    private readonly ObservableAsPropertyHelper<bool> _isLoading;

    private readonly SaveAudienceSpecificityUseCase _saveAudienceSpecificityUseCase;

    public AudienceSpecificityEditorViewModel
    (
        IdentifiedModel<AudienceSpecificity>? specificity,
        SaveAudienceSpecificityUseCase saveAudienceSpecificityUseCase
    )
    {
        OpenMessageDialog = new Interaction<MessageWindowViewModel, Unit>();
        Activator = new ViewModelActivator();

        Close = ReactiveCommand.Create
        (
            () =>
            {
            }
        );

        _saveAudienceSpecificityUseCase = saveAudienceSpecificityUseCase;
        _description = specificity?.Model.Description ?? "";

        var canBeSaved = this
            .WhenAnyValue(vm => vm.Description)
            .Select(description => description.Length > 0);

        Save = ReactiveCommand
            .CreateFromTask
            (
                async () => await _saveAudienceSpecificityUseCase.Handle
                    (new AudienceSpecificity(_description), specificity?.Id),
                canBeSaved
            );

        _canBeSaved = canBeSaved
            .ObserveOn(RxApp.MainThreadScheduler)
            .ToProperty(this, vm => vm.CanBeSaved);

        _isLoading = Save
            .IsExecuting
            .ObserveOn(RxApp.MainThreadScheduler)
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

    private async Task<Unit> HandleSavingErrors(Exception e, int _, CancellationToken token)
    {
        var messageViewModel = new MessageWindowViewModel("Error", e.Message);
        await OpenMessageDialog.Handle(messageViewModel);

        return Unit.Default;
    }

    public ReactiveCommand<Unit, Unit> Save { get; }

    public string Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    public bool IsLoading => _isLoading.Value;

    public bool CanBeSaved => _canBeSaved.Value;

    public ReactiveCommand<Unit, Unit> Close { get; }

    public Interaction<MessageWindowViewModel, Unit> OpenMessageDialog { get; }

    public ViewModelActivator Activator { get; }
}