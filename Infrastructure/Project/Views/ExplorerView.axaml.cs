using Adapters.Project.ViewModels;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Infrastructure.Project.Views;

public partial class ExplorerView : ReactiveUserControl<ExplorerViewModel>
{
    public ExplorerView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}