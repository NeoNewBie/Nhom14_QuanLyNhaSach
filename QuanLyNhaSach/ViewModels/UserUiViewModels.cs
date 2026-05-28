using QuanLyNhaSach.Models;

namespace QuanLyNhaSach.ViewModels;

public class ProductListViewModel
{
    public string? Keyword { get; set; }
    public int? MaDanhMuc { get; set; }
    public List<DanhMuc> DanhMucs { get; set; } = new();
    public List<SanPham> SanPhams { get; set; } = new();
}
