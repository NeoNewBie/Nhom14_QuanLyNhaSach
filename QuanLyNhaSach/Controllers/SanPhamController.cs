using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaSach.Data;
using QuanLyNhaSach.Models;
using QuanLyNhaSach.ViewModels;

namespace QuanLyNhaSach.Controllers;

public class SanPhamController : Controller
{
    private readonly QuanLyBanSachContext _context;

    public SanPhamController(QuanLyBanSachContext context)
    {
        _context = context;
    }

    private int? CurrentUserId => HttpContext.Session.GetInt32("UserId") ?? HttpContext.Session.GetInt32("MaNguoiDung");
    private IActionResult RedirectToLogin() => RedirectToAction("Login", "Account", new { returnUrl = Request.Path + Request.QueryString });

    public async Task<IActionResult> Details(int id)
    {
        var sanPham = await _context.SanPhams
            .Include(x => x.MaDanhMucNavigation)
            .Include(x => x.MaTacGia)
            .Include(x => x.MaNhaXuatBanNavigation)
            .FirstOrDefaultAsync(x => x.MaSanPham == id && x.TrangThai);

        if (sanPham == null) return NotFound();

        ViewBag.SanPhamLienQuan = await _context.SanPhams
            .Include(x => x.MaTacGia)
            .Where(x => x.TrangThai && x.MaDanhMuc == sanPham.MaDanhMuc && x.MaSanPham != id)
            .OrderByDescending(x => x.NgayTao)
            .Take(4)
            .ToListAsync();

        ViewBag.DanhMucs = await _context.DanhMucs
            .Where(x => x.TrangThai)
            .OrderBy(x => x.TenDanhMuc)
            .ToListAsync();

        ViewBag.SoLuongDaBan = await _context.ChiTietDonHangs
            .Where(x => x.MaSanPham == id)
            .SumAsync(x => (int?)x.SoLuong) ?? 0;

        var reviews = await _context.DanhGias
            .Include(x => x.MaNguoiDungNavigation)
            .Where(x => x.MaSanPham == id)
            .OrderByDescending(x => x.NgayDanhGia)
            .ToListAsync();
        ViewBag.DanhGias = reviews;
        ViewBag.DiemTrungBinh = reviews.Any() ? reviews.Average(x => x.SoSao) : 0;

        return View(sanPham);
    }

    [HttpGet]
    public async Task<IActionResult> Review(int maSanPham, int maDonHang)
    {
        var userId = CurrentUserId;
        if (userId == null) return RedirectToLogin();

        var canReview = await _context.DonHangs
            .Include(x => x.ChiTietDonHangs)
            .AnyAsync(x => x.MaDonHang == maDonHang
                           && x.MaNguoiDung == userId.Value
                           && x.TrangThaiDonHang == "Đã giao"
                           && x.ChiTietDonHangs.Any(ct => ct.MaSanPham == maSanPham));
        if (!canReview)
        {
            TempData["Error"] = "Bạn chỉ có thể đánh giá sách trong đơn hàng đã giao.";
            return RedirectToAction("History", "DonHang");
        }

        var product = await _context.SanPhams.FindAsync(maSanPham);
        if (product == null) return NotFound();

        var existing = await _context.DanhGias.FirstOrDefaultAsync(x => x.MaNguoiDung == userId.Value && x.MaSanPham == maSanPham && x.MaDonHang == maDonHang);
        return View(new ReviewViewModel
        {
            MaSanPham = maSanPham,
            MaDonHang = maDonHang,
            TenSanPham = product.TenSanPham,
            SoSao = existing?.SoSao ?? 5,
            NoiDung = existing?.NoiDung
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Review(ReviewViewModel model)
    {
        var userId = CurrentUserId;
        if (userId == null) return RedirectToLogin();

        var product = await _context.SanPhams.FindAsync(model.MaSanPham);
        if (product == null) return NotFound();
        model.TenSanPham = product.TenSanPham;

        var canReview = await _context.DonHangs
            .Include(x => x.ChiTietDonHangs)
            .AnyAsync(x => x.MaDonHang == model.MaDonHang
                           && x.MaNguoiDung == userId.Value
                           && x.TrangThaiDonHang == "Đã giao"
                           && x.ChiTietDonHangs.Any(ct => ct.MaSanPham == model.MaSanPham));
        if (!canReview)
        {
            ModelState.AddModelError(string.Empty, "Bạn chỉ có thể đánh giá sách trong đơn hàng đã giao.");
        }

        if (!ModelState.IsValid) return View(model);

        var existing = await _context.DanhGias.FirstOrDefaultAsync(x => x.MaNguoiDung == userId.Value && x.MaSanPham == model.MaSanPham && x.MaDonHang == model.MaDonHang);
        if (existing == null)
        {
            _context.DanhGias.Add(new DanhGia
            {
                MaNguoiDung = userId.Value,
                MaSanPham = model.MaSanPham,
                MaDonHang = model.MaDonHang,
                SoSao = model.SoSao,
                NoiDung = model.NoiDung,
                NgayDanhGia = DateTime.Now
            });
        }
        else
        {
            existing.SoSao = model.SoSao;
            existing.NoiDung = model.NoiDung;
            existing.NgayDanhGia = DateTime.Now;
        }

        await _context.SaveChangesAsync();
        TempData["Success"] = "Đã lưu đánh giá sách.";
        return RedirectToAction(nameof(Details), new { id = model.MaSanPham });
    }
}
