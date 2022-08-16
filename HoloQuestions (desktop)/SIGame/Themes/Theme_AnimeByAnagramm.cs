using HoloQuestions.SIGame.Elements;
using System.Collections.Generic;

namespace HoloQuestions.SIGame.Themes
{
    public class Theme_AnimeByAnagramm : ITheme
    {
        public void FillTheme(SIGameTheme theme, int base_price, int price_increase)
        {
            theme.Title = "📋 Анаграмма аниме";

            for (int i = 0; i < theme.Questions.Question.Count; i++)
            {
                theme.Questions.Question[i].Price = $"{base_price + i * price_increase}";
                theme.Questions.Question[i].Type = null;
                theme.Questions.Question[i].Right = new SIGameRight("Саша");
                theme.Questions.Question[i].Scenario = new SIGameScenario()
                {
                    Atom = new List<Atom>()
                    {
                        new Atom()
                        {
                            Text = "Кто я?",
                            Type = "say"
                        }
                    }
                };
            }
        }

        public override string ToString()
        {
            return "Анаграмма из названия аниме";
        }
    }
}
