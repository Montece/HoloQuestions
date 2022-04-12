using HoloQuestions.SIGame.Elements;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace HoloQuestions.SIGame
{
    [XmlRoot("package")]
    public class SIGamePack
    {
        //Название пака
        [XmlAttribute("name")]
        public string Title { get; set; }

        //Автор и источник пака
        [XmlElement("info")]
        public SIGameInfo Info { get; set; }

        //Версия пака
        [XmlAttribute("version")]
        public string Version { get; set; }

        //Уникальный номер пака (guid)
        [XmlAttribute("id")]
        public string Id { get; set; }

        //Возрастные ограничения пака
        [XmlAttribute("restriction")]
        public string Restriction { get; set; }

        //Дата создания пака
        [XmlAttribute("date")]
        public string Date { get; set; }

        //Схема xml документа
        [XmlAttribute("xmlns")]
        public string Xmlns { get; set; }

        //Тот, кто опубликовал пак
        [XmlAttribute("publisher")]
        public string Publisher { get; set; }

        //Сложность пака
        [XmlAttribute("difficulty")]
        public string Difficulty { get; set; }

        //Путь к логотипу пака
        [XmlAttribute("logo")]
        public string Logo { get; set; }

        //Раунды пака
        [XmlElement("rounds")]
        public Rounds Rounds { get; set; } = new Rounds();

        //Название файла пака
        [XmlIgnore]
        public string Filename { get; set; }
    }
}