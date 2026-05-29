using System.ComponentModel.DataAnnotations;
using QuanLyNhaSach.Models;

namespace QuanLyNhaSach.Validations;

public class GiaBanHopLeAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var sanPham = (SanPham)validationContext.ObjectInstance;

        if (sanPham.GiaBan <= 0)
        {
            return new ValidationResult("Giá bán phải lớn hơn 0.");
        }

        if (sanPham.GiaBan > sanPham.GiaBia)
        {
            return new ValidationResult("Giá bán không được lớn hơn giá bìa.");
        }

        return ValidationResult.Success;
    }
}
