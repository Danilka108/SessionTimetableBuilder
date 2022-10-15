using System;

namespace Avalonia.SessionTimetableBuilder.Models;

public class BellTime
{
    private readonly byte _hour;
    public byte Hour
    {
        get => _hour;
        init
        {
            if (value is > 24 or < 0)
                throw new ArgumentException("Must be greater than or equal 0 and less than or equal 24");

            _hour = value;
        }
    }

    private readonly byte _minute;
    public byte Minute
    {
        get => _minute;
        init
        {
            if (Minute is > 60 or < 0)
                throw new ArgumentException(
                    "argument 'minute' must be greater than or equal 0 and less than or equal 60");
            _minute = value;
        }
    }

    public string Time => Hour.ToString() + ":" + Minute.ToString();
}