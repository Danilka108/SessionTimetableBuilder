namespace App.Controls.ClassSchedule;

public class ClassScheduleViewModel : ViewModelBase
{
    public ClassScheduleViewModel(Domain.Models.ClassSchedule classSchedule)
    {
        ClassSchedule = classSchedule;
    }

    public Domain.Models.ClassSchedule ClassSchedule { get; }
}