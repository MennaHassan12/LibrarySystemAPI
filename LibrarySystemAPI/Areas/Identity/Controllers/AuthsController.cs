using LibrarySystemAPI.Models;
using LibrarySystemAPI.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibrarySystemAPI.Areas.Identity.Controllers
{
    [ApiController]
    [Area("Identity")]
    [Route("api/[area]/[controller]")]
    public class AuthsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthsController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        // 1. تسجيل حساب جديد (Register)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var userExists = await _userManager.FindByEmailAsync(dto.Email);
            if (userExists != null)
                return BadRequest(new AuthResponseDto(false, "User already exists!"));

            var user = new ApplicationUser
            {
                Email = dto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = dto.Email,
                FullName = dto.FullName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(new AuthResponseDto(false, "User creation failed! Check password strength."));

            return Ok(new AuthResponseDto(true, "User created successfully!"));
        }

        // 2. تسجيل الدخول وتوليد الـ Token (Login)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                // توليد الـ Token للمستخدم
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YOUR_VERY_LONG_SECRET_KEY_HERE_12345"));

                var token = new JwtSecurityToken(
                    issuer: "LibrarySystemAPI",
                    audience: "LibrarySystemFrontend",
                    expires: DateTime.Now.AddDays(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new AuthResponseDto(true, "Login successful", new JwtSecurityTokenHandler().WriteToken(token)));
            }
            return Unauthorized(new AuthResponseDto(false, "Invalid credentials"));
        }
    }
}