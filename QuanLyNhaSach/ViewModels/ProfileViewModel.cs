using System.ComponentModel.DataAnnotations;

namespace QuanLyNhaSach.ViewModels;

public class ProfileViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập họ tên")]
    public string HoTen { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
    public string SoDienThoai { get; set; } = string.Empty;

    public string? DiaChi { get; set; }
    public bool? GioiTinh { get; set; }
    public DateTime? NgaySinh { get; set; }

    public int SoDonHang { get; set; }
    public int SoSachYeuThich { get; set; }
    public decimal TongChiTieu { get; set; }
}
