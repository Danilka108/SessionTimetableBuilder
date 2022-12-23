using System.Reactive;
using System.Reactive.Linq;
using Adapters.ViewModels;
using ReactiveUI;

namespace Adapters.Preview.ViewModels;

public class PreviewViewModel : BaseViewModel
{
    public delegate PreviewViewModel Factory(); 
    
    public Interaction<Unit, Unit> ShowProject { get; }

    public ReactiveCommand<Unit, Unit> OpenProject { get; }

    public PreviewViewModel()
    {
        ShowProject = new Interaction<Unit, Unit>(); 
        
        OpenProject = ReactiveCommand.CreateFromTask(async () =>
        {
            await ShowProject.Handle(Unit.Default);
        });
    }
}