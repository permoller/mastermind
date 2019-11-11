namespace Mastermind.GameLogic
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class Game : IGame
    {
        public int NumberOfPegs { get; }

        public int NumberOfPegsPerLine { get; }

        public int MaxNumberOfGuesses { get; }

        public IReadOnlyList<GuessAndResult> GuessesAndResults => _GuessesAndResults;

        private readonly Line _SecretLine;

        private readonly LineComparer _LineComparer;

        private readonly List<GuessAndResult> _GuessesAndResults = new List<GuessAndResult>();

        private static Line RandomLine(int numberOfPegs, int numberOfPegsPerLine)
        {
            var random = new Random();
            var pegs = new Peg[numberOfPegsPerLine];
            for (var i = 0; i < numberOfPegsPerLine; i++)
            {
                pegs[i] = random.Next(0, numberOfPegs - 1);
            }
            return new Line(pegs);
        }

        public Game(int numberOfPegs, int numberOfPegsPerLine, int maxNumberOfGuesses) : this(numberOfPegs, numberOfPegsPerLine, maxNumberOfGuesses, RandomLine(numberOfPegs, numberOfPegsPerLine))
        {
        }
        public Game(int numberOfPegs, int numberOfPegsPerLine, int maxNumberOfGuesses, Line secret)
        {
            NumberOfPegs = numberOfPegs;
            NumberOfPegsPerLine = numberOfPegsPerLine;
            MaxNumberOfGuesses = maxNumberOfGuesses;
            _SecretLine = secret;
            _LineComparer = new LineComparer();
            ValidateLine(_SecretLine);
        }

        private void ValidateLine(Line line)
        {
            if (line is null)
                throw new ArgumentNullException("Line is null");
            if (line.Pegs.Count != NumberOfPegsPerLine)
                throw new ArgumentException("Incorrect number of pegs.");
            if (line.Pegs.Any(p => p.Number < 0 || p.Number >= NumberOfPegs))
                throw new ArgumentException("Peg is out of range");
        }

        public GamePlayResult Play(Player player)
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
            } while (result.NumberOfCorrectPegs != NumberOfPegsPerLine && _GuessesAndResults.Count < MaxNumberOfGuesses);
            var wasTheSecretGuessed = result.NumberOfCorrectPegs == NumberOfPegsPerLine;
            var gameResult = new GamePlayResult(wasTheSecretGuessed, _SecretLine);
            player.EndGame(this, gameResult);
            return gameResult;
        }
    }
}
