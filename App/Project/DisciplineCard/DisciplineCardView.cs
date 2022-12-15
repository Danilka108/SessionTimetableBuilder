using App.Project.DisciplineEditor;
using App.Project.ExplorerCard;
using Avalonia.Controls;

namespace App.Project.DisciplineCard;

public class DisciplineCardView : ExplorerCardView
{
    protected override Window CreateEditorWindow(ViewModelBase viewModel)
    {
        return new DisciplineEditorWindow
        {
            DataContext = viewModel
        };
    }
}