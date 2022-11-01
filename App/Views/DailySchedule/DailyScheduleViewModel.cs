using System.Collections.Generic;
using System.Linq;
using App.Views.ClassSchedule;
using DynamicData.Kernel;

namespace App.Views.DailySchedule;

public class DailyScheduleViewModel : ViewModelBase
{
    public DailyScheduleViewModel(Models.Schedule.IItem scheduleItem)
    {
        Name = scheduleItem.Name;

        ClassScheduleViewModels =
            scheduleItem
                .DailySchedule
                .AsList()
                .Select(s => new ClassScheduleViewModel(s));
    }

    public string Name { get; }
    public IEnumerable<ClassScheduleViewModel> ClassScheduleViewModels { get; }
}