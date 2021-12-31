using QuanLyNhanVien.Extensions;
using QuanLyNhanVien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dapper;
using Npgsql;
using QuanLyNhanVien.Repository;
using System.IO;
using OfficeOpenXml;
using System.Web.UI.WebControls;
using OfficeOpenXml.Table;
using System.Drawing;
using OfficeOpenXml.Style;

namespace QuanLyNhanVien.Controllers
{
    public class NvController : Controller
    {
        private readonly string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=123;Database=QuanLyNhanVien;";

        // GET: Staff
        public ActionResult Index()

        {
            List<NhanVien> dsNhanVien = ConnectList();
            return View("Index", dsNhanVien);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(NhanVien nv)
        {
            object ketQua = null;
            var dsNhanVien = ConnectList();
            nv.MaNhanVien = Guid.NewGuid();
            nv.HoVaTen = Fomart.Fomartstring(nv.HoVaTen);
            nv.DiaChi = Fomart.Fomartstring(nv.DiaChi);
            nv.ChucVu = Fomart.Fomartstring(nv.ChucVu);

            
            bool TestPhone = Int32.TryParse(nv.SoDienThoai, out _);
            if (!TestPhone || nv.SoDienThoai.Length != 10)
            {
                ketQua = new
                {
                    status = false,
                    message = "* Số điện thoại không tồn tại. yêu cầu nhập lại "
                };
            }
            if (ketQua != null) return Json(ketQua);

            
            foreach (var item in dsNhanVien)
            {
                if (nv.HoVaTen == item.HoVaTen)
                {
                    ketQua = new
                    {
                        success = false,
                        message = "*Tên đã có. Yêu cầu nhập lại",
                        status = false
                    };
                }
                if (nv.SoDienThoai == item.SoDienThoai)
                {
                    ketQua = new
                    {
                        success = false,
                        message = "*Số điện thoại đã có. Yêu cầu nhập lại",
                        status = false
                    
                    };
                }
            }
            if (ketQua != null) return Json(ketQua);

            dsNhanVien.Add(nv);
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var Insert = conn.Execute("INSERT INTO public.nhan_vien (\"MaNhanVien\",\"HoVaTen\",\"NgaySinh\",\"SoDienThoai\",\"DiaChi\",\"ChucVu\",\"SoNamCongTac\",\"PhongBan\") " +
                                          "VALUES(@MaNhanVien,@HoVaTen,@NgaySinh,@SoDienThoai,@DiaChi,@ChucVu,@SoNamCongTac,@PhongBan)", nv);
                if (Insert == 0 )
                {

                    ketQua = new
                    {
                        success = false,
                        message = "*Dữ liệu chưa được update",
                        status = false

                    };

                }
                if (ketQua != null) return Json(ketQua);
            }
            return Json(new { success = true, status = true, data = nv, JsonRequestBehavior.AllowGet });
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var dsNhanVien = ConnectList();
            var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == id);
            return Json(new
            {
                status = true,
                success = true,
                data = nv
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(NhanVien nvNew)
        {
            var dsNhanVien = ConnectList();
            var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == nvNew.MaNhanVien);
            object ketQua = null;
            foreach (var item in dsNhanVien)
            {
                if (nvNew.MaNhanVien != item.MaNhanVien)
                {
                    if (Fomart.Fomartstring(nvNew.HoVaTen) == item.HoVaTen)
                    {
                        ketQua = new
                        {
                            success = false,
                            message = "*Tên đã có. Yêu cầu nhập lại",
                            status = false
                        };
                    }
                    if (nvNew.SoDienThoai == item.SoDienThoai)
                    {
                        ketQua = new
                        {
                            success = false,
                            message = "*Số điện thoại đã có. Yêu cầu nhập lại",
                            status = false
                        };
                    }
                }
            }
            if (ketQua != null) return Json(ketQua);
            bool TestPhone = Int32.TryParse(nvNew.SoDienThoai, out int Phone);
            if (!TestPhone && nvNew.SoDienThoai.Length != 10)
            {
                ketQua = new
                {
                    success = false,
                    status = false,
                    message = "* Số điện thoại không tồn tại. yêu cầu nhập lại "
                };
            }
            if (ketQua != null) return Json(ketQua);
            nv.MaNhanVien = nvNew.MaNhanVien;
            nv.HoVaTen = Fomart.Fomartstring(nvNew.HoVaTen);
            nv.NgaySinh = nvNew.NgaySinh;
            nv.SoDienThoai = nvNew.SoDienThoai;
            nv.DiaChi = Fomart.Fomartstring(nvNew.DiaChi);
            nv.ChucVu = Fomart.Fomartstring(nvNew.ChucVu);
            nv.SoNamCongTac = nvNew.SoNamCongTac;
            nv.PhongBan = nvNew.PhongBan;

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var Update = conn.Execute("UPDATE public.nhan_vien SET \"MaNhanVien\" = @MaNhanVien, \"HoVaTen\" = @HoVaTen, \"NgaySinh\" = @NgaySinh, \"SoDienThoai\" = @SoDienThoai, \"DiaChi\" = @DiaChi, \"ChucVu\" = @ChucVu, \"SoNamCongTac\" = @SoNamCongTac," +
                " \"PhongBan\" = @PhongBan WHERE \"MaNhanVien\" = @MaNhanVien", nv);
                if (Update == 0)
                {

                    ketQua = new
                    {
                        success = false,
                        message = "*Dữ liệu chưa được update",
                        status = false

                    };

                }
                if (ketQua != null) return Json(ketQua);
            }
            return Json(new { success = true, status = true, data = dsNhanVien });

        }
        [HttpGet]
        public ActionResult Delete(Guid ma)
        {
            var dsNhanVien = Pagination();
            var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == ma);
            dsNhanVien.Remove(nv);
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                conn.Execute("DELETE FROM public.nhan_vien WHERE \"MaNhanVien\" = @ma", new { ma });
            }
            return Json(new { success = true, status = true, data=dsNhanVien}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Table(int page = 1, int page_size = 5, int Id = 0, string keyword = "")
        {
           keyword = keyword.ToLower();
            int rows;
            var dsNhanVien = Pagination(page, page_size, Id, keyword);
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                if (Id != 0 && keyword!="")
                {
                    rows = conn.Query<int>("SELECT COUNT(*) FROM public.nhan_vien WHERE \"PhongBan\" = @Id " +
                        "AND (LOWER(\"HoVaTen\") LIKE '%" + keyword + "%' " +
                        "OR \"NgaySinh\"::TEXT LIKE '%" + keyword + "%' " +
                        "OR LOWER(\"DiaChi\") LIKE '%" + keyword + "%' " +
                        "OR \"SoDienThoai\" LIKE '%" + keyword + "%' " +
                        "OR LOWER(\"ChucVu\") LIKE '%" + keyword + "%' " +
                        "OR \"SoNamCongTac\"::TEXT LIKE '%" + keyword + "%') ",new {Id}).FirstOrDefault();
                }
                else if (keyword != ""){
                    rows = conn.Query<int>("SELECT COUNT(*) FROM public.nhan_vien WHERE " +
                        "LOWER(\"HoVaTen\") LIKE '%" + keyword + "%' " +
                        "OR \"NgaySinh\"::TEXT LIKE '%" + keyword + "%' " +
                        "OR LOWER(\"DiaChi\") LIKE '%" + keyword + "%' " +
                        "OR \"SoDienThoai\" LIKE '%" + keyword + "%' " +
                        "OR LOWER(\"ChucVu\") LIKE '%" + keyword + "%' " +
                        "OR \"SoNamCongTac\"::TEXT LIKE '%" + keyword + "%' ").FirstOrDefault();
                }

                else if(Id != 0)
                {
                    rows = conn.Query<int>("SELECT COUNT(*) FROM public.nhan_vien WHERE \"PhongBan\"=@Id", new { Id }).FirstOrDefault();
                }
                else
                {
                    rows = conn.Query<int>("SELECT COUNT(*) FROM public.nhan_vien").FirstOrDefault();
                }
                ViewBag.TotalPages = Math.Ceiling((double)rows / page_size);
                
            }
            return PartialView("_DanhSachNV", dsNhanVien);
        }

