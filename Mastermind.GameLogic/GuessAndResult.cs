namespace Mastermind.GameLogic
{
    public class GuessAndResult
    {
        public GuessAndResult(Line guess, Result result)
        {
            Guess = guess;
            Result = result;
        }

        public Line Guess { get; }

        public Result Result { get; }
    }
}