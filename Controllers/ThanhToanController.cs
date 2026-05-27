using Microsoft.AspNetCore.Mvc;
using QlChoThueNha1.Data;
using QlChoThueNha1.Models;
using System;
using System.Linq;

namespace QlChoThueNha1.Controllers
{
    public class ThanhToanController : Controller
    {
        private readonly AppDbContext _context;

        public ThanhToanController(AppDbContext context)
        {
            _context = context;
        }

        // Hiển thị trang thanh toán
        public IActionResult Index(int? nhaId)
        {
            var payments = _context.ThanhToans.ToList();

            return View(payments);
        }

        // Trang tạo thanh toán
        public IActionResult Create(int? nhaId)
        {
            ThanhToan model = new ThanhToan();

            if (nhaId != null)
            {
                model.NhaId = nhaId.Value;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ThanhToan thanhToan)
        {
            if (ModelState.IsValid)
            {
                thanhToan.NgayThanhToan = DateTime.Now;

                _context.ThanhToans.Add(thanhToan);

                _context.SaveChanges();

                TempData["Success"] = "Yêu cầu thanh toán đã gửi, chờ admin duyệt";

                return RedirectToAction("Index");
            }

            return View(thanhToan);
        }
        // ADMIN DUYỆT THANH TOÁN
        public IActionResult Duyet(int id)
        {
            var thanhToan = _context.ThanhToans.Find(id);

            if (thanhToan == null)
            {
                return NotFound();
            }

            thanhToan.TrangThai = "Đã thanh toán";

            _context.SaveChanges();

            TempData["Success"] = "Đã duyệt thanh toán";

            return RedirectToAction("Index");
        }

        // ADMIN TỪ CHỐI
        public IActionResult TuChoi(int id)
        {
            var thanhToan = _context.ThanhToans.Find(id);

            if (thanhToan == null)
            {
                return NotFound();
            }

            thanhToan.TrangThai = "Từ chối";

            _context.SaveChanges();

            TempData["Error"] = "Đã từ chối thanh toán";

            return RedirectToAction("Index");
        }
    }
}