using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaSach.Data;
using QuanLyNhaSach.Models;
using QuanLyNhaSach.ViewModels;

namespace QuanLyNhaSach.Controllers;

public class GioHangController : Controller
{
    private readonly QuanLyBanSachContext _context;

    public GioHangController(QuanLyBanSachContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userId = RequireLogin();
        if (userId == null) return RedirectToLogin();

        var cart = await GetOrCreateCart(userId.Value);
        var items = await _context.ChiTietGioHangs
            .Include(x => x.MaSanPhamNavigation).ThenInclude(x => x.MaTacGia)
            .Where(x => x.MaGioHang == cart.MaGioHang)
            .ToListAsync();

        return View(new CartViewModel { GioHang = cart, Items = items });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int maSanPham, int soLuong = 1)
    {
        var userId = RequireLogin();
        if (userId == null) return RedirectToLogin();

        var product = await _context.SanPhams.FirstOrDefaultAsync(x => x.MaSanPham == maSanPham && x.TrangThai);
        if (product == null) return NotFound();

        var cart = await GetOrCreateCart(userId.Value);
        var item = await _context.ChiTietGioHangs.FirstOrDefaultAsync(x => x.MaGioHang == cart.MaGioHang && x.MaSanPham == maSanPham);
        if (item == null)
        {
            _context.ChiTietGioHangs.Add(new ChiTietGioHang
            {
                MaGioHang = cart.MaGioHang,
                MaSanPham = maSanPham,
                SoLuong = Math.Max(1, soLuong),
                DonGia = product.GiaBan
            });
        }
        else
        {
            item.SoLuong += Math.Max(1, soLuong);
        }

        await _context.SaveChangesAsync();
        TempData["Success"] = "Đã thêm sách vào giỏ hàng.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int maSanPham, int soLuong)
    {
        var userId = RequireLogin();
        if (userId == null) return RedirectToLogin();

        var cart = await GetOrCreateCart(userId.Value);
        var item = await _context.ChiTietGioHangs.FirstOrDefaultAsync(x => x.MaGioHang == cart.MaGioHang && x.MaSanPham == maSanPham);
        if (item != null)
        {
            if (soLuong <= 0) _context.ChiTietGioHangs.Remove(item);
            else item.SoLuong = soLuong;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int maSanPham)
    {
        var userId = RequireLogin();
        if (userId == null) return RedirectToLogin();

        var cart = await GetOrCreateCart(userId.Value);
        var item = await _context.ChiTietGioHangs.FirstOrDefaultAsync(x => x.MaGioHang == cart.MaGioHang && x.MaSanPham == maSanPham);
        if (item != null)
        {
            _context.ChiTietGioHangs.Remove(item);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Checkout()
    {
        var userId = RequireLogin();
        if (userId == null) return RedirectToLogin();

        var user = await _context.NguoiDungs.FindAsync(userId.Value);
        return View(new CheckoutViewModel
        {
            HoTenNguoiNhan = user?.HoTen ?? string.Empty,
            SoDienThoaiNhan = user?.SoDienThoai ?? string.Empty,
            MaXaGiao = user?.MaXa,
            SoNhaGiao = user?.SoNha,
            DuongGiao = user?.Duong,
            XaPhuongs = await _context.XaPhuongs.Include(x => x.MaTinhNavigation).OrderBy(x => x.MaTinhNavigation.TenTinh).ThenBy(x => x.TenXa).ToListAsync()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CheckoutViewModel model)
    {
        var userId = RequireLogin();
        if (userId == null) return RedirectToLogin();

        var cart = await GetOrCreateCart(userId.Value);
        var items = await _context.ChiTietGioHangs
            .Include(x => x.MaSanPhamNavigation)
            .Where(x => x.MaGioHang == cart.MaGioHang)
            .ToListAsync();

        if (!items.Any())
        {
            TempData["Error"] = "Giỏ hàng đang trống.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            model.XaPhuongs = await _context.XaPhuongs.Include(x => x.MaTinhNavigation).ToListAsync();
            return View(model);
        }

        var tongTien = items.Sum(x => x.SoLuong * x.DonGia);
        var phiShip = 25000m;
        var order = new DonHang
        {
            MaNguoiDung = userId.Value,
            NgayDat = DateTime.Now,
            HoTenNguoiNhan = model.HoTenNguoiNhan,
            SoDienThoaiNhan = model.SoDienThoaiNhan,
            MaXaGiao = model.MaXaGiao,
            SoNhaGiao = model.SoNhaGiao,
            DuongGiao = model.DuongGiao,
            TongTien = tongTien + phiShip,
            PhiShip = phiShip,
            TrangThaiDonHang = "Chờ xác nhận",
            GhiChu = model.GhiChu
        };
        _context.DonHangs.Add(order);
        await _context.SaveChangesAsync();

        foreach (var item in items)
        {
            _context.ChiTietDonHangs.Add(new ChiTietDonHang
            {
                MaDonHang = order.MaDonHang,
                MaSanPham = item.MaSanPham,
                SoLuong = item.SoLuong,
                DonGia = item.DonGia,
                ThanhTien = item.SoLuong * item.DonGia
            });
        }

        _context.ThanhToans.Add(new ThanhToan
        {
            MaDonHang = order.MaDonHang,
            PhuongThucThanhToan = "Thanh toán khi nhận hàng",
            SoTienThanhToan = order.TongTien,
            TrangThaiThanhToan = "Chưa thanh toán"
        });

        _context.ChiTietGioHangs.RemoveRange(items);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Đặt hàng thành công.";
        return RedirectToAction("Details", "DonHang", new { id = order.MaDonHang });
    }

    private int? RequireLogin() => HttpContext.Session.GetInt32("UserId");
    private IActionResult RedirectToLogin() => RedirectToAction("Login", "Account", new { returnUrl = Request.Path.ToString() });

    private async Task<GioHang> GetOrCreateCart(int userId)
    {
        var cart = await _context.GioHangs.FirstOrDefaultAsync(x => x.MaNguoiDung == userId && x.TrangThai);
        if (cart != null) return cart;

        cart = new GioHang { MaNguoiDung = userId, NgayTao = DateTime.Now, TrangThai = true };
        _context.GioHangs.Add(cart);
        await _context.SaveChangesAsync();
        return cart;
    }
}
