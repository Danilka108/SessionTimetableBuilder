using System;
using System.Reactive;
using System.Reactive.Linq;
using App.Project.Audiences;
using App.Project.AudienceSpecificities;
using App.Project.BellTimes;
using App.Project.Browser;
using App.Project.Disciplines;
using App.Project.Teachers;
using ReactiveUI;

namespace App.Project.Explorer;

internal enum ExploredSetItem : byte
{
    Audiences = 0,
    AudienceSpecificities,
    BellTimes,
    Disciplines,
    Teachers,
}

public class ExplorerViewModel : ViewModelBase, IScreen
{
    public delegate ExplorerViewModel Factory(IBrowser browser);

    private byte _exploredSet = (byte)ExploredSetItem.Disciplines;

    public ExplorerViewModel
    (
        IBrowser browser,
        AudiencesViewModel.Factory audiencesViewModelFactory,
        AudienceSpecificitiesViewModel.Factory specificitiesViewModelFactory,
        BellTimesViewModel.Factory bellTimesViewModelFactory,
        DisciplinesViewModel.Factory disciplinesViewModelFactory,
        TeachersViewModel.Factory teachersViewModelFactory
    )
    {
        Create = ReactiveCommand.Create(() => {});
        Router = new RoutingState();

        NavigateToExploredItem = this
            .WhenAnyValue(vm => vm.ExploredSet)
            .SelectMany
            (
                exploredSet =>
                {
                    IRoutableViewModel navigatedViewModel = (ExploredSetItem)exploredSet switch
                    {
                        ExploredSetItem.Audiences => audiencesViewModelFactory.Invoke(this, Create),
                        ExploredSetItem.AudienceSpecificities => specificitiesViewModelFactory
                            .Invoke(this, Create),
                        ExploredSetItem.BellTimes => bellTimesViewModelFactory.Invoke(this, Create),
                        ExploredSetItem.Disciplines => disciplinesViewModelFactory.Invoke(this, Create),
                        ExploredSetItem.Teachers => teachersViewModelFactory.Invoke(this, browser, Create),
                        _ => throw new ArgumentNullException(nameof(exploredSet))
                    };

                    return Router.Navigate.Execute(navigatedViewModel);
                }
            );
    }
    
    public ReactiveCommand<Unit, Unit> Create { get; }

    public IObservable<IRoutableViewModel> NavigateToExploredItem { get; }

    public byte ExploredSet
    {
        get => _exploredSet;
        set => this.RaiseAndSetIfChanged(ref _exploredSet, value);
    }

    public RoutingState Router { get; }
}