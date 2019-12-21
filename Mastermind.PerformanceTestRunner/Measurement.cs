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
            return $"Measurement:{IncludeWhenPickingAWinner}:{Name}:{Value.ToString(CultureInfo.InvariantCulture)}";
        }

        public static bool TryParseMeasurement(string str, out Measurement measurement)
        {
            try
            {
                measurement = FromString(str);
                return true;
            }
            catch
            {
                measurement = null;
                return false;
            }
        }
        public static Measurement FromString(string str)
        {
            try
            {
                var split = str.Split(":", 4);
                return new Measurement(bool.Parse(split[1]), split[2], double.Parse(split[3], CultureInfo.InvariantCulture));
            }
            catch (FormatException e)
            {
                throw new FormatException("Could not parse measurement: " + str, e);
            }
        }
    }
}