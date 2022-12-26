using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Adapters.Common.ViewModels;
using Adapters.Project.Browser;
using Application.Project.Gateways;
using Domain.Project;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Adapters.Project.ViewModels;

public class LecturersViewModel : BaseViewModel, IRoutableViewModel, IActivatableViewModel
{
    public delegate LecturersViewModel Factory(IScreen hostScreen, IBrowser browser);

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    public LecturersViewModel
    (
        IScreen hostScreen,
        IBrowser browser,
        ILecturerGateway gateway,
        MessageDialogViewModel.Factory messageDialogFactory,
        LecturerCardViewModel.Factory cardFactory
    )
    {
        _messageDialogFactory = messageDialogFactory;
        HostScreen = hostScreen;
        Activator = new ViewModelActivator();
        OpenMessageDialog = new Interaction<MessageDialogViewModel, Unit>();

        var cards = gateway
            .ObserveAll()
            .Catch<IEnumerable<Lecturer>, Exception>(ex =>
                CatchObservableExceptions(ex).ToObservable())
            .Select(lecturers => lecturers.Select(l => cardFactory.Invoke(l, browser)))
            .ToPropertyEx(this, vm => vm.Cards);

        this.WhenActivated(d => cards.DisposeWith(d));
    }

    [ObservableAsProperty] public IEnumerable<LecturerCardViewModel> Cards { get; }

    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }

    public string UrlPathSegment => "/Lecturers";

    public IScreen HostScreen { get; }

    public ViewModelActivator Activator { get; }

    private async Task<IEnumerable<Lecturer>> CatchObservableExceptions(Exception _)
    {
        var messageDialog = _messageDialogFactory.Invoke(
            LocalizedMessage.Letter.Error,
            new LocalizedMessage.Error.StorageIsNotAvailable()
        );

        await OpenMessageDialog.Handle(messageDialog);

        return new Lecturer[] { };
    }
}