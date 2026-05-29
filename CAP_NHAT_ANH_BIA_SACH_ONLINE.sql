USE QuanLyBanSach;
GO

-- Cập nhật lại các bìa bị Google Books trả về "image not available".
-- Các URL dưới đây là ảnh bìa trực tiếp từ trang bán sách/thư viện online.
UPDATE SAN_PHAM SET AnhBia = N'https://www.netabooks.vn/Data/Sites/1/Product/78700/thumbs/truyen-kieu-bia-cung.jpg'
WHERE TenSanPham = N'Truyện Kiều';

UPDATE SAN_PHAM SET AnhBia = N'https://www.netabooks.vn/Data/Sites/1/Product/38406/thumbs/de-men-phieu-luu-ky-bia-cung.jpg'
WHERE TenSanPham = N'Dế Mèn Phiêu Lưu Ký';

UPDATE SAN_PHAM SET AnhBia = N'https://minhkhai.com.vn/hinhlon/8935075937673.jpg'
WHERE TenSanPham = N'Lịch sử Việt Nam';

UPDATE SAN_PHAM SET AnhBia = N'https://covers.openlibrary.org/b/isbn/9780375704024-L.jpg'
WHERE TenSanPham IN (N'Rừng Na Uy', N'Norwegian Wood');
GO
