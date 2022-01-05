
let pageIndex = 1
let pageSize = 1
let roomId = 0
let maxPage = 0
let stt = 1
let index = 1
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
                url: '/nv/Edit',
                method: 'POST',
                data: nv,
                success: respon => {
                    if (respon.status) {
                        $('#SuaNhanVien').modal('hide');
                        pagination();
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
    $('#Them').on('click', function (e) {
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
            url: '/Nv/Create' ,
            method: 'POST',
            data: data,
            success: respon => {
                if (respon.success) {
                    $('#ThemNhanVien').modal('hide');
                    alert('Lưu thành công!');
                    pagination();
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
            error:
                respon => $.toast({
                    heading: 'Thông báo',
                    text: "Sever không nhận được dữ liệu",
                    showHideTransition: 'fade',
                    icon: 'error',
                    position: 'top-center'
                })
            
        })
    })

    //Xóa nhân viên
    $('#form-data').on('click', ".btnXoa", function (e) {
        e.preventDefault()
        const idXoa = $(this).attr('data-id')
        deleteNV(idXoa)
    })

    //Phân trang theo ban
    $('.filter-phong-ban').on('change', function () {
        pagination(1)
    })

    $('.filter-page-size').on('change', function () {
        pagination(1)
    })

    //Tìm kiếm
    $("#form-data").on('click', "#btnSreach", function (e) {
        e.preventDefault()
        const key = $('#keySreach').val();
        $('#nhanvien_table').load(`/nv/table?page=${pageIndex}&page_size=${$('.filter-page-size').val()}&id=${$('.filter-phong-ban').val()}&keyword=${key}`);
    })

    //Xuất Ex
    $("#form-data").on('click', "#Export", function (e) {
        e.preventDefault()
        location.href = "/nv/export?page_size=" + $('.filter-page-size').val() +"&phongbanid=" + $('.filter-phong-ban').val() + "&keyword=" + $('#keySreach').val();
    })
});

function deleteNV(id, tableRow) {
    let confirmed = confirm("Bạn có chắc chắn muốn xóa nhân viên này?");
    if (confirmed) {
        $.ajax('/Nv/Delete?ma=' + id).then(respon => {
            if (respon.success) {
                pagination();
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
    
    })
}

function pagination(page) {
    pageIndex = page;
    $('#nhanvien_table').load('/nv/table?page=' + page + '&page_size=' + $('.filter-page-size').val() + '&phongbanid=' + $('.filter-phong-ban').val() + '&keyword=' + $('#keySreach').val()
        + '&index=' + $('.Stt').val()
    );
}

function next(max) {
    maxPage= max;
    if (pageIndex < max) {
        pagination(pageIndex + 1)
    }
}

function previsous() {
    if (pageIndex > 1) {
        pagination(pageIndex - 1)
    }
}




