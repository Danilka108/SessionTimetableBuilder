using System.Reactive;
using System.Threading.Tasks;
using Adapters.Common.ViewModels;
using Avalonia;
using Avalonia.Controls.Mixins;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace Infrastructure.Common.Views;

public partial class MessageWindow : ReactiveWindow<MessageViewModel>
{
    public MessageWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        this.WhenActivated(d =>
        {
            ViewModel!
                .CloseSelf
                .RegisterHandler(DoCloseSelf)
                .DisposeWith(d);
        });
    }

    private async Task DoCloseSelf(InteractionContext<Unit, Unit> context)
    {
        context.SetOutput(Unit.Default);
        Close();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}