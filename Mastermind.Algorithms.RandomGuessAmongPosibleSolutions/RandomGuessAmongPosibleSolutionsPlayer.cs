namespace Mastermind.Algorithms.RandomGuessAmongPosibleSolutions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mastermind.GameLogic;
    public class RandomGuessAmongPosibleSolutionsPlayer : Player
    {
        private IList<Line> _PosibleSolutions;
        private Random _Random = new Random();
        private LineComparer _LineComparer = new LineComparer();

        public override void BeginGame(IGame game)
        {
            var allLines = new List<Line>();

            _PosibleSolutions = GenerateAllLines(game.NumberOfPegs, game.NumberOfPegsPerLine, new Peg[0]).ToList();
        }

        private static IEnumerable<Line> GenerateAllLines(int numberOfPegs, int remainingNumberOfPegsInLine, IEnumerable<Peg> pegs)
        {
            if (remainingNumberOfPegsInLine == 0)
            {
                yield return new Line(pegs.ToArray());
            }
            else
            {
                for (int number = 0; number < numberOfPegs; number++)
                {
                    var lines = GenerateAllLines(numberOfPegs, remainingNumberOfPegsInLine - 1, pegs.Append(new Peg(number)));
                    foreach (var line in lines)
                    {
                        yield return line;
                    }
                }
            }
        }

        public override Line GetGuess(IGame game)
        {
            if (game.GuessesAndResults.Any())
            {
                // filter out all lines that does not give the same result as the previous guess
                var previousResult = game.GuessesAndResults.Last().Result;
                var previousGuess = game.GuessesAndResults.Last().Guess;
                _PosibleSolutions = _PosibleSolutions.Where(l =>
                {
                    var r = _LineComparer.Compare(previousGuess, l);
                    return r.NumberOfCorrectPegs == previousResult.NumberOfCorrectPegs
                    && r.NumberOfCorrectColoredPegsInWrongPosition == previousResult.NumberOfCorrectColoredPegsInWrongPosition;
                }).ToList();
            }
            // return a random line from the remaining lines that could be the secret
            return _PosibleSolutions[_Random.Next(0, _PosibleSolutions.Count - 1)];
        }
    }
}