using AspnetCoreMvcFull.Models.User;
using AspnetCoreMvcFull.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly SignInManager<ApplicationUser> _signInManager;
  private readonly IConfiguration _configuration;

  public AccountController(
      UserManager<ApplicationUser> userManager,
      SignInManager<ApplicationUser> signInManager,
      IConfiguration configuration)
  {
    _userManager = userManager;
    _signInManager = signInManager;
    _configuration = configuration;
  }

  // لاگین
  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginRequest request)
  {
    var user = await _userManager.FindByNameAsync(request.Username);
    if (user == null)
    {
      return Unauthorized(new ResultViewModel("Invalid username or password.",false));
    }

    var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
    if (!result.Succeeded)
    {
      return Unauthorized(new ResultViewModel("Invalid username or password.", false));
    }

    var accessToken = GenerateJwtToken(user);
    var refreshToken = GenerateRefreshToken();

    // ذخیره Refresh Token در دیتابیس (یا هر محل دیگری)
    user.SecurityStamp = refreshToken;
    await _userManager.UpdateAsync(user);

    return Ok(new
    {
      AccessToken = accessToken,
      RefreshToken = refreshToken
    });
  }

  [HttpPost("Register")]
  public async Task<IActionResult> Register([FromBody] RegisterModel model)
  {
    if (!ModelState.IsValid)
      return BadRequest(new ResultViewModel(ModelState));

    // ایجاد یک کاربر جدید
    var user = new ApplicationUser
    {
      UserName = model.Username,
      Email = model.Email
    };

    // اضافه کردن کاربر به دیتابیس
    var result = await _userManager.CreateAsync(user, model.Password);

    if (!result.Succeeded)
      return BadRequest(new ResultViewModel(result));

    return Ok(new ResultViewModel("User registered successfully"));
  }

  // دریافت پروفایل کاربر
  [HttpGet("profile")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<IActionResult> GetProfile()
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var user = await _userManager.Users
        .Include(u => u.Group)
        .FirstOrDefaultAsync(u => u.Id == userId);

    if (user == null)
    {
      return NotFound(new ResultViewModel("User not found.",false));
    }

    return Ok(new
    {
      user.Id,
      user.UserName,
      user.Email,
      user.Gender,
      Group = new
      {
        Id = user.Group?.Id,
        Name = user.Group?.Name,
      }
    });
  }

  // رفرش توکن
  [HttpPost("refresh-token")]
  public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
  {
    var user = await _userManager.Users.FirstOrDefaultAsync(u => u.SecurityStamp == request.RefreshToken);
    if (user == null)
    {
      return Unauthorized(new ResultViewModel("Invalid refresh token.",false));
    }

    var accessToken = GenerateJwtToken(user);
    var newRefreshToken = GenerateRefreshToken();

    // ذخیره Refresh Token جدید
    user.SecurityStamp = newRefreshToken;
    await _userManager.UpdateAsync(user);

    return Ok(new
    {
      AccessToken = accessToken,
      RefreshToken = newRefreshToken
    });
  }

  // تولید JWT
  private string GenerateJwtToken(ApplicationUser user)
  {
    var claims = new[]
    {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(30),
        signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  // تولید Refresh Token
  private string GenerateRefreshToken()
  {
    return Guid.NewGuid().ToString();
  }
}
