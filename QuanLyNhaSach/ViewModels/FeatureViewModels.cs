using System.ComponentModel.DataAnnotations;
using QuanLyNhaSach.Models;

namespace QuanLyNhaSach.ViewModels;

public class SecurityViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập mật khẩu hiện tại")]
    public string MatKhauHienTai { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
    [MinLength(6, ErrorMessage = "Mật khẩu mới tối thiểu 6 ký tự")]
    public string MatKhauMoi { get; set; } = string.Empty;

    [Compare(nameof(MatKhauMoi), ErrorMessage = "Xác nhận mật khẩu không khớp")]
    public string XacNhanMatKhauMoi { get; set; } = string.Empty;
}

public class ReviewViewModel
{
    public int MaSanPham { get; set; }
    public int MaDonHang { get; set; }
    public string TenSanPham { get; set; } = string.Empty;

    [Range(1, 5, ErrorMessage = "Số sao phải từ 1 đến 5")]
    public int SoSao { get; set; } = 5;

    [StringLength(1000, ErrorMessage = "Nội dung đánh giá tối đa 1000 ký tự")]
    public string? NoiDung { get; set; }
}

public class ContactViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập họ tên")]
    public string HoTen { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập email")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
    public string TieuDe { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập nội dung")]
    public string NoiDung { get; set; } = string.Empty;
}

public class AnalyticsViewModel
{
    public decimal DoanhThu { get; set; }
    public int TongDon { get; set; }
    public int DonDaGiao { get; set; }
    public int DonDangXuLy { get; set; }
    public int DonDaHuy { get; set; }
    public List<SanPhamThongKeViewModel> SachBanChay { get; set; } = new();
    public List<DonHang> DonHangGanDay { get; set; } = new();
    public List<SanPham> SachSapHetHang { get; set; } = new();
}
