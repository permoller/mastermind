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

        private readonly int[] _SecretLine;

        private readonly LineComparer _LineComparer;

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
            _SecretLine = secret;
            _LineComparer = new LineComparer();
            ValidateLine(_SecretLine);
        }

        private void ValidateLine(int[] line)
        {
            if (line is null)
                throw new ArgumentNullException("Line is null.");
            if (line.Length != _NumberOfPegsPerLine)
                throw new ArgumentException($"Incorrect number of pegs. Expected {_NumberOfPegsPerLine}. Got {line.Length}.");
            if (line.Any(p => p < 0 || p >= _NumberOfDifferentPegs))
                throw new ArgumentException($"Peg is out of range. Expected less than {_NumberOfDifferentPegs}. Got {string.Join(" ", line)}.");
        }

        public GamePlayResult Play(IPlayer player)
        {
            List<GuessAndResult> guessesAndResults = new List<GuessAndResult>();
            player.BeginGame(_NumberOfDifferentPegs, _NumberOfPegsPerLine, _MaxNumberOfGuesses);
            Result result;

            do
            {
                var guess = player.GetGuess();
                ValidateLine(guess);
                result = _LineComparer.Compare(guess, _SecretLine);
                guessesAndResults.Add(new GuessAndResult(guess, result));
                player.ResultFromPreviousGuess(result.NumberOfPegsWithCorrectColorAndCorrectPosition, result.NumberOfPegsWithCorrectColorAndWrongPosition);
            } while (result.NumberOfPegsWithCorrectColorAndCorrectPosition != _NumberOfPegsPerLine && guessesAndResults.Count < _MaxNumberOfGuesses);
            var wasTheSecretGuessed = result.NumberOfPegsWithCorrectColorAndCorrectPosition == _NumberOfPegsPerLine;
            var gameResult = new GamePlayResult(wasTheSecretGuessed, _SecretLine, guessesAndResults);
            player.EndGame(wasTheSecretGuessed, guessesAndResults.Count, _SecretLine);
            return gameResult;
        }
    }
}
