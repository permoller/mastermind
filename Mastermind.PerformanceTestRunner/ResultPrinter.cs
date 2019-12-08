namespace Mastermind.PerformanceTestRunner
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    internal class ResultPrinter
    {

        private readonly int _NameColumnWidth = 0;
        private readonly int _IntegerValueColumnWidth = 0;
        private readonly IReadOnlyCollection<Result> _Results;

        public ResultPrinter(IReadOnlyCollection<Result> results)
        {
            _Results = results;
            _NameColumnWidth = Math.Max(results.Max(t => t.PlayerName.Length), results.Max(r => r.Unit != null ? r.Name.Length : 0));
            _IntegerValueColumnWidth = results.Max(r => ((int)r.Value).ToString(CultureInfo.CurrentCulture).Length);
        }

        public void PrintPlayerRank()
        {
            var color = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                var playerNames = _Results
                    .Where(r => r.IncludeWhenPickingAWinner)
                    .Select(r => r.PlayerName)
                    .Distinct()
                    .ToList();

                var playerScores = playerNames.Select(p => new
                {
                    Name = p,
                    Score = new int[playerNames.Count]
                }).ToList();
                foreach (var player in playerScores)
                {
                    foreach (var results in _Results.Where(r => r.IncludeWhenPickingAWinner).GroupBy(r => r.Name))
                    {
                        var result = results.FirstOrDefault(r => r.PlayerName == player.Name);
                        if (result != null)
                        {
                            var numberOfBetterPlayers = results.Where(r => r.Value < result.Value).Count();
                            player.Score[numberOfBetterPlayers]++;
                        }
                    }
                }

                var playersOrdered = playerScores.OrderByDescending(p => p.Score[0]);
                for (int i = 1; i < playerNames.Count; i++)
                {
                    var index = i;
                    playersOrdered = playersOrdered.ThenByDescending(p => p.Score[index]);
                }

                PrintHeader("Number of 1st places", "1st|2nd|...");
                foreach (var player in playersOrdered)
                {
                    PrintColumns(player.Name, string.Join("|", player.Score.Select(s => s.ToString())));
                }
            }
            finally
            {
                Console.ForegroundColor = color;
            }
        }

        public void PrintResultsPerTest()
        {
            var color = Console.ForegroundColor;
            try
            {
                foreach (var resultsForSameMeasurement in _Results.GroupBy(r => new { Name = r.Name, Unit = r.Unit }))
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

        private void PrintHeader(string header, string unit = null)
        {
            var line = "".PadLeft(_NameColumnWidth + 1 /* space */ + _IntegerValueColumnWidth + 1 /*decimal seperator*/ + 15 /* decimals */, '-');
            Console.WriteLine();
            Console.WriteLine(line);
            PrintColumns(header, unit);
            Console.WriteLine(line);
        }
        private void PrintResult(string playerName, double value)
        {
            PrintColumns(playerName, value.ToString(CultureInfo.CurrentCulture));
        }
        private void PrintColumns(string name, string value)
        {
            var seperator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var seperatorIndex = value?.IndexOf(seperator);
            if (seperatorIndex.HasValue && seperatorIndex.Value > 0)
            {
                value = value.PadLeft(value.Length + _IntegerValueColumnWidth - seperatorIndex.Value);
            }
            else
            {
                value = value?.PadLeft(_IntegerValueColumnWidth);
            }
            Console.WriteLine($"{name.PadRight(_NameColumnWidth)} {value}");
        }
    }
}