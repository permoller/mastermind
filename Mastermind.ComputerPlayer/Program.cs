namespace Mastermind.ComputerPlayer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Mastermind.GameLogic;
    class Program
    {

        static void Main(string[] args)
        {
            _Stopwatch.Start();
            string mastermindDirectory = GetMastermindDirectory();
            var playerTypes = GetPlayerTypes(mastermindDirectory, args);

            if (playerTypes.Count == 1)
            {
                var player = (IPlayer)Activator.CreateInstance(playerTypes.Single());
                var random = new Random();
                var games = GenerateAllGames(6, 4, new int[0]);
                var results = new List<Tuple<GamePlayResult, TimeSpan>>((int)Math.Pow(6, 4));

                PrintPlayer(player);
                _Stopwatch.Stop();
                var initializationTicks = _Stopwatch.ElapsedTicks;
                foreach (var game in games)
                {
                    _Stopwatch.Restart();
                    var result = game.Play(player);
                    _Stopwatch.Stop();
                    results.Add(new Tuple<GamePlayResult, TimeSpan>(result, _Stopwatch.Elapsed));
                    if (result.Secret[2] == 0 && result.Secret[3] == 0)
                    {
                        Console.Write(" " + string.Join(" ", result.Secret) + "\r");
                    }
                    //PrintGameResult(result);
                }
                Console.WriteLine(" " + string.Join(" ", results.Last().Item1.Secret));

                PrintPerformanceCounters();
                Console.WriteLine($"Initialization: {FormatTicks(initializationTicks)}");
                PrintResults(results);
            }
            else
            {
                foreach (var playerType in playerTypes)
                {
                    var fileName = Process.GetCurrentProcess().MainModule.FileName;
                    var p = Process.Start(fileName, Path.GetFileName(playerType.Assembly.Location) + " " + playerType.FullName);
                    p.WaitForExit();
                }
            }
        }

        private static IEnumerable<Game> GenerateAllGames(int numberOfDifferentPegs, int remainingNumberOfPegsInLine, IEnumerable<int> pegs)
        {
            if (remainingNumberOfPegsInLine == 0)
            {
                var line = pegs.ToArray();
                yield return new Game(numberOfDifferentPegs, line.Length, 10, line);
            }
            else
            {
                for (int peg = 0; peg < numberOfDifferentPegs; peg++)
                {
                    var games = GenerateAllGames(numberOfDifferentPegs, remainingNumberOfPegsInLine - 1, pegs.Append(peg));
                    foreach (var game in games)
                    {
                        yield return game;
                    }
                }
            }
        }

        private static void PrintPerformanceCounters()
        {
            Console.WriteLine();
            var p = Process.GetCurrentProcess();
            var maxMemoryUsageBytes = p.PeakWorkingSet64;
            var maxMemoryUsageMB = maxMemoryUsageBytes / 1024m / 1024m;
            Console.WriteLine($"Peak memory usage: {maxMemoryUsageMB:N2} MB ({maxMemoryUsageBytes:N0} Bytes)");
            var duration = DateTime.Now - p.StartTime;
            Console.WriteLine($"Total duration: {duration.TotalSeconds:N3} s ({duration.Ticks} ticks)");
        }

        private static IReadOnlyCollection<Type> GetPlayerTypes(string mastermindDirectory, string[] playerNames)
        {
            List<Type> playerTypes = new List<Type>();
            Console.WriteLine();
            Console.WriteLine($"Scanning for players in {mastermindDirectory}");
            Console.WriteLine();
            var dllFileNamePattern = "Mastermind.Algorithms.*.dll";
            if (playerNames.Length > 0 && Regex.IsMatch(playerNames[0], "Mastermind\\.Algorithms\\..+\\.dll"))
            {
                dllFileNamePattern = playerNames[0];
                playerNames = playerNames.Skip(1).ToArray();
            }
            var dllFiles = Directory.GetFiles(mastermindDirectory, dllFileNamePattern, SearchOption.AllDirectories);
            foreach (var dllFile in dllFiles)
            {
                var dllFileName = Path.GetFileName(dllFile);
                var assembly = Assembly.LoadFrom(dllFile);
                playerTypes.AddRange(
                    assembly
                    .GetExportedTypes()
                    .Where(t =>
                        t.GetInterfaces().Contains(typeof(IPlayer)) &&
                        !t.IsAbstract &&
                        !t.IsGenericType &&
                        t.GetConstructor(new Type[0]) != null)
                    .Select(t => t));

            }
            playerTypes = playerTypes.Distinct().ToList();
            for (int i = 0; i < playerTypes.Count; i++)
            {
                Console.WriteLine(i + "\t" + playerTypes[i].Name);
                Console.WriteLine();
            }
            Console.WriteLine();

            if (playerNames != null && playerNames.Any())
            {
                if (!playerNames[0].Equals("all", StringComparison.OrdinalIgnoreCase))
                {
                    playerTypes = playerNames.Select(n =>
                    int.TryParse(n, out var index) ? playerTypes[index] :
                    playerTypes.FirstOrDefault(t => t.FullName.Equals(n, StringComparison.OrdinalIgnoreCase)) ??
                    playerTypes.FirstOrDefault(t => t.Name.Equals(n, StringComparison.OrdinalIgnoreCase)) ??
                    playerTypes.FirstOrDefault(t => t.FullName.Contains(n, StringComparison.OrdinalIgnoreCase)) ??
                    throw new Exception($"Player {n} not found.")).ToList();
                }
            }
            else
            {
                Console.Write("Enter the numbers of the players to use (or nothing to use all): ");
                var playerNumbers = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(playerNumbers))
                {
                    playerTypes = playerNumbers
                        .Split(new char[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => int.Parse(s.Trim()))
                        .Select(i => playerTypes[i])
                        .ToList();
                }
            }
            return playerTypes;
        }

        private static string GetMastermindDirectory()
        {
            var mastermindDirectory = string.Join(
                            Path.DirectorySeparatorChar,
                            Directory
                                .GetCurrentDirectory()
                                .Split(Path.DirectorySeparatorChar)
                                .Reverse()
                                .SkipWhile(directoryName => !"Mastermind".Equals(directoryName, StringComparison.OrdinalIgnoreCase))
                                .Reverse());
            if (string.IsNullOrWhiteSpace(mastermindDirectory))
            {
                mastermindDirectory = Directory.GetCurrentDirectory();
            }
            return mastermindDirectory;
        }

        private static Stopwatch _Stopwatch = new Stopwatch();

        private static void PrintPlayer(IPlayer player)
        {
            var line = "---------------------------------------------";
            Console.WriteLine(line);
            Console.WriteLine(player.GetType().Name);
            Console.WriteLine(line);
        }

        private static void PrintGameResult(GamePlayResult result)
        {
            Console.WriteLine();
            foreach (var guessAndResult in result.GuessesAndResults)
            {
                Console.WriteLine("Guess: " +
                    string.Join(" ", guessAndResult.Guess) +
                    " | Correct: " +
                    guessAndResult.Result.NumberOfCorrectPegs +
                    " | Wrong position: " +
                    guessAndResult.Result.NumberOfPegsAtWrongPosition);
            }
            Console.WriteLine($"Secret: {string.Join(" ", result.Secret)}");
            Console.WriteLine($"Was secret guessed: {result.WasTheSecretGuessed}");
            Console.WriteLine($"Duration: {FormatTicks(_Stopwatch.ElapsedTicks)}");
        }

        private static void PrintResults(IReadOnlyList<Tuple<GamePlayResult, TimeSpan>> results)
        {
            Console.WriteLine($"Game count (win/loose): {results.Count(r => r.Item1.WasTheSecretGuessed)} / {results.Count(r => !r.Item1.WasTheSecretGuessed)}");
            Console.WriteLine($"Guesses per game (min/max/avarage): {results.Min(r => r.Item1.GuessesAndResults.Count)} / {results.Max(r => r.Item1.GuessesAndResults.Count)} / {results.Average(r => r.Item1.GuessesAndResults.Count)}");
            Console.WriteLine($"Game duration (first/min/max/avarage): {FormatTicks(results.First().Item2.Ticks)} / {FormatTicks(results.Min(r => r.Item2.Ticks))} / {FormatTicks(results.Max(r => r.Item2.Ticks))} / {FormatTicks((long)Math.Round(results.Average(r => r.Item2.Ticks), 0))}");
        }
        private static string FormatTicks(long ticks)
        {
            return $"{new TimeSpan(ticks).TotalSeconds:N3} s ({ticks} ticks)";
        }
    }
}
