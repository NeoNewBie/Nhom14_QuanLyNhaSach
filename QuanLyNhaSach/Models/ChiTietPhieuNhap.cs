using System;
using System.Collections.Generic;

namespace QuanLyNhaSach.Models;

public partial class ChiTietPhieuNhap
{
    public int MaPhieuNhap { get; set; }

    public int MaSanPham { get; set; }

    public int SoLuong { get; set; }

    public decimal GiaNhap { get; set; }

    public decimal? ThanhTien { get; set; }

    public virtual PhieuNhap MaPhieuNhapNavigation { get; set; } = null!;

    public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
}
