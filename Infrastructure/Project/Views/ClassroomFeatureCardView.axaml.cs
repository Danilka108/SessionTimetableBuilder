using System.Reactive;
using System.Threading.Tasks;
using Adapters.Common.ViewModels;
using Adapters.Project.ViewModels;
using Avalonia.Controls.Mixins;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Infrastructure.Common.Views;
using ReactiveUI;

namespace Infrastructure.Project.Views;

public partial class ClassroomFeatureCardView : ReactiveUserControl<ClassroomFeatureCardViewModel>
{
    public ClassroomFeatureCardView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            ViewModel!
                .OpenEditor
                .RegisterHandler(DoOpenEditor)
                .DisposeWith(d);

            ViewModel!
                .OpenConfirmDialog
                .RegisterHandler(DoOpenConfirmDialog)
                .DisposeWith(d);

            ViewModel!
                .OpenMessageDialog
                .RegisterHandler(DoOpenMessageDialog)
                .DisposeWith(d);
        });
    }

    private async Task DoOpenEditor(
        InteractionContext<ClassroomFeatureEditorViewModel, Unit> context)
    {
        var editor = new ClassroomFeatureEditorWindow
        {
            ViewModel = context.Input
        };

        var projectWindow = ProjectWindow.GetCurrent();
        await editor.ShowDialog(projectWindow);

        context.SetOutput(Unit.Default);
    }

    private async Task DoOpenConfirmDialog(InteractionContext<ConfirmDialogViewModel, bool> context)
    {
        var viewModel = context.Input;
        var dialog = new ConfirmDialogWindow
        {
            DataContext = viewModel
        };

        var confirmResult = await dialog.ShowDialog<bool>(ProjectWindow.GetCurrent());
        context.SetOutput(confirmResult);
    }

    private async Task DoOpenMessageDialog(InteractionContext<MessageDialogViewModel, Unit> context)
    {
        var viewModel = context.Input;

        var dialog = new MessageDialogWindow
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