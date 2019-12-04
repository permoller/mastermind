namespace Mastermind.PerformanceTestRunner
{
    using System.Collections.Generic;
    using Mastermind.GameLogic;
    public interface IPerformanceTest
    {
        /// <summary>Performs the test with the given <paramref name="player"/> and returns a list of test results.
        /// <para>The test is allowed to use random data as part of the test, but if the test is performed twice with the same <paramref name="seed"/> the data used for testing must be the same to make the results comparable.</para></summary>
        /// <param name="player">The player to test</param>
        /// <param name="seed">The seed to use to initialize <see cref="System.Random"/></param>
        /// <returns>A list of the data that is collected by the test.</returns>
        IEnumerable<Measurement> ExecuteTest(IPlayer player, int seed);
    }
}