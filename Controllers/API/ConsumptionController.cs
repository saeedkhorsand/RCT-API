using AspnetCoreMvcFull.Core.Context;
using AspnetCoreMvcFull.Models;
using AspnetCoreMvcFull.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ConsumptionController : ControllerBase
{
  private readonly ApplicationDbContext _context;

  public ConsumptionController(ApplicationDbContext context)
  {
    _context = context;
  }

  // ثبت میزان مصرف
  [HttpPost("add")]
  public async Task<IActionResult> AddConsumption([FromBody] AddConsumptionRequest request)
  {
    var userId = GetUserId();

    var product = await _context.Products.FindAsync(request.ProductId);
    if (product == null)
    {
      return NotFound("Product not found.");
    }

    var consumption = new Consumption
    {
      UserId = userId,
      ProductId = request.ProductId ?? 0,
      ConsumptionTime = request.ConsumptionTime,
      Quantity = request.Quantity
    };

    _context.Consumptions.Add(consumption);
    await _context.SaveChangesAsync();

    return Ok("Consumption added successfully.");
  }

  // دریافت تاریخچه مصرف روز جاری
  [HttpGet("today")]
  public async Task<IActionResult> GetTodayConsumption()
  {
    var userId = GetUserId();
    var today = DateTime.Today;

    var consumptions = await _context.Consumptions
        .Where(c => c.UserId == userId && c.ConsumptionTime.Date == today)
        .Include(c => c.Product)
        .ToListAsync();

    if (!consumptions.Any())
    {
      return NotFound("No consumption records found for today.");
    }

    return Ok(consumptions);
  }

  // دریافت تاریخچه مصرف کامل
  [HttpPost("all")]
  public async Task<IActionResult> GetAllConsumptions(ConsumptionRequest request)
  {
    var userId = GetUserId();

    var consumptions = await _context.Consumptions
        .Where(c => c.UserId == userId &&
                  (c.ConsumptionTime.Date <= request.To ||
                  c.ConsumptionTime.Date >= request.From))
        .Include(c => c.Product)
        .OrderByDescending(c => c.ConsumptionTime)
        .ToListAsync();

    if (!consumptions.Any())
    {
      return NotFound("No consumption records found.");
    }

    return Ok(consumptions);
  }

  // بروزرسانی مصرف (فقط تا سه روز گذشته)
  [HttpPut("update/{id}")]
  public async Task<IActionResult> UpdateConsumption(int id, [FromBody] UpdateConsumptionRequest request)
  {
    var userId = GetUserId();

    var consumption = await _context.Consumptions
        .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

    if (consumption == null)
    {
      return NotFound("Consumption record not found.");
    }

    var threeDaysAgo = DateTime.Today.AddDays(-3);
    if (consumption.ConsumptionTime.Date < threeDaysAgo)
    {
      return BadRequest("You can only update consumption records from the last three days.");
    }

    consumption.Quantity = request.Quantity;
    consumption.ConsumptionTime = request.ConsumptionTime;

    _context.Consumptions.Update(consumption);
    await _context.SaveChangesAsync();

    return Ok("Consumption updated successfully.");
  }

  // متد کمکی برای گرفتن UserId از JWT
  private string GetUserId()
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (userIdClaim == null)
      throw new AuthenticationException("User ID not found in token.");
    return userIdClaim.ToString();
  }
}
