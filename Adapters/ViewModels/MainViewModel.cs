using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace Adapters.ViewModels;

public class MainViewModel : BaseViewModel
{
    public delegate MainViewModel Factory();

    public MainViewModel()
    {
        ShowProject = new Interaction<Unit, Unit>();

        OpenProject = ReactiveCommand.CreateFromTask(async () =>
        {
            await ShowProject.Handle(Unit.Default);
        });
    }

    public Interaction<Unit, Unit> ShowProject { get; }

    public ReactiveCommand<Unit, Unit> OpenProject { get; }
}