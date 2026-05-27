using System;
using System.Collections.Generic;

namespace QuanLyNhaSach.Models;

public partial class ThanhToan
{
    public int MaThanhToan { get; set; }

    public int MaDonHang { get; set; }

    public string PhuongThucThanhToan { get; set; } = null!;

    public decimal SoTienThanhToan { get; set; }

    public string TrangThaiThanhToan { get; set; } = null!;

    public string? MaGiaoDich { get; set; }

    public DateTime? NgayThanhToan { get; set; }

    public virtual DonHang MaDonHangNavigation { get; set; } = null!;
}
