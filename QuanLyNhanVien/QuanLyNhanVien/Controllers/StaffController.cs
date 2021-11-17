using QuanLyNhanVien.Extensions;
using QuanLyNhanVien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace QuanLyNhanVien.Controllers
{

    public class StaffController : Controller
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
                    RanDom.SoNamCongTac = i;
                    dsNhanVien.Add(RanDom);
                }
            }
            SessionExtension.SetList(DANH_SACH_NHAN_VIEN, dsNhanVien);
            dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
            return View(dsNhanVien);
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
                return View("Index", result);
            }
            else
            {
                ViewData["Messeger"] = "* Không tìm thấy dữ liệu";
                return View("Index", dsNhanVien);
            }
        }

     

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(NhanVienCreateDate date)
        {

            var nv = new NhanVien();
            if (ModelState.IsValid)
            {
                DateTime Date;
                var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
                nv.MaNhanVien = MaNV(dsNhanVien.Count, date.MaNhanVien);
                nv.HoVaTen = Fomart.Fomartstring(date.HoVaTen);
                DateTime.TryParseExact(date.NgaySinh, "dd/M/yyyy", null, System.Globalization.DateTimeStyles.None, out Date);
                nv.NgaySinh = Date;
                nv.SoDienThoai = date.SoDienThoai;
                nv.DiaChi = Fomart.Fomartstring(date.DiaChi);
                nv.ChucVu = Fomart.Fomartstring(date.ChucVu);
                nv.SoNamCongTac = date.SoNamCongTac;
                foreach (var item in dsNhanVien)
                {
                    while (nv.HoVaTen == item.HoVaTen)
                    {
                        ViewData["ErrorName"] = "*Tên đã có. Yêu cầu nhập lại";
                        return Create();
                    }

                    while (nv.SoDienThoai == item.SoDienThoai)
                    {
                        ViewData["ErrorPhone"] = "*Số điện thoại đã có. Yêu cầu nhập lại";
                        return Create();
                    }
                }
                dsNhanVien.Add(nv);

                SessionExtension.SetList(DANH_SACH_NHAN_VIEN, dsNhanVien);

                return View("Index", dsNhanVien);
            }
            return View(nv);
        }
        public ActionResult Edit(string maNhanVien)
        {
            if (maNhanVien == null)

            {
                return View();
            }
            var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);

            var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == maNhanVien);
            
            return View(nv);
        }
        [HttpPost]
        public ActionResult Edit( NhanVien nvNew)
        {
            var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);

            var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == nvNew.MaNhanVien);
                foreach (var item in dsNhanVien)
                {     
                if ( nvNew.HoVaTen == item.HoVaTen)
                {
                    ViewData["ErrorName"] = "*Tên đã có. Yêu cầu nhập lại";
                    return View("Edit", nvNew);
                }

                if (nvNew.SoDienThoai == item.SoDienThoai)
                {
                    ViewData["ErrorPhone"] = "*Số điện thoại đã có. Yêu cầu nhập lại";
                    return View("Edit", nvNew);
                }
            }
            nv = nvNew;
            return View("Index",dsNhanVien);
        }

        public ActionResult Delete(string ma)
        {
            var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
             var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == ma);
            dsNhanVien.Remove(nv);
            SessionExtension.SetList(DANH_SACH_NHAN_VIEN, dsNhanVien);
            return View("Index", dsNhanVien);
        }
        public ActionResult Report()
        {
            return View();
        }


    }
}