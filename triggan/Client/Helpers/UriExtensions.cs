using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace triggan.Client.Helpers
{
    public static class UriExtensions
    {
        public static Uri SetPort(this Uri uri, int newPort)
        {
            var builder = new UriBuilder(uri);
            builder.Port = newPort;
            return builder.Uri;
        }
    }
}
