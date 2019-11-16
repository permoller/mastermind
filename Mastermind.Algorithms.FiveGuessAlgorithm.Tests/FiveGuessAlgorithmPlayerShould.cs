namespace Mastermind.Algorithms.FiveGuessAlgorithm.Tests
{
    using Xunit;
    using Mastermind.GameLogic;
    using System.Linq;
    using System;

    public class FiveGuessAlgorithmPlayerShould
    {
        [Fact]
        public void WinGame()
        {
            // Arrange
            var player = new FiveGuessAlgorithmPlayer();

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
            var player = new FiveGuessAlgorithmPlayer();

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
            new Game(4, 2, 10).Play(new FiveGuessAlgorithmPlayer());
        }

        [Theory]
        [InlineData(6, 1, "0")]
        [InlineData(6, 2, "0 0")]
        [InlineData(6, 3, "0 0 1")]
        [InlineData(6, 4, "0 0 1 1")]
        [InlineData(6, 5, "0 0 1 1 2")]
        [InlineData(6, 6, "0 0 1 1 2 2")]
        public void MakeCorrectInitialGuess(int numberOfPegs, int numberOfPegsPerLine, string expectedLineAsString)
        {
            // Arrange
            var expectedLine = expectedLineAsString.Split(' ').Select(s => int.Parse(s)).ToArray();
            var player = new FiveGuessAlgorithmPlayer();
            var game = new Game(numberOfPegs, numberOfPegsPerLine, 1);

            // Act
            var result = game.Play(player);

            // Assert
            Assert.Equal(expectedLine, result.GuessesAndResults.Single().Guess);
        }

        [Fact]
        public void WinAllGamesUsingMaximumFiveGuesses()
        {
            const int NumberOfDifferentPegs = 6;
            const int NumberOfPegsPerLine = 4;
            const int MaxNumberOfGuesses = 5;
            var player = new FiveGuessAlgorithmPlayer();
            var lines = LineGenerator.GenerateAllLines(NumberOfDifferentPegs, NumberOfPegsPerLine);
            // it is too slow to test all lines
            // we only test on average 1% of all the different lines (roughly 13 out of 1296)
            var random = new Random();
            lines = lines.Where(l => random.Next(100) == 10).ToList();
            Assert.All(lines,
             secret =>
             {
                 var game = new Game(NumberOfDifferentPegs, NumberOfPegsPerLine, MaxNumberOfGuesses, secret);
                 var result = game.Play(player);
                 Assert.True(result.WasTheSecretGuessed);
             });
        }
    }
}
