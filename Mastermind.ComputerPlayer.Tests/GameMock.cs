namespace Mastermind.ComputerPlayer.Tests
{
    using System.Collections.Generic;
    using Mastermind.GameLogic;

    internal class GameMock : IGame
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