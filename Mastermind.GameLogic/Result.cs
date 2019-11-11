using System;

namespace Mastermind.GameLogic
{
    public class Result
    {
        public int NumberOfCorrectPegs { get; }

        public int NumberOfCorrectColoredPegsInWrongPosition { get; }

        public Result(int correct, int wrongPosition)
        {
            NumberOfCorrectPegs = correct;
            NumberOfCorrectColoredPegsInWrongPosition = wrongPosition;
        }
    }
}
