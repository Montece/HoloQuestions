using Holo.Themes;
using System;

namespace Holo.SIGame.Elements
{
    [Serializable]
    public class SIG_question
    {
        public int Price;
        public string Question;
        public string Answer;

        public string MediaUrl;
        public string MediaFilename;
        public string MediaPath;

        public Theme Theme;
        public int ID;

        public SIG_question(int id_, int base_cost, SIGamePack pack, Theme theme_)
        {
            Theme = theme_;
            ID = id_;
            Price = base_cost * ID;
        }
    }
}