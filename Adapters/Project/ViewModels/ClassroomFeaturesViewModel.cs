using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Adapters.ViewModels;
using Application.Project.Gateways;
using Domain.Project;
using ReactiveUI;

namespace Adapters.Project.ViewModels;

public class ClassroomFeaturesViewModel : BaseViewModel, IRoutableViewModel, IActivatableViewModel
{
    public delegate ClassroomFeaturesViewModel Factory(IScreen hostScreen);

    private readonly ObservableAsPropertyHelper<IEnumerable<ClassroomFeatureCardViewModel>>
        _cards;

    public ClassroomFeaturesViewModel(
        IScreen hostScreen,
        IClassroomFeatureGateway gateway,
        ClassroomFeatureCardViewModel.Factory cardFactory
    )
    {
        Activator = new ViewModelActivator();

        HostScreen = hostScreen;

        _cards = gateway
            .ObserveAll()
            .Select(features => features.Select(cardFactory.Invoke))
            .ToProperty(this, vm => vm.Cards);

        this.WhenActivated(d => { _cards.DisposeWith(d); });
    }

    public IEnumerable<ClassroomFeatureCardViewModel> Cards => _cards.Value;

    public string UrlPathSegment => "/ClassroomFeatures";
    public IScreen HostScreen { get; }

    public ViewModelActivator Activator { get; }
}