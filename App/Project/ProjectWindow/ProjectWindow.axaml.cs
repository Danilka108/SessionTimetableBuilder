using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace App.Project.ProjectWindow;

public partial class ProjectWindow : ReactiveWindow<ProjectWindow>
{
    public ProjectWindow()
    {
        InitializeComponent();

#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}