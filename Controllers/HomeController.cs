using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QlChoThueNha1.Data;
using QlChoThueNha1.Models;
using System.Diagnostics;

namespace QlChoThueNha1.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Home/Index - Trang chủ
        public IActionResult Index()
        {
            // Lấy nhà mới đăng (5 nhà mới nhất)
            var newHouses = _context.Houses
                .Include(h => h.HouseType)
                .Where(h => h.Status == "Available")
                .OrderByDescending(h => h.Id)
                .Take(6)
                .ToList();

            // Lấy danh sách loại nhà cho search
            ViewBag.HouseTypes = _context.HouseTypes.ToList();
            ViewBag.TotalAvailableHouses = _context.Houses.Count(h => h.Status == "Available");

            return View(newHouses);
        }

        // GET: /Home/About
        public IActionResult About()
        {
            return View();
        }

        // GET: /Home/Contact
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}