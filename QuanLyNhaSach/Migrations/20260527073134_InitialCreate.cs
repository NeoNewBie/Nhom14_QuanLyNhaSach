using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyNhaSach.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DANH_MUC",
                columns: table => new
                {
                    MaDanhMuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDanhMuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MaDanhMucCha = table.Column<int>(type: "int", nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DANH_MUC__B3750887FFD3FCA9", x => x.MaDanhMuc);
                    table.ForeignKey(
                        name: "FK__DANH_MUC__MaDanh__5441852A",
                        column: x => x.MaDanhMucCha,
                        principalTable: "DANH_MUC",
                        principalColumn: "MaDanhMuc");
                });

            migrationBuilder.CreateTable(
                name: "KHUYEN_MAI",
                columns: table => new
                {
                    MaKhuyenMai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenKhuyenMai = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    LoaiGiamGia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GiaTriGiam = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DieuKienApDung = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KHUYEN_M__6F56B3BD39408103", x => x.MaKhuyenMai);
                });

            migrationBuilder.CreateTable(
                name: "NHA_CUNG_CAP",
                columns: table => new
                {
                    MaNhaCungCap = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhaCungCap = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NguoiLienHe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SoDienThoai = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NHA_CUNG__53DA92057A66061F", x => x.MaNhaCungCap);
                });

            migrationBuilder.CreateTable(
                name: "NHA_XUAT_BAN",
                columns: table => new
                {
                    MaNhaXuatBan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhaXuatBan = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SoDienThoai = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Website = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NHA_XUAT__1AED0BFAF5993CD2", x => x.MaNhaXuatBan);
                });

            migrationBuilder.CreateTable(
                name: "TAC_GIA",
                columns: table => new
                {
                    MaTacGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTacGia = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    QuocTich = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TAC_GIA__F24E675636A84915", x => x.MaTacGia);
                });

            migrationBuilder.CreateTable(
                name: "TINH_THANH",
                columns: table => new
                {
                    MaTinh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTinh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TINH_THA__4CC54480865F2FE2", x => x.MaTinh);
                });

            migrationBuilder.CreateTable(
                name: "VAI_TRO",
                columns: table => new
                {
                    MaVaiTro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenVaiTro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VAI_TRO__C24C41CF00E3B467", x => x.MaVaiTro);
                });

            migrationBuilder.CreateTable(
                name: "SAN_PHAM",
                columns: table => new
                {
                    MaSanPham = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenSanPham = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GiaBia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GiaBan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoLuongTon = table.Column<int>(type: "int", nullable: false),
                    AnhBia = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    LoaiSanPham = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaDanhMuc = table.Column<int>(type: "int", nullable: false),
                    MaNhaXuatBan = table.Column<int>(type: "int", nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    GiaNhap = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SAN_PHAM__FAC7442D488C7ED5", x => x.MaSanPham);
                    table.ForeignKey(
                        name: "FK__SAN_PHAM__MaDanh__5FB337D6",
                        column: x => x.MaDanhMuc,
                        principalTable: "DANH_MUC",
                        principalColumn: "MaDanhMuc");
                    table.ForeignKey(
                        name: "FK__SAN_PHAM__MaNhaX__60A75C0F",
                        column: x => x.MaNhaXuatBan,
                        principalTable: "NHA_XUAT_BAN",
                        principalColumn: "MaNhaXuatBan");
                });

            migrationBuilder.CreateTable(
                name: "XA_PHUONG",
                columns: table => new
                {
                    MaXa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenXa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MaTinh = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__XA_PHUON__272520C9C6F8E3AA", x => x.MaXa);
                    table.ForeignKey(
                        name: "FK__XA_PHUONG__MaTin__0C85DE4D",
                        column: x => x.MaTinh,
                        principalTable: "TINH_THANH",
                        principalColumn: "MaTinh");
                });

            migrationBuilder.CreateTable(
                name: "SAN_PHAM_TAC_GIA",
                columns: table => new
                {
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    MaTacGia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SAN_PHAM__85E3A258F1EC2E3E", x => new { x.MaSanPham, x.MaTacGia });
                    table.ForeignKey(
                        name: "FK__SAN_PHAM___MaSan__656C112C",
                        column: x => x.MaSanPham,
                        principalTable: "SAN_PHAM",
                        principalColumn: "MaSanPham");
                    table.ForeignKey(
                        name: "FK__SAN_PHAM___MaTac__66603565",
                        column: x => x.MaTacGia,
                        principalTable: "TAC_GIA",
                        principalColumn: "MaTacGia");
                });

            migrationBuilder.CreateTable(
                name: "NGUOI_DUNG",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    SoDienThoai = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    MatKhau = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    GioiTinh = table.Column<bool>(type: "bit", nullable: true),
                    NgaySinh = table.Column<DateOnly>(type: "date", nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    MaVaiTro = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    MaXa = table.Column<int>(type: "int", nullable: true),
                    SoNha = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Duong = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NGUOI_DU__C539D7620B43E353", x => x.MaNguoiDung);
                    table.ForeignKey(
                        name: "FK_NGUOI_DUNG_XA_PHUONG",
                        column: x => x.MaXa,
                        principalTable: "XA_PHUONG",
                        principalColumn: "MaXa");
                    table.ForeignKey(
                        name: "FK__NGUOI_DUN__MaVai__5070F446",
                        column: x => x.MaVaiTro,
                        principalTable: "VAI_TRO",
                        principalColumn: "MaVaiTro");
                });

            migrationBuilder.CreateTable(
                name: "DANH_GIA",
                columns: table => new
                {
                    MaDanhGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    SoSao = table.Column<int>(type: "int", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayDanhGia = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DANH_GIA__AA9515BF00FF78FF", x => x.MaDanhGia);
                    table.ForeignKey(
                        name: "FK__DANH_GIA__MaNguo__02FC7413",
                        column: x => x.MaNguoiDung,
                        principalTable: "NGUOI_DUNG",
                        principalColumn: "MaNguoiDung");
                    table.ForeignKey(
                        name: "FK__DANH_GIA__MaSanP__03F0984C",
                        column: x => x.MaSanPham,
                        principalTable: "SAN_PHAM",
                        principalColumn: "MaSanPham");
                });

            migrationBuilder.CreateTable(
                name: "DON_HANG",
                columns: table => new
                {
                    MaDonHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    NgayDat = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    HoTenNguoiNhan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoDienThoaiNhan = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PhiShip = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrangThaiDonHang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MaKhuyenMai = table.Column<int>(type: "int", nullable: true),
                    MaXaGiao = table.Column<int>(type: "int", nullable: true),
                    SoNhaGiao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DuongGiao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DON_HANG__129584ADF0632E74", x => x.MaDonHang);
                    table.ForeignKey(
                        name: "FK_DON_HANG_XA_PHUONG",
                        column: x => x.MaXaGiao,
                        principalTable: "XA_PHUONG",
                        principalColumn: "MaXa");
                    table.ForeignKey(
                        name: "FK__DON_HANG__MaKhuy__75A278F5",
                        column: x => x.MaKhuyenMai,
                        principalTable: "KHUYEN_MAI",
                        principalColumn: "MaKhuyenMai");
                    table.ForeignKey(
                        name: "FK__DON_HANG__MaNguo__72C60C4A",
                        column: x => x.MaNguoiDung,
                        principalTable: "NGUOI_DUNG",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "GIO_HANG",
                columns: table => new
                {
                    MaGioHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GIO_HANG__F5001DA34471A290", x => x.MaGioHang);
                    table.ForeignKey(
                        name: "FK__GIO_HANG__MaNguo__693CA210",
                        column: x => x.MaNguoiDung,
                        principalTable: "NGUOI_DUNG",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "PHIEU_NHAP",
                columns: table => new
                {
                    MaPhieuNhap = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    MaNhaCungCap = table.Column<int>(type: "int", nullable: false),
                    NgayNhap = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PHIEU_NH__1470EF3B0592846B", x => x.MaPhieuNhap);
                    table.ForeignKey(
                        name: "FK__PHIEU_NHA__MaNgu__18EBB532",
                        column: x => x.MaNguoiDung,
                        principalTable: "NGUOI_DUNG",
                        principalColumn: "MaNguoiDung");
                    table.ForeignKey(
                        name: "FK__PHIEU_NHA__MaNha__19DFD96B",
                        column: x => x.MaNhaCungCap,
                        principalTable: "NHA_CUNG_CAP",
                        principalColumn: "MaNhaCungCap");
                });

            migrationBuilder.CreateTable(
                name: "CHI_TIET_DON_HANG",
                columns: table => new
                {
                    MaDonHang = table.Column<int>(type: "int", nullable: false),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CHI_TIET__DD39F0EFE69F097B", x => new { x.MaDonHang, x.MaSanPham });
                    table.ForeignKey(
                        name: "FK__CHI_TIET___MaDon__7A672E12",
                        column: x => x.MaDonHang,
                        principalTable: "DON_HANG",
                        principalColumn: "MaDonHang");
                    table.ForeignKey(
                        name: "FK__CHI_TIET___MaSan__7B5B524B",
                        column: x => x.MaSanPham,
                        principalTable: "SAN_PHAM",
                        principalColumn: "MaSanPham");
                });

            migrationBuilder.CreateTable(
                name: "THANH_TOAN",
                columns: table => new
                {
                    MaThanhToan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDonHang = table.Column<int>(type: "int", nullable: false),
                    PhuongThucThanhToan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SoTienThanhToan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrangThaiThanhToan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaGiaoDich = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    NgayThanhToan = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__THANH_TO__D4B258442397542D", x => x.MaThanhToan);
                    table.ForeignKey(
                        name: "FK__THANH_TOA__MaDon__7F2BE32F",
                        column: x => x.MaDonHang,
                        principalTable: "DON_HANG",
                        principalColumn: "MaDonHang");
                });

            migrationBuilder.CreateTable(
                name: "CHI_TIET_GIO_HANG",
                columns: table => new
                {
                    MaGioHang = table.Column<int>(type: "int", nullable: false),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CHI_TIET__3AAC69E1267D00E0", x => new { x.MaGioHang, x.MaSanPham });
                    table.ForeignKey(
                        name: "FK__CHI_TIET___MaGio__6EF57B66",
                        column: x => x.MaGioHang,
                        principalTable: "GIO_HANG",
                        principalColumn: "MaGioHang");
                    table.ForeignKey(
                        name: "FK__CHI_TIET___MaSan__6FE99F9F",
                        column: x => x.MaSanPham,
                        principalTable: "SAN_PHAM",
                        principalColumn: "MaSanPham");
                });

            migrationBuilder.CreateTable(
                name: "CHI_TIET_PHIEU_NHAP",
                columns: table => new
                {
                    MaPhieuNhap = table.Column<int>(type: "int", nullable: false),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    GiaNhap = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "decimal(29,2)", nullable: true, computedColumnSql: "([SoLuong]*[GiaNhap])", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CHI_TIET__DBDC9B79BAF31BB4", x => new { x.MaPhieuNhap, x.MaSanPham });
                    table.ForeignKey(
                        name: "FK__CHI_TIET___MaPhi__1DB06A4F",
                        column: x => x.MaPhieuNhap,
                        principalTable: "PHIEU_NHAP",
                        principalColumn: "MaPhieuNhap");
                    table.ForeignKey(
                        name: "FK__CHI_TIET___MaSan__1EA48E88",
                        column: x => x.MaSanPham,
                        principalTable: "SAN_PHAM",
                        principalColumn: "MaSanPham");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CHI_TIET_DON_HANG_MaSanPham",
                table: "CHI_TIET_DON_HANG",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_CHI_TIET_GIO_HANG_MaSanPham",
                table: "CHI_TIET_GIO_HANG",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_CHI_TIET_PHIEU_NHAP_MaSanPham",
                table: "CHI_TIET_PHIEU_NHAP",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_DANH_GIA_MaSanPham",
                table: "DANH_GIA",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "UQ_DANH_GIA_NguoiDung_SanPham",
                table: "DANH_GIA",
                columns: new[] { "MaNguoiDung", "MaSanPham" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DANH_MUC_MaDanhMucCha",
                table: "DANH_MUC",
                column: "MaDanhMucCha");

            migrationBuilder.CreateIndex(
                name: "IX_DON_HANG_MaKhuyenMai",
                table: "DON_HANG",
                column: "MaKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_DON_HANG_MaNguoiDung",
                table: "DON_HANG",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DON_HANG_MaXaGiao",
                table: "DON_HANG",
                column: "MaXaGiao");

            migrationBuilder.CreateIndex(
                name: "IX_DON_HANG_NgayDat",
                table: "DON_HANG",
                column: "NgayDat");

            migrationBuilder.CreateIndex(
                name: "UQ_GIO_HANG_Active",
                table: "GIO_HANG",
                column: "MaNguoiDung",
                unique: true,
                filter: "([TrangThai]=(1))");

            migrationBuilder.CreateIndex(
                name: "IX_NGUOI_DUNG_MaVaiTro",
                table: "NGUOI_DUNG",
                column: "MaVaiTro");

            migrationBuilder.CreateIndex(
                name: "IX_NGUOI_DUNG_MaXa",
                table: "NGUOI_DUNG",
                column: "MaXa");

            migrationBuilder.CreateIndex(
                name: "UQ__NGUOI_DU__0389B7BDA6B0A6DB",
                table: "NGUOI_DUNG",
                column: "SoDienThoai",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__NGUOI_DU__A9D10534D8682603",
                table: "NGUOI_DUNG",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PHIEU_NHAP_MaNguoiDung",
                table: "PHIEU_NHAP",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_PHIEU_NHAP_MaNhaCungCap",
                table: "PHIEU_NHAP",
                column: "MaNhaCungCap");

            migrationBuilder.CreateIndex(
                name: "IX_SAN_PHAM_MaDanhMuc",
                table: "SAN_PHAM",
                column: "MaDanhMuc");

            migrationBuilder.CreateIndex(
                name: "IX_SAN_PHAM_MaNhaXuatBan",
                table: "SAN_PHAM",
                column: "MaNhaXuatBan");

            migrationBuilder.CreateIndex(
                name: "IX_SAN_PHAM_TAC_GIA_MaTacGia",
                table: "SAN_PHAM_TAC_GIA",
                column: "MaTacGia");

            migrationBuilder.CreateIndex(
                name: "UQ__THANH_TO__0A2A24EA95D752CF",
                table: "THANH_TOAN",
                column: "MaGiaoDich",
                unique: true,
                filter: "[MaGiaoDich] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ_THANH_TOAN_MaDonHang",
                table: "THANH_TOAN",
                column: "MaDonHang",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__VAI_TRO__1DA558149F0B0052",
                table: "VAI_TRO",
                column: "TenVaiTro",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_XA_PHUONG_MaTinh",
                table: "XA_PHUONG",
                column: "MaTinh");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CHI_TIET_DON_HANG");

            migrationBuilder.DropTable(
                name: "CHI_TIET_GIO_HANG");

            migrationBuilder.DropTable(
                name: "CHI_TIET_PHIEU_NHAP");

            migrationBuilder.DropTable(
                name: "DANH_GIA");

            migrationBuilder.DropTable(
                name: "SAN_PHAM_TAC_GIA");

            migrationBuilder.DropTable(
                name: "THANH_TOAN");

            migrationBuilder.DropTable(
                name: "GIO_HANG");

            migrationBuilder.DropTable(
                name: "PHIEU_NHAP");

            migrationBuilder.DropTable(
                name: "SAN_PHAM");

            migrationBuilder.DropTable(
                name: "TAC_GIA");

            migrationBuilder.DropTable(
                name: "DON_HANG");

            migrationBuilder.DropTable(
                name: "NHA_CUNG_CAP");

            migrationBuilder.DropTable(
                name: "DANH_MUC");

            migrationBuilder.DropTable(
                name: "NHA_XUAT_BAN");

            migrationBuilder.DropTable(
                name: "KHUYEN_MAI");

            migrationBuilder.DropTable(
                name: "NGUOI_DUNG");

            migrationBuilder.DropTable(
                name: "XA_PHUONG");

            migrationBuilder.DropTable(
                name: "VAI_TRO");

            migrationBuilder.DropTable(
                name: "TINH_THANH");
        }
    }
}
