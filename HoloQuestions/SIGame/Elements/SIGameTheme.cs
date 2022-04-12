using System.Collections.Generic;
using System.Xml.Serialization;

namespace HoloQuestions.SIGame.Elements
{
    [XmlRoot("theme")]
    public class SIGameTheme
    {
        //Название темы
        [XmlAttribute("name")]
        public string Title { get; set; }
        //Вопросы темы
        [XmlElement("questions")]
        public Questions Questions { get; set; } = new Questions();
    }

    [XmlRoot("themes")]
    public class Themes
    {
        [XmlElement("theme")]
        public List<SIGameTheme> Theme { get; set; } = new List<SIGameTheme>();
    }
}
