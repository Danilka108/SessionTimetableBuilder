using ReactiveUI;

namespace App.Project.AudienceExplorer;

public class AudienceExplorerViewModel : ViewModelBase, IRoutableViewModel
{
    public delegate AudienceExplorerViewModel Factory(IScreen hostScreen);

    public AudienceExplorerViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
    }

    public string? UrlPathSegment { get; } = "/AudienceExplorer";
    public IScreen HostScreen { get; }
}