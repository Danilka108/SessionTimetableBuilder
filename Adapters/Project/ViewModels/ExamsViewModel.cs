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

public class ExamsViewModel : BaseViewModel, IActivatableViewModel, IRoutableViewModel
{
    public delegate ExamsViewModel Factory(IScreen hostScreen, IBrowser browser);

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    public ExamsViewModel(
        IScreen hostScreen,
        IBrowser browser,
        IExamGateway gateway,
        MessageDialogViewModel.Factory messageDialogFactory,
        ExamCardViewModel.Factory cardFactory
    )
    {
        _messageDialogFactory = messageDialogFactory;

        OpenMessageDialog = new Interaction<MessageDialogViewModel, Unit>();
        Activator = new ViewModelActivator();
        HostScreen = hostScreen;

        var cards = gateway
            .ObserveAll()
            .Catch<IEnumerable<Exam>, Exception>(ex =>
                CatchObservableExceptions(ex).ToObservable())
            .Select(exams => exams.Select(exam => cardFactory.Invoke(exam, browser)))
            .ToPropertyEx(this, vm => vm.Cards);

        this.WhenActivated(d => cards.DisposeWith(d));
    }

    [ObservableAsProperty] public IEnumerable<ExamCardViewModel> Cards { get; }

    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }

    public ViewModelActivator Activator { get; }

    public string? UrlPathSegment => "/Exams";

    public IScreen HostScreen { get; }

    private async Task<IEnumerable<Exam>> CatchObservableExceptions(Exception _)
    {
        var messageDialog = _messageDialogFactory.Invoke(
            LocalizedMessage.Letter.Error,
            new LocalizedMessage.Error.StorageIsNotAvailable()
        );

        await OpenMessageDialog.Handle(messageDialog);

        return new Exam[] { };
    }
}