using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace Adapters.Common.ViewModels;

public class MessageDialogViewModel : BaseViewModel
{
    public delegate MessageDialogViewModel Factory(string header, string message);

    public MessageDialogViewModel(string header, string message)
    {
        Finish = new Interaction<Unit, Unit>();
        
        Header = header;
        Message = message;
        
        Close = ReactiveCommand.CreateFromTask(async () =>
        {
            await Finish.Handle(Unit.Default);
        });
    }
    
    public string Message { get; }
    
    public string Header { get; }
    
    public ReactiveCommand<Unit, Unit> Close { get; }
    
    public Interaction<Unit, Unit> Finish { get; }
}