-- =============================================
-- TẠO CƠ SỞ DỮ LIỆU (nếu chưa có)
-- =============================================
CREATE DATABASE QuanLyBanSach;
GO

USE QuanLyBanSach;
GO

-- =============================================
-- 1. Bảng VAI_TRO
-- =============================================
CREATE TABLE VAI_TRO (
    MaVaiTro INT IDENTITY(1,1) PRIMARY KEY,
    TenVaiTro NVARCHAR(50) NOT NULL UNIQUE,
    MoTa NVARCHAR(200) NULL
);
GO

-- =============================================
-- 2. Bảng NGUOI_DUNG
-- =============================================
CREATE TABLE NGUOI_DUNG (
    MaNguoiDung INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    SoDienThoai VARCHAR(15) NOT NULL UNIQUE,
    MatKhau VARCHAR(255) NOT NULL,
    DiaChi NVARCHAR(255) NULL,
    GioiTinh BIT NULL,
    NgaySinh DATE NULL,
    TrangThai BIT NOT NULL DEFAULT 1,
    MaVaiTro INT NOT NULL FOREIGN KEY REFERENCES VAI_TRO(MaVaiTro),
    NgayTao DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

-- =============================================
-- 3. Bảng DANH_MUC
-- =============================================
CREATE TABLE DANH_MUC (
    MaDanhMuc INT IDENTITY(1,1) PRIMARY KEY,
    TenDanhMuc NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(255) NULL,
    MaDanhMucCha INT NULL FOREIGN KEY REFERENCES DANH_MUC(MaDanhMuc),
    TrangThai BIT NOT NULL DEFAULT 1
);
GO

-- =============================================
-- 4. Bảng NHA_XUAT_BAN
-- =============================================
CREATE TABLE NHA_XUAT_BAN (
    MaNhaXuatBan INT IDENTITY(1,1) PRIMARY KEY,
    TenNhaXuatBan NVARCHAR(150) NOT NULL,
    DiaChi NVARCHAR(255) NULL,
    SoDienThoai VARCHAR(15) NULL,
    Email VARCHAR(100) NULL,
    Website VARCHAR(200) NULL
);
GO

-- =============================================
-- 5. Bảng TAC_GIA
-- =============================================
CREATE TABLE TAC_GIA (
    MaTacGia INT IDENTITY(1,1) PRIMARY KEY,
    TenTacGia NVARCHAR(150) NOT NULL,
    QuocTich NVARCHAR(50) NULL,
    MoTa NVARCHAR(255) NULL
);
GO

-- =============================================
-- 6. Bảng KHUYEN_MAI (được tham chiếu bởi DON_HANG)
-- =============================================
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

-- =============================================
-- 7. Bảng SAN_PHAM
-- =============================================
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
    NgayTao DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

-- =============================================
-- 8. Bảng SAN_PHAM_TAC_GIA (liên kết nhiều-nhiều)
-- =============================================
CREATE TABLE SAN_PHAM_TAC_GIA (
    MaSanPham INT NOT NULL,
    MaTacGia INT NOT NULL,
    PRIMARY KEY (MaSanPham, MaTacGia),
    FOREIGN KEY (MaSanPham) REFERENCES SAN_PHAM(MaSanPham),
    FOREIGN KEY (MaTacGia) REFERENCES TAC_GIA(MaTacGia)
);
GO

-- =============================================
-- 9. Bảng GIO_HANG
-- =============================================
CREATE TABLE GIO_HANG (
    MaGioHang INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL FOREIGN KEY REFERENCES NGUOI_DUNG(MaNguoiDung),
    NgayTao DATETIME2 NOT NULL DEFAULT GETDATE(),
    TrangThai BIT NOT NULL DEFAULT 1
);
GO

-- =============================================
-- 10. Bảng CHI_TIET_GIO_HANG
-- =============================================
CREATE TABLE CHI_TIET_GIO_HANG (
    MaGioHang INT NOT NULL,
    MaSanPham INT NOT NULL,
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) NOT NULL,
    PRIMARY KEY (MaGioHang, MaSanPham),
    FOREIGN KEY (MaGioHang) REFERENCES GIO_HANG(MaGioHang),
    FOREIGN KEY (MaSanPham) REFERENCES SAN_PHAM(MaSanPham)
);
GO

