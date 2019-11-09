namespace Mastermind.GameLogic
{
    using System;
    using System.Collections.Generic;

    public class Game : IGame
    {
        private readonly Line _SecretLine;

        private readonly ILineComparer _LineComparer;

        private readonly List<GuessAndResult> _GuessesAndResults = new List<GuessAndResult>();
        private static Line RandomLine(int numberOfPins, int numberOfPinsPerLine)
        {
            var random = new Random();
            var pins = new Pin[numberOfPinsPerLine];
            for (var i = 0; i < numberOfPinsPerLine; i++)
            {
                pins[i] = random.Next(0, numberOfPins - 1);
            }
            return new Line(pins);
        }

        public Game() : this(6, 4)
        {
        }

        public Game(int numberOfPins, int numberOfPinsPerLine) : this(numberOfPins, numberOfPinsPerLine, RandomLine(numberOfPins, numberOfPinsPerLine))
        {
        }

        public Game(int numberOfPins, int numberOfPinsPerLine, Line secret)
        {
            NumberOfPins = numberOfPins;
            NumberOfPinsPerLine = numberOfPinsPerLine;
            _SecretLine = secret;
            _LineComparer = new LineComparer();
        }

        public int NumberOfPins { get; }

        public int NumberOfPinsPerLine { get; }

        public IReadOnlyList<GuessAndResult> GuessesAndResults => _GuessesAndResults;

        public Result Guess(Line guess)
        {
            var result = _LineComparer.Compare(guess, _SecretLine);
            var guessAndResult = new GuessAndResult(guess, result);
            _GuessesAndResults.Add(guessAndResult);
            return result;
        }

        public bool AreAllPinsCorrect(Result result)
        {
            return result.NumberOfCorrectPins == NumberOfPinsPerLine;
        }
    }
}
