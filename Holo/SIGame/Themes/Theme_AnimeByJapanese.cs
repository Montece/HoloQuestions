using Holo.SIGame;
using Holo.SIGame.Elements;
using Holo.Websites.Website_Shikimori;
using Holo.Websites.Website_Shikimori.Structs;

namespace Holo.Themes
{
    class Theme_AnimeByJapane : Theme
    {
        public override void FillQuestion(SIG_question question, Shikimori shiki, string path_)
        {
            Anime anime = shiki.GetAnimeByJapane();
            question.Question = $"{anime.Japanese[0]}";
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
            return "🈵 Аниме на японском";
        }

        public static string GetRawTitle()
        {
            return "Аниме на японском";
        }
    }
}
