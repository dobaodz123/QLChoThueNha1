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

        // ================= LOGIN GET =================
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // ================= LOGIN POST =================
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ thông tin!";
                return View();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.Email == username &&
                    x.Password == password);

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
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            TempData["Success"] = "Đăng nhập thành công!";

            if (user.Role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Home");
        }

        // ================= REGISTER GET =================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // ================= REGISTER POST =================
        [HttpPost]
        public IActionResult Register(User model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) ||
    string.IsNullOrWhiteSpace(model.Password))
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ thông tin!";
                return View(model);
            }

            bool emailExists = _context.Users
                .Any(x => x.Email == model.Email);

            if (emailExists)
            {
                TempData["Error"] = "Email đã tồn tại!";
                return View(model);
            }

            model.Role = "Customer";
            model.CreatedAt = DateTime.Now;

            _context.Users.Add(model);
            _context.SaveChanges();

            TempData["Success"] = "Đăng ký thành công!";

            return RedirectToAction("Login");
        }

        // ================= PROFILE GET =================
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.Email == User.Identity.Name);

            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy tài khoản!";
                return RedirectToAction("Login");
            }

            return View(user);
        }

        // ================= PROFILE POST =================
        [HttpPost]
        public async Task<IActionResult> Profile(User model)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.Email == User.Identity.Name);

            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy tài khoản!";
                return RedirectToAction("Login");
            }

            user.FullName = model.FullName;
            user.Phone = model.Phone;

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                user.Password = model.Password;
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật hồ sơ thành công!";

            return RedirectToAction("Profile");
        }

        // ================= LOGOUT =================
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            TempData["Success"] = "Đăng xuất thành công!";

            return RedirectToAction("Login");
        }
    }
}