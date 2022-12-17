using App.Project.Browser;
using App.Project.Explorer;

namespace App.Project.ProjectWindow;

public class ProjectWindowViewModel : ViewModelBase, IBrowser
{
    public ProjectWindowViewModel
    (
        ExplorerViewModel.Factory explorerViewModelFactory
    )
    {
        ExplorerViewModel = explorerViewModelFactory.Invoke(this);
        Manager = new BrowserManager();
    }

    public ExplorerViewModel ExplorerViewModel { get; }

    public BrowserManager Manager { get; }
}