namespace Mastermind.GameLogic
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class Game
    {
        public int NumberOfPins { get; }

        public int NumberOfPinsPerLine { get; }

        public IReadOnlyList<GuessAndResult> GuessesAndResults => _GuessesAndResults;

        public int MaxNumberOfGuesses { get; }

        private readonly Line _SecretLine;

        private readonly LineComparer _LineComparer;

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

        public Game(int numberOfPins, int numberOfPinsPerLine, int maxNumberOfGuesses) : this(numberOfPins, numberOfPinsPerLine, maxNumberOfGuesses, RandomLine(numberOfPins, numberOfPinsPerLine))
        {
        }
        public Game(int numberOfPins, int numberOfPinsPerLine, int maxNumberOfGuesses, Line secret)
        {
            NumberOfPins = numberOfPins;
            NumberOfPinsPerLine = numberOfPinsPerLine;
            MaxNumberOfGuesses = maxNumberOfGuesses;
            _SecretLine = secret;
            _LineComparer = new LineComparer();
            ValidateLine(_SecretLine);
        }

        private void ValidateLine(Line line)
        {
            if (line is null)
                throw new ArgumentNullException("Line is null");
            if (line.Pins.Count != NumberOfPinsPerLine)
                throw new ArgumentException("Incorrect number of pins.");
            if (line.Pins.Any(p => p.Number < 0 || p.Number >= NumberOfPins))
                throw new ArgumentException("Pin is out of range");
        }

        public bool Play(Player player)
        {
            _GuessesAndResults.Clear();
            player.BeginGame(this);
            Result result;
            Line guess;
            do
            {
                guess = player.GetGuess(this);
                ValidateLine(guess);
                result = _LineComparer.Compare(guess, _SecretLine);
                _GuessesAndResults.Add(new GuessAndResult(guess, result));
            } while (result.NumberOfCorrectPins != NumberOfPinsPerLine && _GuessesAndResults.Count < MaxNumberOfGuesses);
            var wasTheSecretGuessed = result.NumberOfCorrectPins == NumberOfPinsPerLine;
            player.EndGame(this, wasTheSecretGuessed, _SecretLine);
            return wasTheSecretGuessed;
        }
    }
}
