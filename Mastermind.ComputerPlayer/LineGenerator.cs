namespace Mastermind.ComputerPlayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mastermind.GameLogic;

    public static class LineGenerator
    {
        public static IList<Line> GenerateAllDifferentLines(int numberOfPegs, int numberOfPegsPerLine)
        {
            return Enumerable.Range(0, (int)Math.Pow(numberOfPegs, numberOfPegsPerLine))
                    .Select(index => new Line(GetPegsFromLineIndex(index, numberOfPegsPerLine, numberOfPegs)))
                    .ToList();
        }

        private static Peg[] GetPegsFromLineIndex(int index, int numberOfPegsPerLine, int numberOfPegs)
        {
            var pegsForLine = new Peg[numberOfPegsPerLine];
            for (var pegIndex = 0; pegIndex < numberOfPegsPerLine; pegIndex++)
            {
                pegsForLine[pegIndex] = index % numberOfPegs;
                index = index / numberOfPegs;
            }
            return pegsForLine;
        }
    }
}