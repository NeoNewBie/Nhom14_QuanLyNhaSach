using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("GIO_HANG")]
public class GioHang
{
    [Key]
    public int MaGioHang { get; set; }
    public int MaNguoiDung { get; set; }
    public DateTime NgayTao { get; set; } = DateTime.Now;
    public bool TrangThai { get; set; } = true;

    [ForeignKey(nameof(MaNguoiDung))]
    public NguoiDung? NguoiDung { get; set; }

    public ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();
}
