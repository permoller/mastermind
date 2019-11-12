namespace Mastermind.ComputerPlayer.Tests
{
    using System.Linq;
    using Mastermind.GameLogic;
    internal static class LineParser
    {
        public static Line ParseLine(string lineAsString)
        {
            return new Line(lineAsString.Split(" ").Select(pegNumber => (Peg)int.Parse(pegNumber)).ToArray());
        }
    }
}