using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspnetCoreMvcFull.Models;
using AspnetCoreMvcFull.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AspnetCoreMvcFull.Controllers;

public class AuthController : Controller
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly SignInManager<ApplicationUser> _signInManager;
  private readonly IConfiguration _configuration;

  public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
  {
    _userManager = userManager;
    _signInManager = signInManager;
    _configuration = configuration;
  }


  [HttpPost("Register")]
  public async Task<IActionResult> Register([FromBody] RegisterModel model)
  {
    var user = new ApplicationUser
    {
      UserName = model.Email,
      Email = model.Email,
      Gender = model.Gender,
      GroupId = model.GroupId
    };

    var result = await _userManager.CreateAsync(user, model.Password);

    if (!result.Succeeded)
      return BadRequest(result.Errors);

    return Ok("User registered successfully.");
  }

  [HttpPost("Login")]
  public async Task<IActionResult> Login([FromBody] LoginModel model)
  {
    var user = await _userManager.FindByEmailAsync(model.Email);
    if (user == null)
      return Unauthorized();

    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

    if (!result.Succeeded)
      return Unauthorized();

    var token = GenerateJwtToken(user);
    return Ok(new { Token = token });
  }


  private string GenerateJwtToken(ApplicationUser user)
  {
    var claims = new[]
    {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(30),
        signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }




  public IActionResult ForgotPasswordBasic() => View();
  public IActionResult LoginBasic() => View();
  public IActionResult RegisterBasic() => View();
}


public class RegisterModel
{
  public string Email { get; set; }
  public string Password { get; set; }
  public string Gender { get; set; }
  public int GroupId { get; set; }
}

public class LoginModel
{
  public string Email { get; set; }
  public string Password { get; set; }
}

