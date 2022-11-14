using App.Project.Explorer;

namespace App.Project.ProjectWindow;

public class ProjectWindowViewModel : ViewModelBase
{
    public ProjectWindowViewModel(ExplorerViewModel.Factory explorerViewModelFactory)
    {
        ExplorerViewModel = explorerViewModelFactory.Invoke();
    }

    public ExplorerViewModel ExplorerViewModel { get; }
}