using System;
using System.Reactive;
using System.Threading.Tasks;
using Adapters;
using Adapters.Project.ViewModels;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace Infrastructure.Project.Views;

public partial class ExplorerView : ReactiveUserControl<ExplorerViewModel>
{
    public ExplorerView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            ViewModel!
                .OpenEditor
                .RegisterHandler(DoOpenEditor)
                .DisposeWith(d);
        });
    }

    private async Task DoOpenEditor(
        InteractionContext<(BaseViewModel, ExploredSet), Unit> interactionContext)
    {
        var (viewModel, exploredSet) = interactionContext.Input;

        var projectWindow = ProjectWindow.GetCurrent();

        var dialogOpeningTask = exploredSet switch
        {
            ExploredSet.Classrooms when viewModel is ClassroomEditorViewModel v => new
                ClassroomEditorWindow
                {
                    ViewModel = v
                }.ShowDialog(projectWindow),
            ExploredSet.ClassroomFeatures when viewModel is ClassroomFeatureEditorViewModel v => new
                ClassroomFeatureEditorWindow
                {
                    ViewModel = v
                }.ShowDialog(projectWindow),
            ExploredSet.Disciplines when viewModel is DisciplineEditorViewModel v => new
                DisciplineEditorWindow
                {
                    ViewModel = v
                }.ShowDialog(projectWindow),
            _ => throw new ArgumentException(nameof(interactionContext))
        };

        await dialogOpeningTask;

        interactionContext.SetOutput(Unit.Default);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}