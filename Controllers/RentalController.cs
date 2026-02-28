using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QlChoThueNha1.Data;
using QlChoThueNha1.Models;

namespace QlChoThueNha1.Controllers
{
    [Authorize(Roles = "Tenant")]
    public class RentalController : Controller
    {
        private readonly AppDbContext _context;

        public RentalController(AppDbContext context)
        {
            _context = context;
        }

        // ================= CREATE =================
        public IActionResult Create(int houseId)
        {
            var house = _context.Houses.Find(houseId);

            if (house == null)
            {
                TempData["Error"] = "Không tìm thấy nhà!";
                return RedirectToAction("Index", "House");
            }

            var request = new RentalRequest
            {
                House = house
            };

            return View(request);
        }

        [HttpPost]
        public IActionResult Create(RentalRequest request)
        {
            try
            {
                request.Status = "Pending";
                request.UserId = GetCurrentUserId();

                _context.RentalRequests.Add(request);
                _context.SaveChanges();

                TempData["Success"] = "Gửi yêu cầu thuê thành công!";

                return RedirectToAction("MyRequests");
            }
            catch (Exception)
            {
                TempData["Error"] = "Gửi yêu cầu thuê thất bại!";
                return View(request);
            }
        }

        // ================= MY REQUESTS =================
        public IActionResult MyRequests()
        {
            var userId = GetCurrentUserId();

            var requests = _context.RentalRequests
                .Include(r => r.House)
                .Where(r => r.UserId == userId)
                .ToList();

            return View(requests);
        }

        // ================= GET CURRENT USER =================
        private int GetCurrentUserId()
        {
            var idClaim = User?.FindFirst("UserId")
                          ?? User?.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim == null || !int.TryParse(idClaim.Value, out var userId))
            {
                throw new InvalidOperationException("Authenticated user id not found in claims.");
            }

            return userId;

        }
    }
}