using System.Globalization;

namespace Mastermind.ComputerPlayer
{
    public class Measurement
    {
        public Measurement(string name, double value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        /// <summary>Small values are considered better than larger values.</sumary>
        public double Value { get; }

        public override string ToString()
        {
            return $"{Name}:{Value.ToString(CultureInfo.InvariantCulture)}";
        }
        public static Measurement FromString(string str)
        {
            var split = str.Split(":", 2);
            return new Measurement(split[0], double.Parse(split[1], CultureInfo.InvariantCulture));
        }
    }
}