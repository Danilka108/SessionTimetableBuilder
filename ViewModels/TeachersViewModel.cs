using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.SessionTimetableBuilder.Models;

namespace Avalonia.SessionTimetableBuilder.ViewModels;

public class TeachersViewModel : ViewModelBase
{
    public readonly BellTime[] AllBellTimes = new[]
    {
        new BellTime
        {
            Hour = 10,
            Minute = 15,
        },
        new BellTime
        {
            Hour = 12,
            Minute = 00,
        },
        new BellTime
        {
            Hour = 14,
            Minute = 00,
        },
        new BellTime
        {
            Hour = 15,
            Minute = 45,
        },
        new BellTime
        {
            Hour = 17,
            Minute = 30,
        },
        new BellTime
        {
            Hour = 19,
            Minute = 15,
        },
        new BellTime
        {
            Hour = 21,
            Minute = 00,
        },
    };

    public TeachersViewModel()
    {
        Items = new ObservableCollection<Teacher>(new[]
            {
                new Teacher
                {
                    Name = "Попов Евгений Александрович",
                    Schedule = new[]
                    {
                        new ScheduleItem(true, AllBellTimes[0]),
                        new ScheduleItem(false, AllBellTimes[1]),
                        new ScheduleItem(true, AllBellTimes[2]),
                        new ScheduleItem(true, AllBellTimes[3])
                    }
                }
            }
        );
    }

    public ObservableCollection<Teacher> Items { get; }
}