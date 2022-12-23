using System.Reactive;
using System.Threading.Tasks;
using Adapters.Preview.ViewModels;
using Autofac;
using Avalonia;
using Avalonia.Controls.Mixins;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Infrastructure.Project;
using ReactiveUI;

namespace Infrastructure.Preview.Views;

public partial class PreviewWindow : ReactiveWindow<PreviewViewModel>
{
    public ILifetimeScope ParentDiScope { get; init; }
    
    public PreviewWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    
        this.WhenActivated(d =>
        {
            ViewModel!
                .ShowProject
                .RegisterHandler(DoShowProject)
                .DisposeWith(d);
        });
    }
    
    private async Task DoShowProject(InteractionContext<Unit, Unit> context)
    {
        var initializer = new ProjectInitializer("test", ParentDiScope);
        var projectWindow = await initializer.Initialize();
        
        projectWindow.Show();
        Close();
        
        context.SetOutput(Unit.Default);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}