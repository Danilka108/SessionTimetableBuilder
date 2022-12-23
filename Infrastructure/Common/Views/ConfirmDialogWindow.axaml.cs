using System.Reactive;
using System.Threading.Tasks;
using Adapters.Common.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace Infrastructure.Common.Views;

public partial class ConfirmDialogWindow : ReactiveWindow<ConfirmDialogViewModel>
{
    public ConfirmDialogWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        this.WhenActivated(d =>
        {
            ViewModel
                .Finish
                .RegisterHandler(DoFinish)
                .DisposeWith(d);
        });
    }
    
    private async Task DoFinish(InteractionContext<bool, Unit> context)
    {
        context.SetOutput(Unit.Default);
        Close(context.Input);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}