namespace Mastermind.ComputerPlayer.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Mastermind.GameLogic;
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
        [InlineData(8, 4, 4096)]
        public void GenerateAllLines(int numberOfPegs, int numberOfPegsPerLine, int expectedNumberOfLines)
        {
            var lines = LineGenerator.GenerateAllDifferentLines(numberOfPegs, numberOfPegsPerLine);
            Assert.Equal(expectedNumberOfLines, lines.Count);
            Assert.Equal(expectedNumberOfLines, lines.Distinct(new LinePegEqualityComparer()).Count());
        }

        private class LinePegEqualityComparer : IEqualityComparer<Line>
        {
            public bool Equals([AllowNull] Line x, [AllowNull] Line y)
            {
                return (x is null || y is null)
                ? x is null && y is null
                : Enumerable.SequenceEqual(x.Pegs.Select(p => p.Number), y.Pegs.Select(p => p.Number));
            }

            public int GetHashCode([DisallowNull] Line obj)
            {
                if (obj is null)
                {
                    throw new ArgumentNullException(nameof(obj));
                }
                unchecked
                {
                    var hashCode = 1320594601;
                    foreach (var peg in obj.Pegs)
                    {
                        hashCode = hashCode * -1521134295 + peg.Number.GetHashCode();
                    }
                    return hashCode;
                }
            }
        }
    }
}