using Holo.SIGame;
using Holo.SIGame.Elements;
using Holo.Websites.Website_Shikimori;
using Holo.Websites.Website_Shikimori.Structs;
using System;

namespace Holo.Themes
{
    class Theme_YearOfAnime : Theme
    {
        public override void FillQuestion(SIG_question question, Shikimori shiki, string path_)
        { 
            Anime anime = shiki.GetYearOfAnime();
            question.Question = $"Год выхода \"{anime.Russian}\"";
            question.Answer = $"{anime.AiredOn.Value.Year}";
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
            content += $"<atom type=\"say\">Ответ - год выхода аниме</atom>";
            content += $"<atom>{question.Question}</atom>";
            content += "</scenario><right><answer>";
            content += question.Answer;
            content += "</answer></right></question>";
            return content;
        }

        public override string GetPrettyTitle()
        {
            return "📆 Год выхода аниме";
        }

        public static string GetRawTitle()
        {
            return "Год выхода";
        }
    }
}
