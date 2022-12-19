using App.Project.DisciplineEditor;
using App.Project.ExplorerList;
using Avalonia.Controls;

namespace App.Project.Disciplines;

public class DisciplinesView : ExplorerListView
{
    protected override Window CreateEditorWindow(ViewModelBase viewModel)
    {
        return new DisciplineEditorWindow
        {
            DataContext = viewModel
        };
    }
}