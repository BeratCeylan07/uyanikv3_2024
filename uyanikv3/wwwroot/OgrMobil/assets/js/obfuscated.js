function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}


$(document).on('click', '[data-dismiss="modal"]', function () {
    $(".modal").modal('hide');
    $(".modal fade").remove();
    $(".modal-backdrop ").remove();
    $(".show").remove();
    document.getElementById("modalArea").innerHTML = "";

});
let kontenjan;
let msg;
let alertColor;
function bildirimPanel(id){
    let panel = `  <div class="modal fade dialogbox" id="bildirimPanelDialog" data-bs-backdrop="static" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document">
                <div class="modal-content"   style="max-width: 350px !important;     box-shadow: 1px 0px 24px 0px;" >
                    <div class="modal-header">
                        <h5 class="modal-title">Deneme Sınavı Toplu Bildirim Paneli</h5>
                    </div>
                    <form>
                        <div class="modal-body text-start mb-2">
                            <div class="form-group basic">
                                <div class="input-wrapper">
                                    <label class="form-label" for="txtSnsBildirimBaslik">Bildirim Başlık</label>
                                    <input type="text" class="form-control" id="txtSnsBildirimBaslik" placeholder="Bildirim Başlığı">
                                    <i class="clear-input">
                                        <ion-icon name="close-circle"></ion-icon>
                                    </i>
                                </div>
                                <hr />
                                <div class="input-wrapper">
                                    <label class="form-label" for="txtSnsBildirimMesaj">Bildirim Mesaj</label>
                                    <textarea type="text" class="form-control" id="txtSnsBildirimMesaj" style="height:150px;" placeholder="Bildirim İçeriğiniz"></textarea>
                                    <i class="clear-input">
                                        <ion-icon name="close-circle"></ion-icon>
                                    </i>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <div class="btn-inline">
                                <button type="button" class="btn btn-text-secondary"
                                    onclick="$('#bildirimPanelDialog').modal('hide');">Vazgeç</button>
                                <button type="button" class="btn btn-text-primary" id="btnBildirimPush">Gönder</button>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
        <!-- * Dialog Form -->`;
    
    $("#modalArea2").html(panel);
    $("#bildirimPanelDialog").modal('show');
    $("#btnBildirimPush").click(function (){
        const swalWithBootstrapButtons = Swal.mixin({
            customClass: {
                confirmButton: 'btn btn-success',

                cancelButton: 'btn btn-danger swButton'
            },
            buttonsStyling: false
        })

        swalWithBootstrapButtons.fire({
            title: 'İşlemi Onaylıyor musunuz?',
            text: "İlgili Seansa Kayıtlı Tüm Öğrencilere Bildirim Gönderilecektir",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Onaylıyorum',
            cancelButtonText: 'İptal Et',
            allowOutsideClick: false,
            allowEscapeKey: false,
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                var mesajBaslik = $("#txtSnsBildirimBaslik").val();
                var mesaj = $("#txtSnsBildirimMesaj").val();
                var obj = {
                    seansID: id,
                    bildirimMesaj: mesaj,
                    bildirimTitle: mesajBaslik
                }

                $.ajax({
                    url: '/App/seansBildirimPush/',
                    type: 'Post',
                    data: { model: obj },
                    dataType: 'json',
                    success: function (data) {
                        Swal.fire({
                            title: data.msgTitle,
                            html: data.msg,
                            icon: data.msgIcon,
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'Tamam'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                if (data.msgIcon == "warning") {

                                } else {

                                }
                            }
                        })
                    }
                });
            } else if (
                result.dismiss === Swal.DismissReason.cancel
            ) {
                Swal.fire({
                    title: '<strong>İşlem İptal Edildi</strong>',
                    icon: 'info',
                    html:
                        '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                    confirmButtonText:
                        'Kapat'
                })
            }
        });
    })

}

