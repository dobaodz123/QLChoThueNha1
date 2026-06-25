using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QlChoThueNha1.Data;
using QlChoThueNha1.Models;
using System.Security.Claims;
using QlChoThueNha1.Services;

namespace QlChoThueNha1.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        public AccountController(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // ================= LOGIN GET =================
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // ================= LOGIN POST =================
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ thông tin!";
                return View();
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == username);

            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy tài khoản!";
                return View();
            }

            if (!PasswordHasher.VerifyPassword(password, user.Password))
            {
                TempData["Error"] = "Sai mật khẩu!";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "Customer"),
                new Claim("FullName", user.FullName ?? user.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            TempData["Success"] = "Đăng nhập thành công!";

            if (user.Role == "Admin")
                return RedirectToAction("Index", "Admin");

            return RedirectToAction("Index", "Home");
        }

        // ================= REGISTER =================
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ thông tin!";
                return View(model);
            }

            if (_context.Users.Any(x => x.Email == model.Email))
            {
                TempData["Error"] = "Email đã tồn tại!";
                return View(model);
            }

            model.Password = PasswordHasher.HashPassword(model.Password);
            model.Role = "Customer";
            model.CreatedAt = DateTime.Now;

            _context.Users.Add(model);
            _context.SaveChanges();

            TempData["Success"] = "Đăng ký thành công!";
            return RedirectToAction("Login");
        }

        // ================= PROFILE =================
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

            if (user == null)
                return RedirectToAction("Login");

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(User model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

            if (user == null)
                return RedirectToAction("Login");

            user.FullName = model.FullName;
            user.Phone = model.Phone;

            if (!string.IsNullOrWhiteSpace(model.Password))
                user.Password = PasswordHasher.HashPassword(model.Password);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật hồ sơ thành công!";
            return RedirectToAction("Profile");
        }

        // ================= LOGOUT =================
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            TempData["Success"] = "Đăng xuất thành công!";
            return RedirectToAction("Login");
        }

        // ======================================================
        // 🔥 FORGOT PASSWORD - OTP (THÊM MỚI)
        // ======================================================

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                TempData["Error"] = "Email không tồn tại!";
                return View();
            }

            var otp = new Random().Next(100000, 999999).ToString();

            HttpContext.Session.SetString("OTP", otp);
            HttpContext.Session.SetString("ResetEmail", email);

            await _emailService.SendOtpAsync(email, otp);

            return RedirectToAction("VerifyOtp");
        }

        // ================= VERIFY OTP =================
        [HttpGet]
        public IActionResult VerifyOtp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyOtp(string otp)
        {
            var savedOtp = HttpContext.Session.GetString("OTP");

            if (otp != savedOtp)
            {
                TempData["Error"] = "OTP không đúng!";
                return View();
            }

            return RedirectToAction("ResetPassword");
        }

        // ================= RESET PASSWORD =================
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string password)
        {
            var email = HttpContext.Session.GetString("ResetEmail");

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
                return RedirectToAction("Login");

            user.Password = PasswordHasher.HashPassword(password);

            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("OTP");
            HttpContext.Session.Remove("ResetEmail");

            TempData["Success"] = "Đổi mật khẩu thành công!";
            return RedirectToAction("Login");
        }
    }
}