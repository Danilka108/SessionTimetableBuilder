using System;
using App.Project.AudienceExplorer;
using App.Project.AudienceSpecificitiesExplorer;
using ReactiveUI;

namespace App.Project.Explorer;

public class ExplorerViewLocator : IViewLocator
{
    public IViewFor ResolveView<T>(T viewModel, string? contract = null)
    {
        return viewModel switch
        {
            AudienceExplorerViewModel context => new AudienceExplorerView
            {
                DataContext = context
            },
            AudienceSpecificitiesExplorerViewModel context => new AudienceSpecificitiesExplorerView
            {
                DataContext = context
            },
            _ => throw new ArgumentNullException(nameof(viewModel))
        };
    }
}