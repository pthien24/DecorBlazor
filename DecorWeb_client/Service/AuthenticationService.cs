using Blazored.LocalStorage;
using Decor_Common;
using Decor_Models;
using DecorWeb_client.Service.IService;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DecorWeb_client.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
		private readonly AuthenticationStateProvider _autheticationStateProvider;
        public AuthenticationService(HttpClient httpClient ,ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _autheticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;

        }
        public async Task<SignInResponseDTO> LoginUser(SignInRequestDTO signInRequest)
        {
            var content = JsonConvert.SerializeObject(signInRequest);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/account/signin", bodyContent);
            var contentTemp = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(contentTemp))
            {
                Console.WriteLine("Login response is empty.");
                return new SignInResponseDTO
                {
                    IsAuthSuccessful = false,
                    ErrorMessage = "Server returned an empty response."
                };
            }
            var result = JsonConvert.DeserializeObject<SignInResponseDTO>(contentTemp);
            if (response.IsSuccessStatusCode)
            {
                await _localStorage.SetItemAsync(SD.Local_Token, result.Token);
                await _localStorage.SetItemAsync(SD.Local_UserDetails, result.UserDTO);
                ((AuthStateProvider)_autheticationStateProvider).NotifyUseLogedIn(result.Token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
                return new SignInResponseDTO() { IsAuthSuccessful = true };
            }
            else
            {
                return result;
            }
            
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync(SD.Local_Token);
            await _localStorage.RemoveItemAsync(SD.Local_UserDetails);
            ((AuthStateProvider)_autheticationStateProvider).NotifyUseLogedOut();

            _httpClient.DefaultRequestHeaders.Authorization = null;

        }

        public async Task<SignUpResponseDTO> RegisterUser(SignUpRequestDTO signUpRequest)
        {
            var content = JsonConvert.SerializeObject(signUpRequest);
            var bodycontent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/account/signup", bodycontent);
            var contentTemp = await response.Content.ReadAsStringAsync();

            SignUpResponseDTO result = null;

            try
            {
                result = JsonConvert.DeserializeObject<SignUpResponseDTO>(contentTemp);
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"Deserialization error: {ex.Message}");
                return new SignUpResponseDTO
                {
                    IsRegisterationSuccessful = false,
                    Errors = new List<string> { "Unable to process the server response." }
                };
            }

            if (response.IsSuccessStatusCode)
            {
                return new SignUpResponseDTO() { IsRegisterationSuccessful = true };
            }
            else
            {
                return new SignUpResponseDTO()
                {
                    IsRegisterationSuccessful = false,
                    Errors = result?.Errors 
                };
            }
        }
    }
}
