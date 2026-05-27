using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("DON_HANG")]
public class DonHang
{
    [Key]
    public int MaDonHang { get; set; }
    public int MaNguoiDung { get; set; }
    public DateTime NgayDat { get; set; } = DateTime.Now;

    [Required, StringLength(100)]
    public string HoTenNguoiNhan { get; set; } = string.Empty;

    [Required, StringLength(15)]
    public string SoDienThoaiNhan { get; set; } = string.Empty;

    [Required, StringLength(255)]
    public string DiaChiGiaoHang { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TongTien { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PhiShip { get; set; }

    [Required, StringLength(50)]
    public string TrangThaiDonHang { get; set; } = "Chờ xác nhận";

    [StringLength(255)]
    public string? GhiChu { get; set; }

    [ForeignKey(nameof(MaNguoiDung))]
    public NguoiDung? NguoiDung { get; set; }

<<<<<<< HEAD
    public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
    public ThanhToan? ThanhToan { get; set; }
=======
    public int? MaXaGiao { get; set; }

    public string? SoNhaGiao { get; set; }

    public string? DuongGiao { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual KhuyenMai? MaKhuyenMaiNavigation { get; set; }

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;

    public virtual XaPhuong? MaXaGiaoNavigation { get; set; }

    public virtual ThanhToan? ThanhToan { get; set; }
>>>>>>> origin/main
}
