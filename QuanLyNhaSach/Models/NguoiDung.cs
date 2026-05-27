using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("NGUOI_DUNG")]
public class NguoiDung
{
    [Key]
    public int MaNguoiDung { get; set; }

    [Required, StringLength(100)]
    public string HoTen { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(15)]
    public string SoDienThoai { get; set; } = string.Empty;

    [Required, StringLength(255)]
    public string MatKhau { get; set; } = string.Empty;

    [StringLength(255)]
    public string? DiaChi { get; set; }

    public bool? GioiTinh { get; set; }
    public DateTime? NgaySinh { get; set; }
    public bool TrangThai { get; set; } = true;
    public int MaVaiTro { get; set; }
    public DateTime NgayTao { get; set; } = DateTime.Now;

    [ForeignKey(nameof(MaVaiTro))]
    public VaiTro? VaiTro { get; set; }

<<<<<<< HEAD
    public GioHang? GioHang { get; set; }
    public ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
    public ICollection<YeuThich> YeuThichs { get; set; } = new List<YeuThich>();
    public ICollection<DanhGia> DanhGias { get; set; } = new List<DanhGia>();
=======
    public int? MaXa { get; set; }

    public string? SoNha { get; set; }

    public string? Duong { get; set; }

    public virtual ICollection<DanhGium> DanhGia { get; set; } = new List<DanhGium>();

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual GioHang? GioHang { get; set; }

    public virtual VaiTro MaVaiTroNavigation { get; set; } = null!;

    public virtual XaPhuong? MaXaNavigation { get; set; }

    public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; } = new List<PhieuNhap>();
>>>>>>> origin/main
}
