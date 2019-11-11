namespace Mastermind.GameLogic
{
    using System;

    public class LineComparer
    {
        public Result Compare(Line guess, Line secret)
        {
            if (guess.Pegs.Count != secret.Pegs.Count)
                throw new ArgumentException($"Peg count of {nameof(guess)} ({guess.Pegs.Count}) is different from {nameof(secret)} ({secret.Pegs.Count})");

            var numberOfPegsPerLine = guess.Pegs.Count;

            if (numberOfPegsPerLine <= 0)
                throw new ArgumentException("There must be at least one peg in the lines.");

            int correct = 0;
            int wrongPosition = 0;

            var secretPegs = secret.Pegs;
            var guessPegs = guess.Pegs;

            var secretPegsUsed = new bool[numberOfPegsPerLine];
            var guessPegsUsed = new bool[numberOfPegsPerLine];

            // find pegs with the correct color at the correct position
            for (var i = 0; i < numberOfPegsPerLine; i++)
            {
                if (secretPegs[i].Number == guessPegs[i].Number)
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
                var guessPeg = guessPegs[guessIndex];
                for (var secretIndex = 0; secretIndex < numberOfPegsPerLine; secretIndex++)
                {
                    if (secretPegsUsed[secretIndex])
                        continue;
                    var secretPeg = secretPegs[secretIndex];
                    if (guessPeg.Number == secretPeg.Number)
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