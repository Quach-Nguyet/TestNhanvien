

//định dạng ngày
Inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy" }).mask(".js-date");

$(document).ready(() => {

    // keyup, keydown, change, keypress
    $("#Sreach").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#tableNhanVien tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
       });

    // lưu form sửa
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
                url: 'Nv/Edit?=' + nv.Id,
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

    $('.filter-phong-ban').on('change', function () {
        pagination(1)
    })
    $("#SuaNhanVien").on('modal.show', function () {
        console.log("Show");
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
        console.log(nv)
        $('#form-sua').find('#Sua_MaNhanVien').val(nv.MaNhanVien);
        $('#form-sua').find('#Sua_HoVaTen').val(nv.HoVaTen);
        $('#form-sua').find('#Sua_NgaySinh').val(moment(nv.NgaySinh).format('DD/MM/YYYY'));
        $('#form-sua').find('#Sua_DiaChi').val(nv.DiaChi);
        $('#form-sua').find('#Sua_SoDienThoai').val(nv.SoDienThoai);
        $('#form-sua').find('#Sua_ChucVu').val(nv.ChucVu);
        $('#form-sua').find('#Sua_SoNamCongTac').val(nv.SoNamCongTac);
            $('#SuaNhanVien').modal('show');
    }).catch(error => console.log(error))
}


let pageIndex = 1
let roomId = 0
function room(id) {
        console.log(id)
        roomId = id;
        $('#nhanvien_table').load('/nv/table?Id=' + id);
}

function pagination(page) {
    pageIndex = page;
    $('#nhanvien_table').load('/nv/table?page=' + page + '&id=' + $('.filter-phong-ban').val());
}



function next() {
    pagination(pageIndex+1)
}

function previsous() {
    pagination(pageIndex-1)
}



