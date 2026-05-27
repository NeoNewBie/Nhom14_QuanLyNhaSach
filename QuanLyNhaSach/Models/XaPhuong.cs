using System;
using System.Collections.Generic;

namespace QuanLyNhaSach.Models;

public partial class XaPhuong
{
    public int MaXa { get; set; }

    public string TenXa { get; set; } = null!;

    public int MaTinh { get; set; }

    public bool TrangThai { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual TinhThanh MaTinhNavigation { get; set; } = null!;

    public virtual ICollection<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
}
