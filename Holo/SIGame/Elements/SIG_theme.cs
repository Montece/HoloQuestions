using Holo.Themes;
using System;
using System.Collections.Generic;

namespace Holo.SIGame.Elements
{
    [Serializable]
    public class SIG_theme
    {
        public string Title;
        public Theme Theme;
        
        public List<SIG_question> Questions = null;

        public SIG_theme(SIGamePack pack, Theme theme_)
        {
            Theme = theme_;
            Questions = new List<SIG_question>();
            Title = theme_.GetPrettyTitle();

            for (int i = 0; i < Config.CurrentConfig.QuestionsCount; i++)
            {
                SIG_question question = new SIG_question(i + 1, Config.CurrentConfig.BaseCost, pack, Theme);

                Questions.Add(question);
            }
        }
    }
}
