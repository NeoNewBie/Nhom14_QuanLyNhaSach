/* ============================================================
   DATABASE: QuanLyBanSach
   Đề tài: Website quản lý bán sách - ASP.NET Core MVC + EF Core
   Ghi chú: Chạy toàn bộ file này trong SSMS trước khi chạy project.
   ============================================================ */

IF DB_ID(N'QuanLyBanSach') IS NOT NULL
BEGIN
    ALTER DATABASE QuanLyBanSach SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyBanSach;
END
GO

CREATE DATABASE QuanLyBanSach;
GO
USE QuanLyBanSach;
GO

CREATE TABLE VAI_TRO (
    MaVaiTro INT IDENTITY(1,1) PRIMARY KEY,
    TenVaiTro NVARCHAR(50) NOT NULL UNIQUE,
    MoTa NVARCHAR(200) NULL
);
GO

CREATE TABLE NGUOI_DUNG (
    MaNguoiDung INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    SoDienThoai VARCHAR(15) NOT NULL UNIQUE,
    MatKhau VARCHAR(255) NOT NULL,
    DiaChi NVARCHAR(255) NULL,
    GioiTinh BIT NULL,
    NgaySinh DATETIME2 NULL,
    TrangThai BIT NOT NULL DEFAULT 1,
    MaVaiTro INT NOT NULL,
    NgayTao DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_NGUOI_DUNG_VAI_TRO FOREIGN KEY (MaVaiTro) REFERENCES VAI_TRO(MaVaiTro)
);
GO

CREATE TABLE DANH_MUC (
    MaDanhMuc INT IDENTITY(1,1) PRIMARY KEY,
    TenDanhMuc NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(255) NULL,
    TrangThai BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE TAC_GIA (
    MaTacGia INT IDENTITY(1,1) PRIMARY KEY,
    TenTacGia NVARCHAR(150) NOT NULL,
    QuocTich NVARCHAR(50) NULL,
    MoTa NVARCHAR(500) NULL
);
GO

CREATE TABLE NHA_XUAT_BAN (
    MaNhaXuatBan INT IDENTITY(1,1) PRIMARY KEY,
    TenNhaXuatBan NVARCHAR(150) NOT NULL,
    DiaChi NVARCHAR(255) NULL,
    SoDienThoai VARCHAR(15) NULL,
    Email VARCHAR(100) NULL,
    Website VARCHAR(200) NULL
);
GO

CREATE TABLE SAN_PHAM (
    MaSanPham INT IDENTITY(1,1) PRIMARY KEY,
    TenSanPham NVARCHAR(200) NOT NULL,
    MoTa NVARCHAR(MAX) NULL,
    GiaBia DECIMAL(18,2) NOT NULL,
    GiaBan DECIMAL(18,2) NOT NULL,
    GiaNhap DECIMAL(18,2) NOT NULL DEFAULT 0,
    SoLuongTon INT NOT NULL DEFAULT 0 CHECK (SoLuongTon >= 0),
    AnhBia VARCHAR(255) NULL,
    LoaiSanPham NVARCHAR(50) NOT NULL DEFAULT N'Sách giấy',
    MaDanhMuc INT NOT NULL,
    MaTacGia INT NULL,
    MaNhaXuatBan INT NULL,
    TrangThai BIT NOT NULL DEFAULT 1,
    NgayTao DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_SAN_PHAM_DANH_MUC FOREIGN KEY (MaDanhMuc) REFERENCES DANH_MUC(MaDanhMuc),
    CONSTRAINT FK_SAN_PHAM_TAC_GIA FOREIGN KEY (MaTacGia) REFERENCES TAC_GIA(MaTacGia),
    CONSTRAINT FK_SAN_PHAM_NXB FOREIGN KEY (MaNhaXuatBan) REFERENCES NHA_XUAT_BAN(MaNhaXuatBan)
);
GO

CREATE TABLE GIO_HANG (
    MaGioHang INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL,
    NgayTao DATETIME2 NOT NULL DEFAULT GETDATE(),
    TrangThai BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_GIO_HANG_NGUOI_DUNG FOREIGN KEY (MaNguoiDung) REFERENCES NGUOI_DUNG(MaNguoiDung)
);
GO

CREATE TABLE CHI_TIET_GIO_HANG (
    MaGioHang INT NOT NULL,
    MaSanPham INT NOT NULL,
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) NOT NULL,
    CONSTRAINT PK_CHI_TIET_GIO_HANG PRIMARY KEY (MaGioHang, MaSanPham),
    CONSTRAINT FK_CTGH_GIO_HANG FOREIGN KEY (MaGioHang) REFERENCES GIO_HANG(MaGioHang) ON DELETE CASCADE,
    CONSTRAINT FK_CTGH_SAN_PHAM FOREIGN KEY (MaSanPham) REFERENCES SAN_PHAM(MaSanPham)
);
GO

