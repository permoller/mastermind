namespace Mastermind.GameLogic
{
    public class Pin
    {
        public int Number { get; }
        public Pin(int number)
        {
            Number = number;
        }

        public static implicit operator Pin(int number)
        {
            return new Pin(number);
        }
    }
}