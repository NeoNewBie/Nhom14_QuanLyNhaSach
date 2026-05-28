using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("DANH_GIA")]
public partial class DanhGia
{
    [Key]
    public int MaDanhGia { get; set; }

    public int MaNguoiDung { get; set; }
    public int MaSanPham { get; set; }
    public int? MaDonHang { get; set; }

    [Range(1, 5)]
    public int SoSao { get; set; } = 5;

    [StringLength(1000)]
    public string? NoiDung { get; set; }

    public DateTime NgayDanhGia { get; set; } = DateTime.Now;

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;
    public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
    public virtual DonHang? MaDonHangNavigation { get; set; }

    [NotMapped]
    public NguoiDung? NguoiDung { get => MaNguoiDungNavigation; set { if (value != null) MaNguoiDungNavigation = value; } }

    [NotMapped]
    public SanPham? SanPham { get => MaSanPhamNavigation; set { if (value != null) MaSanPhamNavigation = value; } }

    [NotMapped]
    public DonHang? DonHang { get => MaDonHangNavigation; set => MaDonHangNavigation = value; }
}