function customSeansOfOgrAdd(seansID)    {
    let seansAd;
    $.ajax({        
        url: '/App/seansInfo/',
        contentType: 'application/json; charset=utf-8;',
        data: { seansID: seansID },
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            var seansAdResult = jQuery.parseJSON(data);
            seansAd = "";
            seansAd += seansAdResult;
        }
    });
    $.ajax({
        url: '/App/drpOgrSeans/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        data: { seansID: seansID },
        dataType: 'json',
        success: function (data) {
            var ogrObjData = jQuery.parseJSON(data.ogrJSON);
            let ogrSeansSetModal =
                `
                            <div class="modal fade modalbox" id="mdlCustomSeansOfOgrAdd" data-backdrop="static" tabindex="-1" role="dialog">
                                <div class="modal-dialog" role="document">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title">${seansAd}</h5>
                                            <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                        </div>
                                        <div class="modal-body">
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                            <label class="label" for="txtOgrAd">Öğrenci</label>
                                                            <select id="drpOgrList" class="form-control form-control-lg">`
            $.each(ogrObjData, (index, value) => {
                ogrSeansSetModal += `<option value="${value.Id}">${value.Ad} ${value.Soyad}</option>`;
            });
            ogrSeansSetModal += `</select>
                                                        </div>
                                                    </div>  
                                                    <div class="form-group basic">
                                                        <div class="input-wrapper">
                                                            <label class="label" for="drpSeansUcretDurum">Ücret Ödeme</label>
                                                            <select id="drpSeansUcretDurum" class="form-control form-control-lg">
                                                                <option value="1">Ödeme Alındı</option>
                                                                <option value="2">Ödeme Alınmadı</option>
                                                            </select>
                                                            </div>
                                                        </div>

                                            <div class="mt-2">
                                                <button id="btnCustomOgrSeansKayit" type="button" class="btn btn-primary btn-block btn-lg">Kaydet</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                                `;

            $("#modalArea").html(ogrSeansSetModal);
            $("#mdlCustomSeansOfOgrAdd").modal('show    ');
            $("#btnCustomOgrSeansKayit").click(function () {


                const swalWithBootstrapButtons = Swal.mixin({
                    customClass: {
                        confirmButton: 'btn btn-success',

                        cancelButton: 'btn btn-danger swButton'
                    },
                    buttonsStyling: false
                });

                swalWithBootstrapButtons.fire({
                    title: 'İşlemi Onaylıyor musunuz?',
                    text: "Seçilen Öğrenci İlgili Deneme Sınavı Seansına Kayıt Edilecektir",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Onaylıyorum',
                    cancelButtonText: 'İptal Et',
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    reverseButtons: true
                }).then((result) => {
                    if (result.isConfirmed) {
                        var ogr = $("#drpOgrList option:selected").val();
                        var odemeDurum = $("#drpSeansUcretDurum option:selected").val();
                        var obj = {
                            OgrId: ogr,
                            SeansId: seansID,
                            ucretOdemeDurum: odemeDurum
                        }
                        $.ajax({
                            url: '/App/ogrSeansKayit/',
                            type: 'Post',
                            data: { model: obj },
                            dataType: 'json',
                            success: function (data) {

                                Swal.fire({
                                    title: data.msgTitle,
                                    html: data.msg,
                                    icon: data.msgIcon,
                                    confirmButtonColor: '#3085d6',
                                    confirmButtonText: 'Tamam'
                                }).then((result) => {
                                    if (result.isConfirmed) {
                                        if (data.msgIcon == "warning") {

                                        } else {
                                            $("#mdlCustomSeansOfOgrAdd").modal('hide');
                                            $(".modal").modal('hide');
                                            $(".modal fade").remove();
                                            $(".modal-backdrop").remove();
                                        }
                                    }
                                })

                            }
                        });

                    } else if (
                        result.dismiss === Swal.DismissReason.cancel
                    ) {
                        Swal.fire({
                            title: '<strong>İşlem İptal Edildi</strong>',
                            icon: 'info',
                            html:
                                '<b>Herhangi Bir İşlem Yapılmadı</b>',

                            confirmButtonText:
                                'Kapat'
                        })
                    }
                });
            });
        }
    });

}
function ogrSeansSet() {
    $.ajax({
        url: '/App/drpOgrSeans/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            var ogrObjData = jQuery.parseJSON(data.ogrJSON);
            var seansObjData = jQuery.parseJSON(data.seansJSON);
            let ogrSeansSetModal =
                `
                            <div class="modal fade modalbox" id="mdlOgrSeansSet" data-backdrop="static" tabindex="-1" role="dialog">
                                <div class="modal-dialog" role="document">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title">Öğrenci - Deneme Sınav Kaydı</h5>
                                            <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                        </div>
                                        <div class="modal-body">
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                            <label class="label" for="txtOgrAd">Öğrenci</label>
                                                            <select id="drpOgrList" class="form-control form-control-lg">`
            $.each(ogrObjData, (index, value) => {
                ogrSeansSetModal += `<option value="${value.Id}">${value.Ad} ${value.Soyad}</option>`;
            });
            ogrSeansSetModal += `</select>
                                                        </div>
                                                    </div>
                                                    <div class="form-group basic">
                                                        <div class="input-wrapper">
                                                            <label class="label" for="txtSoyAd">Seans</label>
                                                                <select id="drpSeansList" class="form-control form-control-lg">`
            $.each(seansObjData, (index, value) => {
                ogrSeansSetModal += `<option value="${value.Id}">Deneme Adı: ${value.kitapcikBaslik} Tarih: ${value.TarihSTR} / Saat: ${value.Saat} Güncel Kontenjan: ${value.GuncelKontenjan} Ücret: ${value.SeansUcret} TL`;
            });
            ogrSeansSetModal += `</select>
                                                        </div>
                                                    </div>
                                                    <div class="form-group basic">
                                                        <div class="input-wrapper">
                                                            <label class="label" for="txtOgrAd">Ücret Ödeme</label>
                                                            <select id="drpSeansUcretDurum" class="form-control form-control-lg">
                                                                <option value="1">Ödeme Alındı</option>
                                                                <option value="2">Ödeme Alınmadı</option>
                                                            </select>
                                                            </div>
                                                        </div>

                                            <div class="mt-2">
                                                <button id="btnOgrSeansKayit" type="button" class="btn btn-primary btn-block btn-lg" onclick="ogrSeansKayitPush();">Kaydet</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                                `;

            $("#modalArea").html(ogrSeansSetModal);
            $("#mdlOgrSeansSet").modal('show');
        }
    });
}
function customStok(kitapcikID) {
    let customStokModal =
        `
                            <div class="modal fade modalbox" id="mdlCustomStok" data-backdrop="static" tabindex="-1" role="dialog">
                                <div class="modal-dialog" role="document">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title">Kiptaçık Stok İşlemleri</h5>
                                            <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                        </div>
                                        <div class="modal-body">
                                                        <div class="form-group basic">
                                                                <div class="input-wrapper">
                                                                    <label class="label" for="drpStokType">Deneme(Kitapçık) Seçiniz</label>
                                                                    <select id="drpStokType" class="form-control" onchange="btnEnable(2)">
                                                                    <option value="">Stok Türünü Seçiniz</option>
                                                                    <option value="1">Stok Giriş</option>
                                                                    <option value="2">Stok Çıkış(Düş)</option>
                                                                    </select>
                                                                    <i class="clear-input">
                                                                        <ion-icon name="close-circle"></ion-icon>
                                                                    </i>
                                                                </div>
                                                            </div>
                                                    <div class="form-group basic">
                                                            <div class="input-wrapper">
                                                                <label class="label" for="txtSAdet">Adet</label>
                                                                <input type="number" class="form-control" id="txtSAdet"
                                                                    placeholder="Stok Adedi" onkeypress="return isNumberKey(event)">
                                                                <i class="clear-input">
                                                                    <ion-icon name="close-circle"></ion-icon>
                                                                </i>
                                                            </div>
                                                        </div>
                                                <div class="mt-2">
                                                    <button id="btnStokIslem" type="button" class="btn btn-primary btn-block btn-lg" onclick="customStokPush(${kitapcikID});" disabled>Stok Kaydet</button>
                                                </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                                `;

    $("#modalArea").html(customStokModal);
    $("#mdlCustomStok").modal('show');
}
function ogrOnKayitList() {
    menuHide();
    $.ajax({
        url: '/app/ogrOnKayitList/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            let ogrListModal = `
    
    <div class="modal fade modalbox" id="mdlOgrOnKayitList">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Üyelik Ön Kayıt Talepleri</h5>
                <a href="#" data-dismiss="modal">Kapat</a>
            </div>
            <div class="modal-body" style="padding-top: 1rem !important; padding-left: 0px !important; padding-bottom: 0px !important; padding-right: 0px !important; >
    
                <div class="table-responsive">
                    <table class="table table-striped table-bordered table-sm">
                        <thead>
                            <tr>
                                <th scope="col">Kayıt Tarih</th>
                                <th scope="col">Adı</th>
                                <th scope="col">Soyadı</th>
                                <th scope="col">Telefon</th>
                                <th scope="col">Seçenekler</th>
                            </tr>
                        </thead>
                        <tbody id="ogrOnKayittBody">
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</div>
    `;
    $("#modalArea").html(ogrListModal);
    let tbodyResult =  ``;
            var ogrOnKayitData = jQuery.parseJSON(data);
            $.each(ogrOnKayitData, (index, value) => {
                tbodyResult += `<tr><td scope="row">${value.KtarihSTR}</td><td scope="row">${value.Ad}</td><td scope="row">${value.Soyad}</td><td scope="row">${value.Telefon}</td><td><button type="button" class="btn btn-success btn-sm" onclick="uyelikOnKayitOnay(${value.Id})"><ion-icon name="shield-checkmark-outline"></ion-icon> Onayla</button></td></tr>`;
            });
            $("#ogrOnKayittBody").html(tbodyResult);
            $("#mdlOgrOnKayitList").modal('show');
        }
    });
    

}
function uyelikOnKayitOnay(id){
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "Üyelik Ön Kayıt Talebi Onaylanacaktır",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
        
            $.ajax({
                url: '/App/uyelikOnKayitOnay/',
                type: 'Post',
                data: { ogrID: id },
                dataType: 'json',
                success: function (data) {
                    Swal.fire({
                        title: data.msgTitle,
                        html: data.msg,
                        icon: data.msgIcon,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'Tamam'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            if (data.msgIcon == "warning") {

                            } else {
              
                            }
                        }
                    })
                }
            });
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });
}
function uyeGirisler() {
        $.ajax({
            url: '/App/uyeGirisler/',
            contentType: 'application/json; charset=utf-8;',
            type: 'Get',
            dataType: 'json',
            success: function (data) {
                var uyeGirisData = jQuery.parseJSON(data);
                let uyeGirismodalSet =
                    `
                    <div class="modal fade modalbox" id="mdlUyeGirisList" data-backdrop="static" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Üye Girişler</h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body">
                                    <div id="ogrTableBody">
                                          <div class="card text-white bg-secondary mb-2">
                                                <div class="card-body">
 <table id="tblOgrList" class="table table-sm text-white" style="border-radius: 10px; width:100% !important; font-weight: bolder;">
                                                                                        <thead>
                                                                                            <tr>
                                                                                                <th class="text-white"><b>Tarih / Saat</b></th>
                                                                                                <th class="text-white"><b>Ad - Soyad</b></th>
                                                                                                <th class="text-white"></th>
                                                                                            </tr>
                                                                                        </thead>
                                                                                        <tbody>`
                $.each(uyeGirisData, (index, value) => {
                    uyeGirismodalSet += `<tr>
                                                                        <td width="auto">${value.tarihStr} (${value.gun}) / ${value.Saat} </td>
                                                                        <td width="auto">${value.AdSoyad}</td>
                                                                        <td width="10px">
                                                                       <button type="button" class="btn btn-danger btn-block" onclick="uyeGirisAction(${value.Id},0);">Giriş İptal</button></td>

                                                                </tr>`;
                });
                uyeGirismodalSet += `</tbody>
                                                                                    </table>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>`;

                $("#modalArea").html(uyeGirismodalSet);
                $.fn.DataTable.ext.pager.numbers_length = 0;
                $('#tblOgrList').DataTable({
                    "language": {
                        "url": "//cdn.datatables.net/plug-ins/1.12.1/i18n/tr.json"
                    },
                    "pagingType": "full_numbers"
                });
                $("#mdlUyeGirisList").modal('show');

            }
        });
}
function customStokPush(kitapcikID) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "Stok Hareketi Kaydedilecektir",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            var stokType = $("#drpStokType option:selected").val();
            var adet = $("#txtSAdet").val();
            var obj = {
                DenemeId: kitapcikID,
                StokType: stokType,
                Adet: adet
            }
            $.ajax({
                url: '/App/denemeStokAdd/',
                type: 'Post',
                data: { model: obj },
                dataType: 'json',
                success: function (data) {
                    Swal.fire({
                        title: data.msgTitle,
                        text: data.msg,
                        icon: data.msgIcon,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'Tamam'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            if (data.msgIcon == "warning") {

                            } else {
                                $("#mdlDenemeStoklar").modal('hide');
                                $(".modal").modal('hide');
                                $(".modal fade").remove();
                                $(".modal-backdrop").remove();
                            }
                        }
                    })
                }
            });
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });

}
function btnEnable(typ) {
    var val;
    if (typ == 1) {
        val = $("#drpDenemeKitapcik option:selected").val();
    } else if (typ == 2) {
        val = $("#drpStokType option:selected").val();

    }
    if (val == 1 || val == 2) {
        $('#btnStokIslem').prop('disabled', false);
    } else {
        $('#btnStokIslem').prop('disabled', true);
    }
}
function stokKayitSil(stkID) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "Kitapçık Stok Kaydı Silinecektir",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/App/stokKayitSil/',
                type: 'Post',
                data: { stokID: stkID },
                dataType: 'json',
                success: function (data) {
                    Swal.fire({
                        title: data.msgTitle,
                        text: data.msg,
                        icon: data.msgIcon,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'Tamam'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            if (data.msgIcon == "warning") {

                            } else {
                                $(".modal").modal('hide');
                                $(".modal fade").remove();
                                $(".modal-backdrop").remove();
                            }
                        }
                    })
                }
            });
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });
}
function ogrKayitSil(ogrID) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "Seçilen Öğrenci Kaydı Silinecek Olup, Kayıtlı Olduğu Tüm Deneme Sınavlarının Kaydı İptal Edilecektir.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/App/ogrSil/',
                type: 'Post',
                data: { ogrID: ogrID },
                dataType: 'json',
                success: function (data) {
                    Swal.fire({
                        title: data.msgTitle,
                        html: data.msg,
                        icon: data.msgIcon,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'Tamam'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            if (data.msgIcon == "warning") {

                            } else {
                                $(".modal").modal('hide');
                                $(".modal fade").remove();
                                $(".modal-backdrop").remove();
                            }
                        }
                    })
                }
            });
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });
}
function kitapcikSil(kitapcikID) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "Kitapçık Silinecektir",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/App/kitapcikSil/',
                type: 'Post',
                data: { kitapcikID: kitapcikID },
                dataType: 'json',
                success: function (data) {
                    Swal.fire({
                        title: data.msgTitle,
                        text: data.msg,
                        icon: data.msgIcon,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'Tamam'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            if (data.msgIcon == "warning") {

                            } else {
                                $(".modal").modal('hide');
                                $(".modal fade").remove();
                                $(".modal-backdrop").remove();
                            }
                        }
                    })
                }
            });
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });
}
function ogrSeansKayitPush() {


    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    });

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "Seçilen Öğrenci İlgili Deneme Sınavı Seansına Kayıt Edilecektir",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            var ogr = $("#drpOgrList option:selected").val();
            var seans = $("#drpSeansList option:selected").val();
            var odemeDurum = $("#drpSeansUcretDurum option:selected").val();
            var obj = {
                OgrId: ogr,
                SeansId: seans,
                ucretOdemeDurum: odemeDurum
            }
            $.ajax({
                url: '/App/ogrSeansKayit/',
                type: 'Post',
                data: { model: obj },
                dataType: 'json',
                success: function (data) {
                    Swal.fire({
                        title: data.msgTitle,
                        html: data.msg,
                        icon: data.msgIcon,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'Tamam'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            if (data.msgIcon == "warning") {

                            } else {
                                $("#mdlOgrSeansSet").modal('hide');
                                $(".modal").modal('hide');
                                $(".modal fade").remove();
                                $(".modal-backdrop").remove();
                            }
                        }
                    })
                }
            });


        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir İşlem Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });

}

function yeniKitapcik() {
    menuHide();
    $.ajax({
        url: '/App/drpKategoriYayin/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            var yayin = jQuery.parseJSON(data.yayinJSON);
            var kategori = jQuery.parseJSON(data.kategoriJSON);
            let denemeEkleModal =
                `
                            <div class="modal fade modalbox" id="mdlDenemeEkle" data-backdrop="static" tabindex="-1" role="dialog">
                                <div class="modal-dialog" role="document">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title">Yeni Deneme(Kitapçık) Ekle</h5>
                                            <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                        </div>
                                        <div class="modal-body">
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                            <label class="label" for="drpKYayin">Yayın</label>
                                                            <select id="drpKYayin" class="form-control form-control-lg">`
            $.each(yayin, (index, value) => {
                denemeEkleModal += `<option value="${value.Id}">${value.YayinBaslik}</option>`;
            });
            denemeEkleModal += `</select>
                                                        </div>
                                                    </div>
                                                    <div class="form-group basic">
                                                        <div class="input-wrapper">
                                                            <label class="label" for="drpKKategori">Kategori</label>
                                                                <select id="drpKKategori" class="form-control form-control-lg">`
            $.each(kategori, (index, value) => {
                denemeEkleModal += `<option value="${value.Id}">${value.KategoriBaslik}</option>`;
            });
            denemeEkleModal += `</select>
                                                        </div>
                                                    </div>
                                                 <div class="form-group basic">
                                            <div class="input-wrapper">
                                                    <label class="label" for="txtKitapcikAd">Deneme(Kitapçık) Adı</label>
                                                    <input type="text" class="form-control" id="txtKitapcikAd"
                                                        placeholder="Kitapçık Adı">
                                                    <i class="clear-input">
                                                        <ion-icon name="close-circle"></ion-icon>
                                                    </i>
                                                </div>
                                            </div>
<div class="form-group basic">
                                            <div class="input-wrapper">
                                                    <label class="label" for="txtGirisAdFiyat">Giriş Fiyatı(Adet)</label>
                                                    <input type="text" class="form-control" id="txtGirisAdFiyat"
                                                        placeholder="Kitapçık Giriş Fiyatı(Adet Maliyeti)">
                                                    <i class="clear-input">
                                                        <ion-icon name="close-circle"></ion-icon>
                                                    </i>
                                                </div>
                                            </div>
                                            <div class="mt-2">
                                                <button id="btnKitapcikPush" onclick="kitapcikPush();" type="button" class="btn btn-primary btn-block btn-lg">Kitapcik Kaydet</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>`;

            $("#modalArea").html(denemeEkleModal);
            $("#mdlDenemeEkle").modal('show');
        }
    });

}
function kitapcikPush() {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "Yeni Bir Kitapçık Eklenecektir",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            var yayin = $("#drpKYayin option:selected").val();
            var kategori = $("#drpKKategori option:selected").val();
            var kitapcikAd = $("#txtKitapcikAd").val();
            var GirisFiyat = $("#txtGirisAdFiyat").val().toString().replace(".", ",");

            var obj = {
                YayinId: yayin,
                KategoriId: kategori,
                DenemeBaslik: kitapcikAd,
                GirisFiyat: GirisFiyat
            }
            $.ajax({
                url: '/App/yeniKitapcik/',
                type: 'Post',
                data: { model: obj },
                dataType: 'json',
                success: function (data) {
                    if (data.msgIcon == "warning") {
                        Swal.fire({
                            title: data.msgTitle,
                            icon: data.msgIcon,
                            html:
                                '<b>' + data.msg + '</b>',

                            confirmButtonText:
                                'Kapat'
                        })
                    } else if (data.msgIcon == "success") {
                        Swal.fire({
                            title: data.msgTitle,
                            icon: data.msgIcon,
                            html:
                                '<b>' + data.msg + '</b>',

                            confirmButtonText:
                                'Kapat'
                        }).isConfirmed(function () {
                            $("#mdlDenemeEkle").modal('hide');
                            $(".modal").modal('hide');
                            $(".modal fade").remove();
                            $(".modal-backdrop").remove();
                        });

                    }
                },
                Error: function (data) {
                    Swal.fire({
                        title: 'Hata Oluştu',
                        icon: 'error',
                        html:
                            '<b>Lütfen Boş Alan Bırakmayınız</b>',

                        confirmButtonText:
                            'Kapat'
                    })
                }
            });
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });

}
function denemeKontenjanSet(id) {
    let value;
    if (id == null) {
        value = $("#drpDenemeKitapcik option:selected").val();
    } else {
        value = id.value;
    }
    $.ajax({
        url: '/App/kontenjanSet/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        data: { id: value },
        dataType: 'json',
        success: function (data) {
            var kontenjanData = jQuery.parseJSON(data);
            kontenjan = kontenjanData;
        }
    });
}
function kontenjanControl(el) {
    if (Number(el.value) > kontenjan) {
        alertColor = "warning ";
        msg = "Seçmiş Olduğunuz Deneme Kitapçığı İçin Enfazla " + kontenjan + " Kişi Kaydedebilirsiniz";
        document.getElementById('btnSeansOlustur').disabled = true;
        let alert =
            `<div id="kontenjanAlert" class="alert alert-outline-${alertColor} mb-1" role="alert">
                            <b id="msg">${msg}</b>
                        </div>`;
        $("#kontenjanMsg").html(alert);
    }
    else {
        document.getElementById('btnSeansOlustur').disabled = false;
        $("#kontenjanAlert").remove();

    }
}
function menuHide(){
    $(".inset").modal('hide');
}
function ogrSorgula(){
    menuHide();
    let ogrSorguScreen = `<div class="modal fade dialogbox" id="ogrSorguScreen" data-backdrop="static">
            <div class="modal-dialog" role="document">
                <div class="modal-content" style="    
    max-width: 100% !important;
max-height: 100% !important;
    height: 95% !important;
    width: 100% !important;">
<button type="button" class="btn btn-danger" data-dismiss="modal" style="margin-left: auto; margin-right: 15px;
    margin-top: 2rem; padding: 15px;">Kapat</button>

                    <div class="modal-icon text-primary" style="margin-top:0px !important;">
                        <ion-icon name="information-circle-outline" style="font-size:64px;"></ion-icon>
                    </div>
                    <div class="modal-header">
                        <h5 class="modal-title">Öğrenci Sorgu Ekranı</h5>
                    </div>
                    <div class="modal-body" style="padding: 5px 5px 5px 5px;">
                        <div class="form-group">
                            <label for="txtValue" class="text-left">Telefon:</label>
                            <input id="txtValue" type="text" class="form-control" placeholder="Ad,Soyad Veya Telefon" />
                            <hr />
                            <button id="btnOgrSorgu" type="button" class="btn btn-warning btn-block">Sorgula</button>
                        </div>
                        <div class="container-fluid">
                            <div class="row">
                                    <div class="col-md-12">
               
                                        <div id="ogrSorguSonuc"></div>

                                    </div>
                            </div>
                        </div>
                    </div>
    
                </div>
            </div>
        </div>`;
    $("#modalArea2").html(ogrSorguScreen);
    $("#ogrSorguScreen").modal('show');
    $("#btnOgrSorgu").click(function () {
        let ogrSorguSonuc = ``;
        var value = $("#txtValue").val();
        $.ajax({
            url: '/App/ogrSorgu/',
            type: 'POST',
            data: { value: value },
            dataType: 'json',
            success: function (data) {
                var ogrSorguResult = jQuery.parseJSON(data);
                if (ogrSorguResult == null){
                    ogrSorguSonuc += `<div class="alert alert-outline-danger alert-dismissible fade show" role="alert">
                    <h4 class="alert-title">Kayıt Bulunamadı</h4>
                    Mevcut Bilgiler İle Kayıtlı Bir Kişi Bulunamamıştır. Lütfen Kontrol Ederek Tekrar Deneyiniz;
                </div>`;
                }else{
                    ogrSorguSonuc += `           
           <div class="alert alert-outline-success fade show" role="alert" style="padding:0rem 0rem;">
                    <h4 class="alert-title">Öğrenci Bulundu</h4>
   <div class="listview-title alert-title" style="display:contents !important;"><b>Üye/Öğrenci Bilgileri</b></div>
                                   <ul class="listview simple-listview">
                                        <li>Adı: ${ogrSorguResult.Ad} </li>
                                        <li>Soyadı: ${ogrSorguResult.Soyad}</li>
                                        <li>Telefon: ${ogrSorguResult.Telefon}</li>
                                        <li>Üyelik: </li>
                            
                                    </ul>                </div>
           
                        `;
                }
                $("#ogrSorguSonuc").html(ogrSorguSonuc);
                
            }
        });
    });
}
function ogrList() {
    menuHide();

    $.ajax({
        url: '/App/ogrList/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            var ogrObjData = jQuery.parseJSON(data);
            let ogrModalSet =
                `
                    <div class="modal fade modalbox" id="mdlOgrList">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Kayıtlı Öğrenci Listesi</h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body" style="padding-left: 0px !important;
    padding-right: 0px !important;">
                                    <div id="ogrTableBody">
                                          <div class="card text-white bg-secondary mb-2">
                                                <div class="card-header">Aktif Kayıtlı Öğrenci Listesi</div>
                                                <div class="card-body" style="padding: 24px 5px !important;">
 <table id="tblOgrList" class="table table-sm text-white" style="border-radius: 10px; width:100% !important; font-weight: bolder;">
                                                                                        <thead>
                                                                                            <tr>
                                                                                                <th class="text-white"><b>Kategori</b></th>
                                                                                                <th class="text-white"><b>Ad - Soyad</b></th>
                                                                                                <th class="text-white"></th>
                                                                                            </tr>
                                                                                        </thead>
                                                                                        <tbody>`
            $.each(ogrObjData, (index, value) => {
                ogrModalSet += `<tr>
                                                                        <td width="auto">${value.Kategori.AnaKategoriBaslik}</td>
                                                                        <td width="auto">${value.Ad} ${value.Soyad}</td>
                                                                        <td width="10px"><button type="button" class="btn btn-info btn-block" onclick="cokluDenemeKayit(${value.Id});">Deneme Kayıt</button>
                                                                        <button type="button" class="btn btn-warning btn-block" onclick="ogrenciDetay(${value.Id});">Güncelle</button>
                                                                        <button type="button" class="btn btn-danger btn-block" onclick="ogrKayitSil(${value.Id});">Sil</button><button type="button" class="btn btn-primary btn-block" onclick="ogrBilgiGetir(${value.Id});">Bilgileri Göster</button></td>

                                                                </tr>`;
            });
            ogrModalSet += `</tbody>
                                                                                    </table>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>`;

            $("#modalArea").html(ogrModalSet);
            $.fn.DataTable.ext.pager.numbers_length = 0;
            $('#tblOgrList').DataTable({
                "language": {
                    "url": "//cdn.datatables.net/plug-ins/1.12.1/i18n/tr.json"
                },
                "pagingType": "full_numbers"
            });
            $("#mdlOgrList").modal('show');

        }
    });
}
function yeniOkul() {
    let okulModal = `<div class="modal fade modalbox" id="mdlYeniOkul" data-backdrop="static" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Yeni Okul Tanımla</h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body">
                                        <div class="section mt-4 mb-5">
                                            <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtKatAd">Okul Adı</label>
                                                        <input type="text" class="form-control" id="txtOkulAd"
                                                            placeholder="Okul Adı">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                            
                                                <div class="mt-2">
                                                    <button id="btnYeniOkulKaydet" type="button" class="btn btn-primary btn-block btn-lg" onclick="okulPush();">Kategori Oluştur</button>
                                                </div>
                                        </div>
                                    </div>
                                </div>
                        </div>
                </div>`;
    $("#modalArea").html(okulModal);
    $("#mdlYeniOkul").modal('show');


}
function seansListSet() {
    menuHide();
    $.ajax({
        url: '/App/kategoriList/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            var kategoriObjData = jQuery.parseJSON(data);

            let modalSonuc =
                `<div class="modal fade modalbox" id="mdlKategoriList" data-backdrop="static" tabindex="-1" role="dialog">
                                    <div class="modal-dialog" role="document">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h5 class="modal-title">Kategori-Seans Listesi</h5>
                                                <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                            </div>
                                            <div class="modal-body p-0">
                                                 <div class="section full mt-2">
                                                    <div class="section-title">Deneme Sınavı Kategorileri</div>

                                                    <div class="accordion" id="kategoriList">`

            $.each(kategoriObjData, (index, value) => {
                modalSonuc += `    <div class="item">
                                                            <div class="accordion-header">
                                                                <button class="btn collapsed" type="button" data-toggle="collapse" data-target="#kt-${value.Id}" onclick="ktseansListSet(${value.Id})">
                                                                    ${value.KategoriBaslik}
                                                                </button>
                                                            </div>
                                                            <div id="kt-${value.Id}" class="accordion-body collapse" data-parent="#kategoriList" >
                                                                <div class="accordion-content" style="padding: 0px 0px 0px 0px;">
                <div class="section mt-2" style="padding: 0px 0px 0px 0px;">
                    <div id="ktseansListDiv-${value.Id}">
                    </div>
                </div>

                                                                </div>
                                                            </div>
                                                        </div>`;
            });

            modalSonuc += `</div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>`;

            $("#modalArea").html(modalSonuc);
            
            $("#mdlKategoriList").modal('show');
        },
        Error: function (data) {

        }
    });

}
function ktseansListSet(ktID) {
    $.ajax({
        url: '/App/snYayinList/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        data: { katID: ktID },
        dataType: 'json',
        success: function (data) {


            var dataResult = jQuery.parseJSON(data);
            let yayinlarDiv = `<div class="section inset mt-2">
            <div class="section-title">Yayınlar</div>

            <div class="accordion" id="accordionYayinSeansList">`;
                   
            $.each(dataResult, (index, value) => {    
                yayinlarDiv += ` <div class="item">
                <div class="accordion-header">
                    <button class="btn collapsed" onclick="yayinSeansGetir(${value.Id},${ktID});" type="button" data-toggle="collapse" data-target="#accordion-${value.Id}">
                        ${value.YayinBaslik}
                    </button>
                </div>
                <div id="accordion-${value.Id}" class="accordion-body collapse" data-parent="#accordionYayinSeansList">
                    <div class="accordion-content">
                <div class="section-title">Seans Listesi</div>

                <div class="accordion" id="accordionYayinSeans${ktID}${value.Id}">

                        </div>
                    </div>
                </div>
            </div>`;     
            });   
                                   

          yayinlarDiv += `</div>
        </div>`;
            $("#ktseansListDiv-"+ktID).html(yayinlarDiv);

        },
        Error: function (data) {

        }
    });

}
function yayinSeansGetir(yayinID,katID) {
    $.ajax({
        url: '/App/seansList/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        data: { yayinID: yayinID, katID: katID},
        dataType: 'json',
        success: function (data) {
            var seansObjData = jQuery.parseJSON(data.jsonSeansList);
            console.log(seansObjData);

            let modalSonuc =
            `
        
                                                <div class="accordion" id="seansListSonuc">`;
        
        $.each(seansObjData, (index, value) => {
        
            modalSonuc += ` <div class="item">
        <div class="accordion-header">
                            <button class="btn collapsed" type="button" data-toggle="collapse" data-target="#seanss-${value.Id}">
                                ${value.kitapcikBaslik} - ${value.TarihSTR} - ${value.SeansGun} / ${value.Saat}
                            </button>
                            </div>
                            <div id="seanss-${value.Id}" class="accordion-body collapse" data-parent="#seansListSonuc">
                                <div class="accordion-content" style="padding:0px 0px 0px 0px !important;">
        <div style="margin-left:10px !important; margin-right: 10px !important;">`
            if (value.Durum != 2 || value.Durum != 0 && value.Tarih.Date > Date.now.Date) {
                modalSonuc += `
                <button id="${value.Id}" type="button" class="btn btn-info btn-block" onclick="snDt(this.id);"><ion-icon name="create-outline"></ion-icon> Seans Bilgilerini Güncelle</button>
                <button id="${value.Id}" type="button" class="btn btn-success btn-block" onclick="seansOgrMultiAdd(this.id);"><ion-icon name="person-add-outline"></ion-icon> Öğrenci Kaydet</button>
                <button id="${value.Id}" type="button" class="btn btn-danger btn-block" onclick="snIptal(this.id);"><ion-icon name="close-circle-outline"></ion-icon> Seans İptal</button>
                <button id="${value.Id}" type="button" class="btn btn-secondary btn-block" onclick="seansOnKayitList(this.id);"><ion-icon name="list-circle-outline"></ion-icon>Deneme Ön Kayıt Talepleri</button>
                <button id="${value.Id}" type="button" class="btn btn-primary btn-block" onclick="bildirimPanel(this.id)"><ion-icon name="notifications-outline"></ion-icon>Toplu Bildirim Gönder</button>
        
                `;
            
            }
            modalSonuc += `</div>
        <div class="card text-white" style="margin-top:1rem;">
        <div class="card-header">
        Seans Bilgileri
        </div>
        <div class="card-body">
        <ul class="listview flush transparent image-listview">
        <li>
            <a href="#" class="item">
                <div class="icon-box bg-primary">
                    <ion-icon name="today-outline"></ion-icon>
                </div>
                <div class="in">
                     Gün:
                    <span class="badge badge-primary">${value.SeansGun}</span>
                </div>
            </a>
        </li>
        <li>
            <a href="#" class="item">
                <div class="icon-box bg-primary">
                    <ion-icon name="calendar-outline"></ion-icon>
                </div>
                <div class="in">
                    <div>Tarih:</div>
                    <span class="badge badge-primary">${value.TarihSTR}</span>
        
                </div>
            </a>
        </li>
        <li>
            <a href="#" class="item">
                <div class="icon-box bg-primary">
                    <ion-icon name="time-outline"></ion-icon>
                </div>
                <div class="in">
                    <div>Saat:</div>
                    <span class="badge badge-primary">${value.Saat}</span>
                </div>
            </a>
        </li>
        <li>
            <a href="#" class="item">
                <div class="icon-box bg-primary">
                    <ion-icon name="time-outline"></ion-icon>
                </div>
                <div class="in">
                    <div>Seans Ücreti:</div>
                    <span class="badge badge-primary">${value.SeansUcret} TL</span>
                </div>
            </a>
        </li>
        <li>
            <a href="#" class="item">
                <div class="icon-box bg-primary">
                    <ion-icon name="people-outline"></ion-icon>
                </div>
                <div class="in">
                    <div>Kontenjan:</div>
                    <span class="badge badge-primary">${value.Kontenjan} Kişi</span>
                </div>
            </a>
        </li>
        <li>
            <a href="#" class="item">
                <div class="icon-box bg-primary">
                    <ion-icon name="people-circle-outline"></ion-icon>
                </div>
                <div class="in">
                    <div>Toplam Kayıtlı Öğrenci:</div>
                    <span class="badge badge-primary">${value.KayitliOgrenci} Kişi</span>
                </div>
            </a>
        </li>
        <li>
            <a href="#" class="item">
                <div class="icon-box bg-primary">
                    <ion-icon name="person-circle-outline"></ion-icon>
                </div>
                <div class="in">
                    <div>Kalan Kontenjan:</div>
                    <span class="badge badge-primary">${value.GuncelKontenjan} Kişi</span>
                </div>
            </a>
        </li>
        </ul>
        </div>
        </div>
        
        <hr>
        <div class="card text-white bg-secondary mb-2">
        <div class="card-header"><ion-icon name="people-circle-outline"></ion-icon>Deneme Sınavına Kayıtlı Öğrenci Listesi <br><br> <button type="button" class="btn btn-primary btn-block" onclick="seansOgrList(${value.Id},1);"><ion-icon name="print-outline"></ion-icon> Yoklama Bekleyen Liste</button><br><br><button type="button" class="btn btn-primary btn-block" onclick="seansOgrList(${value.Id},4);"><ion-icon name="print-outline"></ion-icon> Kitapçık Alan Liste</button><br><br><button type="button" class="btn btn-primary btn-block" onclick="seansOgrList(${value.Id},3);"><ion-icon name="print-outline"></ion-icon> Katılım Sağlayan Liste</button><br><br><button type="button" class="btn btn-primary btn-block" onclick="seansOgrList(${value.Id},3);"><ion-icon name="print-outline"></ion-icon> Katılmayanlar Liste</button></div>
        <div class="card-body" style="padding: 5px 5px;">
        <div class="table-responsive">
                <table class="table table-sm text-white">
                <thead>
                    <tr>
                        <th>Adı Soyadı</th>
                        <th>Telefon</th>
                        <th>Durum</th>
                        <th></th>
                        <th></th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>`;
            let durumText = ``;
            let btnDurum = ``;
           const cnsOgrList = function ogrListSet() {
                    $.each(value.kayitliOgrList, (ogrIndex, ogrValue) => {
                        if (ogrValue.Durum == 1) {
                            durumText = `Yoklama Bekleniyor`;
                            btnDurum = ``;
                        } else if (ogrValue.Durum == 2) {
                            durumText = `Katılım Sağlandı`;
                            btnDurum = `disabled`;
                        } else if (ogrValue.Durum == 3) {
                            durumText = `Katılmadı`;
                            btnDurum = `disabled`;
                        } else if(ogrValue.Durum == 4) {
                            durumText = `Kitapçık Aldı`;
                            btnDurum = `disabled`;
                        }
                        modalSonuc += `
                    <tr id="ogrRow-${ogrValue.Id}">
                        <td>${ogrValue.Ad} ${ogrValue.Soyad}</td>
                        <td>${ogrValue.Telefon}</td>
                        <td><b id="ogrSndurum-${ogrValue.Id}">${durumText}</b></td>
                        <td><button type="button" class="btn btn-danger btn-sm ogrSnSil" onclick="ogrSil(${ogrValue.Id},${value.Id});" style="height:40px">Öğrenci Deneme Kaydını Sil</button></td>
                        <td><button id="btnOgrYoklamaX-${ogrValue.Id}" type="button" class="btn btn-success btn-sm" onclick="ogrYoklama(${ogrValue.Id},${value.Id},2);" ${btnDurum} style="height:40px">Katılım Sağladı</button></td>
                        <td><button id="btnOgrYoklamaY-${ogrValue.Id}" type="button" class="btn btn-primary btn-sm" onclick="ogrYoklama(${ogrValue.Id},${value.Id},4);" ${btnDurum} style="height:40px">Kitapçık Aldı</button></td>
                    </tr>`;
                    });
            }
            cnsOgrList();

            modalSonuc += `</tbody></table></div></div></div></div>
                                                                                </div>
                                                                            </div>`;
        });
        modalSonuc += `</div>
                                            </div>
                                        
                                    
                               
                            `;
        
            $("#accordionYayinSeans"+katID+yayinID).html(modalSonuc);

        }
    });




   
}
function ogrYoklama(ogrID,seansKayitNo,type){
    var mesajTitle = "Öğrenci Yoklama Onayı";
    var mesaj = "";
    if(type == 2){
        mesaj = "Öğrenci Deneme Sınavına Katılım Sağlamıştır, İşlemi Onaylıyor musunuz?";
    }else if(type == 4){
        mesaj = "Öğrenci Deneme Sınavına Katılmamış Olup, Kitapçığını Elden Teslim Almıştır, İşlemi Onaylıyor musunuz?";
    }
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: mesajTitle,
        text: mesaj,
        icon: 'info',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/App/ogrYoklama/',
                type: 'Post',
                data: { ogrID: ogrID, seansKayitNo: seansKayitNo, type: type },
                dataType: 'json',
                success: function (data) {
                    if (data.msgIcon == "warning") {
                        Swal.fire({
                            title: data.msgTitle,
                            icon: data.msgIcon,
                            html:
                                '<b>' + data.msg + '</b>',

                            confirmButtonText:
                                'Kapat'
                        })
                    } else if (data.msgIcon == "success") {
                        var durumText = ``;
                        if(type == 2){
                            durumText = `Katılım Sağladı`;
                        }else if(type == 4){
                            durumText = `Kitapçığını Teslim Aldı`;
                        }
       
                        
                        Swal.fire({
                            title: data.msgTitle,
                            icon: data.msgIcon,
                            html:
                                '<b>' + data.msg + '</b>',

                            confirmButtonText:
                                'Kapat'
                        });
                        document.getElementById("ogrSndurum-"+ogrID).innerHTML = durumText;

                        document.getElementById("btnOgrYoklamaX-"+ogrID).disabled = true;
                        document.getElementById("btnOgrYoklamaY-"+ogrID).disabled = true;
                    }
                },
                Error: function (data) {
                    Swal.fire({ 
                        title: 'Hata Oluştu',
                        icon: 'error',
                        html:
                            '<b>Sistemsel Bir Hata Oluştu</b>',

                        confirmButtonText:
                            'Kapat'
                    })
                }
            });
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });

}
function SeansOnKayitOnay(ogrID,seansID){



    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "Öğrenci Ön Kaydı Onaylanacaktır",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
          
            $.ajax({
                url: '/App/seansOnKayitOnay/',
                type: 'Post',
                data: { seansID: seansID, ogrID: ogrID },
                dataType: 'json',
                success: function (data) {
                    if (data.msgIcon == "warning") {
                        Swal.fire({
                            title: data.msgTitle,
                            icon: data.msgIcon,
                            html:
                                '<b>' + data.msg + '</b>',

                            confirmButtonText:
                                'Kapat'
                        })
                    } else if (data.msgIcon == "success") {
                        Swal.fire({
                            title: data.msgTitle,
                            icon: data.msgIcon,
                            html:
                                '<b>' + data.msg + '</b>',

                            confirmButtonText:
                                'Kapat'
                        });
                        seanslist.cnsOgrList();

                    }
                },
                Error: function (data) {
                    Swal.fire({
                        title: 'Hata Oluştu',
                        icon: 'error',
                        html:
                            '<b>Lütfen Boş Alan Bırakmayınız</b>',

                        confirmButtonText:
                            'Kapat'
                    })
                }
            });
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });
}
function seansOnKayitList(id){
    $.ajax({
        url: '/App/seansOgrOnKayitList/',
        type: 'get',
        data: { seansID: id },
        dataType: 'json',
        success: function (data) {
            var seansOgrOnKayitList = jQuery.parseJSON(data);
            let OgrSeansList = ``;
            $.each(seansOgrOnKayitList, (index, value) => {
                OgrSeansList += `
                            <tr>
                                <td scope="row">${value.Ogr.Ad} ${value.Ogr.Soyad}</td>
                                <td scope="row">${value.Ogr.Telefon}</td>
                                <td scope="row">Kesin Kayıt Bekliyor</td>
                                <td scope="row"><button type="button" class="btn btn-success btn-sm" onclick="SeansOnKayitOnay(${value.Ogr.Id},${id})">Onay</button><button type="button" class="btn btn-danger btn-sm" onclick="SeansOnKayitOnay(${id},0)" style="margin-left:10px;">Red</button></td>
                            </tr>`;
            });
    

            let ogrSeansonKayitModal = `<div class="modal fade dialogbox" id="mdlSeansOgrOnKayitList" data-backdrop="static">
            <div class="modal-dialog" role="document">
                <div class="modal-content" style="    
    max-width: 100% !important;
max-height: 100% !important;
    height: 95% !important;
    width: 100% !important;">
<button type="button" class="btn btn-danger" onclick="customseansOgrOnKayitModalClose();" style="margin-left: auto;
    margin-top: 2rem; padding: 15px;">Kapat</button>
                    <div class="modal-icon text-primary" style="margin-top:0px !important;">
                        <ion-icon name="information-circle-outline" style="font-size:64px;"></ion-icon>
                    </div>
                    <div class="modal-header">
                        <h5 class="modal-title">Deneme Seansı Ön Kayıt Talebi Bulunan Öğrenciler</h5>
                    </div>
                    <div class="modal-body" style="padding: 5px 5px 5px 5px;">
                        <div class="table-responsive">
                    <table id="ogrSeansOnKayitTable" class="table table-sm table-bordered">
                        <thead>
                            <tr>
                                <th scope="col">Ad soyad</th>
                                <th scope="col">Telefon</th>
                                <th scope="col">Durum</th>
                                <th scope="col">Seçenekler</th>
                            </tr>
                        </thead>
                        <tbody id="seansOfOgr">
                        </tbody>
                    </table>
                </div>
                    </div>
 
                </div>
            </div>
        </div>`;
            $("#modalArea2").html(ogrSeansonKayitModal);
            $("#seansOfOgr").html(OgrSeansList);
            $("#ogrSeansOnKayitTable").DataTable();
            $("#ogrSeansOnKayitTable_length").remove();
            $("#ogrSeansOnKayitTable_info").remove(); 
            $("#mdlSeansOgrOnKayitList").modal('show');
        }
    });
}
function pktDt(id) {
    $.ajax({
        url: '/App/seansDetay/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        data: { id: id },
        dataType: 'json',
        success: function (data) {
            var paketData = jQuery.parseJSON(data.paketDetayJSON);
            let paketDetayModal = `<div class="modal fade modalbox" id="mdlPaketDetay" data-backdrop="static" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">${paketData.PaketBaslik} Bilgileri</h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body">
                                        <div class="section mt-4 mb-5">

                                        <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtPaketAd">Paketin Adı</label>
                                                        <input type="text" class="form-control" id="txtPaketAd"
                                                            placeholder="Paket Adı" value="${paketData.PaketBaslik}">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtPaketAd">Geçerlilik Süresi(Gün)</label>
                                                        <input type="text" class="form-control" id="txtGecerlilikGun"
                                                            placeholder="Paket Kaç Gün Geçerli?" value="${paketData.GecerlilikToplamGun}">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtPaketAd">Toplam Giriş Hakkı</label>
                                                        <input type="text" class="form-control" id="txtToplamGirisHakki"
                                                            placeholder="Toplam Giriş Hakkı?" value="${paketData.ToplamGirisHak}">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtPaketUcret">Paket Ücreti</label>
                                                        <input type="text" class="form-control" id="txtPaketUcret"
                                                            placeholder="Paketin Toplam Ücreti?" value="${paketData.Ucret}">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                <div class="mt-2">
                                                    <button id="btnPaketGuncelle" type="button" class="btn btn-primary btn-block btn-lg" onclick="seansPush();">Seans Bilgi Güncelle</button>
                                                </div>
                                        </div>
                                    </div>
                                </div>
                        </div>
                </div>`;
            $("#modalArea").html(paketDetayModal);
         
            $("#mdlPaketDetay").modal('show');
            $("#btnPaketGuncelle").click(function () {
                const swalWithBootstrapButtons = Swal.mixin({
                    customClass: {
                        confirmButton: 'btn btn-success',

                        cancelButton: 'btn btn-danger swButton'
                    },
                    buttonsStyling: false
                })

                swalWithBootstrapButtons.fire({
                    title: 'İşlemi Onaylıyor musunuz?',
                    text: "Deneme Sınavı Seans Bilgileri Güncellenecektir",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Onaylıyorum',
                    cancelButtonText: 'İptal Et',
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    reverseButtons: true
                }).then((result) => {
                    if (result.isConfirmed) {
                        var paketAd = $("#txtPaketAd").val();
                        var gecerlilikGun = $("#txtGecerlilikGun").val();
                        var toplamGirisHak = $("#txtToplamGirisHakki").val().toString().replace(".", ",");
                        var paketUcret = $("#txtPaketUcret").val().toString().replace(".", ",");

                        var obj = {
                            Id: id,
                            PaketBaslik: paketAd,
                            Ucret: paketUcret,
                            GecerlilikToplamGun: gecerlilikGun,
                            ToplamGirisHak: toplamGirisHak
                        }
                        $.ajax({
                            url: '/App/uyelikPaketAction/',
                            type: 'Post',
                            data: { model: obj, typ: 2 },
                            dataType: 'json',
                            success: function (data) {
                                if (data.msgIcon == "warning") {
                                    Swal.fire({
                                        title: data.msgTitle,
                                        icon: data.msgIcon,
                                        html:
                                            '<b>' + data.msg + '</b>',

                                        confirmButtonText:
                                            'Kapat'
                                    })
                                } else if (data.msgIcon == "success") {
                                    Swal.fire({
                                        title: data.msgTitle,
                                        icon: data.msgIcon,
                                        html:
                                            '<b>' + data.msg + '</b>',

                                        confirmButtonText:
                                            'Kapat'
                                    }).isConfirmed(function () {
                                        $("#mdlPaketDetay").modal('hide');
                                        $(".modal").modal('hide');
                                        $(".modal fade").remove();
                                        $(".modal-backdrop").remove();
                                    });

                                }
                            },
                            Error: function (data) {
                                Swal.fire({
                                    title: 'Hata Oluştu',
                                    icon: 'error',
                                    html:
                                        '<b>Lütfen Boş Alan Bırakmayınız</b>',

                                    confirmButtonText:
                                        'Kapat'
                                })
                            }
                        });
                    } else if (
                        result.dismiss === Swal.DismissReason.cancel
                    ) {
                        Swal.fire({
                            title: '<strong>İşlem İptal Edildi</strong>',
                            icon: 'info',
                            html:
                                '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                            confirmButtonText:
                                'Kapat'
                        })
                    }
                });
            });
        }
    });
}

function snIptal(id) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "Bu Seans İptal Edilecek Olup Bu Seansa Kayıtlı Olan Tüm Öğrencilerin Deneme Sınavı kayıtları silinecektir",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            var obj = {
                Id: id,
                actionType: 0
            }
            $.ajax({
                url: '/App/yeniSeans/',
                type: 'Post',
                data: { id: id },
                dataType: 'json',
                success: function (data) {
                    if (data.msgIcon == "warning") {
                        Swal.fire({
                            title: data.msgTitle,
                            icon: data.msgIcon,
                            html:
                                '<b>' + data.msg + '</b>',

                            confirmButtonText:
                                'Kapat'
                        })
                    } else if (data.msgIcon == "success") {
                        Swal.fire({
                            title: data.msgTitle,
                            icon: data.msgIcon,
                            html:
                                '<b>' + data.msg + '</b>',

                            confirmButtonText:
                                'Kapat'
                        }).isConfirmed(function () {
                            $(".modal").modal('hide');
                            $(".modal fade").remove();
                            $(".modal-backdrop").remove();
                        });

                    }
                },
                Error: function (data) {
                    Swal.fire({
                        title: 'Hata Oluştu',
                        icon: 'error',
                        html:
                            '<b>Lütfen Boş Alan Bırakmayınız</b>',

                        confirmButtonText:
                            'Kapat'
                    })
                }
            });
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });
}

function pktIptal(id) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "Kütüphane Üyelik Paketi, İptal Edilecekdir",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                url: '/App/uyelikPaketAction/',
                type: 'Post',
                data: { id: id },
                dataType: 'json',
                success: function (data) {
                    if (data.msgIcon == "warning") {
                        Swal.fire({
                            title: data.msgTitle,
                            icon: data.msgIcon,
                            html:
                                '<b>' + data.msg + '</b>',

                            confirmButtonText:
                                'Kapat'
                        })
                    } else if (data.msgIcon == "success") {
                        Swal.fire({
                            title: data.msgTitle,
                            icon: data.msgIcon,
                            html:
                                '<b>' + data.msg + '</b>',

                            confirmButtonText:
                                'Kapat'
                        }).isConfirmed(function () {
                            $(".modal").modal('hide');
                            $(".modal fade").remove();
                            $(".modal-backdrop").remove();
                        });

                    }
                },
                Error: function (data) {
                    Swal.fire({
                        title: 'Hata Oluştu',
                        icon: 'error',
                        html:
                            '<b>Lütfen Boş Alan Bırakmayınız</b>',

                        confirmButtonText:
                            'Kapat'
                    })
                }
            });
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });
}

function uyelikPaketler() {

    $.ajax({
        url: '/App/uyelikPaketList/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            var paketObjData = jQuery.parseJSON(data);
            let modalSonuc =
                `<div class="modal fade modalbox" id="mdlPaketList" role="dialog">
                                    <div class="modal-dialog" role="document">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h5 class="modal-title">Kütüphane Üyelik Paketleri</h5>
                                                <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                            </div>
                                            <div class="modal-body p-0">
                                                 <div class="section full mt-2">
                                                     
           
                
<div class="section-title">Güncel Paketler</div>

                                                    <div class="accordion" id="uyelikPaketList">` 

            $.each(paketObjData, (index, value) => {

                modalSonuc += ` <div class="item">
<div class="accordion-header">
                                <button class="btn collapsed" type="button" data-toggle="collapse" data-target="#uyelikPaketList-${value.Id}">
                                    ${value.PaketBaslik}
                                </button>
                                </div>
                                <div id="uyelikPaketList-${value.Id}" class="accordion-body collapse" data-parent="#uyelikPaketList">
                                    <div class="accordion-content" style="padding:0px 0px 0px 0px !important;">
<div style="margin-left:10px !important; margin-right: 10px !important;">`;
                    modalSonuc += `
                    <button id="${value.Id}" type="button" class="btn btn-info btn-block" onclick="uyelikPaketGuncelle(this.id);"><ion-icon name="create-outline"></ion-icon> Paket Bilgilerini Güncelle</button>
                    <button type="button" class="btn btn-success btn-block" onclick="uyelikPaketOgrList(${value.Id});"><ion-icon name="person-add-outline"></ion-icon> Üye Kaydet</button>
                    <button id="${value.Id}" type="button" class="btn btn-danger btn-block" onclick="pktIptal(this.id);"><ion-icon name="close-circle-outline"></ion-icon> Paket İptal</button>`;
                modalSonuc += `</div>
<div class="card text-white" style="margin-top:1rem;">
        <div class="card-header">
            Kütüphane Üyelik Paket Bilgileri
        </div>
        <div class="card-body">
            <ul class="listview flush transparent image-listview">
            <li>
                <a href="#" class="item">
                    <div class="icon-box bg-primary">
                        <ion-icon name="cash-outline"></ion-icon>
                    </div>
                    <div class="in">
                         Ücret:
                        <span class="badge badge-primary">${value.Ucret} TL</span>
                    </div>
                </a>
            </li>
            <li>
                <a href="#" class="item">
                    <div class="icon-box bg-primary">
<ion-icon name="enter-outline"></ion-icon>
</div>
                    <div class="in">
                        <div>Toplam Giriş Hakkı</div>
                        <span class="badge badge-primary">${value.ToplamGirisHak} Kez</span>

                    </div>
                </a>
            </li>
            <li class="">
                <a href="#" class="item">
                    <div class="icon-box bg-primary">
<ion-icon name="today-outline"></ion-icon>
</div>
                    <div class="in">    
                        <div>Toplam Geçerlilik Süresi:</div>
                        <span class="badge badge-primary">${value.GecerlilikToplamGun} Gün</span>
                    </div>
                </a>
            </li>
            <hr />
<b class="text-center">Pakete Dahil Olan İkramlar</b>
<br>
<button type="button" class="btn btn-primary" style="float:right !important;     margin-top: -1.8rem !important;" onclick="uyelikPaketIkramSet(${value.Id});"><ion-icon name="add-outline"></ion-icon> İkram Ekle</button>
<br>
                <li>`;
                $.each(value.paketIkramlar, (ikramIndex, ikramValue) => {
                    modalSonuc += `<a href="#" class="item">
                        <div class="icon-box bg-primary">
<ion-icon name="fast-food-outline"></ion-icon>
</div>
                        <div class="in">
                        <div>${ikramValue.IkramBaslik}</div>
                        <span class="badge badge-primary">${ikramValue.Adet} Adet</span>
                        </div>
                    </a>`;
                });
                
               modalSonuc += `</li><hr/>
            <li>
                <a href="#" class="item">
                    <div class="icon-box bg-primary">
                        <ion-icon name="people-outline"></ion-icon>
                    </div>
                    <div class="in">
                        <div>Toplam Üye:</div>
                        <span class="badge badge-primary">Kişi</span>
                    </div>
                </a>
            </li>
            <li>
                <a href="#" class="item">
                    <div class="icon-box bg-primary">
<ion-icon name="wallet-outline"></ion-icon>
</div>
                    <div class="in">
                        <div>Toplam Ciro:</div>
                        <span class="badge badge-primary">TL</span>
                    </div>
                </a>
            </li>
      
        </ul>
        </div>
    </div>

<hr>
<div class="card text-white bg-secondary mb-2">
   <div class="card-header"><ion-icon name="people-circle-outline"></ion-icon>Üyelik Paketine Kayıtlı Öğrenci Listesi<br><br> <button type="button" class="btn btn-primary btn-block" onclick="paketOgrList(${value.Id});"><ion-icon name="print-outline"></ion-icon> Kayıtlı Öğrenci Listesi Yazdır</button></div>
            <div class="card-body" style=" padding: 5px 5px;">
                    <table class="table table-sm text-white">
                    <thead>
                        <tr>
                            <th>Adı Soyadı</th>
                            <th>Telefon</th>
                            <th>Durum</th>
                            <th>Seçenekler</th>
                        </tr>
                    </thead>
                    <tbody>`;
                let durumText = ``;
                $.each(value.kayitliOgrList, (ogrIndex, ogrValue) => {
                    if (ogrValue.Durum == 1) {
                        durumText = `<ion-icon name="pause-outline"></ion-icon> Aktif Üyelik`;
                    } else if (ogrValue.Durum == 2) {
                        durumText = `<ion-icon name="checkmark-done-outline"></ion-icon> Süresi Doldu`;
                    } 
                    modalSonuc += `
                <tr>
                    <td>${ogrValue.Ad} ${ogrValue.Soyad}</td>
                    <td>${ogrValue.Telefon}</td>
                    <td>${durumText}</td>
                    <td><button type="button" class="btn btn-danger btn-sm" onclick="ogrSil(${ogrValue.Id},${value.Id},110);" style="height: 75px; margin-top: 8px;">Öğrenci Deneme Kaydını Sil</button>
                </tr>`;

                });
                modalSonuc += `</tbody></table></div></div>
                

</div>
                                                                                    </div>
                                                                                </div>`;
            });
            modalSonuc += `</div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>`;
            $("#modalArea").html(modalSonuc);
            $("#mdlPaketList").modal('show');
       
        },
        Error: function (data) {

        }
    });
}
function yeniUyelikPaket() {
    let yeniUyelikPaketModal = `<div class="modal fade modalbox" id="mdlYeniUyelikPaket" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Yeni Üyelik Paketi Oluştur</h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body">
                                        <div class="section mt-4 mb-5">
                                            <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtPaketAd">Paketin Adı</label>
                                                        <input type="text" class="form-control" id="txtPaketAd"
                                                            placeholder="Paket Adı">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtPaketAd">Geçerlilik Süresi(Gün)</label>
                                                        <input type="text" class="form-control" id="txtGecerlilikGun"
                                                            placeholder="Paket Kaç Gün Geçerli?">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtPaketAd">Toplam Giriş Hakkı</label>
                                                        <input type="text" class="form-control" id="txtToplamGirisHakki"
                                                            placeholder="Toplam Giriş Hakkı?">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtPaketUcret">Paket Ücreti</label>
                                                        <input type="text" class="form-control" id="txtPaketUcret"
                                                            placeholder="Paketin Toplam Ücreti?">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                <div class="mt-2">
                                                    <button id="btnPaketOlustur" type="button" class="btn btn-primary btn-block btn-lg" onclick="paketPush();">Yeni Üyelik Paketi Oluştur</button>
                                                </div>
                                        </div>
                                    </div>
                                </div>
                        </div>
                </div>`;
    $("#modalArea").html(yeniUyelikPaketModal);
    $("#mdlYeniUyelikPaket").modal('show');


}

function uyelikPaketGuncelle(id) {
    $.ajax({
        url: '/App/uyelikPaketDetay/',
        type: 'Post',
        data: { id: id },
        dataType: 'json',
        success: function (data) {
            var paketDataObj = jQuery.parseJSON(data);
            Swal.fire({
                title: 'Paket Bilgi Güncelleme',
                html: ` <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" style="color:black !important;" for="txtPaketAd">Paket Adı</label>
                                                        <input type="text" style="color:black !important;" class="form-control" id="txtPaketAd"
                                                            placeholder="İkram Adı?" value="${paketDataObj.PaketBaslik}">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                            <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" style="color:black !important;" for="txtPaketGecerlilikGun">Toplam Geçerlilik Gün</label>
                                                        <input type="text" style="color:black !important;" class="form-control" id="txtPaketGecerlilikGun"
                                                            placeholder="Toplam Geçerlilik Gün" value="${paketDataObj.GecerlilikToplamGun}">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
  <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" style="color:black !important;" for="txtToplamGirisHak">Toplam Giriş Hakkı</label>
                                                        <input type="text" style="color:black !important;" class="form-control" id="txtToplamGirisHak"
                                                            placeholder="Toplam Giriş Hakkı" value="${paketDataObj.ToplamGirisHak}">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
  <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" style="color:black !important;" for="txtPaketUcret">Paket Ücret</label>
                                                        <input type="number" style="color:black !important;" class="form-control" id="txtPaketUcret"
                                                            placeholder="Paket Ücret" value="${paketDataObj.Ucret}">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>`,
                showDenyButton: true,
                confirmButtonText: 'Kaydet',
                denyButtonText: `Vazgeç`,
            }).then((result) => {
                if (result.isConfirmed) {
                    var ikramBaslik = $("#txtIkramBaslik").val();
                    var adet = $("#txtIkramAdet").val();
                    var obj = {
                        IkramBaslik: ikramBaslik,
                        Adet: adet,
                        PaketId: id
                    }
                    $.ajax({
                        url: '/App/paketIkramSet/',
                        type: 'Post',
                        data: { model: obj },
                        dataType: 'json',
                        success: function (data) {
                            if (data.msgIcon == "warning") {
                                Swal.fire({
                                    title: data.msgTitle,
                                    icon: data.msgIcon,
                                    html:
                                        '<b>' + data.msg + '</b>',

                                    confirmButtonText:
                                        'Kapat'
                                })
                            } else if (data.msgIcon == "success") {
                                Swal.fire({
                                    title: data.msgTitle,
                                    icon: data.msgIcon,
                                    html:
                                        '<b>' + data.msg + '</b>',

                                    confirmButtonText:
                                        'Kapat'
                                })

                            }
                        },
                        Error: function (data) {
                            Swal.fire({
                                title: 'Hata Oluştu',
                                icon: 'error',
                                html:
                                    '<b>Lütfen Boş Alan Bırakmayınız</b>',

                                confirmButtonText:
                                    'Kapat'
                            })
                        }
                    });

                } else if (result.isDenied) {
                    Swal.fire({
                        title: 'İkram Tanımlama İşlemi İptal Edildi', text: ``, icon: 'info', confirmButtonText: 'Kapat',
                    })
                }
            })
        }
    });







}
let uyelikTopluKayit = [];
function uyelikTopluKayitFnc(id) {
    var diziElelman = id.toString().replace("sn-", "");
    var diziSearch = uyelikTopluKayit.indexOf(diziElelman);

    if (diziSearch == -1) {
        uyelikTopluKayit.push(diziElelman);
    } else {
        uyelikTopluKayit.pop(diziSearch, 1);
    }


}
let seansTopluOgrKayit = [];
function seansTopluOgrKayitFnc(id) {
    var diziElelman = id.toString().replace("sn-", "");
    var diziSearch = seansTopluOgrKayit.indexOf(diziElelman);

    if (diziSearch == -1) {
        seansTopluOgrKayit.push(diziElelman);
    } else {
        seansTopluOgrKayit.pop(diziSearch, 1);
    }


}
function uyelikPaketOgrList(pktID) {
    uyelikTopluKayit.length = 0;
    uyelikTopluKayit.push(pktID.toString());
    $.ajax({
        url: '/App/ogrList/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        data: { uyeTalep: 1 },
        dataType: 'json',
        success: function (data) {
   
            var ogrList = jQuery.parseJSON(data);
            let ogrdRp = `
            <input id="txtUyeAra" type="text" class="form-control" placeholder="Kişi Filtrele" onkeyup="ara3()">

        <div class="section full mb-2">
            <div class="wide-block p-0">

                <div class="input-list">`;

            $.each(ogrList, (index, value) => {
                ogrdRp += `<div id="myDiv" class="custom-control custom-checkbox uyelikTopluKayitFnc uyeRow">
                    <input id="sn-${value.Id}" onclick="uyelikTopluKayitFnc(this.id)" type="checkbox" class="custom-control-input">
                        <label style="font-size: 15.4px; display: inherit;" class="custom-control-label" for="sn-${value.Id}">${value.Ad} ${value.Soyad} / ${value.Telefon}</label>
                    </div>`;
            });


            ogrdRp += `</div>

            </div>
        </div>

            `;

            Swal.fire({
                title: '<strong>Üyelik Paketine Kayıt Edebileceğiniz Öğrenciler</strong>',
                html: ogrdRp,
                icon: 'info',
                showDenyButton: true,
                confirmButtonText: 'Kaydet',
                denyButtonText: `Vazgeç`,
            }).then((result) => {
                if (result.isConfirmed) {
                    var result = uyelikTopluKayit.map(function (x) {
                        return parseInt(x, uyelikTopluKayit.length);
                    });
                    var data = JSON.stringify(uyelikTopluKayit);

                    $.ajax({
                        url: '/App/ogrUyelikTopluKayit/',
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        type: 'POST',
                        data: data,
                        success: function (result) {
                        }

                    });

                } else if (result.isDenied) {
                    Swal.fire('İşlem iptal Edildi', 'Her Hangi Bir İşlem Yapılmadı', 'info')
                }
            })

        }

    });


}
$.fn.modal.Constructor.prototype._enforceFocus = function () { };

function seansOgrMultiAdd(snID) {
    seansTopluOgrKayit.length = 0;
    seansTopluOgrKayit.push(snID.toString());
    $.ajax({
        url: '/App/ogrList/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        dataType: 'json',
        data: { seansOgrTalep: 1},
        success: function (data) {

            var ogrList = jQuery.parseJSON(data);
            let ogrdRp = `
<input id="txtOgrAra" type="text" class="form-control" placeholder="Öğrenci Filtrele" onkeyup="ara()">
        <div class="section full mb-2">
            <div class="wide-block p-0">
                <div id="ogr" class="input-list">
`;

            $.each(ogrList, (index, value) => {
                ogrdRp += `<div class="custom-control custom-checkbox ogrRow">
                    <input id="sn-${value.Id}" onclick="seansTopluOgrKayitFnc(this.id)" type="checkbox" class="custom-control-input">
                        <label style="font-size: 15.4px; display: inherit;" class="custom-control-label bolum" for="sn-${value.Id}">${value.Ad} ${value.Soyad} / ${value.Telefon}</label>
                    </div>`;
            });


            ogrdRp += `</div>

            </div>
        </div>

            `;

            Swal.fire({
                title: '<strong>Deneme Sınavı Seansına Edebileceğiniz Öğrenciler</strong>',
                html: ogrdRp,
                icon: 'info',
                showDenyButton: true,
                confirmButtonText: 'Kaydet',
                denyButtonText: `Vazgeç`,
            }).then((result) => {
                if (result.isConfirmed) {
                    var result = seansTopluOgrKayit.map(function (x) {
                        return parseInt(x, seansTopluOgrKayit.length);
                    });
                    var data = JSON.stringify(seansTopluOgrKayit);

                    $.ajax({
                        url: '/App/seansOgrMultiKayit/',
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        type: 'POST',
                        data: data,
                        success: function (result) {
                        }

                    });

                } else if (result.isDenied) {
                    Swal.fire('İşlem iptal Edildi', 'Her Hangi Bir İşlem Yapılmadı', 'info')
                }
            })

        }

    });


}
    function ara() {
        var toCheck = $('#txtOgrAra').val().trim();
        $(".ogrRow").hide().filter(':contains(' + toCheck + ')').show();
    }
function ara2() {
    var toCheck = $('#txtseansAra').val().trim();
    $(".seansRow").hide().filter(':contains(' + toCheck + ')').show();
}
function ara3() {
    var toCheck = $('#txtUyeAra').val().trim();
    $(".uyeRow").hide().filter(':contains(' + toCheck + ')').show();
}
function uyelikPaketIkramSet(id) {

    Swal.fire({
        title: 'İkram Tanımlama Ekranı',
        html: ` <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtIkramBaslik">İkram Başlık</label>
                                                        <input type="text" class="form-control" id="txtIkramBaslik"
                                                            placeholder="İkram Adı?">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div><div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtIkramAdet">Adet</label>
                                                        <input type="number" class="form-control" id="txtIkramAdet"
                                                            placeholder="İkram Adet?">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>`,
        showDenyButton: true,
        confirmButtonText: 'Kaydet',
        denyButtonText: `Vazgeç`,
    }).then((result) => {
        if (result.isConfirmed) {
            var ikramBaslik = $("#txtIkramBaslik").val();
            var adet = $("#txtIkramAdet").val();
            var obj = {
                IkramBaslik: ikramBaslik,
                Adet: adet,
                PaketId: id
            }
            $.ajax({
                url: '/App/paketIkramSet/',
                type: 'Post',
                data: { model: obj },
                dataType: 'json',
                success: function (data) {
                    if (data.msgIcon == "warning") {
                        Swal.fire({
                            title: data.msgTitle,
                            icon: data.msgIcon,
                            html:
                                '<b>' + data.msg + '</b>',

                            confirmButtonText:
                                'Kapat'
                        })
                    } else if (data.msgIcon == "success") {
                        Swal.fire({
                            title: data.msgTitle,
                            icon: data.msgIcon,
                            html:
                                '<b>' + data.msg + '</b>',

                            confirmButtonText:
                                'Kapat'
                        })

                    }
                },
                Error: function (data) {
                    Swal.fire({
                        title: 'Hata Oluştu',
                        icon: 'error',
                        html:
                            '<b>Lütfen Boş Alan Bırakmayınız</b>',

                        confirmButtonText:
                            'Kapat'
                    })
                }
            });

        } else if (result.isDenied) {
            Swal.fire({
                title: 'İkram Tanımlama İşlemi İptal Edildi', text: ``, icon: 'info', confirmButtonText: 'Kapat',
            })
        }
    })
}
function paketPush() {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "Kütüphane Üyelik Paketi Oluşturulacaktır",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
               
            var paketAd = $("#txtPaketAd").val();
            var gecerlilikGun = $("#txtGecerlilikGun").val();
            var toplamGirisHak = $("#txtToplamGirisHakki").val().toString().replace(".", ",");
            var paketUcret = $("#txtPaketUcret").val().toString().replace(".", ",");

            var obj = {
                PaketBaslik: paketAd,
                Ucret: paketUcret,
                GecerlilikToplamGun: gecerlilikGun,
                ToplamGirisHak: toplamGirisHak,
            }
            $.ajax({
                url: '/App/uyelikPaketAction/',
                type: 'Post',
                data: { model: obj, typ: 1 },
                dataType: 'json',
                success: function (data) {
                    if (data.msgIcon == "warning") {
                        Swal.fire({
                            title: data.msgTitle,
                            icon: data.msgIcon,
                            html:
                                '<b>' + data.msg + '</b>',

                            confirmButtonText:
                                'Kapat'
                        })
                    } else if (data.msgIcon == "success") {
                        Swal.fire({
                            title: data.msgTitle,
                            icon: data.msgIcon,
                            html:
                                '<b>' + data.msg + '</b>',

                            confirmButtonText:
                                'Kapat'
                        }).isConfirmed(function () {
                            $("#mdlYeniUyelikPaket").modal('hide');
                            $(".modal").modal('hide');
                            $(".modal fade").remove();
                            $(".modal-backdrop").remove();
                        });

                    }
                },
                Error: function (data) {
                    Swal.fire({
                        title: 'Hata Oluştu',
                        icon: 'error',
                        html:
                            '<b>Lütfen Boş Alan Bırakmayınız</b>',

                        confirmButtonText:
                            'Kapat'
                    })
                }
            });
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });
}
function anaKategoriList() {
    $.ajax({
        url: '/App/anaKategoriList/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            var kategoriObjData = jQuery.parseJSON(data);
            let modalSonuc =
                `<div class="modal fade modalbox" id="mdlAnaKategoriList" data-backdrop="static" tabindex="-1" role="dialog">
                                    <div class="modal-dialog" role="document">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h5 class="modal-title">Kategori Listesi</h5>
                                                <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                            </div>
                                            <div class="modal-body p-0">
                                                 <div class="section full mt-2">
                                                    <div class="section-title">Deneme Sınavı Kategorileri</div>

                                                    <div class="accordion" id="seansList">`

            $.each(kategoriObjData, (index, value) => {
                modalSonuc += `    <div class="item">
                                                            <div class="accordion-header">
                                                                <button class="btn collapsed" type="button" data-toggle="collapse" data-target="#seans-${value.Id}">
                                                                    ${value.AnaKategoriBaslik}
                                                                </button>
                                          
                                                        </div>`;
            });

            modalSonuc += `</div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>`;

            $("#modalArea").html(modalSonuc);
            $("#mdlAnaKategoriList").modal('show');
        },
        Error: function (data) {

        }
    });
}
function kategoriList() {

    $.ajax({
        url: '/App/kategoriList/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            var kategoriObjData = jQuery.parseJSON(data);
            let modalSonuc =
                `<div class="modal fade modalbox" id="mdlKategoriList" data-backdrop="static" tabindex="-1" role="dialog">
                                    <div class="modal-dialog" role="document">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h5 class="modal-title">Kategori Listesi</h5>
                                                <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                            </div>
                                            <div class="modal-body p-0">
                                                 <div class="section full mt-2">
                                                    <div class="section-title">Deneme Sınavı Kategorileri</div>

                                                    <div class="accordion" id="seansKategoriList">`

            $.each(kategoriObjData, (index, value) => {
                modalSonuc += `    <div class="item">
                                                            <div class="accordion-header">
                                                                <button class="btn collapsed" type="button" data-toggle="collapse" data-target="#seansKategori-${value.Id}">
                                                                    ${value.KategoriBaslik}
                                                                </button>
                                                            </div>
                                                            <div id="seansKategori-${value.Id}" class="accordion-body collapse" data-parent="#seansKategoriList">
                                                                <div class="accordion-content">
<div class="section mt-2">
                    <div class="card">
                        <ul class="listview flush transparent image-listview">
                            <li>
                                <a href="#" class="item">
                                    <div class="icon-box bg-primary">
                                        <ion-icon name="documents-outline" role="img" class="md hydrated" aria-label="home"></ion-icon>
                                    </div>
                                    <div class="in">
                                         Toplam Kitapçık
                                        <span class="badge badge-primary">${value.toplamKitapcik}</span>
                                    </div>
                                </a>
                            </li>
                            <li>
                                <a href="#" class="item">
                                    <div class="icon-box bg-primary">
                                        <ion-icon name="alarm-outline" role="img" class="md hydrated" aria-label="alarm outline"></ion-icon>
                                    </div>
                                    <div class="in">
                                        Toplam Seans
                                        <span class="badge badge-primary">${value.toplamSeans}</span>
                                    </div>
                                </a>
                            </li>
                            <li>
                                <a href="#" class="item">
                                    <div class="icon-box bg-primary">
                                        <ion-icon name="information-outline" role="img" class="md hydrated" aria-label="add outline"></ion-icon>
                                    </div>
                                    <div class="in">
                                        Kategori Bilgisi: ${value.Aciklama}
                                    </div>
                                </a>
                            </li>
                        </ul>
                    </div>
                </div>

                                                                </div>
                                                            </div>
                                                        </div>`;
            });

            modalSonuc += `</div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>`;

            $("#modalArea").html(modalSonuc);
            $("#mdlKategoriList").modal('show');
        },
        Error: function (data) {

        }
    });
}
function yayinList() {

    $.ajax({
        url: '/App/yayinList/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            var yayinObjData = jQuery.parseJSON(data);
            let modalSonuc =
                `<div class="modal fade modalbox" id="mdlYayinList" data-backdrop="static" tabindex="-1" role="dialog">
                                    <div class="modal-dialog" role="document">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h5 class="modal-title">Yayın Listesi</h5>
                                                <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                            </div>
                                            <div class="modal-body p-0"><div class="section mt-2">            
                                            <div class="row">`
            $.each(yayinObjData, (index, value) => {
                modalSonuc +=
                    `<div class="col-6" style="margin-top:12px;">
                    <div class="card product-card">
                        <div class="card-body">
<img src="${value.Logo}" class="image" alt="product image">
                                <h2 class="title">Yayın Adı: ${value.YayinBaslik}</h2>
                             <div class="price">Toplam Deneme(Kitapçık): ${value.toplamKitapcik}</div>
                            <div class="price">Toplam Seans: ${value.toplamDenemSeans}</div>
            <button type="button" class="btn btn-warning btn-block"><ion-icon name="create-outline"></ion-icon> Düzenle</button>
            <button type="button" class="btn btn-danger btn-block"><ion-icon name="trash-bin-outline"></ion-icon> Sil</button>

                        </div>
                    </div>
                </div>`;
            });
            modalSonuc += `</div></div></div></div></div></div>`;



            $("#modalArea").html(modalSonuc);
            $("#mdlYayinList").modal('show');
        },
        Error: function (data) {

        }
    });
}

function yeniSeans() {
    menuHide();
    $.ajax({
        url: '/App/kitapcikList/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        dataType: 'json',
        success: function (data) {

            let seansModal = `<div class="modal fade modalbox" id="mdlYeniSeans" data-backdrop="static" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Yeni Seans Oluştur</h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body">
                                        <div class="section mt-4 mb-5">
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="drpDenemeKitapcik">Deneme Seçiniz</label>
                                                        <select id="drpDenemeKitapcik" class="form-control" onchange="denemeKontenjanSet(this)">
                                                        <option value="">Deneme Kitapçığı Seçiniz</option>
                                                        `

            var kitapcikData = jQuery.parseJSON(data);
            $.each(kitapcikData, (index, value) => {
                seansModal += `<option value="${value.Id}">Yayın:${value.YayinBaslik} | Kategori: ${value.Kategori.AltKategoriBaslik} | Deneme: ${value.DenemeBaslik}</option>`
            });
            seansModal += ` </select>
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtKontenjan">Kontenjan</label>
                                                        <input type="number" class="form-control" id="txtKontenjan"
                                                            placeholder="Deneme Sınavına Katılacak Toplam Kişi" onkeyup="kontenjanControl(this)" onkeypress="return isNumberKey(event)">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                    <div id="kontenjanMsg"></div>

                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtUcret">Seans Ücreti</label>
                                                        <input type="number" class="form-control" id="txtUcret"
                                                            placeholder="Kişi Başı Deneme Seansı Ücreti"  onkeypress="return isNumberKey(event)">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtTarih">Tarih</label>
                                                        <input type="date" class="form-control" id="txtTarih"
                                                            placeholder="Seans Tarihi">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtSaat">Saat</label>
                                                        <input type="time" class="form-control" id="txtSaat"
                                                            placeholder="Seans Saati">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                <div class="mt-2">
                                                    <button id="btnSeansOlustur" type="button" class="btn btn-primary btn-block btn-lg" onclick="seansPush();">Seans Oluştur</button>
                                                </div>
                                        </div>
                                    </div>
                                </div>
                        </div>
                </div>`;
            $("#modalArea").html(seansModal);
            $("#mdlYeniSeans").modal('show');
        }
    });
}


function seansPush() {

    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "Yeni Deneme Sınavı Seansı Oluşturulacaktır",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            var denemeID = $("#drpDenemeKitapcik option:selected").val();
            var kontenjan = $("#txtKontenjan").val();
            var tarih = $("#txtTarih").val();
            var saat = $("#txtSaat").val();
            var ucret = $("#txtUcret").val().toString().replace(".", ",");
            var obj = {
                DenemeId: denemeID,
                Tarih: tarih,
                Saat: saat,
                SeansUcret: ucret,
                Kontenjan: kontenjan,
                actionType: 1
            }
            $.ajax({
                url: '/App/yeniSeans/',
                type: 'Post',
                data: { model: obj },
                dataType: 'json',
                success: function (data) {
                    if (data.msgIcon == "warning") {
                        Swal.fire({
                            title: data.msgTitle,
                            icon: data.msgIcon,
                            html:
                                '<b>' + data.msg + '</b>',

                            confirmButtonText:
                                'Kapat'
                        })
                    } else if (data.msgIcon == "success") {
                        Swal.fire({
                            title: data.msgTitle,
                            icon: data.msgIcon,
                            html:
                                '<b>' + data.msg + '</b>',

                            confirmButtonText:
                                'Kapat'
                        }).isConfirmed(function () {
                            $("#mdlYeniSeans").modal('hide');
                            $(".modal").modal('hide');
                            $(".modal fade").remove();
                            $(".modal-backdrop").remove();
                        });

                    }
                },
                Error: function (data) {
                    Swal.fire({
                        title: 'Hata Oluştu',
                        icon: 'error',
                        html:
                            '<b>Lütfen Boş Alan Bırakmayınız</b>',

                        confirmButtonText:
                            'Kapat'
                    })
                }
            });
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });

} 
function yeniAnaKategori() {
    let kategoriModal = `<div class="modal fade modalbox" id="mdlYeniAnaKategori" data-backdrop="static" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Yeni Kategori Oluştur</h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body">
                                        <div class="section mt-4 mb-5">
                                            <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtKatAd">Kategori Adı</label>
                                                        <input type="text" class="form-control" id="txtKatAd"
                                                            placeholder="Kategori Adı">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                            <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtKatAciklama">Kategori Açıklama</label>
                                                        <input type="text" class="form-control" id="txtKatAciklama"
                                                            placeholder="Kategori Bilgisi">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                <div class="mt-2">
                                                    <button id="btnKategoriOlustur" type="button" class="btn btn-primary btn-block btn-lg" onclick="AnakategoriPush();">Kategori Oluştur</button>
                                                </div>
                                        </div>
                                    </div>
                                </div>
                        </div>
                </div>`;
    $("#modalArea").html(kategoriModal);
    $("#mdlYeniAnaKategori").modal('show');


}
function AnakategoriPush() {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "İlgili Kategori Sisteme Eklenecektir",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            var katAd = $("#txtKatAd").val();
            var obj = {
                AnaKategoriBaslik: katAd,
            }
            $.ajax({
                url: '/App/yeniAnaKategori/',
                type: 'Post',
                data: { model: obj },
                dataType: 'json',
                success: function (data) {
                    Swal.fire({
                        title: data.msgTitle,
                        text: data.msg,
                        icon: data.msgIcon,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'Tamam'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            if (data.msgIcon == "warning") {

                            } else {
                                $("#mdlYeniAnaKategori").modal('hide');
                                $(".modal").modal('hide');
                                $(".modal fade").remove();
                                $(".modal-backdrop").remove();
                            }
                        }
                    })
                }
            });
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });

}

function yeniKategori() {

    $.ajax({
        url: '/App/anaKategoriList/',
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            var anaKategoriData = jQuery.parseJSON(data);
            let kategoriModal = `<div class="modal fade modalbox" id="mdlYeniKategori" data-backdrop="static" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Yeni Kategori Oluştur</h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body">
                                    <div class="section mt-4 mb-5">
                                            <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtKatAd">Ana Kategori</label>
                                                        <select id="drpAnaKategori" class="form-control">`;
                                                        $.each(anaKategoriData, (index, value) => {
                                                            kategoriModal += `<option value="${value.Id}">${value.AnaKategoriBaslik}</option>`;
                                                        });
                                                       kategoriModal += `</select>
                                                    </div>
                                                </div>
                                        </div>
                                        <div class="section mt-4 mb-5">
                                            <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtKatAd">Kategori Adı</label>
                                                        <input type="text" class="form-control" id="txtKatAd"
                                                            placeholder="Kategori Adı">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                          
                                                <div class="mt-2">
                                                    <button id="btnKategoriOlustur" type="button" class="btn btn-primary btn-block btn-lg" onclick="kategoriPush();">Kategori Oluştur</button>
                                                </div>
                                        </div>
                                    </div>
                                </div>
                        </div>
                </div>`;
            $("#modalArea").html(kategoriModal);
            $("#mdlYeniKategori").modal('show');
        }
    });




}
function kategoriPush() {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "İlgili Kategori Sisteme Eklenecektir",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            var anaKatID = $("#drpAnaKategori option:selected").val();
            var katAd = $("#txtKatAd").val();
            var obj = {
                KategoriBaslik: katAd,
                KatId: anaKatID
            }
            $.ajax({
                url: '/App/yeniAltKategori/',
                type: 'Post',
                data: { model: obj },
                dataType: 'json',
                success: function (data) {
                    Swal.fire({
                        title: data.msgTitle,
                        text: data.msg,
                        icon: data.msgIcon,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'Tamam'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            if (data.msgIcon == "warning") {

                            } else {
                                $("#mdlYeniAnaKategori").modal('hide');
                                $(".modal").modal('hide');
                                $(".modal fade").remove();
                                $(".modal-backdrop").remove();
                            }
                        }
                    })
                }
            });
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });

}

function yeniAnaKategori() {
    let kategoriModal = `<div class="modal fade modalbox" id="mdlYeniAnaKategori" data-backdrop="static" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Yeni Kategori Oluştur</h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body">
                                        <div class="section mt-4 mb-5">
                                            <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtKatAd">Kategori Adı</label>
                                                        <input type="text" class="form-control" id="txtKatAd"
                                                            placeholder="Kategori Adı">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                <div class="mt-2">
                                                    <button id="btnKategoriOlustur" type="button" class="btn btn-primary btn-block btn-lg" onclick="kategoriPush();">Kategori Oluştur</button>
                                                </div>
                                        </div>
                                    </div>
                                </div>
                        </div>
                </div>`;
    $("#modalArea").html(kategoriModal);
    $("#mdlYeniAnaKategori").modal('show');


}

function yeniYayin() {
    let yayinEkleModal = `<div class="modal fade modalbox" id="mdlYeniYayin" data-backdrop="static" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Yeni Yayın Ekle</h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body">
                                        <div class="section mt-4 mb-5">
                                          
                                                    <div class="section mt-4 mb-5">
                                            <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtYayinAd">Yayın(Marka) Adı</label>
                                                        <input type="text" class="form-control" id="txtYayinAd"
                                                            placeholder="Yayın(Marka) Adı">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                        <label class="label" for="txtLogo">Logo</label>
                                                        <input type="text" class="form-control" id="txtLogo"
                                                            placeholder="Yayın Logosu">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                <div class="mt-2">
                                                    <button id="btnYayinKaydet" type="button" class="btn btn-primary btn-block btn-lg" onclick="yayinPush();">Yayın Ekle</button>
                                                </div>
                                        </div>
                                    </div>
                                </div>
                        </div>
                </div>`;
    $("#modalArea").html(yayinEkleModal);
    $("#mdlYeniYayin").modal('show');


}
function yayinPush() {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "Yayın/Marka Sisteme Kayıt Edilecektir",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            var yayinAd = $("#txtYayinAd").val();
            var yayinLogo = $("#txtLogo").val();
            var obj = {
                YayinBaslik: yayinAd,
                Logo: yayinLogo
            }
            $.ajax({
                url: '/App/yeniYayin/',
                type: 'Post',
                data: { model: obj },
                dataType: 'json',
                success: function (data) {
                    Swal.fire({
                        title: data.msgTitle,
                        text: data.msg,
                        icon: data.msgIcon,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'Tamam'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            if (data.msgIcon == "warning") {

                            } else {
                                $("#mdlYeniYayin").modal('hide');
                                $(".modal").modal('hide');
                                $(".modal fade").remove();
                                $(".modal-backdrop").remove();
                            }
                        }
                    })

                }
            });
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });

}
function kategoriPush() {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "İlgili Kategori Sisteme Eklenecektir",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            var katAd = $("#txtKatAd").val();
            var katAciklama = $("#txtKatAciklama").val();
            var obj = {
                KategoriBaslik: katAd,
                Aciklama: katAciklama
            }
            $.ajax({
                url: '/App/yeniKategori/',
                type: 'Post',
                data: { model: obj },
                dataType: 'json',
                success: function (data) {
                    Swal.fire({
                        title: data.msgTitle,
                        text: data.msg,
                        icon: data.msgIcon,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'Tamam'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            if (data.msgIcon == "warning") {

                            } else {
                                $("#mdlYeniKategori").modal('hide');
                                $(".modal").modal('hide');
                                $(".modal fade").remove();
                                $(".modal-backdrop").remove();
                            }
                        }
                    })
                }
            });
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });

}
function okulPush() {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "İlgili Okul Sisteme Tanımlanacaktır",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            var katAd = $("#txtOkulAd").val();
            var obj = {
                OkulBaslik: katAd,
            }
            $.ajax({
                url: '/App/yeniOkul/',
                type: 'Post',
                data: { model: obj },
                dataType: 'json',
                success: function (data) {
                    Swal.fire({
                        title: data.msgTitle,
                        text: data.msg,
                        icon: data.msgIcon,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'Tamam'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            if (data.msgIcon == "warning") {

                            } else {
                                $("#mdlYeniOkul").modal('hide');
                                $(".modal").modal('hide');
                                $(".modal fade").remove();
                                $(".modal-backdrop").remove();
                            }
                        }
                    })
                }
            });
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });

}

function yeniOgr() {
    menuHide();

    $.ajax({
        url: '/App/drpKategoriYayin/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            var kategori = jQuery.parseJSON(data.jsonAnaKategoriList);
            var okul = jQuery.parseJSON(data.okulJSON);
            let ogrModal =
                `
                    <div class="modal fade modalbox" id="mdlYeniOgr" data-backdrop="static" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Yeni Öğrenci Kaydı</h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body">
                                        <div class="form-group basic">
                                            <div class="input-wrapper">
                                                    <label class="label" for="txtOgrAd">Kategori</label>
                                                    <select id="drpKategori" class="form-control">
<option value="">Lütfen Öğrencinin Kategorisini seçiniz</option>`;
            $.each(kategori, (index, value) => {
                ogrModal += `<option value="${value.Id}">${value.AnaKategoriBaslik}</option>`;
            });

            ogrModal += `</select>
                                                    <i class="clear-input">
                                                        <ion-icon name="close-circle"></ion-icon>
                                                    </i>
                                                </div>
                                            </div>
                                        <div class="form-group basic">
                                            <div class="input-wrapper">
                                                    <label class="label" for="drpOkul">Okul</label>
                                                    <select id="drpOkul" class="form-control">
<option value="">Lütfen Öğrencinin Kayıtlı Olduğu Okulu seçiniz</option>`;
            $.each(okul, (index, value) => {
                ogrModal += `<option value="${value.Id}">${value.OkulBaslik}</option>`;
            });

            ogrModal += `</select>
                                                    <i class="clear-input">
                                                        <ion-icon name="close-circle"></ion-icon>
                                                    </i>
                                                </div>
                                            </div>
                                        <div class="form-group basic">
                                            <div class="input-wrapper">
                                                    <label class="label" for="txtOgrAd">Ad</label>
                                                    <input type="text" class="form-control" id="txtOgrAd"
                                                        placeholder="Adı">
                                                    <i class="clear-input">
                                                        <ion-icon name="close-circle"></ion-icon>
                                                    </i>
                                                </div>
                                            </div>
                                            <div class="form-group basic">
                                                <div class="input-wrapper">
                                                    <label class="label" for="txtSoyAd">Soyad</label>
                                                    <input type="text" class="form-control" id="txtOgrSoyad"
                                                        placeholder="Soyadı">
                                                    <i class="clear-input">
                                                        <ion-icon name="close-circle"></ion-icon>
                                                    </i>
                                                </div>
                                            </div>
                                            <div class="form-group basic">
                                                <div class="input-wrapper">
                                                        <label class="label" for="txtTelefon">Telefon</label>
                                                        <input type="number" class="form-control" id="txtTelefon"
                                                            placeholder="Cep Telefonu">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>

                                                <div class="input-list">
                                                <div class="custom-control custom-checkbox">
                                                    <input id="ktpControl" onclick="ktuyeControl()" type="checkbox" class="custom-control-input">
                                                    <label class="custom-control-label" for="ktpControl">Kütüphane Üyesidir</label>
                                                </div>
                                            </div>



                                    <div class="mt-2">
                                        <button id="btnOgrPush" type="button" class="btn btn-primary btn-block btn-lg" onclick="ogrPush();">Öğrenci Kaydet</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                        `;

            $("#modalArea").html(ogrModal);
            $("#mdlYeniOgr").modal('show');
        }
    });


}
let uyDSt;
function ktuyeControl() {
    var checkBox = document.getElementById("ktpControl");
    if (checkBox.checked == true) {
        uyDSt = 2;
    } else {
        uyDSt = 0;
    }
}
function ogrPush() {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "Öğrenci/Kişi Sisteme Kayıt Edilecektir",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            var ad = $("#txtOgrAd").val();
            var soyAd = $("#txtOgrSoyad").val();
            var telefon = $("#txtTelefon").val();
            var kategoriID = $("#drpKategori option:selected").val();
            var okulID = $("#drpOkul option:selected").val();
            var obj = {
                Ad: ad,
                Soyad: soyAd,
                Telefon: telefon,
                kategoriID: kategoriID,
                okulId: okulID
            }
            $.ajax({
                url: '/App/yeniOgrenci/',
                type: 'Post',
                data: { model: obj, uyDSt: uyDSt },
                dataType: 'json',
                success: function (data) {
                    if (result.isConfirmed) {
                        if (data.msgIcon == "warning") {

                        } else {
               
                            Swal.fire({
                                title: data.msgTitle,
                                icon: data.msgIcon,
                                html: data.msg,

                                confirmButtonText:
                                    'Kapat'
                            })
                        }
                    }
                }
            });
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });

}
function kitapcikList() {
    menuHide();
    $.ajax({
        url: '/App/kitapcikListTable/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            var kitapcikObjData = jQuery.parseJSON(data);
            let kitapcikListModal = `
                    <div class="modal fade modalbox" id="mdlKitapcikList" data-backdrop="static" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Güncel Deneme(Kitapçık) Listesi</h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body">
                                    <div id="ogrTableBody">
                                          <div class="card text-white bg-secondary mb-2">
                                                <div class="card-header">Güncel Deneme(Kitapçık) Listesi</div>
                                                <div class="card-body">
                                                        <div class="table-responsive">
                                                                <table id="tblKitapcikList" class="table bg-info text-white table-borderless" style="border-radius: 10px;">
                                                                                        <thead>
                                                                                            <tr>
                                                                                                <th scope="col">Kategori</th>
                                                                                                <th scope="col"><b>Yayın</b></th>
                                                                                                <th scope="col">Kitapçık Başlık</th>
                                                                                                <th scope="col">Toplam Stok</th>
                                                                                                <th scope="col">Dolu Stok</th>
                                                                                                <th scope="col">Kalan Stok</th>
                                                                                                <th scope="col"></th>
                                                                                                <th scope="col"></th>
                                                                                                <th scope="col"></th>
                                                                                            </tr>
                                                                                        </thead>
                                                                                        <tbody class="text-white">`
            $.each(kitapcikObjData, (index, value) => {
                kitapcikListModal += `<tr>
                                                                                                <td width="auto">${value.KategoriBaslik}</td>
                                                                                                <td width="auto">${value.YayinBaslik}</td>
                                                                                                <td width="auto">${value.DenemeBaslik}</td>
                                                                                                <td width="auto">${value.toplamStok}</td>
                                                                                                <td width="auto">${value.doluStok}</td>
                                                                                                <td width="auto">${value.kalanStok}</td>
                                                                                                <td width="auto"><button type="button" class="btn btn-primary" onclick="customStok(${value.Id})">Stok İşlemleri</button>
                                                                                                </td>
                                                                                                <td width="auto"><button type="button" class="btn btn-danger" onclick="kitapcikSil(${value.Id})">Kitapçık Sil</button></td>
                                                                                                <td width="auto"><button type="button" class="btn btn-warning" onclick="kitapcikDetay(${value.Id})">Bilgi Güncelle</button></td>
                                                                                                </tr>`;
            });
            kitapcikListModal += `</tbody>
                                                                                    </table>
                                                        </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>`;

            $("#modalArea").html(kitapcikListModal);
            $('#tblKitapcikList').DataTable({
                "language": {
                    "url": "//cdn.datatables.net/plug-ins/1.12.1/i18n/tr.json"
                }
            });
            $("#mdlKitapcikList").modal('show');

        }
    });
}
function okulList() {
    $.ajax({
        url: '/App/okulList/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            var okuListData = jQuery.parseJSON(data);
            let okulListModal = `
                    <div class="modal fade modalbox" id="mdlOkulList" data-backdrop="static" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Güncel Deneme(Kitapçık) Listesi</h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body">
                                    <div id="ogrTableBody">
                                          <div class="card text-white bg-secondary mb-2">
                                                <div class="card-header">Güncel Okulların Listesi</div>
                                                <div class="card-body">
                                                    <table id="tblKitapcikList" class="table bg-info text-white table-borderless" style="border-radius: 10px;">
                                                                                        <thead>
                                                                                            <tr>
                                                                                                <th scope="col"><b>Okul Adı</b></th>
                                                                                                <th scope="col">Toplam Kayıtlı Öğrenci</th>
                                                                                            </tr>
                                                                                        </thead>
                                                                                        <tbody class="text-white">`
            $.each(okuListData, (index, value) => {
                okulListModal += `<tr>
                                                                                                <td width="auto">${value.OkulBaslik}</td>
                                                                                                <td width="auto">${value.toplamOgrenci}</td>
                              
                                                                                                </tr>`;
            });
            okulListModal += `</tbody>
                                                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>`;

            $("#modalArea").html(okulListModal);
            $('#tblKitapcikList').DataTable({
                "language": {
                    "url": "//cdn.datatables.net/plug-ins/1.12.1/i18n/tr.json"
                }
            });
            $("#mdlOkulList").modal('show');

        }
    });
}

function yeniKitapcikStok() {
    menuHide();
    $.ajax({
        url: '/App/denemStokLists/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        dataType: 'json',
        success: function (data) {

            let stokModal = `<div class="modal fade modalbox" id="mdlDenemeStoklar" data-backdrop="static" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Kitapçık Stok İşlemleri</h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body">
<div class="card" style="height: auto !important;">
        <div class="card-header">Yeni Stok İşlemi</div>
                                            <div class="row" style="padding:15px;">
                                                    <div class="col-sm-6">
                                                    <div class="form-group basic">
                                                            <div class="input-wrapper">
                                                                <label class="label" for="drpDenemeKitapcik">Deneme(Kitapçık) Seçiniz</label>
                                                                <select id="drpDenemeKitapcik" class="form-control" onchange="btnEnable(1)">
                                                                <option value="">Deneme Kitapçığı Seçiniz</option>
                                                                `;
            var kitapcikData = jQuery.parseJSON(data.denemeJSON);
            $.each(kitapcikData, (index, value) => {
                stokModal += `<option value="${value.Id}">Yayın: ${value.YayinBaslik} | ${value.DenemeBaslik} | ${value.Kategori.AltKategoriBaslik}</option>`
            });
            stokModal += `</select>
                                                                <i class="clear-input">
                                                                    <ion-icon name="close-circle"></ion-icon>
                                                                </i>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6">
                                                        <div class="form-group basic">
                                                                <div class="input-wrapper">
                                                                    <label class="label" for="drpStokType">Deneme(Kitapçık) Seçiniz</label>
                                                                    <select id="drpStokType" class="form-control" onchange="btnEnable(2)">
                                                                    <option value="">Stok Türünü Seçiniz</option>
                                                                    <option value="1">Stok Giriş</option>
                                                                    <option value="2">Stok Çıkış(Düş)</option>
                                                                    </select>
                                                                    <i class="clear-input">
                                                                        <ion-icon name="close-circle"></ion-icon>
                                                                    </i>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-12">
                                                    <div class="form-group basic">
                                                            <div class="input-wrapper">
                                                                <label class="label" for="txtSAdet">Adet</label>
                                                                <input type="number" class="form-control" id="txtSAdet"
                                                                    placeholder="Stok Adedi" onkeypress="return isNumberKey(event)">
                                                                <i class="clear-input">
                                                                    <ion-icon name="close-circle"></ion-icon>
                                                                </i>
                                                            </div>
                                                        </div>
                                                    </div>
                                                <div class="mt-2">
                                                    <button id="btnStokIslem" type="button" class="btn btn-primary btn-block btn-lg" onclick="stokPush();" disabled>Stok Kaydet</button>
                                                </div>
<div class="row" style="margin-top:1rem;">
<div class="col-md-6">
<div class="card" style="height: 100% !important;">
        <div class="card-header">Stok Girişler</div>
<div class="card-body"> 
        <table id="tblStokGiris" class="table table-sm">

        <thead>
            <tr>
                <th>Yayın</th>
                <th style="width:200px;">Kitapçık</th>
                <th>Adet</th>
                <th>Seçenekler</th>
            </tr>
        </thead>
        <tbody>`
            var stokGirisData = jQuery.parseJSON(data.stokGirisJSON);
            $.each(stokGirisData, (index, value) => {
                stokModal += `<tr><td>${value.yayinBaslik}</td><td>${value.denemeBaslik}</td><td>${value.Adet}</td><td><button type="button" class="btn btn-danger btn-sm" onclick="stokKayitSil(${value.Id})">Kayıt Sil</button></td></tr>`
            });
            stokModal += `</tbody>

        </table>
    </div>
</div>
</div>
<div class="col-md-6">
<div class="card" style="height: 100% !important;">
        <div class="card-header">Stok Çıkışlar</div>
<div class="card-body">
    <table id="tblStokCikis" class="table table-sm">

    <thead>
        <tr>
            <th>Yayın</th>
            <th style="width:200px;">Kitapçık</th>
            <th>Adet</th>
            <th>Seçenekler</th>

        </tr>
    </thead>
    <tbody>`
            var stokCikisData = jQuery.parseJSON(data.stokCikisJSON);
            $.each(stokCikisData, (index, value) => {
                stokModal += `<tr><td>${value.yayinBaslik}</td><td>${value.denemeBaslik}</td><td>${value.Adet}</td><td><button type="button" class="btn btn-danger btn-sm" onclick="stokKayitSil(${value.id})">Kayıt Sil</button></td></tr>`
            });
            stokModal += `</tbody>

</table>
    </div>
</div>
</div>
</div>
</div>
</div>

</div>
                                        </div>
                                    </div>
                </div>`;
            $("#modalArea").html(stokModal);
            $('#tblStokGiris').DataTable({
                "language": {
                    "url": "//cdn.datatables.net/plug-ins/1.12.1/i18n/tr.json"
                }
            });
            $('#tblStokCikis').DataTable({
                "language": {
                    "url": "//cdn.datatables.net/plug-ins/1.12.1/i18n/tr.json"
                }
            });
            $("#mdlDenemeStoklar").modal('show');
        }
    });
}
function stokPush() {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "Stok Hareketi Kaydedilecektir",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            var kitapcikID = $("#drpDenemeKitapcik option:selected").val();
            var stokType = $("#drpStokType option:selected").val();
            var adet = $("#txtSAdet").val();
            var obj = {
                DenemeId: kitapcikID,
                StokType: stokType,
                Adet: adet
            }
            $.ajax({
                url: '/App/denemeStokAdd/',
                type: 'Post',
                data: { model: obj },
                dataType: 'json',
                success: function (data) {
                    Swal.fire({
                        title: data.msgTitle,
                        text: data.msg,
                        icon: data.msgIcon,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'Tamam'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            if (data.msgIcon == "warning") {

                            } else {
                                $("#mdlDenemeStoklar").modal('hide');
                                $(".modal").modal('hide');
                                $(".modal fade").remove();
                                $(".modal-backdrop").remove();
                            }
                        }
                    })
                }
            });
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });

}
function btnEnable(typ) {
    var val;
    if (typ == 1) {
        val = $("#drpDenemeKitapcik option:selected").val();
    } else if (typ == 2) {
        val = $("#drpStokType option:selected").val();

    }
    if (val == 1 || val == 2) {
        $('#btnStokIslem').prop('disabled', false);
    } else {
        $('#btnStokIslem').prop('disabled', true);
    }
}
function stokKayitSil(stkID) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "Kitapçık Stok Kaydı Silinecektir",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/App/stokKayitSil/',
                type: 'Post',
                data: { stokID: stkID },
                dataType: 'json',
                success: function (data) {
                    Swal.fire({
                        title: data.msgTitle,
                        text: data.msg,
                        icon: data.msgIcon,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'Tamam'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            if (data.msgIcon == "warning") {

                            } else {
                                $(".modal").modal('hide');
                                $(".modal fade").remove();
                                $(".modal-backdrop").remove();
                            }
                        }
                    })
                }
            });
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });
}
function kitapcikSil(kitapcikID) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "Kitapçık Silinecektir",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/App/kitapcikSil/',
                type: 'Post',
                data: { kitapcikID: kitapcikID },
                dataType: 'json',
                success: function (data) {
                    Swal.fire({
                        title: data.msgTitle,
                        text: data.msg,
                        icon: data.msgIcon,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'Tamam'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            if (data.msgIcon == "warning") {

                            } else {
                                $(".modal").modal('hide');
                                $(".modal fade").remove();
                                $(".modal-backdrop").remove();
                            }
                        }
                    })
                }
            });
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });
}



function snDt(id) {
    $.ajax({
        url: '/App/seansDetay/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        data: { id: id },
        dataType: 'json',
        success: function (data) {
            var seansData = jQuery.parseJSON(data.jsonSeansDetayResult);
            var seansKitapcikList = jQuery.parseJSON(data.jsonSeandDetayKitapcikList);
            let seansDetayModal = `<div class="modal fade modalbox" id="mdlSeansDetayliBilgi" data-backdrop="static" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">${seansData.kitapcikBaslik}</h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body">
                                        <div class="section mt-4 mb-5">
                                            <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                            <label class="label" for="drpDenemeKitapcik">Deneme Seçiniz</label>
                                                        <select id="drpDenemeKitapcik" class="form-control" onchange="denemeKontenjanSet(this)">
                                                        <option value="">Deneme Kitapçığı Seçiniz</option>
                                                        `

            $.each(seansKitapcikList, (index, value) => {
                seansDetayModal += `<option value="${value.Id}">Yayın:${value.YayinBaslik} | Kategori: ${value.KategoriBaslik} | Deneme: ${value.DenemeBaslik}</option>`
            });
            seansDetayModal += ` </select>
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtSnKontenjan">Kontenjan</label>
                                                        <input type="number" class="form-control" id="txtSnKontenjan"
                                                            placeholder="Deneme Sınavına Katılacak Toplam Kişi" value="${seansData.Kontenjan}" onkeyup="knControl(this)" onkeypress="return isNumberKey(event)">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                    <div id="kontenjanMsg1"></div>

                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtUcret">Seans Ücreti</label>
                                                        <input type="number" class="form-control" id="txtUcret"
                                                            placeholder="Kişi Başı Deneme Seansı Ücreti" value="${seansData.SeansUcret}" onkeypress="return isNumberKey(event)">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtTarih">Tarih</label>
                                                        <input type="date" class="form-control" id="txtTarih"
                                                            placeholder="Seans Tarihi">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtSaat">Saat</label>
                                                        <input type="time" class="form-control" value="${seansData.Saat}" id="txtSaat"
                                                            placeholder="Seans Saati">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                <div class="mt-2">
                                                    <button id="btnSeansGuncelle" type="button" class="btn btn-primary btn-block btn-lg" onclick="yayinPush();">Seans Bilgi Güncelle</button>
                                                </div>
                                        </div>
                                    </div>
                                </div>
                        </div>
                </div>`;
            $("#modalArea").html(seansDetayModal);
            $("#drpDenemeKitapcik option[value='" + seansData.kitapcikID + "']").prop('selected', true);
            document.getElementById("txtTarih").value = seansData.TarihSTR;
            denemeKontenjanSet();
            knControl();
            $("#mdlSeansDetayliBilgi").modal('show');

            $("#btnSeansGuncelle").click(function () {
                const swalWithBootstrapButtons = Swal.mixin({
                    customClass: {
                        confirmButton: 'btn btn-success',

                        cancelButton: 'btn btn-danger swButton'
                    },
                    buttonsStyling: false
                })

                swalWithBootstrapButtons.fire({
                    title: 'İşlemi Onaylıyor musunuz?',
                    text: "Deneme Sınavı Seans Bilgileri Güncellenecektir",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Onaylıyorum',
                    cancelButtonText: 'İptal Et',
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    reverseButtons: true
                }).then((result) => {
                    if (result.isConfirmed) {
                        var denemeID = $("#drpDenemeKitapcik option:selected").val();
                        var kontenjan = $("#txtSnKontenjan").val();
                        var tarih = $("#txtTarih").val();
                        var saat = $("#txtSaat").val();
                        var ucret = $("#txtUcret").val().toString().replace(".", ",");
                        var obj = {
                            Id: id,
                            DenemeId: denemeID,
                            Tarih: tarih,
                            Saat: saat,
                            SeansUcret: ucret,
                            Kontenjan: kontenjan,
                            Durum: 1,
                            actionType: 2
                        }
                        $.ajax({
                            url: '/App/yeniSeans/',
                            type: 'Post',
                            data: { model: obj },
                            dataType: 'json',
                            success: function (data) {
                                if (result.isConfirmed) {
                                    if (data.msgIcon == "warning") {

                                    } else {
                                        $("#mdlYeniSeans").modal('hide');
                                        $(".modal").modal('hide');
                                        $(".modal fade").remove();
                                        $(".modal-backdrop").remove();
                                    }
                                }
                            }
                        });

                    } else if (
                        result.dismiss === Swal.DismissReason.cancel
                    ) {
                        Swal.fire({
                            title: '<strong>İşlem İptal Edildi</strong>',
                            icon: 'info',
                            html:
                                '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                            confirmButtonText:
                                'Kapat'
                        })
                    }
                });
            });
        }
    });
}

function knControl(el) {
    if (el == null) {
        el = $("#txtSnKontenjan").val();
    }
    if (Number(el.value) > kontenjan) {
        alertColor = "warning";
        msg = "Seçmiş Olduğunuz Deneme Kitapçığı İçin Enfazla " + kontenjan + " Kişi Kaydedebilirsiniz";
        document.getElementById('btnSeansGuncelle').disabled = true;
        let alert =
            `<div id="kontenjanAlert1" class="alert alert-outline-${alertColor} mb-1" role="alert">
                            <b id="msg1">${msg}</b>
                        </div>`;
        $("#kontenjanMsg1").html(alert);
    }
    else {
        document.getElementById('btnSeansGuncelle').disabled = false;
        $("#kontenjanAlert1").remove();

    }
}
$('.ogrSnSil').click(function(e){
    $(this).closest('tr').remove()
 })
 
function ogrSil(ogrID, seansID) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "Seçilen Öğrenci'nin Deneme Sınav Kaydı Silinecektir",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/App/ogrSeansKayitSil/',
                type: 'Post',
                data: { ogrID: ogrID, seansID: seansID },
                dataType: 'json',
                success: function (data) {
                    swalWithBootstrapButtons.fire(
                        data.msgTitle,
                        data.msg,
                        data.msgIcon
                    )
                    $("#ogrSnRw-" + seansID).remove();
                    $("#ogrRow-"+ogrID).remove();
                }
            });

        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });
}
function seansIptal(seansID) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "Seçilen Deneme Sınavı Seansı Iptal Edilecek Ve Tüm Sınav Kayıtları Silinecektir",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/App/seansIptal/',
                type: 'Post',
                data: { seansID: seansID },
                dataType: 'json',
                success: function (data) {
                    swalWithBootstrapButtons.fire(
                        data.msgTitle,
                        data.msg,
                        data.msgIcon
                    )
                    $(".modal").modal('hide');
                    $(".modal fade").remove();
                    $(".modal-backdrop ").remove();
                    document.getElementById("modalArea").innerHTML = "";
                }
            });

        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            })
        }
    });
}

function karZararGozlem() {
    $.ajax({
        url: '/App/karzararAction/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            var karZararData = jQuery.parseJSON(data);
            let karZararModalSet =
                `
                    <div class="modal fade modalbox" id="mdlDenemeKarZarar" data-backdrop="static" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Deneme Kitapçıkları Hedef Kâr & Ciro Gözlem</h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body">
                                    <div id="ogrTableBody">
                                          <div class="card text-white bg-secondary mb-2">
                                                <div class="card-body">
                                                    <table id="tblDenemeKarZararList" class="table table-dark" style="border-radius: 10px;">
                                                                                        <thead>
                                                                                            <tr>
                                                                                                <th scope="col">Yayın</th>
                                                                                                <th scope="col">Kategori</th>
                                                                                                <th scope="col"><b>Deneme Başlık</b></th>
                                                                                                <th scope="col">Toplam Stok</th>
                                                                                                <th scope="col">Toplam Kayıtlı Öğrenci</th>
                                                                                                <th scope="col">Güncel Stok</th>
                                                                                                <th scope="col">Birim Maliyet</th>
                                                                                                <th scope="col">Toplam Maliyet</th>
                                                                                                <th scope="col">Birim Kar</th>
                                                                                                <th scope="col">Toplam Kar</th>
                                                                                                <th scope="col">Hedef Ciro</th>  
                                                                                            </tr>
                                                                                        </thead>
                                                                                        <tbody>`
            $.each(karZararData, (index, value) => {
                karZararModalSet += `<tr>
                                                                                <td width="auto"><b>${value.YayinBaslik}</b></td>
                                                                                <td width="auto">${value.Kategori.KategoriBaslik}</td>
                                                                                <td width="auto">${value.DenemeBaslik}</td>
                                                                                <td width="auto">${value.toplamStok}</td>
                                                                                <td width="auto">${value.doluStok}</td>
                                                                                <td width="auto">${value.kalanStok}</td>
                                                                                <td width="auto">${value.GirisFiyat} TL</td>
                                                                                <td width="auto">${value.toplamMaliyet} TL</td>
                                                                                <td width="auto">${value.birimKar} TL</td>
                                                                                <td width="auto">${value.toplamKar} TL</td>
                                                                                <td width="auto">${value.hedefKar} TL</td>
                                                                        </tr>`;
            });
            karZararModalSet += `</tbody>
                                                                                    </table>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>`;

            $("#modalArea").html(karZararModalSet);
            $('#tblDenemeKarZararList').DataTable({
                "language": {
                    "url": "//cdn.datatables.net/plug-ins/1.12.1/i18n/tr.json"
                }
            });
            $("#mdlDenemeKarZarar").modal('show');

        }
    });
}
function kitapcikDetay(kitapcikID) {
    $.ajax({
        url: '/App/kitapcikDetay/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        data: { id: kitapcikID },
        dataType: 'json',
        success: function (data) {
            var kitapcikDetayData = jQuery.parseJSON(data);

            let kitapcikDetayModal = `<div class="modal fade modalbox" id="mdlKitapcikDetayModal" data-backdrop="static" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">${kitapcikDetayData.DenemeBaslik}</h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body">
                                        <div class="section mt-4 mb-5">
                                            <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                            <label class="label" for="drpDenemeKitapcik">Yayın</label>
                                                        <select id="drpDenemeYayin" class="form-control">
                                                        <option value="">Yayın Seçiniz</option>
                                                        `

            var kitapcikData = jQuery.parseJSON(data);
            $.each(kitapcikData.yayinList, (index, value) => {
                kitapcikDetayModal += `<option value="${value.Id}">${value.YayinBaslik}</option>`
            });
            kitapcikDetayModal += ` </select>
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                              <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                            <label class="label" for="drpDenemeKitapcik">Kategori</label>
                                                        <select id="drpDenemeKategori" class="form-control">
                                                        <option value="">Kategori Seçiniz</option>
                                                        `

            var kategoriData = jQuery.parseJSON(data);
            $.each(kitapcikData.kategoriList, (index, value) => {
                kitapcikDetayModal += `<option value="${value.Id}">${value.KategoriBaslik}</option>`
            });
            kitapcikDetayModal += ` </select>
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtKitapcikAd">Deneme Adı</label>
                                                        <input type="text" class="form-control" id="txtKitapcikAd"
                                                            placeholder="Deneme Kitapçığı Adı" value="${kitapcikDetayData.DenemeBaslik}">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                              
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtUcret">Birim Fiyatı</label>
                                                        <input type="number" class="form-control" id="txtKitapcikAdFiyat"
                                                            placeholder="Kitapçık Birim Maliyeti" value="${kitapcikDetayData.GirisFiyat}">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                <div class="mt-2">
                                                    <button id="btnKitapcikGuncelle" type="button" class="btn btn-primary btn-block btn-lg">Kitapçık Bilgi Güncelle</button>
                                                </div>
                                        </div>
                                    </div>
                                </div>
                        </div>
                </div>`;
            $("#modalArea").html(kitapcikDetayModal);
            $("#drpDenemeYayin option[value='" + kitapcikDetayData.YayinId + "']").prop('selected', true);
            $("#drpDenemeKategori option[value='" + kitapcikDetayData.KategoriId + "']").prop('selected', true);
            $("#mdlKitapcikDetayModal").modal('show');

            $("#btnKitapcikGuncelle").click(function () {
                const swalWithBootstrapButtons = Swal.mixin({
                    customClass: {
                        confirmButton: 'btn btn-success',

                        cancelButton: 'btn btn-danger swButton'
                    },
                    buttonsStyling: false
                })

                swalWithBootstrapButtons.fire({
                    title: 'İşlemi Onaylıyor musunuz?',
                    text: "Kitapçık Bilgileri Güncellenecektir",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Onaylıyorum',
                    cancelButtonText: 'İptal Et',
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    reverseButtons: true
                }).then((result) => {
                    if (result.isConfirmed) {
                        var yayin = $("#drpDenemeYayin option:selected").val();
                        var kategori = $("#drpDenemeKategori option:selected").val();
                        var kitapcikAd = $("#txtKitapcikAd").val();
                        var GirisFiyat = $("#txtKitapcikAdFiyat").val().toString().replace(".", ",");

                        var obj = {
                            Id: kitapcikID,
                            YayinId: yayin,
                            KategoriId: kategori,
                            DenemeBaslik: kitapcikAd,
                            GirisFiyat: GirisFiyat
                        }
                        $.ajax({
                            url: '/App/kitapcikGuncelle/',
                            type: 'Post',
                            data: { model: obj },
                            dataType: 'json',
                            success: function (data) {
                                if (data.msgIcon == "warning") {
                                    Swal.fire({
                                        title: data.msgTitle,
                                        icon: msgIcon,
                                        html:
                                            '<b>' + data.msg + '</b>',

                                        confirmButtonText:
                                            'Kapat'
                                    })
                                } else if (data.msgIcon == "success") {
                                    Swal.fire({
                                        title: data.msgTitle,
                                        icon: data.msgIcon,
                                        html:
                                            '<b>' + data.msg + '</b>',

                                        confirmButtonText:
                                            'Kapat'
                                    }).isConfirmed(function () {

                                        $("#mdlKitapcikDetayModal").modal('hide');
                                        $(".modal").modal('hide');
                                        $(".modal fade").remove();
                                        $(".modal-backdrop").remove();
                                    });

                                }
                            },
                            Error: function (data) {
                                Swal.fire({
                                    title: 'Hata Oluştu',
                                    icon: 'error',
                                    html:
                                        '<b>Lütfen Boş Alan Bırakmayınız</b>',

                                    confirmButtonText:
                                        'Kapat'
                                })
                            }
                        });

                    } else if (
                        result.dismiss === Swal.DismissReason.cancel
                    ) {
                        Swal.fire({
                            title: '<strong>İşlem İptal Edildi</strong>',
                            icon: 'info',
                            html:
                                '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                            confirmButtonText:
                                'Kapat'
                        })
                    } Great
                });
            });
        }
    });
}
function ogrenciDetay(ogrID) {
    $.ajax({
        url: '/App/ogrDetay/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        data: { ogrID: ogrID },
        dataType: 'json',
        success: function (data) {
            var ogrDetayData = jQuery.parseJSON(data);

            let ogrDetayModal = `<div class="modal fade modalbox" id="mdlOgrDetay" data-backdrop="static" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">${ogrDetayData.Ad} ${ogrDetayData.Soyad} İsimli Öğrenci Bilgileri</h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body">
                                        <div class="section mt-4 mb-5">
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtOgrAd">Ad</label>
                                                        <input type="text" class="form-control" id="txtOgrAd"
                                                            placeholder="Öğrenci Adı" value="${ogrDetayData.Ad}">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                              
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtOgrSoyad">Soyad</label>
                                                        <input type="text" class="form-control" id="txtOgrSoyad"
                                                            placeholder="Öğrenci Soyadı" value="${ogrDetayData.Soyad}">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtTelefon">Telefon</label>
                                                        <input type="text" class="form-control" id="txtTelefon"
                                                            placeholder="Öğrenci Cep Telefonu" value="${ogrDetayData.Telefon}">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                             
                                                <div class="mt-2">
                                                    <button id="btnOgrGuncelle" type="button" class="btn btn-primary btn-block btn-lg">Öğrenci Bilgi Güncelle</button>
                                                </div>
                                        </div>
                                    </div>
                                </div>
                        </div>
                </div>`;
            $("#modalArea").html(ogrDetayModal);

            $("#mdlOgrDetay").modal('show');

            $("#btnOgrGuncelle").click(function () {
                const swalWithBootstrapButtons = Swal.mixin({
                    customClass: {
                        confirmButton: 'btn btn-success',

                        cancelButton: 'btn btn-danger swButton'
                    },
                    buttonsStyling: false
                })

                swalWithBootstrapButtons.fire({
                    title: 'İşlemi Onaylıyor musunuz?',
                    text: "Öğrenci Bilgileri Güncellenecektir",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Onaylıyorum',
                    cancelButtonText: 'İptal Et',
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    reverseButtons: true
                }).then((result) => {
                    if (result.isConfirmed) {
                        var ad = $("#txtOgrAd").val();
                        var soyAd = $("#txtOgrSoyad").val();
                        var telefon = $("#txtTelefon").val();
                        var kategoriId = $("#drpKategori option:selected").val();
                        var obj = {
                            Id: ogrID,
                            Ad: ad,
                            Soyad: soyAd,
                            Telefon: telefon,
                            kategoriId: kategoriId
                        }
                        $.ajax({
                            url: '/App/ogrGuncellle/',
                            type: 'Post',
                            data: { model: obj },
                            dataType: 'json',
                            success: function (data) {
                                if (data.msgIcon == "warning") {
                                    Swal.fire({
                                        title: data.msgTitle,
                                        icon: msgIcon,
                                        html:
                                            '<b>' + data.msg + '</b>',

                                        confirmButtonText:
                                            'Kapat'
                                    })
                                } else if (data.msgIcon == "success") {

                                    Swal.fire({
                                        title: data.msgTitle,
                                        icon: data.msgIcon,
                                        html: '<b>' + data.msg + '</b>',
                                        confirmButtonText: 'Kapat'
                                    }).isConfirmed(function () {
                                        $("#mdlOgrDetay").modal('hide');
                                        $(".modal").modal('hide');
                                        $(".modal fade").remove();
                                        $(".modal-backdrop").remove();
                                    });

                                }
                            },
                            Error: function (data) {
                                Swal.fire({
                                    title: 'Hata Oluştu',
                                    icon: 'error',
                                    html:
                                        '<b>Lütfen Boş Alan Bırakmayınız</b>',

                                    confirmButtonText:
                                        'Kapat'
                                })
                            }
                        });

                    } else if (
                        result.dismiss === Swal.DismissReason.cancel
                    ) {
                        Swal.fire({
                            title: '<strong>İşlem İptal Edildi</strong>',
                            icon: 'info',
                            html:
                                '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                            confirmButtonText:
                                'Kapat'
                        });
                    }
                });
            });
        }
    });
}
function kasaGozlem() {
    $.ajax({
        url: '/App/kasaDurum/',
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            let kasaGozlemModal = `<div class="modal fade modalbox" id="mdlKasaGozlem" data-backdrop="static" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title"></h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body">
                                        <div class="section mt-4 mb-5">
                                            <div class="container-fluid">
                                                    <div class="row">
                                                        <div class="col-sm-12">
                                                            <div class="card text-white bg-info mb-2">
                                                                <div class="card-header">Güncel Kasa Durumu</div>
                                                                    <div class="card-body">
                                                                        <h5 class="card-title">${data.karToplam} TL</h5>
                                                                    </div>
                                                            </div> 
                                                        </div>
                                                        <div class="col-sm-6">
                                                               <div class="card text-white bg-danger mb-2" style="margin-top: 1.2rem;">
                                                                <div class="card-header">Kasa Giderleri</div>
                                                                    <div class="card-body">
                                                                        <h5 class="card-title">${data.maliyetToplam} TL</h5>
                                                                    </div>
                                                            </div> 
                                                        </div>
                                                        <div class="col-sm-6" style="margin-top: 1.2rem;">
                                                            <div class="card text-white bg-success mb-2">
                                                                <div class="card-header">Kasa Gelirleri</div>
                                                                    <div class="card-body">
                                                                        <h5 class="card-title">${data.ciroToplam}TL</h5>
                                                                    </div>
                                                            </div>  
                                                        </div>
                                                    </div>                                                    
                                            </div>
                                           
                                        </div>
                                    </div>
                                </div>
                        </div>
                </div>`;
            $("#modalArea").html(kasaGozlemModal);

            $("#mdlKasaGozlem").modal('show');
        },
        Error: function () {

        }
    });
}
function tarihselRaporOlustur() {
    let kasaTarihselSorguModal =
        `<div class="modal fade modalbox" id="mdlKasaTarihselSorguModal" data-backdrop="static" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title"></h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body">
                                        <div class="section mt-4 mb-5">
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtD1">Rapor Başlangıç Tarih</label>
                                                        <input type="date" class="form-control" id="txtD1"
                                                            placeholder="Başlangıç Tarihi">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                                <div class="form-group basic">
                                                    <div class="input-wrapper">
                                                        <label class="label" for="txtD2">Rapor Bitiş Tarih</label>
                                                        <input type="date" class="form-control" id="txtD2"
                                                            placeholder="Rapor Bitiş Tarihi">
                                                        <i class="clear-input">
                                                            <ion-icon name="close-circle"></ion-icon>
                                                        </i>
                                                    </div>
                                                </div>
                                      <div class="mt-2">
                                                <button id="btnTarihselSorgu" type="button" class="btn btn-primary btn-block btn-lg">Kaydet</button>
                                            </div>
                                        <div id="raporListDiv" class="section mt-4 mb-5">
                                            
                                        </div>
                                        </div>
                                    </div>
                                </div>
                        </div>
                </div>`;
    $("#modalArea").html(kasaTarihselSorguModal);

    $.ajax({
        url: '/App/raporList/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            var raporListData = jQuery.parseJSON(data.raporListJSON)
            let raporListDiv = `<table class="table table-sm">
                <thead>
                <tr>
                    <th>Sorgu Başlangıç</th>
                    <th>Sorgu Bitiş</th>
                    <th>Rapor Tarih</th>
                    <th>Rapor Ay</th>
                    <th>Hedef Kâr</th>
                    <th>Toplam Kâr</th>
                    <th>Hedef-Toplam Kâr Farkı</th>
                    <th>Toplam Ciro</th>
                    <th>Toplam Stok Giriş / Toplam Masraf</th>
                    <th>Iskarta Deneme (Stok Çıkış)) / Zarar</th>
                    <th>Toplam Seans / Katılan Öğrenci</th>
                    <th>Şampiyon Deneme</th>
                    <th>Kötü Deneme</th>
                </tr>
                </thead><tbody>`;
            $.each(raporListData, (index, value) => {
                raporListDiv += `
                            <tr>
                                <td>${value.D1}</td>
                                <td>${value.D2}</td>
                                <td>${value.raporTarih}</td>
                                <td>${value.RaporAy}</td>
                                <td>${value.hedefKar}</td>
                                <td>${value.toplamKar}</td>
                                <td>${value.HedefKarToplamKarFark}</td>
                                <td>${value.toplamCiro}</td>
                                <td>${value.toplamStokGirisler} / ${value.toplammasraf}</td>
                                <td>${value.toplamStokCikislar} / ${value.IskartaDenemeZarar}</td>
                                <td>${value.sampiyonDeneme}</td>
                                <td>${value.kotuDeneme}</td>
                            </tr>
                            `;
            });
            raporListDiv += `</tbody></table>`;
        },
        Error: function (data) {

        }
    });

    $("#raporListDiv").html(raporListDiv);
    $("#mdlKasaTarihselSorguModal").modal('show');

    $("#btnTarihselSorgu").click(function () {
        var D1 = $("#txtD1").val();
        var D2 = $("#txtD2").val();
        var raporResult;
        $.ajax({
            url: '/App/tarihselSorgu/',
            contentType: 'application/json; charset=utf-8;',
            type: 'Get',
            data: { D1: D1, D2: D2 },
            dataType: 'json',
            success: function (data) {
                if (data.msgIcon == "warning") {
                    Swal.fire(
                        data.msgTitle,
                        data.msg,
                        data.msgIcon
                    )
                } else {
                    raporResult = jQuery.parseJSON(data.raporSonucjSON);
                    let raporSonuc =
                        `
                        <div class="modal fade modalbox" id="mdlRaporSonuc" data-backdrop="static" tabindex="-1" role="dialog">
                            <div class="modal-dialog" role="document">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title">Tarihsel Bazda Rapor Sonucu </h5>
                                        <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                    </div>
                                    <div class="modal-body">
                                            <button id="btnRaporPush" type="button" class="btn btn-warning">Rapor Kaydet</button>
                                        <div class="container-fluid">
                                        <div class="row" style="margin-top:1rem;">
                                            <div class="col-sm-4">
                                                <div class="alert alert-info" role="alert">
                                                    Rapor Başlangıç Tarihi: ${raporResult.D1}
                                                  </div>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="alert alert-info" role="alert">
                                                    Rapor Bitiş Tarihi: ${raporResult.D2}
                                                </div>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="alert alert-info" role="alert">
                                                    Rapor Oluşturulma Tarihi: ${raporResult.raporTarih}
                                                </div>
                                            </div>
                                        </div>
                                        <hr>
                                        <div class="row">
                                            <div class="col-sm-3">
                                                <div class="card text-white bg-primary mb-3" style="max-width: 100%;">
                                                    <div class="card-header">Toplam Ciro</div>
                                                    <div class="card-body">
                                                      <h5 class="card-title">${raporResult.toplamCiro} TL</h5>
                                                    </div>
                                                  </div>
                                            </div>
                                            <div class="col-sm-3">
                                                <div class="card text-white bg-success mb-3" style="max-width: 100%;">
                                                    <div class="card-header">Hedef Kâr</div>
                                                    <div class="card-body">
                                                      <h5 class="card-title">${raporResult.hedefKar} TL</h5>
                                                    </div>
                                                  </div>
                                            </div>
                                            <div class="col-sm-3">
                                                <div class="card bg-danger mb-3" style="max-width: 100%;">
                                                    <div class="card-header text-white"><strong>Toplam Masraf</strong></div>
                                                    <div class="card-body text-white">
                                                      <h5 class="card-title">${raporResult.toplamMasraf} TL</h5>
                                                    </div>
                                                  </div>
                                            </div>
                                            <div class="col-sm-3">
                                                <div class="card border-success mb-3" style="max-width: 100%;">
                                                    <div class="card-header text-success"><strong>Elde Edilen Kâr</strong></div>
                                                    <div class="card-body text-success">
                                                      <h5 class="card-title">${raporResult.toplamKar} TL</h5>
                                                    </div>
                                                  </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6" style="margin-top:1rem;">
                                                <div class="card text-white bg-info mb-3" style="max-width: 100%;">
                                                    <div class="card-header">Toplam Stok Giriş</div>
                                                    <div class="card-body">
                                                      <h5 class="card-title">${raporResult.toplamStokGirisler} Adet</h5>
                                                    </div>
                                                  </div>
                                            </div>
                                            <div class="col-sm-6" style="margin-top:1rem;">
                                                <div class="card text-white bg-info mb-3" style="max-width: 100%;">
                                                    <div class="card-header">Toplam Stok Giriş</div>
                                                    <div class="card-body">
                                                      <h5 class="card-title">${raporResult.toplamStokCikislar} Adet</h5>
                                                    </div>
                                                  </div>
                                            </div>

                                            <div class="col-sm-6" style="margin-top:1rem;">
                                                <div class="card text-white bg-dark mb-3" style="max-width: 100%;">
                                                    <div class="card-header">Düzenlenen Stok Girişi</div>
                                                    <div class="card-body">
                                                      <h5 class="card-title">${raporResult.toplamDenemeSeans} Seans</h5>
                                                    </div>
                                                  </div>
                                            </div>

                                            <div class="col-sm-6" style="margin-top:1rem;">
                                                <div class="card text-white bg-secondary mb-3" style="max-width: 100%;">
                                                    <div class="card-header text-white">Seanslara Katılan Toplam Öğrenci</div>
                                                    <div class="card-body text-white">
                                                      <h5 class="card-title">${raporResult.toplamSinavaGirenOgr} Kişi</h5>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6" style="margin-top:1rem;">
                                                <div class="card text-white bg-success mb-3" style="max-width: 100%;">
                                                    <div class="card-header text-white">Şampiyon Deneme</div>
                                                    <div class="card-body text-white">
                                                      <h5 class="card-title"> ${raporResult.sampiyonDeneme} / Hedef Kazanç / Elde edilen Kazanç</h5>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="col-sm-6" style="margin-top:1rem;">
                                                <div class="card text-white bg-danger mb-3" style="max-width: 100%;">
                                                    <div class="card-header text-white">Kötü Deneme</div>
                                                    <div class="card-body text-white">
                                                      <h5 class="card-title"> ${raporResult.kotuDeneme} / Hedef Kazanç / Elde edilen kazanç</h5>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                `;

                    $("#modalArea").html(raporSonuc);
                    $("#mdlRaporSonuc").modal('show');
                    $("#btnRaporPush").click(function () {
                        const swalWithBootstrapButtons = Swal.mixin({
                            customClass: {
                                confirmButton: 'btn btn-success',

                                cancelButton: 'btn btn-danger swButton'
                            },
                            buttonsStyling: false
                        })

                        swalWithBootstrapButtons.fire({
                            title: 'İşlemi Onaylıyor musunuz?',
                            text: "Hazırlanan Rapor Sisteme Kaydedilecektir",
                            icon: 'info',
                            showCancelButton: true,
                            confirmButtonText: 'Onaylıyorum',
                            cancelButtonText: 'İptal Et',
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            reverseButtons: true
                        }).then((result) => {
                            if (result.isConfirmed) {

                                $.ajax({
                                    url: '/App/raporKaydet/',
                                    type: 'Post',
                                    data: { model: raporResult },
                                    dataType: 'json',
                                    success: function (data) {

                                        Swal.fire({
                                            title: data.msgTitle,
                                            icon: data.msgIcon,
                                            html: '<b>' + data.msg + '</b>',
                                            confirmButtonText: 'Kapat'
                                        }).then((result) => {
                                            $(".modal").modal('hide');
                                            $(".modal fade").remove();
                                            $(".modal-backdrop").remove();
                                        })

                                    },
                                    Error: function (data) {
                                        Swal.fire({
                                            title: 'Hata Oluştu',
                                            icon: 'error',
                                            html:
                                                '<b>Lütfen Boş Alan Bırakmayınız</b>',

                                            confirmButtonText:
                                                'Kapat'
                                        })
                                    }
                                });

                            } else if (
                                result.dismiss === Swal.DismissReason.cancel
                            ) {
                                Swal.fire({
                                    title: '<strong>İşlem İptal Edildi</strong>',
                                    icon: 'info',
                                    html:
                                        '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                                    confirmButtonText:
                                        'Kapat'
                                })
                            }
                        });
                    });
                }
            }
        });
    });



}

function raporListSet() {
    $.ajax({
        url: '/App/raporList/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            var raporListData = jQuery.parseJSON(data.raporListJSON);
            let raporListDiv = `<table class="table table-sm">
                    <thead>
                    <tr>
                        <th>Sorgu Başlangıç</th>
                        <th>Sorgu Bitiş</th>
                        <th>Rapor Tarih</th>
                        <th>Rapor Ay</th>
                        <th>Hedef Kâr</th>
                        <th>Toplam Kâr</th>
                        <th>Hedef-Toplam Kâr Farkı</th>
                        <th>Toplam Ciro</th>
                        <th>Toplam Stok Giriş / Toplam Masraf</th>
                        <th>Iskarta Deneme (Stok Çıkış)) / Zarar</th>
                        <th>Toplam Seans / Katılan Öğrenci</th>
                        <th>Şampiyon Deneme</th>
                        <th>Kötü Deneme</th>
                    </tr>
                    </thead><tbody>`;
            $.each(raporListData, (index, value) => {
                raporListDiv += `
                                <tr>
                                    <td>${value.D1}</td>
                                    <td>${value.D2}</td>
                                    <td>${value.raporTarih}</td>
                                    <td>${value.RaporAy}</td>
                                    <td>${value.hedefKar}</td>
                                    <td>${value.toplamKar}</td>
                                    <td>${value.HedefKarToplamKarFark}</td>
                                    <td>${value.toplamCiro}</td>
                                    <td>${value.toplamStokGirisler} / ${value.toplammasraf}</td>
                                    <td>${value.toplamStokCikislar} / ${value.IskartaDenemeZarar}</td>
                                    <td>${value.sampiyonDeneme}</td>
                                    <td>${value.kotuDeneme}</td>
                                </tr>
                                `;
            });
            raporListDiv += `</tbody></table>`;
        },
        Error: function (data) {

        }
    });

    $("#raporListDiv").html(raporListDiv);
    

}
function gunlukDenemeSatisList(){
    $.ajax({
        url: '/App/gunlukDenemeToplam/',
        type: 'Get',
        contentType: 'application/json; charset=utf-8;',
        dataType: 'json',
        success: function (data) {
            var gunlukDenemeSatisListData = jQuery.parseJSON(data.jsonGunlukDenemeKayitList);
            let listResultTbody = ``;
            $.each(gunlukDenemeSatisListData, (index, value) => {
                listResultTbody += `<tr><td>${value.denemeAd}</td><td>${value.yayinAd}</td><td>${value.kayitUcret} TL</td><td>${value.tarihSaat}</td></tr>`;
            });
            let gunlukDenemeSatisModalList =
                `
                    <div class="modal fade modalbox" id="mdlGunlukDenemeSatisList" data-backdrop="static" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title"></h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body">
<div class="row" style="margin-top:2rem;">
<div class="col-md-12">
<button class="btn btn-success" onclick="seansOgrListPrint();">Yazdır</button>
</div>
</div>
                                    <div class="body" style="margin-top:0.2rem; font-weight:bold !important;">
<center>
<div class="container-fluid">
<div class="row">
<div class="col-6">
    <div class="card text-white bg-primary mb-2">
    <div class="card-header">Satılan Toplam Deneme</div>
        <div class="card-body">
            <h1 class="card-title" id="toplamDenemekaydi">${data.toplamDenemeKaydi} Adet</h1>
            <p class="card-text">Günlük Deneme Kaydı</p>
        </div>
    </div>
</div>
<div class="col-6">
    <div class="card text-white bg-success mb-2">
    <div class="card-header">Toplam Deneme Cirosu</div>
        <div class="card-body">
            <h1 class="card-title" id="toplamDenemeCiro">${data.toplamDenemeGelir} TL</h1>
            <p class="card-text">Günlük Deneme Satışından Elde Edilen Kazanç</p>
        </div>
    </div>
</div>
</div>
<hr />

    <div class="row">
        <div class="col-12" style="font-weight: bold !important;">
            <h3>Günlük Deneme Satış Listesi</h3>
            <table class="table table-bordered table-striped" style="font-weight:bold; color:black;">
                <thead>
                    <tr>
                        <th scope="col"><h3>Deneme - Kategori</h3></th>
                        <th scope="col"><h3>Yayın</h3></th>
                        <th scope="col"><h3>Kayıt Ücret</h3></th>
                        <th scope="col"><h3>Tarih / Saat</h3></th>
                    </tr>
                </thead>
                <tbody id="gunlukDenemeSatisListBody">
                </tbody>
                <tfoot>
                    <tr>
                    <th scope="col"><h3>Deneme - Kategori</h3></th>
                    <th scope="col"><h3>Yayın</h3></th>
                    <th scope="col"><h3>Kayıt Ücret</h3></th>
                    <th scope="col"><h3>Tarih / Saat</h3></th>
                   </tr>
                </tfoot>
            </table>
        </div>
    </div>
</div>
</center>
</div>
                                </div>
                            </div>
                        </div>
                    </div>`;

            
            $("#modalArea").html(gunlukDenemeSatisModalList);
            $("#gunlukDenemeSatisListBody").html(listResultTbody);
            $("#mdlGunlukDenemeSatisList").modal('show');        
        }
    });
}
let OgrSeansList = ``;
let OgrSeansListBiten = ``;

function ogrSeansGetir(ogrID) {
    $.ajax({
        url: '/App/ogrSeanslar/',
        type: 'Post',
        data: { ogrNO: ogrID },
        dataType: 'json',
        success: function (data) {
            
         

            var ogrKayitliOlduguSeanslar = jQuery.parseJSON(data.jsonSeansListBitmeyen);
            var ogrKayitliOlduguBitenSeanslar = jQuery.parseJSON(data.jsonSeansListBiten);


            OgrSeansList = ``;
            $.each(ogrKayitliOlduguSeanslar, (index, value) => {
                OgrSeansList += `
                            <tr id="ogrSnRw-${value.Id}" class="bg-success">
                                <td scope="row">${value.ogrSeansKayitTarih}</td>
                                <th scope="row">${value.TarihSTR}</th>
                                <td scope="row">${value.yayinBaslik} / ${value.yayinKategoriBaslik} / ${value.kitapcikBaslik}</td>
                                <td scope="row">${value.SeansUcret} TL</td>
                                <td scope="row"><a href="javascript:;" onclick="ogrSil(${ogrID},${value.Id})" class="text-danger"><ion-icon name="close-circle-outline"></ion-icon></a></td>
                            </tr>`;

            });
            OgrSeansListBiten = ``;
            $.each(ogrKayitliOlduguBitenSeanslar, (indexBiten, valueBiten) => {
                OgrSeansListBiten += `
                            <tr id="${valueBiten.Id}" class="bg-secondary">
                                <td scope="row">${valueBiten.ogrSeansKayitTarih}</td>
                                <th scope="row">${valueBiten.TarihSTR}</th>
                                <td scope="row">${valueBiten.yayinBaslik} / ${valueBiten.yayinKategoriBaslik} / ${valueBiten.kitapcikBaslik}</td>
                                <td scope="row">${valueBiten.SeansUcret} TL</td>
                                <td scope="row"><a href="javascript:;" onclick="ogrSil(${ogrID},${valueBiten.Id})" class="text-danger"><ion-icon name="close-circle-outline"></ion-icon></a></td>
                            </tr>`;
            });
            $("#seansBody").html(OgrSeansList);
            $("#seansBodyBiten").html(OgrSeansListBiten);


        }
    });
}
function ogrBilgiGetir(id) {
    $.ajax({
        url: '/App/ogrBilgiGetir/', 
        type: 'Post',
        data: { ogrNO: id },
        dataType: 'json',
        success: function (data) {

            var ogrBilgi = jQuery.parseJSON(data);

            var ogrAdSoyad;
            ogrAdSoyad = ogrBilgi.Ad + " " + ogrBilgi.Soyad;
            var ogrTelefon;
            ogrTelefon = ogrBilgi.Telefon;
            var ogrKayitTarih;
            ogrKayitTarih = ogrBilgi.KtarihSTR;

            var uyelikTur;
            if (ogrBilgi.KutuphaneUye == 2) {
                uyelikTur = "UYANIK ÜYE";
            } else if (ogrBilgi.KutuphaneUye == 1) {
                uyelikTur = "DENEME SINAVI ÜYELİK";
            }
            let ogrDetayModal = `<div class="modal fade dialogbox" id="mdlOgrDetay" data-backdrop="static">
            <div class="modal-dialog" role="document">
                <div class="modal-content" style="    
    max-width: 100% !important;
max-height: 100% !important;
    height: 95% !important;
    width: 100% !important;">
<button type="button" class="btn btn-danger" onclick="customModalClose();" style="margin-left: auto; margin-right: 15px;
    margin-top: 2rem; padding: 15px;">Kapat</button>
    <button type="button" class="btn btn-danger" onclick="ogrParolaPush(${id});" style="margin-left: auto; margin-right: 15px;
    margin-top: 2rem; padding: 15px;">Yeni Şifre Gönder</button>
    <button type="button" class="btn btn-warning" onclick="ogrSinavHatirlat(${id})" style="margin-left: auto; margin-right: 15px;
    margin-top: 2rem; padding: 15px;">Sınav Hatırlat</button>
                    <div class="modal-icon text-primary" style="margin-top:0px !important;">
                        <ion-icon name="information-circle-outline" style="font-size:64px;"></ion-icon>
                    </div>
                    <div class="modal-header">
                        <h5 class="modal-title">Öğrenci Bilgileri</h5>
                    </div>
                    <div class="modal-body" style="padding: 5px 5px 5px 5px;">
    <div class="table-responsive">
                    <table class="table table-sm">
                        <thead>
                            <tr>
                            
                                <th scope="col">Ad Soyad</th>
                                <th scope="col">Telefon</th>
                                <th scope="col">Üyelik Türü</th>
                                <th scope="col">Kayıt Tarihi</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th scope="row">${ogrAdSoyad}</th>
                                <th scope="row">${ogrTelefon}</th>
                                <th scope="row">${uyelikTur}</th>
                                <th scope="row">${ogrKayitTarih}</th>
                            </tr>
                        </tbody>
                    </table>
                </div>
                        <div class="table-responsive">
<b>Kayıtlı Olduğu Deneme Seansları</b>
                    <table id="ogrDetaySeansListTable" class="table table-sm table-bordered">
                        <thead>
                            <tr>
                                <th scope="col">Kayıt Tarih</th>
                                <th scope="col">Seans Tarih / Saat</th>
                                <th scope="col">Yayın / Kategori / Seans</th>
                                <th scope="col">Ücret</th>
                                <th scope="col"></th>
                            </tr>
                        </thead>
                        <tbody id="seansBody">
                        </tbody>
            
                    </table>

                </div>
   <div class="table-responsive">
<b>Sona Ermiş Olan Deneme Seansları</b>
                    <table id="ogrDetaySeansListTable2" class="table table-sm table-bordered">
                        <thead>
                            <tr>

                                <th scope="col">Kayıt Tarih</th>
                                <th scope="col">Seans Tarih / Saat</th>
                                <th scope="col">Yayın / Kategori / Seans</th>
                                <th scope="col">Ücret</th>
                                <th scope="col"></th>
                            </tr>
                        </thead>
                        <tbody id="seansBodyBiten">
                        </tbody>    
                    </table>
                </div>
                    </div>
                </div>
            </div>
        </div>`;
            $("#modalArea2").html(ogrDetayModal);
            $("#mdlOgrDetay").modal('show');
        }
    });
    ogrSeansGetir(id);

}
function ogrSinavHatirlat(id){
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    });

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "Öğrenci Sınav Hatırlatma Mesajı Gönderilecektir",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/App/ogrSinavHatirlat/',
                type: 'Post',
                data: { ogrID: id },
                dataType: 'json',
                success: function (data) {
                    var ogrData = jQuery.parseJSON(data.ogrSetResult);
                    var snData = jQuery.parseJSON(data.seansListResult);
                    
                    let customWpMsg = ``;
                    customWpMsg += "Kayıtlı Olduğunuz Güncel Sınav Bilgileriniz,%0A";
                    $.each(snData, (index, value) => {
                        customWpMsg += `Yayın: ${value.yayinBaslik} - Kategori: ${value.KategoriBaslik} - Seans: ${value.kitapcikBaslik} Tarih / Saat: ${value.TarihSTR} / ${value.Saat} %0A`;
                    });
                    let customWp = `<hr /><a class="btn btn-success" href="whatsapp://send?text=`;
                    if (snData.length == 0){
                        swalWithBootstrapButtons.fire(
                            {
                                title: "<strong>İşlem Başarısız</strong>",
                                html: "<b>Öğrencinin Kayıtlı Olduğu Oturum Bulunamamıştır</b>",
                                icon: "warning",
                                showCancelButton: true,
                                showConfirmButton: false,
                                cancelButtonText: 'Kapat',
                            }
                        )
                    }else{
                        customWp += customWpMsg;
                        customWp += `&phone=` + '+9'+ogrData.Telefon + `"`;
                        customWp += `>Whatsapp Üzerinden İlet</a>`;
                        swalWithBootstrapButtons.fire(
                            {
                                title: "<strong>İşlem Başarılı</strong>",
                                html: customWp,
                                icon: "success",
                                showCancelButton: true,
                                cancelButtonText: 'Kapat',
                                showConfirmButton: false,

                            }
                        )
                    }
                  
                }
            });

        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            });
        }
    });
}
function ogrParolaPush(id) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',

            cancelButton: 'btn btn-danger swButton'
        },
        buttonsStyling: false
    });

    swalWithBootstrapButtons.fire({
        title: 'İşlemi Onaylıyor musunuz?',
        text: "Öğrenci Şifresi Yenilenecektir",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Onaylıyorum',
        cancelButtonText: 'İptal Et',
        allowOutsideClick: false,
        allowEscapeKey: false,
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/App/ogrSifrePush/',
                type: 'Post',
                data: { ogrID: id },
                dataType: 'json',
                success: function (data) {
                    swalWithBootstrapButtons.fire(
                       {
                        title: data.msgTitle,
                        html: data.msg,
                        icon: data.msgIcon,
                        confirmButtonText: 'Kapat',
                       }
                    )
                    $("#ogrRow-"+ogrID).remove();
                }
            });

        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            Swal.fire({
                title: '<strong>İşlem İptal Edildi</strong>',
                icon: 'info',
                html:
                    '<b>Herhangi Bir Değişiklik Yapılmadı</b>',

                confirmButtonText:
                    'Kapat'
            });
        }
    });
}
function customModalClose() {
    $("#mdlOgrDetay").modal('hide');
}
function customseansOgrOnKayitModalClose() {
    $("#mdlSeansOgrOnKayitList").modal('hide');
}
function gunlukSeansOgrList(id) {


    $.ajax({
        url: '/App/anlikDenemeKontrol/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            var ogrAnlikListJSON = jQuery.parseJSON(data);
            let ogrModalSet =
                `
                    <div class="modal fade modalbox" id="mdlOgrList" data-backdrop="static" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title"></h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body">
<div class="row" style="margin-top:2rem;">
<div class="col-md-12">
<button class="btn btn-success" onclick="seansOgrListPrint();">Yazdır</button>
</div>
</div>
                                    <div class="body" style="margin-top:0.2rem; font-weight:bold !important;">
<center>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <center><img src="/assets/uyaniklogo.png" /></center>
        </div>
    </div>
    <hr />
    <div class="row">
        <div class="col-12" style="font-weight: bold !important;">
            <h3>Seansa Kayıtlı Öğrenci Listesi</h3>
            <table class="table table-bordered" style="font-weight:bold; color:black;">
                <thead>
                    <tr>
                        <th scope="col"><h3>Ad</h3></th>
                        <th scope="col"><h3>Soyad</h3></th>
                        <th scope="col"><h3>Deneme Seansı / Saat</h3></th>
                        <th scope="col"><h3>Öğrenci Durum</h3></th>
                    </tr>
                </thead>`
            $.each(ogrAnlikListJSON, (index, value) => {
                ogrModalSet += `<tr>
                                                                                <td width="25%"><h3>${value.Ad}</h3></td>
                                                                                <td width="25%"><h3>${value.Soyad}</h3></td>
                                                                                <td><h3>${value.denemeAd} / Saat: ${value.saat}</h3></td>
                                                                                <td><h3></h3></td>

                                                                        </tr>`;
            });
            ogrModalSet += `<tfoot>
                    <tr>
                        <th scope="col"><h3>Ad</h3></th>
                        <th scope="col"><h3>Soyad</h3></th>
                        <th scope="col"><h3>Deneme Seansı / Saat</h3></th>
                        <th scope="col"><h3>Öğrenci Durum</h3></th>
                   </tr>
                </tfoot>
            </table>
        </div>
    </div>
</div>
</center>
</div>
                                </div>
                            </div>
                        </div>
                    </div>`;

            $("#modalArea").html(ogrModalSet);
            $("#mdlOgrList").modal('show');



        }
    });


}
function seansOgrList(id,istek) {


    $.ajax({
        url: '/App/ogrPrintList/',
        contentType: 'application/json; charset=utf-8;',
        data: { id: id, istek: istek },
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            var ogrObjData = jQuery.parseJSON(data.ogrListJSON);
            var seansObjData = jQuery.parseJSON(data.seansSetJSON);
            let ogrModalSet =
                `
                    <div class="modal fade modalbox" id="mdlOgrList" data-backdrop="static" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title"></h5>
                                    <a href="javascript:;" data-dismiss="modal">Kapat</a>
                                </div>
                                <div class="modal-body">
<div class="row" style="margin-top:2rem;">
<div class="col-md-12">
        <button class="btn btn-success" onclick="seansOgrListPrint();">Yazdır</button>
</div>
</div>
                                    <div class="body" style="margin-top:0.2rem; font-weight:bold !important;">
<center>
<div class="container-fluid">
        <div class="col-12" style="font-weight: bold !important;">
            <table class="table table-bordered table-sm" style="font-weight:bold; color:black;">
                <thead>
               <tr>
                        <th>YAYIN: ${seansObjData.yayinBaslik} SEANS ADI: ${seansObjData.Deneme.DenemeBaslik} <hr /> KATEGORİ: ${seansObjData.yayinKategoriBaslik} <hr /> KİŞİ BAŞI ÜCRET: ${seansObjData.SeansUcret} TL</th>
                        <th><center><img src="/assets/uyaniklogo.png" /></center><center><h3>Seansa Kayıtlı Öğrenci Listesi</h3></center></th>
<th>TARİH: ${seansObjData.TarihSTR} (${seansObjData.SeansGun}) <hr /> SAAT: ${seansObjData.Saat}<hr />KONTENJAN: ${seansObjData.Kontenjan} KİŞİ / KATILIM: ${seansObjData.KayitliOgrenci} KİŞİ</th> 
                    </tr>

                    <tr>
                        <th scope="col"><h3>Ad</h3></th>
                        <th scope="col"><h3>Soyad</h3></th>
                        <th scope="col"><h3>Öğrenci Durum</h3></th>
                    </tr>
                </thead><tbody>`;
            let durumText = ``;

            $.each(seansObjData.kayitliOgrList, (index, value) => {
                if (value.Durum == 1) {
                    durumText = `Yoklama Bekleniyor`;
                } else if (value.Durum == 2) {
                    durumText = `Katılım Sağlandı`;
                } else if (value.Durum == 3) {
                    durumText = `Katılmadı`;
                }
                ogrModalSet += `<tr>
                                            <td width="35%"><h3>${value.Ad}</h3></td>
                                            <td width="35%"><h3>${value.Soyad}</h3></td>
                                            <td><h3>${durumText}</h3></td>

                                        </tr>`;
            });
            ogrModalSet += `</tbody><tfoot>
                    <tr>
                        <th scope="col"><h3>Ad</h3></th>
                        <th scope="col"><h3>Soyad</h3></th>
                        <th scope="col"><h3>Öğrenci Durum</h3></th>
                   </tr>
                </tfoot>
            </table>
        </div>
    </div>
</div>
</center>
</div>
                                </div>
                            </div>
                        </div>
                    </div>`;

            $("#modalArea").html(ogrModalSet);
            $("#mdlOgrList").modal('show');


        }
    });

}
function raporList() {
    let raporListModal = ``;
}
let toplamDeneme = [];
function myFunction(id) {
    var diziElelman = id.toString().replace("sn-", "");
    var diziSearch = toplamDeneme.indexOf(diziElelman);

    if (diziSearch == -1) {
        toplamDeneme.push(diziElelman);
    } else {
        toplamDeneme.pop(diziSearch, 1);
    }

    
}
function seansOgrListPrint() {
    var mode = 'iframe'; // popup
    var close = mode == "popup";
    var options = { mode: mode, popClose: close };
    $("div.body").printArea(options);
}

