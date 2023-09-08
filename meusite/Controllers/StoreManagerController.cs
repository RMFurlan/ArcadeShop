using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using meusite.Data;
using meusite.Models;
using Microsoft.AspNetCore.Authorization;
using meusite.Migrations;
using Microsoft.AspNetCore.Identity;

namespace meusite.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class StoreManagerController : Controller
    {
        private MeuSiteContext _context;

        public StoreManagerController(MeuSiteContext context)
        {
            _context = context;
        }
        // GET: StoreManager
        public async Task<IActionResult> Index()
        {
            var items = _context.Items
                .Include(i => i.Category)
                .Include(i => i.Producer);
            return View(await items.ToListAsync());
        }

        // GET: StoreManager/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.Producer)
                .FirstOrDefaultAsync(i => i.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // GET: StoreManager/Create
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "Name");
            ViewBag.ProducerId = new SelectList(_context.Producers, "ProducerId", "Name");
            return View();
        }

        // POST: StoreManager/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,CategoryId,ProducerId,Title,Price,Description,ItemArtUrl,ItemArtUrl2,ItemArtUrl3,ItemArtUrl4")] Item item)
        {
                _context.Add(item); 
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: StoreManager/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "Name", item.CategoryId);
            ViewBag.ProducerId = new SelectList(_context.Producers, "ProducerId", "Name", item.ProducerId);
            return View(item);
        }

        // POST: StoreManager/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("ItemId,CategoryId,ProducerId,Title,Price,Description,ItemArtUrl,ItemArtUrl2,ItemArtUrl3,ItemArtUrl4")] Item item)
        {
                _context.Update(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: StoreManager/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.Producer)
                .FirstOrDefaultAsync(i => i.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: StoreManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Items == null)
            {
                return Problem("Entity set 'MeuSiteContext.Items'  is null.");
            }
            Item item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
          return (_context.Items?.Any(e => e.ItemId == id)).GetValueOrDefault();
        }
    }
}
