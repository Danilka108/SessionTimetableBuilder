using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.SessionTimetableBuilder.ViewModels;

namespace Avalonia.SessionTimetableBuilder.Views;

public partial class TeachersView : UserControl
{
    public TeachersView()
    {
        InitializeComponent();

        DataContext = new TeachersViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}