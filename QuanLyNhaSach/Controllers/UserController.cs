using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaSach.Models;
using QuanLyNhaSach.ViewModels;

namespace QuanLyNhaSach.Controllers;

public class UserController : Controller
{
    private readonly QuanLyBanSachContext _context;

    public UserController(QuanLyBanSachContext context)
    {
        _context = context;
    }

    private int? CurrentUserId => HttpContext.Session.GetInt32("MaNguoiDung");
    private IActionResult RedirectToLogin() => RedirectToAction("Login", "Account", new { returnUrl = Request.Path + Request.QueryString });

    private async Task LoadStats(ProfileViewModel model, int userId)
    {
        model.SoDonHang = await _context.DonHangs.CountAsync(x => x.MaNguoiDung == userId);
        model.SoSachYeuThich = await _context.YeuThiches.CountAsync(x => x.MaNguoiDung == userId);
        model.TongChiTieu = await _context.DonHangs
            .Where(x => x.MaNguoiDung == userId && x.TrangThaiDonHang != "Đã hủy")
            .SumAsync(x => (decimal?)x.TongTien) ?? 0;
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var userId = CurrentUserId;
        if (userId == null) return RedirectToLogin();

        var user = await _context.NguoiDungs.FindAsync(userId.Value);
        if (user == null) return RedirectToLogin();

        var model = new ProfileViewModel
        {
            HoTen = user.HoTen,
            Email = user.Email,
            SoDienThoai = user.SoDienThoai,
            DiaChi = user.DiaChi,
            GioiTinh = user.GioiTinh,
            NgaySinh = user.NgaySinh
        };
        await LoadStats(model, userId.Value);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(ProfileViewModel model)
    {
        var userId = CurrentUserId;
        if (userId == null) return RedirectToLogin();

        var user = await _context.NguoiDungs.FindAsync(userId.Value);
        if (user == null) return RedirectToLogin();

        model.Email = user.Email;
        await LoadStats(model, userId.Value);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var phoneUsed = await _context.NguoiDungs.AnyAsync(x => x.MaNguoiDung != userId.Value && x.SoDienThoai == model.SoDienThoai);
        if (phoneUsed)
        {
            ModelState.AddModelError(nameof(model.SoDienThoai), "Số điện thoại đã được tài khoản khác sử dụng.");
            return View(model);
        }

        user.HoTen = model.HoTen;
        user.SoDienThoai = model.SoDienThoai;
        user.DiaChi = model.DiaChi;
        user.GioiTinh = model.GioiTinh;
        user.NgaySinh = model.NgaySinh;

        await _context.SaveChangesAsync();
        HttpContext.Session.SetString("HoTen", user.HoTen);
        TempData["Success"] = "Đã cập nhật thông tin cá nhân.";
        return RedirectToAction(nameof(Profile));
    }

    [HttpGet]
    public async Task<IActionResult> Notifications()
    {
        var userId = CurrentUserId;
        if (userId == null) return RedirectToLogin();

        var orders = await _context.DonHangs
            .Where(x => x.MaNguoiDung == userId.Value)
            .OrderByDescending(x => x.NgayDat)
            .Take(8)
            .ToListAsync();

        var wishLowStock = await _context.YeuThiches
            .Include(x => x.SanPham)
            .Where(x => x.MaNguoiDung == userId.Value && x.SanPham != null && x.SanPham.SoLuongTon <= 5)
            .Take(5)
            .ToListAsync();

        ViewBag.Orders = orders;
        ViewBag.WishLowStock = wishLowStock;
        return View();
    }

    [HttpGet]
    public IActionResult Security()
    {
        if (CurrentUserId == null) return RedirectToLogin();
        return View(new SecurityViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Security(SecurityViewModel model)
    {
        var userId = CurrentUserId;
        if (userId == null) return RedirectToLogin();

        var user = await _context.NguoiDungs.FindAsync(userId.Value);
        if (user == null) return RedirectToLogin();

        if (!ModelState.IsValid) return View(model);

        if (user.MatKhau != model.MatKhauHienTai)
        {
            ModelState.AddModelError(nameof(model.MatKhauHienTai), "Mật khẩu hiện tại không đúng.");
            return View(model);
        }

        user.MatKhau = model.MatKhauMoi;
        await _context.SaveChangesAsync();
        TempData["Success"] = "Đã đổi mật khẩu thành công.";
        return RedirectToAction(nameof(Security));
    }
}
