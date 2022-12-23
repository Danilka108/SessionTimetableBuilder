using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace Adapters.Common.ViewModels;

public class MessageDialogViewModel : BaseViewModel
{
    public delegate MessageDialogViewModel Factory(LocalizedMessage.Header header, LocalizedMessage message);

    public MessageDialogViewModel(LocalizedMessage.Header header, LocalizedMessage message)
    {
        Finish = new Interaction<Unit, Unit>();
        
        Header = header;
        Message = message;
        
        Close = ReactiveCommand.CreateFromTask(async () =>
        {
            await Finish.Handle(Unit.Default);
        });
    }
    
    public LocalizedMessage Message { get; }

    public LocalizedMessage.Header Header { get; }
    
    public ReactiveCommand<Unit, Unit> Close { get; }
    
    public Interaction<Unit, Unit> Finish { get; }
}