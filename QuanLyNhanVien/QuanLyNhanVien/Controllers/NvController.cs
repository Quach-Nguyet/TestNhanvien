using QuanLyNhanVien.Extensions;
using QuanLyNhanVien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace QuanLyNhanVien.Controllers
{
    public class NvController : Controller
    {
        private readonly string DANH_SACH_NHAN_VIEN = "DanhSachNhanVien";

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

            return View("Index", dsNhanVien);
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
                return Json(new
                {
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
            object ketQua = null;
            var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
            if (dsNhanVien == null)
            {
                SessionExtension.SetList(DANH_SACH_NHAN_VIEN, new List<NhanVien>());
            }
            nv.MaNhanVien = MaNV(dsNhanVien.Count, nv.MaNhanVien);
            nv.HoVaTen = Fomart.Fomartstring(nv.HoVaTen);
            bool TestPhone = Int32.TryParse(nv.SoDienThoai, out int Phone);
            if (TestPhone && nv.SoDienThoai.Length != 10)
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
            SessionExtension.SetList(DANH_SACH_NHAN_VIEN, dsNhanVien);
            dsNhanVien.Add(nv);
            return Json(new { success = true, status = true, data = nv, JsonRequestBehavior.AllowGet });
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Json(new
                {
                    success = false,
                    message = "Mã nhân viên không được phép trống"
                });
            }
            var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);

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
            var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
            var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == nvNew.MaNhanVien);
            //var nv1 = dsNhanVien.FirstOrDefault(t => t.MaNhanVien != nvNew.MaNhanVien && t.HoVaTen.ToLower() == nvNew.HoVaTen.ToLower());
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
            return Json(new { success = true, status = true, data = dsNhanVien });
        }

        public ActionResult Delete(string ma)
        {
            var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
            var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == ma);
            dsNhanVien.Remove(nv);
            SessionExtension.SetList(DANH_SACH_NHAN_VIEN, dsNhanVien);
            return Json(dsNhanVien, JsonRequestBehavior.AllowGet);
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

        public ActionResult Table()
        {
            var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
            return PartialView("_DanhSachNV", dsNhanVien);
        }
    }
}