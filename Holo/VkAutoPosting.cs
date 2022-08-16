using Holo.SIGame.Elements;
using Holo.Themes;
using Holo.Websites.Website_Shikimori;
using System;
using System.Threading;

namespace Holo
{
    public static class VkAutoPosting
    {
        private static readonly DelayedDay Today = new DelayedDay();

        private static readonly Shikimori shiki = new Shikimori();
        private static readonly Random rand = new Random();
        private static readonly Theme_AnimeByAnagramm anagramm = new Theme_AnimeByAnagramm();
        private static readonly Theme_AnimeByDisappearance disappearance = new Theme_AnimeByDisappearance();
        private static readonly Theme_AnimeByDescription animeDescription = new Theme_AnimeByDescription();

        public static void StartPosting()
        {
            shiki.AddUser("HoloQuestions");
            shiki.FillLists();

            Today.Day = -1;

            UserVk.Authorizate();

            while (true)
            {
                try
                {
                    if (Today.Day != DateTime.Now.Day)
                    {
                        GeneratePosts();
                        Today.Day = DateTime.Now.Day;
                    }

                    if (Today.MorningPost != null)
                    {
                        if (!Today.MorningPost.Posted && Today.MorningPost.PostDate <= DateTime.Now)
                        {
                            UserVk.CreatePost(Today.MorningPost.Question);
                            Today.MorningPost.Posted = true;

                            Output.Print($"Создан пост на {Today.MorningPost.PostDate}. {Today.MorningPost.Question}");
                            Thread.Sleep(10000);
                        }
                    }

                    if (Today.EveningPost != null)
                    {
                        if (!Today.EveningPost.Posted && Today.EveningPost.PostDate <= DateTime.Now)
                        {
                            UserVk.CreatePost(Today.EveningPost.Question);
                            Today.EveningPost.Posted = true;

                            Output.Print($"Создан пост на {Today.EveningPost.PostDate}. {Today.EveningPost.Question}");
                            Thread.Sleep(10000);
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    UserVk.Authorizate();
                }
                catch (Exception x)
                {
                    Output.Error("Ошибка автоматического постинга!", x);
                }

                Thread.Sleep(1000);
            }
        }

        public static void GeneratePosts()
        {
            GenerateMorningPost();
            GenerateEveningPost();
        }

        public static void GenerateMorningPost()
        {
            var morning_question = GetRandomQuestion(/*0*/);
            Today.MorningPost.Question = $"❓ {morning_question.Question}. Что за аниме?";
            Today.MorningPost.PostDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 11, rand.Next(10, 50), 0);
            Today.MorningPost.Posted = false;
            Output.Print($"Отложен пост пост на {Today.MorningPost.PostDate}. {Today.MorningPost.Question}", ConsoleColor.Yellow);
        }

        public static void GenerateEveningPost()
        {
            var evening_question = GetRandomQuestion(/*1*/);
            Today.EveningPost.Question = $"❓ {evening_question.Question}. Что за аниме?";
            Today.EveningPost.PostDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, rand.Next(10, 50), 0);
            Today.EveningPost.Posted = false;
            Output.Print($"Отложен пост пост на {Today.EveningPost.PostDate}. {Today.EveningPost.Question}", ConsoleColor.Yellow);
        }

        private static SIG_question GetRandomQuestion(int theme = -1)
        {
            SIG_question question = new SIG_question(0, 0, null, null);

            if (theme == -1) theme = rand.Next(0, 2 + 1);

            switch (theme)
            {
                case 0:
                    anagramm.FillQuestion(question, shiki, "");
                    break;
                case 1:
                    disappearance.FillQuestion(question, shiki, "");
                    break;
                case 2:
                    animeDescription.FillQuestion(question, shiki, "");
                    break;
                default:
                    return null;
            }

            return question;
        }
    }

    public class DelayedPost
    {
        public DateTime PostDate { get; set; }

        public string Question { get; set; } = "";
        public string Answer { get; set; } = "";

        public bool Posted { get; set; } = false;
    }

    public class DelayedDay
    {
        public int Day { get; set; }
        public DelayedPost MorningPost { get; set; } = new DelayedPost();
        public DelayedPost EveningPost { get; set; } = new DelayedPost();
    }
}
