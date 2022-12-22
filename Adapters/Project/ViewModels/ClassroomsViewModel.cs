using Adapters.ViewModels;
using ReactiveUI;

namespace Adapters.Project.ViewModels;

public class ClassroomsViewModel : BaseViewModel, IRoutableViewModel
{
    public delegate ClassroomsViewModel Factory(IScreen hostScreen);

    public ClassroomsViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
    }

    public string? UrlPathSegment => "/Classrooms";
    
    public IScreen HostScreen { get; }
}