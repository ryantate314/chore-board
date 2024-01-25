using ChoreBoard.Service.Models;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;

namespace ChoreBoard.Service
{
    public class ScheduleBuilder
    {
        // Max value supported by SQL
        public static readonly DateTime MAX_DATE = new DateTime(9999, 12, 31, 23, 59, 59);

        private TaskSchedule _schedule;
        private RecurrencePattern _pattern;

        public ScheduleBuilder(DateTime startDate)
        {
            _schedule = new TaskSchedule()
            {
                StartDate = startDate
            };

            _pattern = new RecurrencePattern();
        }

        public ScheduleBuilder SetFrequency(Frequency frequency)
        {
            _pattern.Frequency = MapFrequency(frequency);

            return this;
        }

        public ScheduleBuilder SetInterval(int interval)
        {
            _pattern.Interval = interval;

            return this;
        }

        public ScheduleBuilder SetCount(int count)
        {
            _pattern.Count = count;

            return this;
        }

        public ScheduleBuilder SetEndDate(DateTime endDate)
        {
            _pattern.Until = endDate > MAX_DATE ? MAX_DATE : endDate;

            return this;
        }

        public ScheduleBuilder SetDaysOfWeek(IEnumerable<int> daysOfWeek)
        {
            _pattern.ByDay = daysOfWeek.Select(MapWeekDay)
                .ToList();

            return this;
        }

        private Ical.Net.FrequencyType MapFrequency(Frequency frequency)
        {
            return (Ical.Net.FrequencyType)frequency;
        }

        private WeekDay MapWeekDay(int daysOfWeek)
        {
            return new WeekDay(DayOfWeek.Monday, daysOfWeek);
        }

        public TaskSchedule Build()
        {
            _schedule.Pattern = _pattern;

            if (_pattern.Until == default)
            {
                if (_pattern.Count > 0)
                {
                    var evnt = new CalendarEvent()
                    {
                        RecurrenceRules = { _pattern },
                        DtStart = new CalDateTime(_schedule.StartDate)
                    };

                    Occurrence? lastOcurrance = evnt.GetOccurrences(DateTime.MinValue, DateTime.MaxValue)
                        .LastOrDefault();

                    // Cannot set both Count and Until in the RRUle
                    _schedule.EndDate = lastOcurrance?.Period.EndTime.Value ?? MAX_DATE;
                }
                else
                {
                    _schedule.EndDate = DateTime.MaxValue;
                }
            }
            else
                _schedule.EndDate = _pattern.Until;

            return _schedule;
        }
    }
}
