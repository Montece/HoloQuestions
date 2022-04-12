using Holo.Systems;
using Holo.Websites.Website_Shikimori.Structs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Holo.Websites.Website_Shikimori
{
    public static class Main_Shikimori
    {
        public const string URL_ = "https://shikimori.one/";
        private const string URL = "https://shikimori.one/api/";

        public const Status CurrentAnimeStatus = Status.completed;
        public const bool UseSecondaryCharacers = true;
        private static readonly Random rand = new Random();

        public static bool UserExists(string id)
        {
            string request = $"users/{id}";

            string answer = Web.GetRequest(URL + request);
            ProfileInfo info;

            try
            {
                info = JsonConvert.DeserializeObject<ProfileInfo>(answer);
            }
            catch
            {
                return false;
            }
            
            bool no_errors = info.ErrorID == 0;
            return no_errors;
        }

        public static UserAnime[] GetUserAnimeList(string username, Status animeStatus)
        {
            string request = $"users/{username}/anime_rates?limit=5000&status={animeStatus.ToString()}";

            string answer = "";
            if (!CacheSystem.Check("animelist", username, ref answer))
            {
                answer = Web.GetRequest(URL + request);
                CacheSystem.Cache("animelist", username, answer);
            }

            UserAnime[] animeList = JsonConvert.DeserializeObject<UserAnime[]>(answer);
            return animeList;
        }

        public static Anime GetAnimeInformation(UserAnime animeList)
        {
            string request = $"animes/{animeList.Anime.ID}";

            string answer = "";
            if (!CacheSystem.Check("animeinfo", animeList.Anime.ID.ToString(), ref answer))
            {
                answer = Web.GetRequest(URL + request);
                CacheSystem.Cache("animeinfo", animeList.Anime.ID.ToString(), answer);
            }

            Anime anime = JsonConvert.DeserializeObject<Anime>(answer);
            return anime;        
        }

        public static void GetAnimeScreenshots(Anime anime)
        {
            string request = $"animes/{anime.ID}/screenshots";

            string answer = "";
            if (!CacheSystem.Check("animescreen", anime.ID.ToString(), ref answer))
            {
                answer = Web.GetRequest(URL + request);
                CacheSystem.Cache("animescreen", anime.ID.ToString(), answer);
            }

            anime.Screenshots = JsonConvert.DeserializeObject<Screenshot[]>(answer);
        }

        public static AnimePerson[] GetAnimeCharacters(int animeID)
        {
            string request = $"animes/{animeID}/roles";

            string answer = "";
            if (!CacheSystem.Check("animepersons", animeID.ToString(), ref answer))
            {
                answer = Web.GetRequest(URL + request);
                CacheSystem.Cache("animepersons", animeID.ToString(), answer);
            }

            return JsonConvert.DeserializeObject<AnimePerson[]>(answer);
        }

        public static Character GetAnimeCharacterInformation(Character character)
        {
            string request = $"characters/{character.ID}";

            string answer = "";
            if (!CacheSystem.Check("characterinfo", character.ID.ToString(), ref answer))
            {
                answer = Web.GetRequest(URL + request);
                CacheSystem.Cache("characterinfo", character.ID.ToString(), answer);
            }

            character = JsonConvert.DeserializeObject<Character>(answer);
            return character;
        }
    }
}
