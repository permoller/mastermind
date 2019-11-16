namespace Mastermind.Algorithms.RandomGuessAmongPosibleSolutions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mastermind.GameLogic;
    public class RandomGuessAmongPosibleSolutionsPlayer : IPlayer
    {
        private int _NumberOfDifferentPegs;
        private int _NumberOfPegsPerLine;
        private int _MaxNumberOfGuesses;
        private Line _Guess;
        private IList<Line> _PosibleSolutions;
        private Random _Random = new Random();
        private LineComparer _LineComparer = new LineComparer();

        public void BeginGame(int numberOfDifferentPegs, int numberOfPegsPerLine, int maxNumberOfGuesses)
        {
            _NumberOfDifferentPegs = numberOfDifferentPegs;
            _NumberOfPegsPerLine = numberOfPegsPerLine;
            _MaxNumberOfGuesses = maxNumberOfGuesses;
            _Guess = null;
            _PosibleSolutions = GenerateAllLines(_NumberOfDifferentPegs, _NumberOfPegsPerLine, new Peg[0]).ToList();
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
            // return a random line from the remaining lines that could be the secret
            return (_Guess = _PosibleSolutions[_Random.Next(0, _PosibleSolutions.Count - 1)]).Pegs.Select(p => p.Number).ToArray();
        }

        public void ResultFromPreviousGuess(int correctColorAndCorrectPosition, int corectColorWrongAndWrongPosition)
        {
            // filter out all lines that does not give the same result as the previous guess
            _PosibleSolutions = _PosibleSolutions.Where(l =>
            {
                var r = _LineComparer.Compare(_Guess, l);
                return r.NumberOfPegsWithCorrectColorAndCorrectPosition == correctColorAndCorrectPosition
                && r.NumberOfPegsWithCorrectColorAndWrongPosition == corectColorWrongAndWrongPosition;
            }).ToList();
        }

        public void EndGame(bool wasTheSecretGuessed, int numberOfGuesses, int[] secret)
        {

        }
    }
}