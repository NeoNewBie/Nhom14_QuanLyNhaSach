using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaSach.Data;
using QuanLyNhaSach.Models;
using QuanLyNhaSach.ViewModels;

namespace QuanLyNhaSach.Controllers;

public class AdminController : Controller
{
    private readonly QuanLyBanSachContext _context;

    public AdminController(QuanLyBanSachContext context)
    {
        _context = context;
    }

    private bool IsAdmin => HttpContext.Session.GetString("Role") == "Admin";
    private IActionResult Deny() => RedirectToAction("Login", "Account", new { returnUrl = Request.Path + Request.QueryString });

    private void LoadProductComboboxes()
    {
        ViewBag.DanhMucs = new SelectList(_context.DanhMucs.Where(x => x.TrangThai).OrderBy(x => x.TenDanhMuc), "MaDanhMuc", "TenDanhMuc");
        ViewBag.TacGias = new SelectList(_context.TacGias.OrderBy(x => x.TenTacGia), "MaTacGia", "TenTacGia");
        ViewBag.NhaXuatBans = new SelectList(_context.NhaXuatBans.OrderBy(x => x.TenNhaXuatBan), "MaNhaXuatBan", "TenNhaXuatBan");
    }

    public async Task<IActionResult> Dashboard()
    {
        if (!IsAdmin) return Deny();

        var bestStats = await _context.ChiTietDonHangs
            .GroupBy(x => x.MaSanPham)
            .Select(g => new { MaSanPham = g.Key, SoLuongBan = g.Sum(x => x.SoLuong) })
            .OrderByDescending(x => x.SoLuongBan)
            .Take(5)
            .ToListAsync();

        var ids = bestStats.Select(x => x.MaSanPham).ToList();
        var products = await _context.SanPhams.Where(x => ids.Contains(x.MaSanPham)).ToListAsync();

        var vm = new DashboardViewModel
        {
            TongSoSach = await _context.SanPhams.CountAsync(x => x.TrangThai),
            TongDonHang = await _context.DonHangs.CountAsync(),
            TongKhachHang = await _context.NguoiDungs.Include(x => x.MaVaiTroNavigation).CountAsync(x => x.MaVaiTroNavigation.TenVaiTro == "User"),
            DoanhThu = await _context.DonHangs.Where(x => x.TrangThaiDonHang != "Đã hủy").SumAsync(x => (decimal?)x.TongTien) ?? 0,
            DonHangGanDay = await _context.DonHangs.Include(x => x.MaNguoiDungNavigation).OrderByDescending(x => x.NgayDat).Take(8).ToListAsync(),
            SachSapHetHang = await _context.SanPhams.Where(x => x.TrangThai && x.SoLuongTon <= 10).OrderBy(x => x.SoLuongTon).Take(8).ToListAsync(),
            SachBanChay = bestStats.Select(s => new SanPhamThongKeViewModel
            {
                SanPham = products.FirstOrDefault(p => p.MaSanPham == s.MaSanPham),
                SoLuongBan = s.SoLuongBan
            }).ToList()
        };

        return View(vm);
    }

    public async Task<IActionResult> Orders(string? trangThai, string? q)
    {
        if (!IsAdmin) return Deny();

        var query = _context.DonHangs.Include(x => x.MaNguoiDungNavigation).AsQueryable();
        if (!string.IsNullOrWhiteSpace(trangThai)) query = query.Where(x => x.TrangThaiDonHang == trangThai);
        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();
            if (int.TryParse(q.Replace("BP-", string.Empty).Replace("#", string.Empty), out var maDonHang))
            {
                query = query.Where(x => x.MaDonHang == maDonHang || x.HoTenNguoiNhan.Contains(q) || x.SoDienThoaiNhan.Contains(q));
            }
            else
            {
                query = query.Where(x => x.HoTenNguoiNhan.Contains(q) || x.SoDienThoaiNhan.Contains(q));
            }
        }

