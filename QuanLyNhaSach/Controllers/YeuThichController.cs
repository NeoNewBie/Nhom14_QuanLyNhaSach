using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaSach.Data;
using QuanLyNhaSach.Models;

namespace QuanLyNhaSach.Controllers;

public class YeuThichController : Controller
{
    private readonly QuanLyBanSachContext _context;
    private const string SessionKey = "WishlistIds";

    public YeuThichController(QuanLyBanSachContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? q, int? maDanhMuc, string? tinhTrang)
    {
        var ids = GetWishlistIds();
        var query = _context.SanPhams
            .Include(x => x.MaTacGia)
            .Include(x => x.MaDanhMucNavigation)
            .Where(x => ids.Contains(x.MaSanPham) && x.TrangThai)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            query = query.Where(x => x.TenSanPham.Contains(q) || x.MaTacGia.Any(tg => tg.TenTacGia.Contains(q)));
        }
        if (maDanhMuc.HasValue)
        {
            query = query.Where(x => x.MaDanhMuc == maDanhMuc.Value);
        }
        if (tinhTrang == "con-hang") query = query.Where(x => x.SoLuongTon > 0);
        if (tinhTrang == "het-hang") query = query.Where(x => x.SoLuongTon <= 0);

        ViewBag.DanhMucs = await _context.DanhMucs.Where(x => x.TrangThai).OrderBy(x => x.TenDanhMuc).ToListAsync();
        ViewBag.TuKhoa = q;
        ViewBag.MaDanhMuc = maDanhMuc;
        ViewBag.TinhTrang = tinhTrang;

        var books = await query.OrderBy(x => x.TenSanPham).ToListAsync();
        var model = books.Select(sp => new YeuThich { MaSanPham = sp.MaSanPham, SanPham = sp, NgayThem = DateTime.Now }).ToList();
        return View(model);
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
        return string.IsNullOrWhiteSpace(referer) ? RedirectToAction("Index", "Home") : Redirect(referer);
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveSelected(List<int> maSanPhams)
    {
        var ids = GetWishlistIds();
        foreach (var id in maSanPhams) ids.Remove(id);
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
