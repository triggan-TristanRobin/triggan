using System;

namespace triggan.Client
{
    public class Settings
    {
        public string OnlineUrl { get; set; }
        public string LocalUrl { get; set; }
        public bool UseLocal { get; set; }

        public string GetFullUrl(string type, string param = null)
        {
            Console.WriteLine("Settings:");
            Console.WriteLine(OnlineUrl);
            Console.WriteLine(LocalUrl);
            Console.WriteLine(UseLocal);
            Console.WriteLine((UseLocal ? LocalUrl : OnlineUrl).Replace("{type}", type).Replace("{param}", param));
            return (UseLocal ? LocalUrl : OnlineUrl).Replace("{type}", type).Replace("{param}", param);
        }
    }
}