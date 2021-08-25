namespace triggan.BlazorApp
{
    public class Settings
    {
        public string OnlineUrl { get; set; }
        public string LocalUrl { get; set; }
        public bool UseLocal { get; set; }
        public int APIPort { get; set; }
        public string APIUri { get; set; }

        public string GetFullUrl(string type, string param = null, string route = null, string queryParam = null, bool? local = null)
        {
            return ((local ?? UseLocal) ? LocalUrl : OnlineUrl)
                .Replace("{type}", type)
                .Replace("{route}", route)
                .Replace("{param}", param)
                .Replace("//", "/")
                .Trim('/')
                + ((local ?? UseLocal) ? "" : $"?{queryParam}");
        }
    }
}