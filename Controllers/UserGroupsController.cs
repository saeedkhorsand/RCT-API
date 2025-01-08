using Microsoft.AspNetCore.Mvc;
using AspnetCoreMvcFull.Core.Context;
using AspnetCoreMvcFull.Models.User;
using Microsoft.EntityFrameworkCore;

namespace AspnetCoreMvcFull.Controllers
{
  public class UserGroupsController : Controller
  {
    private readonly ApplicationDbContext _context;

    public UserGroupsController(ApplicationDbContext context)
    {
      _context = context;
    }

    // GET: UserGroups
    public async Task<IActionResult> Index()
    {
      var userGroups = await _context.Groups.ToListAsync();
      return View(userGroups);
    }

    // GET: UserGroups/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
        return NotFound();

      var userGroup = await _context.Groups
          .Include(g => g.Users)
          .Include(g => g.Products)
          .FirstOrDefaultAsync(m => m.Id == id);

      if (userGroup == null)
        return NotFound();

      return View(userGroup);
    }

    // GET: UserGroups/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: UserGroups/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] UserGroup userGroup)
    {
      if (ModelState.IsValid)
      {
        _context.Add(userGroup);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return View(userGroup);
    }

    // GET: UserGroups/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
        return NotFound();

      var userGroup = await _context.Groups.FindAsync(id);
      if (userGroup == null)
        return NotFound();

      return View(userGroup);
    }

    // POST: UserGroups/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] UserGroup userGroup)
    {
      if (id != userGroup.Id)
        return NotFound();

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(userGroup);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!UserGroupExists(userGroup.Id))
            return NotFound();
          else
            throw;
        }
        return RedirectToAction(nameof(Index));
      }
      return View(userGroup);
    }

    // GET: UserGroups/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
        return NotFound();

      var userGroup = await _context.Groups
          .FirstOrDefaultAsync(m => m.Id == id);

      if (userGroup == null)
        return NotFound();

      return View(userGroup);
    }

    // POST: UserGroups/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var userGroup = await _context.Groups.FindAsync(id);
      _context.Groups.Remove(userGroup);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool UserGroupExists(int id)
    {
      return _context.Groups.Any(e => e.Id == id);
    }
  }
}
