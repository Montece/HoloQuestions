using Holo.SIGame;
using Holo.SIGame.Elements;
using Holo.Websites.Website_Shikimori;
using Holo.Websites.Website_Shikimori.Structs;
using System;

namespace Holo.Themes
{
    class Theme_AnimeByScreenshot : Theme
    {
        Random rand = new Random();

        public override void FillQuestion(SIG_question question, Shikimori shiki, string filename_)
        {
            Anime anime = shiki.GetAnimeByScreenshot();
            Screenshot screenshot = anime.Screenshots[rand.Next(0, anime.Screenshots.Length)];
 
            question.MediaUrl = Main_Shikimori.URL + screenshot.Preview;
            question.MediaPath = Web.GetFilename($"./{filename_}/Images/", ".jpg", out string filename);
            question.MediaFilename = filename;
            question.Answer = $"{anime.Russian}";
        }

        public override void DownloadContent(SIG_question question)
        {
            Web.DownloadFile(question.MediaUrl, question.MediaPath);
        }

        public override string GetXML(SIG_question question)
        {
            string content = "";
            content += $"<question price=\"{question.Price}\">";
            content += SIGamePack.GetQuestionModificator(question);
            content += $"<scenario>";
            content += $"<atom type=\"say\">Ответ - название аниме</atom>";
            content += $"<atom type=\"image\">@{question.MediaFilename}</atom>";
            content += "</scenario><right><answer>";
            content += question.Answer;
            content += "</answer></right></question>";
            return content;
        }

        public override string GetPrettyTitle()
        {
            return "🎞 Кадры из аниме";
        }

        public static string GetRawTitle()
        {
            return "Аниме по кадру";
        }
    }
}
