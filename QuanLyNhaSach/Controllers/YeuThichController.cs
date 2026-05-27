using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaSach.Data;

namespace QuanLyNhaSach.Controllers;

public class YeuThichController : Controller
{
    private readonly QuanLyBanSachContext _context;
    private const string SessionKey = "WishlistIds";

    public YeuThichController(QuanLyBanSachContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var ids = GetWishlistIds();
        var books = await _context.SanPhams
            .Include(x => x.MaTacGia)
            .Include(x => x.MaDanhMucNavigation)
            .Where(x => ids.Contains(x.MaSanPham) && x.TrangThai)
            .ToListAsync();
        return View(books);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(int maSanPham)
    {
        var ids = GetWishlistIds();
        if (!ids.Contains(maSanPham)) ids.Add(maSanPham);
        SaveWishlistIds(ids);
        TempData["Success"] = "Đã thêm vào danh sách yêu thích.";
        var referer = Request.Headers.Referer.ToString();
        if (string.IsNullOrWhiteSpace(referer))
        {
            return RedirectToAction("Index", "Home");
        }
        return Redirect(referer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remove(int maSanPham)
    {
        var ids = GetWishlistIds();
        ids.Remove(maSanPham);
        SaveWishlistIds(ids);
        return RedirectToAction(nameof(Index));
    }

    private List<int> GetWishlistIds()
    {
        var json = HttpContext.Session.GetString(SessionKey);
        return string.IsNullOrWhiteSpace(json) ? new List<int>() : JsonSerializer.Deserialize<List<int>>(json) ?? new List<int>();
    }

    private void SaveWishlistIds(List<int> ids)
    {
        HttpContext.Session.SetString(SessionKey, JsonSerializer.Serialize(ids.Distinct().ToList()));
    }
}
