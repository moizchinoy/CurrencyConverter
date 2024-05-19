using Services;

namespace ServicesTests
{
    public class PagerTests
    {
        [Theory]
        [InlineData("2020-01-01", "2020-01-01", 1, 1, null, null)]
        [InlineData("2020-01-01", "2020-01-02", 1, 1, null, 2)]
        [InlineData("2020-01-01", "2020-01-02", 2, 1, 1, null)]
        public void Verify_Previous_And_Next_Page(
            string strFromDate, string strToDate, int page, int size,
            int? expectedPreviousPage, int? expectedNextPage)
        {
            // Arrange
            var fromDate = DateOnly.Parse(strFromDate);
            var toDate = DateOnly.Parse(strToDate);

            // Act
            var result = Pager.GetPageDetails(fromDate, toDate, page, size);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPreviousPage, result.PreviousPage);
            Assert.Equal(expectedNextPage, result.NextPage);
        }
    }
}
