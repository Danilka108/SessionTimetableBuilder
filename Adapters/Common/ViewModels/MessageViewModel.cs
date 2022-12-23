using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace Adapters.Common.ViewModels;

public class MessageViewModel : BaseViewModel
{
    public delegate MessageViewModel Factory(string header, string message);

    public MessageViewModel(string header, string message)
    {
        CloseSelf = new Interaction<Unit, Unit>();
        
        Header = header;
        Message = message;
        
        Close = ReactiveCommand.CreateFromTask(async () =>
        {
            await CloseSelf.Handle(Unit.Default);
        });
    }
    
    public string Message { get; }
    
    public string Header { get; }
    
    public ReactiveCommand<Unit, Unit> Close { get; }
    
    public Interaction<Unit, Unit> CloseSelf { get; }
}