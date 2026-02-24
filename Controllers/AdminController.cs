using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QlChoThueNha1.Data;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }
    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
        var requests = _context.RentalRequests
            .Include(r => r.House)
            .Include(r => r.User)
            .ToList();

        return View(requests);
    }

    public IActionResult Approve(int id)
    {
        var req = _context.RentalRequests.Find(id);
        if (req == null)
            return NotFound();

        req.Status = "Approved";

        var house = _context.Houses.Find(req.House.Id);
        if (house != null)
            house.Status = "Rented";

        _context.SaveChanges();

        return RedirectToAction("Requests");
    }

    public IActionResult Dashboard()
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
            return RedirectToAction("Login", "Account");

        return View();
    }

    public IActionResult Houses()
    {
        return View(_context.Houses.Include(h => h.HouseType).ToList());
    }

    public IActionResult Requests()
    {
        return View(_context.RentalRequests.Include(r => r.House).ToList());
    }
}
