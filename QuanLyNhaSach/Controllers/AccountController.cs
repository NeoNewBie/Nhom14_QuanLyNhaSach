using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaSach.Data;
using QuanLyNhaSach.Models;
using QuanLyNhaSach.ViewModels;

namespace QuanLyNhaSach.Controllers;

public class AccountController : Controller
{
    private readonly QuanLyBanSachContext _context;

    public AccountController(QuanLyBanSachContext context)
    {
        _context = context;
    }

    public IActionResult Login(string? returnUrl = null)
    {
        if (HttpContext.Session.GetInt32("UserId") != null)
        {
            return RedirectToAction("Index", "Home");
        }
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _context.NguoiDungs
            .Include(x => x.MaVaiTroNavigation)
            .FirstOrDefaultAsync(x => x.Email == model.Email && x.MatKhau == model.MatKhau && x.TrangThai);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không đúng.");
            return View(model);
        }

        HttpContext.Session.SetInt32("UserId", user.MaNguoiDung);
        HttpContext.Session.SetString("UserName", user.HoTen);
        HttpContext.Session.SetString("HoTen", user.HoTen);
        HttpContext.Session.SetString("Role", user.MaVaiTroNavigation.TenVaiTro);
        HttpContext.Session.SetString("VaiTro", user.MaVaiTroNavigation.TenVaiTro);

        if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }

        if (user.MaVaiTroNavigation.TenVaiTro.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        {
            return RedirectToAction("Index", "Home");
        }

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        if (await _context.NguoiDungs.AnyAsync(x => x.Email == model.Email))
        {
            ModelState.AddModelError(nameof(model.Email), "Email đã tồn tại.");
            return View(model);
        }

        if (await _context.NguoiDungs.AnyAsync(x => x.SoDienThoai == model.SoDienThoai))
        {
            ModelState.AddModelError(nameof(model.SoDienThoai), "Số điện thoại đã tồn tại.");
            return View(model);
        }

        var roleUser = await _context.VaiTros.FirstOrDefaultAsync(x => x.TenVaiTro == "User");
        if (roleUser == null)
        {
            roleUser = new VaiTro { TenVaiTro = "User", MoTa = "Người dùng thông thường" };
            _context.VaiTros.Add(roleUser);
            await _context.SaveChangesAsync();
        }

        var user = new NguoiDung
        {
            HoTen = model.HoTen.Trim(),
            Email = model.Email.Trim(),
            SoDienThoai = model.SoDienThoai.Trim(),
            MatKhau = model.MatKhau,
            TrangThai = true,
            MaVaiTro = roleUser.MaVaiTro,
            NgayTao = DateTime.Now
        };

        _context.NguoiDungs.Add(user);
        await _context.SaveChangesAsync();

        _context.GioHangs.Add(new GioHang
        {
            MaNguoiDung = user.MaNguoiDung,
            NgayTao = DateTime.Now,
            TrangThai = true
        });
        await _context.SaveChangesAsync();

        TempData["Success"] = "Đăng ký thành công. Vui lòng đăng nhập.";
        return RedirectToAction(nameof(Login));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SocialLogin(string provider)
    {
        var user = await _context.NguoiDungs
            .Include(x => x.MaVaiTroNavigation)
            .FirstOrDefaultAsync(x => x.Email == "customer1@gmail.com" && x.TrangThai);

        if (user == null)
        {
            TempData["Error"] = "Chưa có tài khoản demo để đăng nhập nhanh. Vui lòng chạy lại file SQL hoặc đăng ký tài khoản mới.";
            return RedirectToAction(nameof(Login));
        }

        HttpContext.Session.SetInt32("UserId", user.MaNguoiDung);
        HttpContext.Session.SetString("UserName", user.HoTen);
        HttpContext.Session.SetString("HoTen", user.HoTen);
        HttpContext.Session.SetString("Role", user.MaVaiTroNavigation.TenVaiTro);
        HttpContext.Session.SetString("VaiTro", user.MaVaiTroNavigation.TenVaiTro);
        TempData["Success"] = $"Đăng nhập demo bằng {provider}.";
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
