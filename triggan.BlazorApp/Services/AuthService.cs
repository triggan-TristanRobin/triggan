using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using triggan.BlogModel;

namespace triggan.BlazorApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient httpClient;
        private readonly AuthenticationStateProvider authStateProvider;
        private readonly ILocalStorageService localStorage;

        public AuthService(HttpClient httpClient, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.authStateProvider = authStateProvider;
            this.localStorage = localStorage;

            Console.WriteLine("Created AuthService instance.");
            Console.WriteLine($"(AuthService) HttpClient requestheader auth: {httpClient.DefaultRequestHeaders.Authorization}");
        }

        public async Task<User> Register(User registerModel)
        {
            var response = await httpClient.PostAsJsonAsync("Signup", registerModel);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<User>()
                : new User { Id = -500 };
        }

        public async Task<User> Signin(UserSigninInfos signinInfos)
        {
            var response = await httpClient.PostAsJsonAsync("Signin", signinInfos);

            if (!response.IsSuccessStatusCode)
            {
                return new User { Id = -500 };
            }

            var signedInUser = await response.Content.ReadFromJsonAsync<User>();
            await localStorage.SetItemAsync("authToken", signedInUser.Token);
            await localStorage.SetItemAsync("user", signedInUser);
            Console.WriteLine($"User signed in");
            ((ApiAuthenticationStateProvider)authStateProvider).MarkUserAsAuthenticated(signedInUser.Id.ToString(), signedInUser.Role);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", signedInUser.Token);
            Console.WriteLine($"(AuthService signin) HttpClient requestheader auth: {httpClient.DefaultRequestHeaders.Authorization}");

            return signedInUser;
        }

        public async Task Signout()
        {
            await localStorage.RemoveItemAsync("authToken");
            await localStorage.RemoveItemAsync("user");
            ((ApiAuthenticationStateProvider)authStateProvider).MarkUserAsLoggedOut();
            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
