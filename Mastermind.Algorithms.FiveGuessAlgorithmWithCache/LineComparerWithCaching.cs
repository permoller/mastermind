namespace Mastermind.Algorithms.FiveGuessAlgorithmWithCache
{
    using System;

    internal class LineComparerWithCaching
    {
        private readonly int?[,] _Cache;
        private readonly int _NumberOfDifferentPegs;
        private readonly int _NumberOfPegsPerLine;
        public int NumberOfDifferentLines { get; }

        public LineComparerWithCaching(int numberOfDifferentPegs, int numberOfPegsPerLine)
        {
            NumberOfDifferentLines = (int)Math.Pow(numberOfDifferentPegs, numberOfPegsPerLine);
            _Cache = new int?[NumberOfDifferentLines, NumberOfDifferentLines];
            _NumberOfDifferentPegs = numberOfDifferentPegs;
            _NumberOfPegsPerLine = numberOfPegsPerLine;
            NumberOfDifferentResults = (_NumberOfPegsPerLine * _NumberOfPegsPerLine + _NumberOfPegsPerLine) + 1;
        }

        public int Compare(int guessIndex, int secretIndex)
        {
            if (guessIndex > NumberOfDifferentLines)
                throw new ArgumentOutOfRangeException(nameof(guessIndex), $"{guessIndex} > {NumberOfDifferentLines}");
            if (secretIndex > NumberOfDifferentLines)
                throw new ArgumentOutOfRangeException(nameof(secretIndex), $"{secretIndex} > {NumberOfDifferentLines}");
            var result = _Cache[guessIndex, secretIndex];

            if (result == null)
            {
                result = RealCompare(GetLine(guessIndex), GetLine(secretIndex));
                _Cache[guessIndex, secretIndex] = result;
                _Cache[secretIndex, guessIndex] = result;
            }
            return result.Value;
        }

        public int[] GetLine(int lineIndex)
        {
            var line = new int[_NumberOfPegsPerLine];
            for (var pegIndex = _NumberOfPegsPerLine - 1; pegIndex >= 0; pegIndex--)
            {
                line[pegIndex] = lineIndex % _NumberOfDifferentPegs;
                lineIndex = lineIndex / _NumberOfDifferentPegs;
            }

            return line;
        }

        public int GetLineIndex(int[] line)
        {
            if (line.Length != _NumberOfPegsPerLine)
            {
                throw new ArgumentException(nameof(line), $"{line.Length} != {_NumberOfPegsPerLine}");
            }
            var lineIndex = 0;
            for (var pegIndex = 0; pegIndex < _NumberOfPegsPerLine; pegIndex++)
            {
                lineIndex = lineIndex * _NumberOfDifferentPegs + line[pegIndex];
            }
            return lineIndex;
        }

        private int RealCompare(int[] guess, int[] secret)
        {
            if (guess.Length != secret.Length)
                throw new ArgumentException($"Peg count of {nameof(guess)} ({guess.Length}) is different from {nameof(secret)} ({secret.Length})");

            var numberOfPegsPerLine = guess.Length;

            if (numberOfPegsPerLine <= 0)
                throw new ArgumentException("There must be at least one peg in the lines.");

            int correct = 0;
            int wrongPosition = 0;

            var secretPegsUsed = new bool[numberOfPegsPerLine];
            var guessPegsUsed = new bool[numberOfPegsPerLine];

            // find pegs with the correct color at the correct position
            for (var i = 0; i < numberOfPegsPerLine; i++)
            {
                if (secret[i] == guess[i])
                {
                    secretPegsUsed[i] = true;
                    guessPegsUsed[i] = true;
                    correct++;
                }
            }

            // find pegs with correct color at the wrong position
            for (var guessIndex = 0; guessIndex < numberOfPegsPerLine; guessIndex++)
            {
                if (guessPegsUsed[guessIndex])
                    continue;
                var guessPeg = guess[guessIndex];
                for (var secretIndex = 0; secretIndex < numberOfPegsPerLine; secretIndex++)
                {
                    if (secretPegsUsed[secretIndex])
                        continue;
                    var secretPeg = secret[secretIndex];
                    if (guessPeg == secretPeg)
                    {
                        secretPegsUsed[secretIndex] = true;
                        guessPegsUsed[guessIndex] = true;
                        wrongPosition++;
                        break;
                    }
                }
            }
            return GetResult(correct, wrongPosition);
        }

        public int NumberOfDifferentResults { get; }

        public int GetResult(int numberOfCorrectPegs, int numberOfPegsAtWrongPosition)
        {
            return numberOfCorrectPegs * (_NumberOfPegsPerLine + 1) + numberOfPegsAtWrongPosition;
        }

        public int GetNumberOfCorrectPegs(int result)
        {
            return result / (_NumberOfPegsPerLine + 1);
        }
        public int GetNumberOfPegsAtWrongPosition(int result)
        {
            return result % (_NumberOfPegsPerLine + 1);
        }
    }
}