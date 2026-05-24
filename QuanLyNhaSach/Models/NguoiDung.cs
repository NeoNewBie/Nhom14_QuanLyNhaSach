using System;
using System.Collections.Generic;

namespace QuanLyNhaSach.Models;

public partial class NguoiDung
{
    public int MaNguoiDung { get; set; }

    public string HoTen { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string SoDienThoai { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public bool? GioiTinh { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public bool TrangThai { get; set; }

    public int MaVaiTro { get; set; }

    public DateTime NgayTao { get; set; }

    public int? MaXa { get; set; }

    public string? SoNha { get; set; }

    public string? Duong { get; set; }

    public virtual ICollection<DanhGium> DanhGia { get; set; } = new List<DanhGium>();

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual GioHang? GioHang { get; set; }

    public virtual VaiTro MaVaiTroNavigation { get; set; } = null!;

    public virtual XaPhuongCuBackup? MaXaNavigation { get; set; }

    public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; } = new List<PhieuNhap>();
}
