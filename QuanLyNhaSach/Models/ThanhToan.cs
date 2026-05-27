using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("THANH_TOAN")]
public class ThanhToan
{
    [Key]
    public int MaThanhToan { get; set; }
    public int MaDonHang { get; set; }

    [Required, StringLength(50)]
    public string PhuongThucThanhToan { get; set; } = "COD";

    [Column(TypeName = "decimal(18,2)")]
    public decimal SoTienThanhToan { get; set; }

    [Required, StringLength(50)]
    public string TrangThaiThanhToan { get; set; } = "Chưa thanh toán";

    [StringLength(100)]
    public string? MaGiaoDich { get; set; }

    public DateTime? NgayThanhToan { get; set; }

    [ForeignKey(nameof(MaDonHang))]
    public DonHang? DonHang { get; set; }
}
