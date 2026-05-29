using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaSach.Data;
using QuanLyNhaSach.Models;
using QuanLyNhaSach.ViewModels;

namespace QuanLyNhaSach.Controllers;

public class GioHangController : Controller
{
    private readonly QuanLyBanSachContext _context;
    private const string VoucherSessionKey = "VoucherCode";
    private const decimal DefaultShippingFee = 25000m;

    public GioHangController(QuanLyBanSachContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userId = RequireLogin();
        if (userId == null) return RedirectToLogin();

        var cart = await GetOrCreateCart(userId.Value);
        var items = await LoadCartItems(cart.MaGioHang);
        var user = await _context.NguoiDungs
            .Include(x => x.MaXaNavigation)
                .ThenInclude(x => x!.MaTinhNavigation)
            .FirstOrDefaultAsync(x => x.MaNguoiDung == userId.Value);

        var voucher = await GetCurrentVoucher(items.Sum(x => x.SoLuong * x.DonGia));
        var discount = CalculateDiscount(voucher, items.Sum(x => x.SoLuong * x.DonGia));

        var vm = new CartViewModel
        {
            GioHang = cart,
            NguoiDung = user,
            Items = items,
            MaGiamGiaKhaDung = await GetAvailableVouchers(),
            MaGiamGiaDangApDung = voucher?.Code,
            MoTaGiamGia = voucher == null ? null : FormatVoucher(voucher),
            GiamGia = discount,
            PhiShip = items.Any() ? DefaultShippingFee : 0
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int maSanPham, int soLuong = 1)
    {
        var userId = RequireLogin();
        if (userId == null) return RedirectToLogin();

        var product = await _context.SanPhams.FirstOrDefaultAsync(x => x.MaSanPham == maSanPham && x.TrangThai);
        if (product == null) return NotFound();

        soLuong = Math.Max(1, soLuong);
        if (product.SoLuongTon <= 0)
        {
            TempData["Error"] = "Sách này hiện đã hết hàng.";
            return RedirectToAction("Details", "SanPham", new { id = maSanPham });
        }

        var cart = await GetOrCreateCart(userId.Value);
        var item = await _context.ChiTietGioHangs.FirstOrDefaultAsync(x => x.MaGioHang == cart.MaGioHang && x.MaSanPham == maSanPham);
        if (item == null)
        {
            _context.ChiTietGioHangs.Add(new ChiTietGioHang
            {
                MaGioHang = cart.MaGioHang,
                MaSanPham = maSanPham,
                SoLuong = Math.Min(soLuong, product.SoLuongTon),
                DonGia = product.GiaBan
            });
        }
        else
        {
            item.SoLuong = Math.Min(item.SoLuong + soLuong, product.SoLuongTon);
            item.DonGia = product.GiaBan;
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
        var item = await _context.ChiTietGioHangs
            .Include(x => x.MaSanPhamNavigation)
            .FirstOrDefaultAsync(x => x.MaGioHang == cart.MaGioHang && x.MaSanPham == maSanPham);
        if (item != null)
        {
            if (soLuong <= 0)
            {
                _context.ChiTietGioHangs.Remove(item);
            }
            else
            {
                item.SoLuong = Math.Min(soLuong, Math.Max(1, item.MaSanPhamNavigation.SoLuongTon));
                item.DonGia = item.MaSanPhamNavigation.GiaBan;
            }
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApplyVoucher(string? code)
    {
        var userId = RequireLogin();
        if (userId == null) return RedirectToLogin();

        if (string.IsNullOrWhiteSpace(code))
        {
            TempData["Error"] = "Vui lòng nhập mã khuyến mãi.";
            return RedirectToAction(nameof(Index));
        }

        var normalized = code.Trim();
        var voucher = await FindVoucher(normalized);
        if (voucher == null)
        {
            TempData["Error"] = "Mã khuyến mãi không hợp lệ hoặc đã hết hạn.";
            return RedirectToAction(nameof(Index));
        }

        var cart = await GetOrCreateCart(userId.Value);
        var items = await LoadCartItems(cart.MaGioHang);
        var tamTinh = items.Sum(x => x.SoLuong * x.DonGia);
        if (voucher.DieuKienApDung.HasValue && tamTinh < voucher.DieuKienApDung.Value)
        {
            TempData["Error"] = $"Đơn hàng cần tối thiểu {voucher.DieuKienApDung.Value:N0} đ để dùng mã này.";
            return RedirectToAction(nameof(Index));
        }

        HttpContext.Session.SetString(VoucherSessionKey, voucher.Code);
        TempData["Success"] = "Đã áp dụng mã khuyến mãi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveVoucher()
    {
        HttpContext.Session.Remove(VoucherSessionKey);
        TempData["Success"] = "Đã bỏ mã khuyến mãi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Checkout()
    {
        var userId = RequireLogin();
        if (userId == null) return RedirectToLogin();

        var cart = await GetOrCreateCart(userId.Value);
        var items = await LoadCartItems(cart.MaGioHang);
        if (!items.Any())
        {
            TempData["Error"] = "Giỏ hàng đang trống.";
            return RedirectToAction(nameof(Index));
        }

        var user = await _context.NguoiDungs.FindAsync(userId.Value);
        var tamTinh = items.Sum(x => x.SoLuong * x.DonGia);
        var voucher = await GetCurrentVoucher(tamTinh);

        return View(new CheckoutViewModel
        {
            HoTenNguoiNhan = user?.HoTen ?? string.Empty,
            SoDienThoaiNhan = user?.SoDienThoai ?? string.Empty,
            MaXaGiao = user?.MaXa,
            SoNhaGiao = user?.SoNha,
            DuongGiao = user?.Duong,
            Items = items,
            MaGiamGiaDangApDung = voucher?.Code,
            GiamGia = CalculateDiscount(voucher, tamTinh),
            PhiShip = DefaultShippingFee,
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
        var items = await LoadCartItems(cart.MaGioHang);

        if (!items.Any())
        {
            TempData["Error"] = "Giỏ hàng đang trống.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            var tamTinhInvalid = items.Sum(x => x.SoLuong * x.DonGia);
            var voucherInvalid = await GetCurrentVoucher(tamTinhInvalid);
            model.Items = items;
            model.MaGiamGiaDangApDung = voucherInvalid?.Code;
            model.GiamGia = CalculateDiscount(voucherInvalid, tamTinhInvalid);
            model.PhiShip = DefaultShippingFee;
            model.XaPhuongs = await _context.XaPhuongs.Include(x => x.MaTinhNavigation).ToListAsync();
            return View(model);
        }

        foreach (var item in items)
        {
            if (item.MaSanPhamNavigation.SoLuongTon < item.SoLuong)
            {
                TempData["Error"] = $"Sách {item.MaSanPhamNavigation.TenSanPham} chỉ còn {item.MaSanPhamNavigation.SoLuongTon} cuốn.";
                return RedirectToAction(nameof(Index));
            }
        }

        var tamTinh = items.Sum(x => x.SoLuong * x.DonGia);
        var voucher = await GetCurrentVoucher(tamTinh);
        var giamGia = CalculateDiscount(voucher, tamTinh);
        var phiShip = DefaultShippingFee;
        var tongTien = Math.Max(0, tamTinh - giamGia + phiShip);

        var order = new DonHang
        {
            MaNguoiDung = userId.Value,
            NgayDat = DateTime.Now,
            HoTenNguoiNhan = model.HoTenNguoiNhan,
            SoDienThoaiNhan = model.SoDienThoaiNhan,
            MaXaGiao = model.MaXaGiao,
            SoNhaGiao = model.SoNhaGiao,
            DuongGiao = model.DuongGiao,
            TongTien = tongTien,
            PhiShip = phiShip,
            TrangThaiDonHang = "Chờ xác nhận",
            GhiChu = model.GhiChu,
            MaKhuyenMai = voucher?.MaKhuyenMai
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
            item.MaSanPhamNavigation.SoLuongTon -= item.SoLuong;
        }

        _context.ThanhToans.Add(new ThanhToan
        {
            MaDonHang = order.MaDonHang,
            PhuongThucThanhToan = model.PhuongThucThanhToan == "Chuyển khoản" ? "Chuyển khoản" : "Thanh toán khi nhận hàng",
            SoTienThanhToan = order.TongTien,
            TrangThaiThanhToan = "Chưa thanh toán"
        });

        _context.ChiTietGioHangs.RemoveRange(items);
        HttpContext.Session.Remove(VoucherSessionKey);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Đặt hàng thành công.";
        return RedirectToAction("Details", "DonHang", new { id = order.MaDonHang });
    }

    private int? RequireLogin() => HttpContext.Session.GetInt32("UserId") ?? HttpContext.Session.GetInt32("MaNguoiDung");
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

    private async Task<List<ChiTietGioHang>> LoadCartItems(int maGioHang)
    {
        return await _context.ChiTietGioHangs
            .Include(x => x.MaSanPhamNavigation)
                .ThenInclude(x => x.MaTacGia)
            .Include(x => x.MaSanPhamNavigation)
                .ThenInclude(x => x.MaDanhMucNavigation)
            .Include(x => x.MaSanPhamNavigation)
                .ThenInclude(x => x.MaNhaXuatBanNavigation)
            .Where(x => x.MaGioHang == maGioHang)
            .ToListAsync();
    }

    private async Task<List<KhuyenMai>> GetAvailableVouchers()
    {
        var now = DateTime.Now;
        return await _context.KhuyenMais
            .Where(x => x.TrangThai && x.NgayBatDau <= now && x.NgayKetThuc >= now)
            .OrderByDescending(x => x.GiaTriGiam)
            .ToListAsync();
    }

    private async Task<KhuyenMai?> GetCurrentVoucher(decimal tamTinh)
    {
        var code = HttpContext.Session.GetString(VoucherSessionKey);
        if (string.IsNullOrWhiteSpace(code)) return null;
        var voucher = await FindVoucher(code);
        if (voucher == null) return null;
        if (voucher.DieuKienApDung.HasValue && tamTinh < voucher.DieuKienApDung.Value) return null;
        return voucher;
    }

    private async Task<KhuyenMai?> FindVoucher(string code)
    {
        var now = DateTime.Now;
        code = code.Trim();
        var all = await _context.KhuyenMais
            .Where(x => x.TrangThai && x.NgayBatDau <= now && x.NgayKetThuc >= now)
            .ToListAsync();

        return all.FirstOrDefault(x =>
            x.Code.Equals(code, StringComparison.OrdinalIgnoreCase) ||
            x.TenKhuyenMai.Contains(code, StringComparison.OrdinalIgnoreCase) ||
            (code.Equals("BOOK10", StringComparison.OrdinalIgnoreCase) && x.LoaiGiamGia.Contains("Phần", StringComparison.OrdinalIgnoreCase)) ||
            (code.Equals("FREESHIP", StringComparison.OrdinalIgnoreCase) && x.GiaTriGiam >= 25000));
    }

    private static decimal CalculateDiscount(KhuyenMai? voucher, decimal tamTinh)
    {
        if (voucher == null) return 0;
        if (voucher.LoaiGiamGia.Contains("Phần", StringComparison.OrdinalIgnoreCase) || voucher.LoaiGiamGia.Contains("%"))
        {
            return Math.Round(tamTinh * voucher.GiaTriGiam / 100m, 0);
        }
        return Math.Min(voucher.GiaTriGiam, tamTinh);
    }

    private static string FormatVoucher(KhuyenMai voucher)
    {
        var discount = voucher.LoaiGiamGia.Contains("Phần", StringComparison.OrdinalIgnoreCase)
            ? $"Giảm {voucher.GiaTriGiam:0}%"
            : $"Giảm {voucher.GiaTriGiam:N0} đ";
        return voucher.DieuKienApDung.HasValue
            ? $"{discount} cho đơn từ {voucher.DieuKienApDung.Value:N0} đ"
            : discount;
    }
}
