using App.ViewModels;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace App.Ui.Teachers;

public class Teachers : UserControl
{
    public Teachers()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new TeachersViewModel();
    }
}