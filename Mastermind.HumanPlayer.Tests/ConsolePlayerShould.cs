namespace Mastermind.HumanPlayer.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;
    using Mastermind.HumanPlayer;
    using Mastermind.GameLogic;
    using Xunit;
    using System.IO;

    public class ConsolePlayerShould
    {
        private string WinText = "Game completed";
        private string LoseText = "Game over";

        [Fact]
        public void WinGame()
        {
            // Arrange
            var secret = new int[] { 1, 1, 2, 2 };
            const string Input = "1234" + "5670" + "1122";
            const string ResultText1 = "Correct: 1 | Wrong position: 1";
            const string ResultText2 = "Correct: 0 | Wrong position: 0";
            const string ResultText3 = "Correct: 4 | Wrong position: 0";

            var console = new ConsoleMock(Input);
            var consolePlayer = new ConsolePlayer(console);
            var game = new Game(8, 4, 10, secret);

            // Act
            var result = game.Play(consolePlayer);

            // Assert
            Assert.True(result.WasTheSecretGuessed);
            Assert.Contains(WinText, console.Output);
            Assert.DoesNotContain(LoseText, console.Output);
            Assert.Contains(ResultText1, console.Output);
            Assert.Contains(ResultText2, console.Output);
            Assert.Contains(ResultText3, console.Output);
        }

        [Fact]
        public void LooseGame()
        {
            // Arrange
            var secret = new int[] { 3, 2, 3, 0 };
            const string secretText = "3 2 3 0";
            var console = new ConsoleMock("1234" + "7654" + "2345" + "5241" + "2634" + "5273" + "5132" + "6352" + "4172" + "6352");
            var consolePlayer = new ConsolePlayer(console);
            var game = new Game(8, 4, 10, secret);

            // Act
            var result = game.Play(consolePlayer);

            // Assert
            Assert.False(result.WasTheSecretGuessed);
            Assert.DoesNotContain(WinText, console.Output);
            Assert.Contains(LoseText, console.Output);
            Assert.Contains(secretText, console.Output);
        }

        [Fact]
        public void IgnoreKeysThatDoesNotMapToPegs()
        {
            // Arrange
            const string Input = "\t192d3\r4 5";
            var expectedPegNumbers = new int[] { 1, 2, 3, 4, 5 };
            var console = new ConsoleMock(Input);
            var consolePlayer = new ConsolePlayer(console);

            // Act
            consolePlayer.BeginGame(8, 5, 10);
            var guess = consolePlayer.GetGuess();
            consolePlayer.ResultFromPreviousGuess(5, 0);
            consolePlayer.EndGame(true, 1, guess);

            // Assert
            Assert.Equal(expectedPegNumbers, guess);
        }

        private class ConsoleMock : IConsole
        {
            IEnumerator<char> _Input;
            StringBuilder _Output;
            public ConsoleMock(IEnumerable<char> input)
            {
                _Output = new StringBuilder();
                Write = (s) => _Output.Append(s);
                WriteLine = (s) => _Output.AppendLine(s);
                _Input = input.GetEnumerator();
                ReadKey = () =>
                {
                    _Input.MoveNext();
                    var c = _Input.Current;
                    ConsoleKey key;
                    switch (c)
                    {
                        case '\r': key = ConsoleKey.Enter; break;
                        case ' ': key = ConsoleKey.Spacebar; break;
                        case '\t': key = ConsoleKey.Tab; break;
                        default:
                            var keyName = "";
                            if (Char.IsDigit(c))
                            {
                                keyName = "D" + c;
                            }
                            else
                            {
                                keyName = char.ToUpper(c).ToString();
                            }
                            key = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), keyName);
                            break;
                    }

                    return new ConsoleKeyInfo(c, key, false, false, false);
                };
            }

            public string Output { get { return _Output.ToString(); } }
            public Action<string> Write { get; }

            public Action<string> WriteLine { get; }

            public Func<ConsoleKeyInfo> ReadKey { get; }

            public int CursorLeft { get => 1; set { } }
            public int CursorTop { get => 1; set { } }

            public Action<int, int> SetCursorPosition => (left, top) => { };
        }
    }
}