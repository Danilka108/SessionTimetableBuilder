using App.Views.Schedule;

namespace App.Views.Teacher;

public class TeacherViewModel : ViewModelBase
{
    public TeacherViewModel(Models.Teacher teacher)
    {
        NameWithInitials = teacher.NameWithInitials;
        ScheduleViewModel = new ScheduleViewModel(teacher.Schedule);
    }

    public string NameWithInitials { get; }
    public ScheduleViewModel ScheduleViewModel { get; }
}