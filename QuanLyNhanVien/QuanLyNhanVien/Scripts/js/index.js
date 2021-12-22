

$(document).ready(() => {
    //định dạng ngày
    Inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy" }).mask(".js-date");


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
                url: 'Nv/Edit',
                method: 'POST',
                data: nv,
                success: respon => {
                    if (respon.status) {
                        $('#SuaNhanVien').modal('hide');
                        $('#nhanvien_table').load('/nv/table');
                        //table_nv.columns.adjust().draw();

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
            url: '/Nv/Create',
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

    //$(document).on('click', '.pb-item', function () {
    //    let phong_ban = $(this).attr('data-name');
    //    $('#nhanvien_table').load(`/nv/table?name="${phong_ban}"`);
    //})

    $('select.dropdown').each(function () {

        var dropdown = $('<div />').addClass('dropdown selectDropdown');

        $(this).wrap(dropdown);

        var label = $('<span />').text($(this).attr('placeholder')).insertAfter($(this));
        var list = $('<ul />');

        $(this).find('option').each(function () {
            list.append($('<li />').append($('<a />').text($(this).text())));
        });

        list.insertAfter($(this));

        if ($(this).find('option:selected').length) {
            label.text($(this).find('option:selected').text());
            list.find('li:contains(' + $(this).find('option:selected').text() + ')').addClass('active');
            $(this).parent().addClass('filled');
        }

    });

    $(document).on('click touch', '.selectDropdown ul li a', function (e) {
        e.preventDefault();
        var dropdown = $(this).parent().parent().parent();
        var active = $(this).parent().hasClass('active');
        var label = active ? dropdown.find('select').attr('placeholder') : $(this).text();
        let phong_ban = $(this).attr('data-name');
        console.log(phong_ban)
        $('#nhanvien_table').load(`/nv/table?Id="${phong_ban}"`);

        dropdown.find('option').prop('selected', false);
        dropdown.find('ul li').removeClass('active');

        dropdown.toggleClass('filled', !active);
        dropdown.children('span').text(label);

        if (!active) {
            dropdown.find('option:contains(' + $(this).text() + ')').prop('selected', true);
            $(this).parent().addClass('active');
        }

        dropdown.removeClass('open');
    });

    $('.dropdown > span').on('click touch', function (e) {
        var self = $(this).parent();
        self.toggleClass('open');
    });

    $(document).on('click touch', function (e) {
        var dropdown = $('.dropdown');
        if (dropdown !== e.target && !dropdown.has(e.target).length) {
            dropdown.removeClass('open');
        }
    });
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
        $('#form-sua').find('#Sua_NgaySinh').val(moment(nv.NgaySinh).format('DD-MM-YYYY'));
        $('#form-sua').find('#Sua_DiaChi').val(nv.DiaChi);
        $('#form-sua').find('#Sua_SoDienThoai').val(nv.SoDienThoai);
        $('#form-sua').find('#Sua_ChucVu').val(nv.ChucVu);
        $('#form-sua').find('#Sua_SoNamCongTac').val(nv.SoNamCongTac);
        $('#SuaNhanVien').modal('show');
    }).catch(error => console.log(error))
}

let pageIndex = 1
let roomId = 0 
function pagination(page) {
    pageIndex = page;
            $('#nhanvien_table').load('/nv/table?page=' +page);
}

function room(id) {
    console.log(id)
    roomId = id;
    $('#nhanvien_table').load('/nv/table?Id=' + id);
    paginaton(pageIndex)
}

function next() {
    pagination(pageIndex+1)
}

function previsous() {
    pagination(pageIndex-1)
}

