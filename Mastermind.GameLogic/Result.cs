namespace Mastermind.GameLogic
{
    public class Result
    {
        /// <summery>
        /// The number of pegs in the guess that have the same peg at the same position in the secret line.
        /// </summery>
        public int NumberOfCorrectPegs { get; }

        /// <summery>
        /// The number of pegs in the guess that have the same peg at a different position in the secret line.
        /// A peg can only count once in the result.
        /// If the same peg is used twice in the guess and is only used once in the secret only one of the pegs in the guess will be considered to have a match in the secret.
        /// </summery>
        public int NumberOfPegsAtWrongPosition { get; }

        internal Result(int correct, int wrongPosition)
        {
            NumberOfCorrectPegs = correct;
            NumberOfPegsAtWrongPosition = wrongPosition;
        }
    }
}
