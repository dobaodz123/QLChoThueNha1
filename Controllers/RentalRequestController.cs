using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QlChoThueNha1.Data;
using QlChoThueNha1.Models;
using System.Security.Claims;

namespace QlChoThueNha1.Controllers
{
    public class RentalRequestController : Controller
    {
        private readonly AppDbContext _context;

        public RentalRequestController(AppDbContext context)
        {
            _context = context;
        }

        // ===================== CREATE GET =====================
        [Authorize]
        public IActionResult Create(int houseId)
        {
            var house = _context.Houses.FirstOrDefault(h => h.Id == houseId);
            if (house == null)
                return NotFound();

            var model = new RentalRequest
            {
                HouseId = houseId
            };

            ViewBag.House = house;
            return View(model);
        }

        // ===================== CREATE POST =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(RentalRequest rentalRequest)
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
                return Unauthorized();

            if (rentalRequest.EndDate <= rentalRequest.StartDate)
            {
                TempData["Error"] = "Ngày kết thúc phải lớn hơn ngày bắt đầu!";
                ViewBag.House = _context.Houses.FirstOrDefault(h => h.Id == rentalRequest.HouseId);
                return View(rentalRequest);
            }

            rentalRequest.UserId = user.UserId;
            rentalRequest.RequestDate = DateTime.Now;
            rentalRequest.Status = "Pending";

            _context.RentalRequests.Add(rentalRequest);
            _context.SaveChanges();

            TempData["Success"] = "Gửi yêu cầu thuê thành công! Vui lòng chờ xét duyệt.";
            return RedirectToAction("MyRequests");
        }

        // ===================== MY REQUESTS =====================
        [Authorize]
        public IActionResult MyRequests()
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
                return Unauthorized();

            var requests = _context.RentalRequests
                .Include(r => r.House)
                .Where(r => r.UserId == user.UserId)
                .ToList();

            return View(requests);
        }

        // ===================== INDEX (ADMIN) =====================
        public IActionResult Index()
        {
            var requests = _context.RentalRequests
                .Include(r => r.House)
                .Include(r => r.User)
                .ToList();

            return View(requests);
        }

        // ===================== DETAILS =====================
        public IActionResult Details(int id)
        {
            var request = _context.RentalRequests
                .Include(r => r.House)
                .Include(r => r.User)
                .FirstOrDefault(r => r.Id == id);

            if (request == null)
                return NotFound();

            return View(request);
        }

        // ===================== APPROVE =====================
        public IActionResult Approve(int id)
        {
            var request = _context.RentalRequests.Find(id);
            if (request != null)
            {
                request.Status = "Approved";
                _context.SaveChanges();
                TempData["Success"] = "Đã duyệt yêu cầu thuê thành công!";
            }
            return RedirectToAction(nameof(Index));
        }

        // ===================== REJECT =====================
        public IActionResult Reject(int id)
        {
            var request = _context.RentalRequests.Find(id);
            if (request != null)
            {
                request.Status = "Rejected";
                _context.SaveChanges();
                TempData["Error"] = "Đã từ chối yêu cầu thuê!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}