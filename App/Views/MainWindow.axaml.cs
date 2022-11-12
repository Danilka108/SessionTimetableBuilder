using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using App.ViewModels;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ProjectPresentation;
using ReactiveUI;

namespace App.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        this.WhenActivated(d => { ViewModel!.ShowProjectWindow.RegisterHandler(DoOpenProject).DisposeWith(d); });

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