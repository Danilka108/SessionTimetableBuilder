using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using App.Project.AudienceCard;
using App.Project.AudienceEditor;
using App.Project.ExplorerList;
using Avalonia.Controls.Mixins;
using Domain.Project.UseCases.Audience;
using ReactiveUI;

namespace App.Project.Audiences;

public class AudiencesViewModel : ExplorerListViewModel, IRoutableViewModel
{
    public delegate AudiencesViewModel Factory(IScreen hostScreen, IObservable<Unit> creating);

    private readonly AudienceCardViewModel.Factory _cardViewModelFactory;
    private readonly AudienceEditorViewModel.Factory _editorViewModelFactory;

    private readonly ObserveAllAudiencesUseCase _observeAllUseCase;

    public AudiencesViewModel
    (
        IScreen hostScreen,
        IObservable<Unit> creating,
        ObserveAllAudiencesUseCase observeAllUseCase,
        AudienceEditorViewModel.Factory editorViewModelFactory,
        AudienceCardViewModel.Factory cardViewModelFactory
    ) : base(creating)
    {
        HostScreen = hostScreen;

        _observeAllUseCase = observeAllUseCase;
        _editorViewModelFactory = editorViewModelFactory;
        _cardViewModelFactory = cardViewModelFactory;

        Init();
    }

    public string UrlPathSegment => "/Audiences";
    public IScreen HostScreen { get; }

    protected override IObservable<IEnumerable<ViewModelBase>> ObserveCards()
    {
        return _observeAllUseCase.Handle()
            .Select
            (
                audiences =>
                {
                    return audiences.Select(audience => _cardViewModelFactory.Invoke(audience));
                }
            );
    }

    protected override ViewModelBase ProvideEditorViewModel()
    {
        return _editorViewModelFactory.Invoke(null);
    }
}