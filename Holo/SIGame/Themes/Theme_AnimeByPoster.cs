using Holo.SIGame;
using Holo.SIGame.Elements;
using Holo.Websites.Website_Shikimori;
using Holo.Websites.Website_Shikimori.Structs;
using System;
using System.IO;

namespace Holo.Themes
{
    class Theme_AnimeByPoster : Theme
    {
        Random rand = new Random();

        public override void FillQuestion(SIG_question question, Shikimori shiki, string filename_)
        {
            Anime anime = shiki.GetAnimeByPoster(); 

            question.MediaUrl = Main_Shikimori.URL_ + anime.Image.Preview;
            question.MediaPath = Web.GetFilename($"./{filename_}/Images/", ".jpg", out string filename);
            question.MediaFilename = filename;
            question.Answer = $"{anime.Russian}";
        }

        public override void DownloadContent(SIG_question question)
        {
            Web.DownloadFile(question.MediaUrl, Path.ChangeExtension(question.MediaPath, "temp"));
            Utility.BlurImage(question.MediaPath);
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
            return "📅 Постеры аниме";
        }

        public static string GetRawTitle()
        {
            return "Аниме по постеру";
        }
    }
}
