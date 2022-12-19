using System;
using App.Project.Browser;
using App.Project.TeacherEditor;
using ReactiveUI;

namespace App.Project.ProjectWindow;

public class BrowserViewLocator : IViewLocator
{
    public IViewFor ResolveView<T>(T viewModel, string? contract = null)
    {
        return viewModel switch
        {
            TeacherEditorViewModel context => new TeacherEditorView
            {
                ViewModel = context
            },
            _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
        };
    }
}