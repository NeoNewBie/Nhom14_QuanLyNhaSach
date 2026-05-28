# QuanLyNhaSach - BookPort

Website quản lý bán sách sử dụng ASP.NET Core MVC, Entity Framework Core, SQL Server, Razor View và Bootstrap.

## 1. Phần mềm cần có

- Visual Studio 2022
- .NET 8 SDK
- SQL Server hoặc SQL Server Express
- SQL Server Management Studio

## 2. Cách mở project

1. Giải nén file zip ra thư mục ngắn, ví dụ: `C:\QuanLyNhaSach_FIGMA_ALL_FUNCTIONS`.
2. Mở Visual Studio.
3. Chọn **Open a project or solution**.
4. Mở file `QuanLyNhaSach.sln`.

Không mở project trực tiếp trong file zip hoặc trong thư mục `Temp`.

## 3. Tạo database

1. Mở SQL Server Management Studio.
2. Mở file `DB_QuanLyBanSach.sql`.
3. Execute toàn bộ file.
4. Database được tạo là `QuanLyBanSach`.

File SQL sẽ xóa database cũ cùng tên rồi tạo lại database mới, vì vậy chỉ chạy trên máy demo/bài tập.

## 4. Connection string

Mở file:

`QuanLyNhaSach/appsettings.json`

SQL Server thường:

```json
"DefaultConnection": "Server=.;Database=QuanLyBanSach;Trusted_Connection=True;TrustServerCertificate=True;"
```

SQL Server Express:

```json
"DefaultConnection": "Server=.\\SQLEXPRESS;Database=QuanLyBanSach;Trusted_Connection=True;TrustServerCertificate=True;"
```

## 5. Tài khoản mẫu

Admin:

```text
admin@bookport.vn / 123456
```

User:

```text
user@bookport.vn / 123456
```

## 6. Mã giảm giá mẫu

```text
BOOK10
STUDENT20
FREESHIP
```

## 7. Chức năng khách hàng

- Xem trang chủ theo giao diện Figma.
- Tìm kiếm sách.
- Lọc sách theo danh mục.
- Xem chi tiết sách.
- Thêm giỏ hàng.
- Mua ngay.
- Cập nhật số lượng trong giỏ.
- Xóa sản phẩm khỏi giỏ.
- Áp dụng/bỏ mã giảm giá.
- Đặt hàng.
- Xem lịch sử đơn hàng.
- Xem chi tiết đơn hàng.
- Hủy đơn khi chưa giao.
- Đặt lại đơn cũ.
- Đánh giá sách khi đơn đã giao.
- Thêm/xóa sách yêu thích.
- Tìm kiếm/lọc/xóa hàng loạt sách yêu thích.
- Cập nhật thông tin cá nhân.
- Đổi mật khẩu.
- Xem thông báo tài khoản.
- Gửi liên hệ.
- Đăng ký nhận tin.
- Xem các trang footer: shipping, returns, privacy, terms, help, sitemap, download app.

## 8. Chức năng admin

- Dashboard tổng quan.
- Analytics.
- Invoice.
- Schedule.
- Calendar.
- Messages.
- Notifications.
- Settings.
- Xuất CSV hiệu suất.
- Danh sách đơn hàng.
- Chi tiết đơn hàng.
- Cập nhật trạng thái đơn.
- Hủy đơn và hoàn tồn kho.
- In đơn.
- Email Customer bằng mailto.
- Quản lý sách.
- Quản lý danh mục.
- Quản lý tác giả.
- Quản lý nhà xuất bản.
- Quản lý khách hàng.

## 9. Lỗi thường gặp

### Không mở được project

Nguyên nhân thường là mở trực tiếp trong file zip. Hãy giải nén ra thư mục riêng rồi mở `QuanLyNhaSach.sln`.

### Không kết nối được database

Kiểm tra lại tên server trong `appsettings.json`: dùng `Server=.` hoặc `Server=.\\SQLEXPRESS` tùy máy.

### Báo database không tồn tại

Chưa chạy file `DB_QuanLyBanSach.sql` trong SSMS.

### Lỗi package NuGet

Trong Visual Studio chọn **Restore NuGet Packages** hoặc build lại solution.

## 10. Ghi chú bảo mật

Bản bài tập lớn đang dùng Session và mật khẩu plain text để dễ chạy, dễ trình bày. Nếu triển khai thật cần dùng ASP.NET Identity hoặc thuật toán băm mật khẩu.

## Cập nhật sách theo thể loại và font tiếng Việt

Bản này đã bổ sung 39 đầu sách, chia theo 8 thể loại. Ảnh bìa sách dùng URL ảnh bìa thật theo ISBN từ OpenLibrary Covers; nếu máy không có Internet, giao diện sẽ tự dùng ảnh dự phòng để không bị vỡ layout. Font chính của website là Segoe UI/Arial để hiển thị tiếng Việt ổn định trên Windows.

Hero trang chủ đã giữ nội dung tiếng Anh: `Discover the future of reading.`
