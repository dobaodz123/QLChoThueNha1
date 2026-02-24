using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QlChoThueNha1.Data;
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
            .Where(h => h.Status == "available")
            .AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
            houses = houses.Where(h => h.Address.Contains(keyword));

        if (houseTypeId.HasValue)
            houses = houses.Where(h => h.HouseTypeId == houseTypeId);

        ViewBag.HouseTypes = _context.HouseTypes.ToList();
        return View(houses.ToList());
    }
    public IActionResult Details(int id)
    {
        var house = _context.Houses
            .Include(h => h.HouseType)
            .FirstOrDefault(h => h.Id == id);

        if (house == null)
            return NotFound();

        return View(house);
    }
    public IActionResult Requests()
    {
        return View(_context.RentalRequests.Include(r => r.House).ToList());
    }

    public IActionResult Approve(int id)
    {
        var req = _context.RentalRequests.Find(id);
        if (req == null)
            return NotFound();

        req.Status = "approved";

        var house = _context.Houses.Find(req.HouseId);
        if (house == null)
            return NotFound();

        house.Status = "rented";

        _context.SaveChanges();
        return RedirectToAction("Requests");
    }


}
