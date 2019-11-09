namespace Mastermind.ConsoleApp
{
    using System;

    internal class ConsoleWrapper : IConsole
    {
        public Action<string> Write => Console.Write;

        public Action<string> WriteLine => Console.WriteLine;

        public Func<ConsoleKeyInfo> ReadKey => Console.ReadKey;

        public int CursorLeft { get => Console.CursorLeft; set => Console.CursorLeft = value; }

        public int CursorTop { get => Console.CursorTop; set => Console.CursorTop = value; }

        public Action<int, int> SetCursorPosition => Console.SetCursorPosition;

    }
}
