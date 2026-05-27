using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaSach.Models;

namespace QuanLyNhaSach.Controllers;

public class YeuThichController : Controller
{
    private readonly QuanLyBanSachContext _context;

    public YeuThichController(QuanLyBanSachContext context)
    {
        _context = context;
    }

    private int? CurrentUserId => HttpContext.Session.GetInt32("MaNguoiDung");
    private IActionResult RedirectToLogin() => RedirectToAction("Login", "Account", new { returnUrl = Request.Path + Request.QueryString });

    public async Task<IActionResult> Index(string? q, string? tinhTrang, int? maDanhMuc)
    {
        var userId = CurrentUserId;
        if (userId == null) return RedirectToLogin();

        var query = _context.YeuThiches
            .Include(x => x.SanPham)
                .ThenInclude(x => x!.TacGia)
            .Include(x => x.SanPham)
                .ThenInclude(x => x!.DanhMuc)
            .Where(x => x.MaNguoiDung == userId.Value)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();
            query = query.Where(x => x.SanPham != null &&
                (x.SanPham.TenSanPham.Contains(q)
                 || (x.SanPham.TacGia != null && x.SanPham.TacGia.TenTacGia.Contains(q))
                 || (x.SanPham.DanhMuc != null && x.SanPham.DanhMuc.TenDanhMuc.Contains(q))));
        }

        if (maDanhMuc.HasValue) query = query.Where(x => x.SanPham != null && x.SanPham.MaDanhMuc == maDanhMuc.Value);
        if (tinhTrang == "con-hang") query = query.Where(x => x.SanPham != null && x.SanPham.SoLuongTon > 0);
        if (tinhTrang == "het-hang") query = query.Where(x => x.SanPham != null && x.SanPham.SoLuongTon <= 0);

        ViewBag.TuKhoa = q;
        ViewBag.TinhTrang = tinhTrang;
        ViewBag.MaDanhMuc = maDanhMuc;
        ViewBag.DanhMucs = await _context.DanhMucs.Where(x => x.TrangThai).OrderBy(x => x.TenDanhMuc).ToListAsync();
        return View(await query.OrderByDescending(x => x.NgayThem).ToListAsync());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int maSanPham)
    {
        var userId = CurrentUserId;
        if (userId == null) return RedirectToLogin();

        var productExists = await _context.SanPhams.AnyAsync(x => x.MaSanPham == maSanPham && x.TrangThai);
        if (!productExists) return NotFound();

        var exists = await _context.YeuThiches.AnyAsync(x => x.MaNguoiDung == userId.Value && x.MaSanPham == maSanPham);
        if (!exists)
        {
            _context.YeuThiches.Add(new YeuThich { MaNguoiDung = userId.Value, MaSanPham = maSanPham, NgayThem = DateTime.Now });
            await _context.SaveChangesAsync();
            TempData["Success"] = "Đã thêm vào danh sách yêu thích.";
        }
        else
        {
            TempData["Success"] = "Sách đã có trong danh sách yêu thích.";
        }

        return RedirectToAction("Details", "SanPham", new { id = maSanPham });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int maSanPham)
    {
        var userId = CurrentUserId;
        if (userId == null) return RedirectToLogin();

        var item = await _context.YeuThiches.FirstOrDefaultAsync(x => x.MaNguoiDung == userId.Value && x.MaSanPham == maSanPham);
        if (item != null)
        {
            _context.YeuThiches.Remove(item);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Đã xóa sách khỏi danh sách yêu thích.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveSelected(int[] maSanPhams)
    {
        var userId = CurrentUserId;
        if (userId == null) return RedirectToLogin();

        if (maSanPhams == null || maSanPhams.Length == 0)
        {
            TempData["Error"] = "Vui lòng chọn ít nhất một sách để xóa.";
            return RedirectToAction(nameof(Index));
        }

        var selected = await _context.YeuThiches
            .Where(x => x.MaNguoiDung == userId.Value && maSanPhams.Contains(x.MaSanPham))
            .ToListAsync();
        _context.YeuThiches.RemoveRange(selected);
        await _context.SaveChangesAsync();
        TempData["Success"] = $"Đã xóa {selected.Count} sách khỏi danh sách yêu thích.";
        return RedirectToAction(nameof(Index));
    }
}
