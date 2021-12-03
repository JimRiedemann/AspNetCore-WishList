using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WishList.Data;
using WishList.Models;

namespace WishList.Controllers
{
    public class WishController : Controller
    {
        #region Fields

        private readonly AppDbContext _context;

        #endregion Fields

        #region Constructors

        public WishController(AppDbContext context)
        {
            _context = context;
        }

        #endregion Constructors

        #region Methods

        // GET: Wishes/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["WisherId"] = new SelectList(_context.Wishers, "WisherId", "Name");
            return View("Create");
        }

        // POST: Wishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WishId,WisherId,WishOrder,Description")] Wish wish)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WisherId"] = new SelectList(_context.Wishers, "WisherId", "Name", wish.WisherId);
            return View(wish);
        }

        // GET: Wishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wish = await _context.Wishes
                .Include(w => w.Wisher)
                .FirstOrDefaultAsync(m => m.WishId == id);
            if (wish == null)
            {
                return NotFound();
            }

            return View(wish);
        }

        // POST: Wishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wish = await _context.Wishes.FindAsync(id);
            _context.Wishes.Remove(wish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Wishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wish = await _context.Wishes
                .Include(w => w.Wisher)
                .FirstOrDefaultAsync(m => m.WishId == id);
            if (wish == null)
            {
                return NotFound();
            }

            return View(wish);
        }

        // GET: Wishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wish = await _context.Wishes.FindAsync(id);
            if (wish == null)
            {
                return NotFound();
            }
            ViewData["WisherId"] = new SelectList(_context.Wishers, "WisherId", "Name", wish.WisherId);
            return View(wish);
        }

        // POST: Wishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WishId,WisherId,WishOrder,Description")] Wish wish)
        {
            if (id != wish.WishId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WishExists(wish.WishId))
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
            ViewData["WisherId"] = new SelectList(_context.Wishers, "WisherId", "Name", wish.WisherId);
            return View(wish);
        }

        // GET: Wishes
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Wishes
                .Include(w => w.Wisher)
                .OrderBy(w => w.Wisher.Name)
                .ThenBy(w => w.WishOrder);
            return View(await appDbContext.ToListAsync());
        }

        private bool WishExists(int id)
        {
            return _context.Wishes.Any(e => e.WishId == id);
        }

        #endregion Methods
    }
}
