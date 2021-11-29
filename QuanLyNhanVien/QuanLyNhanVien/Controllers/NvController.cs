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
                return Content(JsonConvert.SerializeObject(result), "application/json", Encoding.UTF8);
            }
            else
            {
                ViewData["Messeger"] = "* Không tìm thấy dữ liệu";
                return Content(JsonConvert.SerializeObject(dsNhanVien), "application/json", Encoding.UTF8);
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
                if (test != true)
                {
                    ViewData["ErrorYear"] = "* Yêu cầu nhập kí tự số";
                    return View("Create", nv);
                }

                foreach (var item in dsNhanVien)
                {
                    while (nv.HoVaTen == item.HoVaTen)
                    {
                        ViewData["ErrorName"] = "*Tên đã có. Yêu cầu nhập lại";
                        return View("Create", nv);
                    }

                    while (nv.SoDienThoai == item.SoDienThoai)
                    {
                        ViewData["ErrorPhone"] = "*Số điện thoại đã có. Yêu cầu nhập lại";
                        return View("Create", nv);
                    }
                }
                dsNhanVien.Add(nv);

                SessionExtension.SetList(DANH_SACH_NHAN_VIEN, dsNhanVien);

                return View("Index", dsNhanVien);
            }
            return View(nv);
        }

        [HttpGet]
        public ActionResult Edit(string maNhanVien)
        {
            if (maNhanVien == null)

            {
                return View();
            }
            var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);

            var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == maNhanVien);
             Content(JsonConvert.SerializeObject(nv), "application/json", Encoding.UTF8);
             return View(nv);
        }
        [HttpPost]
        public ActionResult Edit(NhanVien nvNew)
        {
            var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
            var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == nvNew.MaNhanVien);

            //var nv1 = dsNhanVien.FirstOrDefault(t => t.MaNhanVien != nvNew.MaNhanVien && t.HoVaTen.ToLower() == nvNew.HoVaTen.ToLower());
            foreach (var item in dsNhanVien)
            {
                if (nvNew.MaNhanVien != item.MaNhanVien)
                {
                    if (nvNew.HoVaTen == item.HoVaTen)
                    {
                        ViewData["ErrorName"] = "*Tên đã có. Yêu cầu nhập lại";
                        return Content(JsonConvert.SerializeObject(nvNew), "application/json", Encoding.UTF8); //return View("Edit", nvNew);
                    }

                    if (nvNew.SoDienThoai == item.SoDienThoai)
                    {
                        ViewData["ErrorPhone"] = "*Số điện thoại đã có. Yêu cầu nhập lại";
                        return Content(JsonConvert.SerializeObject(nvNew), "application/json", Encoding.UTF8);// return View("Edit", nvNew);
                    }
                }

            }
            nv.MaNhanVien = nvNew.MaNhanVien;
            nv.HoVaTen = nvNew.HoVaTen;
            nv.NgaySinh = nvNew.NgaySinh;
            bool TestPhone = Int32.TryParse(nvNew.SoDienThoai, out int Phone);
            if (TestPhone == true && nvNew.SoDienThoai.Length == 10)
            {
                nv.SoDienThoai = nvNew.SoDienThoai;
            }
            else
            {
                ViewData["ErrorFormatPhone"] = "* Số điện thoại không tồn tại. yêu cầu nhập lại ";
                // return View("Edit", nvNew);
                return Content(JsonConvert.SerializeObject(nvNew), "application/json", Encoding.UTF8);
            }

            nv.DiaChi = Fomart.Fomartstring(nvNew.DiaChi);
            nv.ChucVu = Fomart.Fomartstring(nvNew.ChucVu);
            bool test = Int32.TryParse(nvNew.SoNamCongTac, out int nam);
            if (test != true)
            {
                ViewData["ErrorYear"] = "* Yêu cầu nhập kí tự số";
                //return View("Edit", nvNew);
                return Content(JsonConvert.SerializeObject(nvNew), "application/json", Encoding.UTF8);
            }

            return View("Index", dsNhanVien);
        }

        public ActionResult Delete(string ma)
        {
            var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
            var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == ma);
            dsNhanVien.Remove(nv);
            SessionExtension.SetList(DANH_SACH_NHAN_VIEN, dsNhanVien);
            return View("Index", dsNhanVien);
        }

        public ActionResult ChiTietNhanVien(string maNhanVien)
        {
            var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
            NhanVien nv = new NhanVien();
            
                nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == maNhanVien);
            return Content(JsonConvert.SerializeObject(nv), "application/json", Encoding.UTF8);

        }
        public ActionResult Report()
        {
            return View();
        }

    }
}