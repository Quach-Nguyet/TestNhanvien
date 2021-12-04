$(document).ready(function () {
    //định dạng ngày
    Inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy" }).mask(".js-date");

    //Get List

    //Tìm kiếm sách
    $(document).ready(function () {
        $("#Sreach").on("keyup", function () {
            var value = $(this).val().toLowerCase();
            $("#tableNhanVien tr").filter(function () {
                $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
            });
        });
    });

    //// Get sửa
    // openEditPopup= function(maNhanVien) {
    //    $.ajax({
    //        method: 'GET',
    //        url: '/Nv/Edit',
    //        data: { id: maNhanVien }
    //    })
    //        .done(function (response) {
    //            if (response.success) {
    //                $('#Sua_MaNhanVien').val(maNhanVien);
    //                $('#Sua_HoVaTen').val(response.data.HoVaTen);
    //                $('#Sua_NgaySinh').val(response.data.NgaySinh);
    //                $('#Sua_DiaChi').val(response.data.DiaChi);
    //                $('#Sua_SoDienThoai').val(response.data.SoDienThoai);
    //                $('#Sua_ChucVu').val(response.data.ChucVu);
    //                $('#Sua_SoNamCongTac').val(response.data.SoNamCongTac);
    //                $('#SuaNhanVien').modal('show');
    //            } else {
    //                alert(response.msg);
    //            }

    //        });
    //}

    ////Get thêm
    //themNhanVien = function () {
    //    $.ajax({
    //        method: 'GET',
    //        url: '/Nv/Create'
    //    })
    //        .done(function (response) {
    //            if (response.success) {
    //                $('#MaNhanVien').val();
    //                $('#HoVaTen').val();
    //                $('#NgaySinh').val();
    //                $('#DiaChi').val();
    //                $('#SoDienThoai').val();
    //                $('#ChucVu').val();
    //                $('#SoNamCongTac').val();
    //                $('#ThemNhanVien').modal('show');
    //            } else {
    //                alert(response.msg);
    //            }

    //        });
    //}

    //// hành động khi click nút thêm
    //$(document).on("click", ".btnCreate", function () {
    //    themNhanVien();
    //})

    ////hành động khi click nút lưu
    //$('#Them').click(function (e) {
    //    e.preventDefault();
    //    const nv = {
    //        MaNhanVien: $('#MaNhanVien').val(),
    //        HoVaTen: $('#HoVaTen').val(),
    //        NgaySinh: $('#NgaySinh').val(),
    //        SoDienThoai: $('#SoDienThoai').val(),
    //        DiaChi: $('#DiaChi').val(),
    //        ChucVu: $('#ChucVu').val(),
    //        SoNamCongTac: $('#SoNamCongTac').val()
    //    }
    //    $.ajax({
    //        method: 'POST',
    //        url: '/nv/create',
    //        data: nv
    //    })
    //        .done(function (response) {
    //            if (response.success) {
    //                $('#ThemNhanVien').modal('hide');
    //                alert('Lưu thành công!');

    //                //them row moi vao bang
    //                $('#bangnhanvien').append('<tr><td>' + nv.MaNhanVien + '</td><button onclick="edit(this)"></button></tr>');

    //            } else {
    //                alert(response.msg);
    //            }
                   
    //        });
    //});

    ////Hành động khi click nút sửa và lưu
    //$('#Sua').click(function (ex) {
    //    ex.preventDefault();
    //    let nvNew = {
    //        MaNhanVien: $('#Sua_MaNhanVien').val(),
    //        HoVaTen: $('#Sua_HoVaTen').val(),
    //        NgaySinh: $('#Sua_NgaySinh').val(),
    //        SoDienThoai: $('#Sua_SoDienThoai').val(),
    //        DiaChi: $('#Sua_DiaChi').val(),
    //        ChucVu: $('#Sua_ChucVu').val(),
    //        SoNamCongTac: $('#Sua_SoNamCongTac').val()
    //    }

    //    $.ajax({
    //        method: "POST",
    //        url: "/Nv/Edit",
    //        data: nvNew
    //    })
    //        .done(function (response) {
    //            $('#SuaNhanVien').modal('hide');
    //            alert('Lưu thành công!!!');
    //        });
    //});

    ////Get DELETE
    //xoaNhanVien = function (ma) {
    //    $.ajax({
    //        method: 'GET',
    //        url: '/Nv/delete',
    //        data: { ma: ma }
    //    })
    //        .done(function (response) {
    //            $('#Xoanv-@item.MaNhanVien').modal('show');
    //        })
    //}

    //$(document).on("click", ".btnDelete", function () {
    //    xoaNhanVien();
    //})

    //$("#XoaNhanVien").click(function () {
    
});

//function edit(btn) {
//    let id = $(btn).attr("data-id");
//    openEditPopup(id);
//}

//function loadTable() {
//    $('#ds_nhanvien').load("/Nv/DanhSachNv")
//}


