using System;
using ReactiveUI;

namespace App;

public class RoutedViewLocator : IViewLocator
{
    public IViewFor ResolveView<T>(T viewModel, string? contract = null)
    {
        return viewModel switch
        {
            _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
        };
    }
}