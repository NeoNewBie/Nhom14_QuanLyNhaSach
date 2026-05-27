using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("DANG_KY_NHAN_TIN")]
public class DangKyNhanTin
{
    [Key]
    public int MaDangKy { get; set; }

    [Required, StringLength(100)]
    public string Email { get; set; } = string.Empty;

    public DateTime NgayDangKy { get; set; } = DateTime.Now;
    public bool TrangThai { get; set; } = true;
}
