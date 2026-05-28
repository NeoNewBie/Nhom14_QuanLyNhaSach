using QuanLyNhaSach.Models;

namespace QuanLyNhaSach.ViewModels;

public class DashboardViewModel
{
    public int TongSoSach { get; set; }
    public int TongDonHang { get; set; }
    public int TongKhachHang { get; set; }
    public decimal DoanhThu { get; set; }
    public List<DonHang> DonHangGanDay { get; set; } = new();
    public List<SanPham> SachSapHetHang { get; set; } = new();
    public List<SanPhamThongKeViewModel> SachBanChay { get; set; } = new();
}

public class SanPhamThongKeViewModel
{
    public SanPham? SanPham { get; set; }
    public int SoLuongBan { get; set; }
}
