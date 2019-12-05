namespace Mastermind.PerformanceTestRunner
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Mastermind.GameLogic;
    class Program
    {
        public static void Main(string[] args)
        {
            var arguments = new Arguments(args);
            if (arguments.TestsToPerform.Count == 1 && arguments.PlayersToTest.Count == 1)
            {
                TestPlayer(arguments.TestsToPerform[0], arguments.PlayersToTest[0], arguments.Seed);
            }
            else
            {
                if (arguments.PlayersToTest.Count == 0)
                {
                    arguments.PlayersToTest = AskUserForPlayersToTest();
                }

                if (arguments.TestsToPerform.Count == 0)
                {
                    arguments.TestsToPerform = AskUserForTestsToPerform();
                }

                var measurementsPerTestPerPlayer = new Dictionary<Type, Dictionary<Type, IReadOnlyList<Measurement>>>();
                foreach (var testType in arguments.TestsToPerform)
                {
                    var measurementsPerPlayer = new Dictionary<Type, IReadOnlyList<Measurement>>();
                    foreach (var playerType in arguments.PlayersToTest)
                    {
                        Console.WriteLine($"{testType.Name} - {playerType.Name}");
                        var measurements = TestPlayerInNewProcess(testType, playerType, arguments.Seed);
                        measurementsPerPlayer[playerType] = measurements;
                    }
                    measurementsPerTestPerPlayer[testType] = measurementsPerPlayer;
                }
                PrintMeasurements(measurementsPerTestPerPlayer);
            }
        }

        private static void PrintMeasurements(Dictionary<Type, Dictionary<Type, IReadOnlyList<Measurement>>> measurementsPerTestPerPlayer)
        {
            var testTypeToMeasurementName = measurementsPerTestPerPlayer.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Values.SelectMany(mm => mm.Select(m => m.Name)).Distinct());
            var playerTypes = measurementsPerTestPerPlayer.SelectMany(kvp => kvp.Value.Keys).Distinct();
            var lengthOfLongestPlayerName = playerTypes.Select(t => t.Name.Length).Max();

            foreach (var kvp in measurementsPerTestPerPlayer)
            {
                var testType = kvp.Key;
                var measurementsPerPlayer = kvp.Value;
                var measurementNames = kvp.Value.Values.SelectMany(mm => mm.Select(m => m.Name)).Distinct();
                foreach (var name in measurementNames)
                {
                    Console.WriteLine();
                    Console.WriteLine("---------------------------------------------------------------------------");
                    Console.WriteLine($"{testType.Name} - {name}");
                    Console.WriteLine("---------------------------------------------------------------------------");
                    var measurementValuePerPlayer = measurementsPerPlayer.ToDictionary(kvp2 => kvp2.Key, kvp2 => kvp2.Value.FirstOrDefault(m => m.Name == name).Value);

                    foreach (var kvp2 in measurementValuePerPlayer.OrderBy(kvp2 => kvp2.Value))
                    {
                        var playerName = kvp2.Key.Name.PadRight(lengthOfLongestPlayerName);
                        var valueAsString = kvp2.Value.ToString(CultureInfo.CurrentCulture);
                        var seperator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                        var valueSplit = valueAsString.Split(seperator, 2);
                        valueAsString = valueSplit[0].PadLeft(10) + (valueSplit.Length == 1 ? "" : (seperator + valueSplit[1]));
                        Console.WriteLine($"\t{playerName} {valueAsString}");
                    }
                }
            }
        }

        private static IReadOnlyList<Type> AskUserForTestsToPerform()
        {
            return TypeResolver.GetAllTypes(typeof(IPerformanceTest));
        }

        private static IReadOnlyList<Type> AskUserForPlayersToTest()
        {
            return TypeResolver.GetAllTypes(typeof(IPlayer));
        }

        private static IReadOnlyList<Measurement> TestPlayerInNewProcess(Type testType, Type playerType, int seed)
        {
            var fileName = Process.GetCurrentProcess().MainModule.FileName;
            var testAssembly = testType.Assembly.Location;
            var playerAssembly = playerType.Assembly.Location;

            var p = new Process();
            p.StartInfo.FileName = fileName;
            p.StartInfo.Arguments = $"--players {playerAssembly} --tests {testAssembly} --seed {seed}";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            var output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output.Split(Environment.NewLine).Select(l => Measurement.FromString(l)).ToList();
        }

        private static void TestPlayer(Type testType, Type playerType, int seed)
        {
            var test = (IPerformanceTest)Activator.CreateInstance(testType);
            var player = (IPlayer)Activator.CreateInstance(playerType);
            var measurements = test.ExecuteTest(player, seed);
            Console.Write(string.Join(Environment.NewLine, measurements.Select(m => m.ToString())));
        }
    }
}
