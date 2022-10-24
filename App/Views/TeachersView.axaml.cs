using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using App.ViewModels;

namespace App.Views;

public partial class TeachersView : UserControl
{
    public TeachersView()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new TeachersViewModel();
    }
}