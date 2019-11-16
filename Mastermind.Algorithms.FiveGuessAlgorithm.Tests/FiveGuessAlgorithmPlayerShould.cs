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

        [Fact]
        public void MakeSpecialFirstGuess()
        {
            var game = new Game(6, 4, 10);
            var result = game.Play(new FiveGuessAlgorithmPlayer());

        }

        [Fact]
        public void WinAllGamesUsingAtMostFiveGuesses()
        {
            const int NumberOfDifferentPegs = 6;
            const int NumberOfPegsPerLine = 4;
            const int MaxNumberOfGuesses = 5;
            var player = new FiveGuessAlgorithmPlayer();
            Assert.All(LineGenerator.GenerateAllLines(NumberOfDifferentPegs, NumberOfPegsPerLine),
             secret =>
             {
                 var game = new Game(NumberOfDifferentPegs, NumberOfPegsPerLine, MaxNumberOfGuesses, secret);
                 var result = game.Play(player);
                 Assert.True(result.WasTheSecretGuessed);
             });
        }
    }
}
