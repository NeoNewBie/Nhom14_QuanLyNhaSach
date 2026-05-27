using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (HttpContext.Session.GetInt32("MaNguoiDung") != null)
        {
            if (HttpContext.Session.GetString("VaiTro") == "Admin")
                return RedirectToAction("Dashboard", "Admin");
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
            .Include(x => x.VaiTro)
            .FirstOrDefaultAsync(x => x.Email == model.Email && x.MatKhau == model.MatKhau && x.TrangThai);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không đúng, hoặc tài khoản đã bị khóa.");
            return View(model);
        }

        HttpContext.Session.SetInt32("MaNguoiDung", user.MaNguoiDung);
        HttpContext.Session.SetString("HoTen", user.HoTen);
        HttpContext.Session.SetString("VaiTro", user.VaiTro?.TenVaiTro ?? "User");

        if (user.VaiTro?.TenVaiTro == "Admin")
            return RedirectToAction("Dashboard", "Admin");

        if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            return Redirect(model.ReturnUrl);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        if (await _context.NguoiDungs.AnyAsync(x => x.Email == model.Email))
        {
            ModelState.AddModelError(nameof(model.Email), "Email đã được sử dụng.");
            return View(model);
        }

        if (await _context.NguoiDungs.AnyAsync(x => x.SoDienThoai == model.SoDienThoai))
        {
            ModelState.AddModelError(nameof(model.SoDienThoai), "Số điện thoại đã được sử dụng.");
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
            HoTen = model.HoTen,
            Email = model.Email,
            SoDienThoai = model.SoDienThoai,
            MatKhau = model.MatKhau,
            DiaChi = model.DiaChi,
            MaVaiTro = roleUser.MaVaiTro,
            TrangThai = true,
            NgayTao = DateTime.Now
        };

        _context.NguoiDungs.Add(user);
        await _context.SaveChangesAsync();

        _context.GioHangs.Add(new GioHang { MaNguoiDung = user.MaNguoiDung, TrangThai = true, NgayTao = DateTime.Now });
        await _context.SaveChangesAsync();

        TempData["Success"] = "Đăng ký thành công. Bạn có thể đăng nhập ngay.";
        return RedirectToAction(nameof(Login));
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SocialLogin(string provider)
    {
        provider = string.IsNullOrWhiteSpace(provider) ? "Google" : provider.Trim();
        var email = provider.Equals("Facebook", StringComparison.OrdinalIgnoreCase)
            ? "facebook.demo@bookport.vn"
            : "google.demo@bookport.vn";
        var displayName = provider.Equals("Facebook", StringComparison.OrdinalIgnoreCase)
            ? "Khách Facebook Demo"
            : "Khách Google Demo";

        var roleUser = await _context.VaiTros.FirstOrDefaultAsync(x => x.TenVaiTro == "User");
        if (roleUser == null)
        {
            roleUser = new VaiTro { TenVaiTro = "User", MoTa = "Người dùng thông thường" };
            _context.VaiTros.Add(roleUser);
            await _context.SaveChangesAsync();
        }

        var user = await _context.NguoiDungs.Include(x => x.VaiTro).FirstOrDefaultAsync(x => x.Email == email);
        if (user == null)
        {
            user = new NguoiDung
            {
                HoTen = displayName,
                Email = email,
                SoDienThoai = provider.Equals("Facebook", StringComparison.OrdinalIgnoreCase) ? "0999999902" : "0999999901",
                MatKhau = "social-demo",
                DiaChi = "Đà Nẵng",
                TrangThai = true,
                MaVaiTro = roleUser.MaVaiTro,
                NgayTao = DateTime.Now
            };
            _context.NguoiDungs.Add(user);
            await _context.SaveChangesAsync();
            _context.GioHangs.Add(new GioHang { MaNguoiDung = user.MaNguoiDung, TrangThai = true, NgayTao = DateTime.Now });
            await _context.SaveChangesAsync();
            user.VaiTro = roleUser;
        }

        if (!user.TrangThai)
        {
            TempData["Error"] = "Tài khoản demo này đang bị khóa.";
            return RedirectToAction(nameof(Login));
        }

        HttpContext.Session.SetInt32("MaNguoiDung", user.MaNguoiDung);
        HttpContext.Session.SetString("HoTen", user.HoTen);
        HttpContext.Session.SetString("VaiTro", user.VaiTro?.TenVaiTro ?? "User");
        TempData["Success"] = $"Đã đăng nhập bằng {provider} demo.";
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
