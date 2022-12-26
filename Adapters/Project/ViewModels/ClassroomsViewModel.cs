using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Adapters.Common.ViewModels;
using Application.Project.Gateways;
using Domain.Project;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Adapters.Project.ViewModels;

public class ClassroomsViewModel : BaseViewModel, IRoutableViewModel, IActivatableViewModel
{
    public delegate ClassroomsViewModel Factory(IScreen hostScreen);

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    public ClassroomsViewModel(IScreen hostScreen, IClassroomGateway gateway,
        ClassroomCardViewModel.Factory cardFactory,
        MessageDialogViewModel.Factory messageDialogFactory)
    {
        _messageDialogFactory = messageDialogFactory;
        HostScreen = hostScreen;
        Activator = new ViewModelActivator();
        OpenMessageDialog = new Interaction<MessageDialogViewModel, Unit>();

        var cards = gateway
            .ObserveAll()
            .Catch<IEnumerable<Classroom>, Exception>(ex =>
                CatchObservableExceptions(ex).ToObservable())
            .Select(classrooms => classrooms.Select(cardFactory.Invoke))
            .ToPropertyEx(this, vm => vm.Cards);

        this.WhenActivated(d => { cards.DisposeWith(d); });
    }

    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }

    [ObservableAsProperty] public IEnumerable<ClassroomCardViewModel> Cards { get; }

    public ViewModelActivator Activator { get; }

    public string UrlPathSegment => "/Classrooms";

    public IScreen HostScreen { get; }

    private async Task<IEnumerable<Classroom>> CatchObservableExceptions(Exception _)
    {
        var messageDialog = _messageDialogFactory.Invoke(
            LocalizedMessage.Letter.Error,
            new LocalizedMessage.Error.StorageIsNotAvailable()
        );

        await OpenMessageDialog.Handle(messageDialog);

        return new Classroom[] { };
    }
}