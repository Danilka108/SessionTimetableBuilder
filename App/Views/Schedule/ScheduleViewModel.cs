using System.Collections.Generic;
using System.Linq;
using App.Views.DailySchedule;
using DynamicData.Kernel;

namespace App.Views.Schedule;

public class ScheduleViewModel : ViewModelBase
{
    public ScheduleViewModel(Models.Schedule schedule)
    {
        DailyScheduleViewModels = schedule
            .AsList()
            .Select(i => new DailyScheduleViewModel(i));
    }

    public IEnumerable<DailyScheduleViewModel> DailyScheduleViewModels { get; }
}