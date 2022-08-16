using Newtonsoft.Json;
using System;
using System.IO;

namespace Holo
{
    [Serializable]
    public class Config
    {
        [JsonIgnore]
        private const string CONFIG_FILENAME = "config.json";
        [JsonIgnore]
        public static Config CurrentConfig = null;

        [JsonProperty("packs_count")]
        public int PacksCount { get; set; }
        [JsonProperty("pack_path")]
        public string PackPath { get; set; }
        [JsonProperty("pack_title")]
        public string Title { get; set; }
        [JsonProperty("pack_comment")]
        public string Comment { get; set; }
        [JsonProperty("pack_age_limit")]
        public string AgeLimit { get; set; }
        [JsonProperty("pack_difficulty")]
        public int Difficulty { get; set; }
        [JsonProperty("pack_author")]
        public string Author { get; set; }
        [JsonProperty("pack_filename")]
        public string Filename { get; set; }
        [JsonProperty("pack_extension")]
        public string Extension { get; set; }
        [JsonProperty("pack_small_rounds_count")]
        public int SmallRoundsCount { get; set; }
        [JsonProperty("pack_medium_rounds_count")]
        public int MediumRoundsCount { get; set; }
        [JsonProperty("pack_big_rounds_count")]
        public int BigRoundsCount { get; set; }
        [JsonProperty("pack_themes_count")]
        public int ThemesCount { get; set; }
        [JsonProperty("pack_questions_count")]
        public int QuestionsCount { get; set; }
        [JsonProperty("pack_finals_count")]
        public int FinalsCount { get; set; }
        [JsonProperty("pack_base_cost")]
        public int BaseCost { get; set; }
        [JsonProperty("pack_use_final")]
        public bool UseFinal { get; set; }
        [JsonProperty("pack_question_modificator_chance_perc")]
        public int QuestionModificatorChancePerc { get; set; }

        public static Config GetDefault()
        {
            Config config = new Config()
            {
                PacksCount = 1,
                PackPath = "Packs",
                Title = "Аниме-пак от Холо #",
                Comment = "Пак сделан с помощью программы SIGame Anime Fan",
                AgeLimit = "16+",
                Difficulty = 7,
                Author = "Montece",
                Filename = "AnimePackByHolo_",
                Extension = ".siq",
                SmallRoundsCount = 2,
                MediumRoundsCount = 3,
                BigRoundsCount = 4,
                ThemesCount = 5,
                QuestionsCount = 5,
                FinalsCount = 5,
                BaseCost = 100,
                UseFinal = true,
                QuestionModificatorChancePerc = 22,
            };

            return config;
        }

        public static void Load()
        {
            try
            {
                if (File.Exists(CONFIG_FILENAME))
                {
                    string text = File.ReadAllText(CONFIG_FILENAME);
                    CurrentConfig = JsonConvert.DeserializeObject<Config>(text);
                }
                else Save();
            }
            catch (Exception x)
            {
                Output.Error("Ошибка загрузки конфига!", x);
            }

            if (CurrentConfig == null) Save();
        }

        public static void Save()
        {
            try
            {
                if (CurrentConfig == null) CurrentConfig = GetDefault();

                if (File.Exists(CONFIG_FILENAME)) File.Delete(CONFIG_FILENAME);
                string text = CurrentConfig.ToString();
                File.WriteAllText(CONFIG_FILENAME, text);
            }
            catch (Exception x)
            {
                Output.Error("Ошибка загрузки конфига!", x);
            }
        }

        public override string ToString()
        {
            try
            {
                string text = JsonConvert.SerializeObject(this, Formatting.Indented);
                return text;
            }
            catch (Exception x)
            {
                Output.Error("Ошибка загрузки конфига!", x);
                return "error";
            }
        }

        public void IncPacksCount()
        {
            PacksCount++;
            Save();
        }
    }
}
