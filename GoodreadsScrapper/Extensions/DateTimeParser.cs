using System.Globalization;
using System.Text.RegularExpressions;

namespace GoodreadsScrapper.Extensions
{
    internal static class DateTimeParser
    {
        public static DateTime? ParseDate(this string s)
        {
            int? day = null;
            int? month = null;
            int? year = null;

            s = s.Trim();

            bool yearAtStart = false;
            Match yearMatch = new Regex("\\d{4}").Match(s);
            if (yearMatch.Success)
            {
                if (yearMatch.Index == 0) { yearAtStart = true; }
                year = int.Parse(yearMatch.Value);
                s = s.Remove(yearMatch.Index, 4);
            }

            for (int i = 1; i <= 12; i++)
            {
                if (month == null && RemoveMonth(ref s, i)) { month = i; }
            }

            s = $" {new Regex("[^\\d]").Replace(s, "  ")} ";
            MatchCollection numMatch = new Regex("[^\\d](\\d{1,2})[^\\d]").Matches(s);
            int?[] numbers = {
                numMatch.Count > 0 ? int.Parse(numMatch[0].Groups[1].Value) : null,
                numMatch.Count > 1 ? int.Parse(numMatch[1].Groups[1].Value) : null,
                numMatch.Count > 2 ? int.Parse(numMatch[2].Groups[1].Value) : null
            };

            if (month != null && year != null && numbers[0].MaybeDay())
            {
                day = int.Parse(numMatch[0].Groups[1].Value);
            }

            if (day == null && month == null && year != null && numMatch.Count > 1)
            {
                if (yearAtStart && numbers[0].MaybeMonth() && numbers[1].MaybeDay())
                {
                    day = numbers[1];
                    month = numbers[0];
                }
                else if (numbers[0].MaybeDay() && numbers[1].MaybeMonth())
                {
                    day = numbers[0];
                    month = numbers[1];
                }
                else if (numbers[0].MaybeMonth() && numbers[1].MaybeDay())
                {
                    day = numbers[1];
                    month = numbers[0];
                }
            }

            if (day == null && month == null && year == null && numMatch.Count > 2)
            {
                for (int i = 0; i < numbers.Count(); i++)
                {
                    if (month == null && numbers[i].MaybeMonth())
                    {
                        month = numbers[i];
                        numbers[i] = null;
                    }
                    else if (day == null && numbers[i].MaybeDay())
                    {
                        day = numbers[i];
                        numbers[i] = null;
                    }
                    else
                    {
                        year = numbers[i];

                        int yearNow = int.Parse(DateTime.Now.ToString("yy"));
                        year = year + (year <= yearNow ? 2000 : 1900);
                    }
                }
            }

            if (day == null || month == null || year == null)
            {
                return null;
            }

            return new DateTime((int)year, (int)month, (int)day);
        }

        private static bool RemoveMonth(ref string s, int month)
        {
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            for (int i = 0; i < 2; i++)
            {
                int index = s.IndexOf(monthName, StringComparison.CurrentCultureIgnoreCase);
                if (index >= 0)
                {
                    s = s.Remove(index, monthName.Length);
                    return true;
                }
                monthName = monthName[..3];
            }
            return false;
        }

        private static bool MaybeDay(this int? num)
        {
            return num != null && num <= 31;
        }

        private static bool MaybeMonth(this int? num)
        {
            return num != null && num <= 12;
        }
    }
}
