using QuanLyNhaSach.Models;

namespace QuanLyNhaSach.ViewModels;

public class HomeIndexViewModel
{
    public string? TuKhoa { get; set; }
    public int? MaDanhMuc { get; set; }
    public List<DanhMuc> DanhMucs { get; set; } = new();
    public List<SanPham> SachMoi { get; set; } = new();
    public List<SanPham> SachBanChay { get; set; } = new();
    public List<SanPham> TatCaSanPham { get; set; } = new();
}
