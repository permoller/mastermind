namespace Mastermind.ComputerPlayer.Tests
{
    using System.Linq;
    using Xunit;

    public class LineGeneratorShould
    {
        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 1, 2)]
        [InlineData(1, 2, 1)]
        [InlineData(2, 2, 4)]
        [InlineData(2, 3, 8)]
        [InlineData(3, 2, 9)]
        [InlineData(3, 3, 27)]
        [InlineData(6, 4, 1296)]
        [InlineData(8, 4, 4096)]
        public void GenerateAllLines(int numberOfPegs, int numberOfPegsPerLine, int expectedNumberOfLines)
        {
            var lines = LineGenerator.GenerateAllDifferentLines(numberOfPegs, numberOfPegsPerLine);
            Assert.Equal(expectedNumberOfLines, lines.Count);
            Assert.Equal(expectedNumberOfLines, lines.Distinct(new LinePegEqualityComparer()).Count());
        }
    }
}