using Microsoft.AspNetCore.Mvc;
using QuanLyNhaSach.Data;
using QuanLyNhaSach.Models;
using QuanLyNhaSach.ViewModels;

namespace QuanLyNhaSach.Controllers;

public class ContactController : Controller
{
    private readonly QuanLyBanSachContext _context;

    public ContactController(QuanLyBanSachContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var model = new ContactViewModel
        {
            HoTen = HttpContext.Session.GetString("HoTen") ?? string.Empty
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        _context.LienHes.Add(new LienHe
        {
            HoTen = model.HoTen.Trim(),
            Email = model.Email.Trim(),
            TieuDe = model.TieuDe.Trim(),
            NoiDung = model.NoiDung.Trim(),
            NgayGui = DateTime.Now,
            TrangThaiXuLy = "Chưa xử lý"
        });
        await _context.SaveChangesAsync();
        TempData["Success"] = "Đã gửi liên hệ. Quản trị viên sẽ xử lý trong thời gian sớm nhất.";
        return RedirectToAction(nameof(Index));
    }
}
