using System;
using System.Collections.Generic;

namespace QuanLyNhaSach.Models;

public partial class TinhThanh
{
    public int MaTinh { get; set; }

    public string TenTinh { get; set; } = null!;

    public bool TrangThai { get; set; }

    public virtual ICollection<XaPhuong> XaPhuongs { get; set; } = new List<XaPhuong>();
}
