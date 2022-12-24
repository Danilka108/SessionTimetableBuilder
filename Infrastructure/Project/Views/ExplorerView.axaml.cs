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
            ExploredSet.Classrooms => new ClassroomEditorWindow
            {
                DataContext = viewModel
            }.ShowDialog(projectWindow),
            ExploredSet.ClassroomFeatures => new ClassroomFeatureEditorWindow
            {
                DataContext = viewModel
            }.ShowDialog(projectWindow),
            ExploredSet.Disciplines => new DisciplineEditorWindow
            {
                DataContext = viewModel
            }.ShowDialog(projectWindow)
        };

        await dialogOpeningTask;

        interactionContext.SetOutput(Unit.Default);
    }


    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}