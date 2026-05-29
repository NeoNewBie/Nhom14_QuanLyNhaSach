# Ảnh bìa sách online

Bản này không dùng bìa sách tự tạo local.

- `Models/SanPham.cs` đã được sửa để luôn trả về URL ảnh bìa online theo tên sách.
- `CAP_NHAT_ANH_BIA_SACH_ONLINE.sql` cập nhật trực tiếp database hiện tại mà không cần drop DB.
- Nguồn ảnh dùng Google Books Volume ID hoặc Open Library Covers API theo ISBN.

Cách cập nhật nhanh nếu database đã có dữ liệu:

1. Mở SSMS.
2. Chọn đúng server SQL.
3. Chạy file `CAP_NHAT_ANH_BIA_SACH_ONLINE.sql`.
4. Dừng và chạy lại website.
