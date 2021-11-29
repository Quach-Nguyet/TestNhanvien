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
    public class ApiStaffController : Controller
    {
         List<NhanVien> dsNhanVien = new List<NhanVien>();
        // GET: ApiStaff

        public static string MaNV(int i, string manv)
        {
            int ListCount = i + 10000;
            manv = "NV-" + (ListCount.ToString()).Substring(1);
            return manv;
        }
        [HttpGet]
        public ActionResult Index()
        {
            if (dsNhanVien.Count == 0)
            {
                dsNhanVien = new List<NhanVien>();
                for (int i = 0; i < 3; i++)
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
            // return Content(JsonConvert.SerializeObject(dsNhanVien), "application/json", Encoding.UTF8);
            return View();
        }

        public ActionResult ChiTietNhanVien(string maNhanVien)
        {
            NhanVien nv = new NhanVien();
            if (dsNhanVien.Count == 0)
            {
                ViewData["ErrorNull"] = " Danh sach trống";
            }
            else
            {
                nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == maNhanVien);
            }
            //var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == maNhanVien);
            //var nv = new NhanVien()
            //{
            //    HoVaTen = "Nguyệt",
            //    MaNhanVien = "000",
            //    SoDienThoai = "0125634789"
            //};
            return Content(JsonConvert.SerializeObject(nv), "application/json", Encoding.UTF8);

        }

        [HttpGet]
        public ActionResult TimKiemNhanVien(string keyword)
        {
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
        public ActionResult ThemNhanVien()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemNhanVien(NhanVien nv)
        {
            if (ModelState.IsValid)
            {
                if (dsNhanVien == null)
                {
                    dsNhanVien = new List<NhanVien>();
                }

                nv.MaNhanVien = MaNV(dsNhanVien.Count, nv.MaNhanVien);
                nv.HoVaTen = Fomart.Fomartstring(nv.HoVaTen);

                nv.DiaChi = Fomart.Fomartstring(nv.DiaChi);
                nv.ChucVu = Fomart.Fomartstring(nv.ChucVu);
                bool test = Int32.TryParse(nv.SoNamCongTac, out int nam);
                if (test != true)
                {
                    ViewData["ErrorYear"] = "* Yêu cầu nhập kí tự số";
                    return Content(JsonConvert.SerializeObject(nv), "application/json", Encoding.UTF8);
                }

                foreach (var item in dsNhanVien)
                {
                    while (nv.HoVaTen == item.HoVaTen)
                    {
                        ViewData["ErrorName"] = "*Tên đã có. Yêu cầu nhập lại";
                        return Content(JsonConvert.SerializeObject(nv), "application/json", Encoding.UTF8);
                    }

                    while (nv.SoDienThoai == item.SoDienThoai)
                    {
                        ViewData["ErrorPhone"] = "*Số điện thoại đã có. Yêu cầu nhập lại";
                        return Content(JsonConvert.SerializeObject(nv), "application/json", Encoding.UTF8);
                    }
                }
                dsNhanVien.Add(nv);
                return Content(JsonConvert.SerializeObject(dsNhanVien), "application/json", Encoding.UTF8);
            }
            return Content("Some field are requred", "application/json", Encoding.UTF8);
        }

        [HttpPut]
        public ActionResult SuaThongTinNhanVien(string maNhanVien)
        {
            if (maNhanVien == null)

            {
                return Content("Chưa có mã nhân viên cần tìm", "application/json", Encoding.UTF8);
            }

            var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == maNhanVien);

            return Content(JsonConvert.SerializeObject(nv), "application/json", Encoding.UTF8);
        }

        [HttpPost]
        public ActionResult SuaThongTinNhanVien(NhanVien nvNew)
        {
            var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == nvNew.MaNhanVien);

            //var nv1 = dsNhanVien.FirstOrDefault(t => t.MaNhanVien != nvNew.MaNhanVien && t.HoVaTen.ToLower() == nvNew.HoVaTen.ToLower());
            foreach (var item in dsNhanVien)
            {
                if (nvNew.MaNhanVien != item.MaNhanVien)
                {
                    if (nvNew.HoVaTen == item.HoVaTen)
                    {
                        ViewData["ErrorName"] = "*Tên đã có. Yêu cầu nhập lại";
                        return Content(JsonConvert.SerializeObject(nvNew), "application/json", Encoding.UTF8);
                    }

                    if (nvNew.SoDienThoai == item.SoDienThoai)
                    {
                        ViewData["ErrorPhone"] = "*Số điện thoại đã có. Yêu cầu nhập lại";
                        return Content(JsonConvert.SerializeObject(nvNew), "application/json", Encoding.UTF8);
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
                return Content(JsonConvert.SerializeObject(nvNew), "application/json", Encoding.UTF8);
            }

            nv.DiaChi = Fomart.Fomartstring(nvNew.DiaChi);
            nv.ChucVu = Fomart.Fomartstring(nvNew.ChucVu);
            bool test = Int32.TryParse(nvNew.SoNamCongTac, out int nam);
            if (test != true)
            {
                ViewData["ErrorYear"] = "* Yêu cầu nhập kí tự số";
                return Content(JsonConvert.SerializeObject(nvNew), "application/json", Encoding.UTF8);
            }
            return Content(JsonConvert.SerializeObject(dsNhanVien), "application/json", Encoding.UTF8);
        }

        [HttpDelete]
        public ActionResult XoaNhanVien(string ma)
        {
            var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanVien == ma);
            dsNhanVien.Remove(nv);
           return Content(JsonConvert.SerializeObject(dsNhanVien), "application/json", Encoding.UTF8);
        }
        

    }


}