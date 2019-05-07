namespace CalendarCreator.Console
{
    using System;
    using System.IO;

    using CalendarCreator.Services;

    public class Program
    {
        private static void Main(string[] args)
        {
            string fileContent;

            using (var reader = new StreamReader(@"c:\temp\data.txt"))
            {
                fileContent = reader.ReadToEnd();
            }

            var events = CalendarSerializer.DeserializeToCalendarEvents("Emilie jobbar till", fileContent);

            var calendar = CalendarSerializer.SerializeToIcal(events);

            Console.Write(calendar);
        }
    }
}