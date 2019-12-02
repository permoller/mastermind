namespace Mastermind.GameLogic
{
    public interface IPlayer
    {
        /// <summary>Called once at the beginning of each game.
        /// <para>The same player instance may be reused for multiple games, but only one game at the time.</para>
        /// </summary>
        /// <param name="numberOfDifferentPegs">The valid pegs in the game are numbered from 0 to one less than the number of different pegs.</param>
        /// <param name="numberOfPegsPerLine">The number of pegs used in a line in the game.</param>
        /// <param name="maxNumberOfGuesses">The number of guesses the player has to guess the secret.</param>
        void BeginGame(int numberOfDifferentPegs, int numberOfPegsPerLine, int maxNumberOfGuesses);

        /// <summary>Called to get the next guess from the player.</summary>
        /// <returns>An array of peg-numbers representing a line.
        /// <para>The valid peg-numbers and the number of peg per line are given to <see cref="BeginGame"/></para></returns>
        int[] GetGuess();

        /// <summary>Called after each call to <see cref="GetGuess"/> to provide the result of the guess.</summary>
        /// <param name="numberOfCorrectsPegs">The number of pegs in the guess that have the same peg at the same position in the secret line.</param>
        /// <param name="numberOfPegsAtWrongPosition">The number of pegs in the guess that have the same peg at a different position in the secret line.
        /// <para>A peg can only count once in the result. If the same peg is used twice in the guess and is only used once in the secret only one of the pegs in the guess will be considered to have a match in the secret.</para></param>
        void ResultFromPreviousGuess(int numberOfCorrectsPegs, int numberOfPegsAtWrongPosition);

        /// <summary>Called at the end of each game.
        /// <para>The end is when the maximum number of guesses are used or the secret is guessed.</para></summary>
        /// <param name="wasTheSecretGuessed">Tells if the secret was guessed within the allowed number of guesses.</param>
        /// <param name="numberOfGuesses">The number of guesses used.</param>
        /// <param name="secret">The secret that the player was supposed to guess.</param>
        void EndGame(bool wasTheSecretGuessed, int numberOfGuesses, int[] secret);
    }
}