using System.Collections.Generic;
using System.Xml.Serialization;

namespace HoloQuestions.SIGame.Elements
{
    [XmlRoot("round")]
    public class SIGameRound
    {
        //Название раунда
        [XmlAttribute("name")]
        public string Title { get; set; }
        //Темы раунда
        [XmlElement("themes")]
        public Themes Themes { get; set; } = new Themes();

        public SIGameRound()
        {

        }

        public SIGameRound(string title)
        {
            Title = title;
        }
    }

    [XmlRoot("rounds")]
    public class Rounds
    {
        [XmlElement("round")]
        public List<SIGameRound> Round { get; set; } = new List<SIGameRound>();
    }
}
