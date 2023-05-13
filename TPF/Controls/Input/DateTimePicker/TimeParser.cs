using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

namespace TPF.Controls
{
    public static class TimeParser
    {
        public static bool TryParse(string value, DateTime referenceDate, out DateTime result)
        {
            return TryParse(value, referenceDate, DateTimeFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParse(string value, DateTime referenceDate, DateTimeFormatInfo dateTimeFormat, out DateTime result)
        {
            result = referenceDate;

            if (string.IsNullOrWhiteSpace(value)) return false;

            var timeString = value;

            var trailingSymbols = GetTrailingNonDigitSymbols(value);

            if (!string.IsNullOrWhiteSpace(trailingSymbols))
            {
                timeString = value.Substring(0, value.Length - trailingSymbols.Length);
            }

            if (TryParseTime(timeString, referenceDate, dateTimeFormat, out result))
            {
                var calendar = dateTimeFormat.Calendar;

                if (CheckForDesignator(trailingSymbols, dateTimeFormat.AMDesignator))
                {
                    if (calendar.GetHour(result) >= 12)
                    {
                        result = calendar.AddHours(result, -12);
                    }
                }
                else if (CheckForDesignator(trailingSymbols, dateTimeFormat.PMDesignator))
                {
                    if (calendar.GetHour(result) < 12)
                    {
                        result = calendar.AddHours(result, 12);
                    }
                }
                return true;
            }
            return false;
        }

        private static bool TryParseTime(string value, DateTime referenceDate, DateTimeFormatInfo dateTimeFormat, out DateTime result)
        {
            result = referenceDate;

            if (string.IsNullOrWhiteSpace(value)) return false;

            var calendar = dateTimeFormat.Calendar;

            var parts = GetTimeParts(value);

            var isSuccessful = false;

            if (parts.Count > 0)
            {
                var hourString = parts[0];
                var minutesString = parts.Count > 1 ? parts[1] : "0";
                var secondsString = parts.Count > 2 ? parts[2] : "0";

                if (int.TryParse(hourString, out var hour) && int.TryParse(minutesString, out var minutes) && int.TryParse(secondsString, out var seconds))
                {
                    if (hour >= 0 && hour < 24 && minutes >= 0 && minutes < 60 && seconds >= 0 && seconds < 60)
                    {
                        result = new DateTime(calendar.GetYear(referenceDate), calendar.GetMonth(referenceDate), calendar.GetDayOfMonth(referenceDate), hour, minutes, seconds, calendar);
                        isSuccessful = true;
                    }
                }
            }

            return isSuccessful;
        }

        private static string GetTrailingNonDigitSymbols(string input)
        {
            var result = new StringBuilder();

            for (int i = input.Length - 1; i > -1; i--)
            {
                var item = input[i];

                // Sobald wir eine Zahl oder einen Doppelpunkt erreichen sind wir bei der eigentlichen Zeit angekommen
                if (char.IsDigit(item) || item == ':') break;

                result.Insert(0, item);
            }

            return result.ToString();
        }

        private static bool CheckForDesignator(string value, string designator)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            // Beide Strings trimmen und Großschreibung erzwingen für bessere Erfolgschancen
            value = value.Trim().ToUpper();
            designator = designator.Trim().ToUpper();

            var endsWithDesignator = value == designator;

            return endsWithDesignator;
        }

        private static List<string> GetTimeParts(string input)
        {
            var parts = new List<string>();

            var currentPart = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                var currentChar = input[i];

                // Maximal 3 Teile
                if (parts.Count == 3) break;

                // Ist es entweder ein Doppelpunkt oder hat der aktuelle part schon 2 Stellen?
                if (currentChar == ':' || currentPart.Length == 2)
                {
                    if (currentPart.Length == 0) currentPart.Append("0");

                    parts.Add(currentPart.ToString());
                    currentPart.Clear();
                }

                if (char.IsDigit(currentChar))
                {
                    currentPart.Append(currentChar);
                }
            }

            if (parts.Count < 3 && currentPart.Length > 0)
            {
                parts.Add(currentPart.ToString());
                currentPart.Clear();
            }

            return parts;
        }
    }
}