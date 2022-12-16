using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;

namespace App.Project.ExplorerList;

public abstract class ExplorerListViewModel : ViewModelBase, IActivatableViewModel
{
    private ObservableAsPropertyHelper<IEnumerable<ViewModelBase>> _cards;

    public ExplorerListViewModel(IObservable<Unit> creating)
    {
        Activator = new ViewModelActivator();
        OpenEditor = new Interaction<ViewModelBase, Unit>();

        this.WhenActivated(d =>
        {
            creating
                .SelectMany((_, __) => OpenEditor.Handle(ProvideEditorViewModel()))
                .Subscribe()
                .DisposeWith(d);
        });
    }

    public IEnumerable<ViewModelBase> Cards => _cards.Value;

    public Interaction<ViewModelBase, Unit> OpenEditor { get; }

    public void Init()
    {
        _cards = ObserveCards()
            .ToProperty(this, vm => vm.Cards);
    }

    protected abstract IObservable<IEnumerable<ViewModelBase>> ObserveCards();

    protected abstract ViewModelBase ProvideEditorViewModel();

    public ViewModelActivator Activator { get; }
}