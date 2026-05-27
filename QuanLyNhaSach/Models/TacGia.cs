using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("TAC_GIA")]
public class TacGia
{
    [Key]
    public int MaTacGia { get; set; }

    [Required, StringLength(150)]
    public string TenTacGia { get; set; } = string.Empty;

    [StringLength(50)]
    public string? QuocTich { get; set; }

    [StringLength(500)]
    public string? MoTa { get; set; }

    public ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
