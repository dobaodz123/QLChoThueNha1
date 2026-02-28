using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QlChoThueNha1.Data;
using QlChoThueNha1.Models;
using System.Security.Claims;

namespace QlChoThueNha1.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // ================= LOGIN =================
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ thông tin!";
                return View();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == username && u.Password == password);

            if (user == null)
            {
                TempData["Error"] = "Sai tài khoản hoặc mật khẩu!";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "Customer"),
                new Claim("FullName", user.FullName ?? user.Email)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            TempData["Success"] = $"Đăng nhập thành công! Chào mừng {user.FullName ?? user.Email}";

            if (user.Role == "Admin")
                return RedirectToAction("Index", "Admin");
            return RedirectToAction("Index", "Home");
        }

        // ================= LOGOUT =================
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Success"] = "Đăng xuất thành công!";
            return RedirectToAction("Login", "Account");
        }

        // ================= REGISTER =================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Vui lòng kiểm tra lại thông tin đăng ký!";
                return View(model);
            }

            if (_context.Users.Any(u => u.Email == model.Email))
            {
                TempData["Error"] = "Email đã tồn tại, vui lòng dùng email khác!";
                return View(model);
            }

            model.CreatedAt = DateTime.Now;
            model.Role = "Customer";
            _context.Users.Add(model);
            _context.SaveChanges();

            TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }
    }
}