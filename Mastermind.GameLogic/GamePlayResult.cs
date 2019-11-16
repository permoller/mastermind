using System.Collections.Generic;

namespace Mastermind.GameLogic
{
    public class GamePlayResult
    {
        public bool WasTheSecretGuessed { get; }
        public int[] Secret { get; }
        IReadOnlyCollection<GuessAndResult> GuessesAndResults { get; }

        public GamePlayResult(bool wasTheSecretGuessed, int[] secret, IReadOnlyCollection<GuessAndResult> guessesAndResults)
        {
            WasTheSecretGuessed = wasTheSecretGuessed;
            Secret = secret;
            GuessesAndResults = guessesAndResults;
        }
    }
}