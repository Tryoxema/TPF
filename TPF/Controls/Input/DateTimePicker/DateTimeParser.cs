using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace TPF.Controls
{
    public static class DateTimeParser
    {
        public static bool TryParse(string value, DateTime referenceDate, out DateTime result)
        {
            return TryParse(value, referenceDate, DateTimeFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParse(string value, DateTime referenceDate, DateTimeFormatInfo dateTimeFormat, out DateTime result)
        {
            result = referenceDate;

            if (string.IsNullOrWhiteSpace(value)) return false;

            SplitDateAndTime(value, dateTimeFormat, out var datePart, out var timePart);

            var dateParsed = DateParser.TryParse(datePart, referenceDate, dateTimeFormat, out var date);
            var timeParsed = TimeParser.TryParse(timePart, referenceDate, dateTimeFormat, out var time);

            // Wenn es gar keine Zeit gab, dann tun wir so als ob es erfolgreich war
            if (string.IsNullOrWhiteSpace(timePart)) timeParsed = true;

            result = MergeDateAndTime(date, time, dateTimeFormat);

            return dateParsed && timeParsed;
        }

        public static bool TryParseDayOfWeek(string value, DateTime referenceDate, DayOfWeekBehavior behavior, out DateTime result)
        {
            return TryParseDayOfWeek(value, referenceDate, behavior, DateTimeFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParseDayOfWeek(string value, DateTime referenceDate, DayOfWeekBehavior behavior, DateTimeFormatInfo dateTimeFormat, out DateTime result)
        {
            result = referenceDate;

            if (string.IsNullOrWhiteSpace(value)) return false;

            SplitDateAndTime(value, dateTimeFormat, out var datePart, out var timePart);

            var dateParsed = DateParser.TryParseDayOfWeek(datePart, referenceDate, behavior, dateTimeFormat, out var date);
            var timeParsed = TimeParser.TryParse(timePart, referenceDate, dateTimeFormat, out var time);

            // Wenn es gar keine Zeit gab, dann tun wir so als ob es erfolgreich war
            if (string.IsNullOrWhiteSpace(timePart)) timeParsed = true;

            result = MergeDateAndTime(date, time, dateTimeFormat);

            return dateParsed && timeParsed;
        }

        public static bool TryParseSpecialDay(string value, DateTime referenceDate, SpecialDayCollection specialDays, out DateTime result)
        {
            return TryParseSpecialDay(value, referenceDate, specialDays, DateTimeFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParseSpecialDay(string value, DateTime referenceDate, SpecialDayCollection specialDays, DateTimeFormatInfo dateTimeFormat, out DateTime result)
        {
            result = referenceDate;

            if (string.IsNullOrWhiteSpace(value)) return false;

            SplitDateAndTime(value, dateTimeFormat, out var datePart, out var timePart);

            var dateParsed = DateParser.TryParseSpecialDay(datePart, referenceDate, specialDays, dateTimeFormat, out var date);
            var timeParsed = TimeParser.TryParse(timePart, referenceDate, dateTimeFormat, out var time);

            // Wenn es gar keine Zeit gab, dann tun wir so als ob es erfolgreich war
            if (string.IsNullOrWhiteSpace(timePart)) timeParsed = true;

            result = MergeDateAndTime(date, time, dateTimeFormat);

            return dateParsed && timeParsed;
        }

        private static void SplitDateAndTime(string input, DateTimeFormatInfo dateTimeFormat, out string datePart, out string timePart)
        {
            var timeRegex = new Regex(GetTimeRegex(dateTimeFormat), RegexOptions.IgnoreCase);

            var timeMatch = timeRegex.Match(input);

            if (timeMatch.Success)
            {
                timePart = timeMatch.Value;
                datePart = input.Remove(timeMatch.Index, timeMatch.Length).Trim();
            }
            else
            {
                var parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length > 1)
                {
                    timePart = parts.Last();
                    datePart = input.Substring(0, input.Length - timePart.Length).Trim();
                }
                else
                {
                    datePart = input;
                    timePart = "";
                }
            }
        }

        private static string GetTimeRegex(DateTimeFormatInfo dateTimeFormat)
        {
            if (string.IsNullOrWhiteSpace(dateTimeFormat.AMDesignator) || string.IsNullOrWhiteSpace(dateTimeFormat.PMDesignator)) return @"\d+(:\d+)?((:\d+)?((\W|_)*(AM|PM))|(:\d+))";
            else return $@"\d+(:\d+)?((:\d+)?((\W|_)*({dateTimeFormat.AMDesignator}|{dateTimeFormat.PMDesignator}))|(:\d+))";
        }

        private static DateTime MergeDateAndTime(DateTime date, DateTime time, DateTimeFormatInfo dateTimeFormat)
        {
            var calendar = dateTimeFormat.Calendar;

            return new DateTime(calendar.GetYear(date), calendar.GetMonth(date), calendar.GetDayOfMonth(date),
                                calendar.GetHour(time), calendar.GetMinute(time), calendar.GetSecond(time), (int)calendar.GetMilliseconds(time),
                                calendar);
        }
    }
}