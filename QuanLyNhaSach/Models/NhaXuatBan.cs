using System;
using System.Collections.Generic;

namespace QuanLyNhaSach.Models;

public partial class NhaXuatBan
{
    public int MaNhaXuatBan { get; set; }

    public string TenNhaXuatBan { get; set; } = null!;

    public string? DiaChi { get; set; }

    public string? SoDienThoai { get; set; }

    public string? Email { get; set; }

    public string? Website { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
