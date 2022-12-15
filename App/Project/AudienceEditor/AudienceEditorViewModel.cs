using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.CommonControls.MessageWindow;
using Avalonia.Controls.Mixins;
using Domain;
using Domain.Project.Models;
using Domain.Project.UseCases.Audience;
using Domain.Project.UseCases.AudienceSpecificity;
using ReactiveUI;

namespace App.Project.AudienceEditor;

public class AudienceEditorViewModel : ViewModelBase, IActivatableViewModel
{
    public delegate AudienceEditorViewModel Factory(IdentifiedModel<Audience>? audience);

    private readonly ObservableAsPropertyHelper<IEnumerable<IdentifiedModel<AudienceSpecificity>>>
        _allSpecificities;

    private readonly ObservableAsPropertyHelper<bool> _canBeSaved;

    private readonly int? _id;

    private readonly ObservableAsPropertyHelper<bool> _isLoading;

    private readonly SaveAudienceUseCase _saveUseCase;

    private string _capacity;

    private string _number;

    private IEnumerable<IdentifiedModel<AudienceSpecificity>> _selectedSpecificities;

    public AudienceEditorViewModel
    (
        IdentifiedModel<Audience>? audience,
        ObserveAllAudienceSpecificitiesUseCase observeAllSpecificitiesUseCase,
        SaveAudienceUseCase saveUseCase
    )
    {
        Activator = new ViewModelActivator();
        OpenMessageDialog = new Interaction<MessageWindowViewModel, Unit>();

        _saveUseCase = saveUseCase;

        _id = audience?.Id;
        Number = audience?.Model.Number.ToString() ?? "";
        Capacity = audience?.Model.Capacity.ToString() ?? "";

        SelectedSpecificities = audience?.Model.Specificities ??
                                new List<IdentifiedModel<AudienceSpecificity>>();
        _allSpecificities = observeAllSpecificitiesUseCase
            .Handle()
            .ToProperty(this, vm => vm.AllSpecificities);

        Close = ReactiveCommand.Create(() => Unit.Default);

        var canBeSaved = this.WhenAny
        (
            vm => vm.Number,
            vm => vm.Capacity,
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
                    .SelectMany((_, _) => Close.Execute())
                    .Subscribe()
                    .DisposeWith(d);

                savingErrors
                    .Subscribe()
                    .DisposeWith(d);
            }
        );
    }

    public ReactiveCommand<Unit, Unit> Save { get; }

    public ReactiveCommand<Unit, Unit> Close { get; }

    public Interaction<MessageWindowViewModel, Unit> OpenMessageDialog { get; }

    public string Number
    {
        get => _number;
        set => this.RaiseAndSetIfChanged(ref _number, value);
    }

    public string Capacity
    {
        get => _capacity;
        set => this.RaiseAndSetIfChanged(ref _capacity, value);
    }

    public IEnumerable<IdentifiedModel<AudienceSpecificity>> AllSpecificities =>
        _allSpecificities.Value;

    public IEnumerable<IdentifiedModel<AudienceSpecificity>> SelectedSpecificities
    {
        get => _selectedSpecificities;
        set => this.RaiseAndSetIfChanged(ref _selectedSpecificities, value);
    }

    public bool CanBeSaved => _canBeSaved.Value;

    public bool IsLoading => _isLoading.Value;

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
        var parsedNumber = ParseInt(nameof(Number), Number);
        var parsedCapacity = ParseInt(nameof(Capacity), Capacity);

        var audience = new Audience(parsedNumber, parsedCapacity, SelectedSpecificities);
        await _saveUseCase.Handle(audience, _id);
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
        IObservedChange<AudienceEditorViewModel, string> number,
        IObservedChange<AudienceEditorViewModel, string> capacity
    )
    {
        return number.Value.Length > 0 && capacity.Value.Length > 0;
    }
}