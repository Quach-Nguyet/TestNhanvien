

let pageIndex = 1
let roomId = 0
var keySreach = ""
//định dạng ngày
Inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy" }).mask(".js-date");

$(document).ready(() => {
    $.get('/Nv/getPhongBan').done(xhr => {
        if (xhr.data) {
            $.each(xhr.data, function (i, item) {
                $('#insert-room').append(`<option value= ${item.id}>${item.ten_phong_ban}</option>`);
                $('#update-room').append(`<option value= ${item.id}>${item.ten_phong_ban}</option>`);
                $('#dropdown-room').append(`<option value=${item.id}>${item.ten_phong_ban}</option>`);
            })
        }
    })

    //Mở form sửa
    $("#form-data").on('click', ".btnSua", function (e) {
        const id = $(this).attr('data-id')
        e.preventDefault()
        editNV(id);
    })

    //Lưu form sửa
    $('#Sua').on('click', function (e) {
        e.preventDefault();
        let ngaysinh = $('#form-sua').find('#Sua_NgaySinh').val();
        const nv = {
            MaNhanVien: $('#form-sua').find('#Sua_MaNhanVien').val(),
            HoVaTen: $('#form-sua').find('#Sua_HoVaTen').val(),
            NgaySinh: moment(ngaysinh, 'DD/MM/YYYY').format("YYYY-MM-DD"),
            DiaChi: $('#form-sua').find('#Sua_DiaChi').val(),
            SoDienThoai: $('#form-sua').find('#Sua_SoDienThoai').val(),
            ChucVu: $('#form-sua').find('#Sua_ChucVu').val(),
            SoNamCongTac: $('#form-sua').find('#Sua_SoNamCongTac').val(),
            Id: $('#form-sua').find('#Sua_PhongBan').val(),
            PhongBan: $('#update-room').val()
        };


        let isValidate = true;
        if (nv.HoVaTen == null || nv.HoVaTen == "") {
            $.toast({
                heading: 'Error',
                text: 'Họ và tên đang trống',
                showHideTransition: 'fade',
                icon: 'error',
                position: 'top-center'
            })
            isValidate = false
        }

        if (nv.NgaySinh == null || nv.NgaySinh == "") {
            $.toast({
                heading: 'Error',
                text: 'Ngày sinh đang trống',
                showHideTransition: 'fade',
                icon: 'error',
                position: 'top-center'
            })
            isValidate = false
        }

        if (nv.SoDienThoai == null || nv.SoDienThoai == "") {
            $.toast({
                heading: 'Error',
                text: 'Số điện thoại đang trống',
                showHideTransition: 'fade',
                icon: 'error',
                position: 'top-center'
            })
            isValidate = false
        }

        if (nv.ChucVu == null || nv.ChucVu == "") {
            $.toast({
                heading: 'Error',
                text: 'Chức vụ đang trống',
                showHideTransition: 'fade',
                icon: 'error',
                position: 'top-center'
            })
            isValidate = false
        }

        if (isValidate) {
            $.ajax({
                url: 'Nv/Edit?newRoom='+nv.Id,
                method: 'POST',
                data: nv,
                success: respon => {
                    if (respon.status) {
                        $('#SuaNhanVien').modal('hide');
                        $('#nhanvien_table').load('/nv/table');
                    }
                    else {
                        $.toast({
                            heading: 'Thông báo',
                            text: respon.message,
                            showHideTransition: 'fade',
                            icon: 'error',
                            position: 'top-center'
                        })
                        isValidate = false
                    }
                },
                error:
                    respon => $.toast({
                    heading: 'Thông báo',
                    text: respon.message,
                    showHideTransition: 'fade',
                    icon: 'error',
                    position: 'top-center'
                })
            })
        }
    })

    //Lưu form thêm
    $('#form-them').on('click', '#Them', function (e) {
        e.preventDefault()
        let ngaysinh = $('#ThemNhanVien').find('#NgaySinh').val()
        const data = {
            MaNhanVien: $('#ThemNhanVien').find('#MaNhanVien').val(),
            HoVaTen: $('#ThemNhanVien').find('#HoVaTen').val(),
            NgaySinh: moment(ngaysinh,'DD/MM/YYYY').format("YYYY-MM-DD"),
            SoDienThoai: $('#ThemNhanVien').find('#SoDienThoai').val(),
            DiaChi: $('#ThemNhanVien').find('#DiaChi').val(),
            ChucVu: $('#ThemNhanVien').find('#ChucVu').val(),
            SoNamCongTac: $('#ThemNhanVien').find('#SoNamCongTac').val(),
            Id: $('#ThemNhanVien').find('#PhongBan').val(),
            PhongBan: $('#insert-room').val()
        }

        let isValidate = true
        if (data.HoVaTen == null || data.HoVaTen == "") {
            $.toast({
                heading: 'Error',
                text: 'Họ và tên đang trống',
                showHideTransition: 'fade',
                icon: 'error',
                position: 'top-center'
            })
            isValidate = false
        }

        if (data.NgaySinh == null || data.NgaySinh == "") {
            $.toast({
                heading: 'Error',
                text: 'Ngày sinh đang trống',
                showHideTransition: 'fade',
                icon: 'error',
                position: 'top-center'
            })
            isValidate = false
        }
        if (data.SoDienThoai == null || data.SoDienThoai == "") {
            $.toast({
                heading: 'Error',
                text: 'Số điện thoại đang trống',
                showHideTransition: 'fade',
                icon: 'error',
                position: 'top-center'
            })
            isValidate = false
        }

        if (data.ChucVu == null || data.ChucVu == "") {
            $.toast({
                heading: 'Error',
                text: 'Chức vụ đang trống',
                showHideTransition: 'fade',
                icon: 'error',
                position: 'top-center'
            })
            isValidate = false
        }
        if (!isValidate) return

        $.ajax({
            url: '/Nv/Create?nameRoom=' + data.Id,
            method: 'POST',
            data: data,
            success: respon => {
                if (respon.status) {
                    $('#ThemNhanVien').modal('hide');
                    alert('Lưu thành công!');
                    $('#nhanvien_table').load('/nv/table');
                }
                else {
                    $.toast({
                        heading: 'Thông báo',
                        text: respon.message,
                        showHideTransition: 'fade',
                        icon: 'error',
                        position: 'top-center'
                    })
                }
            },
            error: respon => $.toast({
                    heading: 'Thông báo',
                    text: respon.message,
                    showHideTransition: 'fade',
                    icon: 'error',
                    position: 'top-center'
                })
            
        })
    })

    //Phân trang theo ban
    $('.filter-phong-ban').on('change', function () {
        pagination(1)
    })

    //Tìm kiếm
    $("#form-data").on('click', "#btnSreach", function (e) {
        e.preventDefault()
        const key = $('#keySreach').val();
        $('#nhanvien_table').load(`/nv/table?page=${pageIndex}&id=${$('.filter-phong-ban').val()}&keyword=${key}`);
    })
    //Xuất Ex
    $("#form-data").on('click', "#Export", function (e) {
        e.preventDefault()
        location.href = "/nv/export?Id=" + $('.filter-phong-ban').val();
    })
});
function loadTable() {
    $('#nhanvien_table').load('/nv/table');
}

