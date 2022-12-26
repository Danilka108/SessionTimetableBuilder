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

public class GroupsViewModel : BaseViewModel, IRoutableViewModel, IActivatableViewModel
{
    public delegate GroupsViewModel Factory(IScreen hostScreen, IBrowser browser);

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    public GroupsViewModel(
        IScreen hostScreen, IBrowser browser,
        IGroupGateway gateway,
        GroupCardViewModel.Factory cardFactory,
        MessageDialogViewModel.Factory messageDialogFactory
    )
    {
        HostScreen = hostScreen;
        Activator = new ViewModelActivator();

        _messageDialogFactory = messageDialogFactory;
        OpenMessageDialog = new Interaction<MessageDialogViewModel, Unit>();

        var cards = gateway
            .ObserveAll()
            .Catch<IEnumerable<Group>, Exception>(ex =>
                CatchObservableExceptions(ex).ToObservable())
            .Select(groups =>
                groups.Select(group => cardFactory.Invoke(group, browser)))
            .ToPropertyEx(this, vm => vm.Cards);
        
        this.WhenActivated(d => cards.DisposeWith(d));
    }

    [ObservableAsProperty] public IEnumerable<GroupCardViewModel> Cards { get; }
    
    public ReactiveCommand<Unit, Unit> Save { get; }

    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }

    public string UrlPathSegment => "/Groups";

    public IScreen HostScreen { get; }

    public ViewModelActivator Activator { get; }
    
    private async Task<IEnumerable<Group>> CatchObservableExceptions(Exception _)
    {
        var messageDialog = _messageDialogFactory.Invoke(
            LocalizedMessage.Letter.Error,
            new LocalizedMessage.Error.StorageIsNotAvailable()
        );

        await OpenMessageDialog.Handle(messageDialog);

        return new Group[] { };
    }
}