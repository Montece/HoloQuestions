using Newtonsoft.Json;
using System.IO;

namespace Holo.Systems
{
    public static class CacheSystem
    {
        private const string ROOT = "Cache/";
        private const string EXT = ".json";
        private const bool PRETTY = true;

        static CacheSystem()
        {
            if (!Directory.Exists(ROOT)) Directory.CreateDirectory(ROOT);
        }

        public static bool Check(string category, string key, ref string value)
        {
            string dir = ROOT + category + "//";
            string path = dir + key + EXT;

            if (File.Exists(path))
            {
                value = File.ReadAllText(path);
                return true;
            }

            return false;
        }

        public static void Cache(string category, string key, string value)
        {
            string dir = ROOT + category + "//";
            string path = dir + key + EXT;

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            if (File.Exists(path)) File.Delete(path);
            File.Create(path).Dispose();
            if (PRETTY) PrettyJSON(ref value);
            File.WriteAllText(path, value);
        }

        public static void Clear()
        {
            if (Directory.Exists(ROOT)) Directory.Delete(ROOT, true);
        }

        private static void PrettyJSON(ref string json)
        {
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(json);
            json = JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}
