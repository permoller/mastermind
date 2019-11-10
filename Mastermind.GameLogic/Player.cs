namespace Mastermind.GameLogic
{
    public abstract class Player
    {
        public virtual void BeginGame(IGame game)
        {
        }

        public abstract Line GetGuess(IGame game);

        public virtual void EndGame(IGame game, GamePlayResult result)
        {
        }
    }
}