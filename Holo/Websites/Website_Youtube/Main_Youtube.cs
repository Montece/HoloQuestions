using System.IO;
using System.Linq;
using VideoLibrary;
using YouTubeSearch;

namespace Holo.Websites.Website_Youtube
{
    public static class Main_Youtube
    {
        private const int PAGES = 3;
        private static readonly VideoSearch search = new VideoSearch();

        public static VideoSearchComponents Search(string query)
        {
            var videos = search.GetVideos(query, PAGES).Result;
            if (videos.Count > 0) return videos[0];
            return null;
        }

        public static void DownloadAudio(string url, string path)
        {
            string path_ = Path.ChangeExtension(path, "mp4");
            string path__ = path_.Replace("media", "media(t)");

            var youTube = YouTube.Default;
            url = url.Replace("http", "https");
            var video = youTube.GetAllVideos(url).FirstOrDefault();

            File.WriteAllBytes(path_, video.GetBytes());

            FFMpeg.Split(path_, path__, 20);
            FFMpeg.ExtractAudio(path__, path);

            File.Delete(path_);
            File.Delete(path__);
        }
    }
}
