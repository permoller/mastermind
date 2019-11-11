namespace Mastermind.GameLogic
{
    using System.Collections.Generic;

    public class Line
    {
        public IReadOnlyList<Peg> Pegs { get; }

        public Line(params Peg[] pegs)
        {
            Pegs = pegs;
        }
    }
}
