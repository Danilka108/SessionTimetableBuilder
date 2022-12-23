using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace Adapters.Common.ViewModels;

public class ConfirmDialogViewModel : BaseViewModel
{
    public delegate ConfirmDialogViewModel Factory(string action, string message);

    public ConfirmDialogViewModel(string action, string message)
    {
        Finish = new Interaction<bool, Unit>(); 
        
        Action = action;
        Message = message;

        Confirm = ReactiveCommand.CreateFromTask(async () =>
        {
            await Finish.Handle(true);
        });

        Cancel = ReactiveCommand.CreateFromTask(async () =>
        {
            await Finish.Handle(false);
        });
    }
    
    public string Action { get; }
    
    public string Message { get; }
    
    public ReactiveCommand<Unit, Unit> Confirm { get; }
    
    public ReactiveCommand<Unit, Unit> Cancel { get; }
    
    public Interaction<bool, Unit> Finish { get; }
}