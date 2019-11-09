namespace Mastermind.GameLogic
{
    public interface ILineComparer
    {
        Result Compare(Line guess, Line secret);
    }
}