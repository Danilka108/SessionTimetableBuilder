using Adapters.ViewModels;

namespace Adapters.Project.ViewModels;

public class ProjectViewModel : BaseViewModel
{
    public delegate ProjectViewModel Factory(); 
    
    public ExplorerViewModel ExplorerViewModel { get; }
    
    public ProjectViewModel(ExplorerViewModel.Factory explorerFactory)
    {
        ExplorerViewModel = explorerFactory.Invoke();
    }
}