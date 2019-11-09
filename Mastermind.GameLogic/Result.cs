using System;

namespace Mastermind.GameLogic
{
    public class Result
    {
        public int NumberOfCorrectPins { get; }

        public int NumberOfCorrectColoredPinsInWrongPosition { get; }

        public Result(int correct, int wrongPosition)
        {
            NumberOfCorrectPins = correct;
            NumberOfCorrectColoredPinsInWrongPosition = wrongPosition;
        }
    }
}
