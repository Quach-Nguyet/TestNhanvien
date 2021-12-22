using QuanLyNhanVien.Extensions;
using QuanLyNhanVien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dapper;
using Npgsql;
using System.Data;
using System.Data.SqlClient;
using QuanLyNhanVien.Repository;

namespace QuanLyNhanVien.Controllers
{
    public class NvController : Controller
    {
        private readonly string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=123;Database=QuanLyNhanVien;";
        private readonly string DANH_SACH_NHAN_VIEN = "DanhSachNhanVien";
        //  private readonly NhanVienRepository nhanVienRepository;

        public List<NhanVien> ConnectList()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                List<NhanVien> dsNhanVien = conn.Query<NhanVien>("SELECT * FROM public.nhan_vien ORDER BY \"HoVaTen\" ASC").ToList();
                if (dsNhanVien == null)
                {
                    dsNhanVien = new List<NhanVien>();
                }
                return dsNhanVien;
            }

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
        //public List<NhanVien> Pagination(int page=1, int page_size = 5, string name = null)
        //{
        //    using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        if (page < 1) return new List<NhanVien>();
        //        ViewBag.NameRoom = conn.Query<string>("SELECT ten_phong_ban FROM public.phong_ban").ToList();
        //        //ViewBag.IdRoom = conn.Query<int>("SELECT COUNT(*) FROM public.phong_ban").FirstOrDefault();
        //       int idRoom = conn.Query<int>("SELECT id FROM public.phong_ban WHERE ten_phong_ban = @name",new { name }).FirstOrDefault();
        //        ViewBag.IdRoom = idRoom;
        //        if (idRoom != 0)
        //        {
        //            List<NhanVien>  dsNhanVien = conn.Query<NhanVien>("SELECT * FROM public.nhan_vien WHERE \"PhongBan\" = @idRoom ORDER BY \"HoVaTen\" ASC OFFSET((@page - 1)*@page_size) LIMIT @page_size ", new
        //            {
        //                page,
        //                page_size,
        //                idRoom 
        //            }).ToList();
        //            return dsNhanVien;
        //        }
        //        else
        //        {
        //            List<NhanVien> dsNhanVien = conn.Query<NhanVien>("SELECT * FROM public.nhan_vien ORDER BY \"HoVaTen\" ASC OFFSET((@page - 1)*@page_size) LIMIT @page_size", new
        //            {
        //                page = page,
        //                page_size = page_size
        //            }).ToList();
        //            return dsNhanVien;
        //        }

