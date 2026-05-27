using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaSach.Models;
using QuanLyNhaSach.ViewModels;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace QuanLyNhaSach.Controllers;

public class HomeController : Controller
{
    private readonly QuanLyBanSachContext _context;

    public HomeController(QuanLyBanSachContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? q, int? maDanhMuc)
    {
        var query = _context.SanPhams
            .Include(x => x.DanhMuc)
            .Include(x => x.TacGia)
            .Where(x => x.TrangThai)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();
            query = query.Where(x => x.TenSanPham.Contains(q) ||
                                     (x.TacGia != null && x.TacGia.TenTacGia.Contains(q)) ||
                                     (x.DanhMuc != null && x.DanhMuc.TenDanhMuc.Contains(q)));
        }

        if (maDanhMuc.HasValue)
        {
            query = query.Where(x => x.MaDanhMuc == maDanhMuc.Value);
        }

        var tatCa = await query.OrderByDescending(x => x.NgayTao).ToListAsync();

        var bestSellerStats = await _context.ChiTietDonHangs
            .GroupBy(x => x.MaSanPham)
            .Select(g => new { MaSanPham = g.Key, SoLuongBan = g.Sum(x => x.SoLuong) })
            .OrderByDescending(x => x.SoLuongBan)
            .Take(4)
            .ToListAsync();

        var bestSellerIds = bestSellerStats.Select(x => x.MaSanPham).ToList();
        var bestSellers = await _context.SanPhams
            .Include(x => x.TacGia)
            .Where(x => x.TrangThai && bestSellerIds.Contains(x.MaSanPham))
            .ToListAsync();

        if (bestSellers.Count == 0)
        {
            bestSellers = await _context.SanPhams
                .Include(x => x.TacGia)
                .Where(x => x.TrangThai)
                .OrderByDescending(x => x.NgayTao)
                .Take(4)
                .ToListAsync();
        }
        else
        {
            bestSellers = bestSellers.OrderBy(x => bestSellerIds.IndexOf(x.MaSanPham)).ToList();
        }

        var vm = new HomeIndexViewModel
        {
            TuKhoa = q,
            MaDanhMuc = maDanhMuc,
            DanhMucs = await _context.DanhMucs.Where(x => x.TrangThai).OrderBy(x => x.TenDanhMuc).ToListAsync(),
            SachMoi = await _context.SanPhams.Include(x => x.TacGia).Where(x => x.TrangThai).OrderByDescending(x => x.NgayTao).Take(8).ToListAsync(),
            SachBanChay = bestSellers,
            TatCaSanPham = tatCa
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubscribeNewsletter(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            TempData["Error"] = "Email đăng ký nhận tin không hợp lệ.";
            return RedirectToAction(nameof(Index));
        }

        email = email.Trim().ToLowerInvariant();
        var exists = await _context.DangKyNhanTins.FirstOrDefaultAsync(x => x.Email == email);
        if (exists == null)
        {
            _context.DangKyNhanTins.Add(new DangKyNhanTin { Email = email, NgayDangKy = DateTime.Now, TrangThai = true });
            await _context.SaveChangesAsync();
            TempData["Success"] = "Đã đăng ký nhận thông tin sách mới từ BookPort.";
        }
        else
        {
            exists.TrangThai = true;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Email này đã có trong danh sách nhận tin. BookPort đã kích hoạt lại đăng ký.";
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy() => RedirectToAction("Page", "Information", new { slug = "privacy" });

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