-- =============================================
-- 11. Bảng DON_HANG
-- =============================================
CREATE TABLE DON_HANG (
    MaDonHang INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL FOREIGN KEY REFERENCES NGUOI_DUNG(MaNguoiDung),
    NgayDat DATETIME2 NOT NULL DEFAULT GETDATE(),
    HoTenNguoiNhan NVARCHAR(100) NOT NULL,
    SoDienThoaiNhan VARCHAR(15) NOT NULL,
    DiaChiGiaoHang NVARCHAR(255) NOT NULL,
    TongTien DECIMAL(18,2) NOT NULL,
    PhiShip DECIMAL(18,2) NOT NULL DEFAULT 0,
    TrangThaiDonHang NVARCHAR(50) NOT NULL,
    GhiChu NVARCHAR(255) NULL,
    MaKhuyenMai INT NULL FOREIGN KEY REFERENCES KHUYEN_MAI(MaKhuyenMai)
);
GO

-- Bổ sung CHECK cho TrangThaiDonHang (khuyến nghị)
ALTER TABLE DON_HANG ADD CONSTRAINT CK_DON_HANG_TrangThai CHECK (TrangThaiDonHang IN (N'Chờ xác nhận', N'Đã xác nhận', N'Đang giao', N'Đã giao', N'Đã hủy'));
GO

-- =============================================
-- 12. Bảng CHI_TIET_DON_HANG
-- =============================================
CREATE TABLE CHI_TIET_DON_HANG (
    MaDonHang INT NOT NULL,
    MaSanPham INT NOT NULL,
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) NOT NULL,
    ThanhTien DECIMAL(18,2) NOT NULL,
    PRIMARY KEY (MaDonHang, MaSanPham),
    FOREIGN KEY (MaDonHang) REFERENCES DON_HANG(MaDonHang),
    FOREIGN KEY (MaSanPham) REFERENCES SAN_PHAM(MaSanPham)
);
GO

-- =============================================
-- 13. Bảng THANH_TOAN
-- =============================================
CREATE TABLE THANH_TOAN (
    MaThanhToan INT IDENTITY(1,1) PRIMARY KEY,
    MaDonHang INT NOT NULL FOREIGN KEY REFERENCES DON_HANG(MaDonHang),
    PhuongThucThanhToan NVARCHAR(50) NOT NULL,
    SoTienThanhToan DECIMAL(18,2) NOT NULL,
    TrangThaiThanhToan NVARCHAR(50) NOT NULL,
    MaGiaoDich VARCHAR(100) NULL UNIQUE,
    NgayThanhToan DATETIME2 NULL
);
GO

-- (Tùy chọn) Nếu mỗi đơn hàng chỉ có một thanh toán duy nhất, thêm UNIQUE (MaDonHang)
ALTER TABLE THANH_TOAN ADD CONSTRAINT UQ_THANH_TOAN_MaDonHang UNIQUE (MaDonHang);
GO

