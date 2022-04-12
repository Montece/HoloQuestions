using HoloQuestions.SIGame;
using HoloQuestions.SIGame.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Serialization;

namespace HoloQuestions.Managers
{
    public static class SIGameManager
    {
        private static readonly Random random = new Random();

        public static SIGamePack CreatePack(string title, string difficulty, string version, int rounds_count, int themes_count, int questions_count, int finals_count, int base_price, int price_increase, HashSet<ITheme> roundThemes, HashSet<IFinal> finalThemes)
        {
            string restriction = "18+";

            SIGamePack pack = new SIGamePack()
            {
                Title = title,
                Filename = ToValidFilename(title),
                Date = DateTime.Now.ToShortDateString(),
                Difficulty = difficulty,
                Id = Guid.NewGuid().ToString(),
                Xmlns = "http://vladimirkhil.com/ygpackage3.0.xsd",
                Info = new SIGameInfo("Holo", ""),
                Logo = "@logo.png",
                Publisher = "Holo's questions",
                Restriction = restriction,
                Version = version
            };

            for (int i = 1; i <= rounds_count; i++)
            {
                SIGameRound round = new SIGameRound($"Раунд #{i}");

                for (int j = 1; j <= themes_count; j++)
                {
                    ITheme roundTheme = GetRandomTheme(roundThemes);

                    SIGameTheme theme = new SIGameTheme();

                    for (int c = 1; c <= questions_count; c++)
                    {
                        SIGameQuestion question = new SIGameQuestion();
                        theme.Questions.Question.Add(question);
                    }

                    roundTheme.FillTheme(theme, base_price, price_increase);
                    round.Themes.Theme.Add(theme);
                }

                pack.Rounds.Round.Add(round);
            }

            /*SIGameFinal final = new SIGameFinal($"Финал");
            for (int i = 1; i <= finals_count; i++)
            {
                ITheme finalTheme = GetRandomTheme(finalThemes);

                final.Themes
            }*/

            return pack;
        }

        public static void ExportPack(SIGamePack pack)
        {
            //Главная директория
            if (Directory.Exists(pack.Filename)) Directory.Delete(pack.Filename, true);
            Directory.CreateDirectory(pack.Filename);

            Directory.CreateDirectory(pack.Filename + @"\Images");
            Directory.CreateDirectory(pack.Filename + @"\Video");
            Directory.CreateDirectory(pack.Filename + @"\Texts");

            File.WriteAllText(pack.Filename + @"\Texts\authors.xml", "<?xml version=\"1.0\" encoding=\"utf-8\"?><Authors />");
            File.WriteAllText(pack.Filename + @"\Texts\sources.xml", "<?xml version=\"1.0\" encoding=\"utf-8\"?><Sources />");
            File.WriteAllText(pack.Filename + @"\[Content_Types].xml", "<?xml version=\"1.0\" encoding=\"utf-8\"?><Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\"><Default Extension=\"xml\" ContentType=\"si/xml\" /></Types>");

            using (FileStream fs = new FileStream(pack.Filename + @"\content.xml", FileMode.OpenOrCreate))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SIGamePack));
                serializer.Serialize(fs, pack);
            }

            if (File.Exists(pack.Filename + ".siq")) File.Delete(pack.Filename + ".siq");
            ZipFile.CreateFromDirectory(pack.Filename, pack.Filename + ".siq");
            Directory.Delete(pack.Filename, true);
        }

        public static ITheme GetRandomTheme(HashSet<ITheme> themes)
        {
            return themes.ElementAt(random.Next(0, themes.Count));
        }

        public static string ToValidFilename(string filename)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(c, '_');
            }

            return filename;
        }
    }
}
