// ===== FILE: HomeController.cs =====
// 👉 Quản lý các trang chính (Home, About, Contact, Chart)

using Microsoft.AspNetCore.Mvc; // MVC
using Microsoft.EntityFrameworkCore; // Query DB
using QlChoThueNha1.Data; // DbContext
using QlChoThueNha1.Models; // Model
using System.Diagnostics; // Debug lỗi

namespace QlChoThueNha1.Controllers
{
    // ===== CLASS =====
    // 👉 Xử lý request trang chính
    public class HomeController : Controller
    {
        // 👉 Kết nối database
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // ===== INDEX =====
        // 👉 Trang chủ
        public IActionResult Index()
        {
            // 👉 Lấy 6 nhà mới nhất (còn trống)
            var newHouses = _context.Houses
                .Include(h => h.HouseType) // JOIN loại nhà
                .Where(h => h.Status == "Available") // lọc còn trống
                .OrderByDescending(h => h.Id) // mới nhất
                .Take(6) // giới hạn 6
                .ToList();

            ViewBag.HouseTypes = _context.HouseTypes.ToList(); // danh sách loại nhà
            ViewBag.TotalAvailableHouses = _context.Houses.Count(h => h.Status == "Available"); // tổng nhà

            return View(newHouses); // truyền sang View
        }

        // 👉 Trang giới thiệu
        public IActionResult About()
        {
            return View();
        }

        // 👉 Trang liên hệ
        public IActionResult Contact()
        {
            return View();
        }

        // ===== PRICE CHART =====
        // 👉 Trang biểu đồ giá
        public IActionResult PriceChart()
        {
            var houses = _context.Houses
                .Include(h => h.HouseType)
                .Where(h => h.Status == "Available")
                .ToList();

            // 👉 Nhóm theo loại + thống kê giá
            var chartData = houses
                .GroupBy(h => h.HouseType?.Name ?? "Khác")
                .Select(g => new
                {
                    Type = g.Key, // loại nhà
                    AvgPrice = (long)g.Average(h => (double)h.Price), // giá TB
                    MinPrice = (long)g.Min(h => (double)h.Price), // giá thấp
                    MaxPrice = (long)g.Max(h => (double)h.Price), // giá cao
                    Count = g.Count() // số lượng
                })
                .ToList();

            ViewBag.ChartData = System.Text.Json.JsonSerializer.Serialize(chartData); // đưa sang JS

            return View();
        }

        // 👉 Trang hỗ trợ
        public IActionResult Support()
        {
            return View();
        }

        // 👉 Trang chính sách
        public IActionResult Privacy()
        {
            return View();
        }

        // ===== ERROR =====
        // 👉 Trang lỗi
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier // ID debug
            });
        }
    }
}