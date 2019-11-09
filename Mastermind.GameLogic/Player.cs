namespace Mastermind.GameLogic
{
    public abstract class Player
    {
        public virtual void BeginGame(Game game)
        {
        }

        public abstract Line GetGuess(Game game);

        public virtual void EndGame(Game game, bool wasTheSecretGuessed, Line secret)
        {
        }
    }
}