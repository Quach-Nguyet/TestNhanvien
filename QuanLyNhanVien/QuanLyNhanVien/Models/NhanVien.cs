using Npgsql;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Windows;

namespace QuanLyNhanVien.Models
{
    [Table("nhan_vien", Schema = "public")]
    public class NhanVien: BaseEntity
    {
        public static object NhanVienRepository { get; internal set; }
        [Key]
        [Column("ma_nhan_vien")]
        public Guid MaNhanVien { get; set; }

        [Required(ErrorMessage = "Tên nhân viên không được để trống")]
        [MaxLength(500, ErrorMessage = "Tên nhân viên quá dài")]
        [Column("ho_va_ten")]
        public string HoVaTen { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/M/yyyy}")]
        [Column("ngay_sinh")]
        public DateTime NgaySinh { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(10, ErrorMessage = "Số điện thoại không tồn tại")]
        [Column("so_dien_thoai")]
        public string SoDienThoai { get; set; }

        [Column("dia_chi")]
        public string DiaChi { get; set; }

        [Required(ErrorMessage = "Chức vụ không được để trống")]
        [Column("chuc_vu")]
        public string ChucVu { get; set; }

        [Column("so_nam_cong_tac")]
        public int SoNamCongTac { get; set; }
    }

    public abstract class BaseEntity
    {
    }
  // public class Connection
    //  {
    //#pragma warning disable S2223 // Non-constant static fields should not be visible
    //        protected static SqlConnection conn;
    //#pragma warning restore S2223 // Non-constant static fields should not be visible
    //        private const string connStr = "Server=localhost;Port=5432;User Id=postgres;Password=123;Database=QuanLyNhanVien;";

    //        public static SqlConnection getConnection()
    //        {
    //            if (conn == null)
    //            {
    //                conn = new SqlConnection(connStr);
    //                conn.Open();
    //            }
    //            return conn;
    //        }

    //        public static implicit operator NpgsqlConnection(Connection v)
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }
}