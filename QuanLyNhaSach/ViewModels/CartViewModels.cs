using System.ComponentModel.DataAnnotations;
using QuanLyNhaSach.Models;

namespace QuanLyNhaSach.ViewModels;

public class CartViewModel
{
    public GioHang? GioHang { get; set; }
    public NguoiDung? NguoiDung { get; set; }
    public List<ChiTietGioHang> Items { get; set; } = new();
    public List<KhuyenMai> MaGiamGiaKhaDung { get; set; } = new();
    public string? MaGiamGiaDangApDung { get; set; }
    public string? MoTaGiamGia { get; set; }

    public decimal TamTinh => Items.Sum(x => x.SoLuong * x.DonGia);
    public decimal GiamGia { get; set; }
    public decimal PhiShip { get; set; } = 0;
    public decimal TongTien => Math.Max(0, TamTinh - GiamGia + PhiShip);
}

public class CheckoutViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập họ tên người nhận")]
    public string HoTenNguoiNhan { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
    public string SoDienThoaiNhan { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
    public string DiaChiGiaoHang { get; set; } = string.Empty;

    public string? GhiChu { get; set; }
    public string PhuongThucThanhToan { get; set; } = "COD";
    public List<ChiTietGioHang> Items { get; set; } = new();
    public string? MaGiamGiaDangApDung { get; set; }
    public decimal GiamGia { get; set; }
    public decimal PhiShip { get; set; } = 0;
    public decimal TamTinh => Items.Sum(x => x.SoLuong * x.DonGia);
    public decimal TongTien => Math.Max(0, TamTinh - GiamGia + PhiShip);
}
