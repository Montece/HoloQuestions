using Holo.SIGame.Elements;
using Holo.Websites.Website_Shikimori;

namespace Holo.Themes
{
    public abstract class Theme
    {
        public abstract void FillQuestion(SIG_question question, Shikimori shiki, string filename_);
        public abstract void DownloadContent(SIG_question question);
        public abstract string GetXML(SIG_question question);

        public abstract string GetPrettyTitle();
    }
}
