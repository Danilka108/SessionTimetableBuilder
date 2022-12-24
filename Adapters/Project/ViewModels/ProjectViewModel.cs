namespace Adapters.Project.ViewModels;

public class ProjectViewModel : BaseViewModel
{
    public delegate ProjectViewModel Factory(string name);

    public ProjectViewModel(string name, ExplorerViewModel.Factory explorerFactory)
    {
        Name = name;
        ExplorerViewModel = explorerFactory.Invoke();
    }

    public ExplorerViewModel ExplorerViewModel { get; }

    public string Name { get; }
}