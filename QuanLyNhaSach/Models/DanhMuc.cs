using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("DANH_MUC")]
public partial class DanhMuc
{
    [Key]
    public int MaDanhMuc { get; set; }

    [Required, StringLength(100)]
    public string TenDanhMuc { get; set; } = string.Empty;

    [StringLength(255)]
    public string? MoTa { get; set; }

    public int? MaDanhMucCha { get; set; }
    public bool TrangThai { get; set; } = true;

    public virtual DanhMuc? MaDanhMucChaNavigation { get; set; }
    public virtual ICollection<DanhMuc> InverseMaDanhMucChaNavigation { get; set; } = new List<DanhMuc>();
    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
