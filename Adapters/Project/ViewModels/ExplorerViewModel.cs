using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Adapters.Project.ViewModels;

public enum ExploredSet
{
    ClassroomFeatures = 0,
    Classrooms,
    Disciplines
}

public class ExplorerViewModel : BaseViewModel, IActivatableViewModel, IScreen
{
    public delegate ExplorerViewModel Factory();

    private readonly ClassroomFeaturesViewModel.Factory _classroomFeaturesFactory;

    private readonly ClassroomsViewModel.Factory _classroomsFactory;

    private readonly DisciplinesViewModel.Factory _disciplinesFactory;

    public ExplorerViewModel(
        ClassroomsViewModel.Factory classroomsFactory,
        ClassroomFeaturesViewModel.Factory classroomFeaturesFactory,
        DisciplinesViewModel.Factory disciplinesFactory
    )
    {
        Activator = new ViewModelActivator();
        Router = new RoutingState();

        ExploredSet = ExploredSet.ClassroomFeatures;

        _classroomsFactory = classroomsFactory;
        _classroomFeaturesFactory = classroomFeaturesFactory;
        _disciplinesFactory = disciplinesFactory;

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

    public ViewModelActivator Activator { get; }

    public RoutingState Router { get; }

    private IObservable<IRoutableViewModel> NavigateToExploredSet(ExploredSet exploredSet, int _)
    {
        IRoutableViewModel viewModelToNavigate = exploredSet switch
        {
            ExploredSet.Classrooms => _classroomsFactory.Invoke(this),
            ExploredSet.ClassroomFeatures => _classroomFeaturesFactory.Invoke(this),
            ExploredSet.Disciplines => _disciplinesFactory.Invoke(this)
        };

        return Router.Navigate.Execute(viewModelToNavigate);
    }
}