﻿using System;
using System.IO;

namespace triggan.Client
{
    public class Settings
    {
        public string OnlineUrl { get; set; }
        public string LocalUrl { get; set; }
        public bool UseLocal { get; set; }

        public string GetFullUrl(string type, string param = null, string route = null, bool? forceLocal = null)
        {
            var local = forceLocal.HasValue ? forceLocal.Value : UseLocal;
            return (local ? LocalUrl : OnlineUrl).Replace("{type}", type).Replace("{route}", route).Replace("{param}", param).Replace("//", "/");
        }
    }
}