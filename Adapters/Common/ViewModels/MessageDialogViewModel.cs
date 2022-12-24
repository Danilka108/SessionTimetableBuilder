using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace Adapters.Common.ViewModels;

public class MessageDialogViewModel : BaseViewModel
{
    public delegate MessageDialogViewModel Factory(LocalizedMessage.Header header,
        LocalizedMessage message);

    public MessageDialogViewModel(LocalizedMessage.Header header, LocalizedMessage message,
        ILocalizedMessageConverter messageConverter)
    {
        Finish = new Interaction<Unit, Unit>();

        Header = messageConverter.Convert(header);
        Message = messageConverter.Convert(message);

        Close = ReactiveCommand.CreateFromTask(async () => { await Finish.Handle(Unit.Default); });
    }

    public string Message { get; }

    public string Header { get; }

    public ReactiveCommand<Unit, Unit> Close { get; }

    public Interaction<Unit, Unit> Finish { get; }
}