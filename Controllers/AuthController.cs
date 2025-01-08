using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspnetCoreMvcFull.Models;
using AspnetCoreMvcFull.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AspnetCoreMvcFull.Models.ViewModels;

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

  [HttpPost]
  public async Task<IActionResult> Login(LoginRequest model)
  {
    if (!ModelState.IsValid)
    {
      return View("LoginBasic", model);
    }
    model.RememberMe = true;
    // Find user by email or username
    var user = await _userManager.FindByEmailAsync(model.Username)
               ?? await _userManager.FindByNameAsync(model.Username);

    if (user == null)
    {
      ModelState.AddModelError(string.Empty, "Invalid login attempt.");
      return View("LoginBasic", model);
    }

    // Attempt to sign in
    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe ?? false, lockoutOnFailure: false);

    if (result.Succeeded)
    {
      await _signInManager.SignInAsync(user, isPersistent: model.RememberMe ?? false);
      return RedirectToAction("Index", "Dashboards");
    }

    if (result.IsLockedOut)
    {
      ModelState.AddModelError(string.Empty, "User account locked.");
    }
    else
    {
      ModelState.AddModelError(string.Empty, "Invalid login attempt.");
    }

    return View("LoginBasic", model);
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

