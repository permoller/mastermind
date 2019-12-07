namespace Mastermind.PerformanceTestRunner
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using Mastermind.GameLogic;

    class Program
    {
        private static int _NameColumnWidth = 0;
        private static int _IntegerValueColumnWidth = 0;
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


                Console.WriteLine($"Seed: {arguments.Seed}");
                var results = new List<Result>();
                foreach (var testType in arguments.TestsToPerform)
                {

                    foreach (var playerType in arguments.PlayersToTest)
                    {
                        Console.WriteLine($"{testType.Name} - {playerType.Name}");
                        var measurements = TestPlayerInNewProcess(testType, playerType, arguments.Seed);
                        results.AddRange(measurements.Select(m => new Result(testType, playerType, m)));
                    }
                }
                _NameColumnWidth = Math.Max(results.Max(t => t.PlayerName.Length), results.Max(r => r.Unit != null ? r.Name.Length : 0));
                _IntegerValueColumnWidth = results.Max(r => ((int)r.Value).ToString(CultureInfo.CurrentCulture).Length);
                PrintResultsPerTest(results);
                PrintPlayerRank(results);
            }
        }

        private static void PrintPlayerRank(IReadOnlyCollection<Result> results)
        {
            var color = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                var winsPerPlayerRanked = results
                    .Where(r => r.IncludeWhenPickingAWinner)
                    .GroupBy(r => r.Name)
                    .Select(g => g.OrderBy(r => r.Value).First().PlayerName)
                    .GroupBy(p => p)
                    .Select(g => new { PlayerName = g.Key, NumberOfWins = g.Count() })
                    .OrderByDescending(x => x.NumberOfWins);
                PrintHeader("Winner of the most categories", "(number of wins)");
                foreach (var player in winsPerPlayerRanked)
                {
                    PrintResult(player.PlayerName, player.NumberOfWins);
                }
            }
            finally
            {
                Console.ForegroundColor = color;
            }
        }

        private static void PrintResultsPerTest(IReadOnlyCollection<Result> results)
        {
            var color = Console.ForegroundColor;
            try
            {
                foreach (var resultsForSameMeasurement in results.GroupBy(r => new { Name = r.Name, Unit = r.Unit }))
                {
                    Console.ForegroundColor = resultsForSameMeasurement.All(r => r.IncludeWhenPickingAWinner) ? ConsoleColor.Gray : ConsoleColor.DarkGray;
                    PrintHeader(resultsForSameMeasurement.Key.Name, resultsForSameMeasurement.Key.Unit);
                    foreach (var result in resultsForSameMeasurement.OrderBy(r => r.Value))
                    {
                        Console.ForegroundColor = result.IncludeWhenPickingAWinner ? ConsoleColor.Gray : ConsoleColor.DarkGray;
                        PrintResult(result.PlayerName, result.Value);
                    }
                }
            }
            finally
            {
                Console.ForegroundColor = color;
            }
        }

        private static void PrintHeader(string header, string unit = null)
        {
            if (unit != null)
            {
                header = $"{header.PadRight(_NameColumnWidth)} {unit.PadLeft(_IntegerValueColumnWidth)}";
            }
            var line = "".PadLeft(_NameColumnWidth + 1 /* space */ + _IntegerValueColumnWidth + 1 /*decimal seperator*/ + 15 /* decimals */, '-');
            Console.WriteLine();
            Console.WriteLine(line);
            Console.WriteLine(header);
            Console.WriteLine(line);
        }
        private static void PrintResult(string playerName, double value)
        {
            var valueAsString = value.ToString(CultureInfo.CurrentCulture);
            var seperator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var valueSplit = valueAsString.Split(seperator, 2);
            valueAsString = valueSplit[0].PadLeft(_IntegerValueColumnWidth) + (valueSplit.Length == 1 ? "" : (seperator + valueSplit[1]));

            Console.WriteLine($"{playerName.PadRight(_NameColumnWidth)} {valueAsString}");
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
