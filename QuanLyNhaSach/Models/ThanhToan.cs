using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("THANH_TOAN")]
public partial class ThanhToan
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

    public virtual DonHang MaDonHangNavigation { get; set; } = null!;

    [NotMapped]
    public DonHang? DonHang { get => MaDonHangNavigation; set { if (value != null) MaDonHangNavigation = value; } }
}
