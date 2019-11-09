using System;

namespace Mastermind.ConsoleApp
{
    internal interface IConsole
    {
        Action<string> Write { get; }
        Action<string> WriteLine { get; }
        Func<ConsoleKeyInfo> ReadKey { get; }
        int CursorLeft { get; set; }
        int CursorTop { get; set; }

        void SetCursorPosition(int top, int left);
    }
}