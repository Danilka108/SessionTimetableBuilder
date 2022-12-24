using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace Adapters.Preview.ViewModels;

public class PreviewViewModel : BaseViewModel
{
    public delegate PreviewViewModel Factory();

    public PreviewViewModel()
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