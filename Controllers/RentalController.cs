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

        [Authorize(Roles = "Tenant")]
        public IActionResult Create(int houseId)
        {
            var request = new RentalRequest
            {
                House = _context.Houses.Find(houseId)
            };
            return View(request);
        }

        [HttpPost]
        public IActionResult Create(RentalRequest request)
        {
            request.Status = "Pending";
            request.UserId = GetCurrentUserId();

            _context.RentalRequests.Add(request);
            _context.SaveChanges();

            return RedirectToAction("MyRequests");
        }
        public IActionResult GetMyRequests()
        {
            int userId = GetCurrentUserId();

            var requests = _context.RentalRequests
                .Include(r => r.House)
                .Where(r => r.UserId == userId)
                .ToList();

            return View(requests);
        }

        private int GetCurrentUserId()
        {
            var idClaim = User?.FindFirst("UserId") ?? User?.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim == null || !int.TryParse(idClaim.Value, out var userId))
            {
                throw new InvalidOperationException("Authenticated user id not found in claims.");
            }

            return userId;
        }

        public IActionResult MyRequests
        {
            get
            {
                var userId = GetCurrentUserId();

                var requests = _context.RentalRequests
                    .Include(r => r.House)
                    .Where(r => r.UserId == userId)
                    .ToList();

                return View(requests);
            }
        }
    }
}
