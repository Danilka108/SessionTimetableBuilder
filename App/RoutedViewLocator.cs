using System;
using App.Views.CreateTeacher;
using ReactiveUI;

namespace App;

public class RoutedViewLocator : IViewLocator
{
    public IViewFor ResolveView<T>(T viewModel, string? contract = null)
    {
        return viewModel switch
        {
            CreateTeacherViewModel context => new CreateTeacherView { DataContext = context },
            _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
        };
    }
}