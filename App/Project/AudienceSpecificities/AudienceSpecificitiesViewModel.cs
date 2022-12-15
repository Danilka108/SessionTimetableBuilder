using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using App.Project.AudienceSpecificityCard;
using App.Project.AudienceSpecificityEditor;
using App.Project.ExplorerList;
using Domain.Project.UseCases.AudienceSpecificity;
using ReactiveUI;

namespace App.Project.AudienceSpecificities;

public class AudienceSpecificitiesViewModel : ExplorerListViewModel, IRoutableViewModel
{
    public delegate AudienceSpecificitiesViewModel Factory(IScreen hostScreen);

    private readonly AudienceSpecificityCardViewModel.Factory _cardViewModelFactory;
    private readonly AudienceSpecificityEditorViewModel.Factory _editorViewModelFactory;

    private readonly ObserveAllAudienceSpecificitiesUseCase _observeAllUseCase;

    public AudienceSpecificitiesViewModel
    (
        IScreen hostScreen,
        ObserveAllAudienceSpecificitiesUseCase observeAllAudienceSpecificitiesUseCase,
        AudienceSpecificityEditorViewModel.Factory editorViewModelFactory,
        AudienceSpecificityCardViewModel.Factory cardViewModelFactory
    )
    {
        HostScreen = hostScreen;

        _observeAllUseCase = observeAllAudienceSpecificitiesUseCase;

        _editorViewModelFactory = editorViewModelFactory;
        _cardViewModelFactory = cardViewModelFactory;

        Init();
    }

    public string? UrlPathSegment { get; } = "/AudienceSpecificities";
    public IScreen HostScreen { get; }

    protected override IObservable<IEnumerable<ViewModelBase>> ObserveCards()
    {
        return _observeAllUseCase.Handle()
            .Select
            (
                specificities =>
                {
                    return specificities.Select(s => _cardViewModelFactory.Invoke(s));
                }
            );
    }

    protected override ViewModelBase ProvideEditorViewModel()
    {
        return _editorViewModelFactory.Invoke(null);
    }
}