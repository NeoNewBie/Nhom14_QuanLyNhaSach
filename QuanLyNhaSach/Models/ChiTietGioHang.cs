using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("CHI_TIET_GIO_HANG")]
public partial class ChiTietGioHang
{
    public int MaGioHang { get; set; }
    public int MaSanPham { get; set; }
    public int SoLuong { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DonGia { get; set; }

    public virtual GioHang MaGioHangNavigation { get; set; } = null!;
    public virtual SanPham MaSanPhamNavigation { get; set; } = null!;

    [NotMapped]
    public GioHang? GioHang { get => MaGioHangNavigation; set { if (value != null) MaGioHangNavigation = value; } }

    [NotMapped]
    public SanPham? SanPham { get => MaSanPhamNavigation; set { if (value != null) MaSanPhamNavigation = value; } }
}
