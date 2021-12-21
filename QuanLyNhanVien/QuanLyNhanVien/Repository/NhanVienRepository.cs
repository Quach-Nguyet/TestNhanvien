using Dapper;
using Npgsql;
using QuanLyNhanVien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyNhanVien.Repository
{
    public class NhanVienRepository : IRepository<NhanVien>
    {
        private readonly NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=123;Database=QuanLyNhanVien;");
        public void Add(NhanVien item)
        {
            using (conn)
            {
                conn.Open();
                conn.Execute("INSERT INTO nhan_vien(MaNhanVien, HoVaTen, NgaySinh, SoDienThoai, DiaChi, ChucVu, SoNamCongTac) " +
                    "VALUES(@MaNhanVien, @HoVaTen, @NgaySinh, @SoDienThoai, @DiaChi, @ChucVu, @SoNamCongTac)", item);
            }
        }

        public IEnumerable<NhanVien> FindAll()
        {
            conn.Open();
            return conn.Query<NhanVien>("SELECT * FROM public.nhan_vien").ToList();
        }

        public NhanVien FindByID(string MaNhanVien)
        {
            throw new NotImplementedException();
        }

        public void Remove(string MaNhanVien)
        {
            conn.Open();
            conn.Execute("DELETE FROM nhan_vien WHERE MaNhanVien=@MaNhanVien", new { MaNhanVien });
        }

        public void Update(NhanVien item)
        {
            conn.Open();
            conn.Execute("UPDATE nhan_vien SET MaNhanVien = @MaNhanVien, HoVaTen = @HoVaTen, NgaySinh = @NgaySinh, SoDienThoai = @SoDienThoai, DiaChi = @DiaChi, ChucVu = @ChucVu, SoNamCongTac = @SoNamCongTac" +
                " WHERE MaNhanVien=@MaNhanVien", item);
        }
    }
}