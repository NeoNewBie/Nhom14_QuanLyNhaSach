using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaSach.Data;
using QuanLyNhaSach.Models;

namespace QuanLyNhaSach.Services;

public class SanPhamService : ISanPhamService
{
    private readonly QuanLyBanSachContext _context;

    public SanPhamService(QuanLyBanSachContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SanPham>> LayDanhSachSanPhamAsync()
    {
        return await _context.SanPhams
            .Include(s => s.MaDanhMucNavigation)
            .Include(s => s.MaNhaXuatBanNavigation)
            .Include(s => s.MaTacGia)
            .ToListAsync();
    }

    public async Task<SanPham?> LaySanPhamTheoIdAsync(int id)
    {
        return await _context.SanPhams
            .Include(s => s.MaDanhMucNavigation)
            .Include(s => s.MaNhaXuatBanNavigation)
            .Include(s => s.MaTacGia)
            .FirstOrDefaultAsync(m => m.MaSanPham == id && m.TrangThai);
    }

    public async Task<IEnumerable<SanPham>> LaySanPhamTheoDanhMucAsync(int maDanhMuc)
    {
        return await _context.SanPhams
            .Where(s => s.MaDanhMuc == maDanhMuc)
            .Include(s => s.MaDanhMucNavigation)
            .Include(s => s.MaNhaXuatBanNavigation)
            .Include(s => s.MaTacGia)
            .ToListAsync();
    }
}
