using System;

namespace Holo
{
    public static class Output
    {
        private const ConsoleColor DefaultColor = ConsoleColor.Gray;

        public static void Print(object text, ConsoleColor color = DefaultColor)
        {
            Console.ForegroundColor = color;
            Console.Write($"[{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}] ");
            Console.WriteLine($"{text}");
            Console.ForegroundColor = DefaultColor;
        }

        public static void Error(string title, Exception exception)
        {
            Print($"{title} {exception.Message} {exception.StackTrace}", ConsoleColor.Red);
        }
    }
}
