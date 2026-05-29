using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

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
    public string LocalCoverUrl => GetOnlineCoverUrl(TenSanPham);

    [NotMapped]
    public string AnhBiaUrl
    {
        get
        {
            // Ưu tiên bảng URL đã kiểm tra theo tên sách để tránh trường hợp database cũ
            // còn lưu link Google Books trả về "image not available".
            if (!string.IsNullOrWhiteSpace(TenSanPham) &&
                OnlineCoverUrls.TryGetValue(TenSanPham.Trim(), out var exactUrl))
            {
                return exactUrl;
            }

            if (!string.IsNullOrWhiteSpace(AnhBia))
            {
                var value = AnhBia.Trim();
                if (value.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                    value.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    return value;
                }
            }

            return GetOnlineCoverUrl(TenSanPham);
        }
    }

    private static readonly Dictionary<string, string> OnlineCoverUrls = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Truyện Kiều"] = "https://www.netabooks.vn/Data/Sites/1/Product/78700/thumbs/truyen-kieu-bia-cung.jpg",
        ["Dế Mèn Phiêu Lưu Ký"] = "https://www.netabooks.vn/Data/Sites/1/Product/38406/thumbs/de-men-phieu-luu-ky-bia-cung.jpg",
        ["Lập trình C# cơ bản đến nâng cao"] = "https://covers.openlibrary.org/b/isbn/9781617294532-L.jpg",
        ["Data Science với Python"] = "https://covers.openlibrary.org/b/isbn/9781098104030-L.jpg",
        ["Những nguyên tắc để thành công"] = "https://covers.openlibrary.org/b/isbn/9781982137274-L.jpg",
        ["Thói quen nguyên tử"] = "https://covers.openlibrary.org/b/isbn/9780735211292-L.jpg",
        ["1984"] = "https://covers.openlibrary.org/b/isbn/9780451524935-L.jpg",
        ["Lịch sử Việt Nam"] = "https://minhkhai.com.vn/hinhlon/8935075937673.jpg",
        ["Norwegian Wood"] = "https://covers.openlibrary.org/b/isbn/9780375704024-L.jpg",
        ["Harry Potter và Hòn Đá Phù Thủy"] = "https://covers.openlibrary.org/b/isbn/9780747532743-L.jpg",
        ["Clean Code C#"] = "https://covers.openlibrary.org/b/isbn/9780132350884-L.jpg",
        ["ASP.NET Core MVC thực chiến"] = "https://covers.openlibrary.org/b/isbn/9781617298301-L.jpg",
        ["Trí tuệ nhân tạo nhập môn"] = "https://covers.openlibrary.org/b/isbn/9780134610993-L.jpg",
        ["Tư duy nhanh và chậm"] = "https://covers.openlibrary.org/b/isbn/9780374533557-L.jpg",
        ["Đắc nhân tâm"] = "https://covers.openlibrary.org/b/isbn/9780671027032-L.jpg",
        ["Nhà giả kim"] = "https://covers.openlibrary.org/b/isbn/9780062315007-L.jpg",
        ["Khởi nghiệp tinh gọn"] = "https://covers.openlibrary.org/b/isbn/9780307887894-L.jpg",
        ["Quản trị học căn bản"] = "https://covers.openlibrary.org/b/isbn/9780134237473-L.jpg",
        ["Marketing 5.0"] = "https://covers.openlibrary.org/b/isbn/9781119668510-L.jpg",
        ["Sapiens - Lược sử loài người"] = "https://covers.openlibrary.org/b/isbn/9780062316097-L.jpg",
        ["Địa lý Việt Nam hiện đại"] = "https://books.google.com/books/content?id=sCDt0AEACAAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api",
        ["Lịch sử thế giới giản lược"] = "https://books.google.com/books/content?id=RqoV0AEACAAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api",
        ["Tôi thấy hoa vàng trên cỏ xanh"] = "https://books.google.com/books/content?id=xQMhyAEACAAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api",
        ["Rừng Na Uy"] = "https://covers.openlibrary.org/b/isbn/9780375704024-L.jpg",
        ["The Pragmatic Programmer"] = "https://covers.openlibrary.org/b/isbn/9780135957059-L.jpg",
        ["Design Patterns"] = "https://covers.openlibrary.org/b/isbn/9780201633610-L.jpg",
        ["Python Crash Course"] = "https://covers.openlibrary.org/b/isbn/9781718502703-L.jpg",
        ["Fluent Python"] = "https://covers.openlibrary.org/b/isbn/9781492056355-L.jpg",
        ["Deep Work"] = "https://covers.openlibrary.org/b/isbn/9781455586691-L.jpg",
        ["Start With Why"] = "https://covers.openlibrary.org/b/isbn/9781591846444-L.jpg",
        ["Good to Great"] = "https://covers.openlibrary.org/b/isbn/9780066620992-L.jpg",
        ["Blue Ocean Strategy"] = "https://covers.openlibrary.org/b/isbn/9781625274496-L.jpg",
        ["Rich Dad Poor Dad"] = "https://covers.openlibrary.org/b/isbn/9781612680194-L.jpg",
        ["Homo Deus"] = "https://covers.openlibrary.org/b/isbn/9780062464316-L.jpg",
        ["Guns, Germs, and Steel"] = "https://covers.openlibrary.org/b/isbn/9780393317558-L.jpg",
        ["A Brief History of Time"] = "https://covers.openlibrary.org/b/isbn/9780553380163-L.jpg",
        ["Dune"] = "https://covers.openlibrary.org/b/isbn/9780441172719-L.jpg",
        ["Hoàng tử bé"] = "https://covers.openlibrary.org/b/isbn/9780156012195-L.jpg",
        ["The Great Gatsby"] = "https://covers.openlibrary.org/b/isbn/9780743273565-L.jpg",
    };

    private static string GetOnlineCoverUrl(string? title)
    {
        if (!string.IsNullOrWhiteSpace(title) && OnlineCoverUrls.TryGetValue(title.Trim(), out var url))
        {
            return url;
        }

        // Fallback cũng là ảnh từ mạng, không dùng bìa tự tạo local.
        return "https://books.google.com/googlebooks/images/no_cover_thumb.gif";
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
