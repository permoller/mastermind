using Mastermind.GameLogic;

namespace Mastermind.HumanPlayer
{
    // this class is just here to expose the ConsolePlayer functionality without having to make the interals public
    public class HumanPlayer : IPlayer
    {
        IPlayer _Player = new ConsolePlayer(new ConsoleWrapper());

        public void BeginGame(int numberOfDifferentPegs, int numberOfPegsPerLine, int maxNumberOfGuesses)
        {
            _Player.BeginGame(numberOfDifferentPegs, numberOfPegsPerLine, maxNumberOfGuesses);
        }

        public void EndGame(bool wasTheSecretGuessed, int numberOfGuesses, int[] secret)
        {
            _Player.EndGame(wasTheSecretGuessed, numberOfGuesses, secret);
        }

        public int[] GetGuess()
        {
            return _Player.GetGuess();
        }

        public void ResultFromPreviousGuess(int correctColorAndCorrectPosition, int corectColorWrongAndWrongPosition)
        {
            _Player.ResultFromPreviousGuess(correctColorAndCorrectPosition, corectColorWrongAndWrongPosition);
        }
    }
}