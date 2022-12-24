using System.Reactive.Disposables;
using System.Reactive.Linq;
using Application.Project.Gateways;
using ReactiveUI;

namespace Adapters.Project.ViewModels;

public class ClassroomsViewModel : BaseViewModel, IRoutableViewModel, IActivatableViewModel
{
    public delegate ClassroomsViewModel Factory(IScreen hostScreen);

    private readonly ObservableAsPropertyHelper<IEnumerable<ClassroomCardViewModel>> _cards;

    public ClassroomsViewModel(IScreen hostScreen, IClassroomGateway gateway,
        ClassroomCardViewModel.Factory cardFactory)
    {
        HostScreen = hostScreen;
        Activator = new ViewModelActivator();

        _cards = gateway
            .ObserveAll()
            .Select(classrooms => classrooms.Select(cardFactory.Invoke))
            .ToProperty(this, vm => vm.Cards);

        this.WhenActivated(d => { _cards.DisposeWith(d); });
    }

    public IEnumerable<ClassroomCardViewModel> Cards => _cards.Value;

    public string? UrlPathSegment => "/Classrooms";

    public IScreen HostScreen { get; }

    public ViewModelActivator Activator { get; }
}