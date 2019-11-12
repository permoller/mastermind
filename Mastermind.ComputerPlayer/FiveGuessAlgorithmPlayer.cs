namespace Mastermind.ComputerPlayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mastermind.GameLogic;
    // <Summery>
    // From Wikipedia: https://en.wikipedia.org/wiki/Mastermind_(board_game)
    // In 1977, Donald Knuth demonstrated that the codebreaker can solve the pattern in five moves or fewer, using an algorithm that progressively reduces the number of possible patterns.[11] The algorithm works as follows: 
    // 1. Create the set S of 1296 possible codes(1111, 1112 ... 6665, 6666)
    // 2. Start with initial guess 1122
    // 3. Play the guess to get a response of colored and white pegs.
    // 4. If the response is four colored pegs, the game is won, the algorithm terminates.
    // 5. Otherwise, remove from S any code that would not give the same response if it (the guess) were the code.
    // 6. Apply minimax technique to find a next guess as follows: 
    //    For each possible guess, that is, any unused code of the 1296 not just those in S,
    //    calculate how many possibilities in S would be eliminated for each possible colored/white peg score.
    //    The score of a guess is the minimum number of possibilities it might eliminate from S.
    //    A single pass through S for each unused code of the 1296 will provide a hit count for each colored/white peg score found;
    //    the colored/white peg score with the highest hit count will eliminate the fewest possibilities;
    //    calculate the score of a guess by using "minimum eliminated" = "count of elements in S" - (minus) "highest hit count".
    //    From the set of guesses with the maximum score, select one as the next guess, choosing a member of S whenever possible.
    // 7. Repeat from step 3.
    // </Summery>
    public class FiveGuessAlgorithmPlayer : Player
    {
        private IList<Line> _PosibleSolutions;
        private IReadOnlyList<Line> _AllLines;
        private LineComparer _LineComparer = new LineComparer();
        private IEqualityComparer<Line> _LineEqualityComparer = new LinePegEqualityComparer();
        private ResultEqualityComparer _ResultEqualityComparer = new ResultEqualityComparer();

        public override void BeginGame(IGame game)
        {
            // 1. Create the set S of 1296 possible codes(1111, 1112... 6665, 6666)
            _AllLines = LineGenerator.GenerateAllDifferentLines(game.NumberOfPegs, game.NumberOfPegsPerLine).ToList();
            _PosibleSolutions = _AllLines.ToList();
        }

        public override Line GetGuess(IGame game)
        {
            Line guess;
            if (!game.GuessesAndResults.Any())
            {
                // 2. Start with initial guess 1122
                guess = new Line(Enumerable.Range(0, game.NumberOfPegsPerLine).Select(i => new Peg((i % game.NumberOfPegs) / 2)).ToArray());
            }
            else
            {
                // 5. Otherwise, remove from S any code that would not give the same response if it(the guess) were the code.
                var previousResult = game.GuessesAndResults.Last().Result;
                var previousGuess = game.GuessesAndResults.Last().Guess;
                _PosibleSolutions = _PosibleSolutions.Where(l => _ResultEqualityComparer.Equals(previousResult, _LineComparer.Compare(previousGuess, l))).ToList();

                // 6. Apply minimax technique to find a next guess as follows: 
                var guessesWithMaximumScore = new List<Line>();
                var maximumScore = 0;
                // For each possible guess, that is, any unused code of the 1296 not just those in S,
                foreach (var possibleGuess in _AllLines.Except(game.GuessesAndResults.Select(x => x.Guess), _LineEqualityComparer))
                {
                    // calculate how many possibilities in S would be eliminated for each possible colored/white peg score.
                    // The score of a guess is the minimum number of possibilities it might eliminate from S.
                    // A single pass through S for each unused code of the 1296 will provide a hit count for each colored/white peg score found;
                    var hitCounts = new int[game.NumberOfPegs, game.NumberOfPegs];
                    hitCounts.Initialize();
                    foreach (var posibleSolution in _PosibleSolutions)
                    {
                        var result = _LineComparer.Compare(possibleGuess, posibleSolution);
                        hitCounts[result.NumberOfCorrectPegs, result.NumberOfCorrectColoredPegsInWrongPosition]++;
                    }
                    // the colored/white peg score with the highest hit count will eliminate the fewest possibilities;
                    var highestHitCount = 0;
                    for (int i = 0; i < game.NumberOfPegs; i++)
                    {
                        for (int j = 0; j < game.NumberOfPegs; j++)
                        {
                            highestHitCount = Math.Max(highestHitCount, hitCounts[i, j]);
                        }
                    }
                    // calculate the score of a guess by using "minimum eliminated" = "count of elements in S" - (minus) "highest hit count".
                    var minimumEliminated = _PosibleSolutions.Count - highestHitCount;

                    if (minimumEliminated > maximumScore)
                    {
                        guessesWithMaximumScore.Clear();
                        guessesWithMaximumScore.Add(possibleGuess);
                        maximumScore = minimumEliminated;
                    }
                    else if (minimumEliminated == maximumScore)
                    {
                        guessesWithMaximumScore.Add(possibleGuess);
                    }
                }
                // From the set of guesses with the maximum score, select one as the next guess, choosing a member of S whenever possible.
                guess = guessesWithMaximumScore.FirstOrDefault(g => _PosibleSolutions.Contains(g, _LineEqualityComparer)) ?? guessesWithMaximumScore.First();

                // 7. Repeat from step 3.
            }

            // 3. Play the guess to get a response of colored and white pegs.
            return guess;
        }

        public override void EndGame(IGame game, GamePlayResult result)
        {
            // 4. If the response is four colored pegs, the game is won, the algorithm terminates.
        }
    }
}