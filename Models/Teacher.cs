namespace Avalonia.SessionTimetableBuilder.Models;

public record ScheduleItem(bool IsAvailable, BellTime BellTime);

public class Teacher
{
    public string Name { get; init; }
    public ScheduleItem[] Schedule { get; init; }
}
