namespace Mastermind.HumanPlayer
{
    using Mastermind.GameLogic;

    internal class ConsolePlayer : IPlayer
    {
        private readonly IConsole _Console;
        private int _NumberOfDifferentPegs;
        private int _NumberOfPegsPerLine;
        private int _MaxNumberOfGuesses;


        public ConsolePlayer(IConsole console)
        {
            _Console = console;
        }

        public void BeginGame(int numberOfDifferentPegs, int numberOfPegsPerLine, int maxNumberOfGuesses)
        {
            _NumberOfDifferentPegs = numberOfDifferentPegs;
            _NumberOfPegsPerLine = numberOfPegsPerLine;
            _MaxNumberOfGuesses = maxNumberOfGuesses;
            _Console.WriteLine($"The valid pegs are from 0 to {_NumberOfDifferentPegs - 1}");
            _Console.WriteLine($"There are {_NumberOfPegsPerLine} pegs per line");
            _Console.WriteLine($"You have {_MaxNumberOfGuesses} guesses to guess the secret");
            _Console.WriteLine("");
        }

        public int[] GetGuess()
        {
            _Console.Write("Guess:");
            var pegs = new int[_NumberOfPegsPerLine];
            for (var i = 0; i < _NumberOfPegsPerLine; i++)
            {
                pegs[i] = ReadPeg();
            }
            return pegs;
        }


        public void ResultFromPreviousGuess(int correctColorAndCorrectPosition, int corectColorWrongAndWrongPosition)
        {
            _Console.WriteLine($" | Correct: {correctColorAndCorrectPosition} | Wrong position: {corectColorWrongAndWrongPosition}");
        }

        public void EndGame(bool wasTheSecretGuessed, int numberOfGuesses, int[] secret)
        {
            if (wasTheSecretGuessed)
            {
                _Console.WriteLine($"Game completed - The secret was guessed in {numberOfGuesses} tries");
            }
            else
            {
                _Console.WriteLine($"Game over - The secret was not guessed in {_MaxNumberOfGuesses} tries");
                _Console.WriteLine($"The secret was: {string.Join(" ", secret)}");
            }
        }

        private int ReadPeg()
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
                    if (number >= 0 && number < _NumberOfDifferentPegs)
                    {
                        return number;
                    }
                }
                _Console.SetCursorPosition(left, top);
            }
        }
    }
}