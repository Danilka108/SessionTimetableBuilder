using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Adapters.Common.ViewModels;
using Application.Project.Gateways;
using Domain.Project;
using ReactiveUI;

namespace Adapters.Project.ViewModels;

public class DisciplinesViewModel : BaseViewModel, IRoutableViewModel, IActivatableViewModel
{
    public delegate DisciplinesViewModel Factory(IScreen hostScreen);

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    private readonly ObservableAsPropertyHelper<IEnumerable<DisciplineCardViewModel>> _cards;

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
        
        _cards = gateway
            .ObserveAll()
            .Catch<IEnumerable<Discipline>, Exception>(ex =>
                CatchFeaturesObserving(ex).ToObservable())
            .Select(classrooms => classrooms.Select(cardFactory.Invoke))
            .ToProperty(this, vm => vm.Cards);
        
        this.WhenActivated(d => _cards.DisposeWith(d));
    }

    private async Task<IEnumerable<Discipline>> CatchFeaturesObserving(Exception _)
    {
        var messageDialog = _messageDialogFactory.Invoke(
            LocalizedMessage.Header.Error,
            new LocalizedMessage.Error.StorageIsNotAvailable()
        );

        await OpenMessageDialog.Handle(messageDialog);

        return new Discipline[] { };
    }

    public IEnumerable<DisciplineCardViewModel> Cards => _cards.Value;

    public string UrlPathSegment => "/Disciplines";
    
    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }

    public IScreen HostScreen { get; }

    public ViewModelActivator Activator { get; }
}