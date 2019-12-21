namespace Mastermind.Algorithms.FiveGuessAlgorithmWithCache
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
    public class FiveGuessAlgorithmWithCachePlayer : IPlayer
    {
        private int _NumberOfDifferentPegs;
        private int _NumberOfPegsPerLine;
        private int _MaxNumberOfGuesses;
        private IList<int> _PosibleSolutions;
        private IReadOnlyList<int> _AllLines;
        private IList<int> _UsedGuesses;
        private LineComparerWithCaching _LineComparer;
        Dictionary<int, Dictionary<int, LineComparerWithCaching>> _Cache = new Dictionary<int, Dictionary<int, LineComparerWithCaching>>();

        public void BeginGame(int numberOfDifferentPegs, int numberOfPegsPerLine, int maxNumberOfGuesses)
        {
            _NumberOfDifferentPegs = numberOfDifferentPegs;
            _NumberOfPegsPerLine = numberOfPegsPerLine;
            _MaxNumberOfGuesses = maxNumberOfGuesses;
            _UsedGuesses = new List<int>();

            if (!_Cache.TryGetValue(numberOfDifferentPegs, out var dic))
                _Cache.Add(numberOfDifferentPegs, dic = new Dictionary<int, LineComparerWithCaching>());
            if (!dic.TryGetValue(numberOfPegsPerLine, out _LineComparer))
                dic.Add(numberOfPegsPerLine, _LineComparer = new LineComparerWithCaching(_NumberOfDifferentPegs, _NumberOfPegsPerLine));

            // 1. Create the set S of 1296 possible codes(1111, 1112... 6665, 6666)
            _AllLines = Enumerable.Range(0, _LineComparer.NumberOfDifferentLines).ToList();
            _PosibleSolutions = _AllLines.ToList();
        }


        public int[] GetGuess()
        {
            int guess;
            if (!_UsedGuesses.Any())
            {
                // 2. Start with initial guess 1122
                guess = _LineComparer.GetLineIndex(Enumerable.Range(0, _NumberOfPegsPerLine).Select(i => (i % _NumberOfDifferentPegs) / 2).ToArray());
            }
            else
            {
                // 6. Apply minimax technique to find a next guess as follows: 
                var guessesWithMaximumScore = new List<int>();
                var maximumScore = 0;
                // For each possible guess, that is, any unused code of the 1296 not just those in S,
                var possibleGuesses = _AllLines.Except(_UsedGuesses);
                if (!possibleGuesses.Any())
                    throw new InvalidOperationException("All guesses used");
                if (!_PosibleSolutions.Any())
                    throw new InvalidOperationException("No posible solution");
                foreach (var possibleGuess in possibleGuesses)
                {
                    // calculate how many possibilities in S would be eliminated for each possible colored/white peg score.
                    // The score of a guess is the minimum number of possibilities it might eliminate from S.
                    // A single pass through S for each unused code of the 1296 will provide a hit count for each colored/white peg score found;
                    var hitCounts = new int[_LineComparer.NumberOfDifferentResults];
                    hitCounts.Initialize();
                    foreach (var posibleSolution in _PosibleSolutions)
                    {
                        var result = _LineComparer.Compare(possibleGuess, posibleSolution);
                        hitCounts[result]++;
                    }
                    // the colored/white peg score with the highest hit count will eliminate the fewest possibilities;
                    var highestHitCount = 0;
                    for (int i = 0; i < hitCounts.Length; i++)
                    {
                        highestHitCount = Math.Max(highestHitCount, hitCounts[i]);
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
                guess = guessesWithMaximumScore.Cast<int?>().FirstOrDefault(l => _PosibleSolutions.Contains(l.Value)) ?? guessesWithMaximumScore.First();

                // 7. Repeat from step 3.
            }

            // 3. Play the guess to get a response of colored and white pegs.
            _UsedGuesses.Add(guess);
            return _LineComparer.GetLine(guess);
        }

        public void ResultFromPreviousGuess(int numberOfCorrectsPegs, int numberOfPegsAtWrongPosition)
        {
            // 4. If the response is four colored pegs, the game is won, the algorithm terminates.
            if (numberOfCorrectsPegs == _NumberOfPegsPerLine)
            {
                return;
            }
            // 5. Otherwise, remove from S any code that would not give the same response if it(the guess) were the code.
            var result = _LineComparer.GetResult(numberOfCorrectsPegs, numberOfPegsAtWrongPosition);
            var previousGuess = _UsedGuesses.Last();
            _PosibleSolutions = _PosibleSolutions.Where(l =>
            {
                var r = _LineComparer.Compare(previousGuess, l);
                return r == result;
            }).ToList();
        }



        public void EndGame(bool wasTheSecretGuessed, int numberOfGuesses, int[] secret)
        {

        }
    }
}