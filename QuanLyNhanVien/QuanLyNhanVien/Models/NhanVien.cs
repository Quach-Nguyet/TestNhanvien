using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhanVien.Models
{
    [Table("nhan_vien", Schema = "public")]
    public class NhanVien: BaseEntity
    {
        public static object NhanVienRepository { get; internal set; }
        [Key]
        public Guid MaNhanVien { get; set; }

        [Required(ErrorMessage = "Tên nhân viên không được để trống")]
        [MaxLength(500, ErrorMessage = "Tên nhân viên quá dài")]
        public string HoVaTen { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime NgaySinh { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(10, ErrorMessage = "Số điện thoại không tồn tại")]
        public string SoDienThoai { get; set; }

        public string DiaChi { get; set; }

        [Required(ErrorMessage = "Chức vụ không được để trống")]
        public string ChucVu { get; set; }

        public int SoNamCongTac { get; set; }
        [ForeignKey(nameof(phong_ban))]
        public int PhongBan { get; set; }
        
        public PhongBan phong_ban { get; set; }

    }
    public abstract class BaseEntity
    {
    }
    [Table("phong_ban", Schema = "public")]
    public class PhongBan
    {
        [Key]
        public int id { get; set; }
        public string ten_phong_ban { get; set; }
    }
}