using Holo.Websites.Website_Coub.Structs;
using Holo.Websites.Website_Shikimori.Structs;
using Newtonsoft.Json;
using System;

namespace Holo.Websites.Website_Coub
{
    public static class MainCoub
    {
        private static readonly Random rand = new Random();

        public static Coub GetAnimeCoub(Anime anime)
        {
            Coub coub = null;

            coub = GetRandomCoub(anime.Name);

            return coub;
        }

        private static Coub GetRandomCoub(string query, bool is_first = true, int page = 1)
        {
            string answer = Web.GetRequest($"https://coub.com/api/v2/search?q={query}&page={page}");
            Container coubContainer = JsonConvert.DeserializeObject<Container>(answer);
            if (coubContainer.Coub.Length == 0) return null;

            if (is_first) if (coubContainer.TotalPages > 1) GetRandomCoub(query, false, rand.Next(1, coubContainer.TotalPages));

            Coub coub = coubContainer.Coub[rand.Next(0, coubContainer.Coub.Length)];
            return coub;
        }
    }
}
