using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Adapters.Common.ViewModels;
using Application.Project.Gateways;
using Domain.Project;
using ReactiveUI;

namespace Adapters.Project.ViewModels;

public class ClassroomsViewModel : BaseViewModel, IRoutableViewModel, IActivatableViewModel
{
    public delegate ClassroomsViewModel Factory(IScreen hostScreen);

    private readonly ObservableAsPropertyHelper<IEnumerable<ClassroomCardViewModel>> _cards;

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    public ClassroomsViewModel(IScreen hostScreen, IClassroomGateway gateway,
        ClassroomCardViewModel.Factory cardFactory,
        MessageDialogViewModel.Factory messageDialogFactory)
    {
        _messageDialogFactory = messageDialogFactory;
        HostScreen = hostScreen;
        Activator = new ViewModelActivator();
        OpenMessageDialog = new Interaction<MessageDialogViewModel, Unit>();

        _cards = gateway
            .ObserveAll()
            .Catch<IEnumerable<Classroom>, Exception>(ex =>
                CatchFeaturesObserving(ex).ToObservable())
            .Select(classrooms => classrooms.Select(cardFactory.Invoke))
            .ToProperty(this, vm => vm.Cards);

        this.WhenActivated(d => { _cards.DisposeWith(d); });
    }

    private async Task<IEnumerable<Classroom>> CatchFeaturesObserving(Exception _)
    {
        var messageDialog = _messageDialogFactory.Invoke(
            LocalizedMessage.Header.Error,
            new LocalizedMessage.Error.StorageIsNotAvailable()
        );

        await OpenMessageDialog.Handle(messageDialog);

        return new Classroom[] { };
    }

    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }

    public IEnumerable<ClassroomCardViewModel> Cards => _cards.Value;

    public string UrlPathSegment => "/Classrooms";

    public IScreen HostScreen { get; }

    public ViewModelActivator Activator { get; }
}