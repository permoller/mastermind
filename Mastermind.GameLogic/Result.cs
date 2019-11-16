namespace Mastermind.GameLogic
{
    public class Result
    {
        public int NumberOfPegsWithCorrectColorAndCorrectPosition { get; }

        public int NumberOfPegsWithCorrectColorAndWrongPosition { get; }

        internal Result(int correct, int wrongPosition)
        {
            NumberOfPegsWithCorrectColorAndCorrectPosition = correct;
            NumberOfPegsWithCorrectColorAndWrongPosition = wrongPosition;
        }
    }
}
