namespace CalendarCreator.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using CalendarCreator.Models;

    public static class CalendarSerializer
    {
        private const string CalendarFormat = @"BEGIN:VCALENDAR
VERSION:2.0
PRODID:-//hacksw/handcal//NONSGML v1.0//EN
{0}END:VCALENDAR";

        private const string EventFormat = @"BEGIN:VEVENT
DTSTART:{0}
DTEND:{1}
SUMMARY:{2}
END:VEVENT";

        private const string SerializerDateFormat = "yyyyMMddTHHmmssZ";

        private const string DeserializerDateFormat = "yyyy-MM-dd";


        public static string SerializeToIcal(IEnumerable<CalendarEvent> events)
        {
            var eventsStringBuilder = new StringBuilder();

            foreach (var calendarEvent in events)
            {
                eventsStringBuilder.AppendLine(
                    string.Format(
                        EventFormat,
                        calendarEvent.From.ToUniversalTime().ToString(SerializerDateFormat),
                        calendarEvent.Until.ToUniversalTime().ToString(SerializerDateFormat),
                        calendarEvent.Title));
            }

            var ical = string.Format(CalendarFormat, eventsStringBuilder);

            return ical;
        }


        public static IEnumerable<CalendarEvent> DeserializeToCalendarEvents(string title, string text)
        {
            var events = new List<CalendarEvent>();

            using (var reader = new StringReader(text))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var calendarEvent = ParseLine(line);

                    if (calendarEvent == null)
                    {
                        continue;
                    }

                    calendarEvent.Title = string.Format("{0} {1}", title, calendarEvent.Until.ToString("HH:mm"));
                    events.Add(calendarEvent);
                }
            }

            return events;
        }


        private static CalendarEvent ParseLine(string line)
        {
            try
            {
                var indexOfTabBeforeFromTime = line.IndexOf('\t', line.IndexOf('\t') + 1);
                var indexOfTabBeforeUntilTime = line.IndexOf('\t', indexOfTabBeforeFromTime + 1);

                var date = DateTime.ParseExact(
                    line.Substring(0, DeserializerDateFormat.Length),
                    DeserializerDateFormat,
                    null);

                var fromTimeString = line.Substring(
                    indexOfTabBeforeFromTime + 1,
                    indexOfTabBeforeUntilTime - indexOfTabBeforeFromTime - 1);

                var untilTimeString = line.Substring(
                    indexOfTabBeforeUntilTime + 1,
                    line.Length - (indexOfTabBeforeUntilTime + 1));

                var fromTimeHours = GetHours(fromTimeString);
                var fromTimeMinutes = GetMinutes(fromTimeString);

                var untilTimeHours = GetHours(untilTimeString);
                var untilTimeMinutes = GetMinutes(untilTimeString);

                return new CalendarEvent
                           {
                               From = date.AddHours(fromTimeHours).AddMinutes(fromTimeMinutes),
                               Until = date.AddHours(untilTimeHours).AddMinutes(untilTimeMinutes)
                           };
            }
            catch (Exception)
            {
                return null;
            }
        }


        private static int GetHours(string timeString)
        {
            return Convert.ToInt32(timeString.Substring(0, timeString.Length == 3 ? 1 : 2));
        }


        private static int GetMinutes(string timeString)
        {
            return Convert.ToInt32(timeString.Substring(timeString.Length == 3 ? 1 : 2, 2));
        }
    }
}