using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("DANH_GIA")]
public class DanhGia
{
    [Key]
    public int MaDanhGia { get; set; }

    public int MaNguoiDung { get; set; }
    public int MaSanPham { get; set; }
    public int MaDonHang { get; set; }

    [Range(1, 5)]
    public int SoSao { get; set; } = 5;

    [StringLength(1000)]
    public string? NoiDung { get; set; }

    public DateTime NgayDanhGia { get; set; } = DateTime.Now;

    [ForeignKey(nameof(MaNguoiDung))]
    public NguoiDung? NguoiDung { get; set; }

    [ForeignKey(nameof(MaSanPham))]
    public SanPham? SanPham { get; set; }

    [ForeignKey(nameof(MaDonHang))]
    public DonHang? DonHang { get; set; }
}