-- =============================================
-- 14. Bảng DANH_GIA
-- =============================================
CREATE TABLE DANH_GIA (
    MaDanhGia INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL FOREIGN KEY REFERENCES NGUOI_DUNG(MaNguoiDung),
    MaSanPham INT NOT NULL FOREIGN KEY REFERENCES SAN_PHAM(MaSanPham),
    SoSao INT NOT NULL CHECK (SoSao BETWEEN 1 AND 5),
    NoiDung NVARCHAR(MAX) NULL,
    NgayDanhGia DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

-- Bổ sung ràng buộc duy nhất cho cặp (MaNguoiDung, MaSanPham): một người chỉ đánh giá một sản phẩm một lần
ALTER TABLE DANH_GIA ADD CONSTRAINT UQ_DANH_GIA_NguoiDung_SanPham UNIQUE (MaNguoiDung, MaSanPham);
GO

-- =============================================
-- RÀNG BUỘC BỔ SUNG: Mỗi người dùng chỉ có một giỏ hàng đang hoạt động (TrangThai = 1)
-- Sử dụng filtered unique index (SQL Server 2008 trở lên)
-- =============================================
CREATE UNIQUE INDEX UQ_GIO_HANG_Active ON GIO_HANG (MaNguoiDung) WHERE TrangThai = 1;
GO

-- =============================================
-- (Tùy chọn) Thêm chỉ mục cho các cột thường xuyên tìm kiếm
-- =============================================
CREATE INDEX IX_SAN_PHAM_MaDanhMuc ON SAN_PHAM(MaDanhMuc);
CREATE INDEX IX_SAN_PHAM_MaNhaXuatBan ON SAN_PHAM(MaNhaXuatBan);
CREATE INDEX IX_DON_HANG_MaNguoiDung ON DON_HANG(MaNguoiDung);
CREATE INDEX IX_DON_HANG_NgayDat ON DON_HANG(NgayDat);
CREATE INDEX IX_DANH_GIA_MaSanPham ON DANH_GIA(MaSanPham);
CREATE INDEX IX_CHI_TIET_DON_HANG_MaSanPham ON CHI_TIET_DON_HANG(MaSanPham);
CREATE INDEX IX_CHI_TIET_GIO_HANG_MaSanPham ON CHI_TIET_GIO_HANG(MaSanPham);
GO

-- =============================================
-- CHÈN MỘT SỐ DỮ LIỆU MẪU (tùy chọn)
-- =============================================
-- Thêm vai trò mặc định
INSERT INTO VAI_TRO (TenVaiTro, MoTa) VALUES (N'User', N'Người dùng thông thường');
INSERT INTO VAI_TRO (TenVaiTro, MoTa) VALUES (N'Admin', N'Quản trị viên');

-- Thêm một tài khoản admin mẫu (mật khẩu: 123456 - nên mã hóa thực tế)
INSERT INTO NGUOI_DUNG (HoTen, Email, SoDienThoai, MatKhau, DiaChi, GioiTinh, NgaySinh, TrangThai, MaVaiTro)
VALUES (N'Quản trị hệ thống', 'admin@example.com', '0987654321', '123456', N'Hà Nội', 1, '1990-01-01', 1, 2);
GO
-- =============================================
-- TẠO LẠI TOÀN BỘ KHÚC XÃ - TỈNH (2 CẤP)
-- Dùng cho SQL Server
-- =============================================

-- =============================================
-- 1. XÓA CÁC RÀNG BUỘC KHÓA NGOẠI CŨ (NẾU CÓ)
-- =============================================
-- Kiểm tra và xóa khóa ngoại từ NGUOI_DUNG, DON_HANG đến bảng địa chỉ cũ
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_NGUOI_DUNG_MaXa')
    ALTER TABLE NGUOI_DUNG DROP CONSTRAINT FK_NGUOI_DUNG_MaXa;
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_DON_HANG_MaXaGiao')
    ALTER TABLE DON_HANG DROP CONSTRAINT FK_DON_HANG_MaXaGiao;

-- Nếu có bảng XA_PHUONG cũ (có cấp quận/huyện) -> xóa khóa ngoại từ bảng đó lên QUAN_HUYEN
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_XA_PHUONG_MaQuan')
    ALTER TABLE XA_PHUONG DROP CONSTRAINT FK_XA_PHUONG_MaQuan;
	
-- Xóa bảng cũ (nếu tồn tại) – nên đổi tên thay vì xóa để giữ dữ liệu nếu cần
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'XA_PHUONG')
    EXEC sp_rename 'XA_PHUONG', 'XA_PHUONG_CU_BACKUP';
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'QUAN_HUYEN')
    EXEC sp_rename 'QUAN_HUYEN', 'QUAN_HUYEN_CU_BACKUP';
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'TINH_THANH')
    EXEC sp_rename 'TINH_THANH', 'TINH_THANH_CU_BACKUP';
GO

-- =============================================
-- 2. TẠO BẢNG TỈNH/THÀNH PHỐ
-- =============================================
CREATE TABLE TINH_THANH (
    MaTinh INT IDENTITY(1,1) PRIMARY KEY,
    TenTinh NVARCHAR(100) NOT NULL,
    TrangThai BIT NOT NULL DEFAULT 1
);
GO

-- =============================================
-- 3. TẠO BẢNG XÃ/PHƯỜNG (LIÊN KẾT TRỰC TIẾP VỚI TỈNH)
-- =============================================
CREATE TABLE XA_PHUONG (
    MaXa INT IDENTITY(1,1) PRIMARY KEY,
    TenXa NVARCHAR(100) NOT NULL,
    MaTinh INT NOT NULL FOREIGN KEY REFERENCES TINH_THANH(MaTinh),
    TrangThai BIT NOT NULL DEFAULT 1
);
GO

