using System;
using System.Collections.Generic;

namespace Holo.SIGame.Elements
{
    [Serializable]
    public class SIG_round
    {
        public string Title;
        public int ID;

        public List<SIG_theme> Themes = null;

        public SIG_round(int id_, SIGamePack pack)
        {
            ID = id_;
            Title = $"Раунд #{ID}";
            Themes = new List<SIG_theme>();

            for (int i = 0; i < Config.CurrentConfig.ThemesCount; i++)
            {
                Themes.Add(new SIG_theme(pack, pack.GetRandomTheme()));
            }
        }
    }
}
