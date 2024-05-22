using Decor_Models;
using DecorWeb_client.Service;
using DecorWeb_client.Service.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DecorWeb_client.Pages.Authentication
{
    public partial class RedirectToLogin : ComponentBase
    {
        [CascadingParameter]
        public Task<AuthenticationState> _authState { get; set; }

        [Inject]
        public NavigationManager _navigation { get; set; }
        bool notAuthorized { get; set; } = false;
        protected override async Task OnInitializedAsync()
        {
            var authstate = await _authState;
            if (authstate?.User?.Identity is null || !authstate.User.Identity.IsAuthenticated) {
                var returnUrl = _navigation.ToBaseRelativePath(_navigation.Uri);
                if (string.IsNullOrEmpty(returnUrl))
                {
                    _navigation.NavigateTo("login");
                }
                else
                {
                    _navigation.NavigateTo($"login?returnUrl={returnUrl}");
                }
            }
            else
            {
                notAuthorized = true;   
            }
        }
    }
}