function cokluDenemeKayit(id) {
    $.ajax({
        url: '/App/ogrShortSeansList/',
        contentType: 'application/json; charset=utf-8;',
        type: 'Get',
        data: { ogrID: id },
        dataType: 'json',
        success: function (data) {
            toplamDeneme.length = 0;
            toplamDeneme.push(id.toString());
            var snStList = jQuery.parseJSON(data.seansShort);
            let seansdRp = `
                    <input id="txtseansAra" type="text" class="form-control" placeholder="Seans Filtrele" onkeyup="ara2()">

        <div class="section full mb-2">
            <div class="section-title">Lütfen Seans Seçimi Yapınız</div>
            <div class="wide-block p-0">

                <div class="input-list">`;
             
            $.each(snStList, (index, value) => {
                seansdRp += `<div id="myDiv" class="custom-control custom-checkbox seansRow">
                    <input id="sn-${value.Id}" onclick="myFunction(this.id)" type="checkbox" class="custom-control-input">
                        <label style="font-size: 15.4px; display: inherit;" class="custom-control-label" for="sn-${value.Id}">${value.yayinBaslik} - ${value.yayinKategoriBaslik} - ${value.kitapcikBaslik} <br>${value.TarihSTR} - ${value.SeansGun} / Saat: ${value.Saat} <br> Kontenjan: ${value.GuncelKontenjan} Ücret: ${value.SeansUcret} TL</label>
                    </div>`;
            });
                

                seansdRp += `</div>

            </div>
        </div>

            `;

            Swal.fire({
                title: '<strong>Seçilen Öğrencinin <u>Kayıt Olabileceği</u> Deneme Seansları</strong>',
                html: seansdRp,
                icon: 'info',
                showDenyButton: true,
                confirmButtonText: 'Kaydet',
                denyButtonText: `Vazgeç`,
            }).then((result) => {
                if (result.isConfirmed) {
                    var result = toplamDeneme.map(function (x) {
                        return parseInt(x, toplamDeneme.length);
                    });
                    var data = JSON.stringify(toplamDeneme);

                    $.ajax({
                        url: '/App/ogrSeansKayitMulti/',
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        type: 'POST',
                        data: data,
                        success: function (result) {
                            let customWpMsg = ``;
                            let customWpPhone = ``;
                            let msgHtml = `<strong>Öğrencinin Aşağıdaki Seanslara Kaydı Alınmıştır</strong>
<hr />`;
                            var seanslist = jQuery.parseJSON(result.denemeler);

                            $.each(seanslist, (index, value) => {
                                msgHtml += `<strong>Kategori: ${value.kategoriBaslik} - Yayın: ${value.yayinAd} - Seans: ${value.denemeAd} Tarih / Saat: ${value.tarihSaat} Ücret: ${value.kayitUcret} TL</strong><hr />`;
                            });
      
                            msgHtml += `<strong>Toplam Tutar: ${result.toplamTutar}</strong>`;
                            customWpMsg += result.wpCustomMsg + "%0A";
                            $.each(seanslist, (index, value) => {
                                customWpMsg += `Yayın: ${value.yayinAd} - Kategori: ${value.kategoriBaslik} - Seans: ${value.denemeAd} Tarih / Saat: ${value.tarihSaat} %0A`;
                            });
                            
                            let customWp = `<hr /><a class="btn btn-success" href="whatsapp://send?text=`;
                            customWp += customWpMsg;
                            customWp += `&phone=` + result.ogrTelefon + `"`;
                            customWp += `>Whatsapp Üzerinden İlet</a>`;
                            msgHtml += customWp;
                            Swal.fire({
                                title: result.msgTitle,
                                html: msgHtml,
                                icon: 'success',
                                showDenyButton: true,
                                denyButtonText: `Kapat`,
                            });
                          
                        }
 
                    });
                    
                } else if (result.isDenied) {
                    Swal.fire('İşlem iptal Edildi', 'Her Hangi Bir İşlem Yapılmadı', 'info')
                }
            })

        }

    });
   
  
}


function seansOgrListPrint() {
    var mode = 'iframe'; // popup
    var close = mode == "popup";
    var options = { mode: mode, popClose: close };
    $("div.body").printArea(options);
}

