using QuanLyNhaSach.Models;

namespace QuanLyNhaSach.ViewModels;

public class HomeIndexViewModel
{
    public string? Keyword { get; set; }
    public string? TuKhoa { get; set; }
    public int? MaDanhMuc { get; set; }
    public List<DanhMuc> DanhMucs { get; set; } = new();
    public List<SanPham> SachMoi { get; set; } = new();
    public List<SanPham> SachBanChay { get; set; } = new();
    public List<SanPham> SanPhams { get; set; } = new();
    public List<SanPham> TatCaSanPham { get; set; } = new();
}
