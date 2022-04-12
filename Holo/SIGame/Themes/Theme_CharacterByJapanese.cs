using Holo.SIGame;
using Holo.SIGame.Elements;
using Holo.Websites.Website_Shikimori;
using Holo.Websites.Website_Shikimori.Structs;

namespace Holo.Themes
{
    class Theme_CharacterByJapanese : Theme
    {
        public override void FillQuestion(SIG_question question, Shikimori shiki, string path_)
        {
            AnimePerson person = shiki.GetCharacterByJapanese();
            person.Character = Main_Shikimori.GetAnimeCharacterInformation(person.Character);

            question.Question = person.Character.Japanese;
            question.Answer = $"{person.Character.Russian} ({person.Anime.Russian})";
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
            content += $"<atom type=\"say\">Ответ - имя или фамилия персонажа</atom>";
            content += $"<atom>{question.Question}</atom>";
            content += "</scenario><right><answer>";
            content += question.Answer;
            content += "</answer></right></question>";
            return content;
        }

        public override string GetPrettyTitle()
        {
            return "🈵 Персонаж на японском";
        }

        public static string GetRawTitle()
        {
            return "Персонаж на японском";
        }
    }
}