CREATE TABLE DON_HANG (
    MaDonHang INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL,
    NgayDat DATETIME2 NOT NULL DEFAULT GETDATE(),
    HoTenNguoiNhan NVARCHAR(100) NOT NULL,
    SoDienThoaiNhan VARCHAR(15) NOT NULL,
    DiaChiGiaoHang NVARCHAR(255) NOT NULL,
    TongTien DECIMAL(18,2) NOT NULL,
    PhiShip DECIMAL(18,2) NOT NULL DEFAULT 0,
    TrangThaiDonHang NVARCHAR(50) NOT NULL DEFAULT N'Chờ xác nhận',
    GhiChu NVARCHAR(255) NULL,
    CONSTRAINT FK_DON_HANG_NGUOI_DUNG FOREIGN KEY (MaNguoiDung) REFERENCES NGUOI_DUNG(MaNguoiDung),
    CONSTRAINT CK_DON_HANG_TRANG_THAI CHECK (TrangThaiDonHang IN (N'Chờ xác nhận', N'Đã xác nhận', N'Đang giao', N'Đã giao', N'Đã hủy'))
);
GO

CREATE TABLE CHI_TIET_DON_HANG (
    MaDonHang INT NOT NULL,
    MaSanPham INT NOT NULL,
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) NOT NULL,
    ThanhTien DECIMAL(18,2) NOT NULL,
    CONSTRAINT PK_CHI_TIET_DON_HANG PRIMARY KEY (MaDonHang, MaSanPham),
    CONSTRAINT FK_CTDH_DON_HANG FOREIGN KEY (MaDonHang) REFERENCES DON_HANG(MaDonHang) ON DELETE CASCADE,
    CONSTRAINT FK_CTDH_SAN_PHAM FOREIGN KEY (MaSanPham) REFERENCES SAN_PHAM(MaSanPham)
);
GO

CREATE TABLE THANH_TOAN (
    MaThanhToan INT IDENTITY(1,1) PRIMARY KEY,
    MaDonHang INT NOT NULL UNIQUE,
    PhuongThucThanhToan NVARCHAR(50) NOT NULL DEFAULT N'COD',
    SoTienThanhToan DECIMAL(18,2) NOT NULL,
    TrangThaiThanhToan NVARCHAR(50) NOT NULL DEFAULT N'Chưa thanh toán',
    MaGiaoDich VARCHAR(100) NULL,
    NgayThanhToan DATETIME2 NULL,
    CONSTRAINT FK_THANH_TOAN_DON_HANG FOREIGN KEY (MaDonHang) REFERENCES DON_HANG(MaDonHang) ON DELETE CASCADE
);
GO

CREATE TABLE YEU_THICH (
    MaYeuThich INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL,
    MaSanPham INT NOT NULL,
    NgayThem DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_YEU_THICH_NGUOI_DUNG FOREIGN KEY (MaNguoiDung) REFERENCES NGUOI_DUNG(MaNguoiDung),
    CONSTRAINT FK_YEU_THICH_SAN_PHAM FOREIGN KEY (MaSanPham) REFERENCES SAN_PHAM(MaSanPham),
    CONSTRAINT UQ_YEU_THICH UNIQUE (MaNguoiDung, MaSanPham)
);
GO

CREATE TABLE MA_GIAM_GIA (
    MaKhuyenMai INT IDENTITY(1,1) PRIMARY KEY,
    Code VARCHAR(50) NOT NULL UNIQUE,
    MoTa NVARCHAR(255) NULL,
    LoaiGiam VARCHAR(20) NOT NULL DEFAULT 'PhanTram',
    GiaTri DECIMAL(18,2) NOT NULL,
    DonToiThieu DECIMAL(18,2) NOT NULL DEFAULT 0,
    GiamToiDa DECIMAL(18,2) NULL,
    NgayBatDau DATETIME2 NOT NULL DEFAULT GETDATE(),
    NgayKetThuc DATETIME2 NOT NULL,
    SoLuong INT NOT NULL DEFAULT 100,
    DaDung INT NOT NULL DEFAULT 0,
    TrangThai BIT NOT NULL DEFAULT 1,
    CONSTRAINT CK_MA_GIAM_GIA_Loai CHECK (LoaiGiam IN ('PhanTram','SoTien'))
);
GO

