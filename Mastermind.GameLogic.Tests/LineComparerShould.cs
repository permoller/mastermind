namespace Mastermind.GameLogic.Tests
{
    using Xunit;

    public class LineComparerShould
    {
        [Theory]
        // test all combinations when two pegs are available
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
        // additional case to test that we can get one correct colored peg i the wrong position
        [InlineData(0, 1, 1, 2, 0, 1)]
        public void GiveTheExpectedResult(int guessPeg1, int guessPeg2, int secretPeg1, int secretPeg2, int expectedCorrect, int expectedWrongPosition)
        {
            var lineComparer = new LineComparer();
            var guess = new int[] { guessPeg1, guessPeg2 };
            var secret = new int[] { secretPeg1, secretPeg2 };
            var actualResult = lineComparer.Compare(guess, secret);
            Assert.Equal(expectedCorrect, actualResult.NumberOfCorrectPegs);
            Assert.Equal(expectedWrongPosition, actualResult.NumberOfPegsAtWrongPosition);
        }
    }
}