using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using App.Project.AudienceSpecificityCard;
using App.Project.AudienceSpecificityEditor;
using Domain;
using Domain.Project.Models;
using Domain.Project.UseCases;
using ReactiveUI;

namespace App.Project.AudienceSpecificities;

public class AudienceSpecificitiesViewModel : ViewModelBase, IRoutableViewModel
{
    public delegate AudienceSpecificitiesViewModel Factory(IScreen hostScreen);

    private readonly AudienceSpecificityEditorViewModel.Factory _editorViewModelFactory;
    private readonly AudienceSpecificityCardViewModel.Factory _cardViewModelFactory;

    private readonly ObservableAsPropertyHelper<IEnumerable<AudienceSpecificityCardViewModel>>
        _cards;

    public AudienceSpecificitiesViewModel
    (
        IScreen hostScreen,
        ObserveAllAudienceSpecificitiesUseCase observeAllAudienceSpecificitiesUseCase,
        AudienceSpecificityEditorViewModel.Factory editorViewModelFactory,
        AudienceSpecificityCardViewModel.Factory cardViewModelFactory
    )
    {
        HostScreen = hostScreen;

        _editorViewModelFactory = editorViewModelFactory;
        _cardViewModelFactory = cardViewModelFactory;

        OpenEditor = new Interaction<AudienceSpecificityEditorViewModel, Unit>();

        Create = ReactiveCommand.CreateFromTask
            (DoCreateSpecificity);

        _cards = observeAllAudienceSpecificitiesUseCase
            .Handle()
            .Select(MapSpecificitiesToCards)
            .ToProperty(this, vm => vm.Cards);
    }

    private async Task DoCreateSpecificity()
    {
        await OpenEditor.Handle(_editorViewModelFactory.Invoke(null));
    }

    private IEnumerable<AudienceSpecificityCardViewModel> MapSpecificitiesToCards
        (IEnumerable<IdentifiedModel<AudienceSpecificity>> specificities)
    {
        return specificities.Select(
            s => _cardViewModelFactory.Invoke(s));
    }

    public Interaction<AudienceSpecificityEditorViewModel, Unit> OpenEditor { get; }

    public ReactiveCommand<Unit, Unit> Create { get; }

    public IEnumerable<AudienceSpecificityCardViewModel> Cards => _cards.Value;

    public string? UrlPathSegment { get; } = "/AudienceSpecificities";
    public IScreen HostScreen { get; }
}