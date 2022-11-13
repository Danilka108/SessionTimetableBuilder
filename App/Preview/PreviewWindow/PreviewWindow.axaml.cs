using System.Reactive;
using System.Threading.Tasks;
using App.Project;
using Avalonia;
using Avalonia.Controls.Mixins;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Preview.PreviewWindow;

public partial class PreviewWindow : ReactiveWindow<PreviewWindowViewModel>
{
    public PreviewWindow()
    {
        this.WhenActivated
            (d => { ViewModel!.ShowProjectWindow.RegisterHandler(DoOpenProject).DisposeWith(d); });

        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private async Task DoOpenProject(InteractionContext<ProjectInitializer, Unit> context)
    {
        var projectWindow = await context.Input.Initialize();
        projectWindow.Show();

        Close();

        context.SetOutput(Unit.Default);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}