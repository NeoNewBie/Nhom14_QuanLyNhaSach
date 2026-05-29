using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaSach.Data;
using QuanLyNhaSach.Models;
using QuanLyNhaSach.Services;
using QuanLyNhaSach.ViewModels;

namespace QuanLyNhaSach.Controllers;

public class SanPhamsController : Controller
{
    // Inject cả Service và Context (Context dùng tạm cho hàm Create nếu chưa chuyển logic vào Service)
    private readonly ISanPhamService _sanPhamService;
    private readonly QuanLyBanSachContext _context;

    public SanPhamsController(ISanPhamService sanPhamService, QuanLyBanSachContext context)
    {
        _sanPhamService = sanPhamService;
        _context = context;
    }

    // Ví dụ hàm Details sau khi dùng DI Service:
    public async Task<IActionResult> Details(int id)
    {
        // Gọi qua Service thay vì tự Linq truy vấn DB
        var sanPham = await _sanPhamService.LaySanPhamTheoIdAsync(id);

        if (sanPham == null)
        {
            return NotFound();
        }

        return View(sanPham);
    }

    public async Task<IActionResult> Index(string? keyword, int? maDanhMuc)
    {
        var query = _context.SanPhams
            .Include(x => x.MaDanhMucNavigation)
            .Include(x => x.MaNhaXuatBanNavigation)
            .Include(x => x.MaTacGia)
            .Where(x => x.TrangThai)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => x.TenSanPham.Contains(keyword) || (x.MoTa != null && x.MoTa.Contains(keyword)));
        }

        if (maDanhMuc.HasValue)
        {
            query = query.Where(x => x.MaDanhMuc == maDanhMuc.Value);
        }

        var vm = new ProductListViewModel
        {
            Keyword = keyword,
            MaDanhMuc = maDanhMuc,
            DanhMucs = await _context.DanhMucs.Where(x => x.TrangThai).OrderBy(x => x.TenDanhMuc).ToListAsync(),
            SanPhams = await query.OrderByDescending(x => x.NgayTao).ToListAsync()
        };

        return View(vm);
    }
    public async Task<IActionResult> Create()
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");
        ViewBag.DanhMucs = await _context.DanhMucs.Where(x => x.TrangThai).ToListAsync();
        ViewBag.NhaXuatBans = await _context.NhaXuatBans.ToListAsync();
        return View(new SanPham { TrangThai = true, NgayTao = DateTime.Now, LoaiSanPham = "Sách" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TenSanPham,MoTa,GiaBia,GiaBan,GiaNhap,SoLuongTon,AnhBia,LoaiSanPham,MaDanhMuc,MaNhaXuatBan,TrangThai")] SanPham sanPham)
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Account");
        if (ModelState.IsValid)
        {
            sanPham.NgayTao = DateTime.Now;
            _context.Add(sanPham);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.DanhMucs = await _context.DanhMucs.Where(x => x.TrangThai).ToListAsync();
        ViewBag.NhaXuatBans = await _context.NhaXuatBans.ToListAsync();
        return View(sanPham);
    }

    private bool IsAdmin() => HttpContext.Session.GetString("Role") == "Admin";
}
