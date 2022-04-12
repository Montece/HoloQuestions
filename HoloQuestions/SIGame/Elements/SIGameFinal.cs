using System.Xml.Serialization;

namespace HoloQuestions.SIGame.Elements
{
    [XmlRoot("round")]
    public class SIGameFinal
    {
        //Название финала
        [XmlAttribute("name")]
        public string Title { get; set; }
        //Тип финала
        [XmlAttribute("type")]
        public string Type { get; set; } = "final";
        //Темы финала
        [XmlElement("themes")]
        public Themes Themes { get; set; } = new Themes();

        public SIGameFinal()
        {

        }

        public SIGameFinal(string title)
        {
            Title = title;
        }
    }
}
