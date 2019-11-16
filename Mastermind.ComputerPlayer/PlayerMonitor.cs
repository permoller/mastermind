namespace Mastermind.ComputerPlayer
{
    using System;
    using Mastermind.GameLogic;
    internal class PlayerMonitor : Player
    {
        private readonly Player _Player;
        private readonly Action _OnGetGuess;

        public PlayerMonitor(Player player, Action onGetGuess)
        {
            _Player = player;
            _OnGetGuess = onGetGuess;
        }

        override public void BeginGame(IGame game)
        {
            _Player.BeginGame(game);
        }
        public override Line GetGuess(IGame game)
        {
            _OnGetGuess();
            return _Player.GetGuess(game);
        }
        override public void EndGame(IGame game, GamePlayResult result)
        {
            _Player.EndGame(game, result);
        }
    }
}