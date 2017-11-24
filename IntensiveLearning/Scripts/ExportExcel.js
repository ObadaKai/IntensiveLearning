$(document).ready(function () {

    $('[data-toggle="tooltip"]').tooltip();
    $("#btnExport").click(function (e) {
        e.preventDefault();

        //getting data from our table
        var data_type = 'data:application/vnd.ms-excel';
        var table_div = document.getElementById('table_wrapper');
        var table_html = table_div.outerHTML.replace(/ /g, '%20');

        var a = document.createElement('a');
        a.href = data_type + ', ' + table_html;
        a.download = 'exported_table_' + Math.floor((Math.random() * 9999999) + 1000000) + '.xls';
        a.click();
    });

    $("#btnExport2").click(function (e) {
        var data_type = 'data:application/vnd.ms-excel';
        var table_div = document.getElementById('table_wrapper2');
        var table_html = table_div.outerHTML.replace(/ /g, '%20');


        var a = document.createElement('a');
        a.href = data_type + ', ' + table_html;
        a.download = 'exported_table_' + Math.floor((Math.random() * 9999999) + 1000000) + '.xls';
        a.click();

    });

    $("#ToFadeMessage").hide(3000);

    if ($('#desktopTest').is(':hidden')) {
        toggleLeftMenu();
        document.getElementById("TemplateForm").style.marginRight = document.getElementById("leftMenu").clientWidth + 28 + "px";
        document.getElementById("TemplateForm").style.marginLeft = "auto";
    } else {
        document.getElementById("TemplateForm").style.marginRight = "auto";
        document.getElementById("TemplateForm").style.marginLeft = "auto";


    }
    $('#DependedOn').val("")

    if ($('#CenterType').val() == "فرعي") {
        $('#CenterDependency').css('display', 'block');
        $('#CenterCity').css('display', 'none');
        $('#Cityid').val("")
    }
    else {
        $('#CenterDependency').css('display', 'none');
        $('#CenterCity').css('display', 'block');
        $('#DependedOn').val("")
    }



    if ($('#Job').find(":selected").data("value") == 1) {
        $("#EmpCity").css('display', 'block');
        $("#EmpCenter").css('display', 'none');
    }
    else if ($('#Job').find(":selected").data("value") == 2) {
        $("#EmpCity").css('display', 'none');
        $("#EmpCenter").css('display', 'block');
    }
    else {
        $("#EmpCity").css('display', 'none');
        $("#EmpCenter").css('display', 'none');
    }




    if ($('#SmallJob').find(":selected").data("value") == 1) {
        $("#SmallEmpCity").css('display', 'block');
        $("#SmallEmpCenter").css('display', 'none');
    }
    else if ($('Small#Job').find(":selected").data("value") == 2) {
        $("#SmallEmpCity").css('display', 'none');
        $("#SmallEmpCenter").css('display', 'block');
    }
    else {
        $("#SmallEmpCity").css('display', 'none');
        $("#SmallEmpCenter").css('display', 'none');
    }


    if ($("#OrderManyTypes option:selected").text() == "اخرى") {
        $("#OrderType").show();
        $("#OrderType").val("");
    }
    else {
        $("#OrderType").hide();
        $("#OrderType").val("مشتريات");
    }

});
var open = false;
function openLeftMenu() {
    document.getElementById("leftMenu").style.display = "block";
    document.getElementById("leftMenu").style.height = "100%";
    if ($('#desktopTest').is(':hidden')) {
        document.getElementById("TemplateForm").style.marginRight = document.getElementById("leftMenu").clientWidth + 2 + "px";
        document.getElementById("TemplateForm").style.marginLeft = "auto";

    }
    else {
        document.getElementById("TemplateForm").style.marginRight = "auto";
        document.getElementById("TemplateForm").style.marginLeft = "auto";

    }
    //document.getElementById("TemplateForm").classList = "container body-content";
    //document.getElementById("TemplateForm").classList = "container body-content col-md-11 col-lg-11 col-sm-8 col-xs-8";
    //document.getElementById("leftMenu").classList = "col-md-1 col-lg-1 col-sm-4 col-xs-4";
    open = true;
}


