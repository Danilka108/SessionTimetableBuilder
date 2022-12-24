using System;
using Adapters.Project.ViewModels;
using Infrastructure.Project.Views;
using ReactiveUI;

namespace Infrastructure.Project;

public class ExplorerViewLocator : IViewLocator
{
    public IViewFor ResolveView<T>(T viewModel, string? contract = null) => viewModel switch
    {
        ClassroomsViewModel context => new ClassroomsView
        {
            ViewModel = context
        },
        ClassroomFeaturesViewModel context => new ClassroomFeaturesView
        {
            ViewModel = context
        },
        _ => throw new ArgumentException(nameof(viewModel))
    };
}