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

public class ClassroomFeaturesViewModel : BaseViewModel, IRoutableViewModel, IActivatableViewModel
{
    public delegate ClassroomFeaturesViewModel Factory(IScreen hostScreen);

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

        var cards = gateway
            .ObserveAll()
            .Catch<IEnumerable<ClassroomFeature>, Exception>(ex =>
                CatchFeaturesObserving(ex).ToObservable())
            .Select(features => features.Select(cardFactory.Invoke))
            .ToPropertyEx(this, vm => vm.Cards);

        this.WhenActivated(d => { cards.DisposeWith(d); });
    }

    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }

    [ObservableAsProperty] public IEnumerable<ClassroomFeatureCardViewModel> Cards { get; }

    public ViewModelActivator Activator { get; }

    public string UrlPathSegment => "/ClassroomFeatures";
    public IScreen HostScreen { get; }

    private async Task<IEnumerable<ClassroomFeature>> CatchFeaturesObserving(Exception _)
    {
        var messageDialog = _messageDialogFactory.Invoke(
            LocalizedMessage.Header.Error,
            new LocalizedMessage.Error.StorageIsNotAvailable()
        );

        await OpenMessageDialog.Handle(messageDialog);

        return new ClassroomFeature[] { };
    }
}