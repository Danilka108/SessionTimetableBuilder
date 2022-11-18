using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.CommonControls.ConfirmWindow;

public partial class ConfirmWindow : ReactiveWindow<ConfirmWindowViewModel>
{
    public ConfirmWindow()
    {
        this.WhenActivated
        (
            d =>
            {
                ViewModel!
                    .Close
                    .Subscribe(_ => Close(false))
                    .DisposeWith(d);

                ViewModel!
                    .Confirm
                    .Subscribe(_ => Close(true))
                    .DisposeWith(d);
            }
        );

        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}