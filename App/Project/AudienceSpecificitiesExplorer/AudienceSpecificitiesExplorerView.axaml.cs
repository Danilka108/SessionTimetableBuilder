using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace App.Project.AudienceSpecificitiesExplorer;

public partial class AudienceSpecificitiesExplorerView : ReactiveUserControl<AudienceSpecificitiesExplorerViewModel>
{
    public AudienceSpecificitiesExplorerView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}