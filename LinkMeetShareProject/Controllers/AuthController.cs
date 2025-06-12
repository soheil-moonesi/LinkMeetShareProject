using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LinkMeetShareProject.Dto;
using LinkMeetShareProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShareLib;
using LoginDto = LinkMeetShareProject.Dto.LoginDto;

namespace LinkMeetShareProject.Controllers
{

    //todo: p1 : refactor all codebase

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
        private readonly ILogger<AuthController> _logger;
        public AuthController(IConfiguration configuration, UserManager<ApiUser> userManager,
            ILogger<AuthController> logger,UserDtoMapper userDtoMapper)
        {
            _configuration = configuration;
            _userManager = userManager;
            _userDtoMapper = userDtoMapper;
            _logger = logger;
        }

        [HttpPost("gentok")]
        public async Task<string> GenerateToken(ApiUser _user)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception($"Error generating token: {ex.Message}", ex);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto loginDto)
        {
            try
            {
                if (loginDto == null)
                {
                    return BadRequest("Invalid login request");
                }

                var _user = await _userManager.FindByEmailAsync(loginDto.email);
                if (_user == null)
                {
                    return Unauthorized($"User not found with email: {loginDto.email}");
                }
                
                bool isValidUser = await _userManager.CheckPasswordAsync(_user, loginDto.password);
                if (!isValidUser)
                {
                    return Unauthorized("Invalid password");
                }

                var token = await GenerateToken(_user);
                var refreshToken = await CreateRefreshToken(_user);
                
                return Ok(new AuthResponseDto
                {
                    Token = token,
                    UserId = _user.Id,
                    RefreshToken = refreshToken
                });
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public async Task<string> Register(ApiUserDto userDto)
	        //        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)

		{
			_user = _userDtoMapper.dtoToUser(userDto);

          //_user = new ApiUser();
          //_user.Email=userDto.Email;
          // _user.FirstName=userDto.FirstName;
          //  _user.LastName =userDto.LastName;
          //  _user.UserName = userDto.Email;
          
            var result = await _userManager.CreateAsync(_user, userDto.password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, "User");

            }
            //return result.Errors;
            return "okey";
        }

        [HttpPost("rftoken")]
        public async Task<string> CreateRefreshToken(ApiUser _user)
        {
            await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user,
                _loginProvider, _refreshToken);
            var result = await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider,
                _refreshToken, newRefreshToken);
            return newRefreshToken;
        }

        [HttpGet("vr")]
        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {
	        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
	        var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
	        var username = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email)
		        ?.Value;
	        _user = await _userManager.FindByNameAsync(username);

	        if (_user == null || _user.Id != request.UserId)
	        {
		        return null;
	        }

	        var isValidRefreshToken =
		        await _userManager.VerifyUserTokenAsync(_user, _loginProvider, _refreshToken, request.RefreshToken);
	        if (isValidRefreshToken)
	        {
		        var token = await GenerateToken(_user);
		        return new AuthResponseDto()
		        {
			        Token = token,
			        UserId = _user.Id,
			        RefreshToken = await CreateRefreshToken(_user)
		        };
	        }

	        await _userManager.UpdateSecurityStampAsync(_user);
	        return null;
        }

        [Authorize]
        [HttpPost("test")]
        public async Task<string> testcon()
        {
	        return "ss";
        }


	}
}
