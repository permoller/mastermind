namespace Mastermind.PerformanceTestRunner
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
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
                var printer = new ResultPrinter(results);
                printer.PrintResultsPerTest();
                printer.PrintPlayerRank();
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
