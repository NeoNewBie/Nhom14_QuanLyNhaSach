using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("YEU_THICH")]
public class YeuThich
{
    [Key]
    public int MaYeuThich { get; set; }
    public int MaNguoiDung { get; set; }
    public int MaSanPham { get; set; }
    public DateTime NgayThem { get; set; } = DateTime.Now;

    [ForeignKey(nameof(MaNguoiDung))]
    public NguoiDung? NguoiDung { get; set; }

    [ForeignKey(nameof(MaSanPham))]
    public SanPham? SanPham { get; set; }
}
