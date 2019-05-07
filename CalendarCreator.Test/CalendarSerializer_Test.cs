namespace CalendarCreator.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CalendarCreator.Models;
    using CalendarCreator.Services;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CalendarSerializer_Test
    {
        private const string OneEvent = @"BEGIN:VCALENDAR
VERSION:2.0
PRODID:-//hacksw/handcal//NONSGML v1.0//EN
BEGIN:VEVENT
DTSTART:20150101T143000Z
DTEND:20150101T153000Z
SUMMARY:Möte 1
END:VEVENT
END:VCALENDAR";

        private const string TwoEvents = @"BEGIN:VCALENDAR
VERSION:2.0
PRODID:-//hacksw/handcal//NONSGML v1.0//EN
BEGIN:VEVENT
DTSTART:20150101T143000Z
DTEND:20150101T153000Z
SUMMARY:Möte 1
END:VEVENT
BEGIN:VEVENT
DTSTART:20150101T183000Z
DTEND:20150101T193000Z
SUMMARY:Möte 2
END:VEVENT
END:VCALENDAR";

        private readonly CalendarEvent Event1 = new CalendarEvent
                                                    {
                                                        Title = "Möte 1",
                                                        From = new DateTime(2015, 01, 01, 15, 30, 0),
                                                        Until = new DateTime(2015, 01, 01, 16, 30, 0)
                                                    };

        private readonly CalendarEvent Event2 = new CalendarEvent
                                                    {
                                                        Title = "Möte 2",
                                                        From = new DateTime(2015, 01, 01, 19, 30, 0),
                                                        Until = new DateTime(2015, 01, 01, 20, 30, 0)
                                                    };


        [TestMethod]
        public void When_adding_one_event_Then_it_can_be_serialized()
        {
            var events = new List<CalendarEvent> { Event1 };

            var ical = CalendarSerializer.SerializeToIcal(events);

            Assert.AreEqual(OneEvent, ical);
        }


        [TestMethod]
        public void When_adding_two_events_Then_they_can_be_serialized()
        {
            var events = new List<CalendarEvent> { Event1, Event2 };

            var ical = CalendarSerializer.SerializeToIcal(events);

            Assert.AreEqual(TwoEvents, ical);
        }


        [TestMethod]
        public void When_one_line_with_correct_format_Then_one_event_should_be_returned()
        {
            var events = CalendarSerializer.DeserializeToCalendarEvents("Titel", "2015-01-01	Tisd	1530	1630").ToList();

            Assert.AreEqual(Event1.From, events.First().From);
            Assert.AreEqual(Event1.Until, events.First().Until);
        }


        [TestMethod]
        public void When_one_line_with_incorrect_format_Then_no_event_should_be_returned()
        {
            var events = CalendarSerializer.DeserializeToCalendarEvents("Titel", "2015-01-01	Tisd        ");

            Assert.IsFalse(events.Any());
        }


        [TestMethod]
        public void When_one_line_with_correct_format_Then_that_event_should_be_returned()
        {
            var events = CalendarSerializer.DeserializeToCalendarEvents("Titel", "2015-04-28	Tisd	945	2030").ToList();

            Assert.AreEqual(new DateTime(2015, 4, 28, 20, 30, 0), events.First().Until);
            Assert.AreEqual(new DateTime(2015, 4, 28, 9, 45, 0), events.First().From);
        }
    }
}