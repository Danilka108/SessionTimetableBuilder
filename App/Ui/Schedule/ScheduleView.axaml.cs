using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace App.Ui.TeacherSchedule;

// ReSharper disable once PartialTypeWithSinglePart
public partial class TeacherScheduleView : UserControl
{
    public TeacherScheduleView()
    {
        AvaloniaXamlLoader.Load(this);
    }
}