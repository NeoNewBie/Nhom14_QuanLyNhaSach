using System;
using System.Collections.Generic;

namespace QuanLyNhaSach.Models;

public partial class DanhMuc
{
    public int MaDanhMuc { get; set; }

    public string TenDanhMuc { get; set; } = null!;

    public string? MoTa { get; set; }

    public int? MaDanhMucCha { get; set; }

    public bool TrangThai { get; set; }

    public virtual ICollection<DanhMuc> InverseMaDanhMucChaNavigation { get; set; } = new List<DanhMuc>();

    public virtual DanhMuc? MaDanhMucChaNavigation { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
