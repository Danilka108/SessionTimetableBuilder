using System;
using System.Collections;
using System.Collections.Generic;
using Domain.Models;

namespace App.Models;

public interface IScheduleItem
{
    public DayOfWeek DayOfWeek { get; }
    public DailySchedule DailySchedule { get; }
    public string Name { get; }
}

public class Schedule : IEnumerable<IScheduleItem>
{
    private readonly Dictionary<DayOfWeek, DailySchedule> _dailySchedules;
    private readonly Func<DayOfWeek, string> _mapDayOfWeekToString;

    public Schedule(Func<DayOfWeek, string> mapDayOfWeekToString,
        Dictionary<DayOfWeek, DailySchedule> dailySchedules)
    {
        _mapDayOfWeekToString = mapDayOfWeekToString;
        _dailySchedules = dailySchedules;
    }

    IEnumerator<IScheduleItem> IEnumerable<IScheduleItem>.GetEnumerator()
    {
        return new Enumerator(_mapDayOfWeekToString, _dailySchedules.GetEnumerator());
    }

    public IEnumerator GetEnumerator()
    {
        return (this as IEnumerable<IScheduleItem>).GetEnumerator();
    }

    private class Enumerator : IEnumerator<IScheduleItem>
    {
        private readonly IEnumerator<KeyValuePair<DayOfWeek, DailySchedule>> _enumerator;
        private readonly Func<DayOfWeek, string> _itemMapperToName;

        public Enumerator
        (
            Func<DayOfWeek, string> itemMapperToName,
            IEnumerator<KeyValuePair<DayOfWeek, DailySchedule>> enumerator
        )
        {
            _itemMapperToName = itemMapperToName;
            _enumerator = enumerator;
        }

        public IScheduleItem Current => new ScheduleItem(_itemMapperToName, _enumerator.Current);

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

    private class ScheduleItem : IScheduleItem
    {
        public ScheduleItem(Func<DayOfWeek, string> mapperToName, KeyValuePair<DayOfWeek, DailySchedule> pair)
        {
            Name = mapperToName(pair.Key);
            DayOfWeek = pair.Key;
            DailySchedule = pair.Value;
        }

        public DayOfWeek DayOfWeek { get; }
        public DailySchedule DailySchedule { get; }

        public string Name { get; }
    }
}