using System;

namespace Holo.Sytems
{
    public static class ErrorsSystem
    {
        public static void Call(string title, Exception exc)
        {
            Output.Print($"{title} {exc.Message} {exc.StackTrace}", ConsoleColor.Red);
        }
    }
}