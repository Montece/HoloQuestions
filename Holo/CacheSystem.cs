using Newtonsoft.Json;
using System.IO;

namespace Holo
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

        public static bool Check(CacheInfo cacheInfo)
        {  
            if (File.Exists(GetPath(cacheInfo))) return true;
            else return false;
        }

        public static string Get(CacheInfo cacheInfo)
        {
            return File.ReadAllText(GetPath(cacheInfo));
        }

        public static void Cache(CacheInfo cacheInfo, string value)
        {
            if (!Directory.Exists(GetDirectory(cacheInfo))) Directory.CreateDirectory(GetDirectory(cacheInfo));
            string path = GetPath(cacheInfo);
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
            try
            {
                dynamic obj = JsonConvert.DeserializeObject<dynamic>(json);
                json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            }
            catch
            {

            }
        }

        private static string GetPath(CacheInfo cacheInfo)
        {
            return $"{GetDirectory(cacheInfo)}{cacheInfo.Key}{EXT}";
        }

        private static string GetDirectory(CacheInfo cacheInfo)
        {
            return $"{ROOT}{cacheInfo.Category}//";
        }
    }

    public class CacheInfo
    {
        public string Category = "";
        public string Key = "";

        public CacheInfo(string category, string key)
        {
            Category = category;
            Key = key;
        }
    }
}
