/* =========================================================
   DATABASE: QuanLyBanSach
   Dùng cho project ASP.NET Core MVC + EF Core của nhóm.
   Chạy toàn bộ file này trong SSMS trước khi chạy project.
   File sẽ xóa database cũ để tránh lỗi thiếu bảng/cột.
   ========================================================= */
USE master;
GO

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

CREATE TABLE TINH_THANH (
    MaTinh INT IDENTITY(1,1) PRIMARY KEY,
    TenTinh NVARCHAR(100) NOT NULL,
    TrangThai BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE XA_PHUONG (
    MaXa INT IDENTITY(1,1) PRIMARY KEY,
    TenXa NVARCHAR(100) NOT NULL,
    MaTinh INT NOT NULL FOREIGN KEY REFERENCES TINH_THANH(MaTinh),
    TrangThai BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE NGUOI_DUNG (
    MaNguoiDung INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    SoDienThoai VARCHAR(15) NOT NULL UNIQUE,
    MatKhau VARCHAR(255) NOT NULL,
    GioiTinh BIT NULL,
    NgaySinh DATE NULL,
    TrangThai BIT NOT NULL DEFAULT 1,
    MaVaiTro INT NOT NULL FOREIGN KEY REFERENCES VAI_TRO(MaVaiTro),
    NgayTao DATETIME2 NOT NULL DEFAULT GETDATE(),
    MaXa INT NULL FOREIGN KEY REFERENCES XA_PHUONG(MaXa),
    SoNha NVARCHAR(50) NULL,
    Duong NVARCHAR(100) NULL
);
GO

CREATE TABLE DANH_MUC (
    MaDanhMuc INT IDENTITY(1,1) PRIMARY KEY,
    TenDanhMuc NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(255) NULL,
    MaDanhMucCha INT NULL FOREIGN KEY REFERENCES DANH_MUC(MaDanhMuc),
    TrangThai BIT NOT NULL DEFAULT 1
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

CREATE TABLE TAC_GIA (
    MaTacGia INT IDENTITY(1,1) PRIMARY KEY,
    TenTacGia NVARCHAR(150) NOT NULL,
    QuocTich NVARCHAR(50) NULL,
    MoTa NVARCHAR(255) NULL
);
GO

CREATE TABLE KHUYEN_MAI (
    MaKhuyenMai INT IDENTITY(1,1) PRIMARY KEY,
    TenKhuyenMai NVARCHAR(150) NOT NULL,
    LoaiGiamGia NVARCHAR(50) NOT NULL,
    GiaTriGiam DECIMAL(18,2) NOT NULL,
    DieuKienApDung DECIMAL(18,2) NULL,
    NgayBatDau DATETIME2 NOT NULL,
    NgayKetThuc DATETIME2 NOT NULL,
    TrangThai BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE SAN_PHAM (
    MaSanPham INT IDENTITY(1,1) PRIMARY KEY,
    TenSanPham NVARCHAR(200) NOT NULL,
    MoTa NVARCHAR(MAX) NULL,
    GiaBia DECIMAL(18,2) NOT NULL,
    GiaBan DECIMAL(18,2) NOT NULL,
    SoLuongTon INT NOT NULL DEFAULT 0,
    AnhBia VARCHAR(255) NULL,
    LoaiSanPham NVARCHAR(50) NOT NULL,
    MaDanhMuc INT NOT NULL FOREIGN KEY REFERENCES DANH_MUC(MaDanhMuc),
    MaNhaXuatBan INT NULL FOREIGN KEY REFERENCES NHA_XUAT_BAN(MaNhaXuatBan),
    TrangThai BIT NOT NULL DEFAULT 1,
    NgayTao DATETIME2 NOT NULL DEFAULT GETDATE(),
    GiaNhap DECIMAL(18,2) NOT NULL DEFAULT 0
);
GO

CREATE TABLE SAN_PHAM_TAC_GIA (
    MaSanPham INT NOT NULL FOREIGN KEY REFERENCES SAN_PHAM(MaSanPham),
    MaTacGia INT NOT NULL FOREIGN KEY REFERENCES TAC_GIA(MaTacGia),
    PRIMARY KEY (MaSanPham, MaTacGia)
);
GO

CREATE TABLE GIO_HANG (
    MaGioHang INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL FOREIGN KEY REFERENCES NGUOI_DUNG(MaNguoiDung),
    NgayTao DATETIME2 NOT NULL DEFAULT GETDATE(),
    TrangThai BIT NOT NULL DEFAULT 1
);
GO
CREATE UNIQUE INDEX UQ_GIO_HANG_Active ON GIO_HANG(MaNguoiDung) WHERE TrangThai = 1;
GO

CREATE TABLE CHI_TIET_GIO_HANG (
    MaGioHang INT NOT NULL FOREIGN KEY REFERENCES GIO_HANG(MaGioHang),
    MaSanPham INT NOT NULL FOREIGN KEY REFERENCES SAN_PHAM(MaSanPham),
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) NOT NULL,
    PRIMARY KEY (MaGioHang, MaSanPham)
);
GO

CREATE TABLE DON_HANG (
    MaDonHang INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL FOREIGN KEY REFERENCES NGUOI_DUNG(MaNguoiDung),
    NgayDat DATETIME2 NOT NULL DEFAULT GETDATE(),
    HoTenNguoiNhan NVARCHAR(100) NOT NULL,
    SoDienThoaiNhan VARCHAR(15) NOT NULL,
    TongTien DECIMAL(18,2) NOT NULL,
    PhiShip DECIMAL(18,2) NOT NULL DEFAULT 0,
    TrangThaiDonHang NVARCHAR(50) NOT NULL,
    GhiChu NVARCHAR(255) NULL,
    MaKhuyenMai INT NULL FOREIGN KEY REFERENCES KHUYEN_MAI(MaKhuyenMai),
    MaXaGiao INT NULL FOREIGN KEY REFERENCES XA_PHUONG(MaXa),
    SoNhaGiao NVARCHAR(50) NULL,
    DuongGiao NVARCHAR(100) NULL,
    CONSTRAINT CK_DON_HANG_TrangThai CHECK (TrangThaiDonHang IN (N'Chờ xác nhận', N'Đã xác nhận', N'Đang giao', N'Đã giao', N'Đã hủy'))
);
GO

CREATE TABLE CHI_TIET_DON_HANG (
    MaDonHang INT NOT NULL FOREIGN KEY REFERENCES DON_HANG(MaDonHang),
    MaSanPham INT NOT NULL FOREIGN KEY REFERENCES SAN_PHAM(MaSanPham),
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) NOT NULL,
    ThanhTien DECIMAL(18,2) NOT NULL,
    PRIMARY KEY (MaDonHang, MaSanPham)
);
GO

CREATE TABLE THANH_TOAN (
    MaThanhToan INT IDENTITY(1,1) PRIMARY KEY,
    MaDonHang INT NOT NULL UNIQUE FOREIGN KEY REFERENCES DON_HANG(MaDonHang),
    PhuongThucThanhToan NVARCHAR(50) NOT NULL,
    SoTienThanhToan DECIMAL(18,2) NOT NULL,
    TrangThaiThanhToan NVARCHAR(50) NOT NULL,
    MaGiaoDich VARCHAR(100) NULL,
    NgayThanhToan DATETIME2 NULL
);
GO
CREATE UNIQUE INDEX UQ_THANH_TOAN_MaGiaoDich_NotNull ON THANH_TOAN(MaGiaoDich) WHERE MaGiaoDich IS NOT NULL;
GO

CREATE TABLE DANH_GIA (
    MaDanhGia INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL FOREIGN KEY REFERENCES NGUOI_DUNG(MaNguoiDung),
    MaSanPham INT NOT NULL FOREIGN KEY REFERENCES SAN_PHAM(MaSanPham),
    MaDonHang INT NULL FOREIGN KEY REFERENCES DON_HANG(MaDonHang),
    SoSao INT NOT NULL CHECK (SoSao BETWEEN 1 AND 5),
    NoiDung NVARCHAR(MAX) NULL,
    NgayDanhGia DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_DANH_GIA_NguoiDung_SanPham UNIQUE(MaNguoiDung, MaSanPham)
);
GO

CREATE TABLE NHA_CUNG_CAP (
    MaNhaCungCap INT IDENTITY(1,1) PRIMARY KEY,
    TenNhaCungCap NVARCHAR(150) NOT NULL,
    DiaChi NVARCHAR(255) NULL,
    SoDienThoai VARCHAR(15) NULL,
    Email VARCHAR(100) NULL,
    NguoiLienHe NVARCHAR(100) NULL,
    TrangThai BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE PHIEU_NHAP (
    MaPhieuNhap INT IDENTITY(1,1) PRIMARY KEY,
    MaNhaCungCap INT NOT NULL FOREIGN KEY REFERENCES NHA_CUNG_CAP(MaNhaCungCap),
    MaNguoiDung INT NOT NULL FOREIGN KEY REFERENCES NGUOI_DUNG(MaNguoiDung),
    NgayNhap DATETIME2 NOT NULL DEFAULT GETDATE(),
    TongTien DECIMAL(18,2) NOT NULL,
    GhiChu NVARCHAR(255) NULL
);
GO

CREATE TABLE CHI_TIET_PHIEU_NHAP (
    MaPhieuNhap INT NOT NULL FOREIGN KEY REFERENCES PHIEU_NHAP(MaPhieuNhap),
    MaSanPham INT NOT NULL FOREIGN KEY REFERENCES SAN_PHAM(MaSanPham),
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    GiaNhap DECIMAL(18,2) NOT NULL,
    ThanhTien AS (SoLuong * GiaNhap) PERSISTED,
    PRIMARY KEY (MaPhieuNhap, MaSanPham)
);
GO

CREATE INDEX IX_SAN_PHAM_MaDanhMuc ON SAN_PHAM(MaDanhMuc);
CREATE INDEX IX_SAN_PHAM_MaNhaXuatBan ON SAN_PHAM(MaNhaXuatBan);
CREATE INDEX IX_DON_HANG_MaNguoiDung ON DON_HANG(MaNguoiDung);
CREATE INDEX IX_DON_HANG_NgayDat ON DON_HANG(NgayDat);
CREATE INDEX IX_DON_HANG_MaXaGiao ON DON_HANG(MaXaGiao);
CREATE INDEX IX_NGUOI_DUNG_MaXa ON NGUOI_DUNG(MaXa);
CREATE INDEX IX_DANH_GIA_MaSanPham ON DANH_GIA(MaSanPham);
CREATE INDEX IX_CHI_TIET_DON_HANG_MaSanPham ON CHI_TIET_DON_HANG(MaSanPham);
CREATE INDEX IX_CHI_TIET_GIO_HANG_MaSanPham ON CHI_TIET_GIO_HANG(MaSanPham);
GO

CREATE VIEW V_NGUOI_DUNG_DIA_CHI AS
SELECT nd.MaNguoiDung, nd.HoTen, nd.Email, nd.SoDienThoai, nd.SoNha, nd.Duong, x.TenXa, t.TenTinh,
       CONCAT_WS(N', ', nd.SoNha, nd.Duong, x.TenXa, t.TenTinh) AS DiaChiDayDu
FROM NGUOI_DUNG nd
LEFT JOIN XA_PHUONG x ON nd.MaXa = x.MaXa
LEFT JOIN TINH_THANH t ON x.MaTinh = t.MaTinh;
GO

CREATE VIEW V_DON_HANG_DIA_CHI AS
SELECT dh.MaDonHang, dh.MaNguoiDung, dh.SoNhaGiao, dh.DuongGiao, x.TenXa, t.TenTinh,
       CONCAT_WS(N', ', dh.SoNhaGiao, dh.DuongGiao, x.TenXa, t.TenTinh) AS DiaChiGiaoDayDu
FROM DON_HANG dh
LEFT JOIN XA_PHUONG x ON dh.MaXaGiao = x.MaXa
LEFT JOIN TINH_THANH t ON x.MaTinh = t.MaTinh;
GO

INSERT INTO VAI_TRO (TenVaiTro, MoTa) VALUES
(N'User', N'Người dùng thông thường'),
(N'Admin', N'Quản trị viên hệ thống'),
(N'Staff', N'Nhân viên bán hàng');

INSERT INTO TINH_THANH (TenTinh) VALUES (N'Hà Nội'), (N'TP. Hồ Chí Minh'), (N'Đà Nẵng'), (N'Cần Thơ');
INSERT INTO XA_PHUONG (TenXa, MaTinh) VALUES
(N'Phường Trung Hòa', 1), (N'Phường Nhân Chính', 1), (N'Xã Đông Anh', 1),
(N'Phường Bến Nghé', 2), (N'Phường Thảo Điền', 2), (N'Xã Bình Hưng', 2),
(N'Phường Hòa Cường Bắc', 3), (N'Phường An Hải Bắc', 3);

INSERT INTO NGUOI_DUNG (HoTen, Email, SoDienThoai, MatKhau, GioiTinh, NgaySinh, TrangThai, MaVaiTro, NgayTao, MaXa, SoNha, Duong) VALUES
(N'Nguyễn Văn Admin', 'admin@quanlynhasach.vn', '0987654321', 'Admin@123', 1, '1990-01-15', 1, 2, GETDATE(), 1, N'10', N'Đường Nguyễn Phong Sắc'),
(N'Trần Thị Nhân Viên', 'staff@quanlynhasach.vn', '0976543210', 'Staff@123', 0, '1995-05-20', 1, 3, GETDATE(), NULL, NULL, NULL),
(N'Phạm Văn Khách', 'customer1@gmail.com', '0912345678', 'Customer@123', 1, '1998-03-10', 1, 1, GETDATE(), 4, N'25', N'Đường Nguyễn Huệ'),
(N'Võ Thị Mua Sách', 'customer2@gmail.com', '0901234567', 'Customer@456', 0, '2000-07-25', 1, 1, GETDATE(), 7, N'45', N'Đường Thái Phiên');

INSERT INTO DANH_MUC (TenDanhMuc, MoTa, TrangThai) VALUES
(N'Văn học', N'Sách văn học Việt Nam và quốc tế', 1),
(N'Khoa học kỹ thuật', N'Sách khoa học, công nghệ, lập trình', 1),
(N'Kinh tế - Quản lý', N'Sách kinh doanh, quản lý doanh nghiệp', 1),
(N'Tâm lý - Kỹ năng sống', N'Sách phát triển bản thân, tâm lý', 1),
(N'Lịch sử - Địa lý', N'Sách lịch sử, địa lý, văn hóa', 1);

INSERT INTO NHA_XUAT_BAN (TenNhaXuatBan, DiaChi, SoDienThoai, Email, Website) VALUES
(N'NXB Trẻ', N'100 Thụy Khuê, Hà Nội', '0243943203', 'info@nxbtre.com.vn', 'https://www.nxbtre.com.vn'),
(N'NXB Kim Đồng', N'365 Cộng Hòa, TP. Hồ Chí Minh', '0283932632', 'info@nxbkimdong.com.vn', 'https://www.nxbkimdong.com.vn'),
(N'NXB Hội Nhà Văn', N'Hưng Phúc 1, Hà Nội', '0243942206', 'info@nxbhoinhavan.com.vn', 'https://www.nxbhoinhavan.com.vn'),
(N'NXB Lao Động', N'27 Trần Hưng Đạo, Hà Nội', '0243933203', 'info@nxblaodong.com.vn', 'https://www.nxblaodong.com.vn');

INSERT INTO TAC_GIA (TenTacGia, QuocTich, MoTa) VALUES
(N'Nguyễn Du', N'Việt Nam', N'Tác giả kiệt xuất của Truyện Kiều'),
(N'Tô Hoài', N'Việt Nam', N'Tác giả Dế Mèn Phiêu Lưu Ký'),
(N'Trần Hữu Tước', N'Việt Nam', N'Nhà văn hiện đại'),
(N'Haruki Murakami', N'Nhật Bản', N'Tác giả nổi tiếng thế giới'),
(N'George Orwell', N'Anh', N'Tác giả 1984'),
(N'J.K. Rowling', N'Anh', N'Tác giả Harry Potter'),
(N'Stephen Covey', N'Mỹ', N'Tác giả sách phát triển bản thân');

INSERT INTO SAN_PHAM (TenSanPham, MoTa, GiaBia, GiaBan, GiaNhap, SoLuongTon, AnhBia, LoaiSanPham, MaDanhMuc, MaNhaXuatBan, TrangThai, NgayTao) VALUES
(N'Truyện Kiều', N'Tác phẩm kinh điển của Nguyễn Du, một trong những tác phẩm văn học vĩ đại nhất của dân tộc Việt Nam', 85000, 72250, 60000, 50, 'https://books.google.com/books/content?id=95isiQ3ZhN4C&printsec=frontcover&img=1&zoom=1&source=gbs_api', N'Sách', 1, 3, 1, DATEADD(DAY,-60,GETDATE())),
(N'Dế Mèn Phiêu Lưu Ký', N'Cuộc phiêu lưu kỳ thú của chú dế mèn qua thế giới tuổi thơ', 95000, 80750, 70000, 35, 'https://books.google.com/books/content?id=Jces0AEACAAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api', N'Sách', 1, 2, 1, DATEADD(DAY,-30,GETDATE())),
(N'Lập trình C# cơ bản đến nâng cao', N'Hướng dẫn lập trình C# từ cơ bản đến nâng cao', 320000, 272000, 240000, 25, '/images/books/csharp_guide.jpg', N'Sách', 2, 1, 1, DATEADD(DAY,-90,GETDATE())),
(N'Data Science với Python', N'Tìm hiểu data science, machine learning sử dụng Python', 385000, 326750, 300000, 20, '/images/books/datascience_python.jpg', N'Sách', 2, 1, 1, DATEADD(DAY,-60,GETDATE())),
(N'Những nguyên tắc để thành công', N'Các nguyên tắc để đạt được thành công trong kinh doanh và cuộc sống', 198000, 168300, 150000, 45, '/images/books/7_principles_success.jpg', N'Sách', 3, 4, 1, GETDATE()),
(N'Thói quen nguyên tử', N'Cách thay đổi thói quen nhỏ để tạo kết quả lớn', 180000, 153000, 140000, 60, 'https://books.google.com/books/content?id=lFhbDwAAQBAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api', N'Sách', 4, 3, 1, DATEADD(DAY,-30,GETDATE())),
(N'1984', N'Tiểu thuyết dystopian kinh điển của George Orwell', 175000, 148750, 130000, 30, 'https://books.google.com/books/content?id=kotPYEqx7kMC&printsec=frontcover&img=1&zoom=1&source=gbs_api', N'Sách', 1, 2, 1, DATEADD(DAY,-120,GETDATE())),
(N'Lịch sử Việt Nam', N'Tổng quan về lịch sử phát triển của dân tộc Việt Nam', 280000, 238000, 210000, 40, '/images/books/lich_su_vn.jpg', N'Sách', 5, 4, 1, DATEADD(DAY,-60,GETDATE())),
(N'Norwegian Wood', N'Tiểu thuyết tình yêu nổi tiếng của Haruki Murakami', 210000, 178000, 145000, 38, 'https://books.google.com/books/content?id=M37o0AEACAAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api', N'Sách', 1, 3, 1, DATEADD(DAY,-18,GETDATE())),
(N'Harry Potter và Hòn Đá Phù Thủy', N'Tác phẩm fantasy được yêu thích trên toàn thế giới', 250000, 219000, 170000, 44, 'https://books.google.com/books/content?id=wrOQLV6xB-wC&printsec=frontcover&img=1&zoom=1&source=gbs_api', N'Sách', 1, 2, 1, DATEADD(DAY,-10,GETDATE())),
(N'Clean Code C#', N'Kỹ thuật viết mã sạch, dễ bảo trì trong C#', 360000, 306000, 250000, 22, 'https://books.google.com/books/content?id=hjEFCAAAQBAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api', N'Sách', 2, 1, 1, DATEADD(DAY,-8,GETDATE())),
(N'ASP.NET Core MVC thực chiến', N'Xây dựng website bán hàng bằng ASP.NET Core MVC và SQL Server', 390000, 331500, 280000, 28, '/images/books/aspnet_core_mvc.jpg', N'Sách', 2, 1, 1, DATEADD(DAY,-5,GETDATE())),
(N'Trí tuệ nhân tạo nhập môn', N'Nền tảng về AI, machine learning và ứng dụng thực tế', 295000, 250000, 210000, 31, '/images/books/ai_nhap_mon.jpg', N'Sách', 2, 1, 1, DATEADD(DAY,-14,GETDATE())),
(N'Tư duy nhanh và chậm', N'Sách tâm lý học về cách con người ra quyết định', 230000, 195000, 160000, 37, 'https://books.google.com/books/content?id=XQuhEAAAQBAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api', N'Sách', 4, 4, 1, DATEADD(DAY,-21,GETDATE())),
(N'Đắc nhân tâm', N'Nghệ thuật giao tiếp và xây dựng quan hệ bền vững', 160000, 136000, 105000, 55, 'https://books.google.com/books/content?id=eqjvDwAAQBAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api', N'Sách', 4, 4, 1, DATEADD(DAY,-7,GETDATE())),
(N'Nhà giả kim', N'Câu chuyện truyền cảm hứng về hành trình đi tìm ước mơ', 145000, 123000, 98000, 48, 'https://books.google.com/books/content?id=1onn0AEACAAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api', N'Sách', 1, 3, 1, DATEADD(DAY,-16,GETDATE())),
(N'Khởi nghiệp tinh gọn', N'Phương pháp xây dựng sản phẩm và kiểm chứng mô hình kinh doanh', 260000, 221000, 180000, 30, 'https://books.google.com/books/content?id=-KsjEQAAQBAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api', N'Sách', 3, 4, 1, DATEADD(DAY,-12,GETDATE())),
(N'Quản trị học căn bản', N'Kiến thức nền tảng về quản trị doanh nghiệp', 240000, 204000, 170000, 34, '/images/books/quan_tri_hoc.jpg', N'Sách', 3, 4, 1, DATEADD(DAY,-25,GETDATE())),
(N'Marketing 5.0', N'Marketing trong thời đại công nghệ và dữ liệu', 280000, 238000, 195000, 26, 'https://books.google.com/books/content?id=ANfzyQEACAAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api', N'Sách', 3, 4, 1, DATEADD(DAY,-11,GETDATE())),
(N'Sapiens - Lược sử loài người', N'Hành trình phát triển của nhân loại qua các thời kỳ', 320000, 272000, 230000, 29, 'https://books.google.com/books/content?id=hN0vEQAAQBAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api', N'Sách', 5, 1, 1, DATEADD(DAY,-13,GETDATE())),
(N'Địa lý Việt Nam hiện đại', N'Tổng quan lãnh thổ, vùng miền và đặc điểm tự nhiên Việt Nam', 210000, 178500, 145000, 33, '/images/books/dia_ly_vn.jpg', N'Sách', 5, 4, 1, DATEADD(DAY,-17,GETDATE())),
(N'Lịch sử thế giới giản lược', N'Các nền văn minh và sự kiện nổi bật của thế giới', 300000, 255000, 210000, 27, '/images/books/lich_su_the_gioi.jpg', N'Sách', 5, 4, 1, DATEADD(DAY,-20,GETDATE())),
(N'Tôi thấy hoa vàng trên cỏ xanh', N'Truyện dài giàu cảm xúc về tuổi thơ và gia đình', 125000, 106000, 82000, 51, 'https://books.google.com/books/content?id=xQMhyAEACAAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api', N'Sách', 1, 2, 1, DATEADD(DAY,-9,GETDATE())),
(N'Rừng Na Uy', N'Bản dịch tiếng Việt của Norwegian Wood', 220000, 187000, 150000, 24, 'https://books.google.com/books/content?id=M37o0AEACAAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api', N'Sách', 1, 3, 1, DATEADD(DAY,-6,GETDATE()));

INSERT INTO SAN_PHAM_TAC_GIA (MaSanPham, MaTacGia) VALUES (1,1),(2,2),(3,3),(4,3),(5,7),(6,7),(7,5),(8,1),(9,4),(10,6),(11,3),(12,3),(13,3),(14,7),(15,7),(16,4),(17,7),(18,7),(19,7),(20,5),(21,1),(22,5),(23,2),(24,4);

INSERT INTO KHUYEN_MAI (TenKhuyenMai, LoaiGiamGia, GiaTriGiam, DieuKienApDung, NgayBatDau, NgayKetThuc, TrangThai) VALUES
(N'Giảm 10% cho toàn bộ sách', N'Phần trăm', 10, 100000, DATEADD(DAY,-5,GETDATE()), DATEADD(DAY,20,GETDATE()), 1),
(N'Giảm 50.000 VND cho đơn hàng từ 300.000 VND', N'Tiền tệ', 50000, 300000, DATEADD(DAY,-10,GETDATE()), DATEADD(DAY,30,GETDATE()), 1);

INSERT INTO GIO_HANG (MaNguoiDung, NgayTao, TrangThai) VALUES (3, GETDATE(), 1), (4, GETDATE(), 1);
INSERT INTO CHI_TIET_GIO_HANG (MaGioHang, MaSanPham, SoLuong, DonGia) VALUES (1, 7, 1, 148750);

INSERT INTO DON_HANG (MaNguoiDung, NgayDat, HoTenNguoiNhan, SoDienThoaiNhan, MaXaGiao, SoNhaGiao, DuongGiao, TongTien, PhiShip, TrangThaiDonHang, GhiChu, MaKhuyenMai) VALUES
(3, DATEADD(DAY,-10,GETDATE()), N'Phạm Văn Khách', '0912345678', 4, N'25', N'Đường Nguyễn Huệ', 432250, 25000, N'Đã giao', N'Giao giờ hành chính', 1),
(4, DATEADD(DAY,-3,GETDATE()), N'Võ Thị Mua Sách', '0901234567', 7, N'45', N'Đường Thái Phiên', 600000, 30000, N'Đang giao', N'Liên hệ trước khi giao', 2),
(3, DATEADD(DAY,-1,GETDATE()), N'Phạm Văn Khách', '0912345678', 4, N'25', N'Đường Nguyễn Huệ', 153000, 20000, N'Chờ xác nhận', NULL, NULL);

INSERT INTO CHI_TIET_DON_HANG (MaDonHang, MaSanPham, SoLuong, DonGia, ThanhTien) VALUES
(1,1,3,72250,216750),(1,2,2,80750,161500),(2,3,1,272000,272000),(2,6,1,153000,153000),(3,6,1,153000,153000);

INSERT INTO THANH_TOAN (MaDonHang, PhuongThucThanhToan, SoTienThanhToan, TrangThaiThanhToan, MaGiaoDich, NgayThanhToan) VALUES
(1, N'Thanh toán khi nhận hàng', 432250, N'Đã thanh toán', 'TXN20250524001', DATEADD(DAY,-10,GETDATE())),
(2, N'Chuyển khoản ngân hàng', 600000, N'Chưa thanh toán', NULL, NULL),
(3, N'Thanh toán khi nhận hàng', 153000, N'Chưa thanh toán', NULL, NULL);

INSERT INTO DANH_GIA (MaNguoiDung, MaSanPham, SoSao, NoiDung, NgayDanhGia) VALUES
(3,1,5,N'Tác phẩm kinh điển, chất lượng bản in đẹp.',DATEADD(DAY,-9,GETDATE())),
(3,2,5,N'Sách rất dễ thương, phù hợp cho nhiều lứa tuổi.',DATEADD(DAY,-9,GETDATE())),
(4,3,4,N'Nội dung tốt, giải thích rõ ràng.',DATEADD(DAY,-2,GETDATE())),
(4,6,4,N'Sách hay, dễ áp dụng vào cuộc sống.',DATEADD(DAY,-1,GETDATE()));

INSERT INTO NHA_CUNG_CAP (TenNhaCungCap, DiaChi, SoDienThoai, Email, NguoiLienHe, TrangThai) VALUES
(N'Công ty phát hành sách A', N'Hà Nội', '0241111222', 'contact@sacha.vn', N'Nguyễn A', 1);

PRINT N'Tạo database QuanLyBanSach thành công.';
PRINT N'Admin: admin@quanlynhasach.vn / Admin@123';
PRINT N'User : customer1@gmail.com / Customer@123';
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

CREATE TABLE DANG_KY_NHAN_TIN (
    MaDangKy INT IDENTITY(1,1) PRIMARY KEY,
    Email VARCHAR(100) NOT NULL,
    NgayDangKy DATETIME2 NOT NULL DEFAULT GETDATE(),
    TrangThai BIT NOT NULL DEFAULT 1
);
GO


/* DPQ bổ sung sách thật và ảnh bìa online để giao diện đủ dữ liệu demo */
DECLARE @catVanHoc INT = (SELECT TOP 1 MaDanhMuc FROM DANH_MUC WHERE TenDanhMuc=N'Văn học');
DECLARE @catTech INT = (SELECT TOP 1 MaDanhMuc FROM DANH_MUC WHERE TenDanhMuc=N'Khoa học kỹ thuật');
DECLARE @catBiz INT = (SELECT TOP 1 MaDanhMuc FROM DANH_MUC WHERE TenDanhMuc=N'Kinh tế - Quản lý');
DECLARE @catSkill INT = (SELECT TOP 1 MaDanhMuc FROM DANH_MUC WHERE TenDanhMuc=N'Tâm lý - Kỹ năng sống');
DECLARE @catHistory INT = (SELECT TOP 1 MaDanhMuc FROM DANH_MUC WHERE TenDanhMuc=N'Lịch sử - Địa lý');
DECLARE @nxb INT = (SELECT TOP 1 MaNhaXuatBan FROM NHA_XUAT_BAN ORDER BY MaNhaXuatBan);

IF NOT EXISTS (SELECT 1 FROM TAC_GIA WHERE TenTacGia=N'Robert C. Martin') INSERT INTO TAC_GIA(TenTacGia,QuocTich,MoTa) VALUES(N'Robert C. Martin',N'Mỹ',N'Tác giả Clean Code');
IF NOT EXISTS (SELECT 1 FROM TAC_GIA WHERE TenTacGia=N'Andrew Hunt') INSERT INTO TAC_GIA(TenTacGia,QuocTich,MoTa) VALUES(N'Andrew Hunt',N'Mỹ',N'Đồng tác giả The Pragmatic Programmer');
IF NOT EXISTS (SELECT 1 FROM TAC_GIA WHERE TenTacGia=N'Eric Ries') INSERT INTO TAC_GIA(TenTacGia,QuocTich,MoTa) VALUES(N'Eric Ries',N'Mỹ',N'Tác giả The Lean Startup');
IF NOT EXISTS (SELECT 1 FROM TAC_GIA WHERE TenTacGia=N'Cal Newport') INSERT INTO TAC_GIA(TenTacGia,QuocTich,MoTa) VALUES(N'Cal Newport',N'Mỹ',N'Tác giả Deep Work');
IF NOT EXISTS (SELECT 1 FROM TAC_GIA WHERE TenTacGia=N'Yuval Noah Harari') INSERT INTO TAC_GIA(TenTacGia,QuocTich,MoTa) VALUES(N'Yuval Noah Harari',N'Israel',N'Tác giả Sapiens');
IF NOT EXISTS (SELECT 1 FROM TAC_GIA WHERE TenTacGia=N'Frank Herbert') INSERT INTO TAC_GIA(TenTacGia,QuocTich,MoTa) VALUES(N'Frank Herbert',N'Mỹ',N'Tác giả Dune');
IF NOT EXISTS (SELECT 1 FROM TAC_GIA WHERE TenTacGia=N'Antoine de Saint-Exupéry') INSERT INTO TAC_GIA(TenTacGia,QuocTich,MoTa) VALUES(N'Antoine de Saint-Exupéry',N'Pháp',N'Tác giả Hoàng tử bé');
IF NOT EXISTS (SELECT 1 FROM TAC_GIA WHERE TenTacGia=N'F. Scott Fitzgerald') INSERT INTO TAC_GIA(TenTacGia,QuocTich,MoTa) VALUES(N'F. Scott Fitzgerald',N'Mỹ',N'Tác giả The Great Gatsby');

DECLARE @aMartin INT=(SELECT MaTacGia FROM TAC_GIA WHERE TenTacGia=N'Robert C. Martin');
DECLARE @aHunt INT=(SELECT MaTacGia FROM TAC_GIA WHERE TenTacGia=N'Andrew Hunt');
DECLARE @aRies INT=(SELECT MaTacGia FROM TAC_GIA WHERE TenTacGia=N'Eric Ries');
DECLARE @aNewport INT=(SELECT MaTacGia FROM TAC_GIA WHERE TenTacGia=N'Cal Newport');
DECLARE @aHarari INT=(SELECT MaTacGia FROM TAC_GIA WHERE TenTacGia=N'Yuval Noah Harari');
DECLARE @aHerbert INT=(SELECT MaTacGia FROM TAC_GIA WHERE TenTacGia=N'Frank Herbert');
DECLARE @aSaint INT=(SELECT MaTacGia FROM TAC_GIA WHERE TenTacGia=N'Antoine de Saint-Exupéry');
DECLARE @aFitz INT=(SELECT MaTacGia FROM TAC_GIA WHERE TenTacGia=N'F. Scott Fitzgerald');

DECLARE @NewBooks TABLE(Ten NVARCHAR(200), MoTa NVARCHAR(MAX), GiaBia DECIMAL(18,2), GiaBan DECIMAL(18,2), GiaNhap DECIMAL(18,2), Ton INT, Anh VARCHAR(255), Cat INT, TacGia INT);
INSERT INTO @NewBooks VALUES
(N'The Pragmatic Programmer',N'Tư duy thực dụng giúp lập trình viên viết phần mềm tốt hơn.',320000,272000,230000,32,'https://covers.openlibrary.org/b/isbn/9780201616224-L.jpg',@catTech,@aHunt),
(N'Design Patterns',N'Các mẫu thiết kế kinh điển trong lập trình hướng đối tượng.',420000,357000,310000,18,'https://covers.openlibrary.org/b/isbn/9780201633610-L.jpg',@catTech,@aMartin),
(N'Python Crash Course',N'Sách nhập môn Python dễ học, nhiều ví dụ thực hành.',360000,306000,250000,36,'https://covers.openlibrary.org/b/isbn/9781593279288-L.jpg',@catTech,@aMartin),
(N'Fluent Python',N'Kỹ thuật Python nâng cao cho lập trình viên chuyên nghiệp.',520000,442000,390000,16,'https://covers.openlibrary.org/b/isbn/9781491946008-L.jpg',@catTech,@aMartin),
(N'Deep Work',N'Làm việc sâu trong thời đại nhiều xao nhãng.',210000,178500,145000,42,'https://covers.openlibrary.org/b/isbn/9781455586691-L.jpg',@catSkill,@aNewport),
(N'Start With Why',N'Tìm lý do cốt lõi để dẫn dắt bản thân và tổ chức.',225000,191000,150000,40,'https://covers.openlibrary.org/b/isbn/9781591846444-L.jpg',@catSkill,@aNewport),
(N'Good to Great',N'Từ công ty tốt đến công ty vĩ đại.',280000,238000,190000,24,'https://covers.openlibrary.org/b/isbn/9780066620992-L.jpg',@catBiz,@aRies),
(N'Blue Ocean Strategy',N'Chiến lược đại dương xanh trong kinh doanh.',295000,250000,205000,26,'https://covers.openlibrary.org/b/isbn/9781591396192-L.jpg',@catBiz,@aRies),
(N'Rich Dad Poor Dad',N'Bài học tài chính cá nhân nổi tiếng.',180000,153000,120000,50,'https://covers.openlibrary.org/b/isbn/9781612680194-L.jpg',@catBiz,@aRies),
(N'Homo Deus',N'Lược sử tương lai của nhân loại.',340000,289000,240000,28,'https://covers.openlibrary.org/b/isbn/9780062464316-L.jpg',@catHistory,@aHarari),
(N'Guns, Germs, and Steel',N'Phân tích lịch sử phát triển các nền văn minh.',360000,306000,250000,22,'https://covers.openlibrary.org/b/isbn/9780393317558-L.jpg',@catHistory,@aHarari),
(N'A Brief History of Time',N'Lược sử thời gian và các ý tưởng vật lý hiện đại.',260000,221000,175000,30,'https://covers.openlibrary.org/b/isbn/9780553380163-L.jpg',@catHistory,@aHarari),
(N'Dune',N'Tiểu thuyết khoa học viễn tưởng kinh điển.',310000,263500,220000,21,'https://covers.openlibrary.org/b/isbn/9780441172719-L.jpg',@catVanHoc,@aHerbert),
(N'Hoàng tử bé',N'Tác phẩm văn học thiếu nhi giàu triết lý.',120000,102000,82000,47,'https://covers.openlibrary.org/b/isbn/9780156012195-L.jpg',@catVanHoc,@aSaint),
(N'The Great Gatsby',N'Tác phẩm kinh điển của văn học Mỹ.',160000,136000,105000,34,'https://covers.openlibrary.org/b/isbn/9780743273565-L.jpg',@catVanHoc,@aFitz);

DECLARE @Ten NVARCHAR(200), @MoTa NVARCHAR(MAX), @GiaBia DECIMAL(18,2), @GiaBan DECIMAL(18,2), @GiaNhap DECIMAL(18,2), @Ton INT, @Anh VARCHAR(255), @Cat INT, @TacGia INT;
DECLARE cur CURSOR FOR SELECT Ten,MoTa,GiaBia,GiaBan,GiaNhap,Ton,Anh,Cat,TacGia FROM @NewBooks;
OPEN cur;
FETCH NEXT FROM cur INTO @Ten,@MoTa,@GiaBia,@GiaBan,@GiaNhap,@Ton,@Anh,@Cat,@TacGia;
WHILE @@FETCH_STATUS = 0
BEGIN
    IF NOT EXISTS (SELECT 1 FROM SAN_PHAM WHERE TenSanPham=@Ten)
    BEGIN
        INSERT INTO SAN_PHAM(TenSanPham,MoTa,GiaBia,GiaBan,GiaNhap,SoLuongTon,AnhBia,LoaiSanPham,MaDanhMuc,MaNhaXuatBan,TrangThai,NgayTao)
        VALUES(@Ten,@MoTa,@GiaBia,@GiaBan,@GiaNhap,@Ton,@Anh,N'Sách',@Cat,@nxb,1,DATEADD(DAY,-ABS(CHECKSUM(NEWID()))%40,GETDATE()));
        INSERT INTO SAN_PHAM_TAC_GIA(MaSanPham,MaTacGia) VALUES(SCOPE_IDENTITY(),@TacGia);
    END
    FETCH NEXT FROM cur INTO @Ten,@MoTa,@GiaBia,@GiaBan,@GiaNhap,@Ton,@Anh,@Cat,@TacGia;
END
CLOSE cur;
DEALLOCATE cur;

UPDATE SAN_PHAM SET AnhBia='https://covers.openlibrary.org/b/isbn/9780743269513-L.jpg' WHERE TenSanPham=N'Những nguyên tắc để thành công';
UPDATE SAN_PHAM SET AnhBia='https://covers.openlibrary.org/b/isbn/9781617291340-L.jpg' WHERE TenSanPham LIKE N'Lập trình C#%';
UPDATE SAN_PHAM SET AnhBia='https://covers.openlibrary.org/b/isbn/9781492041139-L.jpg' WHERE TenSanPham LIKE N'Data Science%';
UPDATE SAN_PHAM SET AnhBia='https://covers.openlibrary.org/b/isbn/9781617294617-L.jpg' WHERE TenSanPham LIKE N'ASP.NET Core%';
UPDATE SAN_PHAM SET AnhBia='https://covers.openlibrary.org/b/isbn/9780136042594-L.jpg' WHERE TenSanPham LIKE N'Trí tuệ nhân tạo%';
UPDATE SAN_PHAM SET AnhBia='https://covers.openlibrary.org/b/isbn/9780062464316-L.jpg' WHERE TenSanPham LIKE N'Lịch sử Việt Nam';
