using System;
using System.Collections.Generic;

namespace QuanLyNhaSach.Models;

public partial class VNguoiDungDiaChi
{
    public int MaNguoiDung { get; set; }

    public string HoTen { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string SoDienThoai { get; set; } = null!;

    public string? SoNha { get; set; }

    public string? Duong { get; set; }

    public string? TenXa { get; set; }

    public string? TenTinh { get; set; }

    public string DiaChiDayDu { get; set; } = null!;
}
