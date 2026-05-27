using System;
using System.Collections.Generic;

namespace QuanLyNhaSach.Models;

public partial class TacGium
{
    public int MaTacGia { get; set; }

    public string TenTacGia { get; set; } = null!;

    public string? QuocTich { get; set; }

    public string? MoTa { get; set; }

    public virtual ICollection<SanPham> MaSanPhams { get; set; } = new List<SanPham>();
}
