using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using App.CommonControls.ConfirmWindow;
using App.CommonControls.MessageWindow;
using App.Project.AudienceSpecificityEditor;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Project.ExplorerCard;

public partial class ExplorerCardView : ReactiveUserControl<ExplorerCardViewModel>
{
    public ExplorerCardView()
    {
        this.WhenActivated
        (
            d =>
            {
                ViewModel!
                    .OpenEditorDialog
                    .RegisterHandler(DoOpenEditorDialog)
                    .DisposeWith(d);

                ViewModel!
                    .OpenConfirmDialog
                    .RegisterHandler(DoOpenConfirmDialog)
                    .DisposeWith(d);

                ViewModel!
                    .OpenMessageDialog
                    .RegisterHandler(DoOpenMessageDialog)
                    .DisposeWith(d);
            }
        );

        InitializeComponent();
    }

    protected virtual Window CreateEditorWindow(ViewModelBase viewModel)
    {
        throw new NotImplementedException();
    }

    private async Task DoOpenEditorDialog
        (InteractionContext<ViewModelBase, Unit> context)
    {
        var editorWindow = CreateEditorWindow(context.Input);

        if (ProjectWindow.ProjectWindow.GetCurrent() is { } window)
            await editorWindow.ShowDialog(window);

        context.SetOutput(Unit.Default);
    }

    private async Task DoOpenConfirmDialog(InteractionContext<ConfirmWindowViewModel, bool> context)
    {
        var confirmWindow = new ConfirmWindow
        {
            DataContext = context.Input
        };

        if (ProjectWindow.ProjectWindow.GetCurrent() is { } window)
        {
            var result = await confirmWindow.ShowDialog<bool?>(window);
            context.SetOutput(result ?? false);
        }
        else
        {
            context.SetOutput(false);
        }
    }

    private async Task DoOpenMessageDialog
        (InteractionContext<MessageWindowViewModel, Unit> context)
    {
        var messageWindow = new MessageWindow
        {
            DataContext = context.Input
        };

        if (ProjectWindow.ProjectWindow.GetCurrent() is { } window)
            await messageWindow.ShowDialog(window);

        context.SetOutput(Unit.Default);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}