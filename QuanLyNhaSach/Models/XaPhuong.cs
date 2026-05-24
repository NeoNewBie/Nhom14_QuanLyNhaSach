using System;
using System.Collections.Generic;

namespace QuanLyNhaSach.Models;

public partial class XaPhuong
{
    public int MaXa { get; set; }

    public string TenXa { get; set; } = null!;

    public int MaTinh { get; set; }

    public bool TrangThai { get; set; }

    public virtual TinhThanh MaTinhNavigation { get; set; } = null!;
}
