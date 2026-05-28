using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("TAC_GIA")]
public partial class TacGia
{
    [Key]
    public int MaTacGia { get; set; }

    [Required, StringLength(150)]
    public string TenTacGia { get; set; } = string.Empty;

    [StringLength(50)]
    public string? QuocTich { get; set; }

    [StringLength(255)]
    public string? MoTa { get; set; }

    public virtual ICollection<SanPham> MaSanPhams { get; set; } = new List<SanPham>();

    [NotMapped]
    public ICollection<SanPham> SanPhams => MaSanPhams;
}