-- =============================================
-- 4. CẬP NHẬT BẢNG NGUOI_DUNG (BỎ CỘT ĐỊA CHỈ CŨ, THÊM CỘT MỚI)
-- =============================================
-- Xóa cột DiaChi cũ nếu tồn tại
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('NGUOI_DUNG') AND name = 'DiaChi')
    ALTER TABLE NGUOI_DUNG DROP COLUMN DiaChi;

-- Thêm cột mới
ALTER TABLE NGUOI_DUNG ADD
    MaXa INT NULL,
    SoNha NVARCHAR(50) NULL,
    Duong NVARCHAR(100) NULL;
GO

-- Thêm khóa ngoại
ALTER TABLE NGUOI_DUNG ADD CONSTRAINT FK_NGUOI_DUNG_XA_PHUONG
    FOREIGN KEY (MaXa) REFERENCES XA_PHUONG(MaXa);
GO

-- Tạo chỉ mục cho cột khóa ngoại
CREATE INDEX IX_NGUOI_DUNG_MaXa ON NGUOI_DUNG(MaXa);
GO

-- =============================================
-- 5. CẬP NHẬT BẢNG DON_HANG (BỎ CỘT ĐỊA CHỈ GIAO CŨ, THÊM CỘT MỚI)
-- =============================================
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('DON_HANG') AND name = 'DiaChiGiaoHang')
    ALTER TABLE DON_HANG DROP COLUMN DiaChiGiaoHang;

ALTER TABLE DON_HANG ADD
    MaXaGiao INT NULL,
    SoNhaGiao NVARCHAR(50) NULL,
    DuongGiao NVARCHAR(100) NULL;
GO

ALTER TABLE DON_HANG ADD CONSTRAINT FK_DON_HANG_XA_PHUONG
    FOREIGN KEY (MaXaGiao) REFERENCES XA_PHUONG(MaXa);
GO

CREATE INDEX IX_DON_HANG_MaXaGiao ON DON_HANG(MaXaGiao);
GO

-- =============================================
-- 6. CHÈN DỮ LIỆU MẪU (MỘT SỐ TỈNH/THÀNH VÀ XÃ/PHƯỜNG)
-- =============================================
INSERT INTO TINH_THANH (TenTinh) VALUES 
(N'Hà Nội'),
(N'TP. Hồ Chí Minh'),
(N'Đà Nẵng');

-- Hà Nội (MaTinh = 1) – chỉ ví dụ một vài phường mới sau sáp nhập
INSERT INTO XA_PHUONG (TenXa, MaTinh) VALUES 
(N'Phường Trung Hòa', 1),
(N'Phường Nhân Chính', 1),
(N'Xã Đông Anh', 1);

-- TP.HCM (MaTinh = 2)
INSERT INTO XA_PHUONG (TenXa, MaTinh) VALUES 
(N'Phường Bến Nghé', 2),
(N'Phường Thảo Điền', 2),
(N'Xã Bình Hưng', 2);

-- Đà Nẵng (MaTinh = 3)
INSERT INTO XA_PHUONG (TenXa, MaTinh) VALUES 
(N'Phường Hòa Cường Bắc', 3),
(N'Phường An Hải Bắc', 3),
(N'Xã Hòa Liên', 3);
GO

-- =============================================
-- 7. (TÙY CHỌN) TẠO VIEW HIỂN THỊ ĐỊA CHỈ ĐẦY ĐỦ
-- =============================================
CREATE VIEW V_NGUOI_DUNG_DIA_CHI AS
SELECT 
    nd.MaNguoiDung,
    nd.HoTen,
    nd.Email,
    nd.SoDienThoai,
    nd.SoNha,
    nd.Duong,
    x.TenXa,
    t.TenTinh,
    -- Địa chỉ đầy đủ
    CONCAT_WS(N', ', nd.SoNha, nd.Duong, x.TenXa, t.TenTinh) AS DiaChiDayDu
FROM NGUOI_DUNG nd
LEFT JOIN XA_PHUONG x ON nd.MaXa = x.MaXa
LEFT JOIN TINH_THANH t ON x.MaTinh = t.MaTinh;
GO

CREATE VIEW V_DON_HANG_DIA_CHI AS
SELECT 
    dh.MaDonHang,
    dh.MaNguoiDung,
    dh.SoNhaGiao,
    dh.DuongGiao,
    x.TenXa,
    t.TenTinh,
    CONCAT_WS(N', ', dh.SoNhaGiao, dh.DuongGiao, x.TenXa, t.TenTinh) AS DiaChiGiaoDayDu
