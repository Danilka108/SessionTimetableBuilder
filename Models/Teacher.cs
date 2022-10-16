using System.Collections;
using System.Collections.Generic;

namespace Avalonia.SessionTimetableBuilder.Models;

public record LessonTimeBounds(BellTime Start, BellTime End);

public record TeacherScheduleItem(int Index, bool IsAvailable, LessonTimeBounds LessonTimeBounds);

public record Teacher(string Name, IEnumerable<TeacherScheduleItem> Schedule);