using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("NGUOI_DUNG")]
public partial class NguoiDung
{
    [Key]
    public int MaNguoiDung { get; set; }

    [Required, StringLength(100)]
    public string HoTen { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email không được để trống")]
    [StringLength(100)]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [StringLength(15)]
    public string SoDienThoai { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mật khẩu không được để trống")]
    [StringLength(255, MinimumLength = 6,
        ErrorMessage = "Mật khẩu phải từ 6 ký tự trở lên")]
    [DataType(DataType.Password)]
    public string MatKhau { get; set; } = string.Empty;

    public bool? GioiTinh { get; set; }
    public DateTime? NgaySinh { get; set; }
    public bool TrangThai { get; set; } = true;
    public int MaVaiTro { get; set; }
    public DateTime NgayTao { get; set; } = DateTime.Now;
    public int? MaXa { get; set; }
    public string? SoNha { get; set; }
    public string? Duong { get; set; }

    public virtual ICollection<DanhGia> DanhGia { get; set; } = new List<DanhGia>();
    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
    public virtual GioHang? GioHang { get; set; }
    public virtual VaiTro MaVaiTroNavigation { get; set; } = null!;
    public virtual XaPhuong? MaXaNavigation { get; set; }
    public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; } = new List<PhieuNhap>();
    [NotMapped]
    public virtual ICollection<YeuThich> YeuThichs { get; set; } = new List<YeuThich>();

    [NotMapped]
    public VaiTro? VaiTro { get => MaVaiTroNavigation; set { if (value != null) MaVaiTroNavigation = value; } }

    [NotMapped]
    public ICollection<DanhGia> DanhGias => DanhGia;

    [NotMapped]
    public string? DiaChi
    {
        get
        {
            var parts = new[] { SoNha, Duong, MaXaNavigation?.TenXa, MaXaNavigation?.MaTinhNavigation?.TenTinh }
                .Where(x => !string.IsNullOrWhiteSpace(x));
            var text = string.Join(", ", parts);
            return string.IsNullOrWhiteSpace(text) ? null : text;
        }
        set => Duong = value;
    }
}
