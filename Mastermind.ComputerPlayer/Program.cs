namespace Mastermind.ComputerPlayer
{
    using System;
    using System.Linq;
    using Mastermind.GameLogic;
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game(8, 4, 10);
            var player = new RandomGuessAmongPosibleSolutionsPlayer();
            var result = game.Play(player);
            PrintGame(game, result);
        }

        private static void PrintGame(Game game, GamePlayResult result)
        {
            foreach (var guessAndResult in game.GuessesAndResults)
            {
                Console.WriteLine("Guess: " + string.Join(" ", guessAndResult.Guess.Pegs.Select(p => p.Number)) + " | Correct: " + guessAndResult.Result.NumberOfCorrectPegs + " | Wrong position: " + guessAndResult.Result.NumberOfCorrectColoredPegsInWrongPosition);
            }
            Console.WriteLine("Secret: " + string.Join(" ", result.Secret.Pegs.Select(p => p.Number)));
            Console.WriteLine("Was secret guessed: " + result.WasTheSecretGuessed);

        }
    }
}
