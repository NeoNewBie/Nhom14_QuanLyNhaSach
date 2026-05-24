using System;
using System.Collections.Generic;

namespace QuanLyNhaSach.Models;

public partial class KhuyenMai
{
    public int MaKhuyenMai { get; set; }

    public string TenKhuyenMai { get; set; } = null!;

    public string LoaiGiamGia { get; set; } = null!;

    public decimal GiaTriGiam { get; set; }

    public decimal? DieuKienApDung { get; set; }

    public DateTime NgayBatDau { get; set; }

    public DateTime NgayKetThuc { get; set; }

    public bool TrangThai { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
