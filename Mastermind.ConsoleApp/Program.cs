[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Mastermind.ConsoleApp.Tests")]
namespace Mastermind.ConsoleApp
{
    using Mastermind.GameLogic;
    using System;

    public class Program
    {
        public static void Main()
        {
            var numberOfPins = GetNumber("Number of different pins", 1, 9, 8);
            var numberOfPinsPerLine = GetNumber("Number of pins per line", 1, 9, 4);
            var maxNumberOfGuesses = GetNumber("Max number of guesses", 1, 100, 10);

            var game = new Game(numberOfPins, numberOfPinsPerLine, maxNumberOfGuesses);
            var player = new ConsolePlayer(new ConsoleWrapper());
            var victory = game.Play(player);
        }

        private static int GetNumber(string message, int min, int max, int @default)
        {
            Console.Write($"{message} ({min} - {max}) default is {@default}: ");
            if (!int.TryParse(Console.ReadLine(), out var number) || number < min || number > max)
            {
                Console.WriteLine("Using default value of " + @default);
                number = @default;
            }
            return number;
        }
    }
}
