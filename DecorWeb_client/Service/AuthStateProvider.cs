using Blazored.LocalStorage;
using Decor_Common;
using DecorWeb_client.Helper;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json.Linq;
using System.Net.Security;
using System.Security.Claims;

namespace DecorWeb_client.Service
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        public AuthStateProvider (HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorageService.GetItemAsync<string>(SD.Local_Token);
            if (token == null)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
            return new AuthenticationState(new ClaimsPrincipal(new  ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token),"jwtAuthType")));
        }
        public void NotifyUseLogedIn(string Token)
        {
            var authenticationUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(Token), "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(authenticationUser));
            NotifyAuthenticationStateChanged(authState);
        }
        public void NotifyUseLogedOut()
        {
            var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
