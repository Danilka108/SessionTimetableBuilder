using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Adapters.ViewModels;
using Application.Project.Gateways;
using Application.Project.UseCases.ClassroomFeature;
using Domain.Project;
using ReactiveUI;

namespace Adapters.Project.ViewModels;

public class ClassroomFeatureEditorViewModel : BaseViewModel, IActivatableViewModel
{
    public delegate ClassroomFeatureEditorViewModel Factory(int? featureId);

    private readonly int? _featureId;

    private readonly IClassroomFeatureGateway _gateway;

    private readonly SaveClassroomFeatureUseCase _saveUseCase;

    private readonly ObservableAsPropertyHelper<bool> _isLoading;

    private string _description;

    public ClassroomFeatureEditorViewModel(
        int? featureId,
        IClassroomFeatureGateway gateway,
        SaveClassroomFeatureUseCase saveUseCase
    )
    {
        Activator = new ViewModelActivator();

        _gateway = gateway;
        _saveUseCase = saveUseCase;
        _featureId = featureId;

        LoadDescription();

        var canBeSaved = this
            .WhenAnyValue(vm => vm.Description)
            .Select(description => description.Length > 0);

        Save = ReactiveCommand.CreateFromTask(DoSave, canBeSaved);

        var canBeClosed = Save
            .IsExecuting
            .Select(v => !v); 
        
        Close = ReactiveCommand.Create(() => {}, canBeClosed);

        _isLoading = Save
            .IsExecuting
            .ToProperty(this, vm => vm.IsLoading);
        
        this.WhenActivated(d =>
        {
            Save.DisposeWith(d);
        });
    }

    private void LoadDescription()
    {
        if (_featureId is null) Description = "";

        _gateway.Read(_featureId!.Value, CancellationToken.None).ContinueWith(
            async featureTask =>
            {
                var feature = await featureTask;
                Description = feature.Entity.Description;
            });
    }

    private async Task DoSave()
    {
        var newFeature = new ClassroomFeature(Description);

        try
        {
            await _saveUseCase.Handle(newFeature, _featureId);
        }
        catch (SaveClassroomFeatureException)
        {
            throw new NotImplementedException();
        }

        // await Close.Execute(Unit.Default);
    }

    public string Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    public ReactiveCommand<Unit, Unit> Save { get; }

    public ReactiveCommand<Unit, Unit> Close { get; }

    public bool IsLoading => _isLoading.Value;

    public ViewModelActivator Activator { get; }
}