        //    }
        //}
        public List<NhanVien> Pagination(int page=1, int page_size = 5, int Id = 0)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                if (page < 1) return new List<NhanVien>();
                ViewBag.NameRoom = conn.Query<PhongBan>("SELECT ten_phong_ban FROM public.phong_ban").ToList();
                if (Id != 0)
                {
                    List<NhanVien>  dsNhanVien = conn.Query<NhanVien>("SELECT * FROM public.nhan_vien WHERE \"PhongBan\" = @Id ORDER BY \"HoVaTen\" ASC OFFSET((@page - 1)*@page_size) LIMIT @page_size ", new
                    {
                        page,
                        page_size,
                        Id 
                    }).ToList();
                    return dsNhanVien;
                }
                else
                {
                    List<NhanVien> dsNhanVien = conn.Query<NhanVien>("SELECT * FROM public.nhan_vien ORDER BY \"HoVaTen\" ASC OFFSET((@page - 1)*@page_size) LIMIT @page_size", new
                    {
                        page,
                        page_size
                    }).ToList();
                    return dsNhanVien;
                }

            }
        }
        public static string MaNV(int i, string manv)
        {
            int ListCount = i + 10000;
            manv = "NV-" + (ListCount.ToString()).Substring(1);
            return manv;
        }


        // GET: Staff
        public ActionResult Index()

        {
            List<NhanVien> dsNhanVien = ConnectList();
            if (dsNhanVien == null)
            {
                dsNhanVien = new List<NhanVien>();
                for (int i = 0; i < 5; i++)
                {
                    var RanDom = new NhanVien();
                    RanDom.MaNhanVien = Guid.NewGuid();
                    RanDom.HoVaTen = i + "XXXXXXXXXXX";
                    RanDom.NgaySinh = System.DateTime.Now;
                    RanDom.SoDienThoai = i + "XXXXXXXXXXX";
                    RanDom.DiaChi = i + "XXXXXXXXXXX";
                    RanDom.ChucVu = i + "XXXXXXXXXXX";
                    RanDom.SoNamCongTac = i;
                    dsNhanVien.Add(RanDom);
                }
            }
            SessionExtension.SetList(DANH_SACH_NHAN_VIEN, dsNhanVien);
            return View("Index", dsNhanVien);
        }


        [HttpGet]
        public ActionResult Search(string keyword)
        {
            //var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
            var dsNhanVien = ConnectList();
            var result = dsNhanVien.FindAll(item => item.HoVaTen.ToLower().Contains(keyword.Trim().ToLower())
            || item.DiaChi.ToLower().Contains(keyword.Trim().ToLower())
            || item.ChucVu.ToLower().Contains(keyword.Trim().ToLower())
            || item.SoDienThoai.ToLower().Contains(keyword.Trim().ToLower())
            || item.SoNamCongTac.ToString().Contains(keyword.Trim()));
            if (result.Count != 0)
            {
                return Json(new { success = true, status = true, data = result, JsonRequestBehavior.AllowGet });

            }
            else
            {
                return Json(new
                {
                    success = false,
                    status = false,
                    message = "* Không tìm thấy dữ liệu"
                });
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(NhanVien nv)
        {
            //var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
            //if (dsNhanVien == null)
            //{
            //    SessionExtension.SetList(DANH_SACH_NHAN_VIEN, new List<NhanVien>());
            //}

            object ketQua = null;
            var dsNhanVien = ConnectList();
            nv.MaNhanVien = Guid.NewGuid();
            nv.HoVaTen = Fomart.Fomartstring(nv.HoVaTen);
            bool TestPhone = Int32.TryParse(nv.SoDienThoai, out _);
            if (!TestPhone && nv.SoDienThoai.Length != 10)
            {
                ketQua = new
                {
                    status = false,
                    message = "* Số điện thoại không tồn tại. yêu cầu nhập lại "
                };
            }
            if (ketQua != null) return Json(ketQua);
            nv.DiaChi = Fomart.Fomartstring(nv.DiaChi);
            nv.ChucVu = Fomart.Fomartstring(nv.ChucVu);
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
            SessionExtension.SetList(DANH_SACH_NHAN_VIEN, dsNhanVien);
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                conn.Execute("INSERT INTO public.nhan_vien (\"MaNhanVien\",\"HoVaTen\",\"NgaySinh\",\"SoDienThoai\",\"DiaChi\",\"ChucVu\",\"SoNamCongTac\") VALUES(@MaNhanVien,@HoVaTen,@NgaySinh,@SoDienThoai,@DiaChi,@ChucVu,@SoNamCongTac)", nv);
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
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                conn.Execute("UPDATE public.nhan_vien SET \"MaNhanVien\" = @MaNhanVien, \"HoVaTen\" = @HoVaTen, \"NgaySinh\" = @NgaySinh, \"SoDienThoai\" = @SoDienThoai, \"DiaChi\" = @DiaChi, \"ChucVu\" = @ChucVu, \"SoNamCongTac\" = @SoNamCongTac" +
                " WHERE \"MaNhanVien\" = @MaNhanVien", nv);
            }
            SessionExtension.SetList(DANH_SACH_NHAN_VIEN, dsNhanVien);
            return Json(new { success = true, status = true, data = dsNhanVien });

        }

        public ActionResult Delete(Guid ma)
        {
            var dsNhanVien = ConnectList();
            var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == ma);
            dsNhanVien.Remove(nv);
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                conn.Execute("DELETE FROM public.nhan_vien WHERE \"MaNhanVien\" = @ma", new { ma });
            }
            return Json(new { success = true, status = true, data = dsNhanVien });
        }

        //public ActionResult Table(int page=1, int page_size=5, int idRoom = 0)
        //{
        //    int rows;
        //    //var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
        //    var dsNhanVien = Pagination(page, page_size);
        //    using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        if (idRoom != 0)
        //        {
        //             rows = conn.Query<int>("SELECT COUNT(*) FROM public.nhan_vien WHERE \"PhongBan\"=@idRoom",new {idRoom}).FirstOrDefault();
        //        }
        //        else
        //        {
        //             rows = conn.Query<int>("SELECT COUNT(*) FROM public.nhan_vien").FirstOrDefault();
        //        }
        //        ViewBag.TotalPages = Math.Ceiling((double)rows / page_size);

        //    }
        //    return PartialView("_DanhSachNV", dsNhanVien);
        //}
        public ActionResult Table(int page = 1, int page_size = 5, int Id = 0)
        {
            int rows;
            //var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
            var dsNhanVien = Pagination(page, page_size,Id);
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
              
                     rows = conn.Query<int>("SELECT COUNT(*) FROM public.nhan_vien").FirstOrDefault();
                ViewBag.TotalPages = Math.Ceiling((double)rows / page_size);
                
            }
            return PartialView("_DanhSachNV", dsNhanVien);
        }

    }
}