namespace Mastermind.GameLogic
{
    public class GuessAndResult
    {
        public GuessAndResult(int[] guess, Result result)
        {
            Guess = guess;
            Result = result;
        }

        public int[] Guess { get; }

        public Result Result { get; }
    }
}