using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("KHUYEN_MAI")]
public partial class KhuyenMai
{
    [Key]
    public int MaKhuyenMai { get; set; }

    [Required, StringLength(150)]
    public string TenKhuyenMai { get; set; } = string.Empty;

    [StringLength(50)]
    public string LoaiGiamGia { get; set; } = "PhanTram";

    [Column(TypeName = "decimal(18,2)")]
    public decimal GiaTriGiam { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? DieuKienApDung { get; set; }

    public DateTime NgayBatDau { get; set; } = DateTime.Now;
    public DateTime NgayKetThuc { get; set; } = DateTime.Now.AddMonths(1);
    public bool TrangThai { get; set; } = true;

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    [NotMapped]
    public string Code { get => TenKhuyenMai; set => TenKhuyenMai = value; }
    [NotMapped]
    public string? MoTa { get; set; }
    [NotMapped]
    public string LoaiGiam { get => LoaiGiamGia; set => LoaiGiamGia = value; }
    [NotMapped]
    public decimal GiaTri { get => GiaTriGiam; set => GiaTriGiam = value; }
    [NotMapped]
    public decimal DonToiThieu { get => DieuKienApDung ?? 0; set => DieuKienApDung = value; }
    [NotMapped]
    public decimal? GiamToiDa { get; set; }
    [NotMapped]
    public int SoLuong { get; set; } = 100;
    [NotMapped]
    public int DaDung { get; set; } = 0;
}
