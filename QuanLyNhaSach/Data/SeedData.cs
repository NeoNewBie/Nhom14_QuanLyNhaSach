using QuanLyNhaSach.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace QuanLyNhaSach.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new QuanLyBanSachContext(
            serviceProvider.GetRequiredService<DbContextOptions<QuanLyBanSachContext>>()))
        {
            try
            {
                // Skip if data already exists
                if (context.VaiTros.Any())
                {
                    return;
                }

                // 1. Seed VaiTro (Roles)
                var vaiTro1 = new VaiTro { TenVaiTro = "User", MoTa = "Người dùng thông thường" };
                var vaiTro2 = new VaiTro { TenVaiTro = "Admin", MoTa = "Quản trị viên hệ thống" };
                var vaiTro3 = new VaiTro { TenVaiTro = "Staff", MoTa = "Nhân viên bán hàng" };
                context.VaiTros.Add(vaiTro1);
                context.VaiTros.Add(vaiTro2);
                context.VaiTros.Add(vaiTro3);
                context.SaveChanges();

                // 2. Seed TinhThanh (Provinces) - must come before XaPhuong
                var tinh1 = new TinhThanh { TenTinh = "Hà Nội", TrangThai = true };
                var tinh2 = new TinhThanh { TenTinh = "TP. Hồ Chí Minh", TrangThai = true };
                var tinh3 = new TinhThanh { TenTinh = "Đà Nẵng", TrangThai = true };
                var tinh4 = new TinhThanh { TenTinh = "Cần Thơ", TrangThai = true };
                context.TinhThanhs.Add(tinh1);
                context.TinhThanhs.Add(tinh2);
                context.TinhThanhs.Add(tinh3);
                context.TinhThanhs.Add(tinh4);
                context.SaveChanges();

                // 3. Seed XaPhuong (Districts/Wards)
                var xa1 = new XaPhuong { TenXa = "Phường Trung Hòa", MaTinh = tinh1.MaTinh, TrangThai = true };
                var xa2 = new XaPhuong { TenXa = "Phường Nhân Chính", MaTinh = tinh1.MaTinh, TrangThai = true };
                var xa3 = new XaPhuong { TenXa = "Xã Đông Anh", MaTinh = tinh1.MaTinh, TrangThai = true };
                var xa4 = new XaPhuong { TenXa = "Phường Bến Nghé", MaTinh = tinh2.MaTinh, TrangThai = true };
                var xa5 = new XaPhuong { TenXa = "Phường Thảo Điền", MaTinh = tinh2.MaTinh, TrangThai = true };
                var xa6 = new XaPhuong { TenXa = "Xã Bình Hưng", MaTinh = tinh2.MaTinh, TrangThai = true };
                var xa7 = new XaPhuong { TenXa = "Phường Hòa Cường Bắc", MaTinh = tinh3.MaTinh, TrangThai = true };
                var xa8 = new XaPhuong { TenXa = "Phường An Hải Bắc", MaTinh = tinh3.MaTinh, TrangThai = true };
                context.XaPhuongs.Add(xa1);
                context.XaPhuongs.Add(xa2);
                context.XaPhuongs.Add(xa3);
                context.XaPhuongs.Add(xa4);
                context.XaPhuongs.Add(xa5);
                context.XaPhuongs.Add(xa6);
                context.XaPhuongs.Add(xa7);
                context.XaPhuongs.Add(xa8);
                context.SaveChanges();

                // 4. Seed NguoiDung (Users)
                var user1 = new NguoiDung
                {
                    HoTen = "Nguyễn Văn Admin",
                    Email = "admin@quanlynhasach.vn",
                    SoDienThoai = "0987654321",
                    MatKhau = "Admin@123",
                    GioiTinh = true,
                    NgaySinh = new DateOnly(1990, 1, 15),
                    TrangThai = true,
                    MaVaiTro = vaiTro2.MaVaiTro,
                    NgayTao = DateTime.Now,
                    MaXa = xa1.MaXa,
                    SoNha = "10",
                    Duong = "Đường Nguyễn Phong Sắc"
                };
                var user2 = new NguoiDung
                {
                    HoTen = "Trần Thị Nhân Viên",
                    Email = "staff@quanlynhasach.vn",
                    SoDienThoai = "0976543210",
                    MatKhau = "Staff@123",
                    GioiTinh = false,
                    NgaySinh = new DateOnly(1995, 5, 20),
                    TrangThai = true,
                    MaVaiTro = vaiTro3.MaVaiTro,
                    NgayTao = DateTime.Now
                };
                var user3 = new NguoiDung
                {
                    HoTen = "Phạm Văn Khách",
                    Email = "customer1@gmail.com",
                    SoDienThoai = "0912345678",
                    MatKhau = "Customer@123",
                    GioiTinh = true,
                    NgaySinh = new DateOnly(1998, 3, 10),
                    TrangThai = true,
                    MaVaiTro = vaiTro1.MaVaiTro,
                    NgayTao = DateTime.Now,
                    MaXa = xa4.MaXa,
                    SoNha = "25",
                    Duong = "Đường Nguyễn Huệ"
                };
                var user4 = new NguoiDung
                {
                    HoTen = "Võ Thị Mua Sách",
                    Email = "customer2@gmail.com",
                    SoDienThoai = "0901234567",
                    MatKhau = "Customer@456",
                    GioiTinh = false,
                    NgaySinh = new DateOnly(2000, 7, 25),
                    TrangThai = true,
                    MaVaiTro = vaiTro1.MaVaiTro,
                    NgayTao = DateTime.Now,
                    MaXa = xa7.MaXa,
                    SoNha = "45",
                    Duong = "Đường Thái Phiên"
                };
                context.NguoiDungs.Add(user1);
                context.NguoiDungs.Add(user2);
                context.NguoiDungs.Add(user3);
                context.NguoiDungs.Add(user4);
                context.SaveChanges();

                // 5. Seed DanhMuc (Categories)
                var cat1 = new DanhMuc { TenDanhMuc = "Văn học", MoTa = "Sách văn học Việt Nam và quốc tế", TrangThai = true };
                var cat2 = new DanhMuc { TenDanhMuc = "Khoa học kỹ thuật", MoTa = "Sách về khoa học, công nghệ, lập trình", TrangThai = true };
                var cat3 = new DanhMuc { TenDanhMuc = "Kinh tế - Quản lý", MoTa = "Sách kinh doanh, quản lý doanh nghiệp", TrangThai = true };
                var cat4 = new DanhMuc { TenDanhMuc = "Tâm lý - Kỹ năng sống", MoTa = "Sách phát triển bản thân, tâm lý", TrangThai = true };
                var cat5 = new DanhMuc { TenDanhMuc = "Lịch sử - Địa lý", MoTa = "Sách lịch sử, địa lý, văn hóa", TrangThai = true };
                context.DanhMucs.Add(cat1);
                context.DanhMucs.Add(cat2);
                context.DanhMucs.Add(cat3);
                context.DanhMucs.Add(cat4);
                context.DanhMucs.Add(cat5);
                context.SaveChanges();

                // 6. Seed NhaXuatBan (Publishers)
                var pub1 = new NhaXuatBan
                {
                    TenNhaXuatBan = "NXB Trẻ",
                    DiaChi = "100 Thụy Khuê, Hà Nội",
                    SoDienThoai = "0243943203",
                    Email = "info@nxbtre.com.vn",
                    Website = "https://www.nxbtre.com.vn"
                };
                var pub2 = new NhaXuatBan
                {
                    TenNhaXuatBan = "NXB Kim Đồng",
                    DiaChi = "365 Cộng Hòa, TP. Hồ Chí Minh",
                    SoDienThoai = "0283932632",
                    Email = "info@nxbkimdong.com.vn",
                    Website = "https://www.nxbkimdong.com.vn"
                };
                var pub3 = new NhaXuatBan
                {
                    TenNhaXuatBan = "NXB Hội Nhà Văn",
                    DiaChi = "Hưng Phúc 1, Hà Nội",
                    SoDienThoai = "0243942206",
                    Email = "info@nxbhoinhavan.com.vn",
                    Website = "https://www.nxbhoinhavan.com.vn"
                };
                var pub4 = new NhaXuatBan
                {
                    TenNhaXuatBan = "NXB Lao Động",
                    DiaChi = "27 Trần Hưng Đạo, Hà Nội",
                    SoDienThoai = "0243933203",
                    Email = "info@nxblaodong.com.vn",
                    Website = "https://www.nxblaodong.com.vn"
                };
                context.NhaXuatBans.Add(pub1);
                context.NhaXuatBans.Add(pub2);
                context.NhaXuatBans.Add(pub3);
                context.NhaXuatBans.Add(pub4);
                context.SaveChanges();

                // 7. Seed TacGia (Authors)
                var author1 = new TacGia { TenTacGia = "Nguyễn Du", QuocTich = "Việt Nam", MoTa = "Tác giả kiệt xuất của Truyện Kiều" };
                var author2 = new TacGia { TenTacGia = "Tô Hoài", QuocTich = "Việt Nam", MoTa = "Tác giả nổi tiếng, tác phẩm Dế Mèn Phiêu Lưu Ký" };
                var author3 = new TacGia { TenTacGia = "Trần Hữu Tước", QuocTich = "Việt Nam", MoTa = "Nhà văn hiện đại" };
                var author4 = new TacGia { TenTacGia = "Haruki Murakami", QuocTich = "Nhật Bản", MoTa = "Tác giả nổi tiếng thế giới" };
                var author5 = new TacGia { TenTacGia = "George Orwell", QuocTich = "Anh", MoTa = "Tác giả bộ tiểu thuyết dystopian nổi tiếng" };
                var author6 = new TacGia { TenTacGia = "J.K. Rowling", QuocTich = "Anh", MoTa = "Tác giả series Harry Potter" };
                var author7 = new TacGia { TenTacGia = "Stephen Covey", QuocTich = "Mỹ", MoTa = "Tác giả sách phát triển bản thân" };
                context.TacGia.Add(author1);
                context.TacGia.Add(author2);
                context.TacGia.Add(author3);
                context.TacGia.Add(author4);
                context.TacGia.Add(author5);
                context.TacGia.Add(author6);
                context.TacGia.Add(author7);
                context.SaveChanges();

                // 8. Seed SanPham (Products)
                var prod1 = new SanPham
                {
                    TenSanPham = "Truyện Kiều",
                    MoTa = "Tác phẩm kinh điển của Nguyễn Du, một trong những tác phẩm văn học vĩ đại nhất của dân tộc Việt Nam",
                    GiaBia = 85000,
                    GiaBan = 72250,
                    GiaNhap = 60000,
                    SoLuongTon = 50,
                    AnhBia = "truyenkieu.jpg",
                    LoaiSanPham = "Sách",
                    MaDanhMuc = cat1.MaDanhMuc,
                    MaNhaXuatBan = pub3.MaNhaXuatBan,
                    TrangThai = true,
                    NgayTao = DateTime.Now.AddMonths(-2)
                };
                context.SanPhams.Add(prod1);

                var prod2 = new SanPham
                {
                    TenSanPham = "Dế Mèn Phiêu Lưu Ký",
                    MoTa = "Cuộc phiêu lưu kỳ thú của chú dế mèn Tí Hon qua đất nước kỳ ảo",
                    GiaBia = 95000,
                    GiaBan = 80750,
                    GiaNhap = 70000,
                    SoLuongTon = 35,
                    AnhBia = "demenphieuluuky.jpg",
                    LoaiSanPham = "Sách",
                    MaDanhMuc = cat1.MaDanhMuc,
                    MaNhaXuatBan = pub2.MaNhaXuatBan,
                    TrangThai = true,
                    NgayTao = DateTime.Now.AddMonths(-1)
                };
                context.SanPhams.Add(prod2);

                var prod3 = new SanPham
                {
                    TenSanPham = "Lập trình C# cơ bản đến nâng cao",
                    MoTa = "Hướng dẫn chi tiết về lập trình C# từ cơ bản đến các kỹ thuật nâng cao",
                    GiaBia = 320000,
                    GiaBan = 272000,
                    GiaNhap = 240000,
                    SoLuongTon = 25,
                    AnhBia = "csharp_guide.jpg",
                    LoaiSanPham = "Sách",
                    MaDanhMuc = cat2.MaDanhMuc,
                    MaNhaXuatBan = pub1.MaNhaXuatBan,
                    TrangThai = true,
                    NgayTao = DateTime.Now.AddMonths(-3)
                };
                context.SanPhams.Add(prod3);

                var prod4 = new SanPham
                {
                    TenSanPham = "Data Science với Python",
                    MoTa = "Tìm hiểu về data science, machine learning sử dụng Python",
                    GiaBia = 385000,
                    GiaBan = 326750,
                    GiaNhap = 300000,
                    SoLuongTon = 20,
                    AnhBia = "datascience_python.jpg",
                    LoaiSanPham = "Sách",
                    MaDanhMuc = cat2.MaDanhMuc,
                    MaNhaXuatBan = pub1.MaNhaXuatBan,
                    TrangThai = true,
                    NgayTao = DateTime.Now.AddMonths(-2)
                };
                context.SanPhams.Add(prod4);

                var prod5 = new SanPham
                {
                    TenSanPham = "Những nguyên tắc để thành công",
                    MoTa = "7 nguyên tắc vàng để đạt được thành công trong kinh doanh và cuộc sống",
                    GiaBia = 198000,
                    GiaBan = 168300,
                    GiaNhap = 150000,
                    SoLuongTon = 45,
                    AnhBia = "7_principles_success.jpg",
                    LoaiSanPham = "Sách",
                    MaDanhMuc = cat3.MaDanhMuc,
                    MaNhaXuatBan = pub4.MaNhaXuatBan,
                    TrangThai = true,
                    NgayTao = DateTime.Now
                };
                context.SanPhams.Add(prod5);

                var prod6 = new SanPham
                {
                    TenSanPham = "Thói quen nguyên tử",
                    MoTa = "Tìm hiểu cách thay đổi thói quen để cải thiện cuộc sống",
                    GiaBia = 180000,
                    GiaBan = 153000,
                    GiaNhap = 140000,
                    SoLuongTon = 60,
                    AnhBia = "atomic_habits.jpg",
                    LoaiSanPham = "Sách",
                    MaDanhMuc = cat4.MaDanhMuc,
                    MaNhaXuatBan = pub3.MaNhaXuatBan,
                    TrangThai = true,
                    NgayTao = DateTime.Now.AddMonths(-1)
                };
                context.SanPhams.Add(prod6);

                var prod7 = new SanPham
                {
                    TenSanPham = "1984",
                    MoTa = "Tiểu thuyết dystopian kinh điển của George Orwell",
                    GiaBia = 175000,
                    GiaBan = 148750,
                    GiaNhap = 130000,
                    SoLuongTon = 30,
                    AnhBia = "1984.jpg",
                    LoaiSanPham = "Sách",
                    MaDanhMuc = cat1.MaDanhMuc,
                    MaNhaXuatBan = pub2.MaNhaXuatBan,
                    TrangThai = true,
                    NgayTao = DateTime.Now.AddMonths(-4)
                };
                context.SanPhams.Add(prod7);

                var prod8 = new SanPham
                {
                    TenSanPham = "Lịch sử Việt Nam",
                    MoTa = "Tổng quan về lịch sử phát triển của dân tộc Việt Nam từ xưa đến nay",
                    GiaBia = 280000,
                    GiaBan = 238000,
                    GiaNhap = 210000,
                    SoLuongTon = 40,
                    AnhBia = "lich_su_vn.jpg",
                    LoaiSanPham = "Sách",
                    MaDanhMuc = cat5.MaDanhMuc,
                    MaNhaXuatBan = pub4.MaNhaXuatBan,
                    TrangThai = true,
                    NgayTao = DateTime.Now.AddMonths(-2)
                };
                context.SanPhams.Add(prod8);
                context.SaveChanges();

                prod1.MaTacGia.Add(author1);
                prod2.MaTacGia.Add(author2);
                prod3.MaTacGia.Add(author3);
                prod4.MaTacGia.Add(author3);
                prod5.MaTacGia.Add(author7);
                prod6.MaTacGia.Add(author7);
                prod7.MaTacGia.Add(author5);
                prod8.MaTacGia.Add(author1);
                context.SaveChanges();

                // 9. Seed KhuyenMai (Promotions)
                var promo1 = new KhuyenMai
                {
                    TenKhuyenMai = "Giảm 10% cho toàn bộ sách",
                    LoaiGiamGia = "Phần trăm",
                    GiaTriGiam = 10,
                    DieuKienApDung = 100000,
                    NgayBatDau = DateTime.Now.AddDays(-5),
                    NgayKetThuc = DateTime.Now.AddDays(20),
                    TrangThai = true
                };
                var promo2 = new KhuyenMai
                {
                    TenKhuyenMai = "Giảm 50.000 VND cho đơn hàng từ 300.000 VND",
                    LoaiGiamGia = "Tiền tệ",
                    GiaTriGiam = 50000,
                    DieuKienApDung = 300000,
                    NgayBatDau = DateTime.Now.AddDays(-10),
                    NgayKetThuc = DateTime.Now.AddDays(30),
                    TrangThai = true
                };
                var promo3 = new KhuyenMai
                {
                    TenKhuyenMai = "Mua 2 tặng 1 sách khoa học kỹ thuật",
                    LoaiGiamGia = "Tặng sách",
                    GiaTriGiam = 1,
                    DieuKienApDung = 2,
                    NgayBatDau = DateTime.Now.AddDays(-1),
                    NgayKetThuc = DateTime.Now.AddDays(15),
                    TrangThai = true
                };
                context.KhuyenMais.Add(promo1);
                context.KhuyenMais.Add(promo2);
                context.KhuyenMais.Add(promo3);
                context.SaveChanges();

                // 10. Seed GioHang (Shopping Carts)
                var cart1 = new GioHang
                {
                    MaNguoiDung = user3.MaNguoiDung,
                    NgayTao = DateTime.Now,
                    TrangThai = true
                };
                var cart2 = new GioHang
                {
                    MaNguoiDung = user4.MaNguoiDung,
                    NgayTao = DateTime.Now,
                    TrangThai = true
                };
                context.GioHangs.Add(cart1);
                context.GioHangs.Add(cart2);
                context.SaveChanges();

                // 11. Seed DonHang (Orders)
                var order1 = new DonHang
                {
                    MaNguoiDung = user3.MaNguoiDung,
                    NgayDat = DateTime.Now.AddDays(-10),
                    HoTenNguoiNhan = "Phạm Văn Khách",
                    SoDienThoaiNhan = "0912345678",
                    MaXaGiao = xa4.MaXa,
                    SoNhaGiao = "25",
                    DuongGiao = "Đường Nguyễn Huệ",
                    TongTien = 432250,
                    PhiShip = 25000,
                    TrangThaiDonHang = "Đã giao",
                    GhiChu = "Giao vào giờ hành chính",
                    MaKhuyenMai = promo1.MaKhuyenMai
                };
                var order2 = new DonHang
                {
                    MaNguoiDung = user4.MaNguoiDung,
                    NgayDat = DateTime.Now.AddDays(-3),
                    HoTenNguoiNhan = "Võ Thị Mua Sách",
                    SoDienThoaiNhan = "0901234567",
                    MaXaGiao = xa7.MaXa,
                    SoNhaGiao = "45",
                    DuongGiao = "Đường Thái Phiên",
                    TongTien = 600000,
                    PhiShip = 30000,
                    TrangThaiDonHang = "Đang giao",
                    GhiChu = "Liên hệ trước khi giao",
                    MaKhuyenMai = promo2.MaKhuyenMai
                };
                var order3 = new DonHang
                {
                    MaNguoiDung = user3.MaNguoiDung,
                    NgayDat = DateTime.Now.AddDays(-1),
                    HoTenNguoiNhan = "Phạm Văn Khách",
                    SoDienThoaiNhan = "0912345678",
                    MaXaGiao = xa4.MaXa,
                    SoNhaGiao = "25",
                    DuongGiao = "Đường Nguyễn Huệ",
                    TongTien = 153000,
                    PhiShip = 20000,
                    TrangThaiDonHang = "Chờ xác nhận",
                    GhiChu = "",
                    MaKhuyenMai = null
                };
                context.DonHangs.Add(order1);
                context.DonHangs.Add(order2);
                context.DonHangs.Add(order3);
                context.SaveChanges();

                // 12. Seed ChiTietDonHang (Order Details)
                context.ChiTietDonHangs.Add(new ChiTietDonHang
                {
                    MaDonHang = order1.MaDonHang,
                    MaSanPham = prod1.MaSanPham,
                    SoLuong = 3,
                    DonGia = 72250,
                    ThanhTien = 216750
                });
                context.ChiTietDonHangs.Add(new ChiTietDonHang
                {
                    MaDonHang = order1.MaDonHang,
                    MaSanPham = prod2.MaSanPham,
                    SoLuong = 2,
                    DonGia = 80750,
                    ThanhTien = 161500
                });
                context.ChiTietDonHangs.Add(new ChiTietDonHang
                {
                    MaDonHang = order2.MaDonHang,
                    MaSanPham = prod3.MaSanPham,
                    SoLuong = 1,
                    DonGia = 272000,
                    ThanhTien = 272000
                });
                context.ChiTietDonHangs.Add(new ChiTietDonHang
                {
                    MaDonHang = order2.MaDonHang,
                    MaSanPham = prod6.MaSanPham,
                    SoLuong = 1,
                    DonGia = 153000,
                    ThanhTien = 153000
                });
                context.SaveChanges();

                // 13. Seed ThanhToan (Payments)
                context.ThanhToans.Add(new ThanhToan
                {
                    MaDonHang = order1.MaDonHang,
                    PhuongThucThanhToan = "Thanh toán khi nhận hàng",
                    SoTienThanhToan = 432250,
                    TrangThaiThanhToan = "Đã thanh toán",
                    MaGiaoDich = "TXN20250524001",
                    NgayThanhToan = DateTime.Now.AddDays(-10)
                });
                context.ThanhToans.Add(new ThanhToan
                {
                    MaDonHang = order2.MaDonHang,
                    PhuongThucThanhToan = "Chuyển khoản ngân hàng",
                    SoTienThanhToan = 600000,
                    TrangThaiThanhToan = "Chưa thanh toán",
                    MaGiaoDich = null,
                    NgayThanhToan = null
                });
                context.SaveChanges();

                // 14. Seed DanhGia (Ratings/Reviews)
                context.DanhGia.Add(new DanhGium
                {
                    MaNguoiDung = user3.MaNguoiDung,
                    MaSanPham = prod1.MaSanPham,
                    SoSao = 5,
                    NoiDung = "Tác phẩm kinh điển, rất hay. Chất lượng bản in đẹp lắm!",
                    NgayDanhGia = DateTime.Now.AddDays(-9)
                });
                context.DanhGia.Add(new DanhGium
                {
                    MaNguoiDung = user3.MaNguoiDung,
                    MaSanPham = prod2.MaSanPham,
                    SoSao = 5,
                    NoiDung = "Sách rất dễ yêu, phù hợp cho cả trẻ em và người lớn.",
                    NgayDanhGia = DateTime.Now.AddDays(-9)
                });
                context.DanhGia.Add(new DanhGium
                {
                    MaNguoiDung = user4.MaNguoiDung,
                    MaSanPham = prod3.MaSanPham,
                    SoSao = 4,
                    NoiDung = "Nội dung tốt, giải thích rõ ràng. Một vài phần hơi khó.",
                    NgayDanhGia = DateTime.Now.AddDays(-2)
                });
                context.DanhGia.Add(new DanhGium
                {
                    MaNguoiDung = user4.MaNguoiDung,
                    MaSanPham = prod6.MaSanPham,
                    SoSao = 4,
                    NoiDung = "Sách hay, đầy cảm hứng. Giúp thay đổi cách suy nghĩ.",
                    NgayDanhGia = DateTime.Now.AddDays(-1)
                });
                context.SaveChanges();

                // 15. Seed ChiTietGioHang (Shopping Cart Details)
                context.ChiTietGioHangs.Add(new ChiTietGioHang
                {
                    MaGioHang = cart1.MaGioHang,
                    MaSanPham = prod7.MaSanPham,
                    SoLuong = 1,
                    DonGia = 148750
                });
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                var logger = loggerFactory?.CreateLogger("SeedData");
                logger?.LogError(ex, "An error occurred while seeding the database: {Message}", ex.Message);
                throw;
            }
        }
    }
}
