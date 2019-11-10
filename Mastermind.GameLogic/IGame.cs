namespace Mastermind.GameLogic
{
    using System.Collections.Generic;

    public interface IGame
    {
        int NumberOfPins { get; }
        int NumberOfPinsPerLine { get; }
        int MaxNumberOfGuesses { get; }
        IReadOnlyList<GuessAndResult> GuessesAndResults { get; }
    }
}
