using Newtonsoft.Json;
using QuanLyNhanVien.Extensions;
using QuanLyNhanVien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace QuanLyNhanVien.Controllers
{
    public class NvController : Controller
    {
        private readonly string DANH_SACH_NHAN_VIEN = "DanhSachNhanVien";
        private readonly string NHAN_VIEN = "NhanVien";

        public static string MaNV(int i, string manv)
        {
            int ListCount = i + 10000;
            manv = "NV-" + (ListCount.ToString()).Substring(1);
            return manv;
        }



        // GET: Staff
        public ActionResult Index()
        {
            var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
            //if (dsNhanVien == null)
            //{
            //    dsNhanVien = new List<NhanVien>();
            //    for (int i = 0; i < 5; i++)
            //    {
            //        var RanDom = new NhanVien();
            //        RanDom.MaNhanVien = MaNV(i, RanDom.MaNhanVien);
            //        RanDom.HoVaTen = i + "XXXXXXXXXXX";
            //        RanDom.NgaySinh = System.DateTime.Now;
            //        RanDom.SoDienThoai = i + "XXXXXXXXXXX";
            //        RanDom.DiaChi = i + "XXXXXXXXXXX";
            //        RanDom.ChucVu = i + "XXXXXXXXXXX";
            //        RanDom.SoNamCongTac = "" + i;
            //        dsNhanVien.Add(RanDom);
            //    }
            //}
            //SessionExtension.SetList(DANH_SACH_NHAN_VIEN, dsNhanVien);
            //dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
            return View("Index",dsNhanVien);
        }

        [HttpGet]
        
        public ActionResult DanhSachNv()
        {
            var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
            if (dsNhanVien == null)
            {
                dsNhanVien = new List<NhanVien>();
                for (int i = 0; i < 5; i++)
                {
                    var RanDom = new NhanVien();
                    RanDom.MaNhanVien = MaNV(i, RanDom.MaNhanVien);
                    RanDom.HoVaTen = i + "XXXXXXXXXXX";
                    RanDom.NgaySinh = System.DateTime.Now;
                    RanDom.SoDienThoai = i + "XXXXXXXXXXX";
                    RanDom.DiaChi = i + "XXXXXXXXXXX";
                    RanDom.ChucVu = i + "XXXXXXXXXXX";
                    RanDom.SoNamCongTac = "" + i;
                    dsNhanVien.Add(RanDom);
                }
            }
            SessionExtension.SetList(DANH_SACH_NHAN_VIEN, dsNhanVien);
           dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
            return Json(dsNhanVien,JsonRequestBehavior.AllowGet);
            //return PartialView("~/View/Nv/_DanhSachNV", dsNhanVien);
        }


        [HttpGet]
        public ActionResult Search(string keyword)
        {
            var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
            var result = dsNhanVien.FindAll(item => item.HoVaTen.ToLower().Contains(keyword.Trim().ToLower())
            || item.DiaChi.ToLower().Contains(keyword.Trim().ToLower())
            || item.ChucVu.ToLower().Contains(keyword.Trim().ToLower())
            || item.SoDienThoai.ToLower().Contains(keyword.Trim().ToLower())
            || item.SoNamCongTac.ToString().Contains(keyword.Trim()));
            if (result.Count != 0)
            {
                return Json(result);
            }
            else
            {

                ViewData["Messeger"] = "* Không tìm thấy dữ liệu";
                return Json(new
                {
                    status = false,
                    message = "* Không tìm thấy dữ liệu"
                }) ;
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
            if (ModelState.IsValid)
            {
                object ketQua = null;
                var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
                if (dsNhanVien == null)
                {
                    SessionExtension.SetList(DANH_SACH_NHAN_VIEN, new List<NhanVien>());
                }

                nv.MaNhanVien = MaNV(dsNhanVien.Count, nv.MaNhanVien);
                nv.HoVaTen = Fomart.Fomartstring(nv.HoVaTen);
                nv.DiaChi = Fomart.Fomartstring(nv.DiaChi);
                nv.ChucVu = Fomart.Fomartstring(nv.ChucVu);
                bool test = Int32.TryParse(nv.SoNamCongTac, out int nam);
                if (!test)
                {
                    
                    return Json(new { 
                        success = false,
                        message = "Yêu cầu nhập kí tự số",
                        status = false
                    });
                }
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
                    if (ketQua == null) return Json(ketQua);
                }
                dsNhanVien.Add(nv);

                SessionExtension.SetList(DANH_SACH_NHAN_VIEN, dsNhanVien);

                return Json(dsNhanVien);
                // return Json(new { success = true });
            }
            return Json(new { success = true });
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {

            if (string.IsNullOrWhiteSpace(id))
            {
                return Json(new
                {
                    success = false,
                    msg = "Mã nhân viên không được phép trống"
                });
            }
            var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);

            var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien ==id);
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
            var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
            var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == nvNew.MaNhanVien);

            //var nv1 = dsNhanVien.FirstOrDefault(t => t.MaNhanVien != nvNew.MaNhanVien && t.HoVaTen.ToLower() == nvNew.HoVaTen.ToLower());
            object ketQua = null;
            foreach (var item in dsNhanVien) { 
                  if (nvNew.MaNhanVien != item.MaNhanVien)
                  {
                    if (nvNew.HoVaTen == item.HoVaTen)
                    {
                        ViewData["ErrorName"] = "*Tên đã có. Yêu cầu nhập lại";
                        ketQua = new { 
                            message = "Tên đã có. Yêu cầu nhập lại",
                            status = false,
                            success = false
                        };
                    }

                    if (nvNew.SoDienThoai == item.SoDienThoai)
                    {
                        /// nhớ sửa
                       ketQua = new
                        {
                            message = "*Số điện thoại đã có. Yêu cầu nhập lại",
                            status = false
                    };
                    }
                }

            }
            if (ketQua != null) return Json(ketQua);

            nv.MaNhanVien = nvNew.MaNhanVien;
            nv.HoVaTen = Fomart.Fomartstring(nvNew.HoVaTen);
            nv.NgaySinh = nvNew.NgaySinh;
            bool TestPhone = Int32.TryParse(nvNew.SoDienThoai, out int Phone);
            if (TestPhone && nvNew.SoDienThoai.Length == 10)
            {
                nv.SoDienThoai = nvNew.SoDienThoai;
            }
            else
            {
                ketQua = new {
                    status = false,
                    message = "* Số điện thoại không tồn tại. yêu cầu nhập lại "
            };
            }
            if (ketQua != null) return Json(ketQua);
            nv.DiaChi = Fomart.Fomartstring(nvNew.DiaChi);
            nv.ChucVu = Fomart.Fomartstring(nvNew.ChucVu);
            bool test = Int32.TryParse(nvNew.SoNamCongTac, out int nam);
            if (!test)
            {
                ketQua = new
                {
                    status = false,
                    message = "* Yêu cầu nhập kí tự số"
                };
            }
            else
                nv.SoNamCongTac = nvNew.SoNamCongTac;
           // return Json(dsNhanVien);
            return Json(new { success = true, status = true});
        }

        public ActionResult Delete(string ma)
        {
            var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
            var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == ma);
            dsNhanVien.Remove(nv);
            SessionExtension.SetList(DANH_SACH_NHAN_VIEN, dsNhanVien);
            // return Json(dsNhanVien);
            return View("Index",dsNhanVien);
        }

        public ActionResult ChiTietNhanVien(string maNhanVien)
        {
            var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
           var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == maNhanVien);
            return Json(nv);

        }
        public ActionResult Report()
        {
            return View();
        }

        public ActionResult Table() {
            var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
            return PartialView("~/View/Nv/_DanhSachNV", dsNhanVien);
        }

      

    }
}