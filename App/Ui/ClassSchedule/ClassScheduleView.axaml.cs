using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace App.Ui.ClassSchedule;

// ReSharper disable once PartialTypeWithSinglePart
public partial class ClassScheduleView : UserControl
{
    public ClassScheduleView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}