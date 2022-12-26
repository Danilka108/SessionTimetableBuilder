using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace Adapters.Common.ViewModels;

public class MessageDialogViewModel : BaseViewModel
{
    public delegate MessageDialogViewModel Factory(LocalizedMessage.Letter letter,
        LocalizedMessage message);

    public MessageDialogViewModel(LocalizedMessage.Letter letter, LocalizedMessage message,
        ILocalizedMessageConverter messageConverter)
    {
        Finish = new Interaction<Unit, Unit>();

        Header = messageConverter.Convert(letter);
        Message = messageConverter.Convert(message);

        Close = ReactiveCommand.CreateFromTask(async () => { await Finish.Handle(Unit.Default); });
    }

    public string Message { get; }

    public string Header { get; }

    public ReactiveCommand<Unit, Unit> Close { get; }

    public Interaction<Unit, Unit> Finish { get; }
}