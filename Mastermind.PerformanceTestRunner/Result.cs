namespace Mastermind.PerformanceTestRunner
{
    using System;
    using System.Text.RegularExpressions;

    internal class Result
    {
        public Result(Type testType, Type pleyerType, Measurement measurement)
        {
            PlayerName = pleyerType.Name;
            Value = measurement.Value;

            IncludeWhenPickingAWinner = measurement.IncludeWhenPickingAWinner;
            var m = Regex.Match(measurement.Name, "^(.+) *\\((.+)\\)$");
            var measurementName = measurement.Name;
            if (m.Success)
            {
                measurementName = m.Groups[1].Value;
                Unit = m.Groups[2].Value;
            }
            Name = $"{testType.Name} - {measurementName}";
        }

        public string Name { get; }
        public string PlayerName { get; }
        public double Value { get; }
        public string Unit { get; }
        public bool IncludeWhenPickingAWinner { get; }
    }
}
