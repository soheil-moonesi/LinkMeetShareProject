using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LinkMeetShareProject.Dto;
using LinkMeetShareProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LinkMeetShareProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly UserManager<ApiUser> _userManager;
        private ApiUser _user;
        private UserDtoMapper _userDtoMapper;
        private const string _loginProvider = "LinkMeetShare";
        private const string _refreshToken = "RefreshToken";
        public AuthController(IConfiguration configuration, UserManager<ApiUser> userManager,
            ILogger<AuthController> logger,UserDtoMapper userDtoMapper)
        {
            _configuration = configuration;
            _userManager = userManager;
            _userDtoMapper = userDtoMapper;
        }

        [HttpPost]
        public async Task<string> GenerateToken(ApiUser _user, IConfiguration _configuration)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(_user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(_user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                new Claim("uid", _user.Id)
            }.Union(userClaims).Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("login")]
        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            var _user = await _userManager.FindByEmailAsync(loginDto.Email);
            bool isValidUser = await _userManager.CheckPasswordAsync(_user, loginDto.Password);
            var token = await GenerateToken(_user, _configuration);
            return new AuthResponseDto()
            {
                Token = token,
                UserId = _user.Id,
                RefreshToken = await CreateRefreshToken(_user)
            };
        }

        [HttpPost("Register")]
        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        {
            _user = _userDtoMapper.dtoToUser(userDto);

          //_user = new ApiUser();
          //_user.Email=userDto.Email;
          // _user.FirstName=userDto.FirstName;
          //  _user.LastName =userDto.LastName;
          //  _user.UserName = userDto.Email;
          
            var result = await _userManager.CreateAsync(_user, userDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, "User");

            }
            return result.Errors;
        }


        public async Task<string> CreateRefreshToken(ApiUser _user)
        {
            await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user,
                _loginProvider, _refreshToken);
            var result = await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider,
                _refreshToken, newRefreshToken);
            return newRefreshToken;
        }





    }
}
