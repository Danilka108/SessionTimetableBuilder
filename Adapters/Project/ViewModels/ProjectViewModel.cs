using Adapters.ViewModels;

namespace Adapters.Project.ViewModels;

public class ProjectViewModel : BaseViewModel
{
    public delegate ProjectViewModel Factory(); 
    
    public ProjectViewModel()
    {
    }
}