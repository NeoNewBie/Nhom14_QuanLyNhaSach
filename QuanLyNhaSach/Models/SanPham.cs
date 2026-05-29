using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuanLyNhaSach.Validations;

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
    [GiaBanHopLe]
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
    public string AnhBiaUrl
    {
        get
        {
            var key = (TenSanPham ?? string.Empty).Trim().ToLowerInvariant();
            var file = (AnhBia ?? string.Empty).Trim().ToLowerInvariant();

            string? googleCover = key switch
            {
                var k when k.Contains("truyện kiều") => "https://books.google.com/books/content?id=95isiQ3ZhN4C&printsec=frontcover&img=1&zoom=1&source=gbs_api",
                var k when k.Contains("dế mèn") => "https://books.google.com/books/content?id=Jces0AEACAAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api",
                var k when k.Contains("1984") => "https://covers.openlibrary.org/b/isbn/9780451524935-L.jpg",
                var k when k.Contains("những nguyên tắc") || k.Contains("7 thói quen") => "https://covers.openlibrary.org/b/isbn/9780743269513-L.jpg",
                var k when k.Contains("thói quen nguyên tử") || k.Contains("atomic") => "https://covers.openlibrary.org/b/isbn/9780735211292-L.jpg",
                var k when k.Contains("đắc nhân tâm") => "https://covers.openlibrary.org/b/isbn/9780671027032-L.jpg",
                var k when k.Contains("nhà giả kim") => "https://covers.openlibrary.org/b/isbn/9780061122415-L.jpg",
                var k when k.Contains("tư duy nhanh") => "https://covers.openlibrary.org/b/isbn/9780374533557-L.jpg",
                var k when k.Contains("sapiens") => "https://covers.openlibrary.org/b/isbn/9780062316097-L.jpg",
                var k when k.Contains("homo deus") => "https://covers.openlibrary.org/b/isbn/9780062464316-L.jpg",
                var k when k.Contains("harry potter") => "https://covers.openlibrary.org/b/isbn/9780590353427-L.jpg",
                var k when k.Contains("clean code") => "https://covers.openlibrary.org/b/isbn/9780132350884-L.jpg",
                var k when k.Contains("pragmatic programmer") => "https://covers.openlibrary.org/b/isbn/9780201616224-L.jpg",
                var k when k.Contains("design patterns") => "https://covers.openlibrary.org/b/isbn/9780201633610-L.jpg",
                var k when k.Contains("python crash") => "https://covers.openlibrary.org/b/isbn/9781593279288-L.jpg",
                var k when k.Contains("fluent python") => "https://covers.openlibrary.org/b/isbn/9781491946008-L.jpg",
                var k when k.Contains("c#") || k.Contains("csharp") => "https://covers.openlibrary.org/b/isbn/9781617291340-L.jpg",
                var k when k.Contains("data science") => "https://covers.openlibrary.org/b/isbn/9781492041139-L.jpg",
                var k when k.Contains("asp.net") => "https://covers.openlibrary.org/b/isbn/9781617294617-L.jpg",
                var k when k.Contains("trí tuệ nhân tạo") || k.Contains("artificial intelligence") => "https://covers.openlibrary.org/b/isbn/9780136042594-L.jpg",
                var k when k.Contains("rừng na uy") || k.Contains("norwegian") => "https://covers.openlibrary.org/b/isbn/9780375704024-L.jpg",
                var k when k.Contains("tôi thấy hoa vàng") => "https://books.google.com/books/content?id=xQMhyAEACAAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api",
                var k when k.Contains("khởi nghiệp") || k.Contains("lean startup") => "https://covers.openlibrary.org/b/isbn/9780307887894-L.jpg",
                var k when k.Contains("marketing 5.0") => "https://covers.openlibrary.org/b/isbn/9781119668510-L.jpg",
                var k when k.Contains("deep work") => "https://covers.openlibrary.org/b/isbn/9781455586691-L.jpg",
                var k when k.Contains("start with why") => "https://covers.openlibrary.org/b/isbn/9781591846444-L.jpg",
                var k when k.Contains("good to great") => "https://covers.openlibrary.org/b/isbn/9780066620992-L.jpg",
                var k when k.Contains("blue ocean") => "https://covers.openlibrary.org/b/isbn/9781591396192-L.jpg",
                var k when k.Contains("rich dad") => "https://covers.openlibrary.org/b/isbn/9781612680194-L.jpg",
                var k when k.Contains("dune") => "https://covers.openlibrary.org/b/isbn/9780441172719-L.jpg",
                var k when k.Contains("little prince") || k.Contains("hoàng tử bé") => "https://covers.openlibrary.org/b/isbn/9780156012195-L.jpg",
                var k when k.Contains("great gatsby") => "https://covers.openlibrary.org/b/isbn/9780743273565-L.jpg",
                var k when k.Contains("brief history of time") => "https://covers.openlibrary.org/b/isbn/9780553380163-L.jpg",
                var k when k.Contains("guns, germs") => "https://covers.openlibrary.org/b/isbn/9780393317558-L.jpg",
                _ => null
            };

            if (!string.IsNullOrWhiteSpace(googleCover)) return googleCover;
            if (string.IsNullOrWhiteSpace(AnhBia)) return "/images/books/placeholder.svg";
            if (AnhBia.StartsWith("http", StringComparison.OrdinalIgnoreCase) || AnhBia.StartsWith("/")) return AnhBia;
            return "/images/books/" + AnhBia.TrimStart('~', '/');
        }
    }

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
