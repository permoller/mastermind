namespace Mastermind.ComputerPlayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mastermind.GameLogic;
    public class RandomNextGuessPlayer : Player
    {
        private IList<Line> _LinesThatCouldBeTheSecret;
        private Random _Random = new Random();
        private LineComparer _LineComparer = new LineComparer();
        private ResultEqualityComparer _ResultEqualityComparer = new ResultEqualityComparer();

        public override void BeginGame(IGame game)
        {
            _LinesThatCouldBeTheSecret = LineGenerator.GenerateAllDifferentLines(game.NumberOfPegs, game.NumberOfPegsPerLine);
        }

        public override Line GetGuess(IGame game)
        {
            if (game.GuessesAndResults.Any())
            {
                // filter out all lines that does not give the same result as the previous guess
                var previousResult = game.GuessesAndResults.Last().Result;
                var previousGuess = game.GuessesAndResults.Last().Guess;
                _LinesThatCouldBeTheSecret = _LinesThatCouldBeTheSecret.Where(l => _ResultEqualityComparer.Equals(previousResult, _LineComparer.Compare(previousGuess, l))).ToList();
            }
            // return a random line from the remaining lines that could be the secret
            return _LinesThatCouldBeTheSecret[_Random.Next(0, _LinesThatCouldBeTheSecret.Count - 1)];
        }
    }
}