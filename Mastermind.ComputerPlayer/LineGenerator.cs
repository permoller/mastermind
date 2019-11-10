namespace Mastermind.ComputerPlayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mastermind.GameLogic;

    public static class LineGenerator
    {
        public static IList<Line> GenerateAllDifferentLines(int numberOfPins, int numberOfPinsPerLine)
        {
            return Enumerable.Range(0, (int)Math.Pow(numberOfPins, numberOfPinsPerLine))
                    .Select(index => new Line(GetPinsFromLineIndex(index, numberOfPinsPerLine, numberOfPins)))
                    .ToList();
        }

        private static Pin[] GetPinsFromLineIndex(int index, int numberOfPinsPerLine, int numberOfPins)
        {
            var pinsForLine = new Pin[numberOfPinsPerLine];
            for (var pinIndex = 0; pinIndex < numberOfPinsPerLine; pinIndex++)
            {
                pinsForLine[pinIndex] = index % numberOfPins;
                index = index / numberOfPins;
            }
            return pinsForLine;
        }
    }
}