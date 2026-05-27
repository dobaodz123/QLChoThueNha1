using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QlChoThueNha1.Data;
using QlChoThueNha1.Models;

namespace QlChoThueNha1.Controllers
{
    public class DanhGiaController : Controller
    {
        private readonly AppDbContext _context;

        public DanhGiaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /DanhGia?NhaId=1
        public async Task<IActionResult> Index(int? nhaId)
        {
            if (!nhaId.HasValue)
                return BadRequest();

            var danhGias = await _context.DanhGias
                .AsNoTracking()
                .Where(x => x.NhaId == nhaId.Value)
                .OrderByDescending(x => x.NgayDanhGia)
                .ToListAsync();

            return View(danhGias);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them([Bind("NhaId,NguoiDanhGia,NoiDung,SoSao")] DanhGia dg)
        {
            if (dg == null)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index), new { nhaId = dg.NhaId });
            }

            dg.NgayDanhGia = DateTime.UtcNow;

            _context.DanhGias.Add(dg);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { nhaId = dg.NhaId });
        }
    }
}