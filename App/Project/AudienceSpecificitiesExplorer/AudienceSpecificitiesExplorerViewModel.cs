using Domain.Project.UseCases;
using ReactiveUI;

namespace App.Project.AudienceSpecificitiesExplorer;

public class AudienceSpecificitiesExplorerViewModel : ViewModelBase, IRoutableViewModel
{
    public delegate AudienceSpecificitiesExplorerViewModel Factory(IScreen hostScreen);

    // private readonly ObservableAsPropertyHelper<IEnumerable<AudienceSpecificityViewModel>> _specificities;

    public AudienceSpecificitiesExplorerViewModel(IScreen hostScreen,
        ObserveAllSpecificitiesUseCase observeAllSpecificitiesUseCase)
    {
        HostScreen = hostScreen;
        // _specificities = observeAllSpecificitiesUseCase
        //     .Handle()
        //     .Select(specificities => specificities.Select(specificityViewModelFactory.Invoke))
        //     .ToProperty(this, vm => vm.Specificities);
    }

    // public IEnumerable<AudienceSpecificityViewModel> Specificities => _specificities.Value;
    public string? UrlPathSegment { get; } = "/AudienceExplorer";
    public IScreen HostScreen { get; }
}