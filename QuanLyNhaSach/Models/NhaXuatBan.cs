using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("NHA_XUAT_BAN")]
public partial class NhaXuatBan
{
    [Key]
    public int MaNhaXuatBan { get; set; }

    [Required, StringLength(150)]
    public string TenNhaXuatBan { get; set; } = string.Empty;

    [StringLength(255)]
    public string? DiaChi { get; set; }

    [StringLength(15)]
    public string? SoDienThoai { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(200)]
    public string? Website { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
