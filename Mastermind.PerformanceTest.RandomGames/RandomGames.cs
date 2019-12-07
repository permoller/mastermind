namespace Mastermind.PerformanceTest.RandomGames
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Mastermind.PerformanceTestRunner;
    using Mastermind.GameLogic;
    public class RandomGames : IPerformanceTest
    {
        public IEnumerable<Measurement> ExecuteTest(IPlayer player, int seed)
        {
            var swTotal = new Stopwatch();
            swTotal.Start();
            var games = GenerateRandomGames(seed).ToList();
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

        private IEnumerable<Game> GenerateRandomGames(int seed)
        {
            var random = new Random(seed);
            for (int i = 0; i < 100; i++)
            {
                var numberOfPinsPerLine = random.Next(2, 6);
                var numberOfDifferentPins = random.Next(1, 12 - numberOfPinsPerLine);
                yield return new Game(numberOfDifferentPins, numberOfPinsPerLine, 10, Enumerable.Range(0, numberOfPinsPerLine).Select(p => random.Next(0, numberOfDifferentPins)).ToArray());
            }
        }
    }
}