        ViewBag.TrangThai = trangThai;
        ViewBag.TuKhoa = q;
        return View(await query.OrderByDescending(x => x.NgayDat).ToListAsync());
    }

    public async Task<IActionResult> OrderDetail(int id)
    {
        if (!IsAdmin) return Deny();

        var order = await _context.DonHangs
            .Include(x => x.MaNguoiDungNavigation)
            .Include(x => x.ThanhToan)
            .Include(x => x.ChiTietDonHangs)
                .ThenInclude(x => x.MaSanPhamNavigation)
                    .ThenInclude(x => x!.MaTacGia)
            .FirstOrDefaultAsync(x => x.MaDonHang == id);

        if (order == null) return NotFound();
        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateOrderStatus(int id, string trangThai)
    {
        if (!IsAdmin) return Deny();

        var order = await _context.DonHangs.Include(x => x.ThanhToan).FirstOrDefaultAsync(x => x.MaDonHang == id);
        if (order == null) return NotFound();

        order.TrangThaiDonHang = trangThai;
        if (order.ThanhToan != null && trangThai == "Đã giao")
        {
            order.ThanhToan.TrangThaiThanhToan = "Đã thanh toán";
            order.ThanhToan.NgayThanhToan = DateTime.Now;
        }

        await _context.SaveChangesAsync();
        TempData["Success"] = "Đã cập nhật trạng thái đơn hàng.";
        return RedirectToAction(nameof(OrderDetail), new { id });
    }

    public async Task<IActionResult> Products(string? q)
    {
        if (!IsAdmin) return Deny();

        var query = _context.SanPhams.Include(x => x.MaDanhMucNavigation).Include(x => x.MaTacGia).Include(x => x.MaNhaXuatBanNavigation).AsQueryable();
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.TenSanPham.Contains(q) || x.MaTacGia.Any(tg => tg.TenTacGia.Contains(q)));
        ViewBag.TuKhoa = q;
        return View(await query.OrderByDescending(x => x.NgayTao).ToListAsync());
    }

    [HttpGet]
    public async Task<IActionResult> ProductForm(int? id)
    {
        if (!IsAdmin) return Deny();
        LoadProductComboboxes();

        if (id == null) return View(new SanPham { TrangThai = true, NgayTao = DateTime.Now, LoaiSanPham = "Sách giấy" });

        var product = await _context.SanPhams.FindAsync(id.Value);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProductForm(SanPham model)
    {
        if (!IsAdmin) return Deny();

        if (!ModelState.IsValid)
        {
            LoadProductComboboxes();
            return View(model);
        }

        if (model.MaSanPham == 0)
        {
            model.NgayTao = DateTime.Now;
            _context.SanPhams.Add(model);
        }
        else
        {
            var product = await _context.SanPhams.FindAsync(model.MaSanPham);
            if (product == null) return NotFound();

            product.TenSanPham = model.TenSanPham;
            product.MoTa = model.MoTa;
            product.GiaBia = model.GiaBia;
            product.GiaBan = model.GiaBan;
            product.GiaNhap = model.GiaNhap;
            product.SoLuongTon = model.SoLuongTon;
            product.AnhBia = model.AnhBia;
            product.LoaiSanPham = model.LoaiSanPham;
            product.MaDanhMuc = model.MaDanhMuc;
            product.MaNhaXuatBan = model.MaNhaXuatBan;
            product.TrangThai = model.TrangThai;
        }

        await _context.SaveChangesAsync();
        TempData["Success"] = "Đã lưu thông tin sách.";
        return RedirectToAction(nameof(Products));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        if (!IsAdmin) return Deny();
        var product = await _context.SanPhams.FindAsync(id);
        if (product != null)
        {
            product.TrangThai = false;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Products));
    }

    public async Task<IActionResult> Categories()
    {
        if (!IsAdmin) return Deny();
        return View(await _context.DanhMucs.OrderBy(x => x.TenDanhMuc).ToListAsync());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveCategory(DanhMuc model)
    {
        if (!IsAdmin) return Deny();
        if (string.IsNullOrWhiteSpace(model.TenDanhMuc)) return RedirectToAction(nameof(Categories));

        if (model.MaDanhMuc == 0)
            _context.DanhMucs.Add(model);
        else
        {
            var category = await _context.DanhMucs.FindAsync(model.MaDanhMuc);
            if (category != null)
            {
                category.TenDanhMuc = model.TenDanhMuc;
                category.MoTa = model.MoTa;
                category.TrangThai = model.TrangThai;
            }
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Categories));
    }

    public async Task<IActionResult> Authors()
    {
        if (!IsAdmin) return Deny();
        return View(await _context.TacGias.OrderBy(x => x.TenTacGia).ToListAsync());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveAuthor(TacGia model)
    {
        if (!IsAdmin) return Deny();
        if (string.IsNullOrWhiteSpace(model.TenTacGia)) return RedirectToAction(nameof(Authors));

        if (model.MaTacGia == 0)
            _context.TacGias.Add(model);
        else
        {
            var author = await _context.TacGias.FindAsync(model.MaTacGia);
            if (author != null)
            {
                author.TenTacGia = model.TenTacGia;
                author.QuocTich = model.QuocTich;
                author.MoTa = model.MoTa;
            }
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Authors));
    }

    public async Task<IActionResult> Publishers()
    {
        if (!IsAdmin) return Deny();
        return View(await _context.NhaXuatBans.OrderBy(x => x.TenNhaXuatBan).ToListAsync());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SavePublisher(NhaXuatBan model)
    {
        if (!IsAdmin) return Deny();
        if (string.IsNullOrWhiteSpace(model.TenNhaXuatBan)) return RedirectToAction(nameof(Publishers));

        if (model.MaNhaXuatBan == 0)
            _context.NhaXuatBans.Add(model);
        else
        {
            var publisher = await _context.NhaXuatBans.FindAsync(model.MaNhaXuatBan);
            if (publisher != null)
            {
                publisher.TenNhaXuatBan = model.TenNhaXuatBan;
                publisher.DiaChi = model.DiaChi;
                publisher.SoDienThoai = model.SoDienThoai;
                publisher.Email = model.Email;
                publisher.Website = model.Website;
            }
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Publishers));
    }

    public async Task<IActionResult> Customers(string? q)
    {
        if (!IsAdmin) return Deny();
        var query = _context.NguoiDungs.Include(x => x.MaVaiTroNavigation).Where(x => x.MaVaiTroNavigation.TenVaiTro == "User").AsQueryable();
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.HoTen.Contains(q) || x.Email.Contains(q) || x.SoDienThoai.Contains(q));
        ViewBag.TuKhoa = q;
        return View(await query.OrderByDescending(x => x.NgayTao).ToListAsync());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleCustomer(int id)
    {
        if (!IsAdmin) return Deny();
        var user = await _context.NguoiDungs.FindAsync(id);
        if (user != null)
        {
            user.TrangThai = !user.TrangThai;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Customers));
    }

    public async Task<IActionResult> Analytics()
    {
        if (!IsAdmin) return Deny();

        var bestStats = await _context.ChiTietDonHangs
            .GroupBy(x => x.MaSanPham)
            .Select(g => new { MaSanPham = g.Key, SoLuongBan = g.Sum(x => x.SoLuong) })
            .OrderByDescending(x => x.SoLuongBan)
            .Take(10)
            .ToListAsync();
        var ids = bestStats.Select(x => x.MaSanPham).ToList();
        var products = await _context.SanPhams.Where(x => ids.Contains(x.MaSanPham)).ToListAsync();

        var vm = new AnalyticsViewModel
        {
            DoanhThu = await _context.DonHangs.Where(x => x.TrangThaiDonHang != "Đã hủy").SumAsync(x => (decimal?)x.TongTien) ?? 0,
            TongDon = await _context.DonHangs.CountAsync(),
            DonDaGiao = await _context.DonHangs.CountAsync(x => x.TrangThaiDonHang == "Đã giao"),
            DonDangXuLy = await _context.DonHangs.CountAsync(x => x.TrangThaiDonHang == "Chờ xác nhận" || x.TrangThaiDonHang == "Đã xác nhận" || x.TrangThaiDonHang == "Đang giao"),
            DonDaHuy = await _context.DonHangs.CountAsync(x => x.TrangThaiDonHang == "Đã hủy"),
            DonHangGanDay = await _context.DonHangs.Include(x => x.MaNguoiDungNavigation).OrderByDescending(x => x.NgayDat).Take(10).ToListAsync(),
            SachSapHetHang = await _context.SanPhams.Where(x => x.TrangThai && x.SoLuongTon <= 10).OrderBy(x => x.SoLuongTon).Take(10).ToListAsync(),
            SachBanChay = bestStats.Select(s => new SanPhamThongKeViewModel
            {
                SanPham = products.FirstOrDefault(p => p.MaSanPham == s.MaSanPham),
                SoLuongBan = s.SoLuongBan
            }).ToList()
        };
        return View(vm);
    }

    public async Task<IActionResult> Invoice(string? q)
    {
        if (!IsAdmin) return Deny();
        var query = _context.DonHangs.Include(x => x.MaNguoiDungNavigation).Include(x => x.ThanhToan).AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();
            query = query.Where(x => x.HoTenNguoiNhan.Contains(q) || x.SoDienThoaiNhan.Contains(q));
        }
        ViewBag.TuKhoa = q;
        return View(await query.OrderByDescending(x => x.NgayDat).ToListAsync());
    }

    public async Task<IActionResult> Schedule()
    {
        if (!IsAdmin) return Deny();
        var orders = await _context.DonHangs
            .Include(x => x.MaNguoiDungNavigation)
            .Where(x => x.TrangThaiDonHang == "Chờ xác nhận" || x.TrangThaiDonHang == "Đã xác nhận" || x.TrangThaiDonHang == "Đang giao")
            .OrderBy(x => x.NgayDat)
            .ToListAsync();
        return View(orders);
    }

    public async Task<IActionResult> Calendar()
    {
        if (!IsAdmin) return Deny();
        var orders = await _context.DonHangs
            .Include(x => x.MaNguoiDungNavigation)
            .OrderByDescending(x => x.NgayDat)
            .Take(60)
            .ToListAsync();
        return View(orders);
    }

    public async Task<IActionResult> Messages(string? trangThai)
    {
        if (!IsAdmin) return Deny();
        var query = _context.LienHes.AsQueryable();
        if (!string.IsNullOrWhiteSpace(trangThai)) query = query.Where(x => x.TrangThaiXuLy == trangThai);
        ViewBag.TrangThai = trangThai;
        return View(await query.OrderByDescending(x => x.NgayGui).ToListAsync());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkMessage(int id, string trangThai)
    {
        if (!IsAdmin) return Deny();
        var message = await _context.LienHes.FindAsync(id);
        if (message == null) return NotFound();
        message.TrangThaiXuLy = string.IsNullOrWhiteSpace(trangThai) ? "Đã xử lý" : trangThai;
        await _context.SaveChangesAsync();
        TempData["Success"] = "Đã cập nhật trạng thái tin nhắn.";
        return RedirectToAction(nameof(Messages));
    }

    public async Task<IActionResult> Notifications()
    {
        if (!IsAdmin) return Deny();
        ViewBag.LowStock = await _context.SanPhams.Where(x => x.TrangThai && x.SoLuongTon <= 10).OrderBy(x => x.SoLuongTon).ToListAsync();
        ViewBag.NewOrders = await _context.DonHangs.Include(x => x.MaNguoiDungNavigation).Where(x => x.TrangThaiDonHang == "Chờ xác nhận").OrderByDescending(x => x.NgayDat).ToListAsync();
        ViewBag.Messages = await _context.LienHes.Where(x => x.TrangThaiXuLy == "Chưa xử lý").OrderByDescending(x => x.NgayGui).ToListAsync();
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Settings()
    {
        if (!IsAdmin) return Deny();
        var userId = HttpContext.Session.GetInt32("MaNguoiDung");
        var admin = userId == null ? null : await _context.NguoiDungs.FindAsync(userId.Value);
        return View(admin);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Settings(string hoTen, string soDienThoai, string? matKhauMoi)
    {
        if (!IsAdmin) return Deny();
        var userId = HttpContext.Session.GetInt32("MaNguoiDung");
        if (userId == null) return Deny();
        var admin = await _context.NguoiDungs.FindAsync(userId.Value);
        if (admin == null) return Deny();

        if (!string.IsNullOrWhiteSpace(hoTen)) admin.HoTen = hoTen.Trim();
        if (!string.IsNullOrWhiteSpace(soDienThoai))
        {
            soDienThoai = soDienThoai.Trim();
            var phoneExists = await _context.NguoiDungs.AnyAsync(x => x.SoDienThoai == soDienThoai && x.MaNguoiDung != admin.MaNguoiDung);
            if (phoneExists)
            {
                TempData["Error"] = "Số điện thoại đã được tài khoản khác sử dụng.";
                return RedirectToAction(nameof(Settings));
            }
            admin.SoDienThoai = soDienThoai;
        }
        if (!string.IsNullOrWhiteSpace(matKhauMoi)) admin.MatKhau = matKhauMoi.Trim();
        await _context.SaveChangesAsync();
        HttpContext.Session.SetString("HoTen", admin.HoTen);
        TempData["Success"] = "Đã cập nhật thiết lập quản trị.";
        return RedirectToAction(nameof(Settings));
    }

    public async Task<IActionResult> ExportPerformance()
    {
        if (!IsAdmin) return Deny();
        var orders = await _context.DonHangs.Include(x => x.MaNguoiDungNavigation).OrderByDescending(x => x.NgayDat).ToListAsync();
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("MaDonHang,KhachHang,NgayDat,TrangThai,TongTien");
        foreach (var o in orders)
        {
            sb.AppendLine($"BP-{o.MaDonHang:000000},{o.HoTenNguoiNhan},{o.NgayDat:yyyy-MM-dd HH:mm},{o.TrangThaiDonHang},{o.TongTien:0}");
        }
        return File(System.Text.Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "BookPort_Performance.csv");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelOrder(int id)
    {
        if (!IsAdmin) return Deny();
        var order = await _context.DonHangs
            .Include(x => x.ChiTietDonHangs)
                .ThenInclude(x => x.MaSanPhamNavigation)
            .Include(x => x.ThanhToan)
            .FirstOrDefaultAsync(x => x.MaDonHang == id);
        if (order == null) return NotFound();
        if (order.TrangThaiDonHang == "Đã giao" || order.TrangThaiDonHang == "Đã hủy")
        {
            TempData["Error"] = "Không thể hủy đơn đã giao hoặc đã hủy.";
            return RedirectToAction(nameof(OrderDetail), new { id });
        }
        foreach (var detail in order.ChiTietDonHangs)
        {
            if (detail.SanPham != null) detail.SanPham.SoLuongTon += detail.SoLuong;
        }
        order.TrangThaiDonHang = "Đã hủy";
        if (order.ThanhToan != null) order.ThanhToan.TrangThaiThanhToan = "Đã hủy";
        await _context.SaveChangesAsync();
        TempData["Success"] = "Đã hủy đơn hàng và hoàn lại tồn kho.";
        return RedirectToAction(nameof(OrderDetail), new { id });
    }

}
