using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using QlChoThueNha1.Data;
using QlChoThueNha1.Models;

public class RentalRequestController : Controller
{
    private readonly AppDbContext _context;

    public RentalRequestController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Create(int houseId)
    {
        ViewBag.HouseId = houseId;
        return View();
    }

    [HttpPost]
    public IActionResult Create(RentalRequest req)
    {
        req.Status = "pending";
        _context.RentalRequests.Add(req);
        _context.SaveChanges();
        return RedirectToAction("Index", "House");
    }

    // Converted property into an action method and fixed the TenantName reference by
    // resolving the current user and filtering by `UserId` which exists on `RentalRequest`.
    public IActionResult MyRequests()
    {
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrEmpty(username))
        {
            // No username in session — return an empty list to the view.
            // Alternatively you could RedirectToAction("Login", "Account") depending on app flow.
            return View(new List<RentalRequest>());
        }

        User? user = _context.Users.FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            // No matching user found — return an empty list.
            return View(new List<RentalRequest>());
        }

        var requests = _context.RentalRequests
            .Where(r => r.UserId == user.UserId)
            .ToList();

        return View(requests);
    }
}
