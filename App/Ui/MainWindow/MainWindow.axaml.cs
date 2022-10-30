using Avalonia.Controls;

namespace App.Ui.MainWindow;

// ReSharper disable once PartialTypeWithSinglePart
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}