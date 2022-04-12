using System;

namespace Holo
{
    public static class Output
    {
        public static void Print(object text, ConsoleColor color = ConsoleColor.Green)
        {
            Console.ForegroundColor = color;
            Console.Write($"[{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}] ");
            Console.WriteLine($"{text}");
        }

        public static void Error(Exception text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}]: {text.ToString()}");
        }
    }
}
