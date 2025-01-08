using AspnetCoreMvcFull.Core.Context;
using AspnetCoreMvcFull.Models.User;
using AspnetCoreMvcFull.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspnetCoreMvcFull.Controllers;

public class ParticipantsController : Controller
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly RoleManager<IdentityRole> _roleManager;
  private readonly ApplicationDbContext _context;

  public ParticipantsController(
      UserManager<ApplicationUser> userManager,
      RoleManager<IdentityRole> roleManager,
      ApplicationDbContext context)
  {
    _userManager = userManager;
    _roleManager = roleManager;
    _context = context;
  }

  // نمایش لیست کاربران
  public async Task<IActionResult> Index()
  {
    var users = await _userManager.Users
        .Include(u => u.Group)
        .Select(u => new UserViewModel
        {
          Id = u.Id,
          UserName = u.UserName,
          Email = u.Email,
          PhoneNumber = u.PhoneNumber,
          Gender = u.Gender,
          GroupName = u.Group != null ? u.Group.Name : "-",
          IsLocked = u.LockoutEnabled && u.LockoutEnd > DateTimeOffset.UtcNow
        })
        .ToListAsync();

    return View(users);
  }

  // فرم ایجاد کاربر
  public IActionResult Create()
  {
    var model = new UserFormViewModel
    {
      Groups = _context.Groups.ToList()
    };
    return View(model);
  }

  [HttpPost]
  public async Task<IActionResult> Create(UserFormViewModel model)
  {
    if (!ModelState.IsValid)
    {
      model.Groups = _context.Groups.ToList();
      return View(model);
    }

    if (model.Password != model.ConfirmPassword)
    {
      ModelState.AddModelError("", "Passwords do not match.");
      model.Groups = _context.Groups.ToList();
      return View(model);
    }

    var user = new ApplicationUser
    {
      UserName = model.UserName,
      Email = model.Email,
      PhoneNumber = model.PhoneNumber,
      Gender = model.Gender,
      GroupId = model.GroupId
    };

    var result = await _userManager.CreateAsync(user, model.Password);
    if (!result.Succeeded)
    {
      foreach (var error in result.Errors)
        ModelState.AddModelError("", error.Description);
      model.Groups = _context.Groups.ToList();
      return View(model);
    }

    return RedirectToAction(nameof(Index));
  }

  // فرم ویرایش کاربر
  public async Task<IActionResult> Edit(string id)
  {
    var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
    if (user == null) return NotFound();

    var model = new UserFormViewModel
    {
      Id = user.Id,
      UserName = user.UserName,
      Email = user.Email,
      PhoneNumber = user.PhoneNumber,
      Gender = user.Gender,
      GroupId = user.GroupId ?? 1,
      IsLocked = user.LockoutEnabled && user.LockoutEnd > DateTimeOffset.UtcNow,
      Groups = _context.Groups.ToList()
    };

    return View(model);
  }

  [HttpPost]
  public async Task<IActionResult> Edit(UserFormViewModel model)
  {
    // Remove password validation if the operation is an edit and passwords are empty
    if (!model.IsNew && string.IsNullOrEmpty(model.Password) && string.IsNullOrEmpty(model.ConfirmPassword))
    {
      ModelState.Remove(nameof(model.Password));
      ModelState.Remove(nameof(model.ConfirmPassword));
    }

    if (!ModelState.IsValid)
    {
      model.Groups = _context.Groups.ToList();
      return View(model);
    }

    var user = model.IsNew
        ? new ApplicationUser()
        : await _userManager.FindByIdAsync(model.Id);

    if (user == null) return NotFound();

    // Update user fields
    user.UserName = model.UserName;
    user.Email = model.Email;
    user.PhoneNumber = model.PhoneNumber;
    user.Gender = model.Gender;
    user.GroupId = model.GroupId;
    user.LockoutEnabled = model.IsLocked;
    user.LockoutEnd = model.IsLocked ? DateTimeOffset.MaxValue : null;

    // Handle password change if provided
    if (!string.IsNullOrEmpty(model.Password))
    {
      var token = await _userManager.GeneratePasswordResetTokenAsync(user);
      var resetResult = await _userManager.ResetPasswordAsync(user, token, model.Password);

      if (!resetResult.Succeeded)
      {
        foreach (var error in resetResult.Errors)
          ModelState.AddModelError("", error.Description);
        model.Groups = _context.Groups.ToList();
        return View(model);
      }
    }

    // Save changes
    var result = model.IsNew
        ? await _userManager.CreateAsync(user, model.Password ?? string.Empty)
        : await _userManager.UpdateAsync(user);

    if (!result.Succeeded)
    {
      foreach (var error in result.Errors)
        ModelState.AddModelError("", error.Description);
      model.Groups = _context.Groups.ToList();
      return View(model);
    }

    return RedirectToAction(nameof(Index));
  }
}
