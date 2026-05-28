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

    public async Task<IActionResult> Profile()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account", new { returnUrl = Request.Path.ToString() });

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
            XaPhuongs = await _context.XaPhuongs.Include(x => x.MaTinhNavigation).OrderBy(x => x.MaTinhNavigation.TenTinh).ThenBy(x => x.TenXa).ToListAsync(),
            SoDonHang = orders.Count,
            TongChiTieu = orders.Where(x => x.TrangThaiDonHang != "Đã hủy").Sum(x => x.TongTien)
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(ProfileViewModel model)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        var user = await _context.NguoiDungs.FindAsync(userId.Value);
        if (user == null) return RedirectToAction("Logout", "Account");

        if (!ModelState.IsValid)
        {
            model.XaPhuongs = await _context.XaPhuongs.Include(x => x.MaTinhNavigation).ToListAsync();
            return View(model);
        }

        if (await _context.NguoiDungs.AnyAsync(x => x.MaNguoiDung != userId.Value && x.Email == model.Email))
        {
            ModelState.AddModelError(nameof(model.Email), "Email đã được tài khoản khác sử dụng.");
            model.XaPhuongs = await _context.XaPhuongs.Include(x => x.MaTinhNavigation).ToListAsync();
            return View(model);
        }

        user.HoTen = model.HoTen;
        user.Email = model.Email;
        user.SoDienThoai = model.SoDienThoai;
        user.GioiTinh = model.GioiTinh;
        user.NgaySinh = model.NgaySinh;
        user.MaXa = model.MaXa;
        user.SoNha = model.SoNha;
        user.Duong = model.Duong;

        if (!string.IsNullOrWhiteSpace(model.MatKhauMoi))
        {
            if (model.MatKhauCu != user.MatKhau)
            {
                ModelState.AddModelError(nameof(model.MatKhauCu), "Mật khẩu cũ không đúng.");
                model.XaPhuongs = await _context.XaPhuongs.Include(x => x.MaTinhNavigation).ToListAsync();
                return View(model);
            }
            user.MatKhau = model.MatKhauMoi;
        }

        await _context.SaveChangesAsync();
        HttpContext.Session.SetString("UserName", user.HoTen);
        TempData["Success"] = "Đã cập nhật thông tin cá nhân.";
        return RedirectToAction(nameof(Profile));
    }
}
