using AspnetCoreMvcFull.Core.Context;
using AspnetCoreMvcFull.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspnetCoreMvcFull.Controllers
{
  public class ConsumptionsController : Controller
  {
    private readonly ApplicationDbContext _context;

    public ConsumptionsController(ApplicationDbContext context)
    {
      _context = context;
    }

    public IActionResult Index(string userId, int? groupId, int? productId, DateTime? startDate, DateTime? endDate)
    {
      var consumptions = _context.Consumptions.Include(c => c.User).Include(c => c.Product).AsQueryable();

      // اعمال فیلترها
      if (!string.IsNullOrEmpty(userId))
      {
        consumptions = consumptions.Where(c => c.UserId == userId);
        ViewBag.FilterUserId = userId;
      }
      if (groupId.HasValue)
      {
        consumptions = consumptions.Where(c => c.User.GroupId == groupId);
        ViewBag.FilterGroupId = groupId;
      }
      if (productId.HasValue)
      {
        consumptions = consumptions.Where(c => c.ProductId == productId);
        ViewBag.FilterProductId = productId;
      }
      if (startDate.HasValue)
      {
        consumptions = consumptions.Where(c => c.ConsumptionTime >= startDate.Value);
        ViewBag.FilterStartDate = startDate.Value.ToString("yyyy-MM-dd");
      }
      if (endDate.HasValue)
      {
        consumptions = consumptions.Where(c => c.ConsumptionTime <= endDate.Value);
        ViewBag.FilterEndDate = endDate.Value.ToString("yyyy-MM-dd");
      }

      // ارسال تعداد نتایج
      ViewBag.RowCount = consumptions.Count();

      // ارسال داده‌ها
      ViewBag.Users = _context.Users.ToList();
      ViewBag.Groups = _context.Groups.ToList();
      ViewBag.Products = _context.Products.ToList();

      return View(consumptions.ToList());
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
      var consumption = _context.Consumptions
          .Include(c => c.User)
          .Include(c => c.Product)
          .FirstOrDefault(c => c.Id == id);

      if (consumption == null)
      {
        return NotFound();
      }

      ViewBag.Products = _context.Products.ToList();
      ViewBag.Users = _context.Users.ToList();
      ViewBag.Groups = _context.Groups.ToList();

      return View(consumption);
    }

    [HttpPost]
    public IActionResult EditConsumption(Consumption updatedConsumption)
    {
      if (!ModelState.IsValid)
      {
        ViewBag.Products = _context.Products.ToList();
        ViewBag.Users = _context.Users.ToList();
        ViewBag.Groups = _context.Groups.ToList();
        return View(updatedConsumption);
      }

      var existingConsumption = _context.Consumptions.FirstOrDefault(c => c.Id == updatedConsumption.Id);

      if (existingConsumption == null)
      {
        return NotFound();
      }

      existingConsumption.ProductId = updatedConsumption.ProductId;
      existingConsumption.Quantity = updatedConsumption.Quantity;
      existingConsumption.ConsumptionTime = updatedConsumption.ConsumptionTime;

      _context.SaveChanges();

      return RedirectToAction("Index");
    }


  }

}
