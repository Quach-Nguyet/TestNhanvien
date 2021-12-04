using Newtonsoft.Json;
using QuanLyNhanVien.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace QuanLyNhanVien.Controllers
{
    public class SachController : Controller
    {
        private readonly string _sachSessionKey = "SACH_SESSION";

        // GET: Sach
        public ActionResult Index()
        {
            var dsSach = SessionExtension.GetList<Sach>(_sachSessionKey);
            return View(dsSach);
        }

        [HttpGet]
        public ActionResult GetList()
        {
            var dsSach = SessionExtension.GetList<Sach>(_sachSessionKey);
            //tạo response cho client
            //fake 1 số dữ liệu
            // dsSach = new System.Collections.Generic.List<Sach>();
            // dsSach.Add(new Sach()
            // {
            //     Id = Guid.NewGuid(),
            //     TenSach = "SGK lớp 1",
            //     NgayXuatBan = DateTime.Now,
            //     NhaXuatBan = "Kim Đồng",
            //     SoTrang = 300,
            //     TrangThai = true
            // });
             SessionExtension.SetList<Sach>(_sachSessionKey,dsSach);
             if (dsSach == null)
            {
                dsSach = new System.Collections.Generic.List<Sach>();
                dsSach.Add(new Sach()
                {
                    Id = Guid.NewGuid(),
                    TenSach = "SGK lớp 1",
                    NgayXuatBan = DateTime.Now,
                    NhaXuatBan = "Kim Đồng",
                    SoTrang = 300,
                    TrangThai = true
                });
            }
            var response = new ResponseModel(dsSach, "Lấy dữ liệu thành công!");
            return Content(JsonConvert.SerializeObject(response), "application/json", Encoding.UTF8);
        }

        [HttpPost]
        public ActionResult Create(SachModel model)
        {
            if (!ModelState.IsValid)
            {
                //tạo response cho client
                var response = new ResponseModel(null, "Một số trường bắt buộc nhập!", 2);
                return Content(JsonConvert.SerializeObject(response), "application/json", Encoding.UTF8);
            }
            var sach = new Sach()
            {
                Id = Guid.NewGuid(), //Tạo 1 mã sách tự sinh ra
                TenSach = model.TenSach,
                NgayXuatBan = DateTime.ParseExact(model.NgayXuatBan, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None),
                NhaXuatBan = model.NhaXuatBan,
                SoTrang = model.SoTrang,
                TrangThai = model.TrangThai
            };
            // thêm vào session
            var dsSach = SessionExtension.GetList<Sach>(_sachSessionKey);
            dsSach.Add(sach);
            SessionExtension.SetList<Sach>(_sachSessionKey, dsSach);

            // trả cho client sách đã thêm mới
            var result = new ResponseModel(sach, "Thêm mới thành công", 1);
            return Content(JsonConvert.SerializeObject(result), "application/json", Encoding.UTF8);
        }

        public ActionResult Get(Guid id)
        {
            // lấy danh sách dữ liệu trong session
            var dsSach = SessionExtension.GetList<Sach>(_sachSessionKey);
            //tìm kiếm sách
            var sach = dsSach.FirstOrDefault(t => t.Id == id);

            //nếu không tìm thấy sách thì báo cho client
            //tạo response cho client
            var response = sach == null ? new ResponseModel(null, "Không tìm thấy sách", 2) : new ResponseModel(sach);
            return Content(JsonConvert.SerializeObject(response), "application/json", Encoding.UTF8);
        }

        [HttpPost]
        public ActionResult Edit(SachModel model)
        {
            if (!ModelState.IsValid)
            {
                //tạo response cho client
                var response = new ResponseModel(null, "Một số trường bắt buộc nhập!", 2);
                return Content(JsonConvert.SerializeObject(response), "application/json", Encoding.UTF8);
            }
            // lấy danh sách dữ liệu trong session
            var dsSach = SessionExtension.GetList<Sach>(_sachSessionKey);
            // vị trí sách cần sửa trong danh sách dữ liệu
            var sachIndex = dsSach.FindIndex(t => t.Id == model.Id);
            if (sachIndex < 0) // Nếu không tìm thấy sách trong danh sách dữ liệu thì thông báo cho người dùng
            {
                var response = new ResponseModel(null, "Không tìm thấy quyển sách này!", 2);
                return Content(JsonConvert.SerializeObject(response), "application/json", Encoding.UTF8);
            }
            // sửa dữ liệu tại vị trí trên trong danh sách
            dsSach[sachIndex].TenSach = model.TenSach;
            dsSach[sachIndex].NgayXuatBan = DateTime.ParseExact(model.NgayXuatBan, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None);
            dsSach[sachIndex].NhaXuatBan = model.NhaXuatBan;
            dsSach[sachIndex].SoTrang = model.SoTrang;
            dsSach[sachIndex].TrangThai = model.TrangThai;

            // thêm lại giá trị đã sửa vào session
            SessionExtension.SetList<Sach>(_sachSessionKey, dsSach);

            // trả cho client sách đã sửa
            var result = new ResponseModel(dsSach[sachIndex], "Thêm mới thành công", 1);
            return Content(JsonConvert.SerializeObject(result), "application/json", Encoding.UTF8);
        }
    }

    /// <summary>
    /// đây là dữ liệu chính
    /// </summary>
    public class Sach
    {
        public Guid Id { get; set; }
        public string TenSach { get; set; }
        public string NhaXuatBan { get; set; }
        public DateTime NgayXuatBan { get; set; }
        public int SoTrang { get; set; }

        // false: ngừng xuất bản, true: còn xuất bản
        public bool TrangThai { get; set; }
    }

    /// <summary>
    /// Class này dùng để client truyền dữ liệu lên dễ dàng hơn sau đó mình sẽ format về DỮ LIỆU CHÍNH để lưu vào database/file/session
    /// </summary>
    public class SachModel
    {
        public Guid? Id { get; set; }

        [Required]
        public string TenSach { get; set; }

        public string NhaXuatBan { get; set; }

        // Date format: dd/MM/yyyy
        [Required]
        public string NgayXuatBan { get; set; }

        [Required]
        public int SoTrang { get; set; }

        // false: ngừng xuất bản, true: còn xuất bản
        public bool TrangThai { get; set; }
    }

    /// <summary>
    /// Cái class này là định dạng trả về cho client sẽ có message, status (0: lỗi hệ thống, 1: thành công, 2: lỗi do client truyền sai params,... - có thể có thêm status tùy em thích quy định ntn cũng đc), data (đây sẽ là dữ liệu chung, không có kiểu dữ liệu cố định nên sẽ là object hoặc dynamic)
    /// </summary>
    public class ResponseModel
    {
        public ResponseModel(object data, string message = null, int status = 1)
        {
            Data = data;
            Message = message;
            Status = status;
        }

        public string Message { get; set; }
        public int Status { get; set; }
        public object Data { get; set; }
    }
}