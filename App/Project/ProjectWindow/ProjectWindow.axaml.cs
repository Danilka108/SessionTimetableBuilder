using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
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

    public static Window? GetCurrent()
    {
        if (
            Application.Current?.ApplicationLifetime
                is IClassicDesktopStyleApplicationLifetime desktop
            && desktop.Windows.FirstOrDefault(w => w is ProjectWindow)
                is { } window
        )
        {
            return window;
        }

        return null;
    }
}