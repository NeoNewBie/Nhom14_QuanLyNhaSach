using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("CHI_TIET_GIO_HANG")]
public class ChiTietGioHang
{
    public int MaGioHang { get; set; }
    public int MaSanPham { get; set; }
    public int SoLuong { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DonGia { get; set; }

    [ForeignKey(nameof(MaGioHang))]
    public GioHang? GioHang { get; set; }

    [ForeignKey(nameof(MaSanPham))]
    public SanPham? SanPham { get; set; }
}
