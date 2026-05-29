using System.Collections.Generic;
using System.Threading.Tasks;
using QuanLyNhaSach.Models;

namespace QuanLyNhaSach.Services;

public interface ISanPhamService
{
    Task<IEnumerable<SanPham>> LayDanhSachSanPhamAsync();
    Task<SanPham?> LaySanPhamTheoIdAsync(int id);
    Task<IEnumerable<SanPham>> LaySanPhamTheoDanhMucAsync(int maDanhMuc);
}
