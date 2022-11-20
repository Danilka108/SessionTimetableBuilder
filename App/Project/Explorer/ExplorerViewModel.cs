using System;
using System.Reactive;
using System.Reactive.Linq;
using App.Project.Audiences;
using App.Project.AudienceSpecificities;
using App.Project.BellTimes;
using ReactiveUI;

namespace App.Project.Explorer;

internal enum ExploredSetItem : byte
{
    Audience = 0,
    AudienceSpecificities,
    BellTimes
}

public class ExplorerViewModel : ViewModelBase, IScreen
{
    public delegate ExplorerViewModel Factory();

    private byte _exploredSet = (byte)ExploredSetItem.BellTimes;

    public ExplorerViewModel
    (
        AudiencesViewModel.Factory audiencesViewModelFactory,
        AudienceSpecificitiesViewModel.Factory specificitiesViewModelFactory,
        BellTimesViewModel.Factory bellTimesViewModelFactory
    )
    {
        Router = new RoutingState();

        NavigateToExploredItem = this
            .WhenAnyValue(vm => vm.ExploredSet)
            .SelectMany
            (
                exploredSet =>
                {
                    IRoutableViewModel navigatedViewModel = (ExploredSetItem)exploredSet switch
                    {
                        ExploredSetItem.Audience => audiencesViewModelFactory.Invoke(this),
                        ExploredSetItem.AudienceSpecificities => specificitiesViewModelFactory
                            .Invoke(this),
                        ExploredSetItem.BellTimes => bellTimesViewModelFactory.Invoke(this),
                        _ => throw new ArgumentNullException(nameof(exploredSet))
                    };

                    return Router.Navigate.Execute(navigatedViewModel);
                }
            );
    }

    public IObservable<IRoutableViewModel> NavigateToExploredItem { get; }

    public byte ExploredSet
    {
        get => _exploredSet;
        set => this.RaiseAndSetIfChanged(ref _exploredSet, value);
    }

    public RoutingState Router { get; }
}