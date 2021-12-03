using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WishList.Data;
using WishList.Models;

namespace WishList.Controllers
{
    public class WishController : Controller
    {
        #region Fields

        private readonly AppDbContext _context;
        private List<Wisher> _wisherList;

        #endregion Fields

        #region Constructors

        public WishController(AppDbContext context)
        {
            _context = context;
            _wisherList = _context.Wishers.ToList();
        }

        #endregion Constructors

        #region Methods

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public IActionResult Create(Models.Wish item)
        {
            _context.Wishes.Add(item);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var wish = _context.Wishes.FirstOrDefault(e => e.WishId == id);
            _context.Wishes.Remove(wish);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            var model = _context.Wishes.Include("Wisher").OrderBy(w => w.Wisher.Name).ThenBy(w => w.WishOrder).ToList();
            //model.EmployeesList = data.Select(x => new Itemlist { Value = x.EmployeeId, Text = x.EmployeeName }).ToList();
            return View("Index", model);
        }

        public IActionResult WisherDropdown()
        {
            Wisher model = new Wisher();
            return View(model);
        }

        #endregion Methods
    }
}
