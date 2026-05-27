using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaSach.Models;

namespace QuanLyNhaSach.Controllers;

public class DonHangController : Controller
{
    private readonly QuanLyBanSachContext _context;

    public DonHangController(QuanLyBanSachContext context)
    {
        _context = context;
    }

    private int? CurrentUserId => HttpContext.Session.GetInt32("MaNguoiDung");
    private bool IsAdmin => HttpContext.Session.GetString("VaiTro") == "Admin";

    private IActionResult RedirectToLogin() => RedirectToAction("Login", "Account", new { returnUrl = Request.Path + Request.QueryString });

    public async Task<IActionResult> History(string? q)
    {
        var userId = CurrentUserId;
        if (userId == null) return RedirectToLogin();

        var query = _context.DonHangs
            .Include(x => x.ChiTietDonHangs)
                .ThenInclude(x => x.SanPham)
                    .ThenInclude(x => x!.NhaXuatBan)
            .Include(x => x.ChiTietDonHangs)
                .ThenInclude(x => x.SanPham)
                    .ThenInclude(x => x!.TacGia)
            .Where(x => x.MaNguoiDung == userId.Value)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();
            if (int.TryParse(q.Replace("BP-", string.Empty).Replace("#", string.Empty), out var maDonHang))
            {
                query = query.Where(x => x.MaDonHang == maDonHang
                    || x.TrangThaiDonHang.Contains(q)
                    || x.ChiTietDonHangs.Any(ct => ct.SanPham != null && ct.SanPham.TenSanPham.Contains(q)));
            }
            else
            {
                query = query.Where(x => x.TrangThaiDonHang.Contains(q)
                    || x.ChiTietDonHangs.Any(ct => ct.SanPham != null && ct.SanPham.TenSanPham.Contains(q)));
            }
        }

        ViewBag.TuKhoa = q;
        var orders = await query.OrderByDescending(x => x.NgayDat).ToListAsync();
        return View(orders);
    }

    public async Task<IActionResult> Details(int id)
    {
        var userId = CurrentUserId;
        if (userId == null) return RedirectToLogin();

        var order = await _context.DonHangs
            .Include(x => x.NguoiDung)
            .Include(x => x.ThanhToan)
            .Include(x => x.ChiTietDonHangs)
                .ThenInclude(x => x.SanPham)
            .FirstOrDefaultAsync(x => x.MaDonHang == id);

        if (order == null) return NotFound();
        if (!IsAdmin && order.MaNguoiDung != userId.Value) return RedirectToAction("Index", "Home");

        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reorder(int id)
    {
        var userId = CurrentUserId;
        if (userId == null) return RedirectToLogin();

        var order = await _context.DonHangs
            .Include(x => x.ChiTietDonHangs)
                .ThenInclude(x => x.SanPham)
            .FirstOrDefaultAsync(x => x.MaDonHang == id && x.MaNguoiDung == userId.Value);

        if (order == null) return NotFound();

        var cart = await _context.GioHangs
            .Include(x => x.ChiTietGioHangs)
            .FirstOrDefaultAsync(x => x.MaNguoiDung == userId.Value && x.TrangThai);

        if (cart == null)
        {
            cart = new GioHang { MaNguoiDung = userId.Value, TrangThai = true, NgayTao = DateTime.Now };
            _context.GioHangs.Add(cart);
            await _context.SaveChangesAsync();
        }

        foreach (var detail in order.ChiTietDonHangs)
        {
            if (detail.SanPham == null || !detail.SanPham.TrangThai || detail.SanPham.SoLuongTon <= 0) continue;
            var quantity = Math.Min(detail.SoLuong, detail.SanPham.SoLuongTon);
            var existing = await _context.ChiTietGioHangs.FirstOrDefaultAsync(x => x.MaGioHang == cart.MaGioHang && x.MaSanPham == detail.MaSanPham);
            if (existing == null)
            {
                _context.ChiTietGioHangs.Add(new ChiTietGioHang
                {
                    MaGioHang = cart.MaGioHang,
                    MaSanPham = detail.MaSanPham,
                    SoLuong = quantity,
                    DonGia = detail.SanPham.GiaBan
                });
            }
            else
            {
                existing.SoLuong = Math.Min(existing.SoLuong + quantity, detail.SanPham.SoLuongTon);
                existing.DonGia = detail.SanPham.GiaBan;
            }
        }

        await _context.SaveChangesAsync();
        TempData["Success"] = "Đã thêm lại các sách còn hàng vào giỏ.";
        return RedirectToAction("Index", "GioHang");
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        var userId = CurrentUserId;
        if (userId == null) return RedirectToLogin();

        var order = await _context.DonHangs
            .Include(x => x.ChiTietDonHangs)
                .ThenInclude(x => x.SanPham)
            .Include(x => x.ThanhToan)
            .FirstOrDefaultAsync(x => x.MaDonHang == id && x.MaNguoiDung == userId.Value);

        if (order == null) return NotFound();
        if (order.TrangThaiDonHang == "Đã giao" || order.TrangThaiDonHang == "Đã hủy")
        {
            TempData["Error"] = "Đơn hàng đã giao hoặc đã hủy nên không thể hủy lại.";
            return RedirectToAction(nameof(History));
        }

        foreach (var detail in order.ChiTietDonHangs)
        {
            if (detail.SanPham != null)
            {
                detail.SanPham.SoLuongTon += detail.SoLuong;
            }
        }

        order.TrangThaiDonHang = "Đã hủy";
        if (order.ThanhToan != null)
        {
            order.ThanhToan.TrangThaiThanhToan = "Đã hủy";
        }

        await _context.SaveChangesAsync();
        TempData["Success"] = "Đã hủy đơn hàng và hoàn lại tồn kho.";
        return RedirectToAction(nameof(History));
    }

}
