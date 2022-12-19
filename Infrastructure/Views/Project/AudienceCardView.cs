using App.Project.AudienceEditor;
using App.Project.ExplorerCard;
using Avalonia.Controls;

namespace App.Project.AudienceCard;

public class AudienceCardView : ExplorerCardView
{
    protected override Window CreateEditorWindow(ViewModelBase viewModel)
    {
        return new AudienceEditorWindow
        {
            DataContext = viewModel
        };
    }
}