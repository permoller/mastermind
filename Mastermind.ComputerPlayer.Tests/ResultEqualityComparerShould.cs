namespace Mastermind.ComputerPlayer.Tests
{
    using Xunit;
    using Mastermind.GameLogic;
    using System;

    public class ResultEqualityComparerShould
    {
        [Theory]
        [InlineData(0, 0, 0, 0, true)]
        [InlineData(0, 0, 0, 1, false)]
        [InlineData(0, 0, 1, 0, false)]
        [InlineData(0, 0, 1, 1, false)]

        [InlineData(0, 1, 0, 0, false)]
        [InlineData(0, 1, 0, 1, true)]
        [InlineData(0, 1, 1, 0, false)]
        [InlineData(0, 1, 1, 1, false)]

        [InlineData(1, 0, 0, 0, false)]
        [InlineData(1, 0, 0, 1, false)]
        [InlineData(1, 0, 1, 0, true)]
        [InlineData(1, 0, 1, 1, false)]

        [InlineData(1, 1, 0, 0, false)]
        [InlineData(1, 1, 0, 1, false)]
        [InlineData(1, 1, 1, 0, false)]
        [InlineData(1, 1, 1, 1, true)]
        public void CompareResults(int correctX, int wrongPositionX, int correctY, int wrongPositionY, bool expectedToBeEqual)
        {
            var x = new Result(correctX, wrongPositionX);
            var y = new Result(correctY, wrongPositionY);
            var comparer = new ResultEqualityComparer();
            if (expectedToBeEqual)
            {
                Assert.True(comparer.Equals(x, y));
                Assert.Equal(comparer.GetHashCode(x), comparer.GetHashCode(y));
            }
            else
            {
                Assert.False(comparer.Equals(x, y));
            }
        }

        [Fact]
        public void HandleNull()
        {
            var comparer = new ResultEqualityComparer();
            Assert.True(comparer.Equals(null, null), "null equals null");
            Assert.False(comparer.Equals(null, new Result(0, 0)), "null equals result");
            Assert.False(comparer.Equals(new Result(0, 0), null), "result equals null");
            Assert.Throws<ArgumentNullException>(() => comparer.GetHashCode(null));
        }
    }
}
