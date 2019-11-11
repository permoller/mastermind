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
            _Console.WriteLine($"The valid pegs are from 0 to {game.NumberOfPegs - 1}");
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
                _Console.WriteLine($"The secret was: {string.Join(" ", result.Secret.Pegs.Select(p => p.Number))}");
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
                _Console.WriteLine($" | Correct: {result.NumberOfCorrectPegs} | Wrong position: {result.NumberOfCorrectColoredPegsInWrongPosition}");
            }
        }

        private Line ReadLine(IGame game)
        {
            _Console.Write("Guess:");
            var pegs = new List<Peg>();
            for (var i = 0; i < game.NumberOfPegsPerLine; i++)
            {
                pegs.Add(ReadPeg(game));
            }
            return new Line(pegs.ToArray());
        }
        private Peg ReadPeg(IGame game)
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
                    if (number >= 0 && number < game.NumberOfPegs)
                    {
                        return new Peg(number);
                    }
                }
                _Console.SetCursorPosition(left, top);
            }
        }
    }
}