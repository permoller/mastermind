namespace Mastermind.ComputerPlayer.Tests
{
    using Xunit;
    public class LinePegEqualityComparerShould
    {
        [Theory]
        [InlineData("1", "1", true)]
        [InlineData("1", "2", false)]
        [InlineData("1 2", "1 2", true)]
        [InlineData("1 2", "1 1", false)]
        public void CompareLinesCorrect(string lineAsString1, string lineAsString2, bool expectedToBeEqual)
        {
            var line1 = LineParser.ParseLine(lineAsString1);
            var line2 = LineParser.ParseLine(lineAsString2);
            var comparer = new LinePegEqualityComparer();
            Assert.Equal(expectedToBeEqual, comparer.Equals(line1, line2));
            if (expectedToBeEqual)
            {
                Assert.Equal(comparer.GetHashCode(line1), comparer.GetHashCode(line2));
            }
        }

    }
}