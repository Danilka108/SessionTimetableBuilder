using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using App.Project.DisciplineCard;
using App.Project.DisciplineEditor;
using App.Project.ExplorerList;
using Domain.Project.UseCases.Discipline;
using ReactiveUI;

namespace App.Project.Disciplines;

public class DisciplinesViewModel : ExplorerListViewModel, IRoutableViewModel
{
    public delegate DisciplinesViewModel Factory(IScreen hostScreen, IObservable<Unit> creating);

    private readonly DisciplineCardViewModel.Factory _cardViewModelFactory;
    private readonly DisciplineEditorViewModel.Factory _editorViewModelFactory;

    private readonly ObserveAllDisciplinesUseCase _observeAllUseCase;

    public DisciplinesViewModel
    (
        IScreen hostScreen,
        IObservable<Unit> creating,
        ObserveAllDisciplinesUseCase observeAllUseCase,
        DisciplineCardViewModel.Factory cardViewModelFactory,
        DisciplineEditorViewModel.Factory editorViewModelFactory
    ) : base(creating)
    {
        HostScreen = hostScreen;

        _editorViewModelFactory = editorViewModelFactory;
        _observeAllUseCase = observeAllUseCase;
        _cardViewModelFactory = cardViewModelFactory;

        Init();
    }

    public string? UrlPathSegment => "/Disciplines";
    public IScreen HostScreen { get; }

    protected override IObservable<IEnumerable<ViewModelBase>> ObserveCards()
    {
        return _observeAllUseCase
            .Handle()
            .Select
            (
                disciplines => disciplines.Select(_cardViewModelFactory.Invoke)
            );
    }

    protected override ViewModelBase ProvideEditorViewModel()
    {
        return _editorViewModelFactory.Invoke(null);
    }
}