namespace Mastermind.ComputerPlayer.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Mastermind.GameLogic;
    using Xunit;
    public class RandomNextGuessPlayerShould
    {

        [Fact]
        public void WinGame()
        {
            // Arrange
            var player = new RandomNextGuessPlayer();
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
            var player = new RandomNextGuessPlayer();
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
            var player = new RandomNextGuessPlayer();

            // Act
            game.Play(player);

            // Assert - The game must complete without errors
        }

        private class GameMock : IGame
        {
            public GameMock(int numberOfPegs, int numberOfPegsPerLine, int maxNumberOfGuesses, IReadOnlyList<GuessAndResult> guessesAndResults)
            {
                NumberOfPegs = numberOfPegs;
                NumberOfPegsPerLine = numberOfPegsPerLine;
                MaxNumberOfGuesses = maxNumberOfGuesses;
                GuessesAndResults = guessesAndResults;
            }

            public int NumberOfPegs { get; }

            public int NumberOfPegsPerLine { get; }

            public int MaxNumberOfGuesses { get; }

            public IReadOnlyList<GuessAndResult> GuessesAndResults { get; }
        }
    }
}