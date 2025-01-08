using Microsoft.AspNetCore.Mvc;
using AspnetCoreMvcFull.Core.Context;
using AspnetCoreMvcFull.Models.User;
using Microsoft.EntityFrameworkCore;
using AspnetCoreMvcFull.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspnetCoreMvcFull.Models.ViewModels;

namespace AspnetCoreMvcFull.Controllers
{
  public class ProductsController : Controller
  {
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
      _context = context;
    }

    // GET: Products
    public async Task<IActionResult> Index()
    {
      var products = await _context.Products
          .Include(p => p.Group)
          .ToListAsync();
      return View(products);
    }

    // GET: Products/Create
    public async Task<IActionResult> Create()
    {
      ViewData["GroupId"] = new SelectList(await _context.Groups.ToListAsync(), "Id", "Name");
      return View();
    }

    // POST: Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,GroupId")] ProductRequest productRequest)
    {
      Product product = new Product
      {
        Id = productRequest.Id,
        GroupId = productRequest.GroupId,
        Name = productRequest.Name
      };

      if (ModelState.IsValid)
      {
        _context.Add(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      ViewData["GroupId"] = new SelectList(await _context.Groups.ToListAsync(), "Id", "Name", product.GroupId);
      return View(product);
    }

    // GET: Products/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
        return NotFound();

      var product = await _context.Products.FindAsync(id);
      if (product == null)
        return NotFound();

      ViewData["GroupId"] = new SelectList(await _context.Groups.ToListAsync(), "Id", "Name", product.GroupId);
      return View(product);
    }

    // POST: Products/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,GroupId")] ProductRequest productRequest)
    {
      if (id != productRequest.Id)
        return NotFound();

      Product product = new Product
      {
        Id = productRequest.Id,
        GroupId = productRequest.GroupId,
        Name = productRequest.Name
      };

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(product);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!ProductExists(product.Id))
            return NotFound();
          else
            throw;
        }
        return RedirectToAction(nameof(Index));
      }
      ViewData["GroupId"] = new SelectList(await _context.Groups.ToListAsync(), "Id", "Name", product.GroupId);
      return View(product);
    }

    // GET: Products/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
        return NotFound();

      var product = await _context.Products
          .Include(p => p.Group)
          .FirstOrDefaultAsync(m => m.Id == id);
      if (product == null)
        return NotFound();

      return View(product);
    }

    // POST: Products/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var product = await _context.Products.FindAsync(id);
      _context.Products.Remove(product);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(int id)
    {
      return _context.Products.Any(e => e.Id == id);
    }
  }
}
