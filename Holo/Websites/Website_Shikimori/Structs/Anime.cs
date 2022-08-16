using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Holo.Websites.Website_Shikimori.Structs
{
    [Serializable]
    public class Anime
    {
        [JsonProperty("id")] //2966 - Волчица
        public int ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("russian")]
        public string Russian { get; set; }
        [JsonProperty("image")]
        public Image Image { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("kind")]
        public string Kind { get; set; }
        [JsonProperty("score")]
        public string Score { get; set; }
        [JsonProperty("rating")]
        public string Rating { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("episodes")]
        public int Episodes { get; set; }
        [JsonProperty("episodes_aired")]
        public int EpisodesAired { get; set; }
        [JsonProperty("aired_on")]
        public DateTime? AiredOn { get; set; }
        [JsonProperty("released_on")]
        public DateTime? ReleasedOn { get; set; }
        [JsonProperty("english")]
        public string[] English { get; set; }
        [JsonProperty("japanese")]
        public string[] Japanese { get; set; }
        [JsonProperty("synonyms")]
        public string[] Synonyms { get; set; }
        [JsonProperty("license_name_ru")]
        public object LicenseNameRu { get; set; }
        [JsonProperty("duration")]
        public int Duration { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("franchise")]
        public string Franchise { get; set; }
        [JsonProperty("favoured")]
        public bool Favoured { get; set; }
        [JsonProperty("anons")]
        public bool Anons { get; set; }
        [JsonProperty("ongoing")]
        public bool Ongoing { get; set; }
        [JsonProperty("thread_id")]
        public int? ThreadID { get; set; }
        [JsonProperty("topic_id")]
        public int? TopicID { get; set; }
        [JsonProperty("myanimelist_id")]
        public int MyAnimeListID { get; set; }
        [JsonProperty("rates_scores_stats")]
        public Rating[] RatesScoresStats { get; set; }
        [JsonProperty("rates_statuses_stats")]
        public WatchStatus[] RatesStatusesStats { get; set; }
        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
        [JsonProperty("genres")]
        public Genre[] Genres { get; set; }
        [JsonProperty("studios")]
        public Studio[] Studios { get; set; }
        [JsonProperty("videos")]
        public Video[] Videos { get; set; }
        //Addition
        public AnimePerson[] Persons { get; set; }
        public Screenshot[] Screenshots { get; set; }
        public string UserOwner { get; set; }
        public List<AnimeMusic> OPs { get; set; }
        public List<AnimeMusic> EDs { get; set; }
    }
}
