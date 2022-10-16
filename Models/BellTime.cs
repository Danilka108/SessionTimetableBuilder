using System;

namespace Avalonia.SessionTimetableBuilder.Models;

public record BellTime(byte Hour, byte Minute)
{
    public byte Hour { get; } = Hour > 24
        ? throw new ArgumentException("Must be greater than or equal " +
                                      "0 and less than or equal 24")
        : Hour;

    public byte Minute { get; } = Minute > 60
        ? throw new ArgumentException("argument 'minute' must be greater " +
                                      "than or equal 0 and less than or equal 60")
        : Hour;

    public override string ToString() => (Hour < 10 ? "0" : "") + Hour + ":" +
                          (Minute < 10 ? "0" : "") + Minute;
}