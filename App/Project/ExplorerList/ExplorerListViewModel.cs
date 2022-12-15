using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;

namespace App.Project.ExplorerList;

public abstract class ExplorerListViewModel : ViewModelBase
{
    private ObservableAsPropertyHelper<IEnumerable<ViewModelBase>> _cards;

    public ExplorerListViewModel()
    {
        OpenEditor = new Interaction<ViewModelBase, Unit>();
        Create = ReactiveCommand.CreateFromTask(DoOpenEditor);
    }

    public ReactiveCommand<Unit, Unit> Create { get; }

    public IEnumerable<ViewModelBase> Cards => _cards.Value;

    public Interaction<ViewModelBase, Unit> OpenEditor { get; }

    public void Init()
    {
        _cards = ObserveCards()
            .ToProperty(this, vm => vm.Cards);
    }

    private async Task DoOpenEditor()
    {
        await OpenEditor.Handle(ProvideEditorViewModel());
    }

    protected abstract IObservable<IEnumerable<ViewModelBase>> ObserveCards();

    protected abstract ViewModelBase ProvideEditorViewModel();
}