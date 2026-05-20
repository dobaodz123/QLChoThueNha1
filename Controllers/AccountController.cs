// 👉 File: AccountController.cs
// 🎯 Mục đích: Xử lý toàn bộ chức năng tài khoản (Login, Logout, Register)

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QlChoThueNha1.Data;
using QlChoThueNha1.Models;
using System.Security.Claims;
namespace QlChoThueNha1.Controllers
{
    // ================= CLASS =================
    // 🎯 Mục đích: Điều khiển logic tài khoản
    // ⚙ Chức năng: Nhận request và xử lý
    public class AccountController : Controller
    {
        // ================= FIELD =================
        // 🎯 Mục đích: Kết nối database
        // ⚙ Chức năng: Truy vấn bảng Users
        private readonly AppDbContext _context;

        // ================= CONSTRUCTOR =================
        // 🎯 Mục đích: Nhận DbContext từ hệ thống
        // ⚙ Chức năng: Gán vào biến _context
        public AccountController(AppDbContext context)
        {
            _context = context;
        }
       
        // ================= FUNCTION: LOGIN (GET) =================
        // 🎯 Mục đích: Hiển thị trang đăng nhập
        // ⚙ Chức năng: Điều hướng người dùng
        [HttpGet]
        public IActionResult Login()
        {
           
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // ================= FUNCTION: LOGIN (POST) =================
        // 🎯 Mục đích: Xử lý đăng nhập
        // ⚙ Chức năng: Kiểm tra + xác thực + tạo session
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ thông tin!";
                return View();
            }

            // 🎯 Tìm user trong database
            // ⚙ Chức năng: So khớp email + password
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

            
            if (user.Role == "Admin")
                return RedirectToAction("Index", "Admin");

            return RedirectToAction("Index", "Home");
        }

        // ================= FUNCTION: LOGOUT =================
        // 🎯 Mục đích: Đăng xuất người dùng
        // ⚙ Chức năng: Xóa session / cookie
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        // ================= FUNCTION: REGISTER (GET) =================
        // 🎯 Mục đích: Hiển thị form đăng ký
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // ================= FUNCTION: REGISTER (POST) =================
        // 🎯 Mục đích: Tạo tài khoản mới
        // ⚙ Chức năng: Validate + lưu DB
        [HttpPost]
        public IActionResult Register(User model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_context.Users.Any(u => u.Email == model.Email))
            {
                return View(model);
            }

            model.CreatedAt = DateTime.Now;
            model.Role = "Customer";

            _context.Users.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }
    }
}