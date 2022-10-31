using System;
using System.Collections.Generic;
using System.Linq;
using App.Controls.Groups;
using App.Controls.Teachers;
using Domain.Models;

namespace App.Controls.MainWindow;

public class MainWindowViewModel : ViewModelBase
{
    private static readonly ClassTimeBounds[] ClassesTileBounds =
    {
        new(new BellTime(8, 30), new BellTime(10, 00)),
        new(new BellTime(10, 15), new BellTime(11, 45)),
        new(new BellTime(12, 00), new BellTime(13, 30)),
        new(new BellTime(14, 00), new BellTime(15, 30)),
        new(new BellTime(15, 45), new BellTime(17, 15)),
        new(new BellTime(17, 30), new BellTime(19, 00)),
        new(new BellTime(19, 15), new BellTime(20, 45)),
        new(new BellTime(21, 00), new BellTime(22, 30))
    };

    public TeachersViewModel TeachersViewModel => new(CreateTestTeachers());
    public GroupsViewModel GroupsViewModel => new();

    private static IEnumerable<Models.Teacher> CreateTestTeachers()
    {
        var timeBounds1 = ClassesTileBounds.Where((_, i) => i % 2 == 0).ToList();
        var timeBounds2 = ClassesTileBounds.Where((_, i) => i % 2 != 0).ToList();

        var teacher1 = new Models.Teacher("Попов", "Евгений", "Александрович", new Models.Schedule(
            new Dictionary<DayOfWeek, Domain.Models.DailySchedule>
            {
                { DayOfWeek.Monday, new Domain.Models.DailySchedule(timeBounds1) },
                { DayOfWeek.Tuesday, new Domain.Models.DailySchedule(timeBounds2) },
                { DayOfWeek.Wednesday, new Domain.Models.DailySchedule(timeBounds1) },
                { DayOfWeek.Thursday, new Domain.Models.DailySchedule(timeBounds2) },
                { DayOfWeek.Friday, new Domain.Models.DailySchedule(timeBounds1) },
                { DayOfWeek.Saturday, new Domain.Models.DailySchedule(timeBounds2) },
                { DayOfWeek.Sunday, new Domain.Models.DailySchedule(timeBounds1) }
            }));

        var teacher2 = new Models.Teacher("Левяков", "Станислав", "Вячеславович", new Models.Schedule(
            new Dictionary<DayOfWeek, Domain.Models.DailySchedule>
            {
                { DayOfWeek.Monday, new Domain.Models.DailySchedule(timeBounds2) },
                { DayOfWeek.Tuesday, new Domain.Models.DailySchedule(timeBounds1) },
                { DayOfWeek.Wednesday, new Domain.Models.DailySchedule(timeBounds2) },
                { DayOfWeek.Thursday, new Domain.Models.DailySchedule(timeBounds1) },
                { DayOfWeek.Friday, new Domain.Models.DailySchedule(timeBounds2) },
                { DayOfWeek.Saturday, new Domain.Models.DailySchedule(timeBounds1) },
                { DayOfWeek.Sunday, new Domain.Models.DailySchedule(timeBounds2) }
            }));

        return new[]
        {
            teacher1, teacher2
        };
    }
}