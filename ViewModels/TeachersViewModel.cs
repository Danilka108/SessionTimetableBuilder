using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.SessionTimetableBuilder.Models;

namespace Avalonia.SessionTimetableBuilder.ViewModels;

public class TeachersViewModel : ViewModelBase
{
    public readonly LessonTimeBounds[] AllLessonsTileBounds = new[]
    {
        new LessonTimeBounds(new BellTime(8, 30), new BellTime(10, 00)),
        new LessonTimeBounds(new BellTime(10, 15), new BellTime(11, 45)),
        new LessonTimeBounds(new BellTime(12, 00), new BellTime(13, 30)),
        new LessonTimeBounds(new BellTime(14, 00), new BellTime(15, 30)),
        new LessonTimeBounds(new BellTime(15, 45), new BellTime(17, 15)),
        new LessonTimeBounds(new BellTime(17, 30), new BellTime(19, 00)),
        new LessonTimeBounds(new BellTime(19, 15), new BellTime(20, 45)),
        new LessonTimeBounds(new BellTime(21, 00), new BellTime(22, 30)),
    };

    public TeachersViewModel()
    {
        var schedule = new List<TeacherScheduleItem>();
        for (var i = 0; i < AllLessonsTileBounds.Length - 1; i++)
        {
            schedule.Add(new TeacherScheduleItem(i + 1, true, AllLessonsTileBounds[i]));
        }
        
        Items = new ObservableCollection<Teacher>(new[]
            {
                new Teacher("Попов Евгений Александрович", schedule)
            }
        );
    }

    public ObservableCollection<Teacher> Items { get; }
}