        public ActionResult DropDow()
        {
            List<PhongBan> room;
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                room = conn.Query<PhongBan>("SELECT * FROM public.phong_ban").ToList();
                ViewBag.room = room;
            }
            return PartialView("_DropDown", room);
        }

        [HttpGet]
        public ActionResult Export(int Id = 0, string keyword = "")
        {
            var listNhanVien = ConnectList(Id, keyword);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var excelPackage = new ExcelPackage(new MemoryStream()))
            {
                excelPackage.Workbook.Worksheets.Add("First Sheet");
                var workSheet = excelPackage.Workbook.Worksheets["First Sheet"];
                workSheet.Cells[1, 1].LoadFromCollection(listNhanVien, true, TableStyles.Dark9);
                FormatForExcel(workSheet, listNhanVien);
                return File(excelPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "QuanLyNhanVien.xlsx");
            }
        }

        #region
        private List<NhanVien> Pagination(int page = 1, int page_size = 5, int Id = 0, string keyword = "")
        {
            keyword = keyword.ToLower();
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                if (page < 1) return new List<NhanVien>();
                _ = new List<NhanVien>();
                List<NhanVien> dsNhanVien;
                if (Id != 0 && keyword != "")
                {
                    dsNhanVien = conn.Query<NhanVien>("SELECT * FROM public.nhan_vien WHERE \"PhongBan\" = @Id " +
                        "AND (LOWER(\"HoVaTen\") LIKE '%" + keyword + "%' " +
                        "OR \"NgaySinh\"::TEXT LIKE '%" + keyword + "%' " +
                        "OR LOWER(\"DiaChi\") LIKE '%" + keyword + "%' " +
                        "OR \"SoDienThoai\" LIKE '%" + keyword + "%' " +
                        "OR LOWER(\"ChucVu\") LIKE '%" + keyword + "%' " +
                        "OR \"SoNamCongTac\"::TEXT LIKE '%" + keyword + "%' " +
                        ") ORDER BY \"HoVaTen\" ASC OFFSET((@page - 1)*@page_size) LIMIT @page_size ", new
                    {
                        page,
                        page_size,
                        Id
                    }).ToList();

                }
                else if(keyword !="")
                {
                    dsNhanVien = conn.Query<NhanVien>("SELECT * FROM public.nhan_vien WHERE " +
                        "LOWER(\"HoVaTen\") LIKE '%"+keyword+"%' " +
                        "OR \"NgaySinh\"::TEXT LIKE '%" + keyword + "%' " +
                        "OR LOWER(\"DiaChi\") LIKE '%" + keyword + "%' " +
                        "OR \"SoDienThoai\" LIKE '%" + keyword + "%' " +
                        "OR LOWER(\"ChucVu\") LIKE '%" + keyword + "%' " +
                        "OR \"SoNamCongTac\"::TEXT LIKE '%" + keyword + "%' " +
                        "ORDER BY \"HoVaTen\" ASC OFFSET((@page - 1)*@page_size) LIMIT @page_size ", new
                    {
                        page,
                        page_size,
                    }).ToList();
                }
                else if(Id!=0){
                    dsNhanVien = conn.Query<NhanVien>("SELECT * FROM public.nhan_vien WHERE \"PhongBan\" = @Id "+
                        "ORDER BY \"HoVaTen\" ASC OFFSET((@page - 1)*@page_size) LIMIT @page_size ", new
                        {
                            page,
                            page_size,
                            Id
                        }).ToList();
                }
                else
                {
                    dsNhanVien = conn.Query<NhanVien>("SELECT * FROM public.nhan_vien ORDER BY \"HoVaTen\" ASC OFFSET((@page - 1)*@page_size) LIMIT @page_size", new
                    {
                        page,
                        page_size
                    }).ToList();

                }
                foreach (var nhanVien in dsNhanVien)
                {
                    int phong_ban_id = nhanVien.PhongBan;
                    nhanVien.phong_ban = conn.Query<PhongBan>("select * from public.phong_ban where \"id\" = @phong_ban_id", new { phong_ban_id }).FirstOrDefault();
                }
                return dsNhanVien;
            }
        }

        private List<NhanVien> ConnectList(int Id = 0, string keyword="")
        {
            List<NhanVien> dsNhanVien;
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {

                conn.Open();
                if (Id!=0 && keyword != "")
                {
                    dsNhanVien = conn.Query<NhanVien>("SELECT * FROM public.nhan_vien WHERE \"PhongBan\" = @Id " +
                           "AND (LOWER(\"HoVaTen\") LIKE '%" + keyword + "%' " +
                           "OR \"NgaySinh\"::TEXT LIKE '%" + keyword + "%' " +
                           "OR LOWER(\"DiaChi\") LIKE '%" + keyword + "%' " +
                           "OR \"SoDienThoai\" LIKE '%" + keyword + "%' " +
                           "OR LOWER(\"ChucVu\") LIKE '%" + keyword + "%' " +
                           "OR \"SoNamCongTac\"::TEXT LIKE '%" + keyword + "%' " +
                           ") ORDER BY \"HoVaTen\" ASC ", new{Id}).ToList();

                }
                else if(Id != 0) dsNhanVien = conn.Query<NhanVien>("SELECT * FROM public.nhan_vien  WHERE \"PhongBan\" = @Id ORDER BY \"HoVaTen\" ASC", new { Id }).ToList();
                else if (keyword != "")
                {
                    dsNhanVien = conn.Query<NhanVien>("SELECT * FROM public.nhan_vien WHERE " +
                        "LOWER(\"HoVaTen\") LIKE '%" + keyword + "%' " +
                        "OR \"NgaySinh\"::TEXT LIKE '%" + keyword + "%' " +
                        "OR LOWER(\"DiaChi\") LIKE '%" + keyword + "%' " +
                        "OR \"SoDienThoai\" LIKE '%" + keyword + "%' " +
                        "OR LOWER(\"ChucVu\") LIKE '%" + keyword + "%' " +
                        "OR \"SoNamCongTac\"::TEXT LIKE '%" + keyword + "%' " +
                        "ORDER BY \"HoVaTen\" ASC ").ToList();
                }
                else dsNhanVien = conn.Query<NhanVien>("SELECT * FROM public.nhan_vien ORDER BY \"HoVaTen\" ASC").ToList();
                foreach (var nhanVien in dsNhanVien)
                {
                    int phong_ban_id = nhanVien.PhongBan;
                    nhanVien.phong_ban = conn.Query<PhongBan>("select * from public.phong_ban where \"id\" = @phong_ban_id", new { phong_ban_id }).FirstOrDefault();
                }
            }
            return dsNhanVien;
        }

        private void FormatForExcel(ExcelWorksheet worksheet, List<NhanVien> listNhanVien)
        {
            worksheet.DefaultColWidth = 20;
            worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells.Style.WrapText = true;
            worksheet.Cells[1, 1].Value = "STT";
            worksheet.Cells[1, 2].Value = "Mã hhân viên";
            worksheet.Cells[1, 3].Value = "Họ và tên";
            worksheet.Cells[1, 4].Value = "Ngày sinh";
            worksheet.Cells[1, 5].Value = "Số điện thoại";
            worksheet.Cells[1, 6].Value = "Địa chỉ";
            worksheet.Cells[1, 7].Value = "Chức Vụ";
            worksheet.Cells[1, 8].Value = "Số năm công tác";
            worksheet.Cells[1, 9].Value = "Id Phòng Ban";

            using (var range = worksheet.Cells["A1:I1"])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.Font.SetFromFont(new System.Drawing.Font("Times new roman", 14));
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                range.Style.Border.Bottom.Color.SetColor(Color.Gray);
            }

            for (int i=0; i < listNhanVien.Count; i++)
            {
                var item = listNhanVien[i];
                worksheet.Cells[i + 2, 1].Value = i + 1;
                worksheet.Cells[i + 2, 2].Value = item.MaNhanVien;
                worksheet.Cells[i + 2, 3].Value = item.HoVaTen;
                worksheet.Cells[i + 2, 4].Style.Numberformat.Format = "dd-mm-yyyy";
                worksheet.Cells[i + 2, 4].Value = item.NgaySinh;
                worksheet.Cells[i + 2, 5].Value = item.SoDienThoai;
                worksheet.Cells[i + 2, 6].Value = item.DiaChi;
                worksheet.Cells[i + 2, 7].Value = item.ChucVu;
                worksheet.Cells[i + 2, 8].Value = item.SoNamCongTac;
                worksheet.Cells[i + 2, 9].Value = item.PhongBan;
            }
        }

        [HttpGet]
        public JsonResult getPhongBan()
        {
            List<PhongBan> data;
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                data = conn.Query<PhongBan>("SELECT * FROM public.phong_ban").ToList();
            }
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}