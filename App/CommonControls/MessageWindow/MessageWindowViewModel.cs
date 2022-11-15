using System.Reactive;
using ReactiveUI;

namespace App.CommonControls.MessageWindow;

public class MessageWindowViewModel : ViewModelBase
{
    public MessageWindowViewModel(string header, string message)
    {
        Header = header;
        Message = message;

        Close = ReactiveCommand.Create
        (
            () =>
            {
            }
        );
    }

    public string Header { get; }

    public string Message { get; }

    public ReactiveCommand<Unit, Unit> Close { get; }
}