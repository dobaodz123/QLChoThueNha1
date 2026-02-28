using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QlChoThueNha1.Data;
using QlChoThueNha1.Models;

public class HouseController : Controller
{
    private readonly AppDbContext _context;
    public HouseController(AppDbContext context)
    {
        _context = context;
    }

    // TRANG CHỦ KHÁCH THUÊ
    public IActionResult Index(string keyword, int? houseTypeId)
    {
        var houses = _context.Houses
            .Include(h => h.HouseType)
            .AsQueryable(); // bỏ filter Available để hiện tất cả

        if (!string.IsNullOrEmpty(keyword))
            houses = houses.Where(h => h.Address.Contains(keyword) || h.Name.Contains(keyword));

        if (houseTypeId.HasValue)
            houses = houses.Where(h => h.HouseTypeId == houseTypeId);

        ViewBag.HouseTypes = _context.HouseTypes.ToList();
        return View(houses.ToList());
    }

    // CHI TIẾT NHÀ
    public IActionResult Details(int id)
    {
        var house = _context.Houses
            .Include(h => h.HouseType)
            .FirstOrDefault(h => h.Id == id);

        if (house == null)
            return NotFound();

        return View(house);
    }

    // ===================== CREATE GET =====================
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        ViewBag.HouseTypes = _context.HouseTypes.ToList();
        return View();
    }

    // ===================== CREATE POST =====================
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public IActionResult Create(House house)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.HouseTypes = _context.HouseTypes.ToList();
            TempData["Error"] = "Vui lòng kiểm tra lại thông tin!";
            return View(house);
        }

        house.Status = "Available";
        _context.Houses.Add(house);
        _context.SaveChanges();

        TempData["Success"] = "Thêm nhà thành công!";
        return RedirectToAction("Index");
    }

    // ===================== EDIT GET =====================
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(int id)
    {
        var house = _context.Houses.Find(id);
        if (house == null) return NotFound();

        ViewBag.HouseTypes = _context.HouseTypes.ToList();
        return View(house);
    }

    // ===================== EDIT POST =====================
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(House house)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.HouseTypes = _context.HouseTypes.ToList();
            TempData["Error"] = "Vui lòng kiểm tra lại thông tin!";
            return View(house);
        }

        _context.Houses.Update(house);
        _context.SaveChanges();

        TempData["Success"] = "Cập nhật nhà thành công!";
        return RedirectToAction("Index");
    }

    // ===================== DELETE =====================
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var house = _context.Houses.Find(id);
        if (house != null)
        {
            // Xóa các yêu cầu thuê liên quan trước
            var rentalRequests = _context.RentalRequests.Where(r => r.HouseId == id).ToList();
            _context.RentalRequests.RemoveRange(rentalRequests);

            _context.Houses.Remove(house);
            _context.SaveChanges();
            TempData["Success"] = "Xóa nhà thành công!";
        }
        return RedirectToAction("Index");
    }

    public IActionResult Requests()
    {
        return View(_context.RentalRequests.Include(r => r.House).ToList());
    }

    public IActionResult Approve(int id)
    {
        var req = _context.RentalRequests.Find(id);
        if (req == null) return NotFound();

        req.Status = "Approved";
        var house = _context.Houses.Find(req.HouseId);
        if (house == null) return NotFound();

        house.Status = "Rented";
        _context.SaveChanges();

        TempData["Success"] = "Đã duyệt yêu cầu thuê thành công!";
        return RedirectToAction("Requests");
    }

    // ===================== REJECT =====================
    public IActionResult Reject(int id)
    {
        var req = _context.RentalRequests.Find(id);
        if (req == null) return NotFound();

        req.Status = "Rejected";
        _context.SaveChanges();

        TempData["Error"] = "Đã từ chối yêu cầu thuê!";
        return RedirectToAction("Requests");
    }
}