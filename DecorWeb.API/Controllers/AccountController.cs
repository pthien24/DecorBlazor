using Decor_Common;
using Decor_DataAccess;
using Decor_Models;
using DecorWeb.API.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DecorWeb.API.Controllers
{
    [Route("api/[controller]/[action]/")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly APISetting _apiSetting;
        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<APISetting> options) {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _apiSetting = options.Value;
        }
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestDTO signUpRequestDTO)
        {
            if (signUpRequestDTO == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = new ApplicationUser()
            {
                UserName = signUpRequestDTO.Email,
                Email = signUpRequestDTO.Email,
                Name = signUpRequestDTO.Name,
                PhoneNumber = signUpRequestDTO.PhoneNumber,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, signUpRequestDTO.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new SignUpResponseDTO()
                {
                    IsRegisterationSuccessful = false,
                    Errors = result.Errors.Select(u => u.Description)
                });
            }
            var RoleResult = await _userManager.AddToRoleAsync(user, SD.Role_Admin);
            if (!RoleResult.Succeeded)
            {
                return BadRequest(new SignUpResponseDTO()
                {
                    IsRegisterationSuccessful = false,
                    Errors = result.Errors.Select(u => u.Description).ToList()
                });
            }
            return StatusCode(201);
        }
        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestDTO signInRequestDTO)
        {
            if (signInRequestDTO == null || !ModelState.IsValid)
            {
                return BadRequest(new SignInResponseDTO
                {
                    IsAuthSuccessful = false,
                    ErrorMessage = "Invalid sign-in request data."
                });
            }
            var result = await _signInManager.PasswordSignInAsync(signInRequestDTO.UserName, signInRequestDTO.Password, false, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new SignInResponseDTO()
                {
                    IsAuthSuccessful = false,
                    ErrorMessage = "Invalid Authentication"
                });
            }
            var user = await _userManager.FindByNameAsync(signInRequestDTO.UserName);
            if(user == null)
            {
                return Unauthorized(new SignInResponseDTO()
                {
                    IsAuthSuccessful = false,
                    ErrorMessage = "user not found"
                });
            }
            var signinCredential = GetSignCredentials();
            var claims = await GetClaims(user);

            var tokenOption = new JwtSecurityToken(
                issuer: _apiSetting.ValidAudience,
                audience: _apiSetting.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: signinCredential);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOption);
            return Ok(new SignInResponseDTO()
            {
                IsAuthSuccessful = true,
                Token = token,
                UserDTO = new UserDTO()
                {
                    Name = user.Name,
                    Id = user.Id,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                }
            });
        }

        private SigningCredentials GetSignCredentials()
        {
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiSetting.SecretKey));
            return new SigningCredentials(secret,SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim("Id",user.Id)
            }; 
            var roles = await _userManager.GetRolesAsync(await _userManager.FindByEmailAsync(user.Email));
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
    }
}
