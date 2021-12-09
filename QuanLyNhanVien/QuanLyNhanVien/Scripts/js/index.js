
   
function loadTable() {
    $('#ds_nhanvien').load("/Nv/_DanhSachNV");
}


$(document).ready(() => {
    //định dạng ngày
    Inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy" }).mask(".js-date");

       //Tìm kiếm 
    $("#Sreach").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#tableNhanVien tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });

    //$('#bangNhanVien').DataTable({
    //    "autoWidth": true,
    //    "processing": true, 
    //    "serverSide": true,   
    //    "filter": false,
    //    "orderMulti": false, 
    //    "ajax": {
    //        "url": "/Nv/LoadData",
    //        "type": "GET",
    //        "datatype": "json"
    //    },
    //    "columnDefs": [{
    //        "targets": [0],
    //        "visible": true,
    //        "searchable": false
    //    }],
    //    'columns': [
    //        { 'data': 'MaNhanVien', 'name': 'Mã nhân viên', 'autoWith': true },
    //        { 'data': 'HoVaTen', 'name': 'Tên nhân viên', 'autoWith': true },
    //        { 'data': 'NgaySinh', 'name': 'Ngày sinh', 'autoWith': true },
    //        { 'data': 'DiaChi', 'name': 'Địa chỉ', 'autoWith': true },
    //        { 'data': 'SoDienThoai', 'name': 'Số điện thoại', 'autoWith': true },
    //        { 'data': 'ChucVu', 'name': 'Chức vụ', 'autoWith': true },
    //        { 'data': 'SoNamCongTac', 'name': 'Số năm công tác', 'autoWith': true },
    //        {
    //                "render": function (data, type, full, meta) { return `<button class="btn btn-outline-secondary btnEdit" data-id="${item.MaNhanVien}" >Sửa</button>`; }
    //        },
    //        {
    //            data: null,
    //            render: function (data, type, row) {
    //                return ` <button class="btn btn-outline-danger btnDelete" data-id="${item.MaNhanVien}"onclick="$('#XoaNhanVien').modal('show')">Xóa</button>`; }
    //        },
    //    ]
    //});

        $.get('/nv/DanhSachNv').done(response => {
            let tbody = '<tr>'
            response.forEach(item => {
                tbody += `<tr id=${item.MaNhanVien} >`
                tbody += `<td>${item.MaNhanVien}</td>`
                tbody += `<td>${item.HoVaTen}</td>`
                tbody += `<td>${moment(item.NgaySinh).format("DD/MM/YYYY")}</td>`
                tbody += `<td>${item.DiaChi}</td>`
                tbody += `<td>${item.SoDienThoai}</td>`
                tbody += `<td>${item.ChucVu}</td>`
                tbody += `<td>${item.SoNamCongTac}</td>`
                tbody += `<td><button class="btn btn-outline-secondary btnEdit" data-id="${item.MaNhanVien}" >Sửa</button> 
                      <button class="btn btn-outline-danger btnDelete" data-id="${item.MaNhanVien}"onclick="$('#XoaNhanVien').modal('show')">Xóa</button></td>`
                tbody += '</tr>'
            })
            $('#bangNhanVien tbody').append(tbody);
        }).catch(error => {
            console.log(error);
        })
    

    $('#XoaNhanVien').on('click', '#Xoa', function () {
        const ma = $('.btnDelete').attr('data-id')
        $.ajax('/Nv/Delete?ma=' + ma).then(response => {
            if (response.status == true)
                location.reload();
        //    {
        //        $('#XoaNhanVien').modal('hide');
        //        $(`#tableNhanVien tr #${ma}`).remove();
        //        //loadTable();
        //    }
        }).catch(error => console.log(error))
    })

   
    ////
$('#tableNhanVien').on('click', '.btnEdit', function () {

    const id = $(this).attr('data-id')
    $.ajax('/Nv/Edit?id=' + id).then(respon => {
        const nv = respon.data
        $('#form-sua').find('#Sua_MaNhanVien').val(respon.data.MaNhanVien);
        $('#form-sua').find('#Sua_HoVaTen').val(respon.data.HoVaTen);
        $('#form-sua').find('#Sua_NgaySinh').val(moment(respon.data.NgaySinh).format("DD/MM/YYYY"));
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

$('#form-them').on('click', '#Them', function (e) {
    e.preventDefault()
        const data =  {
            MaNhanVien: $('#ThemNhanVien').find('#MaNhanVien').val(),
            HoVaTen: $('#ThemNhanVien').find('#HoVaTen').val(),
            NgaySinh: $('#ThemNhanVien').find('#NgaySinh').val(),
            SoDienThoai: $('#ThemNhanVien').find('#SoDienThoai').val(),
            DiaChi: $('#ThemNhanVien').find('#DiaChi').val(),
            ChucVu: $('#ThemNhanVien').find('#ChucVu').val(),
            SoNamCongTac: $('#ThemNhanVien').find('#SoNamCongTac').val(),

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
    if (!isValidate) return

    $.ajax({
        url: '/Nv/Create',
        method: 'POST',
        data: data,
        success: respon => {
            if (respon.status == true) {
                $('#ThemNhanVien').modal('hide');
                alert('Lưu thành công!');
                location.reload()
                //$('#bangNhanVien').append(`<tr><td>${data.MaNhanVien}</td><td>${data.HoVaTen}</td><td>${data.NgaySinh}</td><td>${data.SoDienThoai}</td><td>${data.DiaChi}</td><td>${data.ChucVu}</td><td>${data.SoNamCongTac}</td> 
                //      <td><button class="btn btn-outline-secondary btnEdit" data-id="${data.MaNhanVien}" >Sửa</button>
                //          <button class="btn btn-outline-danger btnDelete" data-id="${data.MaNhanVien}"onclick="$('#XoaNhanVien').modal('show')">Xóa</button>
                //      </td> </tr >`);
            }
            else {
                $.toast({
                    heading: 'Thông báo',
                    text: respon.message,
                    showHideTransition: 'fade',
                    icon: 'error'
                })
            }
        },
        error: respon => $.toast({
            heading: 'Thông báo',
            text: respon.message,
            showHideTransition: 'fade',
            icon: 'error'
        })
    })
})
});

