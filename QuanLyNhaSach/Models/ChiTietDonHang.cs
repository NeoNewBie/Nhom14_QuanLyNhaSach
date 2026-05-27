using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("CHI_TIET_DON_HANG")]
public class ChiTietDonHang
{
    public int MaDonHang { get; set; }
    public int MaSanPham { get; set; }
    public int SoLuong { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DonGia { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ThanhTien { get; set; }

    [ForeignKey(nameof(MaDonHang))]
    public DonHang? DonHang { get; set; }

    [ForeignKey(nameof(MaSanPham))]
    public SanPham? SanPham { get; set; }
}
