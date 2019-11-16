namespace Mastermind.GameLogic
{
    public interface IPlayer
    {
        /// <summery>
        /// Called once at the beginning of each game.
        /// The same player instance may be reused for multiple games, but only one game at the time.
        /// </summery>
        /// <param name="numberOfDifferentPegs">The valid pegs in the game are numbered from 0 to one less than the number of different pegs.</param>
        /// <param name="numberOfPegsPerLine">The number of pegs used in a line in the game.</param>
        /// <param name="maxNumberOfGuesses">The number of guesses the player has to guess the secret.</param>
        void BeginGame(int numberOfDifferentPegs, int numberOfPegsPerLine, int maxNumberOfGuesses);

        /// <summery>Called to get the next guess from the player.</summery>
        /// <returns>
        /// An array of peg-numbers representing a line.
        /// The valid peg-numbers and the number pf peg-numbers are given to <see cref="BeginGame"/>
        /// </returns>
        int[] GetGuess();

        /// <summery>Called after each call to <see cref="GetGuess"/> to provide the result of the guess.</summery>
        /// <param name="correctColorAndCorrectPosition">The number of pins in the guess that was of the correct color and at the correct position in the line.</param>
        /// <param name="corectColorWrongAndWrongPosition">The number of pins in the guess that was of the correct color but not at the correct position in the line.</param>
        void ResultFromPreviousGuess(int correctColorAndCorrectPosition, int corectColorWrongAndWrongPosition);

        /// <summery>Called at the end of each game. That is when the maximum number of guesses are used or the secret is guessed.</summery>
        /// <param name="wasTheSecretGuessed">Tells if the secret was guessed within the allowed number of guesses.</param>
        /// <param name="numberOfGuesses">The number of guesses used.</param>
        /// <param name="secret">The secret that the player was supposed to guess.</param>
        void EndGame(bool wasTheSecretGuessed, int numberOfGuesses, int[] secret);
    }
}