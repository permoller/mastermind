namespace Mastermind.GameLogic
{
    public class GamePlayResult
    {
        public bool WasTheSecretGuessed { get; }
        public Line Secret { get; }

        public GamePlayResult(bool wasTheSecretGuessed, Line secret)
        {
            WasTheSecretGuessed = wasTheSecretGuessed;
            Secret = secret;
        }
    }
}