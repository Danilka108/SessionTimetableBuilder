using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace App.Ui.Teachers;

// ReSharper disable once PartialTypeWithSinglePart
public partial class TeachersView : UserControl
{
    private const string MondayResourceKey = "MondayString";
    private const string TuesdayResourceKey = "TuesdayString";
    private const string WednesdayResourceKey = "WednesdayString";
    private const string ThursdayResourceKey = "ThursdayString";
    private const string FridayResourceKey = "FridayString";
    private const string SaturdayResourceKey = "SaturdayString";
    private const string SundayResourceKey = "SundayString";

    public TeachersView()
    {
        DataContext = new TeachersViewModel(mapDayOfWeekToString);
        AvaloniaXamlLoader.Load(this);
    }

    private string mapDayOfWeekToString(DayOfWeek dayOfWeek)
    {
        var key = dayOfWeek switch
        {
            DayOfWeek.Monday => MondayResourceKey,
            DayOfWeek.Tuesday => TuesdayResourceKey,
            DayOfWeek.Wednesday => WednesdayResourceKey,
            DayOfWeek.Thursday => ThursdayResourceKey,
            DayOfWeek.Friday => FridayResourceKey,
            DayOfWeek.Saturday => SaturdayResourceKey,
            DayOfWeek.Sunday => SundayResourceKey
        };

        object? value = null;
        var isFound = Application.Current?.TryFindResource(key, out value);

        if (isFound != true) throw new MapDayOfWeekToStringException($"Failed to found resource '{key}'");

        if (value is string v) return v;

        throw new MapDayOfWeekToStringException("Resource with 'string' type is expected");
    }

    public class MapDayOfWeekToStringException : Exception
    {
        public MapDayOfWeekToStringException(string msg) : base(msg)
        {
        }
    }
}