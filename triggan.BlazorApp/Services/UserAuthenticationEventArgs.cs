using System;
using triggan.BlogModel;

namespace triggan.BlazorApp.Services
{
    public class UserAuthenticationEventArgs : EventArgs
    {
        public User User { get; set; }
        public bool SignedIn { get; set; }
    }
}
