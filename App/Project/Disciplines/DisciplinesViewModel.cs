using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using App.Project.DisciplineCard;
using App.Project.DisciplineEditor;
using App.Project.ExplorerList;
using Domain.Project.UseCases.Discipline;
using ReactiveUI;

namespace App.Project.Disciplines;

public class DisciplinesViewModel : ExplorerListViewModel, IRoutableViewModel
{
    public delegate DisciplinesViewModel Factory(IScreen hostScreen);

    private readonly ObserveAllDisciplinesUseCase _observeAllUseCase;
    private readonly DisciplineCardViewModel.Factory _cardViewModelFactory;
    private readonly DisciplineEditorViewModel.Factory _editorViewModelFactory;

    public DisciplinesViewModel
    (
        IScreen hostScreen,
        ObserveAllDisciplinesUseCase observeAllUseCase,
        DisciplineCardViewModel.Factory cardViewModelFactory,
        DisciplineEditorViewModel.Factory editorViewModelFactory
    )
    {
        HostScreen = hostScreen;
        
        _editorViewModelFactory = editorViewModelFactory;
        _observeAllUseCase = observeAllUseCase;
        _cardViewModelFactory = cardViewModelFactory;

        Init();
    }

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

    public string? UrlPathSegment => "/Disciplines";
    public IScreen HostScreen { get; }
}