CREATE TABLE DANG_KY_NHAN_TIN (
    MaDangKy INT IDENTITY(1,1) PRIMARY KEY,
    Email VARCHAR(100) NOT NULL UNIQUE,
    NgayDangKy DATETIME2 NOT NULL DEFAULT GETDATE(),
    TrangThai BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE LIEN_HE (
    MaLienHe INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    TieuDe NVARCHAR(150) NOT NULL,
    NoiDung NVARCHAR(MAX) NOT NULL,
    NgayGui DATETIME2 NOT NULL DEFAULT GETDATE(),
    TrangThaiXuLy NVARCHAR(50) NOT NULL DEFAULT N'Chưa xử lý'
);
GO

CREATE TABLE DANH_GIA (
    MaDanhGia INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL,
    MaSanPham INT NOT NULL,
    MaDonHang INT NOT NULL,
    SoSao INT NOT NULL CHECK (SoSao BETWEEN 1 AND 5),
    NoiDung NVARCHAR(1000) NULL,
    NgayDanhGia DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_DANH_GIA_NGUOI_DUNG FOREIGN KEY (MaNguoiDung) REFERENCES NGUOI_DUNG(MaNguoiDung),
    CONSTRAINT FK_DANH_GIA_SAN_PHAM FOREIGN KEY (MaSanPham) REFERENCES SAN_PHAM(MaSanPham),
    CONSTRAINT FK_DANH_GIA_DON_HANG FOREIGN KEY (MaDonHang) REFERENCES DON_HANG(MaDonHang),
    CONSTRAINT UQ_DANH_GIA UNIQUE (MaNguoiDung, MaSanPham, MaDonHang)
);
GO

CREATE INDEX IX_SAN_PHAM_TenSanPham ON SAN_PHAM(TenSanPham);
CREATE UNIQUE INDEX UQ_THANH_TOAN_MaGiaoDich_NotNull ON THANH_TOAN(MaGiaoDich) WHERE MaGiaoDich IS NOT NULL;
CREATE INDEX IX_SAN_PHAM_MaDanhMuc ON SAN_PHAM(MaDanhMuc);
CREATE INDEX IX_DON_HANG_MaNguoiDung ON DON_HANG(MaNguoiDung);
CREATE INDEX IX_DON_HANG_NgayDat ON DON_HANG(NgayDat);
CREATE INDEX IX_CT_DON_HANG_MaSanPham ON CHI_TIET_DON_HANG(MaSanPham);
CREATE INDEX IX_DANH_GIA_MaSanPham ON DANH_GIA(MaSanPham);
CREATE INDEX IX_LIEN_HE_TrangThai ON LIEN_HE(TrangThaiXuLy);
GO

INSERT INTO VAI_TRO (TenVaiTro, MoTa) VALUES
(N'User', N'Khách hàng mua sách'),
(N'Admin', N'Quản trị hệ thống');
GO

INSERT INTO NGUOI_DUNG (HoTen, Email, SoDienThoai, MatKhau, DiaChi, GioiTinh, NgaySinh, TrangThai, MaVaiTro) VALUES
(N'Quản trị hệ thống', 'admin@bookport.vn', '0900000001', '123456', N'Đà Nẵng', 1, '1995-01-01', 1, 2),
(N'Nguyễn Văn Khách', 'user@bookport.vn', '0900000002', '123456', N'123 Nguyễn Văn Linh, Đà Nẵng', 1, '2002-05-20', 1, 1),
(N'Trần Minh Anh', 'minhanh@gmail.com', '0900000003', '123456', N'Quận Hải Châu, Đà Nẵng', 0, '2001-08-12', 1, 1);
GO

INSERT INTO DANH_MUC (TenDanhMuc, MoTa) VALUES
(N'Lập trình', N'Sách công nghệ thông tin, ASP.NET Core, SQL Server, thiết kế phần mềm'),
(N'Kinh tế', N'Sách kinh doanh, quản trị, tài chính cá nhân và khởi nghiệp'),
(N'Văn học', N'Tiểu thuyết, truyện ngắn và các tác phẩm văn học nổi bật'),
(N'Tâm lý - Kỹ năng', N'Sách phát triển bản thân, thói quen, tư duy và giao tiếp'),
(N'Manga', N'Truyện tranh Nhật Bản và các bộ manga nổi tiếng'),
(N'Khoa học', N'Sách khoa học phổ thông, vũ trụ, lịch sử và tri thức'),
(N'Thiếu nhi', N'Sách dành cho thiếu nhi, học sinh và độc giả nhỏ tuổi'),
(N'Ngoại văn', N'Sách tiếng Anh, tác phẩm kinh điển và sách học thuật quốc tế');
GO

INSERT INTO TAC_GIA (TenTacGia, QuocTich, MoTa) VALUES
(N'Robert C. Martin', N'Mỹ', N'Tác giả sách Clean Code và các nguyên tắc lập trình sạch'),
(N'Andrew Hunt và David Thomas', N'Mỹ', N'Tác giả The Pragmatic Programmer'),
(N'Nguyễn Nhật Ánh', N'Việt Nam', N'Nhà văn nổi tiếng với nhiều tác phẩm tuổi học trò'),
(N'Haruki Murakami', N'Nhật Bản', N'Tác giả văn học hiện đại Nhật Bản'),
(N'Dale Carnegie', N'Mỹ', N'Tác giả sách kỹ năng giao tiếp kinh điển'),
(N'Eiichiro Oda', N'Nhật Bản', N'Tác giả bộ manga One Piece'),
(N'Martin Fowler', N'Anh', N'Tác giả sách Refactoring'),
(N'Erich Gamma và cộng sự', N'Quốc tế', N'Nhóm tác giả Design Patterns'),
(N'Jon Duckett', N'Anh', N'Tác giả sách thiết kế web HTML và CSS'),
(N'Eric Ries', N'Mỹ', N'Tác giả The Lean Startup'),
(N'Jim Collins', N'Mỹ', N'Tác giả Good to Great'),
(N'Daniel Kahneman', N'Israel/Mỹ', N'Tác giả Thinking, Fast and Slow'),
(N'Robert T. Kiyosaki', N'Mỹ', N'Tác giả Rich Dad Poor Dad'),
(N'James Clear', N'Mỹ', N'Tác giả Atomic Habits'),
(N'Paulo Coelho', N'Brazil', N'Tác giả The Alchemist'),
(N'Antoine de Saint-Exupéry', N'Pháp', N'Tác giả The Little Prince'),
(N'George Orwell', N'Anh', N'Tác giả 1984'),
(N'F. Scott Fitzgerald', N'Mỹ', N'Tác giả The Great Gatsby'),
(N'Stephen Hawking', N'Anh', N'Nhà vật lý lý thuyết và tác giả sách khoa học phổ thông'),
(N'Carl Sagan', N'Mỹ', N'Nhà thiên văn học và tác giả Cosmos'),
(N'Yuval Noah Harari', N'Israel', N'Tác giả Sapiens'),
(N'Masashi Kishimoto', N'Nhật Bản', N'Tác giả Naruto'),
(N'Tsugumi Ohba', N'Nhật Bản', N'Tác giả Death Note'),
(N'Akira Toriyama', N'Nhật Bản', N'Tác giả Dragon Ball'),
(N'J.K. Rowling', N'Anh', N'Tác giả Harry Potter'),
(N'Joseph Albahari', N'Mỹ', N'Tác giả C# in a Nutshell'),
(N'Ben Forta', N'Mỹ', N'Tác giả sách SQL'),
(N'Cal Newport', N'Mỹ', N'Tác giả Deep Work'),
(N'Stephen R. Covey', N'Mỹ', N'Tác giả The 7 Habits of Highly Effective People'),
(N'Simon Sinek', N'Anh/Mỹ', N'Tác giả Start With Why'),
(N'Andrew Lock', N'Anh', N'Tác giả ASP.NET Core in Action'),
(N'Alan Beaulieu', N'Mỹ', N'Tác giả Learning SQL'),
(N'Peter Thiel', N'Mỹ', N'Tác giả Zero to One'),
(N'Carol S. Dweck', N'Mỹ', N'Tác giả Mindset'),
(N'Koyoharu Gotouge', N'Nhật Bản', N'Tác giả Demon Slayer'),
(N'E. B. White', N'Mỹ', N'Tác giả Charlotte''s Web'),
(N'Roald Dahl', N'Anh', N'Tác giả Matilda'),
(N'J. R. R. Tolkien', N'Anh', N'Tác giả The Hobbit'),
(N'Jane Austen', N'Anh', N'Tác giả Pride and Prejudice');
GO

INSERT INTO NHA_XUAT_BAN (TenNhaXuatBan, DiaChi, SoDienThoai, Email, Website) VALUES
(N'NXB Trẻ', N'TP. Hồ Chí Minh', '0280000001', 'contact@nxbtre.vn', 'https://nxbtre.vn'),
(N'NXB Kim Đồng', N'Hà Nội', '0240000001', 'info@kimdong.vn', 'https://nxbkimdong.com.vn'),
(N'Pearson / Prentice Hall', N'United States', '0010000001', 'support@pearson.com', 'https://www.pearson.com'),
(N'O''Reilly Media', N'United States', '0010000002', 'support@oreilly.com', 'https://www.oreilly.com'),
(N'Manning Publications', N'United States', '0010000003', 'support@manning.com', 'https://www.manning.com'),
(N'NXB Lao Động', N'Hà Nội', '0240000002', 'contact@laodong.vn', NULL),
(N'NXB Tổng hợp TP.HCM', N'TP. Hồ Chí Minh', '0280000002', 'info@tonghop.vn', NULL),
(N'HarperCollins', N'United States', '0010000004', 'support@harpercollins.com', 'https://www.harpercollins.com'),
(N'Penguin Random House', N'United States', '0010000005', 'support@penguinrandomhouse.com', 'https://www.penguinrandomhouse.com'),
(N'Bloomsbury', N'United Kingdom', '0440000001', 'support@bloomsbury.com', 'https://www.bloomsbury.com');
GO

INSERT INTO SAN_PHAM (TenSanPham, MoTa, GiaBia, GiaBan, GiaNhap, SoLuongTon, AnhBia, LoaiSanPham, MaDanhMuc, MaTacGia, MaNhaXuatBan, TrangThai, NgayTao) VALUES
(N'Clean Code', N'Sách trình bày các nguyên tắc viết mã sạch, dễ đọc và dễ bảo trì.', 250000, 219000, 150000, 35, 'https://covers.openlibrary.org/b/isbn/9780132350884-L.jpg?default=false', N'Sách giấy', 1, 1, 3, 1, DATEADD(DAY,-4,GETDATE())),
(N'ASP.NET Core in Action', N'Sách hướng dẫn xây dựng ứng dụng web bằng ASP.NET Core, MVC, Razor và các kỹ thuật thực tế.', 230000, 199000, 140000, 30, 'https://covers.openlibrary.org/b/isbn/9781617298301-L.jpg?default=false', N'Sách giấy', 1, 31, 5, 1, DATEADD(DAY,-2,GETDATE())),
(N'Learning SQL', N'Giới thiệu SQL từ cơ bản đến truy vấn nâng cao, phù hợp học cơ sở dữ liệu.', 210000, 185000, 125000, 26, 'https://covers.openlibrary.org/b/isbn/9781492057611-L.jpg?default=false', N'Sách giấy', 1, 32, 4, 1, DATEADD(DAY,-5,GETDATE())),
(N'The Pragmatic Programmer', N'Cuốn sách kinh điển về tư duy nghề nghiệp và phương pháp làm việc của lập trình viên.', 320000, 279000, 190000, 22, 'https://covers.openlibrary.org/b/isbn/9780201616224-L.jpg?default=false', N'Sách giấy', 1, 2, 3, 1, DATEADD(DAY,-3,GETDATE())),
(N'Refactoring', N'Hướng dẫn cải tiến cấu trúc mã nguồn mà không làm thay đổi hành vi bên ngoài của phần mềm.', 340000, 299000, 210000, 18, 'https://covers.openlibrary.org/b/isbn/9780134757599-L.jpg?default=false', N'Sách giấy', 1, 7, 3, 1, DATEADD(DAY,-9,GETDATE())),
(N'Design Patterns', N'Tổng hợp các mẫu thiết kế phần mềm thường dùng trong lập trình hướng đối tượng.', 360000, 319000, 230000, 15, 'https://covers.openlibrary.org/b/isbn/9780201633610-L.jpg?default=false', N'Sách giấy', 1, 8, 3, 1, DATEADD(DAY,-11,GETDATE())),
(N'C# 12 in a Nutshell', N'Tài liệu tham khảo chuyên sâu về ngôn ngữ C# và nền tảng .NET hiện đại.', 420000, 379000, 260000, 12, 'https://covers.openlibrary.org/b/isbn/9781098147440-L.jpg?default=false', N'Sách giấy', 1, 26, 4, 1, DATEADD(DAY,-6,GETDATE())),
(N'HTML and CSS', N'Sách nhập môn thiết kế giao diện web với HTML, CSS, bố cục và hình ảnh trực quan.', 260000, 229000, 160000, 24, 'https://covers.openlibrary.org/b/isbn/9781118008188-L.jpg?default=false', N'Sách giấy', 1, 9, 8, 1, DATEADD(DAY,-13,GETDATE())),
(N'The Lean Startup', N'Phương pháp khởi nghiệp tinh gọn, kiểm chứng ý tưởng nhanh và giảm lãng phí.', 220000, 185000, 120000, 31, 'https://covers.openlibrary.org/b/isbn/9780307887894-L.jpg?default=false', N'Sách giấy', 2, 10, 9, 1, DATEADD(DAY,-10,GETDATE())),
(N'Good to Great', N'Phân tích cách các doanh nghiệp chuyển mình từ tốt đến vĩ đại.', 240000, 205000, 140000, 25, 'https://covers.openlibrary.org/b/isbn/9780066620992-L.jpg?default=false', N'Sách giấy', 2, 11, 8, 1, DATEADD(DAY,-21,GETDATE())),
(N'Tư Duy Nhanh Và Chậm', N'Tác phẩm nổi tiếng về hai hệ thống tư duy và cách con người ra quyết định.', 260000, 225000, 160000, 20, 'https://covers.openlibrary.org/b/isbn/9780374533557-L.jpg?default=false', N'Sách giấy', 2, 12, 9, 1, DATEADD(DAY,-17,GETDATE())),
(N'Cha Giàu Cha Nghèo', N'Sách tài chính cá nhân giúp người đọc thay đổi tư duy về tiền bạc và đầu tư.', 170000, 139000, 85000, 38, 'https://covers.openlibrary.org/b/isbn/9781612680194-L.jpg?default=false', N'Sách giấy', 2, 13, 8, 1, DATEADD(DAY,-14,GETDATE())),
(N'Zero to One', N'Góc nhìn về khởi nghiệp công nghệ, đổi mới sáng tạo và xây dựng sản phẩm khác biệt.', 210000, 179000, 115000, 27, 'https://covers.openlibrary.org/b/isbn/9780804139298-L.jpg?default=false', N'Sách giấy', 2, 33, 9, 1, DATEADD(DAY,-24,GETDATE())),
(N'Norwegian Wood', N'Tiểu thuyết nổi tiếng của Haruki Murakami về tuổi trẻ, tình yêu và ký ức.', 180000, 149000, 95000, 18, 'https://covers.openlibrary.org/b/isbn/9780375704024-L.jpg?default=false', N'Sách giấy', 3, 4, 9, 1, DATEADD(DAY,-7,GETDATE())),
(N'Nhà Giả Kim', N'Câu chuyện biểu tượng về hành trình theo đuổi ước mơ và kho báu của mỗi người.', 160000, 129000, 80000, 45, 'https://covers.openlibrary.org/b/isbn/9780061122415-L.jpg?default=false', N'Sách giấy', 3, 15, 8, 1, DATEADD(DAY,-18,GETDATE())),
(N'Hoàng Tử Bé', N'Tác phẩm kinh điển dành cho mọi lứa tuổi về tình bạn, tình yêu và sự trưởng thành.', 120000, 99000, 60000, 55, 'https://covers.openlibrary.org/b/isbn/9780156012195-L.jpg?default=false', N'Sách giấy', 3, 16, 8, 1, DATEADD(DAY,-20,GETDATE())),
(N'1984', N'Tiểu thuyết phản địa đàng nổi tiếng về quyền lực, kiểm soát và tự do cá nhân.', 170000, 145000, 90000, 34, 'https://covers.openlibrary.org/b/isbn/9780451524935-L.jpg?default=false', N'Sách giấy', 3, 17, 9, 1, DATEADD(DAY,-15,GETDATE())),
(N'Gatsby Vĩ Đại', N'Tác phẩm kinh điển của văn học Mỹ về giấc mơ, tình yêu và sự xa hoa.', 150000, 125000, 78000, 29, 'https://covers.openlibrary.org/b/isbn/9780743273565-L.jpg?default=false', N'Sách giấy', 3, 18, 9, 1, DATEADD(DAY,-12,GETDATE())),
(N'Mắt Biếc', N'Tác phẩm văn học Việt Nam nổi tiếng về tình yêu tuổi học trò và những hoài niệm trong trẻo.', 120000, 99000, 60000, 50, 'https://covers.openlibrary.org/b/isbn/9786041140783-L.jpg?default=false', N'Sách giấy', 3, 3, 1, 1, DATEADD(DAY,-30,GETDATE())),
(N'Đắc Nhân Tâm', N'Sách kỹ năng giao tiếp và ứng xử kinh điển, phù hợp với nhiều thế hệ độc giả.', 130000, 99000, 58000, 60, 'https://covers.openlibrary.org/b/isbn/9780671027032-L.jpg?default=false', N'Sách giấy', 4, 5, 8, 1, DATEADD(DAY,-16,GETDATE())),
(N'7 Thói Quen Hiệu Quả', N'Bộ nguyên tắc phát triển cá nhân, quản lý bản thân và xây dựng hiệu quả lâu dài.', 230000, 199000, 135000, 24, 'https://covers.openlibrary.org/b/isbn/9780743269513-L.jpg?default=false', N'Sách giấy', 4, 29, 8, 1, DATEADD(DAY,-19,GETDATE())),
(N'Deep Work', N'Hướng dẫn rèn luyện khả năng tập trung sâu trong học tập và công việc hiện đại.', 210000, 179000, 115000, 26, 'https://covers.openlibrary.org/b/isbn/9781455586691-L.jpg?default=false', N'Sách giấy', 4, 28, 8, 1, DATEADD(DAY,-8,GETDATE())),
(N'Start With Why', N'Giúp người đọc hiểu cách truyền cảm hứng và xây dựng mục tiêu bắt đầu từ câu hỏi vì sao.', 220000, 189000, 120000, 23, 'https://covers.openlibrary.org/b/isbn/9781591846444-L.jpg?default=false', N'Sách giấy', 4, 30, 8, 1, DATEADD(DAY,-22,GETDATE())),
(N'Mindset', N'Cuốn sách về tư duy phát triển và cách thay đổi thái độ học tập, làm việc.', 210000, 175000, 110000, 28, 'https://covers.openlibrary.org/b/isbn/9780345472328-L.jpg?default=false', N'Sách giấy', 4, 34, 9, 1, DATEADD(DAY,-26,GETDATE())),
(N'Atomic Habits', N'Phương pháp xây dựng thói quen nhỏ để tạo nên thay đổi lớn trong cuộc sống.', 240000, 209000, 145000, 36, 'https://covers.openlibrary.org/b/isbn/9780735211292-L.jpg?default=false', N'Sách giấy', 4, 14, 9, 1, DATEADD(DAY,-1,GETDATE())),
(N'One Piece Tập 1', N'Manga phiêu lưu nổi tiếng về hành trình trở thành Vua Hải Tặc.', 35000, 30000, 18000, 90, 'https://covers.openlibrary.org/b/isbn/9781569319017-L.jpg?default=false', N'Manga', 5, 6, 2, 1, DATEADD(DAY,-1,GETDATE())),
(N'Naruto Tập 1', N'Câu chuyện về ninja trẻ tuổi Naruto và hành trình khẳng định bản thân.', 35000, 30000, 18000, 82, 'https://covers.openlibrary.org/b/isbn/9781569319000-L.jpg?default=false', N'Manga', 5, 22, 2, 1, DATEADD(DAY,-4,GETDATE())),
(N'Death Note Tập 1', N'Manga trinh thám, tâm lý xoay quanh cuốn sổ tử thần và cuộc đấu trí căng thẳng.', 45000, 39000, 23000, 65, 'https://covers.openlibrary.org/b/isbn/9781421501680-L.jpg?default=false', N'Manga', 5, 23, 2, 1, DATEADD(DAY,-6,GETDATE())),
(N'Dragon Ball Tập 1', N'Manga hành động, hài hước nổi tiếng về Songoku và hành trình tìm ngọc rồng.', 40000, 35000, 21000, 70, 'https://covers.openlibrary.org/b/isbn/9781569319208-L.jpg?default=false', N'Manga', 5, 24, 2, 1, DATEADD(DAY,-9,GETDATE())),
(N'Demon Slayer Tập 1', N'Manga hành động giả tưởng về Tanjiro và cuộc chiến chống quỷ.', 50000, 45000, 28000, 58, 'https://covers.openlibrary.org/b/isbn/9781974700523-L.jpg?default=false', N'Manga', 5, 35, 2, 1, DATEADD(DAY,-11,GETDATE())),
(N'Lược Sử Thời Gian', N'Tác phẩm khoa học phổ thông nổi tiếng trình bày các khái niệm vũ trụ học.', 220000, 189000, 120000, 17, 'https://covers.openlibrary.org/b/isbn/9780553380163-L.jpg?default=false', N'Sách giấy', 6, 19, 8, 1, DATEADD(DAY,-9,GETDATE())),
(N'Vũ Trụ Trong Vỏ Hạt Dẻ', N'Sách khoa học phổ thông về vũ trụ, không gian, thời gian và các lý thuyết vật lý hiện đại.', 230000, 199000, 130000, 14, 'https://covers.openlibrary.org/b/isbn/9780553802023-L.jpg?default=false', N'Sách giấy', 6, 19, 8, 1, DATEADD(DAY,-6,GETDATE())),
(N'Cosmos', N'Hành trình khám phá vũ trụ, khoa học và vị trí của con người trong không gian bao la.', 260000, 229000, 165000, 19, 'https://covers.openlibrary.org/b/isbn/9780345539434-L.jpg?default=false', N'Sách giấy', 6, 20, 9, 1, DATEADD(DAY,-12,GETDATE())),
(N'Sapiens: Lược Sử Loài Người', N'Cuốn sách tổng hợp lịch sử phát triển của loài người từ cổ đại đến hiện đại.', 300000, 259000, 190000, 21, 'https://covers.openlibrary.org/b/isbn/9780062316097-L.jpg?default=false', N'Sách giấy', 6, 21, 8, 1, DATEADD(DAY,-14,GETDATE())),
(N'Harry Potter và Hòn Đá Phù Thủy', N'Tập đầu tiên trong loạt truyện thiếu nhi giả tưởng nổi tiếng về thế giới phù thủy.', 250000, 219000, 150000, 32, 'https://covers.openlibrary.org/b/isbn/9780747532699-L.jpg?default=false', N'Sách giấy', 7, 25, 10, 1, DATEADD(DAY,-5,GETDATE())),
(N'Charlotte''s Web', N'Câu chuyện thiếu nhi cảm động về tình bạn giữa cô bé, chú lợn Wilbur và nhện Charlotte.', 140000, 119000, 75000, 27, 'https://covers.openlibrary.org/b/isbn/9780064400558-L.jpg?default=false', N'Sách giấy', 7, 36, 8, 1, DATEADD(DAY,-18,GETDATE())),
(N'Matilda', N'Truyện thiếu nhi hài hước về cô bé thông minh Matilda và tình yêu với sách.', 150000, 129000, 80000, 25, 'https://covers.openlibrary.org/b/isbn/9780142410370-L.jpg?default=false', N'Sách giấy', 7, 37, 9, 1, DATEADD(DAY,-23,GETDATE())),
(N'The Hobbit', N'Tác phẩm phiêu lưu giả tưởng kinh điển, mở đầu cho thế giới Trung Địa.', 240000, 199000, 135000, 18, 'https://covers.openlibrary.org/b/isbn/9780547928227-L.jpg?default=false', N'Sách giấy', 8, 38, 8, 1, DATEADD(DAY,-27,GETDATE())),
(N'Pride and Prejudice', N'Tiểu thuyết kinh điển tiếng Anh về tình yêu, định kiến và xã hội Anh thế kỷ XIX.', 160000, 135000, 85000, 22, 'https://covers.openlibrary.org/b/isbn/9780141439518-L.jpg?default=false', N'Sách giấy', 8, 39, 9, 1, DATEADD(DAY,-28,GETDATE()));
GO

INSERT INTO GIO_HANG (MaNguoiDung, TrangThai) VALUES (2, 1), (3, 1);
GO

INSERT INTO CHI_TIET_GIO_HANG (MaGioHang, MaSanPham, SoLuong, DonGia) VALUES
(1, 2, 1, 199000),
(1, 21, 1, 99000),
(2, 15, 1, 129000);
GO

INSERT INTO YEU_THICH (MaNguoiDung, MaSanPham) VALUES
(2, 1), (2, 15), (2, 27), (3, 21), (3, 32);
GO

INSERT INTO MA_GIAM_GIA (Code, MoTa, LoaiGiam, GiaTri, DonToiThieu, GiamToiDa, NgayBatDau, NgayKetThuc, SoLuong, DaDung, TrangThai) VALUES
('BOOK10', N'Giảm 10% cho đơn từ 100.000 đ', 'PhanTram', 10, 100000, 50000, DATEADD(DAY,-10,GETDATE()), DATEADD(DAY,60,GETDATE()), 100, 0, 1),
('STUDENT20', N'Giảm 20.000 đ cho sinh viên', 'SoTien', 20000, 150000, NULL, DATEADD(DAY,-10,GETDATE()), DATEADD(DAY,60,GETDATE()), 80, 0, 1),
('FREESHIP', N'Ưu đãi vận chuyển: giảm 15.000 đ', 'SoTien', 15000, 50000, NULL, DATEADD(DAY,-10,GETDATE()), DATEADD(DAY,60,GETDATE()), 120, 0, 1);
GO

INSERT INTO DANG_KY_NHAN_TIN (Email) VALUES
('reader1@example.com'), ('reader2@example.com');
GO

INSERT INTO LIEN_HE (HoTen, Email, TieuDe, NoiDung, TrangThaiXuLy) VALUES
(N'Nguyễn Văn Khách', 'user@bookport.vn', N'Hỏi về đơn hàng', N'Tôi muốn kiểm tra thời gian giao sách.', N'Chưa xử lý'),
(N'Trần Minh Anh', 'minhanh@gmail.com', N'Góp ý website', N'Giao diện dễ dùng, cần thêm nhiều manga hơn.', N'Đã xử lý');
GO

INSERT INTO DON_HANG (MaNguoiDung, NgayDat, HoTenNguoiNhan, SoDienThoaiNhan, DiaChiGiaoHang, TongTien, PhiShip, TrangThaiDonHang, GhiChu) VALUES
(2, DATEADD(DAY,-10,GETDATE()), N'Nguyễn Văn Khách', '0900000002', N'123 Nguyễn Văn Linh, Đà Nẵng', 318000, 0, N'Đã giao', N'Giao giờ hành chính'),
(2, DATEADD(DAY,-3,GETDATE()), N'Nguyễn Văn Khách', '0900000002', N'123 Nguyễn Văn Linh, Đà Nẵng', 185000, 0, N'Đang giao', NULL),
(3, DATEADD(DAY,-1,GETDATE()), N'Trần Minh Anh', '0900000003', N'Quận Hải Châu, Đà Nẵng', 60000, 0, N'Chờ xác nhận', NULL);
GO

INSERT INTO CHI_TIET_DON_HANG (MaDonHang, MaSanPham, SoLuong, DonGia, ThanhTien) VALUES
(1, 1, 1, 219000, 219000),
(1, 21, 1, 99000, 99000),
(2, 9, 1, 185000, 185000),
(3, 27, 2, 30000, 60000);
GO

INSERT INTO DANH_GIA (MaNguoiDung, MaSanPham, MaDonHang, SoSao, NoiDung, NgayDanhGia) VALUES
(2, 1, 1, 5, N'Sách trình bày rõ ràng, phù hợp để học cách viết code sạch.', DATEADD(DAY,-7,GETDATE())),
(2, 21, 1, 4, N'Sách đẹp, giao nhanh, đóng gói tốt.', DATEADD(DAY,-7,GETDATE()));
GO

INSERT INTO THANH_TOAN (MaDonHang, PhuongThucThanhToan, SoTienThanhToan, TrangThaiThanhToan, NgayThanhToan) VALUES
(1, N'COD', 318000, N'Đã thanh toán', DATEADD(DAY,-8,GETDATE())),
(2, N'COD', 185000, N'Chưa thanh toán', NULL),
(3, N'COD', 60000, N'Chưa thanh toán', NULL);
GO

PRINT N'Tạo database QuanLyBanSach thành công.';
PRINT N'Tài khoản Admin: admin@bookport.vn / 123456';
PRINT N'Tài khoản User: user@bookport.vn / 123456';
GO
