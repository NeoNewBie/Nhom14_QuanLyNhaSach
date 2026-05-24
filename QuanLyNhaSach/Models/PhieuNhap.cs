using System;
using System.Collections.Generic;

namespace QuanLyNhaSach.Models;

public partial class PhieuNhap
{
    public int MaPhieuNhap { get; set; }

    public int MaNguoiDung { get; set; }

    public int MaNhaCungCap { get; set; }

    public DateTime NgayNhap { get; set; }

    public decimal TongTien { get; set; }

    public string? GhiChu { get; set; }

    public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;

    public virtual NhaCungCap MaNhaCungCapNavigation { get; set; } = null!;
}
