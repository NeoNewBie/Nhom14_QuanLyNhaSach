using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("GIO_HANG")]
public partial class GioHang
{
    [Key]
    public int MaGioHang { get; set; }
    public int MaNguoiDung { get; set; }
    public DateTime NgayTao { get; set; } = DateTime.Now;
    public bool TrangThai { get; set; } = true;

    public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();
    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;

    [NotMapped]
    public NguoiDung? NguoiDung { get => MaNguoiDungNavigation; set { if (value != null) MaNguoiDungNavigation = value; } }
}
