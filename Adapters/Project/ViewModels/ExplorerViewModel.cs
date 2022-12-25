using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Adapters.Project.Browser;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Adapters.Project.ViewModels;

public enum ExploredSet
{
    ClassroomFeatures = 0,
    Classrooms,
    Disciplines,
    Lecturers
}

public class ExplorerViewModel : BaseViewModel, IActivatableViewModel, IScreen
{
    public delegate ExplorerViewModel Factory(IBrowser browser);

    private readonly ClassroomFeaturesViewModel.Factory _classroomFeaturesFactory;

    private readonly ClassroomsViewModel.Factory _classroomsFactory;

    private readonly DisciplinesViewModel.Factory _disciplinesFactory;

    private readonly ClassroomEditorViewModel.Factory _classroomEditorFactory;

    private readonly ClassroomFeatureEditorViewModel.Factory _classroomFeatureEditorFactory;

    private readonly DisciplineEditorViewModel.Factory _disciplineEditorFactory;

    private readonly LecturersViewModel.Factory _lecturersFactory;

    private readonly LecturerEditorViewModel.Factory _lecturerEditorFactory;

    private readonly IBrowser _browser;

    public ExplorerViewModel(
        IBrowser browser,
        ClassroomsViewModel.Factory classroomsFactory,
        ClassroomFeaturesViewModel.Factory classroomFeaturesFactory,
        DisciplinesViewModel.Factory disciplinesFactory,
        ClassroomEditorViewModel.Factory classroomEditorFactory,
        ClassroomFeatureEditorViewModel.Factory classroomFeatureEditorFactory,
        DisciplineEditorViewModel.Factory disciplineEditorFactory,
        LecturersViewModel.Factory lecturersFactory,
        LecturerEditorViewModel.Factory lecturerEditorFactory
    )
    {
        Activator = new ViewModelActivator();
        Router = new RoutingState();
        OpenEditor = new Interaction<(BaseViewModel, ExploredSet), Unit>();

        ExploredSet = ExploredSet.ClassroomFeatures;

        _browser = browser;
        _classroomEditorFactory = classroomEditorFactory;
        _classroomFeatureEditorFactory = classroomFeatureEditorFactory;
        _disciplineEditorFactory = disciplineEditorFactory;
        _classroomsFactory = classroomsFactory;
        _classroomFeaturesFactory = classroomFeaturesFactory;
        _disciplinesFactory = disciplinesFactory;
        _lecturersFactory = lecturersFactory;
        _lecturerEditorFactory = lecturerEditorFactory;

        Create = ReactiveCommand.CreateFromTask(async () =>
        {
            if (ExploredSet == ExploredSet.Lecturers)
            {
                await browser.Manager.Browse.Execute(_lecturerEditorFactory.Invoke(null));
                return;
            }

            await OpenEditor.Handle(ProduceEditor(ExploredSet));
        });

        this.WhenActivated(d =>
        {
            this
                .WhenAnyValue(vm => vm.ExploredSet)
                .SelectMany(NavigateToExploredSet)
                .Subscribe()
                .DisposeWith(d);
        });
    }

    [Reactive] public ExploredSet ExploredSet { get; set; }

    public ReactiveCommand<Unit, Unit> Create { get; }

    public Interaction<(BaseViewModel, ExploredSet), Unit> OpenEditor { get; }

    public ViewModelActivator Activator { get; }

    public RoutingState Router { get; }

    private IObservable<IRoutableViewModel> NavigateToExploredSet(ExploredSet exploredSet)
    {
        IRoutableViewModel viewModelToNavigate = exploredSet switch
        {
            ExploredSet.Classrooms => _classroomsFactory.Invoke(this),
            ExploredSet.ClassroomFeatures => _classroomFeaturesFactory.Invoke(this),
            ExploredSet.Disciplines => _disciplinesFactory.Invoke(this),
            ExploredSet.Lecturers => _lecturersFactory.Invoke(this, _browser)
        };

        return Router.Navigate.Execute(viewModelToNavigate);
    }

    private (BaseViewModel, ExploredSet) ProduceEditor(ExploredSet exploredSet)
    {
        BaseViewModel editorToOpen = exploredSet switch
        {
            ExploredSet.Classrooms => _classroomEditorFactory.Invoke(null),
            ExploredSet.ClassroomFeatures => _classroomFeatureEditorFactory.Invoke(null),
            ExploredSet.Disciplines => _disciplineEditorFactory.Invoke(null),
            _ => throw new ArgumentException(nameof(exploredSet)),
        };

        return (editorToOpen, exploredSet);
    }
}