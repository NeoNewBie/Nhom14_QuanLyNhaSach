using System;
using System.Collections.Generic;

namespace QuanLyNhaSach.Models;

public partial class TinhThanhCuBackup
{
    public int MaTinh { get; set; }

    public string TenTinh { get; set; } = null!;

    public bool TrangThai { get; set; }

    public virtual ICollection<XaPhuongCuBackup> XaPhuongCuBackups { get; set; } = new List<XaPhuongCuBackup>();
}
