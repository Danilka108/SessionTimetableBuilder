using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace Adapters.Common.ViewModels;

public class ConfirmDialogViewModel : BaseViewModel
{
    public delegate ConfirmDialogViewModel Factory(LocalizedMessage.Header action,
        LocalizedMessage message);

    public ConfirmDialogViewModel(LocalizedMessage.Header action, LocalizedMessage message,
        ILocalizedMessageConverter messageConverter)
    {
        Finish = new Interaction<bool, Unit>();

        Action = messageConverter.Convert(action);
        Message = messageConverter.Convert(message);

        Confirm = ReactiveCommand.CreateFromTask(async () => { await Finish.Handle(true); });

        Cancel = ReactiveCommand.CreateFromTask(async () => { await Finish.Handle(false); });
    }

    public string Action { get; }

    public string Message { get; }

    public ReactiveCommand<Unit, Unit> Confirm { get; }

    public ReactiveCommand<Unit, Unit> Cancel { get; }

    public Interaction<bool, Unit> Finish { get; }
}