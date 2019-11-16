namespace Mastermind.ComputerPlayer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Mastermind.GameLogic;
    class Program
    {
        static void Main(string[] args)
        {
            string mastermindDirectory = GetMastermindDirectory();
            var players = GetPlayers(mastermindDirectory);

            var random = new Random();
            var gamesToTestFunctionalityOfPlayer = new Game[]{
                // small size game (4 different lines) with many of tries - normally player will win
                new Game(2, 2, 20),
                // medium size game (1296 different lines) with few tries - normally player will lose
                new Game(6, 4, 2),
                // random game small to medium size game
                new Game(random.Next(2,6), random.Next(2,4), random.Next(2,10)),
                // large size game (32768 different lines) with many tries
                new Game(8,5,20)
            };

            foreach (var player in players)
            {
                PrintPlayer(player);
                foreach (var game in gamesToTestFunctionalityOfPlayer)
                {
                    var result = game.Play(player);
                    PrintGame(game, result);
                }
            }
        }

        private static IReadOnlyCollection<IPlayer> GetPlayers(string mastermindDirectory)
        {
            List<Type> playerTypes = new List<Type>();
            Console.WriteLine();
            Console.WriteLine("Scanning for players in " + mastermindDirectory);
            Console.WriteLine();
            var dllFiles = Directory.GetFiles(mastermindDirectory, "*.dll", SearchOption.AllDirectories);
            foreach (var dllFile in dllFiles)
            {

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
            Console.Write("Enter the numbers of the players to use (or nothing to use all): ");
            var playerNumbers = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(playerNumbers))
                return playerTypes.Select(t => (IPlayer)Activator.CreateInstance(t)).ToList();
            return playerNumbers
                .Split(new char[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.Parse(s.Trim()))
                .Select(i => playerTypes[i])
                .Select(t => (IPlayer)Activator.CreateInstance(t))
                .ToList();
        }

        private static string GetMastermindDirectory()
        {
            var mastermindDirectory = string.Join(
                            Path.DirectorySeparatorChar,
                            Directory
                                .GetCurrentDirectory()
                                .Split(Path.DirectorySeparatorChar)
                                .Reverse()
                                .SkipWhile(directoryName => !StringComparer.OrdinalIgnoreCase.Equals(directoryName, "Mastermind"))
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
            _Stopwatch.Restart();
        }

        private static void PrintGame(Game game, GamePlayResult result)
        {
            _Stopwatch.Stop();
            Console.WriteLine();
            foreach (var guessAndResult in game.GuessesAndResults)
            {
                Console.WriteLine("Guess: " + string.Join(" ", guessAndResult.Guess.Pegs.Select(p => p.Number)) + " | Correct: " + guessAndResult.Result.NumberOfPegsWithCorrectColorAndCorrectPosition + " | Wrong position: " + guessAndResult.Result.NumberOfPegsWithCorrectColorAndWrongPosition);
            }
            Console.WriteLine("Secret: " + string.Join(" ", result.Secret.Pegs.Select(p => p.Number)));
            Console.WriteLine("Was secret guessed: " + result.WasTheSecretGuessed);
            Console.WriteLine("Duration in ms: " + _Stopwatch.ElapsedMilliseconds);
            _Stopwatch.Restart();
        }
    }
}
