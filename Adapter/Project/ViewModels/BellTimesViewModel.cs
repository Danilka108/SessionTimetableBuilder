using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using App.Project.BellTimeCard;
using App.Project.BellTimeEditor;
using App.Project.ExplorerList;
using Domain.Project.UseCases.BellTime;
using ReactiveUI;

namespace App.Project.BellTimes;

public class BellTimesViewModel : ExplorerListViewModel, IRoutableViewModel
{
    public delegate BellTimesViewModel Factory(IScreen hostScreen, IObservable<Unit> creating);

    private readonly BellTimeCardViewModel.Factory _cardViewModelFactory;
    private readonly BellTimeEditorViewModel.Factory _editorViewModelFactory;

    private readonly ObserveAllBellTimesUseCase _observeAllUseCase;

    public BellTimesViewModel
    (
        IScreen hostScreen,
        IObservable<Unit> creating,
        BellTimeCardViewModel.Factory cardViewModelFactory,
        ObserveAllBellTimesUseCase observeAllUseCase,
        BellTimeEditorViewModel.Factory editorViewModelFactory
    ) : base(creating)
    {
        HostScreen = hostScreen;

        _editorViewModelFactory = editorViewModelFactory;
        _observeAllUseCase = observeAllUseCase;
        _cardViewModelFactory = cardViewModelFactory;

        Init();
    }

    public string UrlPathSegment => "/BellTimes";
    public IScreen HostScreen { get; }

    protected override IObservable<IEnumerable<ViewModelBase>> ObserveCards()
    {
        return _observeAllUseCase.Handle()
            .Select
            (
                bellTimes => bellTimes.Select(_cardViewModelFactory.Invoke)
            );
    }

    protected override ViewModelBase ProvideEditorViewModel()
    {
        return _editorViewModelFactory.Invoke(null);
    }
}