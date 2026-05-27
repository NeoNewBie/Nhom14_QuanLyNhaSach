using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuanLyNhaSach.Models;

public partial class QuanLyBanSachContext : DbContext
{
    public QuanLyBanSachContext()
    {
    }

    public QuanLyBanSachContext(DbContextOptions<QuanLyBanSachContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }

    public virtual DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }

    public virtual DbSet<DanhGium> DanhGia { get; set; }

    public virtual DbSet<DanhMuc> DanhMucs { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<NhaXuatBan> NhaXuatBans { get; set; }

    public virtual DbSet<PhieuNhap> PhieuNhaps { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<TacGium> TacGia { get; set; }

    public virtual DbSet<ThanhToan> ThanhToans { get; set; }

    public virtual DbSet<TinhThanh> TinhThanhs { get; set; }

    public virtual DbSet<TinhThanhCuBackup> TinhThanhCuBackups { get; set; }

    public virtual DbSet<VDonHangDiaChi> VDonHangDiaChis { get; set; }

    public virtual DbSet<VNguoiDungDiaChi> VNguoiDungDiaChis { get; set; }

    public virtual DbSet<VaiTro> VaiTros { get; set; }

    public virtual DbSet<XaPhuong> XaPhuongs { get; set; }

    public virtual DbSet<XaPhuongCuBackup> XaPhuongCuBackups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => new { e.MaDonHang, e.MaSanPham }).HasName("PK__CHI_TIET__DD39F0EF88B2BD7D");

            entity.ToTable("CHI_TIET_DON_HANG", tb =>
                {
                    tb.HasTrigger("TRG_CapNhatKho_KhiBanHang");
                    tb.HasTrigger("TRG_XoaGioHang_KhiMuaHang");
                });

            entity.HasIndex(e => e.MaSanPham, "IX_CHI_TIET_DON_HANG_MaSanPham");

            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ThanhTien).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.MaDonHangNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaDonHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET___MaDon__0D7A0286");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET___MaSan__0E6E26BF");
        });

        modelBuilder.Entity<ChiTietGioHang>(entity =>
        {
            entity.HasKey(e => new { e.MaGioHang, e.MaSanPham }).HasName("PK__CHI_TIET__3AAC69E196AF685B");

            entity.ToTable("CHI_TIET_GIO_HANG");

            entity.HasIndex(e => e.MaSanPham, "IX_CHI_TIET_GIO_HANG_MaSanPham");

            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.MaGioHangNavigation).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.MaGioHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET___MaGio__02084FDA");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET___MaSan__02FC7413");
        });

        modelBuilder.Entity<ChiTietPhieuNhap>(entity =>
        {
            entity.HasKey(e => new { e.MaPhieuNhap, e.MaSanPham }).HasName("PK__CHI_TIET__DBDC9B794E25B2E1");

            entity.ToTable("CHI_TIET_PHIEU_NHAP", tb => tb.HasTrigger("TRG_CapNhatKho_KhiNhapHang"));

            entity.Property(e => e.GiaNhap).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ThanhTien)
                .HasComputedColumnSql("([SoLuong]*[GiaNhap])", true)
                .HasColumnType("decimal(29, 2)");

            entity.HasOne(d => d.MaPhieuNhapNavigation).WithMany(p => p.ChiTietPhieuNhaps)
                .HasForeignKey(d => d.MaPhieuNhap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET___MaPhi__45BE5BA9");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietPhieuNhaps)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET___MaSan__46B27FE2");
        });

        modelBuilder.Entity<DanhGium>(entity =>
        {
            entity.HasKey(e => e.MaDanhGia).HasName("PK__DANH_GIA__AA9515BF9F283065");

            entity.ToTable("DANH_GIA");

            entity.HasIndex(e => e.MaSanPham, "IX_DANH_GIA_MaSanPham");

            entity.HasIndex(e => new { e.MaNguoiDung, e.MaSanPham }, "UQ_DANH_GIA_NguoiDung_SanPham").IsUnique();

            entity.Property(e => e.NgayDanhGia).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DanhGia)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DANH_GIA__MaNguo__160F4887");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.DanhGia)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DANH_GIA__MaSanP__17036CC0");
        });

        modelBuilder.Entity<DanhMuc>(entity =>
        {
            entity.HasKey(e => e.MaDanhMuc).HasName("PK__DANH_MUC__B3750887F3B36BC8");

            entity.ToTable("DANH_MUC");

            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.TenDanhMuc).HasMaxLength(100);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaDanhMucChaNavigation).WithMany(p => p.InverseMaDanhMucChaNavigation)
                .HasForeignKey(d => d.MaDanhMucCha)
                .HasConstraintName("FK__DANH_MUC__MaDanh__6754599E");
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.MaDonHang).HasName("PK__DON_HANG__129584AD4E564846");

            entity.ToTable("DON_HANG");

            entity.HasIndex(e => e.MaNguoiDung, "IX_DON_HANG_MaNguoiDung");

            entity.HasIndex(e => e.MaXaGiao, "IX_DON_HANG_MaXaGiao");

            entity.HasIndex(e => e.NgayDat, "IX_DON_HANG_NgayDat");

            entity.Property(e => e.DuongGiao).HasMaxLength(100);
            entity.Property(e => e.GhiChu).HasMaxLength(255);
            entity.Property(e => e.HoTenNguoiNhan).HasMaxLength(100);
            entity.Property(e => e.NgayDat).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.PhiShip).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SoDienThoaiNhan)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.SoNhaGiao).HasMaxLength(50);
            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThaiDonHang).HasMaxLength(50);

            entity.HasOne(d => d.MaKhuyenMaiNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaKhuyenMai)
                .HasConstraintName("FK__DON_HANG__MaKhuy__08B54D69");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DON_HANG__MaNguo__05D8E0BE");

            entity.HasOne(d => d.MaXaGiaoNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaXaGiao)
                .HasConstraintName("FK_DON_HANG_XA_PHUONG");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.MaGioHang).HasName("PK__GIO_HANG__F5001DA3B341FAD7");

            entity.ToTable("GIO_HANG");

            entity.HasIndex(e => e.MaNguoiDung, "UQ_GIO_HANG_Active")
                .IsUnique()
                .HasFilter("([TrangThai]=(1))");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaNguoiDungNavigation).WithOne(p => p.GioHang)
                .HasForeignKey<GioHang>(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GIO_HANG__MaNguo__7C4F7684");
        });

        modelBuilder.Entity<KhuyenMai>(entity =>
        {
            entity.HasKey(e => e.MaKhuyenMai).HasName("PK__KHUYEN_M__6F56B3BDFD2509B9");

            entity.ToTable("KHUYEN_MAI");

            entity.Property(e => e.DieuKienApDung).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GiaTriGiam).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LoaiGiamGia).HasMaxLength(50);
            entity.Property(e => e.TenKhuyenMai).HasMaxLength(150);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__NGUOI_DU__C539D7628D47F0BF");

            entity.ToTable("NGUOI_DUNG");

            entity.HasIndex(e => e.MaXa, "IX_NGUOI_DUNG_MaXa");

            entity.HasIndex(e => e.SoDienThoai, "UQ__NGUOI_DU__0389B7BD7F9BE7A0").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__NGUOI_DU__A9D10534D19B70F3").IsUnique();

            entity.Property(e => e.Duong).HasMaxLength(100);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.MatKhau)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.SoNha).HasMaxLength(50);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaVaiTroNavigation).WithMany(p => p.NguoiDungs)
                .HasForeignKey(d => d.MaVaiTro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NGUOI_DUN__MaVai__6383C8BA");

            entity.HasOne(d => d.MaXaNavigation).WithMany(p => p.NguoiDungs)
                .HasForeignKey(d => d.MaXa)
                .HasConstraintName("FK_NGUOI_DUNG_XA_PHUONG");
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.MaNhaCungCap).HasName("PK__NHA_CUNG__53DA92055BDCAC2B");

            entity.ToTable("NHA_CUNG_CAP");

            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NguoiLienHe).HasMaxLength(100);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TenNhaCungCap).HasMaxLength(150);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        modelBuilder.Entity<NhaXuatBan>(entity =>
        {
            entity.HasKey(e => e.MaNhaXuatBan).HasName("PK__NHA_XUAT__1AED0BFA50A03E68");

            entity.ToTable("NHA_XUAT_BAN");

            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TenNhaXuatBan).HasMaxLength(150);
            entity.Property(e => e.Website)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PhieuNhap>(entity =>
        {
            entity.HasKey(e => e.MaPhieuNhap).HasName("PK__PHIEU_NH__1470EF3BEDCAC06E");

            entity.ToTable("PHIEU_NHAP");

            entity.Property(e => e.GhiChu).HasMaxLength(255);
            entity.Property(e => e.NgayNhap).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.PhieuNhaps)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PHIEU_NHA__MaNgu__40F9A68C");

            entity.HasOne(d => d.MaNhaCungCapNavigation).WithMany(p => p.PhieuNhaps)
                .HasForeignKey(d => d.MaNhaCungCap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PHIEU_NHA__MaNha__41EDCAC5");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.MaSanPham).HasName("PK__SAN_PHAM__FAC7442DF557371C");

            entity.ToTable("SAN_PHAM");

            entity.HasIndex(e => e.MaDanhMuc, "IX_SAN_PHAM_MaDanhMuc");

            entity.HasIndex(e => e.MaNhaXuatBan, "IX_SAN_PHAM_MaNhaXuatBan");

            entity.Property(e => e.AnhBia)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.GiaBan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GiaBia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GiaNhap).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LoaiSanPham).HasMaxLength(50);
            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TenSanPham).HasMaxLength(200);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaDanhMucNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.MaDanhMuc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SAN_PHAM__MaDanh__72C60C4A");

            entity.HasOne(d => d.MaNhaXuatBanNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.MaNhaXuatBan)
                .HasConstraintName("FK__SAN_PHAM__MaNhaX__73BA3083");

            entity.HasMany(d => d.MaTacGia).WithMany(p => p.MaSanPhams)
                .UsingEntity<Dictionary<string, object>>(
                    "SanPhamTacGium",
                    r => r.HasOne<TacGium>().WithMany()
                        .HasForeignKey("MaTacGia")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__SAN_PHAM___MaTac__797309D9"),
                    l => l.HasOne<SanPham>().WithMany()
                        .HasForeignKey("MaSanPham")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__SAN_PHAM___MaSan__787EE5A0"),
                    j =>
                    {
                        j.HasKey("MaSanPham", "MaTacGia").HasName("PK__SAN_PHAM__85E3A258AF926D61");
                        j.ToTable("SAN_PHAM_TAC_GIA");
                    });
        });

        modelBuilder.Entity<TacGium>(entity =>
        {
            entity.HasKey(e => e.MaTacGia).HasName("PK__TAC_GIA__F24E67569CDFE3D2");

            entity.ToTable("TAC_GIA");

            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.QuocTich).HasMaxLength(50);
            entity.Property(e => e.TenTacGia).HasMaxLength(150);
        });

        modelBuilder.Entity<ThanhToan>(entity =>
        {
            entity.HasKey(e => e.MaThanhToan).HasName("PK__THANH_TO__D4B2584432E6E9E0");

            entity.ToTable("THANH_TOAN");

            entity.HasIndex(e => e.MaDonHang, "UQ_THANH_TOAN_MaDonHang").IsUnique();

            entity.HasIndex(e => e.MaGiaoDich, "UQ__THANH_TO__0A2A24EA41B2C610").IsUnique();

            entity.Property(e => e.MaGiaoDich)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhuongThucThanhToan).HasMaxLength(50);
            entity.Property(e => e.SoTienThanhToan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThaiThanhToan).HasMaxLength(50);

            entity.HasOne(d => d.MaDonHangNavigation).WithOne(p => p.ThanhToan)
                .HasForeignKey<ThanhToan>(d => d.MaDonHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__THANH_TOA__MaDon__123EB7A3");
        });

        modelBuilder.Entity<TinhThanh>(entity =>
        {
            entity.HasKey(e => e.MaTinh).HasName("PK__TINH_THA__4CC544806FC657B9");

            entity.ToTable("TINH_THANH");

            entity.Property(e => e.TenTinh).HasMaxLength(100);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        modelBuilder.Entity<TinhThanhCuBackup>(entity =>
        {
            entity.HasKey(e => e.MaTinh).HasName("PK__TINH_THA__4CC544802B0EAEDD");

            entity.ToTable("TINH_THANH_CU_BACKUP");

            entity.Property(e => e.TenTinh).HasMaxLength(100);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        modelBuilder.Entity<VDonHangDiaChi>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V_DON_HANG_DIA_CHI");

            entity.Property(e => e.DiaChiGiaoDayDu).HasMaxLength(356);
            entity.Property(e => e.DuongGiao).HasMaxLength(100);
            entity.Property(e => e.SoNhaGiao).HasMaxLength(50);
            entity.Property(e => e.TenTinh).HasMaxLength(100);
            entity.Property(e => e.TenXa).HasMaxLength(100);
        });

        modelBuilder.Entity<VNguoiDungDiaChi>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V_NGUOI_DUNG_DIA_CHI");

            entity.Property(e => e.DiaChiDayDu).HasMaxLength(356);
            entity.Property(e => e.Duong).HasMaxLength(100);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.SoNha).HasMaxLength(50);
            entity.Property(e => e.TenTinh).HasMaxLength(100);
            entity.Property(e => e.TenXa).HasMaxLength(100);
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.HasKey(e => e.MaVaiTro).HasName("PK__VAI_TRO__C24C41CFD7087C81");

            entity.ToTable("VAI_TRO");

            entity.HasIndex(e => e.TenVaiTro, "UQ__VAI_TRO__1DA558141E36A961").IsUnique();

            entity.Property(e => e.MoTa).HasMaxLength(200);
            entity.Property(e => e.TenVaiTro).HasMaxLength(50);
        });

        modelBuilder.Entity<XaPhuong>(entity =>
        {
            entity.HasKey(e => e.MaXa).HasName("PK__XA_PHUON__272520C94DC829FC");

            entity.ToTable("XA_PHUONG");

            entity.Property(e => e.TenXa).HasMaxLength(100);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaTinhNavigation).WithMany(p => p.XaPhuongs)
                .HasForeignKey(d => d.MaTinh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__XA_PHUONG__MaTin__72910220");
        });

        modelBuilder.Entity<XaPhuongCuBackup>(entity =>
        {
            entity.HasKey(e => e.MaXa).HasName("PK__XA_PHUON__272520C94B84A7C4");

            entity.ToTable("XA_PHUONG_CU_BACKUP");

            entity.Property(e => e.TenXa).HasMaxLength(100);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaTinhNavigation).WithMany(p => p.XaPhuongCuBackups)
                .HasForeignKey(d => d.MaTinh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__XA_PHUONG__MaTin__1F98B2C1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
