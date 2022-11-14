using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace App.Project.AudienceExplorer;

public partial class AudienceExplorerView : ReactiveUserControl<AudienceExplorerViewModel>
{
    public AudienceExplorerView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}