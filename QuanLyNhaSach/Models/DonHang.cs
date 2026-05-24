using System;
using System.Collections.Generic;

namespace QuanLyNhaSach.Models;

public partial class DonHang
{
    public int MaDonHang { get; set; }

    public int MaNguoiDung { get; set; }

    public DateTime NgayDat { get; set; }

    public string HoTenNguoiNhan { get; set; } = null!;

    public string SoDienThoaiNhan { get; set; } = null!;

    public decimal TongTien { get; set; }

    public decimal PhiShip { get; set; }

    public string TrangThaiDonHang { get; set; } = null!;

    public string? GhiChu { get; set; }

    public int? MaKhuyenMai { get; set; }

    public int? MaXaGiao { get; set; }

    public string? SoNhaGiao { get; set; }

    public string? DuongGiao { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual KhuyenMai? MaKhuyenMaiNavigation { get; set; }

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;

    public virtual XaPhuongCuBackup? MaXaGiaoNavigation { get; set; }

    public virtual ThanhToan? ThanhToan { get; set; }
}
