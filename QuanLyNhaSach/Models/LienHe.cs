using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("LIEN_HE")]
public class LienHe
{
    [Key]
    public int MaLienHe { get; set; }

    [Required, StringLength(100)]
    public string HoTen { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(150)]
    public string TieuDe { get; set; } = string.Empty;

    [Required]
    public string NoiDung { get; set; } = string.Empty;

    public DateTime NgayGui { get; set; } = DateTime.Now;

    [StringLength(50)]
    public string TrangThaiXuLy { get; set; } = "Chưa xử lý";
}
