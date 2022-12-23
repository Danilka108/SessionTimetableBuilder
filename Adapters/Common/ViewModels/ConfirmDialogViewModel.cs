using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace Adapters.Common.ViewModels;

public class ConfirmDialogViewModel : BaseViewModel
{
    public delegate ConfirmDialogViewModel Factory(LocalizedMessage.Header header, LocalizedMessage message);

    public ConfirmDialogViewModel(LocalizedMessage.Header header, LocalizedMessage message)
    {
        Finish = new Interaction<bool, Unit>(); 
        
        Header = header;
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
    
    public LocalizedMessage.Header Header { get; }
    
    public LocalizedMessage Message { get; }
    
    public ReactiveCommand<Unit, Unit> Confirm { get; }
    
    public ReactiveCommand<Unit, Unit> Cancel { get; }
    
    public Interaction<bool, Unit> Finish { get; }
}