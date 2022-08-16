using Holo.SIGame;
using Holo.SIGame.Elements;
using Holo.Websites.Website_Shikimori;
using Holo.Websites.Website_Shikimori.Structs;
using Holo.Websites.Website_Themes_Moe;
using Holo.Websites.Website_Youtube;
using System;

namespace Holo.Themes
{
    class Theme_AnimeByOpening : Theme
    {
        public override void FillQuestion(SIG_question question, Shikimori shiki, string filename_)
        {
            Anime anime = shiki.GetAnimeByOpening();
            AnimeMusic music = anime.OPs.GetRandomElement();

            question.MediaUrl = music.URL;
            question.MediaPath = Web.GetFilename($"./{filename_}/Audio/", ".mp3", out string filename);
            question.MediaFilename = filename;
            question.Answer = $"{anime.Russian}";
        }

        public override void DownloadContent(SIG_question question)
        {
            Main_Themes_Moe.DownloadAudio(question.MediaUrl, question.MediaPath);
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
            return "🎵 OP";
        }

        public static string GetRawTitle()
        {
            return "Аниме по опенингу";
        }
    }
}
