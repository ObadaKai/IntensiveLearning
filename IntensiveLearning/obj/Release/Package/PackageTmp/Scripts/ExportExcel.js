$(document).ready(function () {
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
});
var open = true;
function openLeftMenu() {
    document.getElementById("leftMenu").style.height = "100%";
    //document.getElementById("TemplateForm").classList = "container body-content";
    //document.getElementById("TemplateForm").classList = "container body-content col-md-11 col-lg-11 col-sm-8 col-xs-8";
    //document.getElementById("leftMenu").classList = "col-md-1 col-lg-1 col-sm-4 col-xs-4";
    open = true;
}
function closeLeftMenu() {
    document.getElementById("leftMenu").style.height = "0%";
    //document.getElementById("TemplateForm").classList = "container body-content";
    open = false;
    }



function toggleLeftMenu() {
    open ? closeLeftMenu() : openLeftMenu();
}

function GoToDefault() {
    window.location.href = "/Home/Default";
}