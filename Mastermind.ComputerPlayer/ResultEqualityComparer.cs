using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Mastermind.GameLogic;

namespace Mastermind.ComputerPlayer
{
    public class ResultEqualityComparer : IEqualityComparer<Result>
    {
        public bool Equals([AllowNull] Result x, [AllowNull] Result y)
        {
            return (x is null || y is null)
            ? x is null && y is null
            : x.NumberOfCorrectPins == y.NumberOfCorrectPins && x.NumberOfCorrectColoredPinsInWrongPosition == y.NumberOfCorrectColoredPinsInWrongPosition;
        }

        public int GetHashCode([DisallowNull] Result obj)
        {
            if (obj is null)
            {
                // throw exception as per the documentation for IEqualityComparer<T>.GetHashCode(T obj)
                throw new ArgumentNullException(nameof(obj));
            }
            // unchecked is default but it is here explicitly in case the code is ever executed in a chacked context
            unchecked
            {
                // the magic numbers are auto-generated by VS Code
                var hashCode = 1320594601;
                hashCode = hashCode * -1521134295 + obj.NumberOfCorrectPins.GetHashCode();
                hashCode = hashCode * -1521134295 + obj.NumberOfCorrectColoredPinsInWrongPosition.GetHashCode();
                return hashCode;
            }
        }
    }
}