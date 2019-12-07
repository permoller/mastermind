using System;
using System.Globalization;

namespace Mastermind.PerformanceTestRunner
{
    public class Measurement
    {
        public Measurement(bool includeWhenPickingAWinner, string name, double value)
        {
            IncludeWhenPickingAWinner = includeWhenPickingAWinner;
            Name = name;
            Value = value;
        }

        public string Name { get; }

        /// <summary>Small values are considered better than larger values.</sumary>
        public double Value { get; }

        public bool IncludeWhenPickingAWinner { get; }

        public override string ToString()
        {
            return $"{IncludeWhenPickingAWinner}:{Name}:{Value.ToString(CultureInfo.InvariantCulture)}";
        }
        public static Measurement FromString(string str)
        {
            try
            {
                var split = str.Split(":", 3);
                return new Measurement(bool.Parse(split[0]), split[1], double.Parse(split[2], CultureInfo.InvariantCulture));
            }
            catch (FormatException e)
            {
                throw new FormatException("Could not parse measurement: " + str, e);
            }
        }
    }
}