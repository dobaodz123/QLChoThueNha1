using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using QlChoThueNha1.Data;
using QlChoThueNha1.Models;
using System.Linq;

namespace QlChoThueNha1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HouseTypeController : Controller
    {
        private readonly AppDbContext _context;

        public HouseTypeController(AppDbContext context)
        {
            _context = context;
        }

        // ================= DANH SÁCH =================
        public IActionResult Index()
        {
            var list = _context.HouseTypes.ToList();
            return View(list);
        }

        // ================= THÊM =================
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(HouseType model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.HouseTypes.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ================= SỬA =================
        public IActionResult Edit(int id)
        {
            var item = _context.HouseTypes.Find(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(HouseType model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.HouseTypes.Update(model);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ================= XÓA =================
        public IActionResult Delete(int id)
        {
            var item = _context.HouseTypes.Find(id);
            if (item == null) return NotFound();

            _context.HouseTypes.Remove(item);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}