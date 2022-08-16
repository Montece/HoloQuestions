using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HoloQuestions.Managers
{
    public static class ShikiProfilesManager
    {
        public static List<ShikiProfile> ShikimoriProfiles = new List<ShikiProfile>();
        private const string FILENAME = "profiles.json";

        public static bool AddProfile(string name)
        {
            if (ShikimoriProfiles.Where(p => p.Name.Equals(name)).Count() == 0)
            {
                ShikimoriProfiles.Add(new ShikiProfile()
                {
                    Name = name,
                    Selected = true
                });

                Save();

                return true;
            }

            return false;
        }

        public static void RemoveProfile(ShikiProfile profile)
        {
            ShikimoriProfiles.Remove(profile);
            Save();
        }

        public static void Load()
        {
            if (File.Exists(FILENAME))
            {
                ShikimoriProfiles = JsonConvert.DeserializeObject<List<ShikiProfile>>(File.ReadAllText(FILENAME));
            }
        }

        public static void Save()
        {
            File.WriteAllText(FILENAME, JsonConvert.SerializeObject(ShikimoriProfiles));
        }
    }

    [Serializable]
    public class ShikiProfile
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Selected")]
        public bool Selected { get; set; }

        public void InvertSelected()
        {
            Selected = !Selected;
            ShikiProfilesManager.Save();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
