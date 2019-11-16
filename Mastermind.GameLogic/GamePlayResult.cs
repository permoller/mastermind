namespace Mastermind.GameLogic
{
    public class GamePlayResult
    {
        public bool WasTheSecretGuessed { get; }
        public int[] Secret { get; }

        public GamePlayResult(bool wasTheSecretGuessed, int[] secret)
        {
            WasTheSecretGuessed = wasTheSecretGuessed;
            Secret = secret;
        }
    }
}