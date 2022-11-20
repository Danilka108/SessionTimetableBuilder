using App.Project.AudienceSpecificityEditor;
using App.Project.ExplorerCard;
using App.Project.ExplorerList;
using Avalonia.Controls;

namespace App.Project.AudienceSpecificities;

public class AudienceSpecificitiesView : ExplorerListView 
{
    protected override Window CreateEditorWindow(ViewModelBase viewModel)
    {
        return new AudienceSpecificityEditorWindow
        {
            DataContext = viewModel
        };
    }
}