using System;
using System.Collections.Generic;

namespace QuanLyNhaSach.Models;

public partial class SanPham
{
    public int MaSanPham { get; set; }

    public string TenSanPham { get; set; } = null!;

    public string? MoTa { get; set; }

    public decimal GiaBia { get; set; }

    public decimal GiaBan { get; set; }

    public int SoLuongTon { get; set; }

    public string? AnhBia { get; set; }

    public string LoaiSanPham { get; set; } = null!;

    public int MaDanhMuc { get; set; }

    public int? MaNhaXuatBan { get; set; }

    public bool TrangThai { get; set; }

    public DateTime NgayTao { get; set; }

    public decimal GiaNhap { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();

    public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();

    public virtual ICollection<DanhGium> DanhGia { get; set; } = new List<DanhGium>();

    public virtual DanhMuc MaDanhMucNavigation { get; set; } = null!;

    public virtual NhaXuatBan? MaNhaXuatBanNavigation { get; set; }

    public virtual ICollection<TacGium> MaTacGia { get; set; } = new List<TacGium>();
}
