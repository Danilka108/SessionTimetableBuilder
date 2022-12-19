using App.Project.AudienceEditor;
using App.Project.ExplorerList;
using Avalonia.Controls;

namespace App.Project.Audiences;

public class AudiencesView : ExplorerListView
{
    protected override Window CreateEditorWindow(ViewModelBase viewModel)
    {
        return new AudienceEditorWindow
        {
            DataContext = viewModel
        };
    }
}