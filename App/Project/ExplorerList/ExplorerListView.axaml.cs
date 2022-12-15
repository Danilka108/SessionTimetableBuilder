using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Project.ExplorerList;

public partial class ExplorerListView : ReactiveUserControl<ExplorerListViewModel>
{
    public ExplorerListView()
    {
        this.WhenActivated
        (
            d =>
            {
                ViewModel!
                    .OpenEditor
                    .RegisterHandler(DoOpenEditorDialog)
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

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}