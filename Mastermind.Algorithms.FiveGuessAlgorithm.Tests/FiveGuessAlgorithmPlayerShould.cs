namespace Mastermind.Algorithms.FiveGuessAlgorithm.Tests
{
    using Xunit;
    using Mastermind.GameLogic;

    public class FiveGuessAlgorithmPlayerShould
    {
        [Fact]
        public void PlayGame()
        {
            new Game(4, 2, 10).Play(new FiveGuessAlgorithmPlayer());
        }
    }
}
