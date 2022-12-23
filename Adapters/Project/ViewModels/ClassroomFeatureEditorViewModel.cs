using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Adapters.Common.ViewModels;
using Adapters.ViewModels;
using Application.Project.Gateways;
using Application.Project.UseCases.ClassroomFeature;
using Domain.Project;
using ReactiveUI;

namespace Adapters.Project.ViewModels;

public class ClassroomFeatureEditorViewModel : BaseViewModel, IActivatableViewModel
{
    public delegate ClassroomFeatureEditorViewModel Factory(Identified<ClassroomFeature>? feature);

    private readonly int? _featureId;

    private readonly IClassroomFeatureGateway _gateway;

    private readonly SaveClassroomFeatureUseCase _saveUseCase;

    private readonly ObservableAsPropertyHelper<bool> _isLoading;

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    private string _description;

    public ClassroomFeatureEditorViewModel(
        Identified<ClassroomFeature>? feature,
        IClassroomFeatureGateway gateway,
        SaveClassroomFeatureUseCase saveUseCase,
        MessageDialogViewModel.Factory messageDialogFactory
    )
    {
        _featureId = feature?.Id;
        Description = feature?.Entity.Description ?? "";

        Activator = new ViewModelActivator();
        OpenMessageDialog = new Interaction<MessageDialogViewModel, Unit>();
        Finish = new Interaction<Unit, Unit>();

        _messageDialogFactory = messageDialogFactory;
        _gateway = gateway;
        _saveUseCase = saveUseCase;

        var canBeSaved = this
            .WhenAnyValue(vm => vm.Description)
            .Select(description => description.Length > 0);

        Save = ReactiveCommand.CreateFromTask(DoSave, canBeSaved);

        var canBeClosed = Save
            .IsExecuting
            .Select(v => !v);

        Close = ReactiveCommand.CreateFromTask(
            async () => { await Finish.Handle(Unit.Default); }, canBeClosed);

        _isLoading = Save
            .IsExecuting
            .ToProperty(this, vm => vm.IsLoading);

        this.WhenActivated(d => { Save.DisposeWith(d); });
    }

    private async Task DoSave()
    {
        var newFeature = new ClassroomFeature(Description);

        try
        {
            await _saveUseCase.Handle(newFeature, _featureId);
            await Finish.Handle(Unit.Default);
        }
        catch (SaveClassroomFeatureException e)
        {
            await OpenMessageDialog.Handle(_messageDialogFactory.Invoke("Error", e.Message));
        }
    }

    public string Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }

    public ReactiveCommand<Unit, Unit> Save { get; }

    public ReactiveCommand<Unit, Unit> Close { get; }

    public Interaction<Unit, Unit> Finish { get; }

    public bool IsLoading => _isLoading.Value;

    public ViewModelActivator Activator { get; }
}