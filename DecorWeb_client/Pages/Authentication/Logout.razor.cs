using DecorWeb_client.Service.IService;
using Microsoft.AspNetCore.Components;

namespace DecorWeb_client.Pages.Authentication
{
    public partial class Logout : ComponentBase
    {
        [Inject]
        public IAuthenticationService _authService { get; set; }

        [Inject]
        public NavigationManager _navigation { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await _authService.Logout();
            _navigation.NavigateTo("/");
        }
    }
}