function deleteNV(id, tableRow) {
    let confirmed = confirm("Bạn có chắc chắn muốn xóa nhân viên này?");
    if (confirmed) {
        $.ajax('/Nv/Delete?ma=' + id).then(respon => {
            if (respon.success) {
                //$(tableRow).closest('tr').remove();
                $('#nhanvien_table').load('/nv/table');
            }
        })
    }
}

function editNV(id) {
    $.ajax('/Nv/Edit?id=' + id).then(respon => {
        const nv = respon.data
        $('#form-sua').find('#Sua_MaNhanVien').val(nv.MaNhanVien);
        $('#form-sua').find('#Sua_HoVaTen').val(nv.HoVaTen);
        $('#form-sua').find('#Sua_NgaySinh').val(moment(nv.NgaySinh).format('DD/MM/YYYY'));
        $('#form-sua').find('#Sua_DiaChi').val(nv.DiaChi);
        $('#form-sua').find('#Sua_SoDienThoai').val(nv.SoDienThoai);
        $('#form-sua').find('#Sua_ChucVu').val(nv.ChucVu);
        $('#form-sua').find('#Sua_SoNamCongTac').val(nv.SoNamCongTac);
        $('#update-room').val(nv.phong_ban.id);
        $('#update-room').trigger('change');
        $('#SuaNhanVien').modal('show');
    }).catch(error => console.log(error))
}

//function room(id) {
//        roomId = id;
//        $('#nhanvien_table').load('/nv/table?Id=' + id);
//}

function pagination(page) {
    pageIndex = page;
    $('#nhanvien_table').load('/nv/table?page=' + page + '&id=' + $('.filter-phong-ban').val() + '&keyword=' + $('#keySreach').val());
}

function next() {
    pagination(pageIndex+1)
}

function previsous() {
    pagination(pageIndex-1)
}


