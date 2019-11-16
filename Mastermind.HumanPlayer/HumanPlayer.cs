using Mastermind.GameLogic;

namespace Mastermind.HumanPlayer
{
    public class HumanPlayer : Player
    {
        Player player = new ConsolePlayer(new ConsoleWrapper());

        public override void BeginGame(IGame game)
        {
            player.BeginGame(game);
        }

        public override Line GetGuess(IGame game)
        {
            return player.GetGuess(game);
        }

        public override void EndGame(IGame game, GamePlayResult result)
        {
            player.EndGame(game, result);
        }
    }
}