using Holo.SIGame;
using Holo.SIGame.Enums;
using Holo.Websites.Website_Shikimori;
using VkNet.Model;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Holo
{
    [Serializable]
    public class VkUser
    {
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("surname")]
        public string Surname;
        [JsonProperty("id")]
        public long ID;
        [JsonProperty("small_packs_count")]
        public int SmallPacksCount;
        [JsonProperty("medium_packs_count")]
        public int MediumPacksCount;
        [JsonProperty("big_packs_count")]
        public int BigPacksCount;
        [JsonProperty("themes")]
        private Dictionary<int, int> ThemesCount = new Dictionary<int, int>();
        [JsonProperty("packs")]
        private List<int> Packs = new List<int>();

        [JsonIgnore]
        public VkUserStatus Status = VkUserStatus.Nothing;
        [JsonIgnore]
        public PackType packageType;
        [JsonIgnore]
        public Shikimori shikimori = null;
        [JsonIgnore]
        public SIGamePack package = null;

        public VkUser()
        {
            shikimori = new Shikimori();
        }

        public VkUser(string name, string surname, long id)
        {
            shikimori = new Shikimori();
            SmallPacksCount = 0;
            MediumPacksCount = 0;
            BigPacksCount = 0;
            Name = name;
            Surname = surname;
            ID = id;
            Save();
        }

        public static Dictionary<long, VkUser> LoadAllUsers()
        {
            Dictionary<long, VkUser> users = new Dictionary<long, VkUser>();
            if (!Directory.Exists("./Users/")) Directory.CreateDirectory("./Users/");
            foreach (string file in Directory.GetFiles("./Users/"))
            {
                try
                {
                    string userJson = File.ReadAllText(file);
                    VkUser user = JsonConvert.DeserializeObject<VkUser>(userJson);
                    users.Add(user.ID, user);
                }
                catch (Exception x)
                {
                    Output.Print(x.ToString());
                }
            }
            return users;
        }

        public void SetName(string name)
        {
            Name = name;
            Save();
        }

        public void SetSurname(string surname)
        {
            Surname = surname;
            Save();
        }

        public void IncSmallPackagesCount()
        {
            SmallPacksCount++;
            Save();
        }

        public void IncMediumPackagesCount()
        {
            MediumPacksCount++;
            Save();
        }

        public void IncBigPackagesCount()
        {
            BigPacksCount++;
            Save();
        }

        public void AddTheme(int id)
        {
            if (ThemesCount.ContainsKey(id)) ThemesCount[id]++;
            else ThemesCount.Add(id, 1);
            Save();
        }

        public void AddPack(int id)
        {
            Packs.Add(id);
            Save();
        }

        public void Clear()
        {
            Status = VkUserStatus.Nothing;
            package = null;
            shikimori = new Shikimori();
        }

        public void ChooseTheme(int theme)
        {
            ThemesCount[theme]++;
        }

        private void Save()
        {
            if (!Directory.Exists("./Users/")) Directory.CreateDirectory("./Users/");
            if (!File.Exists($"./Users/user{ID}.json")) File.Create($"./Users/user{ID}.json").Dispose();
            File.WriteAllText($"./Users/user{ID}.json", JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }

    public enum VkUserStatus
    {
        Nothing,
        EnteringShikimori,
        EnteringThemes,
        Creating
    }
}
