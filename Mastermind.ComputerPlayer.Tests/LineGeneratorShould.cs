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
        public void GenerateAllLines(int numberOfPins, int numberOfPinsPerLine, int expectedNumberOfLines)
        {
            var lines = LineGenerator.GenerateAllDifferentLines(numberOfPins, numberOfPinsPerLine);
            Assert.Equal(expectedNumberOfLines, lines.Count);
            Assert.Equal(expectedNumberOfLines, lines.Distinct(new LinePinEqualityComparer()).Count());
        }

        private class LinePinEqualityComparer : IEqualityComparer<Line>
        {
            public bool Equals([AllowNull] Line x, [AllowNull] Line y)
            {
                return (x is null || y is null)
                ? x is null && y is null
                : Enumerable.SequenceEqual(x.Pins.Select(p => p.Number), y.Pins.Select(p => p.Number));
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
                    foreach (var pin in obj.Pins)
                    {
                        hashCode = hashCode * -1521134295 + pin.Number.GetHashCode();
                    }
                    return hashCode;
                }
            }
        }
    }
}