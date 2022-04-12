using Holo.SIGame;
using Holo.SIGame.Elements;
using Holo.Websites.Website_Shikimori;
using Holo.Websites.Website_Shikimori.Structs;

namespace Holo.Themes
{
    class Theme_CharacterByImage : Theme
    {
        public override void FillQuestion(SIG_question question, Shikimori shiki, string filename_)
        {
            AnimePerson person = shiki.GetCharacterByImage();
            question.MediaUrl = Main_Shikimori.URL_ + person.Character.Image.Preview;
            question.MediaPath = Web.GetFilename($"./{filename_}/Images/", ".jpg", out string filename);
            question.MediaFilename = filename;
            question.Answer = $"{person.Character.Russian} ({person.Anime.Russian})";
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
            content += $"<atom type=\"say\">Ответ - имя или фамилия персонажа</atom>";
            content += $"<atom type=\"image\">@{question.MediaFilename}</atom>";
            content += "</scenario><right><answer>";
            content += question.Answer;
            content += "</answer></right></question>";
            return content;
        }

        public override string GetPrettyTitle()
        {
            return "🖼 Персонаж по картинке";
        }

        public static string GetRawTitle()
        {
            return "Персонаж по картинке";
        }
    }
}
