﻿namespace Mastermind.PerformanceTest.AllGames4Pins4PerLine
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Mastermind.PerformanceTestRunner;
    using Mastermind.GameLogic;
    public class AllGames4Pins4PerLine : IPerformanceTest
    {
        public IEnumerable<Measurement> ExecuteTest(IPlayer player, int seed)
        {
            var swTotal = new Stopwatch();
            swTotal.Start();
            var random = new Random(seed);
            var games = GenerateAllGames().OrderBy(g => random.Next()).ToList();
            var sw = new Stopwatch();
            var sumNumberOfGuesses = 0L;
            var maxNumberOfGuesses = 0L;
            var minNumberOfGuesses = long.MaxValue;
            var numberOfLostGames = 0;
            TimeSpan? first = null;
            foreach (var game in games)
            {
                sw.Start();
                var result = game.Play(player);
                sw.Stop();
                if (first is null)
                {
                    first = sw.Elapsed;
                }
                var numberOfGuesses = result.GuessesAndResults.Count;
                sumNumberOfGuesses += numberOfGuesses;
                maxNumberOfGuesses = Math.Max(numberOfGuesses, maxNumberOfGuesses);
                minNumberOfGuesses = Math.Min(numberOfGuesses, minNumberOfGuesses);
                if (!result.WasTheSecretGuessed)
                    numberOfLostGames++;
            }

            swTotal.Stop();
            var process = Process.GetCurrentProcess();
            yield return new Measurement(false, "Total duration (ms)", swTotal.Elapsed.TotalMilliseconds);
            yield return new Measurement(false, "Total duration games only (ms)", sw.Elapsed.TotalMilliseconds);
            yield return new Measurement(false, "Duration first game (ms)", first.Value.TotalMilliseconds);
            yield return new Measurement(false, "Avarage duration per game (ms)", sw.Elapsed.TotalMilliseconds / games.Count);
            yield return new Measurement(false, "Minimum number of guesses per game", minNumberOfGuesses);
            yield return new Measurement(false, "Number of lost games", numberOfLostGames);

            yield return new Measurement(numberOfLostGames == 0, "Peak working set memory (MB)", (((double)process.PeakWorkingSet64) / 1024) / 1024);
            yield return new Measurement(numberOfLostGames == 0, "Total CPU time (ms)", process.TotalProcessorTime.TotalMilliseconds);
            yield return new Measurement(numberOfLostGames == 0, "Avarage number of guesses per game", ((double)sumNumberOfGuesses) / games.Count);
            yield return new Measurement(numberOfLostGames == 0, "Maximum number of guesses per game", maxNumberOfGuesses);

        }

        private IEnumerable<Game> GenerateAllGames()
        {
            var numberOfDifferentPins = 4;
            for (var p0 = 0; p0 < numberOfDifferentPins; p0++)
                for (var p1 = 0; p1 < numberOfDifferentPins; p1++)
                    for (var p2 = 0; p2 < numberOfDifferentPins; p2++)
                        for (var p3 = 0; p3 < numberOfDifferentPins; p3++)
                            yield return new Game(numberOfDifferentPins, 4, 10, new int[] { p0, p1, p2, p3 });
        }
    }
}
