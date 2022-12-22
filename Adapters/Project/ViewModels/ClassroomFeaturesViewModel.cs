using System.Reactive.Disposables;
using Adapters.ViewModels;
using Application.Project.Gateways;
using Domain.Project;
using ReactiveUI;

namespace Adapters.Project.ViewModels;

public class ClassroomFeaturesViewModel : BaseViewModel, IRoutableViewModel, IActivatableViewModel
{
    public delegate ClassroomFeaturesViewModel Factory(IScreen hostScreen);

    private readonly IClassroomFeatureGateway _gateway;

    private readonly ObservableAsPropertyHelper<IEnumerable<Identified<ClassroomFeature>>>
        _features;

    public ClassroomFeaturesViewModel(IScreen hostScreen, IClassroomFeatureGateway gateway)
    {
        Activator = new ViewModelActivator();

        _gateway = gateway;
        HostScreen = hostScreen;

        _features = _gateway
            .ObserveAll()
            .ToProperty(this, vm => vm.Features);

        this.WhenActivated(d => { _features.DisposeWith(d); });
    }

    public string UrlPathSegment => "/ClassroomFeatures";
    public IScreen HostScreen { get; }

    public IEnumerable<Identified<ClassroomFeature>> Features => _features.Value;

    public ViewModelActivator Activator { get; }
}