namespace Mastermind.ConsoleApp
{
    using System.Collections.Generic;
    using System.Linq;
    using Mastermind.GameLogic;

    internal class ConsolePlayer : Player
    {
        private readonly IConsole _Console;

        public ConsolePlayer(IConsole console)
        {
            _Console = console;
        }

        public override void BeginGame(IGame game)
        {
            _Console.WriteLine($"The valid pins are from 0 to {game.NumberOfPins - 1}");
        }

        public override void EndGame(IGame game, GamePlayResult result)
        {
            PrintLastResult(game);
            if (result.WasTheSecretGuessed)
            {
                _Console.WriteLine($"Game completed - The secret was guessed in {game.GuessesAndResults.Count} tries");
            }
            else
            {
                _Console.WriteLine($"Game over - The secret was not guessed in {game.MaxNumberOfGuesses} tries");
                _Console.WriteLine($"The secret was: {string.Join(" ", result.Secret.Pins.Select(p => p.Number))}");
            }
        }

        public override Line GetGuess(IGame game)
        {
            PrintLastResult(game);
            return ReadLine(game);
        }

        public void PrintLastResult(IGame game)
        {
            if (game.GuessesAndResults.Count > 0)
            {
                var result = game.GuessesAndResults.Last().Result;
                _Console.WriteLine($" | Correct: {result.NumberOfCorrectPins} | Wrong position: {result.NumberOfCorrectColoredPinsInWrongPosition}");
            }
        }

        private Line ReadLine(IGame game)
        {
            _Console.Write("Guess:");
            var pins = new List<Pin>();
            for (var i = 0; i < game.NumberOfPinsPerLine; i++)
            {
                pins.Add(ReadPin(game));
            }
            return new Line(pins.ToArray());
        }
        private Pin ReadPin(IGame game)
        {
            _Console.Write(" ");
            var top = _Console.CursorTop;
            var left = _Console.CursorLeft;
            while (true)
            {

                var keyInfo = _Console.ReadKey();
                var c = keyInfo.KeyChar;
                if (int.TryParse(c.ToString(), out var number))
                {
                    if (number >= 0 && number < game.NumberOfPins)
                    {
                        return new Pin(number);
                    }
                }
                _Console.SetCursorPosition(left, top);
            }
        }
    }
}