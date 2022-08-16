using Holo.SIGame.Elements;
using Holo.SIGame.Enums;
using Holo.Themes;
using Holo.Websites.Website_Shikimori;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Holo.SIGame
{
    public class SIGamePack
    {
        private readonly List<SIG_round> Rounds = new List<SIG_round>();
        private readonly List<SIG_final> Finals = new List<SIG_final>();
        public List<Theme> Themes = new List<Theme>();
        private static readonly Random rand = new Random();

        public int ID { get; set; }

        private PackType type;
        private string Filename;
        private string FilenameTemp;

        public SIGamePack(PackType type_)
        {
            type = type_;
        }  

        public string Fill(Shikimori shiki)
        {
            int RoundsCount = 0;
            switch (type)
            {
                case PackType.Small:
                    RoundsCount = Config.CurrentConfig.SmallRoundsCount;
                    break;
                case PackType.Medium:
                    RoundsCount = Config.CurrentConfig.MediumRoundsCount;
                    break;
                case PackType.Big:
                    RoundsCount = Config.CurrentConfig.BigRoundsCount;
                    break;
                default:
                    break;
            }

            if (!Directory.Exists(Config.CurrentConfig.PackPath)) Directory.CreateDirectory(Config.CurrentConfig.PackPath);

            Filename = Config.CurrentConfig.PackPath + @"/" + Config.CurrentConfig.Filename + ID + Config.CurrentConfig.Extension;
            FilenameTemp = Filename + "(temp)";

            for (int i = 0; i < RoundsCount; i++)
            {
                SIG_round round = new SIG_round(i + 1, this);
                Rounds.Add(round);
            }

            for (int i = 0; i < Rounds.Count; i++)
            {
                for (int j = 0; j < Rounds[i].Themes.Count; j++)
                {
                    for (int c = 0; c < Rounds[i].Themes[j].Questions.Count; c++)
                    {
                        try
                        {
                            Output.Print($"Pack #{ID}: Fill r[{i + 1}/{Rounds.Count}] t[{j + 1}/{Rounds[i].Themes.Count}] q[{c + 1}/{Rounds[i].Themes[j].Questions.Count}]");
                            Rounds[i].Themes[j].Questions[c].Theme.FillQuestion(Rounds[i].Themes[j].Questions[c], shiki, FilenameTemp);
                        }
                        catch
                        {
                           //c--;
                        }
                    }
                }
            }

            for (int i = 0; i < Config.CurrentConfig.FinalsCount; i++)
            {
                //Finals.Add(new SIG_final(this, shiki));
            }

            SaveToSIQ(Filename, shiki);
            return Filename;
        }

        public void SaveToSIQ(string filename_, Shikimori shiki)
        {
            Directory.CreateDirectory($"./{FilenameTemp}/Images");
            Directory.CreateDirectory($"./{FilenameTemp}/Video");
            Directory.CreateDirectory($"./{FilenameTemp}/Texts");
            Directory.CreateDirectory($"./{FilenameTemp}/Audio");

            File.WriteAllText($"./{FilenameTemp}/Texts/authors.xml", "<?xml version=\"1.0\" encoding=\"utf-8\"?><Authors />");
            File.WriteAllText($"./{FilenameTemp}/Texts/sources.xml", "<?xml version=\"1.0\" encoding=\"utf-8\"?><Sources />");
            File.WriteAllText($"./{FilenameTemp}/[Content_Types].xml", "<?xml version=\"1.0\" encoding=\"utf-8\"?><Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\" ><Default Extension = \"xml\" ContentType = \"si/xml\" /></Types>");

            string content = "";

            for (int i = 0; i < Rounds.Count; i++)
            {
                content += $"<round name=\"{Rounds[i].Title}\"><themes>";

                for (int j = 0; j < Rounds[i].Themes.Count; j++)
                {
                    content += $"<theme name=\"{Rounds[i].Themes[j].Title}\"><questions>";

                    for (int c = 0; c < Rounds[i].Themes[j].Questions.Count; c++)
                    {
                        /*try
                        {*/
                            Output.Print($"Pack #{ID}: Download r[{i + 1}/{Rounds.Count}] t[{j + 1}/{Rounds[i].Themes.Count}] q[{c + 1}/{Rounds[i].Themes[j].Questions.Count}]");
                            Rounds[i].Themes[j].Questions[c].Theme.DownloadContent(Rounds[i].Themes[j].Questions[c]);
                            content += Rounds[i].Themes[j].Questions[c].Theme.GetXML(Rounds[i].Themes[j].Questions[c]);
                        /*}
                        catch
                        {
                            Rounds[i].Themes[j].Questions[c].Theme.FillQuestion(Rounds[i].Themes[j].Questions[c], shiki, FilenameTemp);
                            c--;
                        }*/
                    }

                    content += "</questions></theme>";
                }

                content += "</themes></round>";
            }

            /*content += $"<round name=\"Финал\" type=\"final\"><themes>";
            foreach (SIG_final final in Finals)
            {
                content += $"<theme name=\"{final.Title}\"><questions><question price=\"0\">";
                content += final.GetXML();
                content += "</question></questions></theme>";
            }
            content += "</themes></round>";*/

            new Bitmap(Properties.Resources.pack_logo).Save($"./{FilenameTemp}/Images/pack_logo.png");

            //after autors <sources><source>{SIGameConstants.SOURCE}</source></sources>
            File.WriteAllText($"./{FilenameTemp}/content.xml", $"<?xml version=\"1.0\" encoding=\"UTF-8\"?><package xmlns=\"http://vladimirkhil.com/ygpackage3.0.xsd\" name=\"{Config.CurrentConfig.Title}{ID}\" version=\"1.0\" id=\"{Guid.NewGuid()}\" restriction=\"{Config.CurrentConfig.AgeLimit}\" date=\"{DateTime.Now.ToShortDateString()}\" difficulty=\"{Config.CurrentConfig.Difficulty}\" publisher=\"Montece\" logo=\"@pack_logo.png\"><info><authors><author>{Config.CurrentConfig.Author}</author></authors><comments>{Config.CurrentConfig.Comment}</comments></info><rounds>{content}</rounds></package>");

            if (File.Exists(filename_)) File.Delete(filename_);
            Zip.ZipFile.CreateFromDirectory($"./{FilenameTemp}", filename_, System.IO.Compression.CompressionLevel.Optimal, false);

            Directory.Delete($"./{FilenameTemp}", true);
        }

        public static string GetQuestionModificator(SIG_question question)
        {
            int chance = rand.Next(1, 101);
            string mod = "";

            if (chance >= 1 && chance < Config.CurrentConfig.QuestionModificatorChancePerc)
            {
                switch (rand.Next(1, 4))
                {
                    case 1: //Кот в мешке
                        mod += $"<type name=\"bagcat\">";
                        mod += $"<param name=\"theme\"></param>";
                        mod += $"<param name=\"cost\">{question.Price}</param>";
                        mod += $"<param name=\"self\">false</param>";
                        mod += $"<param name=\"knows\">before</param>";
                        mod += $"</type>";
                        break;
                    case 2: //Без риска
                        mod += $"<type name=\"sponsored\" />";
                        break;
                    case 3: //Со ставкой
                        mod += $"<type name=\"auction\" />";
                        break;
                    default:
                        break;
                }
            }
            
            return mod;
        }

        public static string Anagramm(string russian)
        {
            string[] CurrentWords = russian.Split(' ', '!', ',', '.', '?', '-', ':', ')', '(', ']', '[', '\'', '"');

            for (int j = 0; j < CurrentWords.Length; j++)
            {
                List<char> CurrentWord = CurrentWords[j].ToCharArray().ToList();
                List<char> NewWord = new List<char>();

                int i = 0;
                while (i < CurrentWord.Count)
                {
                    int index = rand.Next(0, CurrentWord.Count);
                    NewWord.Add(CurrentWord[index]);
                    CurrentWord.RemoveAt(index);
                }

                CurrentWords[j] = new string(NewWord.ToArray()).ToLower();
            }

            string result = "";
            for (int i = 0; i < CurrentWords.Length; i++)
            {
                result += CurrentWords[i] + " ";
            }

            return result;
        }

        public Theme GetRandomTheme()
        {
            var theme = Themes[rand.Next(0, Themes.Count)];
            return theme;
        }
    }
}
