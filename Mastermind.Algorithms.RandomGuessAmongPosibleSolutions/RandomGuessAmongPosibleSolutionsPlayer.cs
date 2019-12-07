namespace Mastermind.Algorithms.RandomGuessAmongPosibleSolutions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mastermind.GameLogic;

    /// <summery>
    /// Every guess is made by chosing a random line from at list of all posible solutions.
    /// For every guess the lines that will not give the same result as the guess are removed from a list of posible solutions.
    /// </summery>
    public class RandomGuessAmongPosibleSolutionsPlayer : IPlayer
    {
        private int _NumberOfDifferentPegs;
        private int _NumberOfPegsPerLine;
        private int _MaxNumberOfGuesses;
        private int[] _Guess;
        private IList<int[]> _PosibleSolutions;
        private Random _Random = new Random(1);
        private LineComparer _LineComparer = new LineComparer();

        public void BeginGame(int numberOfDifferentPegs, int numberOfPegsPerLine, int maxNumberOfGuesses)
        {
            _NumberOfDifferentPegs = numberOfDifferentPegs;
            _NumberOfPegsPerLine = numberOfPegsPerLine;
            _MaxNumberOfGuesses = maxNumberOfGuesses;
            _Guess = null;
            _PosibleSolutions = GenerateAllLines(_NumberOfDifferentPegs, _NumberOfPegsPerLine, new int[0]).ToList();
        }

        private static IEnumerable<int[]> GenerateAllLines(int numberOfPegs, int remainingNumberOfPegsInLine, IEnumerable<int> pegs)
        {
            if (remainingNumberOfPegsInLine == 0)
            {
                yield return pegs.ToArray();
            }
            else
            {
                for (int peg = 0; peg < numberOfPegs; peg++)
                {
                    var lines = GenerateAllLines(numberOfPegs, remainingNumberOfPegsInLine - 1, pegs.Append(peg));
                    foreach (var line in lines)
                    {
                        yield return line;
                    }
                }
            }
        }

        public int[] GetGuess()
        {
            // return a random line from the remaining lines that could be the secret
            if (_PosibleSolutions.Count == 0)
                throw new InvalidOperationException("No possible solution");

            return _Guess = _PosibleSolutions[_Random.Next(0, _PosibleSolutions.Count - 1)];
        }

        public void ResultFromPreviousGuess(int numberOfCorrectsPegs, int numberOfPegsAtWrongPosition)
        {
            // filter out all lines that does not give the same result as the previous guess
            _PosibleSolutions = _PosibleSolutions.Where(l =>
            {
                var r = _LineComparer.Compare(_Guess, l);
                return r.NumberOfCorrectPegs == numberOfCorrectsPegs
                && r.NumberOfPegsAtWrongPosition == numberOfPegsAtWrongPosition;
            }).ToList();
        }

        public void EndGame(bool wasTheSecretGuessed, int numberOfGuesses, int[] secret)
        {

        }
    }
}