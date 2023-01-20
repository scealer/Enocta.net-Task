/*Modal ekranını açar 1*/
$(document).keydown(function (e) {
    if (e.keyCode == 112) {
        e.preventDefault();
        $('#myModal').modal('show');
    }
    else if (e.keyCode == 113) {
        e.preventDefault();
        $('#myModal').modal('hide');
    }
});

/*Bildirim yapılacak text bu fonksiyona gelir.*/
function Bildirim(text) {
    $.notify({
        icon: 'fa fa-info',
        message: text
    },
        {
            type: 'info',
            timer: 5000
        });
}
/*Hata Bildirimi yapar.*/
function HataBildirim() {
    $.notify({
        icon: 'fa fa-warning',
        message: 'Bilgilerinizi kontrol ediniz!'
    },
        {
            type: 'danger',
            timer: 5000
        });
}

//Image Chosen Dropdown
$(".my-select").chosen({ width: "32px;", height: "32px;" });
$(".my-select2").chosen();



//Modal Ekranı
$('#myModal').on('show.bs.modal', function (e) {
    console.debug('modal shown!');
    $('.my-select', this).chosen({ width: "700px" });
    $('.my-select2', this).chosen({ width: "700px" });
});
$('#myModal').on('hidden.bs.modal', function () {
    $(this).find('#frmSiparis')[0].reset();
    $(this).find('#tabloveri').children("tr:has('td')").remove();
});



/*Tarih Formatını ayarlar*/
function jsDate(x) {
    var jsonDateRE = /^\/Date\((-?\d+)(\+|-)?(\d+)?\)\/$/;
    var arr = jsonDateRE.exec(x);
    if (arr) {
        // 0 - complete results; 1 - ticks; 2 - sign; 3 - minutes
        return new Date(parseInt(arr[1]));
    }
    return x;
}

function formatDate(formattedDate) {
    try {
        formattedDate = new Date(formattedDate);
        var d = formattedDate.getDate();
        var m = formattedDate.getMonth();
        m = 1;  // JavaScript months are 0-11
        m = ("00" + m).substr(("00" + m).length - 2);
        d = ("00" + d).substr(("00" + d).length - 2);
        var y = formattedDate.getFullYear();
        return (d + "-" + m + "-" + y);
    } catch (e) {
        return formattedDate;
    }

}

function formatDateTime(formattedDate) {
    try {
        var d = formattedDate.getDate();
        var m = formattedDate.getMonth();
        var h = formattedDate.getHours();
        var mm = formattedDate.getMinutes();
        m = 1;  // JavaScript months are 0-11
        m = ("00" + m).substr(("00" + m).length - 2);
        d = ("00" + d).substr(("00" + d).length - 2);
        h = ("00" + h).substr(("00" + h).length - 2);
        mm = ("00" + mm).substr(("00" + mm).length - 2);
        var y = formattedDate.getFullYear();
        return (d + "-" + m + "-" + y + " " + h + ":" + mm);
    } catch (e) {
        return formattedDate;
    }

}

function formatTime(formattedDate) {
    try {
        var h = formattedDate.getHours();
        var mm = formattedDate.getMinutes();
        h = ("00" + h).substr(("00" + h).length - 2);
        mm = ("00" + mm).substr(("00" + mm).length - 2);
        return (h + ":" + mm);
    } catch (e) {
        return formattedDate;
    }

}

function inputFormatDate(formattedDate) {
    try {
        formattedDate = new Date(formattedDate);
        var d = formattedDate.getDate();
        var m = formattedDate.getMonth();
        m = 1;  // JavaScript months are 0-11
        m = ("00" + m).substr(("00" + m).length - 2);
        d = ("00" + d).substr(("00" + d).length - 2);
        var y = formattedDate.getFullYear();
        return (y + "-" + m + "-" + d);
    } catch (e) {
        return formattedDate;
    }

}

/*Textbox'ın sadece int değer almasını sağlar.*/
function IsNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

// Sayfalama yapılıyor. 'simplePagination' jQuery Plugin
// Consider adding an ID to your table
// incase a second table ever enters the picture.
var items = $("table tbody tr");

var numItems = items.length;
var perPage = 15;

// Only show the first 2 (or first `per_page`) items initially.
items.slice(perPage).hide();

// Now setup the pagination using the `.pagination-page` div.
$(".pagination-page").pagination({
    items: numItems,
    itemsOnPage: perPage,
    cssStyle: "light-theme",

    // This is the actual page changing functionality.
    onPageClick: function (pageNumber) {
        // We need to show and hide `tr`s appropriately.
        var showFrom = perPage * (pageNumber - 1);
        var showTo = showFrom + perPage;

        // We'll first hide everything...
        items.hide()
            // ... and then only show the appropriate rows.
            .slice(showFrom, showTo).show();
    }
});


// We'll create a function to check the URL fragment
// and trigger a change of page accordingly.
function checkFragment() {
    // If there's no hash, treat it like page 1.
    var hash = window.location.hash || "#page-1";

    // We'll use a regular expression to check the hash string.
    hash = hash.match(/^#page-(\d+)$/);

    if (hash) {
        // The `selectPage` function is described in the documentation.
        // We've captured the page number in a regex group: `(\d+)`.
        $(".pagination-page").pagination("selectPage", parseInt(hash[1]));
    }
};

// We'll call this function whenever back/forward is pressed...
$(window).bind("popstate", checkFragment);

// ... and we'll also call it when the page has loaded
// (which is right now).
checkFragment();


//MailGönder
//function GoMail() {
//    $.post("Mail/NowSendMail",
//        function (data, status) {
//            if (data.success == true) {
//                Bildirim(data.message);
//            }
//        });

//}

$("#divTadilat").hide();
$("#btnSiparisKaydet").hide();
$('#rdbDefault').prop('checked', true);

function TekliGoster() {

    $("#txtPara").show();
    $("#drpMIslemListeID2").show();
    $("#drpMIslemListeID1").show();
    $("#btnSiparisKaydet2").show();
    $("#btnSiparisKaydet").hide();
    $("#divTadilat").hide();
}
function CokluGoster() {

    $("#txtPara").hide();
    $("#drpMIslemListeID1").hide();
    $("#drpMIslemListeID2").hide();
    $("#btnSiparisKaydet2").hide();
    $("#btnSiparisKaydet").show();
    $("#divTadilat").show();
}