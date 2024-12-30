using AspnetCoreMvcFull.Core.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
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
    var user = await _context.Users.FindAsync(request.UserId);
    if (user == null)
    {
      return NotFound("User not found.");
    }

    var product = await _context.Products.FindAsync(request.ProductId);
    if (product == null)
    {
      return NotFound("Product not found.");
    }

    var consumption = new Consumption
    {
      UserId = request.UserId,
      ProductId = request.ProductId,
      ConsumptionTime = request.ConsumptionTime,
      Quantity = request.Quantity
    };

    _context.Consumptions.Add(consumption);
    await _context.SaveChangesAsync();

    return Ok("Consumption added successfully.");
  }

  // دریافت تاریخچه مصرف روز جاری
  [HttpGet("today/{userId}")]
  public async Task<IActionResult> GetTodayConsumption(int userId)
  {
    var today = DateTime.Today;

    var consumptions = await _context.Consumptions
        .Where(c => c.UserId == userId && c.ConsumptionTime.Date == today)
        .Include(c => c.Product)
        .ToListAsync();

    if (consumptions == null || !consumptions.Any())
    {
      return NotFound("No consumption records found for today.");
    }

    return Ok(consumptions);
  }

  // دریافت تاریخچه مصرف کامل
  [HttpGet("all/{userId}")]
  public async Task<IActionResult> GetAllConsumptions(int userId)
  {
    var consumptions = await _context.Consumptions
        .Where(c => c.UserId == userId)
        .Include(c => c.Product)
        .OrderByDescending(c => c.ConsumptionTime)
        .ToListAsync();

    if (consumptions == null || !consumptions.Any())
    {
      return NotFound("No consumption records found.");
    }

    return Ok(consumptions);
  }

  // بروزرسانی مصرف (فقط تا سه روز گذشته)
  [HttpPut("update/{id}")]
  public async Task<IActionResult> UpdateConsumption(int id, [FromBody] UpdateConsumptionRequest request)
  {
    var consumption = await _context.Consumptions.FindAsync(id);
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
}
