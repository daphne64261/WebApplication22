/*============== 放大鏡處理 ==============*/
function trigger_setup(popup_id) {
    $("#" + popup_id + " table tbody tr").bind("click", { pid: popup_id }, fill_out);
}
function fill_out(event) {
    //測試: 點選視窗下一頁不會到這段, 非造成視窗關閉原因
    var m_value = $(this).find("td").eq(0).text().trim();
    var m_text = $(this).find("td").eq(1).text().trim();
    var popup_id = event.data.pid;
    var v_id = $("#" + popup_id).data("to-value-id");
    var t_id = $("#" + popup_id).data("to-text-id");
    //$("#" + v_id).val(m_value);
    //$("#" + t_id).val(m_text);
    //20200408 改寫, 
    // 因val 無法觸發onchange事件, 故改寫如下, 測試OK!
    // 調整change()再最後一個val下觸發, 不然第一個val直接跑到對應change事件, 不會執行下一個val
    $("#" + v_id).val(m_value);
    $("#" + t_id).val(m_text).change();

    $("#" + popup_id).dialog("close");
}
/*========================================*/

function show_please_wait(caller) {
    var info_id = caller.id + "_info";
    var m_popup = "<div id='" + info_id + "'>耐心再等一下...</div>";
    $(caller).after(m_popup);
    $("#" + info_id).dialog({
        title: "請稍候",
        position: { my: "left top", at: "left bottom", of: $(caller) },
        modal: true,
        classes: {
            "ui-dialog": "custom-red"
        },
        closeOnEscape: false,
        close: function () {
            $(this).remove();
        },
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
        }
    });
}

$(document).ready(function () {
    $("#main").mouseenter(function () {
        $("#My_Main_Menu").slideUp();
    });
    $("#transart_logo").mouseenter(function () {
        $("#My_Main_Menu").slideDown();
    });
    /*============== F1按鍵處理 ==============*/
    $(document).keydown(function (event) {
        if (event.key === "F1") {
            event.preventDefault();
            var x = $("#pdf_id").val();
            $.get("/Home/Pdf_Url", { pdf_id: x }, function (data) {
                window.open(data);
            });
        }
    });
    window.onhelp = function () { return false; };
    /*========================================*/
    /*============== 日期選單處理 ==============*/
    $(".datepicker").datepicker(
        {
            autoSize: false,
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            dateFormat: "yy/mm/dd"
        }
    ).datepicker("option", $.datepicker.regional['zh-TW']);
    /*========================================*/
    /*========== 網頁中上方的功能鍵處理 ==========*/
    $("button:disabled").removeClass("btn-info").addClass("btn-btn-primary disabled");

    //$("#spl_save").addClass("please_wait");
    //$("#spl_del").addClass("please_wait");

    $(".please_wait").one("click", function () {
        show_please_wait(this);
    });

    //$("#spl_save").click(function () {
    //    $("form").submit();
    //});
    //$("#spl_cancel").click(function () {
    //    $("form").trigger("reset");
    //});

    /*========================================*/
    /*============== 放大鏡處理 ==============*/
    $(".fa-search.Find").click(function () {
        var v_id = $(this).data("to-value-id");
        var t_id = $(this).data("to-text-id");
        var f_kind = $(this).data("from");
        var popup_id = t_id + "_popup";
        var query_string = "PopupID=" + popup_id + "&FieldKind=" + f_kind;        
        var m_popup = "<div id='" + popup_id + "'></div>";
        $(this).after(m_popup);
        $("#" + popup_id).attr("data-to-value-id", v_id);
        $("#" + popup_id).attr("data-to-text-id", t_id);
        $("#" + popup_id).load("/Popup/Show", query_string, function (response, status, xhr) {
            if (status === "success") {
                trigger_setup(popup_id);
            }
            else {
                alert("Error: " + xhr.status + ": " + xhr.statusText);
            }
        });

        $("#" + popup_id).dialog({
            title: "請選擇",
            position: { my: "left top", at: "left bottom", of: $(this) },
            maxWindth: 1024,
            minWidth: 600,
            modal: true,
            close: function () {
                $(this).remove();
            }
        });
    });
    $(".fa-search.Find_Emp_of_Dept").click(function () {
        var v_id = $(this).data("to-value-id");
        var t_id = $(this).data("to-text-id");
        var dpno_id = $(this).data("dpno-id");
        var dpno = $("#" + dpno_id).val();
        if (typeof dpno === "undefined") {
            dpno = "";
        }
        var popup_id = t_id + "_popup";
        var query_string = "PopupID=" + popup_id + "&DPNO=" + dpno;
        //alert("1st query_string===" + query_string);//MS_NAME_popup
        var m_popup = "<div id='" + popup_id + "'></div>";
        $(this).after(m_popup);
        $("#" + popup_id).attr("data-to-value-id", v_id);
        $("#" + popup_id).attr("data-to-text-id", t_id);

        $("#" + popup_id).load("/Popup/EmpOfDept", query_string, function (response, status, xhr) {
            if (status === "success") {
                trigger_setup(popup_id);
            }
            else {
                alert("Error: " + xhr.status + ": " + xhr.statusText);
            }
        });

        $("#" + popup_id).dialog({
            dialogClass: '.fa-search.Find_Emp_of_Dept',  
            title: "請選擇",
            //position: { my: "left top", at: "left bottom", of: $(this) },
            position: { my: "center", at: "center", of: $(this) },
            maxWindth: 1024,
            minWidth: 600,
            modal: true,
            close: function () {
                $(this).remove();
            }
        });
    });
    /*========================================*/
});