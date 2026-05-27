using Microsoft.EntityFrameworkCore;

namespace QuanLyNhaSach.Models;

public class QuanLyBanSachContext : DbContext
{
    public QuanLyBanSachContext(DbContextOptions<QuanLyBanSachContext> options) : base(options) { }

    public DbSet<VaiTro> VaiTros => Set<VaiTro>();
    public DbSet<NguoiDung> NguoiDungs => Set<NguoiDung>();
    public DbSet<DanhMuc> DanhMucs => Set<DanhMuc>();
    public DbSet<TacGia> TacGias => Set<TacGia>();
    public DbSet<NhaXuatBan> NhaXuatBans => Set<NhaXuatBan>();
    public DbSet<SanPham> SanPhams => Set<SanPham>();
    public DbSet<GioHang> GioHangs => Set<GioHang>();
    public DbSet<ChiTietGioHang> ChiTietGioHangs => Set<ChiTietGioHang>();
    public DbSet<DonHang> DonHangs => Set<DonHang>();
    public DbSet<ChiTietDonHang> ChiTietDonHangs => Set<ChiTietDonHang>();
    public DbSet<ThanhToan> ThanhToans => Set<ThanhToan>();
    public DbSet<YeuThich> YeuThiches => Set<YeuThich>();
    public DbSet<KhuyenMai> KhuyenMais => Set<KhuyenMai>();
    public DbSet<DangKyNhanTin> DangKyNhanTins => Set<DangKyNhanTin>();
    public DbSet<LienHe> LienHes => Set<LienHe>();
    public DbSet<DanhGia> DanhGias => Set<DanhGia>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NguoiDung>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<NguoiDung>()
            .HasIndex(x => x.SoDienThoai)
            .IsUnique();

        modelBuilder.Entity<GioHang>()
            .HasIndex(x => new { x.MaNguoiDung, x.TrangThai })
            .HasDatabaseName("IX_GIO_HANG_NguoiDung_TrangThai");

        modelBuilder.Entity<ChiTietGioHang>()
            .HasKey(x => new { x.MaGioHang, x.MaSanPham });

        modelBuilder.Entity<ChiTietDonHang>()
            .HasKey(x => new { x.MaDonHang, x.MaSanPham });

        modelBuilder.Entity<YeuThich>()
            .HasIndex(x => new { x.MaNguoiDung, x.MaSanPham })
            .IsUnique();

        modelBuilder.Entity<ThanhToan>()
            .HasIndex(x => x.MaDonHang)
            .IsUnique();

        modelBuilder.Entity<KhuyenMai>()
            .HasIndex(x => x.Code)
            .IsUnique();

        modelBuilder.Entity<DangKyNhanTin>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<DanhGia>()
            .HasIndex(x => new { x.MaNguoiDung, x.MaSanPham, x.MaDonHang })
            .IsUnique();
    }
}