FROM DON_HANG dh
LEFT JOIN XA_PHUONG x ON dh.MaXaGiao = x.MaXa
LEFT JOIN TINH_THANH t ON x.MaTinh = t.MaTinh;
GO

-- =============================================
-- 8. CẬP NHẬT DỮ LIỆU MẪU CHO ADMIN (NẾU CÓ)
-- =============================================
UPDATE NGUOI_DUNG 
SET MaXa = 1, SoNha = N'Số 10', Duong = N'Đường Nguyễn Phong Sắc'
WHERE MaNguoiDung = 1;
GO

PRINT N'Đã tạo lại cấu trúc tỉnh - xã thành công!';
PRINT N'Các bảng cũ (nếu có) đã được đổi tên thành *_CU_BACKUP.';
PRINT N'Bạn có thể xóa các bảng backup sau khi kiểm tra dữ liệu.';
GO

-- =============================================
-- 15. Bảng NHA_CUNG_CAP
-- =============================================
CREATE TABLE NHA_CUNG_CAP (
    MaNhaCungCap INT IDENTITY(1,1) PRIMARY KEY,
    TenNhaCungCap NVARCHAR(150) NOT NULL,
    NguoiLienHe NVARCHAR(100) NULL,
    SoDienThoai VARCHAR(15) NULL,
    Email VARCHAR(100) NULL,
    DiaChi NVARCHAR(255) NULL,
    TrangThai BIT NOT NULL DEFAULT 1
);
GO
-- =============================================
-- 16. Bảng PHIEU_NHAP
-- =============================================
CREATE TABLE PHIEU_NHAP (
    MaPhieuNhap INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NOT NULL, -- Admin hoặc nhân viên kho thực hiện
    MaNhaCungCap INT NOT NULL,
    NgayNhap DATETIME2 NOT NULL DEFAULT GETDATE(),
    TongTien DECIMAL(18,2) NOT NULL DEFAULT 0,
    GhiChu NVARCHAR(255) NULL,
    FOREIGN KEY (MaNguoiDung) REFERENCES NGUOI_DUNG(MaNguoiDung),
    FOREIGN KEY (MaNhaCungCap) REFERENCES NHA_CUNG_CAP(MaNhaCungCap)
);
GO

-- =============================================
-- 17. Bảng CHI_TIET_PHIEU_NHAP
-- =============================================
CREATE TABLE CHI_TIET_PHIEU_NHAP (
    MaPhieuNhap INT NOT NULL,
    MaSanPham INT NOT NULL,
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    GiaNhap DECIMAL(18,2) NOT NULL, -- Giá vốn khi nhập về
    ThanhTien AS (SoLuong * GiaNhap) PERSISTED, -- Cột tự động tính toán
    PRIMARY KEY (MaPhieuNhap, MaSanPham),
    FOREIGN KEY (MaPhieuNhap) REFERENCES PHIEU_NHAP(MaPhieuNhap),
    FOREIGN KEY (MaSanPham) REFERENCES SAN_PHAM(MaSanPham)
);
GO
ALTER TABLE SAN_PHAM ADD GiaNhap DECIMAL(18,2) NOT NULL DEFAULT 0;
GO
-- Trigger tự động tăng số lượng tồn kho khi nhập hàng
CREATE TRIGGER TRG_CapNhatKho_KhiNhapHang
ON CHI_TIET_PHIEU_NHAP
AFTER INSERT
AS
BEGIN
    UPDATE SAN_PHAM
    SET SoLuongTon = SoLuongTon + (SELECT SoLuong FROM inserted WHERE inserted.MaSanPham = SAN_PHAM.MaSanPham)
    FROM SAN_PHAM
    JOIN inserted ON SAN_PHAM.MaSanPham = inserted.MaSanPham;
END;
GO

-- Trigger tự động giảm số lượng tồn kho khi có đơn hàng ĐÃ XÁC NHẬN
CREATE TRIGGER TRG_CapNhatKho_KhiBanHang
ON CHI_TIET_DON_HANG
AFTER INSERT
AS
BEGIN
    UPDATE SAN_PHAM
    SET SoLuongTon = SoLuongTon - (SELECT SoLuong FROM inserted WHERE inserted.MaSanPham = SAN_PHAM.MaSanPham)
    FROM SAN_PHAM
    JOIN inserted ON SAN_PHAM.MaSanPham = inserted.MaSanPham;
END;
GO