namespace Mastermind.Algorithms.RandomGuessAmongPosibleSolutions.Tests
{
    using Xunit;
    using Mastermind.GameLogic;
    using System.Linq;

    public class RandomGuessAmongPosibleSolutionsPlayerShould
    {
        [Fact]
        public void WinGame()
        {
            // Arrange
            var player = new RandomGuessAmongPosibleSolutionsPlayer();

            // Act
            player.BeginGame(8, 4, 10);
            var guess1 = player.GetGuess();
            player.ResultFromPreviousGuess(0, 0);
            var guess2 = player.GetGuess();
            player.ResultFromPreviousGuess(4, 0);
            player.EndGame(true, 2, guess2);

            // Assert - The game must complete without errors
        }

        [Fact]
        public void LooseGame()
        {
            // Arrange
            var player = new RandomGuessAmongPosibleSolutionsPlayer();

            // Act
            player.BeginGame(9, 4, 2);
            var guess1 = player.GetGuess();
            player.ResultFromPreviousGuess(0, 0);
            var guess2 = player.GetGuess();
            player.ResultFromPreviousGuess(0, 0);
            var secret = Enumerable.Repeat(Enumerable.Range(0, 10).Except(guess1).Except(guess2).First(), 4);
            player.EndGame(false, 2, secret.ToArray());

            // Assert - The game must complete without errors
        }
        [Fact]
        public void PlayGame()
        {
            new Game(4, 2, 10).Play(new RandomGuessAmongPosibleSolutionsPlayer());
        }
    }
}
