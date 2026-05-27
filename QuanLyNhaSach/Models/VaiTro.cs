using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("VAI_TRO")]
public class VaiTro
{
    [Key]
    public int MaVaiTro { get; set; }

    [Required, StringLength(50)]
    public string TenVaiTro { get; set; } = string.Empty;

    [StringLength(200)]
    public string? MoTa { get; set; }

    public ICollection<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
}
