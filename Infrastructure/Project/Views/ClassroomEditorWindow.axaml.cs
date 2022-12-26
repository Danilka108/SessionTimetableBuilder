using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Adapters.Project.ViewModels;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace Infrastructure.Project.Views;

public partial class ClassroomEditorWindow : ReactiveWindow<ClassroomEditorViewModel>
{
    public ClassroomEditorWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        this.WhenActivated(d =>
        {
            ViewModel!
                .Finish
                .RegisterHandler(DoFinish)
                .DisposeWith(d);
        });
    }

    private async Task DoFinish(InteractionContext<Unit, Unit> context)
    {
        context.SetOutput(Unit.Default);
        Close();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}