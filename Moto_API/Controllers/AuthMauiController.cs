using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Models.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Moto_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MotoDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        protected LoginResponseDTO _response;
        private string secretKey;
        private readonly IMapper _mapper;

        public AuthController(MotoDbContext db, IMapper mapper, IConfiguration configuration,
            UserManager<ApplicationUser> userManager/*, RoleManager<IdentityRole> roleManager*/)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
           // _roleManager = roleManager;
            this._response = new();
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<LoginResponseDTO> Login([FromBody] LoginRequestDTO loginDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginDto.UserName.ToLower());
            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.Unauthorized;
            }

            var isValidPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isValidPassword)
            {
                _response.StatusCode = HttpStatusCode.Unauthorized;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var roles = await _userManager.GetRolesAsync(user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // dodane id
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<UserDTO>(user),
                //UserId = user.Id,
                //Username = user.UserName
            };
            return loginResponseDTO;

        }

        //[HttpPost("register")]
        //[AllowAnonymous]
        //public async Task<IActionResult> Register(RegisterDto registerDto)
        //{
        //    ApplicationUser user = new()
        //    {
        //        UserName = registerDto.Username,
        //        Email = registerDto.Username,
        //        NormalizedEmail = registerDto.Username.ToUpper(),
        //        Name = registerDto.Name,
        //    };

        //    try
        //    {
        //        var result = await _userManager.CreateAsync(user, registerDto.Password);

        //        if (result.Succeeded)
        //        {
        //            if (!_roleManager.RoleExistsAsync("customer").GetAwaiter().GetResult())
        //            {
        //                await _roleManager.CreateAsync(new IdentityRole("customer"));
        //                //await _roleManager.CreateAsync(new IdentityRole("admin"));
        //            }

        //            await _userManager.AddToRoleAsync(user, "customer");
        //            var userToReturn = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == registerDto.Username);
        //            return Ok(userToReturn);
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }

        //    return Ok();
        //}
    }
}
