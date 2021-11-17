using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLyNhanVien.Models
{
    public class NhanVienCreateDate
    {
        public String MaNhanVien { get; set; }

        [Required(ErrorMessage = "Tên nhân viên không được để trống")]
        [MaxLength(500, ErrorMessage = "Tên nhân viên quá dài")]
        public string HoVaTen { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        public string NgaySinh { get; set; }


        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public string SoDienThoai { get; set; }

        public string DiaChi { get; set; }

        [Required(ErrorMessage = "Chức vụ không được để trống")]
        public string ChucVu { get; set; }

        public int? SoNamCongTac { get; set; }
    }
    public class NhanVien
    {
        public String MaNhanVien { get; set; }

        [Required(ErrorMessage = "Tên nhân viên không được để trống")]
        [MaxLength(500, ErrorMessage = "Tên nhân viên quá dài")]
        public string HoVaTen { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/M/yyyy}")]
        public DateTime NgaySinh { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public string SoDienThoai { get; set; }

        public string DiaChi { get; set; }

        [Required(ErrorMessage = "Chức vụ không được để trống")]
        public string ChucVu { get; set; }

        public int? SoNamCongTac { get; set; }
    }
}