function closeLeftMenu() {
    document.getElementById("leftMenu").style.height = "0%";
    if ($('#desktopTest').is(':hidden')) {
        document.getElementById("TemplateForm").style.marginRight = document.getElementById("leftMenu").clientWidth / 2 + 1 + "px";
        document.getElementById("TemplateForm").style.marginLeft = document.getElementById("leftMenu").clientWidth / 2 + 1 + "px";

    } else {
        document.getElementById("TemplateForm").style.marginRight = "auto";
        document.getElementById("TemplateForm").style.marginLeft = "auto";
    }
    //document.getElementById("TemplateForm").classList = "container body-content";
    open = false;
}



function toggleLeftMenu() {
    open ? closeLeftMenu() : openLeftMenu();
}

$("#CenterType").on("change", function () {
    if (this.value == "فرعي") {
        $('#CenterDependency').css('display', 'block');
        $('#CenterCity').css('display', 'none');
        $('#Cityid').val("")
    }
    else {
        $('#CenterDependency').css('display', 'none');
        $('#CenterCity').css('display', 'block');
        $('#DependedOn').val("")
    }
});


$("#Job").on("change", function () {
    if ($(this).find(":selected").data("value") == 1) {
        $("#EmpCity").css('display', 'block');
        $("#EmpCenter").css('display', 'none');
    }
    else if ($(this).find(":selected").data("value") == 2) {
        $("#EmpCity").css('display', 'none');
        $("#EmpCenter").css('display', 'block');
    }
    else {
        $("#EmpCity").css('display', 'none');
        $("#EmpCenter").css('display', 'none');
    }


});


$("#SmallJob").on("change", function () {
    if ($(this).find(":selected").data("value") == 1) {
        $("#SmallEmpCity").css('display', 'block');
        $("#SmallEmpCenter").css('display', 'none');
    }
    else if ($(this).find(":selected").data("value") == 2) {
        $("#SmallEmpCity").css('display', 'none');
        $("#SmallEmpCenter").css('display', 'block');
    }
    else {
        $("#SmallEmpCity").css('display', 'none');
        $("#SmallEmpCenter").css('display', 'none');
    }


});

$("#OrderManyTypes").on("change", function () {
    if ($("#OrderManyTypes option:selected" ).text() == "اخرى") {
        $("#OrderType").show();
        $("#OrderType").val("");

    }
    else {
        $("#OrderType").hide();
        $("#OrderType").val("مشتريات");

    }

});



var toggleimport = false;
$('#btnImport').on("click", function () {
    toggleimport ? toggleimport = false : toggleimport = true;
    toggleimport ? $('#ImportExcel').css('display', 'block') : $('#ImportExcel').css('display', 'none');

});


function GoToDefault() {
    window.location.href = "/Home/Default";
}




var uploadField = document.getElementById("file");

uploadField.onchange = function () {
    var size = 0;
    for (var i = 0; i < this.files.length; i++) {
        size += this.files[i].size;
    }
    if (size > 10485760) {
        alert("حجم الملفات كبير");
        this.value = "";
    };
};
var uploadFieldHuge = document.getElementById("fileHuge");

uploadFieldHuge.onchange = function () {
    var size = 0;
    for (var i = 0; i < this.files.length; i++) {
        size += this.files[i].size;
    }
    if (size > 20971520) {
        alert("حجم الملفات كبير");
        this.value = "";
    };
};


function PaymentsOpen(){
    $('#ChooseOldPayment').hide();
    $('#PaymentSubmitBtn').show();
    $('#PaymentData').show();
    
};


function PaymentRefuse() {
    $('#PaymentRefuse').hide();
    $('#PaymentCancelation').show();
}

function BuyingRefuse() {
    $('#BuyingRefuse').hide();
    $('#BuyingCancelation').show();
}

function ProofRefuse() {
    $('#ProofRefuse').hide();
    $('#ProofCancelation').show();
}

function RefuseOrderFirstLevel() {
    $('#RefuseOrderFirstLevel').hide();
    $('#RefuseOrderFirstLevelCancelation').show();
}

function RefuseOrderSecondLevel() {
    $('#RefuseOrderSecondLevel').hide();
    $('#RefuseOrderSecondLevelCancelation').show();
}

function RefuseOrderThirdLevel() {
    $('#RefuseOrderThirdLevel').hide();
    $('#RefuseOrderThirdLevelCancelation').show();
}