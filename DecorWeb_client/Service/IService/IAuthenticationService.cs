using Decor_Models;

namespace DecorWeb_client.Service.IService
{
    public interface IAuthenticationService
    {
        Task<SignUpResponseDTO> RegisterUser(SignUpRequestDTO signUpRequest);
        Task<SignInResponseDTO> LoginUser(SignInRequestDTO signInRequest);
        Task Logout();
    }
}
