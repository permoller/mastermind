namespace Mastermind
{
    using Mastermind.GameLogic;
    using System;
    using System.Collections.Generic;

    public class Program
    {
        public static void Main()
        {
            var game = new Game(8, 4);
            Result result;
            Line guess;
            do
            {
                guess = ReadLine(game);
                result = game.Guess(guess);
                WriteResult(result);
            } while (!game.AreAllPinsCorrect(result));
            Console.Write("Number of guesses: ");
            Console.WriteLine(game.GuessesAndResults.Count);
        }
        private static void WriteResult(Result result)
        {
            Console.Write(" | ");
            Console.Write(result.NumberOfCorrectPins);
            Console.WriteLine(" (" + result.NumberOfCorrectColoredPinsInWrongPosition + ")");
        }
        private static Line ReadLine(Game game)
        {
            Console.Write("Guess:");
            var pins = new List<Pin>();
            for (var i = 0; i < game.NumberOfPinsPerLine; i++)
            {
                pins.Add(ReadPin(game));
            }
            return new Line(pins.ToArray());
        }
        private static Pin ReadPin(Game game)
        {
            Console.Write(" ");
            while (true)
            {
                var keyInfo = Console.ReadKey();
                var c = keyInfo.KeyChar;
                if (int.TryParse(c.ToString(), out var number))
                {
                    if (number >= 0 && number < game.NumberOfPins)
                    {
                        return new Pin(number);
                    }
                }
                Console.CursorLeft--;
            }
        }
    }
}
