
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Data;
using RestaurantManagementSystem.Models;


[Authorize(Roles = "Admin")]
public class MenuitemsController : Controller
{
    private readonly AppDbContext _context;

    public MenuitemsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: MENUITEMS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.MenuItems.ToListAsync());
    }

    // GET: MENUITEMS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var menuitem = await _context.MenuItems
            .FirstOrDefaultAsync(m => m.Id == id);
        if (menuitem == null)
        {
            return NotFound();
        }

        return View(menuitem);
    }

    // GET: MENUITEMS/Create
    public IActionResult Create()
    {
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
        return View();
    }

    // POST: MENUITEMS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Price,ImagePath,CategoryId")] Menuitem menuitem , IFormFile? imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine("wwwroot/images/menu", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                menuitem.ImagePath = "/images/menu/" + fileName;
            }

            _context.Add(menuitem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", menuitem.CategoryId);
        return View(menuitem);
    }

    // GET: MENUITEMS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var menuitem = await _context.MenuItems.FindAsync(id);
        if (menuitem == null)
        {
            return NotFound();
        }
        return View(menuitem);
    }

    // POST: MENUITEMS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Price,ImagePath,CategoryId,Category")] Menuitem menuitem)
    {
        if (id != menuitem.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(menuitem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuitemExists(menuitem.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(menuitem);
    }

    // GET: MENUITEMS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var menuitem = await _context.MenuItems
            .FirstOrDefaultAsync(m => m.Id == id);
        if (menuitem == null)
        {
            return NotFound();
        }

        return View(menuitem);
    }

    // POST: MENUITEMS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var menuitem = await _context.MenuItems.FindAsync(id);
        if (menuitem != null)
        {
            _context.MenuItems.Remove(menuitem);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MenuitemExists(int? id)
    {
        return _context.MenuItems.Any(e => e.Id == id);
    }
}
