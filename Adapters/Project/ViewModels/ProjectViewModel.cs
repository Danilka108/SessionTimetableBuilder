using Adapters.ViewModels;

namespace Adapters.Project.ViewModels;

public class ProjectViewModel : BaseViewModel
{
    public delegate ProjectViewModel Factory(string name); 
    
    public ExplorerViewModel ExplorerViewModel { get; }
    
    public ProjectViewModel(string name, ExplorerViewModel.Factory explorerFactory)
    {
        Name = name;
        ExplorerViewModel = explorerFactory.Invoke();
    }
    
    public string Name { get; }
}