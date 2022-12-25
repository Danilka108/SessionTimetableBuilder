using System;
using Adapters.Project.ViewModels;
using Infrastructure.Project.Views;
using ReactiveUI;

namespace Infrastructure.Project;

public class BrowserViewLocator : IViewLocator
{
    public IViewFor? ResolveView<T>(T? viewModel, string? contract = null)
    {
        return viewModel switch
        {
            LecturerEditorViewModel context => new LecturerEditorView
            {
                ViewModel = context
            },
            _ => throw new ArgumentNullException(nameof(viewModel))
        };
    }
}