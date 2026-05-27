using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("MA_GIAM_GIA")]
public class KhuyenMai
{
    [Key]
    public int MaKhuyenMai { get; set; }

    [Required, StringLength(50)]
    public string Code { get; set; } = string.Empty;

    [StringLength(255)]
    public string? MoTa { get; set; }

    [Required, StringLength(20)]
    public string LoaiGiam { get; set; } = "PhanTram"; // PhanTram hoặc SoTien

    [Column(TypeName = "decimal(18,2)")]
    public decimal GiaTri { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DonToiThieu { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? GiamToiDa { get; set; }

    public DateTime NgayBatDau { get; set; } = DateTime.Now;
    public DateTime NgayKetThuc { get; set; } = DateTime.Now.AddMonths(1);
    public int SoLuong { get; set; } = 100;
    public int DaDung { get; set; } = 0;
    public bool TrangThai { get; set; } = true;
}
