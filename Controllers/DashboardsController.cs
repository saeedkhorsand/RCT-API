using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspnetCoreMvcFull.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AspnetCoreMvcFull.Controllers;

[Authorize]
public class DashboardsController : Controller
{
  public IActionResult Index()
  {
    return View();
  }
}
