namespace Mastermind.PerformanceTestRunner
{
    using System;
    using System.Collections.Generic;
    using Mastermind.GameLogic;

    public class Arguments
    {
        public Arguments(string[] args)
        {
            var testsToPerform = new List<Type>();
            var playersToTest = new List<Type>();
            Seed = new Random().Next();
            Action<string> parser = null;

            foreach (var arg in args)
            {
                if (EqualsOrdinalIgnoreCase(arg, "--players") || EqualsOrdinalIgnoreCase(arg, "-p"))
                {
                    parser = a =>
                    {
                        if (EqualsOrdinalIgnoreCase(a, "all"))
                        {
                            playersToTest.AddRange(TypeResolver.GetAllTypes(typeof(IPlayer)));
                        }
                        else
                        {
                            playersToTest.AddRange(TypeResolver.GetTypeInAssemblies(a, typeof(IPlayer)));
                        }
                    };
                }
                else if (EqualsOrdinalIgnoreCase(arg, "--tests") || EqualsOrdinalIgnoreCase(arg, "-t"))
                {
                    parser = a =>
                    {
                        if (EqualsOrdinalIgnoreCase(a, "all"))
                        {
                            testsToPerform.AddRange(TypeResolver.GetAllTypes(typeof(IPerformanceTest)));
                        }
                        else
                        {
                            testsToPerform.AddRange(TypeResolver.GetTypeInAssemblies(a, typeof(IPerformanceTest)));
                        }
                    };
                }
                else if (EqualsOrdinalIgnoreCase(arg, "--seed") || EqualsOrdinalIgnoreCase(arg, "-s"))
                {
                    parser = a => { Seed = int.Parse(a); };
                }
                else if (parser is null)
                {
                    throw new ArgumentException($"'{arg}' not recognized as valid option. Example of valid options are: --players FiveGuess RandomGuess -tests all");
                }
                else
                {
                    try
                    {
                        parser(arg);
                    }
                    catch (FormatException e)
                    {
                        throw new ArgumentException($"Could not parse argument {arg}", e);
                    }
                }
            }
            TestsToPerform = testsToPerform;
            PlayersToTest = playersToTest;
        }

        private bool EqualsOrdinalIgnoreCase(string x, string y)
        {
            return string.Equals(x, y, StringComparison.OrdinalIgnoreCase);
        }
        public IReadOnlyList<Type> TestsToPerform { get; internal set; }
        public IReadOnlyList<Type> PlayersToTest { get; internal set; }
        public int Seed { get; internal set; }
    }
}