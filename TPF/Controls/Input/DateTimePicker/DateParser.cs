using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TPF.Controls
{
    public class DateParser
    {
        public static bool TryParse(string value, DateTime referenceDate, out DateTime result)
        {
            return TryParse(value, referenceDate, DateTimeFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParse(string value, DateTime referenceDate, DateTimeFormatInfo dateTimeFormat, out DateTime result)
        {
            var successful = false;
            result = referenceDate;

            if (string.IsNullOrWhiteSpace(value)) return false;

            var datePartOrderInfo = GetDatePartOrderInfo(dateTimeFormat);

            if (HasInputSeparators(value))
            {
                var dateParts = SplitDateString(value, dateTimeFormat);

                if (dateParts.Count > 1)
                {
                    var dateFormatParts = dateTimeFormat.ShortDatePattern.Split(new string[] { dateTimeFormat.DateSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    var yearIndex = dateFormatParts.IndexOf(dateFormatParts.FirstOrDefault(x => x.Contains("y") || x.Contains("Y")));

                    // Wenn vom Jahr nur bis zu 2 Stellen angegeben sind, dann auf aktuelles Jahrhundert anheben
                    if (yearIndex >= 0 && yearIndex < dateParts.Count && dateParts[yearIndex].Length <= 2)
                    {
                        var year = dateParts[yearIndex];

                        // Wenn beim Jahr nur eine Stelle angegeben wurde dann eine 0 vorne dran machen
                        if (year.Length == 1) year = "0" + year;

                        dateParts[yearIndex] = $"{dateTimeFormat.Calendar.GetYear(referenceDate) / 100}{year}";
                    }

                    successful = DateTime.TryParse(string.Join(dateTimeFormat.DateSeparator, dateParts), dateTimeFormat, DateTimeStyles.None, out result);

                    if (!successful) result = referenceDate;
                }
                else
                {
                    var part = dateParts[0];

                    // Fängt das Format mit Tag an?
                    if (datePartOrderInfo.FirstPart == DatePart.Day)
                    {
                        if (int.TryParse(part, out var day))
                        {
                            successful = TryCreateDateFromDay(day, referenceDate, dateTimeFormat, out result);
                        }
                    }// Fängt das Format mit Monat an?
                    else if (datePartOrderInfo.FirstPart == DatePart.Month)
                    {
                        // Ist der Monat als Zahl oder als Wort geschrieben?
                        if (int.TryParse(part, out var month))
                        {
                            successful = TryCreateDateFromMonth(month, referenceDate, dateTimeFormat, out result);
                        }
                        else
                        {
                            var isLetter = char.IsLetter(part, part.Length - 1);

                            if (isLetter)
                            {
                                month = GetMonthByName(part, dateTimeFormat);

                                successful = TryCreateDateFromMonth(month, referenceDate, dateTimeFormat, out result);
                            }
                        }
                    }// Fängt das Format mit Jahr an?
                    else if (datePartOrderInfo.FirstPart == DatePart.Year)
                    {
                        if (int.TryParse(part, out var year))
                        {
                            successful = TryCreateDateFromYear(year, referenceDate, dateTimeFormat, out result);
                        }
                    }
                }
            }
            else
            {
                var parts = GetDateParts(value, dateTimeFormat, datePartOrderInfo);

                int? year = null, month = null, day = null;

                for (int i = 0; i < parts.Count; i++)
                {
                    var part = parts[i];

                    var currentDatePart = DatePart.Year;

                    switch (i)
                    {
                        case 0:
                        {
                            currentDatePart = datePartOrderInfo.FirstPart.GetValueOrDefault();
                            break;
                        }
                        case 1:
                        {
                            currentDatePart = datePartOrderInfo.SecondPart.GetValueOrDefault();
                            break;
                        }
                        case 2:
                        {
                            currentDatePart = datePartOrderInfo.LastPart.GetValueOrDefault();
                            break;
                        }
                    }

                    switch (currentDatePart)
                    {
                        case DatePart.Year:
                        {
                            if (int.TryParse(part, out var number)) year = number;
                            break;
                        }
                        case DatePart.Month:
                        {
                            if (int.TryParse(part, out var number)) month = number;
                            break;
                        }
                        case DatePart.Day:
                        {
                            if (int.TryParse(part, out var number)) day = number;
                            break;
                        }
                    }
                }

                if (year == null) year = dateTimeFormat.Calendar.GetYear(referenceDate);
                if (month == null) month = dateTimeFormat.Calendar.GetMonth(referenceDate);
                if (day == null) day = dateTimeFormat.Calendar.GetDayOfMonth(referenceDate);

                successful = TryCreateDateTime(year.Value, month.Value, day.Value, dateTimeFormat, out result);

                if (!successful) result = referenceDate;
            }

            result = MergeDateAndTime(result, referenceDate, dateTimeFormat);

            return successful;
        }

        public static bool TryParseDayOfWeek(string value, DateTime referenceDate, DayOfWeekBehavior behavior, out DateTime result)
        {
            return TryParseDayOfWeek(value, referenceDate, behavior, DateTimeFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParseDayOfWeek(string value, DateTime referenceDate, DayOfWeekBehavior behavior, DateTimeFormatInfo dateTimeFormat, out DateTime result)
        {
            result = referenceDate;

            if (string.IsNullOrWhiteSpace(value)) return false;

            var shortDayNames = dateTimeFormat.AbbreviatedDayNames;
            var dayNames = dateTimeFormat.DayNames;

            var regexBuilder = new StringBuilder();

            for (int i = 0; i < dayNames.Length; i++)
            {
                var dayName = Regex.Escape(dayNames[i].TrimEnd('.'));
                var shortDayName = Regex.Escape(shortDayNames[i].TrimEnd('.'));

                regexBuilder.AppendFormat(@"|((?<=\W|\b){0}(?=\W|\b))|((?<=\W|\b){1}(?=\W|\b))", dayName, shortDayName);
            }

            regexBuilder.Remove(0, 1);

            var regex = new Regex(regexBuilder.ToString(), RegexOptions.IgnoreCase);

            var match = regex.Match(value);

            if (match.Success)
            {
                // Die erste Gruppe müssen wir überspringen weil die das Ergebnis durcheinander bringt
                var groups = match.Groups.Cast<Group>().Skip(1).ToList();
                var index = groups.IndexOf(groups.FirstOrDefault(x => x.Success)) / 2;

                var calendar = dateTimeFormat.Calendar;
                var dayOfWeek = calendar.GetDayOfWeek(result);

                switch (behavior)
                {
                    case DayOfWeekBehavior.SameWeek:
                    {
                        var difference = index - (int)calendar.GetDayOfWeek(result);
                        result = calendar.AddDays(result, difference);
                        return true;
                    }
                    case DayOfWeekBehavior.SameDayOrNextOccurence:
                    {
                        while (calendar.GetDayOfWeek(result) != (DayOfWeek)index)
                        {
                            result = calendar.AddDays(result, 1);
                        }
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool TryParseSpecialDay(string value, DateTime referenceDate, SpecialDayCollection specialDays, out DateTime result)
        {
            return TryParseSpecialDay(value, referenceDate, specialDays, DateTimeFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParseSpecialDay(string value, DateTime referenceDate, SpecialDayCollection specialDays, DateTimeFormatInfo dateTimeFormat, out DateTime result)
        {
            result = referenceDate;

            if (string.IsNullOrWhiteSpace(value)) return false;

            var regexBuilder = new StringBuilder();

            for (int i = 0; i < specialDays.Count; i++)
            {
                var dayName = Regex.Escape(specialDays[i].Name);

                regexBuilder.AppendFormat(@"|(?<{0}>(?<=\W|\b){0}(?=\W|\b))", dayName);
            }

            regexBuilder.Remove(0, 1);

            var regex = new Regex(regexBuilder.ToString(), RegexOptions.IgnoreCase);

            var match = regex.Match(value);

            if (match.Success)
            {
                // Die erste Gruppe müssen wir überspringen weil die das Ergebnis durcheinander bringt
                var groups = match.Groups.Cast<Group>().Skip(1).ToList();
                var index = groups.IndexOf(groups.FirstOrDefault(x => x.Success)) + 1;

                var name = regex.GroupNameFromNumber(index);

                var specialDay = specialDays.FirstOrDefault(x => x.Name == name);

                result = dateTimeFormat.Calendar.AddDays(result, specialDay.DayDifferenceFromToday);

                return true;
            }

            return false;
        }

        private static DatePartOrderInfo GetDatePartOrderInfo(DateTimeFormatInfo dateTimeFormat)
        {
            var orderInfo = new DatePartOrderInfo();

            var splitRegex = new Regex(@"(\w)\1*", RegexOptions.IgnoreCase);
            var matches = splitRegex.Matches(dateTimeFormat.ShortDatePattern);

            for (int i = 0; i < matches.Count; i++)
            {
                var matchValue = matches[i].Captures[0].Value;

                if (matchValue.Contains("d") || matchValue.Contains("D"))
                {
                    orderInfo.AddPart(DatePart.Day);
                }
                else if (matchValue.Contains("M"))
                {
                    orderInfo.AddPart(DatePart.Month);
                }
                else if (matchValue.Contains("y") || matchValue.Contains("Y"))
                {
                    orderInfo.AddPart(DatePart.Year);
                    orderInfo.YearLength = matchValue.Length;
                }
            }

            return orderInfo;
        }

        private static bool HasInputSeparators(string input)
        {
            // Über Regex nach einem "Nicht Wort" suchen, also irgendein Zeichen was der Separator sein sollte
            var regex = new Regex(@"\W", RegexOptions.IgnoreCase);

            return regex.IsMatch(input);
        }

        private static List<string> SplitDateString(string baseDateString, DateTimeFormatInfo dateTimeFormat)
        {
            var regex = new Regex(BuildSplitRegex(dateTimeFormat), RegexOptions.IgnoreCase);

            var dateParts = regex.Matches(baseDateString).OfType<Match>().Select(x => x.Value).Where(x => !string.IsNullOrEmpty(x)).ToList();

            return dateParts;
        }

        private static string BuildSplitRegex(DateTimeFormatInfo dateTimeFormat)
        {
            // Regex anfangen mit Ausdruck der alle Zahlen ausliest
            var regexBuilder = new StringBuilder("[0-9]+");

            // Die ausgeschriebenen Monate und die abgekürzten beide berücksichtigen
            var monthNames = dateTimeFormat.AbbreviatedMonthNames.Union(dateTimeFormat.MonthNames).ToList();

            for (int i = 0; i < monthNames.Count; i++)
            {
                var monthName = monthNames[i];

                // Leere Monate überspringen
                if (string.IsNullOrWhiteSpace(monthName)) continue;

                var escapedName = Regex.Escape(monthName);

                regexBuilder.AppendFormat(@"|(?<=\W|\b|[0-9_]){0}(?=\W|\b|[0-9_])", escapedName);
            }

            return regexBuilder.ToString();
        }

        private static int GetMonthByName(string monthString, DateTimeFormatInfo dateTimeFormat)
        {
            var monthNames = dateTimeFormat.MonthNames;
            var shortMonthNames = dateTimeFormat.AbbreviatedMonthNames;

            int i;

            for (i = 0; i < shortMonthNames.Length - 1; i++)
            {
                if (shortMonthNames[i].Equals(monthString, StringComparison.CurrentCultureIgnoreCase) || monthNames[i].Equals(monthString, StringComparison.CurrentCultureIgnoreCase)) break;
            }

            return i + 1;
        }

        private static List<string> GetDateParts(string dateString, DateTimeFormatInfo dateTimeFormat, DatePartOrderInfo datePartOrderInfo)
        {
            // Falls der Text einen Monat als Text enthält und nicht als Zahl spalten wir das in mehrere Teile auf
            var splitString = SplitDateString(dateString, dateTimeFormat);

            var parts = new List<string>();

            // Wenn kein Monat als Text enthalten ist, haben wir nach dem Teilen immer noch nur ein Teil
            if (splitString.Count == 1)
            {
                var maximumPartCount = Math.Max(1, (datePartOrderInfo.FirstPart != null ? 1 : 0) + (datePartOrderInfo.SecondPart != null ? 1 : 0) + (datePartOrderInfo.LastPart != null ? 1 : 0));
                var currentPartString = new StringBuilder();
                var currentPart = datePartOrderInfo.FirstPart ?? DatePart.Year;

                for (int i = 0; i < dateString.Length; i++)
                {
                    var currentChar = dateString[i];

                    if (parts.Count == maximumPartCount) break;

                    if (currentPartString.Length == (currentPart == DatePart.Year ? datePartOrderInfo.YearLength.GetValueOrDefault(2) : 2))
                    {
                        parts.Add(currentPartString.ToString());
                        currentPartString.Clear();

                        if (parts.Count < maximumPartCount)
                        {
                            currentPart = parts.Count == 1 ? datePartOrderInfo.SecondPart.GetValueOrDefault() : datePartOrderInfo.LastPart.GetValueOrDefault();
                        }
                    }

                    currentPartString.Append(currentChar);
                }

                if (parts.Count < maximumPartCount && currentPartString.Length > 0)
                {
                    parts.Add(currentPartString.ToString());
                    currentPartString.Clear();
                }
            }
            else if (splitString.Count > 1)
            {
                for (int i = 0; i < splitString.Count; i++)
                {
                    var part = splitString[i];

                    if (char.IsLetter(part[0]))
                    {
                        var monthPart = GetMonthByName(part, dateTimeFormat).ToString();

                        parts.Add(monthPart);
                    }
                    else parts.Add(part);
                }
            }

            return parts;
        }

        private static bool TryCreateDateFromDay(int day, DateTime referenceDate, DateTimeFormatInfo dateTimeFormat, out DateTime changedDate)
        {
            var calendar = dateTimeFormat.Calendar;

            var result = TryCreateDateTime(calendar.GetYear(referenceDate), calendar.GetMonth(referenceDate), day, dateTimeFormat, out changedDate);

            if (!result) changedDate = referenceDate;

            return result;
        }

        private static bool TryCreateDateFromMonth(int month, DateTime referenceDate, DateTimeFormatInfo dateTimeFormat, out DateTime changedDate)
        {
            var calendar = dateTimeFormat.Calendar;

            var result = TryCreateDateTime(calendar.GetYear(referenceDate), month, calendar.GetDayOfMonth(referenceDate), dateTimeFormat, out changedDate);

            if (!result) changedDate = referenceDate;

            return result;
        }

        private static bool TryCreateDateFromYear(int year, DateTime referenceDate, DateTimeFormatInfo dateTimeFormat, out DateTime changedDate)
        {
            var calendar = dateTimeFormat.Calendar;

            if (year < 100) year = (calendar.GetYear(referenceDate) / 100 * 100) + year;

            var result = TryCreateDateTime(year, calendar.GetMonth(referenceDate), calendar.GetDayOfMonth(referenceDate), dateTimeFormat, out changedDate);

            if (!result) changedDate = referenceDate;

            return result;
        }

        private static bool TryCreateDateTime(int year, int month, int day, DateTimeFormatInfo dateTimeFormat, out DateTime result)
        {
            var dateString = $"{year}/{month}/{day}";

            var formatClone = (DateTimeFormatInfo)dateTimeFormat.Clone();

            formatClone.ShortDatePattern = "yyyy/MM/dd";

            return DateTime.TryParse(dateString, formatClone, DateTimeStyles.None, out result);
        }

        private static DateTime MergeDateAndTime(DateTime date, DateTime time, DateTimeFormatInfo dateTimeFormat)
        {
            var calendar = dateTimeFormat.Calendar;

            return new DateTime(calendar.GetYear(date), calendar.GetMonth(date), calendar.GetDayOfMonth(date),
                                calendar.GetHour(time), calendar.GetMinute(time), calendar.GetSecond(time), (int)calendar.GetMilliseconds(time),
                                calendar);
        }

        private enum DatePart
        {
            Year,
            Month,
            Day
        }

        private class DatePartOrderInfo
        {
            public DatePart? FirstPart { get; set; }

            public DatePart? SecondPart { get; set; }

            public DatePart? LastPart { get; set; }

            public void AddPart(DatePart part)
            {
                if (FirstPart == null) FirstPart = part;
                else if (SecondPart == null) SecondPart = part;
                else if (LastPart == null) LastPart = part;
            }

            public int? YearLength { get; set; }
        }
    }
}