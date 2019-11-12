namespace Mastermind.ComputerPlayer.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Mastermind.GameLogic;
    using Xunit;
    public class FiveGuessAlgorithmPlayerShould
    {
        [Fact]
        public void WinGame()
        {
            // Arrange
            var player = new FiveGuessAlgorithmPlayer();
            var guessesAndResults = new List<GuessAndResult>();
            var game = new GameMock(8, 4, 10, guessesAndResults);

            // Act
            player.BeginGame(game);
            var guess1 = player.GetGuess(game);
            guessesAndResults.Add(new GuessAndResult(guess1, new Result(0, 0)));
            var guess2 = player.GetGuess(game);
            guessesAndResults.Add(new GuessAndResult(guess2, new Result(4, 0)));
            player.EndGame(game, new GamePlayResult(true, guess2));

            // Assert - The game must complete without errors
        }

        [Fact]
        public void LooseGame()
        {
            // Arrange
            var player = new FiveGuessAlgorithmPlayer();
            var guessesAndResults = new List<GuessAndResult>();
            var game = new GameMock(9, 4, 2, guessesAndResults);

            // Act
            player.BeginGame(game);
            var guess1 = player.GetGuess(game);
            guessesAndResults.Add(new GuessAndResult(guess1, new Result(0, 0)));
            var guess2 = player.GetGuess(game);
            guessesAndResults.Add(new GuessAndResult(guess2, new Result(0, 0)));
            var usedPegNumbers = guess1.Pegs.Select(p => p.Number).Union(guess2.Pegs.Select(p => p.Number));
            var pegNumberNotUsed = Enumerable.Range(0, 10).Except(usedPegNumbers).First();
            player.EndGame(game, new GamePlayResult(false, new Line(pegNumberNotUsed, pegNumberNotUsed, pegNumberNotUsed, pegNumberNotUsed)));

            // Assert - The game must complete without errors
        }

        [Fact]
        public void PlayGame()
        {
            // Arrange
            var game = new Game(8, 4, 10);
            var player = new FiveGuessAlgorithmPlayer();

            // Act
            game.Play(player);

            // Assert - The game must complete without errors
        }

        // This works but it is pretty slow
        // [Fact]
        // public void WinAllGamesWithMaxFiveGuesses()
        // {
        //     // Arrange
        //     foreach (var secret in LineGenerator.GenerateAllDifferentLines(6, 4))
        //     {
        //         var game = new Game(6, 4, 5, secret);
        //         var player = new FiveGuessAlgorithmPlayer();

        //         // Act
        //         var result = game.Play(player);

        //         // Assert
        //         Assert.True(result.WasTheSecretGuessed, string.Join(" ", secret.Pegs.Select(p => p.Number)));
        //     }
        // }

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
            var expectedLine = LineParser.ParseLine(expectedLineAsString);
            var player = new FiveGuessAlgorithmPlayer();
            var game = new Game(numberOfPegs, numberOfPegsPerLine, 1);
            var comparer = new LinePegEqualityComparer();

            // Act
            game.Play(player);

            // Assert
            Assert.Equal(expectedLine, game.GuessesAndResults.Single().Guess, comparer);
        }
    }
}