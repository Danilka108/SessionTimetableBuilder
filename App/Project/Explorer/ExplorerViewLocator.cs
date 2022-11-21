using System;
using App.Project.Audiences;
using App.Project.AudienceSpecificities;
using App.Project.BellTimes;
using App.Project.Disciplines;
using ReactiveUI;

namespace App.Project.Explorer;

public class ExplorerViewLocator : IViewLocator
{
    public IViewFor ResolveView<T>(T viewModel, string? contract = null)
    {
        return viewModel switch
        {
            AudiencesViewModel context => new AudiencesView
            {
                DataContext = context
            },
            AudienceSpecificitiesViewModel context => new AudienceSpecificitiesView
            {
                DataContext = context
            },
            BellTimesViewModel context => new BellTimesView
            {
                DataContext = context
            },
            DisciplinesViewModel context => new DisciplinesView
            {
                DataContext = context
            },
            _ => throw new ArgumentNullException(nameof(viewModel))
        };
    }
}