using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("CHI_TIET_DON_HANG")]
public partial class ChiTietDonHang
{
    public int MaDonHang { get; set; }
    public int MaSanPham { get; set; }
    public int SoLuong { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DonGia { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ThanhTien { get; set; }

    public virtual DonHang MaDonHangNavigation { get; set; } = null!;
    public virtual SanPham MaSanPhamNavigation { get; set; } = null!;

    [NotMapped]
    public DonHang? DonHang { get => MaDonHangNavigation; set { if (value != null) MaDonHangNavigation = value; } }

    [NotMapped]
    public SanPham? SanPham { get => MaSanPhamNavigation; set { if (value != null) MaSanPhamNavigation = value; } }
}
