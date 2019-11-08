namespace Mastermind.GameLogic
{
    using System.Collections.Generic;

    public class Line
    {
        public IReadOnlyList<Pin> Pins { get; }

        public Line(params Pin[] pins)
        {
            Pins = pins;
        }
    }
}
