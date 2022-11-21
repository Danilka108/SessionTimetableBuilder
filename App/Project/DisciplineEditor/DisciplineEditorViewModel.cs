using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using App.CommonControls.MessageWindow;
using Domain;
using Domain.Project.Models;
using Domain.Project.UseCases;
using Domain.Project.UseCases.Discipline;
using ReactiveUI;

namespace App.Project.DisciplineEditor;

public class DisciplineEditorViewModel : ViewModelBase, IActivatableViewModel
{
    public delegate DisciplineEditorViewModel Factory(IdentifiedModel<Discipline>? discipline);

    private readonly SaveDisciplineUseCase _saveUseCase;

    private readonly int? _id;
    private string _name;

    private readonly ObservableAsPropertyHelper<IEnumerable<IdentifiedModel<AudienceSpecificity>>>
        _allRequirements;

    private readonly ObservableAsPropertyHelper<bool> _isLoading;

    private readonly ObservableAsPropertyHelper<bool> _canBeSaved;

    private IEnumerable<IdentifiedModel<AudienceSpecificity>> _selectedRequirements;

    public DisciplineEditorViewModel
    (
        IdentifiedModel<Discipline>? discipline,
        SaveDisciplineUseCase saveUseCase,
        ObserveAllAudienceSpecificitiesUseCase observeAllRequirementsUseCase
    )
    {
        Activator = new ViewModelActivator();
        OpenMessageDialog = new Interaction<MessageWindowViewModel, Unit>();

        _saveUseCase = saveUseCase;
        _id = discipline?.Id;
        Name = discipline?.Model.Name ?? "";

        _allRequirements = observeAllRequirementsUseCase
            .Handle()
            .ToProperty(this, vm => vm.AllRequirements);

        SelectedRequirements = discipline?.Model.AudienceRequirements ??
                               new List<IdentifiedModel<AudienceSpecificity>>();

        Close = ReactiveCommand.Create(() => Unit.Default);

        var canBeSaved = this
            .WhenAnyValue(vm => vm.Name)
            .Select(name => name.Length > 0);

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
                savingErrors
                    .Subscribe()
                    .DisposeWith(d);

                Save
                    .SelectMany(Close.Execute)
                    .Subscribe()
                    .DisposeWith(d);
            }
        );
    }

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
        var model = new Discipline(Name, SelectedRequirements);
        await _saveUseCase.Handle(model, _id);
    }

    public ReactiveCommand<Unit, Unit> Close { get; }

    public ReactiveCommand<Unit, Unit> Save { get; }

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public IEnumerable<IdentifiedModel<AudienceSpecificity>> AllRequirements =>
        _allRequirements.Value;

    public IEnumerable<IdentifiedModel<AudienceSpecificity>> SelectedRequirements
    {
        get => _selectedRequirements;
        set => this.RaiseAndSetIfChanged(ref _selectedRequirements, value);
    }

    public bool IsLoading => _isLoading.Value;

    public bool CanBeSaved => _canBeSaved.Value;

    public Interaction<MessageWindowViewModel, Unit> OpenMessageDialog { get; }

    public ViewModelActivator Activator { get; }
}