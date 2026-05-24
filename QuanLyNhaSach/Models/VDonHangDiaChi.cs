using System;
using System.Collections.Generic;

namespace QuanLyNhaSach.Models;

public partial class VDonHangDiaChi
{
    public int MaDonHang { get; set; }

    public int MaNguoiDung { get; set; }

    public string? SoNhaGiao { get; set; }

    public string? DuongGiao { get; set; }

    public string? TenXa { get; set; }

    public string? TenTinh { get; set; }

    public string DiaChiGiaoDayDu { get; set; } = null!;
}
