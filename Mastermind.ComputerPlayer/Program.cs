namespace Mastermind.ComputerPlayer
{
    using System;
    using System.Linq;
    using Mastermind.GameLogic;
    class Program
    {
        static void Main(string[] args)
        {
            //var game = new Game(8, 4, 10);
            //var player = new RandomGuessAmongPosibleSolutionsPlayer();
            foreach (var secret in LineGenerator.GenerateAllDifferentLines(6, 4))
            {
                var game = new Game(6, 4, 5, secret);
                var player = new FiveGuessAlgorithmPlayer();
                var result = game.Play(player);
                if (!result.WasTheSecretGuessed)
                    PrintGame(game, result);
                else if (secret.Pegs[0].Number == 0 && secret.Pegs[1].Number == 0 && secret.Pegs[2].Number == 0)
                    Console.WriteLine("Secret: " + string.Join(" ", result.Secret.Pegs.Select(p => p.Number)));
            }

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
