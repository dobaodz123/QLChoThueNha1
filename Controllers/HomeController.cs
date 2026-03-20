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

        // GET: /Home/Index - Trang chá»§
        public IActionResult Index()
        {
            // Láº¥y nhÃ  má»›i Ä‘Äƒng (5 nhÃ  má»›i nháº¥t)
            var newHouses = _context.Houses
                .Include(h => h.HouseType)
                .Where(h => h.Status == "Available")
                .OrderByDescending(h => h.Id)
                .Take(6)
                .ToList();

            // Láº¥y danh sÃ¡ch loáº¡i nhÃ  cho search
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
        // GET: /Home/PriceChart
        public IActionResult PriceChart()
        {
            var houses = _context.Houses
                .Include(h => h.HouseType)
                .Where(h => h.Status == "Available")
                .ToList();

            // Nhóm theo loại nhà, tính giá trung bình
            var chartData = houses
                .GroupBy(h => h.HouseType?.Name ?? "Khác")
                .Select(g => new
                {
                    Type = g.Key,
                    AvgPrice = (long)g.Average(h => (double)h.Price),
                    MinPrice = (long)g.Min(h => (double)h.Price),
                    MaxPrice = (long)g.Max(h => (double)h.Price),
                    Count = g.Count()
                })
                .ToList();

            ViewBag.ChartData = System.Text.Json.JsonSerializer.Serialize(chartData);
            return View();
        }

        // GET: /Home/Support
        public IActionResult Support()
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