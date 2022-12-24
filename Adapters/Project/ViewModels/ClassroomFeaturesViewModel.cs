using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Adapters.Common.ViewModels;
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

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    public ClassroomFeaturesViewModel(
        IScreen hostScreen,
        IClassroomFeatureGateway gateway,
        ClassroomFeatureCardViewModel.Factory cardFactory,
        MessageDialogViewModel.Factory messageDialogFactory
    )
    {
        _messageDialogFactory = messageDialogFactory;
        Activator = new ViewModelActivator();
        OpenMessageDialog = new Interaction<MessageDialogViewModel, Unit>();

        HostScreen = hostScreen;

        _cards = gateway
            .ObserveAll()
            .Catch<IEnumerable<ClassroomFeature>, Exception>(ex =>
                CatchFeaturesObserving(ex).ToObservable())
            .Select(features => features.Select(cardFactory.Invoke))
            .ToProperty(this, vm => vm.Cards);

        this.WhenActivated(d => { _cards.DisposeWith(d); });
    }
    
    private async Task<IEnumerable<ClassroomFeature>> CatchFeaturesObserving(Exception _)
    {
        var messageDialog = _messageDialogFactory.Invoke(
            LocalizedMessage.Header.Error,
            new LocalizedMessage.Error.StorageIsNotAvailable()
        );

        await OpenMessageDialog.Handle(messageDialog);

        return new ClassroomFeature[] { };
    }
    
    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }

    public IEnumerable<ClassroomFeatureCardViewModel> Cards => _cards.Value;

    public string UrlPathSegment => "/ClassroomFeatures";
    public IScreen HostScreen { get; }

    public ViewModelActivator Activator { get; }
}