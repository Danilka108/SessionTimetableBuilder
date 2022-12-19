using App.Project.AudienceSpecificityEditor;
using App.Project.ExplorerCard;
using Avalonia.Controls;

namespace App.Project.AudienceSpecificityCard;

public class AudienceSpecificityCardView : ExplorerCardView
{
    protected override Window CreateEditorWindow(ViewModelBase viewModel)
    {
        return new AudienceSpecificityEditorWindow
        {
            DataContext = viewModel
        };
    }
}