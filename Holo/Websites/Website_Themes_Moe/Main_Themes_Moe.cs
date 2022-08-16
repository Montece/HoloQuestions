using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holo.Websites.Website_Themes_Moe
{
    public static class Main_Themes_Moe
    {
        public const string URL = "https://themes.moe/";
        private const string API_URL = URL + "api";

        public static SearchAnimeResponseDto GetAnimeMusic(string title)
        {
            int animeID = SearchAnime(title);
            if (animeID == 0) return null;

            return GetAnimeMusic(animeID);
        }
        
        public static SearchAnimeResponseDto GetAnimeMusic(int animeID)
        {
            if (animeID == 0) return null;

            SearchAnimeResponseDto anime = GetAnimeByID(animeID).FirstOrDefault();

            return anime;
        }

        private static int SearchAnime(string title)
        {
            string animes = Web.GetRequest($"{API_URL}/anime/search/{title}");
            if (animes.Contains("404") || animes.Contains("Not Found")) return 0;
            return JsonConvert.DeserializeObject<dynamic>(animes)[0];
        }

        private static SearchAnimeResponseDto[] GetAnimeByID(int animeID)
        {
            string animes = Web.GetRequest($"{API_URL}/themes/{animeID}", new CacheInfo("moe_animes", animeID.ToString()));
            if (animes.Contains("404") || animes.Contains("Not Found")) return new SearchAnimeResponseDto[] { };
            return JsonConvert.DeserializeObject<SearchAnimeResponseDto[]>(animes);
        }

        public static void DownloadAudio(string url, string path)
        {
            string path_donwload = Path.ChangeExtension(path, "webm");
            string path_split = path.Replace("media", "media(t)");

            if (!url.Contains("https")) url = url.Replace("http", "https");
            Web.DownloadFile(url, path_donwload);

            FFMpeg.Split(path_donwload, path_split, 20);
            FFMpeg.ExtractAudio(path_split, path);

            File.Delete(path_donwload);
            File.Delete(path_split);
        }
    }
}
