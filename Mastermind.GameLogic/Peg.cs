namespace Mastermind.GameLogic
{
    public class Peg
    {
        public int Number { get; }
        public Peg(int number)
        {
            Number = number;
        }

        public static implicit operator Peg(int number)
        {
            return new Peg(number);
        }
    }
}