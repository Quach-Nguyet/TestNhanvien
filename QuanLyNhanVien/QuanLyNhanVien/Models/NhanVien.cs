using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLyNhanVien.Models
{
    public class NhanVien
    {
        public String MaNhanVien { get; set; }

        [Required(ErrorMessage = "Tên nhân viên không được để trống")]
        [MaxLength(500, ErrorMessage = "Tên nhân viên quá dài")]
        public string HoVaTen { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        [DataType(DataType.Date)]
        public DateTime NgaySinh { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(10, ErrorMessage = "Số điện thoại không tồn tại")]
        public string SoDienThoai { get; set; }

        public string DiaChi { get; set; }

        [Required(ErrorMessage = "Chức vụ không được để trống")]
        public string ChucVu { get; set; }

        public string SoNamCongTac { get; set; }
    }
}