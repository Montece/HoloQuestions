using Holo.SIGame;
using Holo.Themes;
using Holo.Websites.Website_Shikimori;
using Holo.Websites.Website_Themes_Moe;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Holo
{
    public class Program
    {
        private static Thread BotThread = null;
        private static Thread AutoPostingThread = null;

        ///<summary> Entry Point </summary>
        private static void Main(string[] args)
        {
            Console.Title = "Вопросы Волчицы Холо";
            Output.Print("Холо проснулась");

            Config.Load();
            UserVk.Init();

            BotThread = new Thread(GroupVk.StartBot);
            BotThread.Start();

            bool status = true;
            while (status)
            {
                string text = Console.ReadLine();
                switch (text)
                {
                    case "exit":
                        status = false;
                        break;
                    case "stop":
                        status = false;
                        break;
                    case "autoposting start":
                        AutoPostingThread = new Thread(VkAutoPosting.StartPosting);
                        AutoPostingThread.Start();
                        Output.Print("Авто-создание постов включено");
                        break;
                    case "autoposting stop":
                        AutoPostingThread.Abort();
                        AutoPostingThread = null;
                        Output.Print("Авто-создание постов выключено");
                        break;
                    case "autoposting regen morning":
                        if (AutoPostingThread != null)
                        {
                            VkAutoPosting.GenerateMorningPost();
                            Output.Print("Пост на утро пересоздан");
                        }
                        else Output.Print("Авто-создание постов не включено", ConsoleColor.Yellow);
                        break;
                    case "autoposting regen evening":
                        if (AutoPostingThread != null)
                        {
                            VkAutoPosting.GenerateEveningPost();
                            Output.Print("Пост на вечер пересоздан");
                        }
                        else Output.Print("Авто-создание постов не включено", ConsoleColor.Yellow);
                        break;
                    case "test":
                        Output.Print("Execute test command", ConsoleColor.Magenta);
                        SIGamePack pack = new SIGamePack(SIGame.Enums.PackType.Big);
                        pack.Themes.Add(new Theme_AnimeByOpening());
                        Shikimori shiki = new Shikimori();
                        shiki.AddUser("Montece");
                        shiki.FillLists();
                        pack.Fill(shiki);
                        pack.SaveToSIQ("D:\\pack.siq", shiki);
                        break;
                    case "clear":
                        Console.Clear();
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine("Холо уснула");
            Console.ReadLine();
        }
    }
}
