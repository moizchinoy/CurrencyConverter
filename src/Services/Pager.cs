namespace Services
{
    public class PageDetails
    {
        public DateOnly PageFromDate { get; set; }
        public DateOnly PageToDate { get; set; }
        public int? PreviousPage { get; set; }
        public int? NextPage { get; set; }
    }

    public class Pager
    {
        public static PageDetails GetPageDetails(DateOnly fromDate, DateOnly toDate, int page, int size)
        {
            if (fromDate > toDate)
            {
                throw new ArgumentException("From date can not be greater than To date");
            }

            if (page < 1)
            {
                throw new ArgumentException("page can not be less than 1");
            }

            if (size < 1)
            {
                throw new ArgumentException("size can not be less than 1");
            }

            var dates = new List<DateOnly>();
            var startDate = fromDate;
            while (startDate <= toDate)
            {
                dates.Add(startDate);
                startDate = startDate.AddDays(1);
            }

            var pageFromIndex = (page - 1) * size;
            if (pageFromIndex >= dates.Count)
            {
                throw new Exception("Now more data");
            }
            var pageToIndex = Math.Min(page * size, dates.Count - 1);

            var pageFromDate = dates[pageFromIndex];
            var pageToDate = dates[pageToIndex];

            return new PageDetails
            {
                PageFromDate = pageFromDate,
                PageToDate = pageToDate,
                PreviousPage = page - 1 < 1 ? null : page - 1,
                NextPage = page * size >= dates.Count ? null : page + 1
            };
        }
    }
}
