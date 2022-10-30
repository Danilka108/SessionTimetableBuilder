using System.Collections;

namespace Domain.Models;

public record ClassTimeBounds(BellTime Start, BellTime End);

public record ClassSchedule(uint Index, ClassTimeBounds TimeBounds, bool IsLast);

public class DailySchedule : IEnumerable<ClassSchedule>
{
    private readonly IEnumerable<ClassSchedule> _classesSchedules;

    public DailySchedule(IEnumerable<ClassSchedule> classesSchedules)
    {
        _classesSchedules = classesSchedules;
    }

    public DailySchedule(IEnumerable<ClassTimeBounds> classesTimeBounds)
    {
        var timeBoundsAsList = classesTimeBounds.ToList();

        _classesSchedules = timeBoundsAsList.Select((bounds, i) =>
            new ClassSchedule((uint)i, bounds, i == timeBoundsAsList.Count - 1));
    }

    public IEnumerator<ClassSchedule> GetEnumerator()
    {
        return _classesSchedules.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public record Schedule(Dictionary<DayOfWeek, IEnumerable<ClassSchedule>> DailySchedules);