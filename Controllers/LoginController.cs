using Microsoft.AspNetCore.Mvc;
using QlChoThueNha1.Data;
using System.Linq;
using QlChoThueNha1; // sửa lại theo tên project

namespace QlChoThueNha1.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context; // sửa lại DbContext của bạn

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public IActionResult Index(string email, string password)
        {
            var user = _context.Users
                .FirstOrDefault(x => x.Email == email && x.Password == password);

            if (user != null)
            {
                // Lưu thông tin vào Session
                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetString("Role", user.Role);

                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "Sai tài khoản hoặc mật khẩu!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
            TempData["Success"] = "Đăng nhập thành công!";
            return RedirectToAction("Index", "Home");
            TempData["Error"] = "Sai tài khoản hoặc mật khẩu!";
            return View();
        }
    }
}
