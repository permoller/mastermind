namespace Mastermind.Algorithms.FiveGuessAlgorithmWithCache
{
    using Mastermind.GameLogic;
    using System;

    internal class LineComparerWithCaching
    {

        LineComparer _LineComparer = new LineComparer();
        Result[,] _Cache;

        public LineComparerWithCaching(int numberOfDifferentPegs, int numberOfPegsPerLine)
        {
            var numberOfDifferentLines = (int)Math.Pow(numberOfDifferentPegs, numberOfPegsPerLine);
            _Cache = new Result[numberOfDifferentLines, numberOfDifferentLines];
        }

        public Result Compare(int numberOfDifferentPegs, int[] guess, int[] secret)
        {
            var guessIndex = GetLineIndex(numberOfDifferentPegs, guess);
            var secretIndex = GetLineIndex(numberOfDifferentPegs, secret);
            var result = _Cache[guessIndex, secretIndex];

            if (result == null)
            {
                result = _LineComparer.Compare(guess, secret);
                _Cache[guessIndex, secretIndex] = result;
                _Cache[secretIndex, guessIndex] = result;
            }

            return result;
        }
        private int GetLineIndex(int numberOfDifferentPegs, int[] line)
        {
            var index = 0;
            for (var i = 0; i < line.Length; i++)
            {
                index = index * numberOfDifferentPegs + line[i];
            }
            return index;

        }
    }
}