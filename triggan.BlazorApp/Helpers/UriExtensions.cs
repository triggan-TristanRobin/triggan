using System;

namespace triggan.BlazorApp.Helpers
{
    public static class UriExtensions
    {
        public static Uri SetPort(this Uri uri, int? newPort)
        {
            var builder = new UriBuilder(uri);
            builder.Port = newPort ?? builder.Port;
            return builder.Uri;
        }
    }
}