using System;
using System.Collections;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Domain.Models;

namespace App.Models;

public class Schedule : IEnumerable<Schedule.IItem>
{
    private readonly Dictionary<DayOfWeek, DailySchedule> _dailySchedules;

    public Schedule(
        Dictionary<DayOfWeek, DailySchedule> dailySchedules)
    {
        _dailySchedules = dailySchedules;
    }

    IEnumerator<IItem> IEnumerable<IItem>.GetEnumerator()
    {
        return new Enumerator(_dailySchedules.GetEnumerator());
    }

    public IEnumerator GetEnumerator()
    {
        return (this as IEnumerable<IItem>).GetEnumerator();
    }

    public interface IItem
    {
        public DayOfWeek DayOfWeek { get; }
        public DailySchedule DailySchedule { get; }
        public string Name { get; }
    }

    private class Enumerator : IEnumerator<IItem>
    {
        private readonly IEnumerator<KeyValuePair<DayOfWeek, DailySchedule>> _enumerator;

        public Enumerator
        (
            IEnumerator<KeyValuePair<DayOfWeek, DailySchedule>> enumerator
        )
        {
            _enumerator = enumerator;
        }

        public IItem Current => new Item(_enumerator.Current);

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        public void Reset()
        {
            _enumerator.Reset();
        }

        public void Dispose()
        {
            _enumerator.Dispose();
        }
    }

    private class Item : IItem
    {
        private const string MondayResourceKey = "MondayString";
        private const string TuesdayResourceKey = "TuesdayString";
        private const string WednesdayResourceKey = "WednesdayString";
        private const string ThursdayResourceKey = "ThursdayString";
        private const string FridayResourceKey = "FridayString";
        private const string SaturdayResourceKey = "SaturdayString";
        private const string SundayResourceKey = "SundayString";

        public Item(KeyValuePair<DayOfWeek, DailySchedule> pair)
        {
            DayOfWeek = pair.Key;
            DailySchedule = pair.Value;
        }

        public DayOfWeek DayOfWeek { get; }
        public DailySchedule DailySchedule { get; }

        public string Name => MapDayOfWeekToStringResource();

        private string MapDayOfWeekToStringResource()
        {
            var key = DayOfWeek switch
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

            if (isFound != true)
                throw new FindResourceException($"Failed to find resource '{key}'");

            if (value is string v) return v;

            throw new FindResourceException("Resource with 'string' type is expected");
        }
    }
}