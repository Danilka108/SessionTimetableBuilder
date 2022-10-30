using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace App.Ui.TeacherDailySchedule;

// public static class ScheduleItemExtensions
// {
//     public static object ConvertToStaticResouce(this ScheduleItem item)
//     {
//         return item switch
//             {
//                 DayOfWeek.Monday => Resources["MondayString"],
//                 DayOfWeek.Tuesday => Resources["TuesdayString"],
//                 DayOfWeek.Wednesday => Resources["WednesdayString"],
//                 DayOfWeek.Thursday => Resources["ThursdayString"],
//                 DayOfWeek.Friday => Resources["FridayString"],
//                 DayOfWeek.Saturday => Resources["SaturdayString"],
//                 DayOfWeek.Sunday => Resources["SundayString"]
//             }
//             ;
//     }
// }

// ReSharper disable once PartialTypeWithSinglePart
public partial class TeacherDailyScheduleView : UserControl
{
    public TeacherDailyScheduleView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}