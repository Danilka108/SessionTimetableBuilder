using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using App.Project.AudienceSpecificityCard;
using App.Project.AudienceSpecificityEditor;
using Domain.Project.UseCases;
using ReactiveUI;

namespace App.Project.AudienceSpecificities;

public class AudienceSpecificitiesViewModel : ViewModelBase, IRoutableViewModel
{
    public delegate AudienceSpecificitiesViewModel Factory(IScreen hostScreen);

    private readonly ObservableAsPropertyHelper<IEnumerable<AudienceSpecificityCardViewModel>>
        _specificities;

    public AudienceSpecificitiesViewModel
    (
        IScreen hostScreen,
        ObserveAllAudienceSpecificitiesUseCase observeAllAudienceSpecificitiesUseCase,
        AudienceSpecificityCardViewModel.Factory specificityCardViewModelFactory,
        AudienceSpecificityEditorViewModel.Factory editorViewModelFactory
    )
    {
        HostScreen = hostScreen;

        OpenEditor = new Interaction<AudienceSpecificityEditorViewModel, Unit>();

        Create = ReactiveCommand.CreateFromTask
            (async () => await OpenEditor.Handle(editorViewModelFactory.Invoke(null)));

        _specificities = observeAllAudienceSpecificitiesUseCase
            .Handle()
            .Select(specificities => specificities.Select(specificityCardViewModelFactory.Invoke))
            .ToProperty(this, vm => vm.Specificities);
    }

    public Interaction<AudienceSpecificityEditorViewModel, Unit> OpenEditor { get; }

    public ReactiveCommand<Unit, Unit> Create { get; }

    public IEnumerable<AudienceSpecificityCardViewModel> Specificities => _specificities.Value;

    public string? UrlPathSegment { get; } = "/AudienceSpecificities";
    public IScreen HostScreen { get; }
}