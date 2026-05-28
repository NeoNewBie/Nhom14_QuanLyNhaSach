using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaSach.Data;
using QuanLyNhaSach.Models;

namespace QuanLyNhaSach.Controllers;

public class DonHangController : Controller
{
    private readonly QuanLyBanSachContext _context;

    public DonHangController(QuanLyBanSachContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> History()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account", new { returnUrl = Request.Path.ToString() });

        var orders = await _context.DonHangs
            .Include(x => x.ChiTietDonHangs)
            .Where(x => x.MaNguoiDung == userId.Value)
            .OrderByDescending(x => x.NgayDat)
            .ToListAsync();

        return View(orders);
    }

    public async Task<IActionResult> Details(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account", new { returnUrl = Request.Path.ToString() });

        var order = await _context.DonHangs
            .Include(x => x.MaNguoiDungNavigation)
            .Include(x => x.MaXaGiaoNavigation).ThenInclude(x => x!.MaTinhNavigation)
            .Include(x => x.ChiTietDonHangs).ThenInclude(x => x.MaSanPhamNavigation)
            .Include(x => x.ThanhToan)
            .FirstOrDefaultAsync(x => x.MaDonHang == id && x.MaNguoiDung == userId.Value);

        if (order == null) return NotFound();
        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        var order = await _context.DonHangs.FirstOrDefaultAsync(x => x.MaDonHang == id && x.MaNguoiDung == userId.Value);
        if (order != null && order.TrangThaiDonHang == "Chờ xác nhận")
        {
            order.TrangThaiDonHang = "Đã hủy";
            await _context.SaveChangesAsync();
            TempData["Success"] = "Đã hủy đơn hàng.";
        }
        return RedirectToAction(nameof(History));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reorder(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        var order = await _context.DonHangs
            .Include(x => x.ChiTietDonHangs)
            .FirstOrDefaultAsync(x => x.MaDonHang == id && x.MaNguoiDung == userId.Value);
        if (order == null) return NotFound();

        var cart = await _context.GioHangs.FirstOrDefaultAsync(x => x.MaNguoiDung == userId.Value && x.TrangThai);
        if (cart == null)
        {
            cart = new GioHang { MaNguoiDung = userId.Value, NgayTao = DateTime.Now, TrangThai = true };
            _context.GioHangs.Add(cart);
            await _context.SaveChangesAsync();
        }

        foreach (var detail in order.ChiTietDonHangs)
        {
            var item = await _context.ChiTietGioHangs.FirstOrDefaultAsync(x => x.MaGioHang == cart.MaGioHang && x.MaSanPham == detail.MaSanPham);
            if (item == null)
            {
                _context.ChiTietGioHangs.Add(new ChiTietGioHang
                {
                    MaGioHang = cart.MaGioHang,
                    MaSanPham = detail.MaSanPham,
                    SoLuong = detail.SoLuong,
                    DonGia = detail.DonGia
                });
            }
            else
            {
                item.SoLuong += detail.SoLuong;
            }
        }
        await _context.SaveChangesAsync();
        TempData["Success"] = "Đã thêm lại sản phẩm vào giỏ hàng.";
        return RedirectToAction("Index", "GioHang");
    }
}
