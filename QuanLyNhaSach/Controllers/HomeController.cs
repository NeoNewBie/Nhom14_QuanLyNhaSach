using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaSach.Data;
using QuanLyNhaSach.Models;
using QuanLyNhaSach.ViewModels;
using System.Diagnostics;

namespace QuanLyNhaSach.Controllers;

public class HomeController : Controller
{
    private readonly QuanLyBanSachContext _context;

    public HomeController(QuanLyBanSachContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? keyword, string? q, int? maDanhMuc)
    {
        keyword ??= q;
        var query = _context.SanPhams
            .Include(x => x.MaDanhMucNavigation)
            .Include(x => x.MaNhaXuatBanNavigation)
            .Include(x => x.MaTacGia)
            .Where(x => x.TrangThai)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => x.TenSanPham.Contains(keyword) || (x.MoTa != null && x.MoTa.Contains(keyword)));
        }

        if (maDanhMuc.HasValue)
        {
            query = query.Where(x => x.MaDanhMuc == maDanhMuc.Value);
        }

        var vm = new HomeIndexViewModel
        {
            Keyword = keyword,
            TuKhoa = keyword,
            MaDanhMuc = maDanhMuc,
            DanhMucs = await _context.DanhMucs.Where(x => x.TrangThai).OrderBy(x => x.TenDanhMuc).ToListAsync(),
            SachMoi = await _context.SanPhams.Include(x => x.MaTacGia).Where(x => x.TrangThai).OrderByDescending(x => x.NgayTao).Take(8).ToListAsync(),
            SachBanChay = await _context.SanPhams.Include(x => x.MaTacGia).Where(x => x.TrangThai).OrderByDescending(x => x.ChiTietDonHangs.Count).Take(8).ToListAsync(),
            SanPhams = await query.OrderBy(x => x.TenSanPham).Take(24).ToListAsync(),
            TatCaSanPham = await query.OrderBy(x => x.TenSanPham).ToListAsync()
        };

        return View(vm);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