$(document).ready(() => {

    $.get('/nv/DanhSachNv').done(response => {
        let tbody = '<tr>'
        response.forEach(item => {
            tbody += '<tr>'
            tbody += `<td>${item.MaNhanVien}</td>`
            tbody += `<td>${item.HoVaTen}</td>`
            tbody += `<td>${moment(item.NgaySinh).format("DD/MM/YYYY")}</td>`
            tbody += `<td>${item.DiaChi}</td>`
            tbody += `<td>${item.SoDienThoai}</td>`
            tbody += `<td>${item.ChucVu}</td>`
            tbody += `<td>${item.SoNamCongTac}</td>`
            tbody += `<td><button class="btn btn-outline-secondary btnEdit" data-id="${item.MaNhanVien}" >Sửa</button> 
                      <button class="btn btn-outline-danger btnXoa" data-id="${item.MaNhanVien}">Xóa</button></td>`
            tbody += '</tr>'
        })
        $('#bangNhanVien tbody').append(tbody)
               }).catch(error => {
        console.log(error)
    })

    ////
$('#tableNhanVien').on('click', '.btnEdit', function () {

    const id = $(this).attr('data-id')
    $.ajax('/Nv/Edit?id=' + id).then(respon => {
        const nv = respon.data
        console.log(respon)
        $('#form-sua').find('#Sua_MaNhanVien').val(respon.data.MaNhanVien);
        $('#form-sua').find('#Sua_HoVaTen').val(respon.data.HoVaTen);
        $('#form-sua').find('#Sua_NgaySinh').val(respon.data.NgaySinh);
        $('#form-sua').find('#Sua_DiaChi').val(respon.data.DiaChi);
        $('#form-sua').find('#Sua_SoDienThoai').val(respon.data.SoDienThoai);
        $('#form-sua').find('#Sua_ChucVu').val(respon.data.ChucVu);
        $('#form-sua').find('#Sua_SoNamCongTac').val(respon.data.SoNamCongTac);

        $('#SuaNhanVien').modal('show');
    }).catch(error => console.log(error))
})

// lưu form sửa
    $('#SuaNhanVien').on('click', '#Sua', function (e) {
        e.preventDefault()
    const data = {
        MaNhanVien: $('#form-sua').find('#Sua_MaNhanVien').val(),
        HoVaTen: $('#form-sua').find('#Sua_HoVaTen').val(),
        NgaySinh: $('#form-sua').find('#Sua_NgaySinh').val(),
        DiaChi: $('#form-sua').find('#Sua_DiaChi').val(),
        SoDienThoai: $('#form-sua').find('#Sua_SoDienThoai').val(),
        ChucVu: $('#form-sua').find('#Sua_ChucVu').val(),
        SoNamCongTac: $('#form-sua').find('#Sua_SoNamCongTac').val(),
    }

    let isValidate = true
    if (data.HoVaTen == null || data.HoVaTen == "") {
        $.toast({
            heading: 'Error',
            text: 'Họ và tên đang trống',
            showHideTransition: 'fade',
            icon: 'error'
        })
        isValidate = false
    }

    if (data.NgaySinh == null || data.NgaySinh == "") {
        $.toast({
            heading: 'Error',
            text: 'Ngày sinh đang trống',
            showHideTransition: 'fade',
            icon: 'error'
        })
        isValidate = false
    }

    if (data.SoDienThoai == null || data.SoDienThoai == "") {
        $.toast({
            heading: 'Error',
            text: 'Số điện thoại đang trống',
            showHideTransition: 'fade',
            icon: 'error'
        })
        isValidate = false
    }

    if (data.ChucVu == null || data.ChucVu == "") {
        $.toast({
            heading: 'Error',
            text: 'Chức vụ đang trống',
            showHideTransition: 'fade',
            icon: 'error'
        })
        isValidate = false
    }

    $.ajax({
        url: 'Nv/Edit',
        method: 'POST',
        data: data,
        success: respon => {
            if (respon.status == true) location.reload()
            else {
                $.toast({
                    heading: 'Thông báo',
                    text: respon.message,
                    showHideTransition: 'fade',
                    icon: 'error'
                })
            }
        },
        error:respon=> $.toast({
                        heading: 'Thông báo',
                        text: respon.message,
                        showHideTransition: 'fade',
                        icon: 'error'
                    })
    })
})

$('#ThemNhanVien').on('click', '.btnCreate', function (e) {
    e.preventDefault()
        const data = {
            MaNhanVien: $('#form-them').find('#MaNhanVien').val(),
            MaNhanVien: $('#form-them').find('#HoVaTen').val(),
            MaNhanVien: $('#form-them').find('#NgaySinh').val(),
            MaNhanVien: $('#form-them').find('#SoDienThoai').val(),
            MaNhanVien: $('#form-them').find('#DiaChi').val(),
            MaNhanVien: $('#form-them').find('#ChucVu').val(),
            MaNhanVien: $('#form-them').find('#SoNamCongTac').val(),

        }

    let isValidate = true
    if (data.HoVaTen == null || data.HoVaTen == "") {
        $.toast({
            heading: 'Error',
            text: 'Họ và tên đang trống',
            showHideTransition: 'fade',
            icon: 'error'
        })
        isValidate = false
    }

    if (data.NgaySinh == null || data.NgaySinh == "") {
        $.toast({
            heading: 'Error',
            text: 'Ngày sinh đang trống',
            showHideTransition: 'fade',
            icon: 'error'
        })
        isValidate = false
    }

    if (data.SoDienThoai == null || data.SoDienThoai == "") {
        $.toast({
            heading: 'Error',
            text: 'Số điện thoại đang trống',
            showHideTransition: 'fade',
            icon: 'error'
        })
        isValidate = false
    }

    if (data.ChucVu == null || data.ChucVu == "") {
        $.toast({
            heading: 'Error',
            text: 'Chức vụ đang trống',
            showHideTransition: 'fade',
            icon: 'error'
        })
        isValidate = false
    }

    $.ajax({
        url: '/Nv/Create',
        method: 'POST',
        data: data,
        success: respon => {
            if (respon.status == true) location.reload()
            else {
                $.toast({
                    heading: 'Thông báo',
                    text: respon.message,
                    showHideTransition: 'fade',
                    icon: 'error'
                })
            }
        },
        error:respon=> $.toast({
            heading: 'Thông báo',
            text: respon.message,
            showHideTransition: 'fade',
            icon: 'error'
        })

    })
})
//console.log($('#form-them'))
});

