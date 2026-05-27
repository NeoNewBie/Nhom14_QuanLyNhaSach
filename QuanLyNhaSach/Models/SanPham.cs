using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("SAN_PHAM")]
public class SanPham
{
    [Key]
    public int MaSanPham { get; set; }

    [Required, StringLength(200)]
    public string TenSanPham { get; set; } = string.Empty;

    public string? MoTa { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal GiaBia { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal GiaBan { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal GiaNhap { get; set; }

    public int SoLuongTon { get; set; }

    [StringLength(255)]
    public string? AnhBia { get; set; }

    [StringLength(50)]
    public string LoaiSanPham { get; set; } = "Sách giấy";

    public int MaDanhMuc { get; set; }
    public int? MaTacGia { get; set; }
    public int? MaNhaXuatBan { get; set; }
    public bool TrangThai { get; set; } = true;
    public DateTime NgayTao { get; set; } = DateTime.Now;

    [ForeignKey(nameof(MaDanhMuc))]
    public DanhMuc? DanhMuc { get; set; }

    [ForeignKey(nameof(MaTacGia))]
    public TacGia? TacGia { get; set; }

    [ForeignKey(nameof(MaNhaXuatBan))]
    public NhaXuatBan? NhaXuatBan { get; set; }

    public ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();
    public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
    public ICollection<YeuThich> YeuThichs { get; set; } = new List<YeuThich>();
    public ICollection<DanhGia> DanhGias { get; set; } = new List<DanhGia>();
}
