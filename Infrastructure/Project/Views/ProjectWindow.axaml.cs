using System.Linq;
using Adapters.Project.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Infrastructure.Project.Views;

public partial class ProjectWindow : ReactiveWindow<ProjectViewModel>
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
            App.Current?.ApplicationLifetime
                is IClassicDesktopStyleApplicationLifetime desktop
            && desktop.Windows.FirstOrDefault(w => w is ProjectWindow)
                is { } window
        ) return window;

        return null;
    }
}