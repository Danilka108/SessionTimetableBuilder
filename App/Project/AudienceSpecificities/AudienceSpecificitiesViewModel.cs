using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using App.Project.AudienceSpecificityCard;
using App.Project.AudienceSpecificityEditor;
using App.Project.ExplorerList;
using Domain;
using Domain.Project.Models;
using Domain.Project.UseCases;
using Domain.Project.UseCases.AudienceSpecificity;
using ReactiveUI;

namespace App.Project.AudienceSpecificities;

public class AudienceSpecificitiesViewModel : ExplorerListViewModel, IRoutableViewModel
{
    public delegate AudienceSpecificitiesViewModel Factory(IScreen hostScreen);

    private readonly ObserveAllAudienceSpecificitiesUseCase _observeAllUseCase;
    private readonly AudienceSpecificityEditorViewModel.Factory _editorViewModelFactory;
    private readonly AudienceSpecificityCardViewModel.Factory _cardViewModelFactory;

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

    public string? UrlPathSegment { get; } = "/AudienceSpecificities";
    public IScreen HostScreen { get; }
}