namespace Mastermind.PerformanceTest.SingleGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mastermind.ComputerPlayer;
    using Mastermind.GameLogic;
    public class SingleGameTest : IPerformanceTest
    {
        public ComparisonResult CompareMeasurements(string name, string value1, string value2)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Measurement> ExecuteTest(IPlayer player, int seed)
        {
            var random = new Random(seed);
            var game = new Game(6, 4, 10, Enumerable.Range(0, 4).Select(_ => random.Next(6)).ToArray());
            var result = game.Play(player);
            yield return new Measurement("Game", string.Join(",", result.Secret));
        }
    }
}
