using Holo.Websites.Website_Shikimori;
using Holo.Websites.Website_Shikimori.Structs;
using System;

namespace Holo.SIGame.Elements
{
    [Serializable]
    public class SIG_final
    {
        public string Title;
        public string Question;
        public string Answer;

        SIGamePack pack;

        public SIG_final(SIGamePack pack_, Shikimori shiki)
        {
            pack = pack_;

            FillQuestion(shiki);
        }

        void FillQuestion(Shikimori shiki)
        {
            Anime a = shiki.GetRandomAnime();

            string n = Environment.NewLine;
            string genres = "";
            for (int i = 0; i < a.Genres.Length; i++)
            {
                genres += a.Genres[i].Russian + ", ";
            }
            genres = genres.Remove(genres.Length - 2);
            string stuidos = "";
            for (int i = 0; i < a.Studios.Length; i++)
            {
                stuidos += a.Studios[i].Name + ", ";
            }
            stuidos = stuidos.Remove(stuidos.Length - 2);

            Question = $"Назовите аниме{n}Тип: {a.Kind + n}Оценка: {a.Score + n}Статус: {a.Status + n}Эпизодов: {a.Episodes} + {n}" +
                $"Дата выхода: {a.AiredOn.Value.ToShortDateString()}{n}Рейтинг: {a.Rating + n}Жанры: {genres + n}Студии: {stuidos + n}Смотрел: {a.UserOwner}";
            Answer = $"{a.Russian}";
            Title = $"Аниме {a.AiredOn.Value.Year} года [{a.Score}]";
        }

        public string GetXML()
        {
            string content = "";
            content += $"<scenario>";
            content += $"<atom type=\"say\">Ответ - название аниме</atom>";
            content += $"<atom>{Question}</atom>";
            content += "</scenario><right><answer>";
            content += Answer;
            content += "</answer></right>";
            return content;
        }
    }
}