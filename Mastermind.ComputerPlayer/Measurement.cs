namespace Mastermind.ComputerPlayer
{
    public class Measurement
    {
        public Measurement(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }

        public override string ToString()
        {
            return $"{Name}:{Value}";
        }
        public static Measurement FromString(string str)
        {
            var split = str.Split(":", 2);
            return new Measurement(split[0], split[1]);
        }
    }
}