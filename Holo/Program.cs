using Holo.Websites.Website_Shikimori;
using System;
using System.Threading;

namespace Holo
{
    public class Program
    {
        const int USER_ID = -1;

        private static void Initialization()
        {
            Console.Title = "Holo's Questions";
            Output.Print($"Холо проснулась");
        }

        //Entry Point
        private static void Main(string[] args)
        {
            Initialization();
            Config.Load();

            new Thread(Vk.StartBot).Start();

            bool status = true;
            while (status)
            {
                string text = Console.ReadLine();
                switch (text)
                {
                    case "стоп":
                        status = false;
                        break;
                    default:
                        break;
                }
            }

            Console.ReadLine();
        }
    }
}
