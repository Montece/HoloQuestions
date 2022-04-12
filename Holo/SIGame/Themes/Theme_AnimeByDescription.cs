using Holo.SIGame;
using Holo.SIGame.Elements;
using Holo.Websites.Website_Shikimori;
using Holo.Websites.Website_Shikimori.Structs;
using System;

namespace Holo.Themes
{
    class Theme_AnimeByDescription : Theme
    {
        public override void FillQuestion(SIG_question question, Shikimori shiki, string path_)
        {
            Anime anime = shiki.GetAnimeByDescription();

            bool flag = true;
            do
            {
                flag = false;
                int foundS1 = anime.Description.IndexOf("[");
                int foundS2 = anime.Description.IndexOf("]", foundS1 + 1);
                if (foundS1 != foundS2 && foundS1 >= 0 && foundS2 >= 0)
                {
                    anime.Description = anime.Description.Remove(foundS1, foundS2 - foundS1 + 1);
                    flag = true;
                }
            }
            while (flag);

            string name1 = anime.Russian;
            string name2 = anime.Name;
            string name1_ = new string('*', name1.Length);
            string name2_ = new string('*', name2.Length);
            anime.Description = anime.Description.Replace(name1, name1_);
            anime.Description = anime.Description.Replace(name2, name2_);

            question.Question = anime.Description;
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
            return "📄 Описание аниме";
        }

        public static string GetRawTitle()
        {
            return "Аниме по описанию";
        }
    }
}
