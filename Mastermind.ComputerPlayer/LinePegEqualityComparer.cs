namespace Mastermind.ComputerPlayer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Mastermind.GameLogic;

    public class LinePegEqualityComparer : IEqualityComparer<Line>
    {
        public bool Equals([AllowNull] Line x, [AllowNull] Line y)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x is null || y is null)
                return false;
            return Enumerable.SequenceEqual(x.Pegs.Select(p => p.Number), y.Pegs.Select(p => p.Number));
        }

        public int GetHashCode([DisallowNull] Line obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            unchecked
            {
                var hashCode = 1320594601;
                foreach (var peg in obj.Pegs)
                {
                    hashCode = hashCode * -1521134295 + peg.Number.GetHashCode();
                }
                return hashCode;
            }
        }
    }
}