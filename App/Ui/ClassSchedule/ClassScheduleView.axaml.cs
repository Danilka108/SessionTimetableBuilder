using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace App.Ui.TeacherClassSchedule;

// ReSharper disable once PartialTypeWithSinglePart
public partial class TeacherClassScheduleView : UserControl
{
    public TeacherClassScheduleView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}