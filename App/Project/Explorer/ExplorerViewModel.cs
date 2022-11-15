using System;
using System.Reactive.Linq;
using App.Project.AudienceExplorer;
using App.Project.AudienceSpecificities;
using ReactiveUI;

namespace App.Project.Explorer;

internal enum ExploredSetItem : byte
{
    Audience = 0,
    AudienceSpecificities
}

public class ExplorerViewModel : ViewModelBase, IScreen
{
    public delegate ExplorerViewModel Factory();

    private byte _exploredSet = (byte)ExploredSetItem.Audience;

    public ExplorerViewModel
    (
        AudienceExplorerViewModel.Factory audienceExplorerViewModelFactory,
        AudienceSpecificitiesViewModel.Factory specificitiesViewModelFactory
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
                        ExploredSetItem.Audience => audienceExplorerViewModelFactory.Invoke(this),
                        ExploredSetItem.AudienceSpecificities => specificitiesViewModelFactory
                            .Invoke(this),
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