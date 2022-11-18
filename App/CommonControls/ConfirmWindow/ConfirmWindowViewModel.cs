using System.Reactive;
using ReactiveUI;

namespace App.CommonControls.ConfirmWindow;

public class ConfirmWindowViewModel : ViewModelBase
{
    public ConfirmWindowViewModel(string header, string message, string action)
    {
        Header = header;
        Message = message;
        Action = action;

        Confirm = ReactiveCommand.Create
        (
            () =>
            {
            }
        );

        Close = ReactiveCommand.Create
        (
            () =>
            {
            }
        );
    }

    public ReactiveCommand<Unit, Unit> Confirm { get; }

    public ReactiveCommand<Unit, Unit> Close { get; }

    public string Message { get; }

    public string Header { get; }

    public string Action { get; }
}