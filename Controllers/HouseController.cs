// ===== FILE: HouseController.cs =====
// 👉 Quản lý nhà (xem, thêm, sửa, xóa, duyệt thuê)

using Microsoft.AspNetCore.Authorization; // ✅ Phân quyền (Admin)
using Microsoft.AspNetCore.Mvc; // MVC
using Microsoft.EntityFrameworkCore; // Query DB
using QlChoThueNha1.Data; // DbContext
using QlChoThueNha1.Models; // Model

public class HouseController : Controller
{
    // 👉 Kết nối database
    private readonly AppDbContext _context;

    public HouseController(AppDbContext context)
    {
        _context = context;
    }

    // ===== INDEX =====
    // 👉 Trang danh sách nhà + tìm kiếm + lọc
    public IActionResult Index(string keyword, int? houseTypeId)
    {
        var houses = _context.Houses
            .Include(h => h.HouseType) // JOIN loại nhà
            .AsQueryable(); // cho phép filter tiếp

        // 👉 Tìm theo tên / địa chỉ
        if (!string.IsNullOrEmpty(keyword))
            houses = houses.Where(h => h.Address.Contains(keyword) || h.Name.Contains(keyword));

        // 👉 Lọc theo loại nhà
        if (houseTypeId.HasValue)
            houses = houses.Where(h => h.HouseTypeId == houseTypeId);

        ViewBag.HouseTypes = _context.HouseTypes.ToList(); // dữ liệu filter
        return View(houses.ToList()); // trả về danh sách
    }

    // ===== DETAILS =====
    // 👉 Xem chi tiết 1 nhà
    public IActionResult Details(int id)
    {
        var house = _context.Houses
            .Include(h => h.HouseType)
            .FirstOrDefault(h => h.Id == id);

        if (house == null) return NotFound(); // không tồn tại

        return View(house);
    }

    // ===== CREATE GET =====
    // 👉 Mở form thêm nhà (Admin)
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        ViewBag.HouseTypes = _context.HouseTypes.ToList();
        return View();
    }

    // ===== CREATE POST =====
    // 👉 Thêm nhà vào DB
    [HttpPost]
    [ValidateAntiForgeryToken] // chống CSRF
    [Authorize(Roles = "Admin")]
    public IActionResult Create(House house)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.HouseTypes = _context.HouseTypes.ToList();
            return View(house);
        }

        house.Status = "Available"; // mặc định còn trống
        _context.Houses.Add(house);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    // ===== EDIT GET =====
    // 👉 Mở form sửa nhà
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(int id)
    {
        var house = _context.Houses.Find(id);
        if (house == null) return NotFound();

        ViewBag.HouseTypes = _context.HouseTypes.ToList();
        return View(house);
    }

    // ===== EDIT POST =====
    // 👉 Cập nhật nhà
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(House house)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.HouseTypes = _context.HouseTypes.ToList();
            return View(house);
        }

        _context.Houses.Update(house);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    // ===== DELETE =====
    // 👉 Xóa nhà
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var house = _context.Houses.Find(id);
        if (house != null)
        {
            // 👉 Xóa yêu cầu thuê liên quan trước
            var rentalRequests = _context.RentalRequests.Where(r => r.HouseId == id).ToList();
            _context.RentalRequests.RemoveRange(rentalRequests);

            _context.Houses.Remove(house);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    // ===== REQUESTS =====
    // 👉 Danh sách yêu cầu thuê
    public IActionResult Requests()
    {
        return View(_context.RentalRequests.Include(r => r.House).ToList());
    }

    // ===== APPROVE =====
    // 👉 Duyệt yêu cầu thuê
    // ✅ THÊM DÒNG NÀY - Chỉ Admin mới được phép
    [Authorize(Roles = "Admin")]
    public IActionResult Approve(int id)
    {
        var req = _context.RentalRequests.Find(id);
        if (req == null) return NotFound();

        req.Status = "Approved"; // duyệt

        var house = _context.Houses.Find(req.HouseId);
        if (house == null) return NotFound();

        house.Status = "Rented"; // nhà đã thuê
        _context.SaveChanges();

        return RedirectToAction("Requests");
    }

    // ===== REJECT =====
    // 👉 Từ chối yêu cầu thuê
    // ✅ THÊM DÒNG NÀY - Chỉ Admin mới được phép
    [Authorize(Roles = "Admin")]
    public IActionResult Reject(int id)
    {
        var req = _context.RentalRequests.Find(id);
        if (req == null) return NotFound();

        req.Status = "Rejected";
        _context.SaveChanges();

        return RedirectToAction("Requests");
    }
}