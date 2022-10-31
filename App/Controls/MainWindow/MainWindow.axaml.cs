using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Controls.MainWindow;

// ReSharper disable once PartialTypeWithSinglePart
public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }
}