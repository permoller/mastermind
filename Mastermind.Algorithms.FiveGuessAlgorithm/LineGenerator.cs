namespace Mastermind.Algorithms.FiveGuessAlgorithm
{
    using System.Collections.Generic;
    using System.Linq;

    internal static class LineGenerator
    {
        internal static IReadOnlyList<int[]> GenerateAllLines(int numberOfDifferentPegs, int numberOfPegsPerLine)
        {
            return GenerateAllLines(numberOfDifferentPegs, numberOfPegsPerLine, new int[0]).ToList();
        }
        private static IEnumerable<int[]> GenerateAllLines(int numberOfDifferentPegs, int remainingNumberOfPegsInLine, IEnumerable<int> pegs)
        {
            if (remainingNumberOfPegsInLine == 0)
            {
                yield return pegs.ToArray();
            }
            else
            {
                for (int peg = 0; peg < numberOfDifferentPegs; peg++)
                {
                    var lines = GenerateAllLines(numberOfDifferentPegs, remainingNumberOfPegsInLine - 1, pegs.Append(peg));
                    foreach (var line in lines)
                    {
                        yield return line;
                    }
                }
            }
        }
    }
}