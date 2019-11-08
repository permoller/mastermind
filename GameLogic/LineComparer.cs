namespace Mastermind.GameLogic
{
    using System;

    public class LineComparer : ILineComparer
    {
        public Result Compare(Line guess, Line secret)
        {
            if (guess.Pins.Count != secret.Pins.Count)
                throw new ArgumentException($"Pin count of {nameof(guess)} ({guess.Pins.Count}) is different from {nameof(secret)} ({secret.Pins.Count})");

            var numberOfPinsPerLine = guess.Pins.Count;

            if (numberOfPinsPerLine <= 0)
                throw new ArgumentException("There must be at least one pin in the lines.");

            int correct = 0;
            int wrongPosition = 0;

            var secretPins = secret.Pins;
            var guessPins = guess.Pins;

            var secretPinsUsed = new bool[numberOfPinsPerLine];
            var guessPinsUsed = new bool[numberOfPinsPerLine];

            // find pins with the correct color at the correct position
            for (var i = 0; i < numberOfPinsPerLine; i++)
            {
                if (secretPins[i].Number == guessPins[i].Number)
                {
                    secretPinsUsed[i] = true;
                    guessPinsUsed[i] = true;
                    correct++;
                }
            }

            // find pins with correct color at the wrong position
            for (var guessIndex = 0; guessIndex < numberOfPinsPerLine; guessIndex++)
            {
                if (guessPinsUsed[guessIndex])
                    continue;
                var guessPin = guessPins[guessIndex];
                for (var secretIndex = 0; secretIndex < numberOfPinsPerLine; secretIndex++)
                {
                    if (secretPinsUsed[secretIndex])
                        continue;
                    var secretPin = secretPins[secretIndex];
                    if (guessPin.Number == secretPin.Number)
                    {
                        secretPinsUsed[secretIndex] = true;
                        guessPinsUsed[guessIndex] = true;
                        wrongPosition++;
                        break;
                    }
                }
            }
            return new Result(correct, wrongPosition);
        }
    }
}