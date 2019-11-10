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
            var usedPinNumbers = guess1.Pins.Select(p => p.Number).Union(guess2.Pins.Select(p => p.Number));
            var pinNumberNotUsed = Enumerable.Range(0, 10).Except(usedPinNumbers).First();
            player.EndGame(game, new GamePlayResult(false, new Line(pinNumberNotUsed, pinNumberNotUsed, pinNumberNotUsed, pinNumberNotUsed)));

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
            public GameMock(int numberOfPins, int numberOfPinsPerLine, int maxNumberOfGuesses, IReadOnlyList<GuessAndResult> guessesAndResults)
            {
                NumberOfPins = numberOfPins;
                NumberOfPinsPerLine = numberOfPinsPerLine;
                MaxNumberOfGuesses = maxNumberOfGuesses;
                GuessesAndResults = guessesAndResults;
            }

            public int NumberOfPins { get; }

            public int NumberOfPinsPerLine { get; }

            public int MaxNumberOfGuesses { get; }

            public IReadOnlyList<GuessAndResult> GuessesAndResults { get; }
        }
    }
}