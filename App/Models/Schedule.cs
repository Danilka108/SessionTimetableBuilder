using System;
using System.Collections;
using System.Collections.Generic;

namespace App.Ui.Models;

public class Schedule : IEnumerable<Schedule.IItem>
{
    private readonly Dictionary<DayOfWeek, Domain.Models.DailySchedule> _dailySchedules;
    private readonly Func<DayOfWeek, string> _mapDayOfWeekToString;

    public Schedule(Func<DayOfWeek, string> mapDayOfWeekToString,
        Dictionary<DayOfWeek, Domain.Models.DailySchedule> dailySchedules)
    {
        _mapDayOfWeekToString = mapDayOfWeekToString;
        _dailySchedules = dailySchedules;
    }

    IEnumerator<IItem> IEnumerable<IItem>.GetEnumerator()
    {
        return new Enumerator(_mapDayOfWeekToString, _dailySchedules.GetEnumerator());
    }

    public IEnumerator GetEnumerator()
    {
        return (this as IEnumerable<IItem>).GetEnumerator();
    }

    public interface IItem
    {
        public DayOfWeek DayOfWeek { get; }
        public Domain.Models.DailySchedule DailySchedule { get; }
        public string Name { get; }
    }

    private class Enumerator : IEnumerator<IItem>
    {
        private readonly Func<DayOfWeek, string> _itemMapperToName;
        private Dictionary<DayOfWeek, Domain.Models.DailySchedule>.Enumerator _dictionaryEnumerator;

        public Enumerator
        (
            Func<DayOfWeek, string> itemMapperToName,
            Dictionary<DayOfWeek, Domain.Models.DailySchedule>.Enumerator dictionaryEnumerator
        )
        {
            _itemMapperToName = itemMapperToName;
            _dictionaryEnumerator = dictionaryEnumerator;
        }

        public IItem Current => new Item(_itemMapperToName, _dictionaryEnumerator.Current);

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            return _dictionaryEnumerator.MoveNext();
        }

        public void Reset()
        {
            (_dictionaryEnumerator as IEnumerator).Reset();
        }

        public void Dispose()
        {
            _dictionaryEnumerator.Dispose();
        }
    }

    private class Item : IItem
    {
        public Item(Func<DayOfWeek, string> mapperToName, KeyValuePair<DayOfWeek, Domain.Models.DailySchedule> pair)
        {
            Name = mapperToName(pair.Key);
            DayOfWeek = pair.Key;
            DailySchedule = pair.Value;
        }

        public DayOfWeek DayOfWeek { get; }
        public Domain.Models.DailySchedule DailySchedule { get; }

        public string Name { get; }
    }
}