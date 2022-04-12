using Holo.SIGame;
using Holo.SIGame.Elements;
using Holo.Websites.Website_Shikimori;
using Holo.Websites.Website_Shikimori.Structs;
using Holo.Websites.Website_Youtube;

namespace Holo.Themes
{
    class Theme_AnimeByOst : Theme
    {
        public override void FillQuestion(SIG_question question, Shikimori shiki, string filename_)
        {
            Anime anime = shiki.GetAnimeByOst();
            var video = Main_Youtube.Search(anime.Name + " ost");

            question.MediaUrl = video.getUrl();
            question.MediaPath = Web.GetFilename($"./{filename_}/Audio/", ".mp3", out string filename);
            question.MediaFilename = filename;
            question.Answer = $"{anime.Russian}";
        }

        public override void DownloadContent(SIG_question question)
        {
            Main_Youtube.DownloadAudio(question.MediaUrl, question.MediaPath);
        }

        public override string GetXML(SIG_question question)
        {
            string content = "";
            content += $"<question price=\"{question.Price}\">";
            content += SIGamePack.GetQuestionModificator(question);
            content += $"<scenario>";
            content += $"<atom type=\"say\">Ответ - название аниме</atom>";
            content += $"<atom type=\"voice\">@{question.MediaFilename}</atom>";
            content += "</scenario><right><answer>";
            content += question.Answer;
            content += "</answer></right></question>";
            return content;
        }

        public override string GetPrettyTitle()
        {
            return "🔊 OST";
        }

        public static string GetRawTitle()
        {
            return "Аниме по осту";
        }
    }
}
