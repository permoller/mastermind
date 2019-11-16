namespace Mastermind.GameLogic
{
    using System;

    public class LineComparer
    {
        public Result Compare(int[] guess, int[] secret)
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
            return new Result(correct, wrongPosition);
        }
    }
}