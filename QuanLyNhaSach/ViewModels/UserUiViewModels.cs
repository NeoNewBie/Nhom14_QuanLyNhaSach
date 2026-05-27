using System.ComponentModel.DataAnnotations;
using QuanLyNhaSach.Models;

namespace QuanLyNhaSach.ViewModels;

public class HomeIndexViewModel
{
    public string? Keyword { get; set; }
    public int? MaDanhMuc { get; set; }
    public List<DanhMuc> DanhMucs { get; set; } = new();
    public List<SanPham> SachMoi { get; set; } = new();
    public List<SanPham> SachBanChay { get; set; } = new();
    public List<SanPham> SanPhams { get; set; } = new();
}

public class ProductListViewModel
{
    public string? Keyword { get; set; }
    public int? MaDanhMuc { get; set; }
    public List<DanhMuc> DanhMucs { get; set; } = new();
    public List<SanPham> SanPhams { get; set; } = new();
}

public class LoginViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập email")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    [DataType(DataType.Password)]
    public string MatKhau { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}

public class RegisterViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập họ tên")]
    public string HoTen { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập email")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
    public string SoDienThoai { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    [DataType(DataType.Password)]
    public string MatKhau { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
    [DataType(DataType.Password)]
    [Compare(nameof(MatKhau), ErrorMessage = "Mật khẩu xác nhận không khớp")]
    public string XacNhanMatKhau { get; set; } = string.Empty;
}

public class CartViewModel
{
    public GioHang? GioHang { get; set; }
    public List<ChiTietGioHang> Items { get; set; } = new();
    public decimal TongTien => Items.Sum(x => x.SoLuong * x.DonGia);
    public decimal PhiShip => Items.Count == 0 ? 0 : 25000;
    public decimal ThanhToan => TongTien + PhiShip;
}

public class CheckoutViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập họ tên người nhận")]
    public string HoTenNguoiNhan { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
    public string SoDienThoaiNhan { get; set; } = string.Empty;

    public int? MaXaGiao { get; set; }

    public string? SoNhaGiao { get; set; }
    public string? DuongGiao { get; set; }
    public string? GhiChu { get; set; }
    public List<XaPhuong> XaPhuongs { get; set; } = new();
}

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

    public bool? GioiTinh { get; set; }
    public DateOnly? NgaySinh { get; set; }
    public int? MaXa { get; set; }
    public string? SoNha { get; set; }
    public string? Duong { get; set; }
    public string? MatKhauCu { get; set; }
    public string? MatKhauMoi { get; set; }
    public List<XaPhuong> XaPhuongs { get; set; } = new();
    public int SoDonHang { get; set; }
    public decimal TongChiTieu { get; set; }
}
