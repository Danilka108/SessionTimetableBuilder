using System.Collections.Generic;
using System.Linq;
using App.Controls.DailySchedule;
using DynamicData.Kernel;

namespace App.Controls.Schedule;

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