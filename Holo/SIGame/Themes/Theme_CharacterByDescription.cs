using Holo.SIGame;
using Holo.SIGame.Elements;
using Holo.Websites.Website_Shikimori;
using Holo.Websites.Website_Shikimori.Structs;

namespace Holo.Themes
{
    class Theme_CharacterByDescription : Theme
    {
        public override void FillQuestion(SIG_question question, Shikimori shiki, string filename_)
        {
            AnimePerson person;
            do
            {
                person = shiki.GetCharacterByDescription();
                person.Character = Main_Shikimori.GetAnimeCharacterInformation(person.Character);
            }
            while (person.Character == null || person.Character.Description == null);
            
            bool flag = true;
            do
            {
                flag = false;
                int foundS1 = person.Character.Description.IndexOf("[");
                int foundS2 = person.Character.Description.IndexOf("]", foundS1 + 1);
                if (foundS1 != foundS2 && foundS1 >= 0 && foundS2 >= 0)
                {
                    person.Character.Description = person.Character.Description.Remove(foundS1, foundS2 - foundS1 + 1);
                    flag = true;
                }
            }
            while (flag);

            string[] names1 = person.Character.Russian.Split(' ');
            string[] names2 = person.Character.Name.Split(' ');
            for (int i = 0; i < names1.Length; i++) if (person.Character.Description.ToLower().Contains(names1[i].ToLower())) person.Character.Description = person.Character.Description.Replace(names1[i], new string('*', names1[i].Length));
            for (int i = 0; i < names2.Length; i++) if (person.Character.Description.ToLower().Contains(names2[i].ToLower())) person.Character.Description = person.Character.Description.Replace(names2[i], new string('*', names2[i].Length));

            question.Question = person.Character.Description;
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
            return "📄 Описание персонажа";
        }

        public static string GetRawTitle()
        {
            return "Персонаж по описанию";
        }
    }
}
