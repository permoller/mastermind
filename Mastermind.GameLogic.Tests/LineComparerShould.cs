using Xunit;

namespace Mastermind.GameLogic.Tests
{
    public class LineComparerShould
    {
        [Theory]
        // test all combinations when two pins are available
        [InlineData(0, 0, 0, 0, 2, 0)]
        [InlineData(0, 0, 0, 1, 1, 0)]
        [InlineData(0, 0, 1, 0, 1, 0)]
        [InlineData(0, 0, 1, 1, 0, 0)]

        [InlineData(0, 1, 0, 0, 1, 0)]
        [InlineData(0, 1, 0, 1, 2, 0)]
        [InlineData(0, 1, 1, 0, 0, 2)]
        [InlineData(0, 1, 1, 1, 1, 0)]

        [InlineData(1, 0, 0, 0, 1, 0)]
        [InlineData(1, 0, 0, 1, 0, 2)]
        [InlineData(1, 0, 1, 0, 2, 0)]
        [InlineData(1, 0, 1, 1, 1, 0)]

        [InlineData(1, 1, 0, 0, 0, 0)]
        [InlineData(1, 1, 0, 1, 1, 0)]
        [InlineData(1, 1, 1, 0, 1, 0)]
        [InlineData(1, 1, 1, 1, 2, 0)]
        // additional case to test that we can get one correct colored pin i the wrong position
        [InlineData(0, 1, 1, 2, 0, 1)]
        public void GiveTheExpectedResult(int guessPin1, int guessPin2, int secretPin1, int secretPin2, int expectedCorrect, int expectedWrongPosition)
        {
            var lineComparer = new LineComparer();
            var guess = new Line(guessPin1, guessPin2);
            var secret = new Line(secretPin1, secretPin2);
            var actualResult = lineComparer.Compare(guess, secret);
            Assert.Equal(expectedCorrect, actualResult.NumberOfCorrectPins);
            Assert.Equal(expectedWrongPosition, actualResult.NumberOfCorrectColoredPinsInWrongPosition);
        }
    }
}