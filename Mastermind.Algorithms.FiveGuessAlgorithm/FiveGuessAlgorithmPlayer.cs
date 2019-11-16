namespace Mastermind.Algorithms.FiveGuessAlgorithm
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
    public class FiveGuessAlgorithmPlayer : IPlayer
    {
        private int _NumberOfDifferentPegs;
        private int _NumberOfPegsPerLine;
        private int _MaxNumberOfGuesses;
        private IList<Line> _PosibleSolutions;
        private IReadOnlyList<Line> _AllLines;
        private IList<Line> _UsedGuesses;
        private LineComparer _LineComparer = new LineComparer();

        public void BeginGame(int numberOfDifferentPegs, int numberOfPegsPerLine, int maxNumberOfGuesses)
        {
            _NumberOfDifferentPegs = numberOfDifferentPegs;
            _NumberOfPegsPerLine = numberOfPegsPerLine;
            _MaxNumberOfGuesses = maxNumberOfGuesses;
            _UsedGuesses = new List<Line>();

            // 1. Create the set S of 1296 possible codes(1111, 1112... 6665, 6666)
            _AllLines = GenerateAllLines(_NumberOfDifferentPegs, _NumberOfPegsPerLine, new Peg[0]).ToList();
            _PosibleSolutions = _AllLines.ToList();
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

        public int[] GetGuess()
        {
            Line guess;
            if (!_UsedGuesses.Any())
            {
                // 2. Start with initial guess 1122
                guess = new Line(Enumerable.Range(0, _NumberOfPegsPerLine).Select(i => new Peg((i % _NumberOfDifferentPegs) / 2)).ToArray());
            }
            else
            {
                // 6. Apply minimax technique to find a next guess as follows: 
                var guessesWithMaximumScore = new List<Line>();
                var maximumScore = 0;
                // For each possible guess, that is, any unused code of the 1296 not just those in S,
                var possibleGuesses = _AllLines.Except(_UsedGuesses);
                if (!possibleGuesses.Any())
                    throw new Exception("No posible solutions");
                foreach (var possibleGuess in possibleGuesses)
                {
                    // calculate how many possibilities in S would be eliminated for each possible colored/white peg score.
                    // The score of a guess is the minimum number of possibilities it might eliminate from S.
                    // A single pass through S for each unused code of the 1296 will provide a hit count for each colored/white peg score found;
                    var hitCounts = new int[_NumberOfPegsPerLine + 1, _NumberOfPegsPerLine + 1];
                    hitCounts.Initialize();
                    foreach (var posibleSolution in _PosibleSolutions)
                    {
                        var result = _LineComparer.Compare(possibleGuess, posibleSolution);
                        hitCounts[result.NumberOfPegsWithCorrectColorAndCorrectPosition, result.NumberOfPegsWithCorrectColorAndWrongPosition]++;
                    }
                    // the colored/white peg score with the highest hit count will eliminate the fewest possibilities;
                    var highestHitCount = 0;
                    for (int i = 0; i <= _NumberOfPegsPerLine; i++)
                    {
                        for (int j = 0; j <= _NumberOfPegsPerLine; j++)
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
                guess = guessesWithMaximumScore.FirstOrDefault(l => _PosibleSolutions.Any(l2 => IsEqual(l, l2))) ?? guessesWithMaximumScore.First();

                // 7. Repeat from step 3.
            }

            // 3. Play the guess to get a response of colored and white pegs.
            _UsedGuesses.Add(guess);
            return guess.Pegs.Select(p => p.Number).ToArray();
        }

        private bool IsEqual(Line x, Line y)
        {
            return Enumerable.SequenceEqual(x.Pegs.Select(p => p.Number), y.Pegs.Select(p => p.Number));
        }

        public void ResultFromPreviousGuess(int correctColorAndCorrectPosition, int corectColorWrongAndWrongPosition)
        {
            // 4. If the response is four colored pegs, the game is won, the algorithm terminates.
            if (correctColorAndCorrectPosition == _NumberOfPegsPerLine)
            {
                return;
            }
            // 5. Otherwise, remove from S any code that would not give the same response if it(the guess) were the code.
            _PosibleSolutions = _PosibleSolutions.Where(l =>
            {
                var previousGuess = _UsedGuesses.Last();
                var r = _LineComparer.Compare(previousGuess, l);
                return r.NumberOfPegsWithCorrectColorAndCorrectPosition == correctColorAndCorrectPosition
                && r.NumberOfPegsWithCorrectColorAndWrongPosition == corectColorWrongAndWrongPosition;
            }).ToList();
        }



        public void EndGame(bool wasTheSecretGuessed, int numberOfGuesses, int[] secret)
        {

        }
    }
}