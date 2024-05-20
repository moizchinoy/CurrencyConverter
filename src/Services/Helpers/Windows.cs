namespace Services.Helpers
{
    public class Windows
    {
        public static IEnumerable<(DateOnly FromDate, DateOnly ToDate)> GetWindows(DateOnly fromDate, DateOnly toDate)
        {
            var windows = new List<(DateOnly, DateOnly)>();

            var firstWindowStartDate = GetPreviousMultiple(fromDate);
            var lastWindowEndDate = GetNextMultiple(toDate);

            while (firstWindowStartDate < lastWindowEndDate)
            {
                DateOnly windowToDate = firstWindowStartDate.Day == 26 ?
                    GetLastDayofTheMonth(firstWindowStartDate) : firstWindowStartDate.AddDays(5 - 1);
                windows.Add((firstWindowStartDate, windowToDate));
                firstWindowStartDate = windowToDate.AddDays(1);
            }

            return windows;
        }

        private static DateOnly GetLastDayofTheMonth(DateOnly date) =>
            new(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

        private static DateOnly GetPreviousMultiple(DateOnly date)
        {
            if (date.Day <= 5)
            {
                return DateOnly.Parse($"{date.Year}-{date.Month}-01");
            }
            else if (date.Day <= 10)
            {
                return DateOnly.Parse($"{date.Year}-{date.Month}-06");
            }
            else if (date.Day <= 15)
            {
                return DateOnly.Parse($"{date.Year}-{date.Month}-11");
            }
            else if (date.Day <= 20)
            {
                return DateOnly.Parse($"{date.Year}-{date.Month}-16");
            }
            else if (date.Day <= 25)
            {
                return DateOnly.Parse($"{date.Year}-{date.Month}-21");
            }
            else
            {
                return DateOnly.Parse($"{date.Year}-{date.Month}-26");
            }
        }

        private static DateOnly GetNextMultiple(DateOnly date)
        {
            if (date.Day <= 5)
            {
                return DateOnly.Parse($"{date.Year}-{date.Month}-05");
            }
            else if (date.Day <= 10)
            {
                return DateOnly.Parse($"{date.Year}-{date.Month}-10");
            }
            else if (date.Day <= 15)
            {
                return DateOnly.Parse($"{date.Year}-{date.Month}-15");
            }
            else if (date.Day <= 20)
            {
                return DateOnly.Parse($"{date.Year}-{date.Month}-20");
            }
            else if (date.Day <= 25)
            {
                return DateOnly.Parse($"{date.Year}-{date.Month}-25");
            }
            else
            {
                return GetLastDayofTheMonth(date);
            }
        }
    }
}
