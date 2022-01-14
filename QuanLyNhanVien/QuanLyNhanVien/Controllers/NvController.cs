using QuanLyNhanVien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Npgsql;
using System.IO;
using OfficeOpenXml;
using System.Web.UI.WebControls;
using OfficeOpenXml.Table;
using System.Drawing;
using OfficeOpenXml.Style;
using Dapper.FastCrud;

namespace QuanLyNhanVien.Controllers
{
    public class NvController : Controller
    {
        private readonly string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=123;Database=QuanLyNhanVien;";

        // GET: Staff
        public ActionResult Index()
        {
            return View("Index");
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
            var dsNhanVien = Pagination(1, getSumRows());
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
            OrmConfiguration.DefaultDialect = SqlDialect.PostgreSql;
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                conn.Insert<NhanVien>(nv);
            }
            return Json(new { success = true, status = true, data = nv, JsonRequestBehavior.AllowGet });
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var dsNhanVien = Pagination(1, getSumRows());
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
            var dsNhanVien = Pagination(1, getSumRows());
            var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == nvNew.MaNhanVien);
            object ketQua = null;
            if (nv == null)
            {
                ketQua = new
                {
                    success = false,
                    message = "*Mã nhân viên không tồn tại",
                    status = false
                };
            }
            if (ketQua != null) return Json(ketQua);
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
                var test = conn.Update<NhanVien>(nv);
                if (!test)
                {
                    ketQua = new
                    {
                        success = false,
                        status = false,
                        message = "Dữ liệu chưa được update"
                    };
                }
                if (ketQua != null) return Json(ketQua);
            }
            return Json(new { success = true, status = true, data = dsNhanVien });

        }

        [HttpGet]
        public ActionResult Delete(Guid ma)
        {
            object ketQua = null;
            var dsNhanVien = Pagination(1, getSumRows()); ;
            var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == ma);
            if (nv == null)
            {
                ketQua = new
                {
                    success = false,
                    message = "*Mã nhân viên không tồn tại",
                    status = false
                };
            }
            if (ketQua != null) return Json(ketQua);
            dsNhanVien.Remove(nv);
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var test = conn.Delete<NhanVien>(nv);
                if (!test)
                {
                    ketQua = new
                    {
                        success = false,
                        status = false,
                        message = "Dữ liệu chưa được update"
                    };
                }
                if (ketQua != null) return Json(ketQua);
            }
            return Json(new { success = true, status = true, data = dsNhanVien }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Table(int page = 1, int page_size = 5, int PhongBanId = 0, string keyword = "")
        {
            keyword = keyword.ToLower();
            var dsNhanVien = Pagination(page, page_size, PhongBanId, keyword);
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                string where = " 1=1 ";

                if (PhongBanId != 0)
                {
                    where += $" AND \"{nameof(NhanVien.PhongBan):C}\"={PhongBanId}";
                }

                if (!string.IsNullOrEmpty(keyword))
                {
                    where += $" AND ({getProperties(keyword)})";
                }
                ViewBag.TotalPages = getPage(page_size, where);
                ViewBag.Stt = getStt(page, page_size);
            }
            return PartialView("_DanhSachNV", dsNhanVien);
        }

        [HttpGet]
        public ActionResult Export(int page = 1, int page_size = 5, int PhongBanId = 0, string keyword = "")
        {
            var listNhanVien = Pagination(page, page_size, PhongBanId, keyword);
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

        private int getStt(int page = 1, int page_size = 5)
        {
            return (page - 1) * page_size + 1;
        }

        [HttpGet]
        public JsonResult getPhongBan()
        {
            List<PhongBan> data;
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                data = (List<PhongBan>)conn.Find<PhongBan>();
            }
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        #region private_method
        private int getSumRows()
        {
            OrmConfiguration.DefaultDialect = SqlDialect.PostgreSql;
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                int rows = conn.Count<NhanVien>();
                return rows;
            }
        }

        private int getPage(int page_size = 1, string where = "")
        {
            int rows = 0;
            OrmConfiguration.DefaultDialect = SqlDialect.PostgreSql;
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                rows = conn.Count<NhanVien>(nv => nv
                                            .Include<PhongBan>(join => join.InnerJoin())
                                            .Where($"{where}"));

            }
            return (int)Math.Ceiling((decimal)rows / page_size);
        }

        private List<NhanVien> Pagination(int page = 1, int page_size = 5, int PhongBanId = 0, string keyword = "")
        {
            keyword = keyword.ToLower();
            OrmConfiguration.DefaultDialect = SqlDialect.PostgreSql;
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                string condition = "1=1";
                if (page < 1) return new List<NhanVien>();
                _ = new List<NhanVien>();
                List<NhanVien> dsNhanVien;
                int skip = page_size != 0 ? (page - 1) * page_size : 0;
                if (PhongBanId != 0)
                {
                    if (keyword != "")
                    {
                        condition += $" AND \"{nameof(NhanVien.PhongBan):C}\"={PhongBanId} AND ({getProperties(keyword)})";
                    }
                    else
                    {
                        condition += $" AND \"{nameof(NhanVien.PhongBan):C}\"={PhongBanId}";
                    }
                }
                else if (keyword != "")
                {
                    condition += $" AND ({getProperties(keyword)})";
                }
                dsNhanVien = conn.Find<NhanVien>(nv => nv.Include<PhongBan>(join => join.InnerJoin()).Where($"{condition}").Top(page_size).Skip(skip).OrderBy($"{nameof(NhanVien.HoVaTen):C} ASC")).ToList();
                return dsNhanVien;
            }
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
            worksheet.Cells[1, 9].Value = "Tên phòng ban/chức vụ";

            using (var range = worksheet.Cells["A1:I1"])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.Font.SetFromFont(new System.Drawing.Font("Times new roman", 14));
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                range.Style.Border.Bottom.Color.SetColor(Color.Gray);
            }

            for (int i = 0; i < listNhanVien.Count; i++)
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
                worksheet.Cells[i + 2, 9].Value = item.phong_ban.ten_phong_ban;
            }
        }

        private string getProperties(string keyword ="")
        {
            string where = "";
            OrmConfiguration.DefaultDialect = SqlDialect.PostgreSql;
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                where += $" LOWER(\"{nameof(NhanVien.HoVaTen)}\") LIKE '%{keyword}%' " +
                         $"OR LOWER(\"{nameof(NhanVien.DiaChi)}\") LIKE '%{keyword}%' " +
                         $"OR LOWER(\"{nameof(NhanVien.ChucVu)}\") LIKE '%{keyword}%' " +
                         $"OR LOWER(\"{nameof(NhanVien.SoDienThoai)}\") LIKE '%{keyword}%' " +
                         $"OR TO_CHAR(\"{nameof(NhanVien.NgaySinh)}\",'DD/MM/YYY') LIKE '%{keyword}%' " +
                         $"OR \"{nameof(NhanVien.SoNamCongTac)}\"::TEXT LIKE '%{keyword}%' " +
                         $"OR LOWER({nameof(NhanVien.phong_ban.ten_phong_ban)}) LIKE '%{keyword}%' ";
                }
                return where;
            
        }
        #endregion
    }
}