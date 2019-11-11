namespace Mastermind.GameLogic
{
    using System.Collections.Generic;

    public interface IGame
    {
        int NumberOfPegs { get; }
        int NumberOfPegsPerLine { get; }
        int MaxNumberOfGuesses { get; }
        IReadOnlyList<GuessAndResult> GuessesAndResults { get; }
    }
}
