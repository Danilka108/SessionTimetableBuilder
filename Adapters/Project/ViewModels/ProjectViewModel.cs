using Adapters.Project.Browser;

namespace Adapters.Project.ViewModels;

public class ProjectViewModel : BaseViewModel, IBrowser
{
    public delegate ProjectViewModel Factory(string name);

    public ProjectViewModel(string name, ExplorerViewModel.Factory explorerFactory)
    {
        Name = name;
        ExplorerViewModel = explorerFactory.Invoke(this);
        Manager = new BrowserManager();
    }

    public ExplorerViewModel ExplorerViewModel { get; }

    public string Name { get; }
    
    public BrowserManager Manager { get; }
}