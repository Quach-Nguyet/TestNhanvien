$(document).ready(function () {

    $(document).ready(function () {
        $("#Sreach").on("keyup", function () {
            var value = $(this).val().toLowerCase();
            $("#tableNhanVien tr").filter(function () {
                $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
            });
        });
    });


     openEditPopup= function(maNhanVien) {
        $.ajax({
            method: "GET",
            url: "/Nv/Edit",
            data: { maNhanVien: maNhanVien }
        })
            .done(function (response) {
                if (response.success) {
                    $('#Sua_MaNhanVien').val(maNhanVien);
                    $('#Sua_HoVaTen').val(response.data.HoVaTen);
                    $('#Sua_NgaySinh').val(response.data.NgaySinh);
                    $('#Sua_DiaChi').val(response.data.DiaChi);
                    $('#Sua_SoDienThoai').val(response.data.SoDienThoai);
                    $('#Sua_ChucVu').val(response.data.ChucVu);
                    $('#Sua_SoNamCongTac').val(response.data.SoNamCongTac);
                    $('#SuaNhanVien').modal('show');
                } else {
                    alert(response.msg);
                }

            });
    }

    $(document).on("click", ".btnEdit", function () {
        let id = $(this).attr("data-id");
        openEditPopup(id);
    })



    themNhanVien = function () {
        $.ajax({
            method: "GET",
            url: "/Nv/Create"
        })
            .done(function (response) {
                if (response.success) {
                    $('#MaNhanVien').val();
                    $('#HoVaTen').val();
                    $('#NgaySinh').val();
                    $('#DiaChi').val();
                    $('#SoDienThoai').val();
                    $('#ChucVu').val();
                    $('#SoNamCongTac').val();
                    $('#ThemNhanVien').modal('show');
                } else {
                    alert(response.msg);
                }

            });
    }

    $(document).on("click", ".btnCreate", function () {
        themNhanVien();
    })
    //
    $('#Them').submit(function (e) {
        e.preventDefault();
        let nv = {
            MaNhanVien: $('#MaNhanVien').val(),
            HoVaTen: $('#HoVaTen').val(),
            NgaySinh: $('#NgaySinh').val(),
            SoDienThoai: $('#SoDienThoai').val(),
            DiaChi: $('#DiaChi').val(),
            ChucVu: $('#ChucVu').val(),
            SoNamCongTac: $('#SoNamCongTac').val()
        }

        $.ajax({
            method: "POST",
            url: "/Nv/Create",
            data: nv
        })
            .done(function (response) {
                if (response.success) {
                    console.log(response.success);
                    $('#ThemNhanVien').modal('hide');
                    alert('Lưu thành công!');
                }
            });
    });
    $('#Sua').onclick(function (ex) {
        ex.preventDefault();
        let nvNew = {
            MaNhanVien: $('#Sua_MaNhanVien').val(),
            HoVaTen: $('#Sua_HoVaTen').val(),
            NgaySinh: $('#Sua_NgaySinh').val(),
            SoDienThoai: $('#Sua_SoDienThoai').val(),
            DiaChi: $('#Sua_DiaChi').val(),
            ChucVu: $('#Sua_ChucVu').val(),
            SoNamCongTac: $('#Sua_SoNamCongTac').val()
        }

        $.ajax({
            method: "POST",
            url: "/Nv/Edit",
            data: nvNew
        })
            .done(function (response) {
                if (response.success) {
                    $('#SuaNhanVien').modal('hide');
                    alert('Lưu thành công!!!');
                }
            });
    });

  
});