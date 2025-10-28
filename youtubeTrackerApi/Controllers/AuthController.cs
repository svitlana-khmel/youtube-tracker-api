using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using EyeTrackingApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (result.Succeeded)
        {
            return Ok(new { message = "Registration successful" });
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        //var user = await _userManager.FindByEmailAsync(model.Email);
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null) return Unauthorized();

        //var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        // if (!result.Succeeded) return Unauthorized();
        // var token = GenerateJwtToken(user);
        // return Ok(new { token });



        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            return Unauthorized("Invalid credentials");

        // ✅ Manually issue JWT
        var token = GenerateJwtToken(user);
        return Ok(new { token });


    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] RefreshDto dto)
    {
        if (string.IsNullOrEmpty(dto.Token))
            return BadRequest(new { ok = false, error = "No token provided" });

        var handler = new JwtSecurityTokenHandler();
        try
        {
            var jwtToken = handler.ReadJwtToken(dto.Token);

            // Optional: Validate token signature (skip lifetime)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var validationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = false // Important: allow expired tokens
            };

            var principal = handler.ValidateToken(dto.Token, validationParameters, out var validatedToken);

            // Get user email from claims
            var email = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (email == null) return Unauthorized();

            // Generate new token
            var user = _userManager.FindByEmailAsync(email).Result;
            if (user == null) return Unauthorized();

            var newToken = GenerateJwtToken(user);
            return Ok(new { token = newToken });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { ok = false, error = "Invalid token", details = ex.Message });
        }
    }

}


// [ApiController]
// [Route("api/[controller]")]
// public class AuthController : ControllerBase
// {
//     private readonly UserManager<ApplicationUser> _userManager;
//     private readonly SignInManager<ApplicationUser> _signInManager;
//     private readonly IConfiguration _configuration;

//     [HttpPost("login")]
//     public async Task<IActionResult> Login([FromBody] LoginModel model)
//     {
//         var user = await _userManager.FindByEmailAsync(model.Email);
//         if (user == null) return Unauthorized();

//         var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
//         if (!result.Succeeded) return Unauthorized();

//         var token = GenerateJwtToken(user);
//         return Ok(new { token });
//     }

//     private string GenerateJwtToken(ApplicationUser user)
//     {
//         var claims = new[]
//         {
//             new Claim(JwtRegisteredClaimNames.Sub, user.Email),
//             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//         };

//         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
//         var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//         var token = new JwtSecurityToken(
//             issuer: _configuration["Jwt:Issuer"],
//             audience: _configuration["Jwt:Audience"],
//             claims: claims,
//             expires: DateTime.Now.AddHours(1),
//             signingCredentials: creds
//         );

//         return new JwtSecurityTokenHandler().WriteToken(token);
//     }
// }