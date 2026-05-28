using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaSach.Models;

[Table("SAN_PHAM")]
public partial class SanPham
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
    public int? MaNhaXuatBan { get; set; }
    public bool TrangThai { get; set; } = true;
    public DateTime NgayTao { get; set; } = DateTime.Now;

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
    public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();
    public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();
    public virtual ICollection<DanhGia> DanhGia { get; set; } = new List<DanhGia>();
    public virtual DanhMuc MaDanhMucNavigation { get; set; } = null!;
    public virtual NhaXuatBan? MaNhaXuatBanNavigation { get; set; }
    public virtual ICollection<TacGia> MaTacGia { get; set; } = new List<TacGia>();
    [NotMapped]
    public virtual ICollection<YeuThich> YeuThichs { get; set; } = new List<YeuThich>();

    [NotMapped]
    public DanhMuc? DanhMuc { get => MaDanhMucNavigation; set { if (value != null) MaDanhMucNavigation = value; } }

    [NotMapped]
    public NhaXuatBan? NhaXuatBan { get => MaNhaXuatBanNavigation; set => MaNhaXuatBanNavigation = value; }

    [NotMapped]
    public TacGia? TacGia
    {
        get => MaTacGia.FirstOrDefault();
        set { if (value != null && !MaTacGia.Any(x => x.MaTacGia == value.MaTacGia)) MaTacGia.Add(value); }
    }

    [NotMapped]
    public ICollection<DanhGia> DanhGias => DanhGia;

    [NotMapped]
    public int? TacGiaId
    {
        get => TacGia?.MaTacGia;
        set { }
    }
}
