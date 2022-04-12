using System.Xml.Serialization;

namespace HoloQuestions.SIGame.Elements
{
    [XmlRoot("info")]
    public class SIGameInfo
    {
        //Автор
        [XmlElement("authors")]
        public SIGameAuthor Authors { get; set; }
        //Источник
        [XmlElement("sources")]
        public SIGameSource Sources { get; set; }

        public SIGameInfo()
        {

        }

        public SIGameInfo(string author, string source)
        {
            Authors = new SIGameAuthor(author);
            Sources = new SIGameSource(source);
        }
    }

    [XmlRoot("authors")]
    public class SIGameAuthor
    {
        [XmlElement("author")]
        public string Author { get; set; }

        public SIGameAuthor()
        {

        }

        public SIGameAuthor(string author)
        {
            Author = author;
        }
    }

    [XmlRoot("sources")]
    public class SIGameSource
    {
        [XmlElement("source")]
        public string Source { get; set; }

        public SIGameSource()
        {

        }

        public SIGameSource(string source)
        {
            Source = source;
        }
    }
}
