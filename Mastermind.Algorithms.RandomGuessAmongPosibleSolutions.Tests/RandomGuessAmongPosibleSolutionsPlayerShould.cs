namespace Mastermind.Algorithms.RandomGuessAmongPosibleSolutions.Tests
{
    using Xunit;
    using Mastermind.GameLogic;

    public class RandomGuessAmongPosibleSolutionsPlayerShould
    {
        [Fact]
        public void PlayGame()
        {
            new Game(4, 2, 10).Play(new RandomGuessAmongPosibleSolutionsPlayer());
        }
    }
}
