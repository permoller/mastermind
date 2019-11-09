using System.Collections.Generic;

namespace Mastermind.GameLogic
{
    public interface IGame
    {
        int NumberOfPins { get; }

        int NumberOfPinsPerLine { get; }

        IReadOnlyList<GuessAndResult> GuessesAndResults { get; }

        Result Guess(Line guess);

        bool AreAllPinsCorrect(Result result);
    }
}
