using App.Controls.Schedule;

namespace App.Controls.Teacher;

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