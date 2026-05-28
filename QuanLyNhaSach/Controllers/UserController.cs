using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaSach.Data;
using QuanLyNhaSach.ViewModels;

namespace QuanLyNhaSach.Controllers;

public class UserController : Controller
{
    private readonly QuanLyBanSachContext _context;

    public UserController(QuanLyBanSachContext context)
    {
        _context = context;
    }

    private int? CurrentUserId => HttpContext.Session.GetInt32("UserId") ?? HttpContext.Session.GetInt32("MaNguoiDung");
    private IActionResult LoginRedirect() => RedirectToAction("Login", "Account", new { returnUrl = Request.Path.ToString() });

    public async Task<IActionResult> Profile()
    {
        var userId = CurrentUserId;
        if (userId == null) return LoginRedirect();

        var user = await _context.NguoiDungs.FindAsync(userId.Value);
        if (user == null) return RedirectToAction("Logout", "Account");

        var orders = await _context.DonHangs.Where(x => x.MaNguoiDung == userId.Value).ToListAsync();
        var vm = new ProfileViewModel
        {
            MaNguoiDung = user.MaNguoiDung,
            HoTen = user.HoTen,
            Email = user.Email,
            SoDienThoai = user.SoDienThoai,
            GioiTinh = user.GioiTinh,
            NgaySinh = user.NgaySinh,
            MaXa = user.MaXa,
            SoNha = user.SoNha,
            Duong = user.Duong,
            DiaChi = user.DiaChi,
            XaPhuongs = await _context.XaPhuongs.Include(x => x.MaTinhNavigation).OrderBy(x => x.MaTinhNavigation.TenTinh).ThenBy(x => x.TenXa).ToListAsync(),
            SoDonHang = orders.Count,
            TongChiTieu = orders.Where(x => x.TrangThaiDonHang != "Đã hủy").Sum(x => x.TongTien),
            SoSachYeuThich = GetWishlistCount()
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(ProfileViewModel model)
    {
        var userId = CurrentUserId;
        if (userId == null) return LoginRedirect();

        var user = await _context.NguoiDungs.FindAsync(userId.Value);
        if (user == null) return RedirectToAction("Logout", "Account");

        if (!ModelState.IsValid)
        {
            model.XaPhuongs = await _context.XaPhuongs.Include(x => x.MaTinhNavigation).ToListAsync();
            model.SoSachYeuThich = GetWishlistCount();
            return View(model);
        }

        if (await _context.NguoiDungs.AnyAsync(x => x.MaNguoiDung != userId.Value && x.Email == model.Email))
        {
            ModelState.AddModelError(nameof(model.Email), "Email đã được tài khoản khác sử dụng.");
            model.XaPhuongs = await _context.XaPhuongs.Include(x => x.MaTinhNavigation).ToListAsync();
            model.SoSachYeuThich = GetWishlistCount();
            return View(model);
        }

        user.HoTen = model.HoTen.Trim();
        user.Email = model.Email.Trim();
        user.SoDienThoai = model.SoDienThoai.Trim();
        user.GioiTinh = model.GioiTinh;
        user.NgaySinh = model.NgaySinh;
        user.MaXa = model.MaXa;
        user.SoNha = model.SoNha;
        user.Duong = string.IsNullOrWhiteSpace(model.DiaChi) ? model.Duong : model.DiaChi;

        if (!string.IsNullOrWhiteSpace(model.MatKhauMoi))
        {
            if (model.MatKhauCu != user.MatKhau)
            {
                ModelState.AddModelError(nameof(model.MatKhauCu), "Mật khẩu cũ không đúng.");
                model.XaPhuongs = await _context.XaPhuongs.Include(x => x.MaTinhNavigation).ToListAsync();
                model.SoSachYeuThich = GetWishlistCount();
                return View(model);
            }
            user.MatKhau = model.MatKhauMoi;
        }

        await _context.SaveChangesAsync();
        HttpContext.Session.SetString("UserName", user.HoTen);
        HttpContext.Session.SetString("HoTen", user.HoTen);
        TempData["Success"] = "Đã cập nhật thông tin cá nhân.";
        return RedirectToAction(nameof(Profile));
    }

    public async Task<IActionResult> Notifications()
    {
        var userId = CurrentUserId;
        if (userId == null) return LoginRedirect();
        var orders = await _context.DonHangs
            .Where(x => x.MaNguoiDung == userId.Value)
            .OrderByDescending(x => x.NgayDat)
            .Take(8)
            .ToListAsync();
        ViewBag.Orders = orders;
        ViewBag.WishLowStock = new List<QuanLyNhaSach.Models.YeuThich>();
        return View();
    }

    public IActionResult Security()
    {
        if (CurrentUserId == null) return LoginRedirect();
        return View(new SecurityViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Security(SecurityViewModel model)
    {
        var userId = CurrentUserId;
        if (userId == null) return LoginRedirect();
        if (!ModelState.IsValid) return View(model);

        var user = await _context.NguoiDungs.FindAsync(userId.Value);
        if (user == null) return RedirectToAction("Logout", "Account");
        if (user.MatKhau != model.MatKhauHienTai)
        {
            ModelState.AddModelError(nameof(model.MatKhauHienTai), "Mật khẩu hiện tại không đúng.");
            return View(model);
        }
        user.MatKhau = model.MatKhauMoi;
        await _context.SaveChangesAsync();
        TempData["Success"] = "Đã đổi mật khẩu thành công.";
        return RedirectToAction(nameof(Profile));
    }

    private int GetWishlistCount()
    {
        var json = HttpContext.Session.GetString("WishlistIds");
        if (string.IsNullOrWhiteSpace(json)) return 0;
        try
        {
            return JsonSerializer.Deserialize<List<int>>(json)?.Distinct().Count() ?? 0;
        }
        catch
        {
            return 0;
        }
    }
}
