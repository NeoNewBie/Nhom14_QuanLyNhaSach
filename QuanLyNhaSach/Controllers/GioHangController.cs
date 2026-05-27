using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    private int? CurrentUserId => HttpContext.Session.GetInt32("MaNguoiDung");
    private const string VoucherSessionKey = "MaGiamGiaDangApDung";

    private IActionResult RedirectToLogin()
    {
        return RedirectToAction("Login", "Account", new { returnUrl = Request.Path + Request.QueryString });
    }

    private GioHang GetOrCreateCart(int userId)
    {
        var cart = _context.GioHangs
            .Include(x => x.ChiTietGioHangs)
            .FirstOrDefault(x => x.MaNguoiDung == userId && x.TrangThai);

        if (cart != null) return cart;

        cart = new GioHang { MaNguoiDung = userId, TrangThai = true, NgayTao = DateTime.Now };
        _context.GioHangs.Add(cart);
        _context.SaveChanges();
        return cart;
    }

    private static decimal CalculateVoucherDiscount(KhuyenMai voucher, decimal subtotal)
    {
        if (subtotal < voucher.DonToiThieu) return 0;

        decimal discount = voucher.LoaiGiam == "SoTien"
            ? voucher.GiaTri
            : subtotal * voucher.GiaTri / 100m;

        if (voucher.GiamToiDa.HasValue)
        {
            discount = Math.Min(discount, voucher.GiamToiDa.Value);
        }

        return Math.Min(subtotal, Math.Max(0, discount));
    }

    private async Task<KhuyenMai?> GetValidVoucherAsync(string? code, decimal subtotal)
    {
        if (string.IsNullOrWhiteSpace(code)) return null;
        code = code.Trim().ToUpperInvariant();
        var now = DateTime.Now;
        var voucher = await _context.KhuyenMais.FirstOrDefaultAsync(x => x.Code == code && x.TrangThai);
        if (voucher == null) return null;
        if (voucher.NgayBatDau > now || voucher.NgayKetThuc < now) return null;
        if (voucher.SoLuong <= voucher.DaDung) return null;
        if (subtotal < voucher.DonToiThieu) return null;
        return voucher;
    }

    private async Task<CartViewModel> BuildCartViewModelAsync(int userId)
    {
        var cart = GetOrCreateCart(userId);
        var items = await _context.ChiTietGioHangs
            .Include(x => x.SanPham)
                .ThenInclude(x => x!.TacGia)
            .Include(x => x.SanPham)
                .ThenInclude(x => x!.DanhMuc)
            .Where(x => x.MaGioHang == cart.MaGioHang)
            .ToListAsync();

        var subtotal = items.Sum(x => x.SoLuong * x.DonGia);
        var code = HttpContext.Session.GetString(VoucherSessionKey);
        var voucher = await GetValidVoucherAsync(code, subtotal);
        if (!string.IsNullOrWhiteSpace(code) && voucher == null)
        {
            HttpContext.Session.Remove(VoucherSessionKey);
            code = null;
        }

        var now = DateTime.Now;
        return new CartViewModel
        {
            GioHang = cart,
            Items = items,
            NguoiDung = await _context.NguoiDungs.FindAsync(userId),
            MaGiamGiaDangApDung = voucher?.Code,
            MoTaGiamGia = voucher?.MoTa,
            GiamGia = voucher == null ? 0 : CalculateVoucherDiscount(voucher, subtotal),
            MaGiamGiaKhaDung = await _context.KhuyenMais
                .Where(x => x.TrangThai && x.NgayBatDau <= now && x.NgayKetThuc >= now && x.SoLuong > x.DaDung)
                .OrderBy(x => x.DonToiThieu)
                .ToListAsync()
        };
    }

    public async Task<IActionResult> Index()
    {
        var userId = CurrentUserId;
        if (userId == null) return RedirectToLogin();
        return View(await BuildCartViewModelAsync(userId.Value));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApplyVoucher(string code)
    {
        var userId = CurrentUserId;
        if (userId == null) return RedirectToLogin();

        var cartVm = await BuildCartViewModelAsync(userId.Value);
        if (!cartVm.Items.Any())
        {
            TempData["Error"] = "Giỏ hàng đang trống nên chưa thể áp dụng mã giảm giá.";
            return RedirectToAction(nameof(Index));
        }

        var voucher = await GetValidVoucherAsync(code, cartVm.TamTinh);
        if (voucher == null)
        {
            TempData["Error"] = "Mã giảm giá không tồn tại, đã hết hạn, hết lượt hoặc chưa đạt giá trị đơn tối thiểu.";
            return RedirectToAction(nameof(Index));
        }

        HttpContext.Session.SetString(VoucherSessionKey, voucher.Code);
        TempData["Success"] = $"Đã áp dụng mã {voucher.Code}.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveVoucher()
    {
        HttpContext.Session.Remove(VoucherSessionKey);
        TempData["Success"] = "Đã bỏ mã giảm giá khỏi giỏ hàng.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int maSanPham, int soLuong = 1, bool muaNgay = false)
    {
        var userId = CurrentUserId;
        if (userId == null) return RedirectToLogin();

        var product = await _context.SanPhams.FirstOrDefaultAsync(x => x.MaSanPham == maSanPham && x.TrangThai);
        if (product == null) return NotFound();

        if (product.SoLuongTon <= 0)
        {
            TempData["Error"] = "Sản phẩm đã hết hàng.";
            return RedirectToAction("Details", "SanPham", new { id = maSanPham });
        }

        var cart = GetOrCreateCart(userId.Value);
        var item = await _context.ChiTietGioHangs.FirstOrDefaultAsync(x => x.MaGioHang == cart.MaGioHang && x.MaSanPham == maSanPham);

        soLuong = Math.Max(1, soLuong);
        if (item == null)
        {
            item = new ChiTietGioHang
            {
                MaGioHang = cart.MaGioHang,
                MaSanPham = maSanPham,
                SoLuong = Math.Min(soLuong, product.SoLuongTon),
                DonGia = product.GiaBan
            };
            _context.ChiTietGioHangs.Add(item);
        }
        else
        {
            item.SoLuong = Math.Min(item.SoLuong + soLuong, product.SoLuongTon);
            item.DonGia = product.GiaBan;
        }

        await _context.SaveChangesAsync();
        TempData["Success"] = "Đã thêm sản phẩm vào giỏ hàng.";
        if (muaNgay) return RedirectToAction(nameof(Checkout));
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int maSanPham, int soLuong)
    {
        var userId = CurrentUserId;
        if (userId == null) return RedirectToLogin();

        var cart = GetOrCreateCart(userId.Value);
        var item = await _context.ChiTietGioHangs
            .Include(x => x.SanPham)
            .FirstOrDefaultAsync(x => x.MaGioHang == cart.MaGioHang && x.MaSanPham == maSanPham);

        if (item == null) return RedirectToAction(nameof(Index));

        var tonKho = item.SanPham?.SoLuongTon ?? 0;
        if (soLuong <= 0 || tonKho <= 0)
        {
            _context.ChiTietGioHangs.Remove(item);
        }
        else
        {
            item.SoLuong = Math.Max(1, Math.Min(soLuong, tonKho));
            item.DonGia = item.SanPham?.GiaBan ?? item.DonGia;
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int maSanPham)
    {
        var userId = CurrentUserId;
        if (userId == null) return RedirectToLogin();

        var cart = GetOrCreateCart(userId.Value);
        var item = await _context.ChiTietGioHangs.FirstOrDefaultAsync(x => x.MaGioHang == cart.MaGioHang && x.MaSanPham == maSanPham);
        if (item != null)
        {
            _context.ChiTietGioHangs.Remove(item);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        var userId = CurrentUserId;
        if (userId == null) return RedirectToLogin();

        var user = await _context.NguoiDungs.FindAsync(userId.Value);
        var cartVm = await BuildCartViewModelAsync(userId.Value);

        if (!cartVm.Items.Any())
        {
            TempData["Error"] = "Giỏ hàng đang trống.";
            return RedirectToAction(nameof(Index));
        }

        return View(new CheckoutViewModel
        {
            HoTenNguoiNhan = user?.HoTen ?? string.Empty,
            SoDienThoaiNhan = user?.SoDienThoai ?? string.Empty,
            DiaChiGiaoHang = user?.DiaChi ?? string.Empty,
            Items = cartVm.Items,
            MaGiamGiaDangApDung = cartVm.MaGiamGiaDangApDung,
            GiamGia = cartVm.GiamGia,
            PhiShip = cartVm.PhiShip
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CheckoutViewModel model)
    {
        var userId = CurrentUserId;
        if (userId == null) return RedirectToLogin();

        var cart = GetOrCreateCart(userId.Value);
        var items = await _context.ChiTietGioHangs
            .Include(x => x.SanPham)
            .Where(x => x.MaGioHang == cart.MaGioHang)
            .ToListAsync();

        var subtotal = items.Sum(x => x.SoLuong * x.DonGia);
        var voucherCode = HttpContext.Session.GetString(VoucherSessionKey);
        var voucher = await GetValidVoucherAsync(voucherCode, subtotal);
        var discount = voucher == null ? 0 : CalculateVoucherDiscount(voucher, subtotal);

        model.Items = items;
        model.MaGiamGiaDangApDung = voucher?.Code;
        model.GiamGia = discount;

        if (!items.Any())
        {
            TempData["Error"] = "Giỏ hàng đang trống.";
            return RedirectToAction(nameof(Index));
        }

        foreach (var item in items)
        {
            if (item.SanPham == null || item.SoLuong > item.SanPham.SoLuongTon)
            {
                ModelState.AddModelError(string.Empty, $"Số lượng tồn của {item.SanPham?.TenSanPham ?? "sản phẩm"} không đủ.");
            }
        }

        if (!ModelState.IsValid) return View(model);

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var note = model.GhiChu;
            if (voucher != null)
            {
                note = string.IsNullOrWhiteSpace(note)
                    ? $"Áp dụng mã giảm giá: {voucher.Code} (-{discount:N0} đ)"
                    : $"{note} | Áp dụng mã giảm giá: {voucher.Code} (-{discount:N0} đ)";
            }

            var donHang = new DonHang
            {
                MaNguoiDung = userId.Value,
                NgayDat = DateTime.Now,
                HoTenNguoiNhan = model.HoTenNguoiNhan,
                SoDienThoaiNhan = model.SoDienThoaiNhan,
                DiaChiGiaoHang = model.DiaChiGiaoHang,
                PhiShip = model.PhiShip,
                TongTien = Math.Max(0, subtotal - discount + model.PhiShip),
                TrangThaiDonHang = "Chờ xác nhận",
                GhiChu = note
            };
            _context.DonHangs.Add(donHang);
            await _context.SaveChangesAsync();

            foreach (var item in items)
            {
                _context.ChiTietDonHangs.Add(new ChiTietDonHang
                {
                    MaDonHang = donHang.MaDonHang,
                    MaSanPham = item.MaSanPham,
                    SoLuong = item.SoLuong,
                    DonGia = item.DonGia,
                    ThanhTien = item.SoLuong * item.DonGia
                });

                if (item.SanPham != null)
                {
                    item.SanPham.SoLuongTon -= item.SoLuong;
                }
            }

            _context.ThanhToans.Add(new ThanhToan
            {
                MaDonHang = donHang.MaDonHang,
                PhuongThucThanhToan = model.PhuongThucThanhToan,
                SoTienThanhToan = donHang.TongTien,
                TrangThaiThanhToan = model.PhuongThucThanhToan == "COD" ? "Chưa thanh toán" : "Đang xử lý"
            });

            if (voucher != null)
            {
                voucher.DaDung += 1;
            }

            _context.ChiTietGioHangs.RemoveRange(items);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            HttpContext.Session.Remove(VoucherSessionKey);
            TempData["Success"] = "Đặt hàng thành công.";
            return RedirectToAction("Details", "DonHang", new { id = donHang.MaDonHang });
        }
        catch
        {
            await transaction.RollbackAsync();
            ModelState.AddModelError(string.Empty, "Có lỗi khi đặt hàng. Vui lòng thử lại.");
            return View(model);
        }
    }
}
