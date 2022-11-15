using System;
using App.Project.AudienceExplorer;
using App.Project.AudienceSpecificities;
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
            AudienceSpecificitiesViewModel context => new AudienceSpecificitiesView
            {
                DataContext = context
            },
            _ => throw new ArgumentNullException(nameof(viewModel))
        };
    }
}