using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreBoard.Service.Models
{
    public class TaskSchedule
    {
        public Guid TaskDefinitionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RecurrencePattern Pattern { get; set; }
    }

    public class ScheduleBuilder
    {
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
            _pattern.Until = endDate;

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

                    _schedule.EndDate = lastOcurrance?.Period.EndTime.Value ?? DateTime.MaxValue;
                }
                else
                {
                    _pattern.Until = DateTime.MaxValue;
                }
            }
            _schedule.EndDate = _pattern.Until;

            return _schedule;
        }
    }
}
