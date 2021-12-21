using Npgsql;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Windows;

namespace QuanLyNhanVien.Models
{
    [Table("nhan_vien", Schema = "public")]
    public class NhanVienTest
    {
        [Key]
        public String ma_nhan_vien { get; set; }

        [Required(ErrorMessage = "Tên nhân viên không được để trống")]
        [MaxLength(500, ErrorMessage = "Tên nhân viên quá dài")]
       
        public string ho_va_ten { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/M/yyyy}")]
      
        public DateTime ngay_sinh { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(10, ErrorMessage = "Số điện thoại không tồn tại")]

        public string so_dien_thoai { get; set; }

        public string dia_chi { get; set; }

        [Required(ErrorMessage = "Chức vụ không được để trống")]
        public string chuc_vu { get; set; }

       
        public string so_nam_cong_tac { get; set; }
    }

#pragma warning disable S1118 // Utility classes should not have public constructors
   
}