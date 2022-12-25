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

public class DisciplinesViewModel : BaseViewModel, IRoutableViewModel, IActivatableViewModel
{
    public delegate DisciplinesViewModel Factory(IScreen hostScreen);

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    public DisciplinesViewModel(
        IScreen hostScreen,
        IDisciplineGateway gateway,
        MessageDialogViewModel.Factory messageDialogFactory,
        DisciplineCardViewModel.Factory cardFactory
    )
    {
        _messageDialogFactory = messageDialogFactory;
        HostScreen = hostScreen;

        Activator = new ViewModelActivator();
        OpenMessageDialog = new Interaction<MessageDialogViewModel, Unit>();

        var cards = gateway
            .ObserveAll()
            .Catch<IEnumerable<Discipline>, Exception>(ex =>
                CatchObservableExceptions(ex).ToObservable())
            .Select(classrooms => classrooms.Select(cardFactory.Invoke))
            .ToPropertyEx(this, vm => vm.Cards);

        this.WhenActivated(d => cards.DisposeWith(d));
    }

    [ObservableAsProperty] public IEnumerable<DisciplineCardViewModel> Cards { get; }

    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }

    public ViewModelActivator Activator { get; }

    public string UrlPathSegment => "/Disciplines";

    public IScreen HostScreen { get; }

    private async Task<IEnumerable<Discipline>> CatchObservableExceptions(Exception _)
    {
        var messageDialog = _messageDialogFactory.Invoke(
            LocalizedMessage.Header.Error,
            new LocalizedMessage.Error.StorageIsNotAvailable()
        );

        await OpenMessageDialog.Handle(messageDialog);

        return new Discipline[] { };
    }
}