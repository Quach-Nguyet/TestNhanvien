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

        // GET: Staff
        public ActionResult Index()
        {
            // tạo 1 số nhân viên
            var nv = new List<NhanVien>() {
                new NhanVien()
            {
                MaNhanvien = Guid.NewGuid(),
                HoVaTen = "Ánh Nguyệt",

            },
                new NhanVien()
                {
                    MaNhanvien = Guid.NewGuid(),
                    HoVaTen = "Ngô Công"
                }
            };
            SessionExtension.SetList(DANH_SACH_NHAN_VIEN, nv);
            //

            var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
            return View(dsNhanVien);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(NhanVien nv)
        {
            if (ModelState.IsValid) { 
                nv.MaNhanvien = Guid.NewGuid();
                var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);
                dsNhanVien.Add(nv);

                SessionExtension.SetList(DANH_SACH_NHAN_VIEN, dsNhanVien);

                return View("Index", dsNhanVien);
            }
            return View(nv);
        }
        public ActionResult Edit(string maNhanVien)
        {
            var dsNhanVien = SessionExtension.GetList<NhanVien>(DANH_SACH_NHAN_VIEN);

            var nv = dsNhanVien.FirstOrDefault(t => t.MaNhanvien == Guid.Parse(maNhanVien));

            return View(nv);
        }
        [HttpPost]
        public ActionResult Edit(NhanVien nv)
        {
            return View("Index");
        }
        public ActionResult Delete()
        {
            return View();
        }
        public ActionResult Report()
        {
            return View();
        }


    }
}