using System;
using System.Reactive;
using System.Threading.Tasks;
using Adapters.Common.ViewModels;
using Adapters.Project.ViewModels;
using Avalonia;
using Avalonia.Controls.Mixins;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Infrastructure.Common.Views;
using ReactiveUI;

namespace Infrastructure.Project.Views;

public partial class ClassroomFeatureEditorWindow : ReactiveWindow<ClassroomFeatureEditorViewModel>
{
    public ClassroomFeatureEditorWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        this.WhenActivated(d =>
        {
            ViewModel!
                .CloseSelf
                .RegisterHandler(DoCloseSelf)
                .DisposeWith(d);

            ViewModel!
                .OpenMessageDialog
                .RegisterHandler(DoOpenMessageDialog)
                .DisposeWith(d);
        });
    }

    private async Task DoCloseSelf(InteractionContext<Unit, Unit> context)
    {
        context.SetOutput(Unit.Default);
        Close();
    }

    private async Task DoOpenMessageDialog(InteractionContext<MessageViewModel, Unit> context)
    {
        var viewModel = context.Input;

        var dialog = new MessageWindow
        {
            DataContext = viewModel
        };

        await dialog.ShowDialog(ProjectWindow.GetCurrent());

        context.SetOutput(Unit.Default);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}