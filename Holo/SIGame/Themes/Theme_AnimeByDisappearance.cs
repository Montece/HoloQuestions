using Holo.SIGame;
using Holo.SIGame.Elements;
using Holo.Websites.Website_Shikimori;
using Holo.Websites.Website_Shikimori.Structs;
using System;
using System.Text;

namespace Holo.Themes
{
    class Theme_AnimeByDisappearance : Theme
    {
        public override void FillQuestion(SIG_question question, Shikimori shiki, string path_)
        {
            Anime anime = shiki.GetAnimeByDisappearance();
            StringBuilder builder = new StringBuilder(anime.Russian);
            for (int i = 0; i < builder.Length; i += 2) builder[i] = '*';
            question.Question = $"{builder.ToString()}";
            question.Answer = $"{anime.Russian}";
        }

        public override void DownloadContent(SIG_question question)
        {
            
        }

        public override string GetXML(SIG_question question)
        {
            string content = "";
            content += $"<question price=\"{question.Price}\">";
            content += SIGamePack.GetQuestionModificator(question);
            content += $"<scenario>";
            content += $"<atom type=\"say\">Ответ - название аниме</atom>";
            content += $"<atom>{question.Question}</atom>";
            content += "</scenario><right><answer>";
            content += question.Answer;
            content += "</answer></right></question>";
            return content;
        }

        public override string GetPrettyTitle()
        {
            return "✂ Исчезающее название";
        }

        public static string GetRawTitle()
        {
            return "Аниме по замене каждой 2 буквы";
        }
    }
}
