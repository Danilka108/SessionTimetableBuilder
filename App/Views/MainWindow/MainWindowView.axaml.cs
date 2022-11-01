using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Views.MainWindow;

// ReSharper disable once PartialTypeWithSinglePart
public partial class MainWindowView : ReactiveWindow<MainWindowViewModel>
{
    public MainWindowView()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }
}