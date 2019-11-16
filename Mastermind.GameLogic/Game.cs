namespace Mastermind.GameLogic
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class Game
    {
        private readonly int _NumberOfDifferentPegs;

        private readonly int _NumberOfPegsPerLine;

        private readonly int _MaxNumberOfGuesses;

        private readonly Line _SecretLine;

        private readonly LineComparer _LineComparer;

        private readonly List<GuessAndResult> _GuessesAndResults = new List<GuessAndResult>();

        public IReadOnlyList<GuessAndResult> GuessesAndResults => _GuessesAndResults;

        private static int[] RandomLine(int numberOfDifferentPegs, int numberOfPegsPerLine)
        {
            var random = new Random();
            var pegs = new int[numberOfPegsPerLine];
            for (var i = 0; i < numberOfPegsPerLine; i++)
            {
                pegs[i] = random.Next(0, numberOfDifferentPegs - 1);
            }
            return pegs;
        }

        public Game(int numberOfDifferentPegs, int numberOfPegsPerLine, int maxNumberOfGuesses) : this(numberOfDifferentPegs, numberOfPegsPerLine, maxNumberOfGuesses, RandomLine(numberOfDifferentPegs, numberOfPegsPerLine))
        {
        }
        public Game(int numberOfDifferentPegs, int numberOfPegsPerLine, int maxNumberOfGuesses, int[] secret)
        {
            _NumberOfDifferentPegs = numberOfDifferentPegs;
            _NumberOfPegsPerLine = numberOfPegsPerLine;
            _MaxNumberOfGuesses = maxNumberOfGuesses;
            _SecretLine = new Line(secret.Select(i => new Peg(i)).ToArray());
            _LineComparer = new LineComparer();
            ValidateLine(_SecretLine);
        }

        private void ValidateLine(Line line)
        {
            if (line is null)
                throw new ArgumentNullException("Line is null.");
            if (line.Pegs.Count != _NumberOfPegsPerLine)
                throw new ArgumentException($"Incorrect number of pegs. Expected {_NumberOfPegsPerLine}. Got {line.Pegs.Count}.");
            if (line.Pegs.Any(p => p.Number < 0 || p.Number >= _NumberOfDifferentPegs))
                throw new ArgumentException($"Peg is out of range. Expected less than {_NumberOfDifferentPegs}. Got {string.Join(" ", line.Pegs.Select(p => p.Number))}.");
        }

        public GamePlayResult Play(IPlayer player)
        {
            _GuessesAndResults.Clear();
            player.BeginGame(_NumberOfDifferentPegs, _NumberOfPegsPerLine, _MaxNumberOfGuesses);
            Result result;
            Line guess;
            do
            {
                guess = new Line(player.GetGuess().Select(i => new Peg(i)).ToArray());
                ValidateLine(guess);
                result = _LineComparer.Compare(guess, _SecretLine);
                _GuessesAndResults.Add(new GuessAndResult(guess, result));
                player.ResultFromPreviousGuess(result.NumberOfPegsWithCorrectColorAndCorrectPosition, result.NumberOfPegsWithCorrectColorAndWrongPosition);
            } while (result.NumberOfPegsWithCorrectColorAndCorrectPosition != _NumberOfPegsPerLine && _GuessesAndResults.Count < _MaxNumberOfGuesses);
            var wasTheSecretGuessed = result.NumberOfPegsWithCorrectColorAndCorrectPosition == _NumberOfPegsPerLine;
            var gameResult = new GamePlayResult(wasTheSecretGuessed, _SecretLine);
            player.EndGame(wasTheSecretGuessed, _GuessesAndResults.Count, _SecretLine.Pegs.Select(p => p.Number).ToArray());
            return gameResult;
        }
    }
}
