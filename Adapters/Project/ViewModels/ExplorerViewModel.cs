using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Adapters.ViewModels;
using ReactiveUI;

namespace Adapters.Project.ViewModels;

public enum ExploredSet : int
{
    ClassroomFeatures = 0,
    Classrooms
}

public class ExplorerViewModel : BaseViewModel, IActivatableViewModel, IScreen
{
    public delegate ExplorerViewModel Factory();

    private ExploredSet _exploredSet;

    private readonly ClassroomsViewModel.Factory _classroomsFactory;

    private readonly ClassroomFeaturesViewModel.Factory _classroomFeaturesFactory;

    public ExplorerViewModel(
        ClassroomsViewModel.Factory classroomsFactory,
        ClassroomFeaturesViewModel.Factory classroomFeaturesFactory
    )
    {
        Activator = new ViewModelActivator();
        Router = new RoutingState();
        
        ExploredSet = ExploredSet.ClassroomFeatures;

        _classroomsFactory = classroomsFactory;
        _classroomFeaturesFactory = classroomFeaturesFactory;

        this.WhenActivated(d =>
        {
            this
                .WhenAnyValue(vm => vm.ExploredSet)
                .SelectMany(NavigateToExploredSet)
                .Subscribe()
                .DisposeWith(d);
        });
    }

    private IObservable<IRoutableViewModel> NavigateToExploredSet(ExploredSet exploredSet, int _)
    {
        IRoutableViewModel viewModelToNavigate = exploredSet switch
        {
            ExploredSet.Classrooms => _classroomsFactory.Invoke(this),
            ExploredSet.ClassroomFeatures => _classroomFeaturesFactory.Invoke(this),
        };


        return Router.Navigate.Execute(viewModelToNavigate);
    }

    public ExploredSet ExploredSet
    {
        get => _exploredSet;
        set => this.RaiseAndSetIfChanged(ref _exploredSet, value);
    }

    public ViewModelActivator Activator { get; }

    public RoutingState Router { get; }
}