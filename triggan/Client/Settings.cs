using System;
using System.IO;

namespace triggan.Client
{
    public class Settings
    {
        public string OnlineUrl { get; set; }
        public string LocalUrl { get; set; }
        public bool UseLocal { get; set; }

        public string GetFullUrl(string type, string param = null, string route = null, bool? local = null)
        {
            var path = ((local ?? UseLocal) ? LocalUrl : OnlineUrl).Replace("{type}", type).Replace("{route}", route).Replace("{param}", param);
            if (local ?? UseLocal) path = path.Replace("//", "/");
            return path;
        }
    }
}