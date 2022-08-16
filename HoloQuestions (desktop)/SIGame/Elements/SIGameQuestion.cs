using System.Collections.Generic;
using System.Xml.Serialization;

namespace HoloQuestions.SIGame.Elements
{
    [XmlRoot("question")]
    public class SIGameQuestion
    {
        //Цена вопроса
        [XmlAttribute("price")]
        public string Price { get; set; }
        //События вопроса
        [XmlElement("scenario")]
        public SIGameScenario Scenario { get; set; }
        //Правильные ответы
        [XmlElement("right")]
        public SIGameRight Right { get; set; }
        //Тип вопроса (пусто, bagcat (кот в мешке), sponsored (без риска), auction (ставка))
        [XmlElement("type")]
        public SIGameQuestionType Type { get; set; }
    }

    [XmlRoot("atom")]
    public class Atom
    {
        [XmlAttribute("type")]
        public string Type { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot("scenario")]
    public class SIGameScenario
    {
        [XmlElement("atom")]
        public List<Atom> Atom { get; set; }
    }

    [XmlRoot("right")]
    public class SIGameRight
    {
        [XmlElement("answer")]
        public string Answer { get; set; }

        public SIGameRight()
        {

        }

        public SIGameRight(string answer)
        {
            Answer = answer;
        }
    }

    [XmlRoot("param")]
    public class SIGameQuestionParam
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot("type")]
    public class SIGameQuestionType
    {
        [XmlElement("param")]
        public List<SIGameQuestionParam> Param { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
    }

    [XmlRoot("questions")]
    public class Questions
    {
        [XmlElement("question")]
        public List<SIGameQuestion> Question { get; set; } = new List<SIGameQuestion>();
    }
}
