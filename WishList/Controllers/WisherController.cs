using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WishList.Data;
using WishList.Models;
using static System.String;

namespace WishList.Controllers
{
    public class WisherController : Controller
    {
        #region Fields

        private readonly AppDbContext _context;

        #endregion Fields

        #region Constructors

        public WisherController(AppDbContext context)
        {
            _context = context;
        }

        #endregion Constructors

        #region Methods

        // GET: Wishers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Wishers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WisherId,Name,BirthDate")] Wisher wisher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wisher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wisher);
        }

        // GET: Wishers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wisher = await _context.Wishers
                .FirstOrDefaultAsync(m => m.WisherId == id);
            if (wisher == null)
            {
                return NotFound();
            }

            return View(wisher);
        }

        // POST: Wishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wisher = await _context.Wishers.FindAsync(id);
            _context.Wishers.Remove(wisher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Wishers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wisher = await _context.Wishers
                .FirstOrDefaultAsync(m => m.WisherId == id);
            if (wisher == null)
            {
                return NotFound();
            }

            return View(wisher);
        }

        // GET: Wishers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wisher = await _context.Wishers.FindAsync(id);
            if (wisher == null)
            {
                return NotFound();
            }
            return View(wisher);
        }

        // POST: Wishers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WisherId,Name,BirthDate")] Wisher wisher)
        {
            if (id != wisher.WisherId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wisher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WisherExists(wisher.WisherId))
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
            return View(wisher);
        }

        // GET: Wishers
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["NameSortParm"] = IsNullOrEmpty(sortOrder) ? "NameDesc" : "";
            ViewData["BirthDateSortParm"] = sortOrder == "BirthDate" ? "BirthDateDesc" : "BirthDate";
            ViewData["WishesSortParm"] = sortOrder == "Wishes" ? "WishesDesc" : "Wishes";

            var wishers = from w in _context.Wishers
                          select w;
            switch (sortOrder)
            {
                case "NameDesc":
                    wishers = wishers.OrderByDescending(w => w.Name);
                    break;

                case "BirthDate":
                    wishers = wishers.OrderBy(w => w.BirthDate);
                    break;

                case "BirthDateDesc":
                    wishers = wishers.OrderByDescending(w => w.BirthDate);
                    break;

                case "Wishes":
                    wishers = wishers.OrderBy(w => w.WishesMade.Count).ThenBy(w => w.Name);
                    break;

                case "WishesDesc":
                    wishers = wishers.OrderByDescending(w => w.WishesMade.Count).ThenByDescending(w => w.Name);
                    break;

                default:
                    wishers = wishers.OrderBy(w => w.Name);
                    break;
            }
            return View(await wishers.Include("WishesMade").ToListAsync());
        }

        private bool WisherExists(int id)
        {
            return _context.Wishers.Any(e => e.WisherId == id);
        }

        #endregion Methods
    }
}
