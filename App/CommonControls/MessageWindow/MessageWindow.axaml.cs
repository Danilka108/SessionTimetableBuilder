using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Reactive.Disposables;

namespace App.CommonControls.MessageWindow;

public partial class MessageWindow : ReactiveWindow<MessageWindowViewModel>
{
    public MessageWindow()
    {
        this.WhenActivated
        (
            d =>
            {
                ViewModel!.Close.Subscribe(_ => Close())
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