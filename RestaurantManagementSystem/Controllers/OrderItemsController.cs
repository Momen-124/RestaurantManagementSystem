using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Data;
using RestaurantManagementSystem.Models;

public class OrderItemsController : Controller
{
    private readonly AppDbContext _context;

    public OrderItemsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: ORDERITEMS
    public async Task<IActionResult> Index()
    {
        var items = await _context.OrderItems
            .Include(o => o.Order)
            .Include(o => o.MenuItem)
            .ToListAsync();
        return View(items);
    }

    // GET: ORDERITEMS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var orderItem = await _context.OrderItems
            .Include(o => o.Order)
            .Include(o => o.MenuItem)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (orderItem == null)
        {
            return NotFound();
        }

        return View(orderItem);
    }

    // GET: ORDERITEMS/Create
    public IActionResult Create()
    {
        ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "CustomerName");
        ViewData["MenuItemId"] = new SelectList(_context.MenuItems, "Id", "Name");
        return View();
    }

    // POST: ORDERITEMS/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,OrderId,MenuItemId,Quantity")] orderItem orderItem)
    {
        if (ModelState.IsValid)
        {
            _context.Add(orderItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "CustomerName", orderItem.OrderId);
        ViewData["MenuItemId"] = new SelectList(_context.MenuItems, "Id", "Name", orderItem.MenuItemId);
        return View(orderItem);
    }

    // GET: ORDERITEMS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var orderItem = await _context.OrderItems.FindAsync(id);
        if (orderItem == null)
        {
            return NotFound();
        }
        ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "CustomerName", orderItem.OrderId);
        ViewData["MenuItemId"] = new SelectList(_context.MenuItems, "Id", "Name", orderItem.MenuItemId);
        return View(orderItem);
    }

    // POST: ORDERITEMS/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,OrderId,MenuItemId,Quantity")] orderItem orderItem)
    {
        if (id != orderItem.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(orderItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemExists(orderItem.Id))
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
        ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "CustomerName", orderItem.OrderId);
        ViewData["MenuItemId"] = new SelectList(_context.MenuItems, "Id", "Name", orderItem.MenuItemId);
        return View(orderItem);
    }

    // GET: ORDERITEMS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var orderItem = await _context.OrderItems
            .Include(o => o.Order)
            .Include(o => o.MenuItem)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (orderItem == null)
        {
            return NotFound();
        }

        return View(orderItem);
    }

    // POST: ORDERITEMS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var orderItem = await _context.OrderItems.FindAsync(id);
        if (orderItem != null)
        {
            _context.OrderItems.Remove(orderItem);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool OrderItemExists(int? id)
    {
        return _context.OrderItems.Any(e => e.Id == id);
    }
}
