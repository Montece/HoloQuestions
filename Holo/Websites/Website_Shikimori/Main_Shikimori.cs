using Holo.Websites.Website_Shikimori.Structs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Holo.Websites.Website_Shikimori
{
    public static class Main_Shikimori
    {
        public const string URL = "https://shikimori.one/";
        private const string API_URL = URL + "api";

        public const Status CurrentAnimeStatus = Status.completed;
        public const bool UseSecondaryCharacers = true;

        public static bool UserExists(string id)
        {
            string request = $"{API_URL}/users/{id}";

            string answer = Web.GetRequest(request);
            ProfileInfo info;

            try
            {
                info = JsonConvert.DeserializeObject<ProfileInfo>(answer);
            }
            catch
            {
                return false;
            }
            
            bool no_errors = info.ErrorID.Equals(0);
            return no_errors;
        }

        public static List<UserAnime> GetUserAnimeList(string username, Status animeStatus, int limit = 5000)
        {
            string request = $"{API_URL}/users/{username}/anime_rates?limit={limit}&status={animeStatus}";

            string answer = Web.GetRequest(request);

            List<UserAnime> animeList = JsonConvert.DeserializeObject<List<UserAnime>>(answer);
            return animeList;
        }

        public static Anime GetAnimeInformation(UserAnime animeList)
        {
            string request = $"{API_URL}/animes/{animeList.Anime.ID}";

            string answer = Web.GetRequest(request, new CacheInfo("animeinfo", animeList.Anime.ID.ToString()));

            Anime anime = JsonConvert.DeserializeObject<Anime>(answer);
            return anime;        
        }

        public static void GetAnimeScreenshots(Anime anime)
        {
            string request = $"{API_URL}/animes/{anime.ID}/screenshots";

            string answer = Web.GetRequest(request, new CacheInfo("animescreen", anime.ID.ToString()));

            anime.Screenshots = JsonConvert.DeserializeObject<Screenshot[]>(answer);
        }

        public static AnimePerson[] GetAnimeCharacters(int animeID)
        {
            string request = $"{API_URL}/animes/{animeID}/roles";

            string answer = Web.GetRequest(request, new CacheInfo("animepersons", animeID.ToString()));

            return JsonConvert.DeserializeObject<AnimePerson[]>(answer);
        }

        public static Character GetAnimeCharacterInformation(Character character)
        {
            string request = $"{API_URL}/characters/{character.ID}";

            string answer = Web.GetRequest(request, new CacheInfo("characterinfo", character.ID.ToString()));

            character = JsonConvert.DeserializeObject<Character>(answer);
            return character;
        }
    }
}
