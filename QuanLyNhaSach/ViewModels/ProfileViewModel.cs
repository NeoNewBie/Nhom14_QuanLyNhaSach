using System.ComponentModel.DataAnnotations;
using QuanLyNhaSach.Models;

namespace QuanLyNhaSach.ViewModels;

public class ProfileViewModel
{
    public int MaNguoiDung { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập họ tên")]
    public string HoTen { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập email")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
    public string SoDienThoai { get; set; } = string.Empty;

    public string? DiaChi { get; set; }
    public bool? GioiTinh { get; set; }
    public DateTime? NgaySinh { get; set; }
    public int? MaXa { get; set; }
    public string? SoNha { get; set; }
    public string? Duong { get; set; }
    public string? MatKhauCu { get; set; }
    public string? MatKhauMoi { get; set; }
    public List<XaPhuong> XaPhuongs { get; set; } = new();
    public int SoDonHang { get; set; }
    public int SoSachYeuThich { get; set; }
    public decimal TongChiTieu { get; set; }
}
