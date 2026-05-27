using Microsoft.AspNetCore.Mvc;

namespace QuanLyNhaSach.Controllers;

public class InformationController : Controller
{
    public IActionResult Page(string slug)
    {
        var data = slug?.ToLowerInvariant() switch
        {
            "shipping" => ("Chính sách vận chuyển", "BookPort hỗ trợ giao hàng toàn quốc. Phí vận chuyển được hiển thị ở bước thanh toán. Với dữ liệu mẫu hiện tại, phí vận chuyển mặc định là 0 đ để thuận tiện khi demo bài tập lớn."),
            "returns" => ("Đổi trả và hoàn tiền", "Khách hàng có thể liên hệ quản trị viên để đổi trả sách khi sản phẩm bị lỗi in ấn, giao sai tựa sách hoặc hư hỏng trong quá trình vận chuyển."),
            "privacy" => ("Chính sách bảo mật", "Thông tin tài khoản, đơn hàng và liên hệ của khách hàng chỉ được sử dụng cho mục đích quản lý bán sách trong hệ thống BookPort."),
            "terms" => ("Điều khoản sử dụng", "Người dùng cần cung cấp thông tin chính xác khi đăng ký, đặt hàng và không sử dụng website cho mục đích gây ảnh hưởng đến hoạt động của hệ thống."),
            "help" => ("Trung tâm trợ giúp", "Bạn có thể tìm kiếm sách, thêm vào giỏ hàng, áp dụng mã giảm giá, đặt hàng, theo dõi đơn hàng và gửi liên hệ đến quản trị viên."),
            "gift" => ("Số dư voucher", "Chức năng voucher được áp dụng trực tiếp tại trang giỏ hàng. Các mã mẫu có thể dùng: BOOK10, FREESHIP, STUDENT20."),
            "sitemap" => ("Sơ đồ website", "Các khu vực chính: Trang chủ, chi tiết sách, giỏ hàng, đăng nhập, đăng ký, tài khoản, yêu thích, lịch sử đơn hàng và trang quản trị."),
            _ => ("Thông tin BookPort", "BookPort là website quản lý bán sách được xây dựng bằng ASP.NET Core MVC, Entity Framework Core và SQL Server.")
        };

        ViewBag.Title = data.Item1;
        ViewBag.Content = data.Item2;
        return View();
    }

    public IActionResult AppDownload()
    {
        ViewBag.Title = "Tải ứng dụng BookPort";
        ViewBag.Content = "Phiên bản bài tập lớn hiện triển khai dưới dạng website. Nút tải ứng dụng được chuyển thành trang hướng dẫn sử dụng website trên trình duyệt, phù hợp khi nộp và demo trong Visual Studio.";
        return View("Page");
    }
}
