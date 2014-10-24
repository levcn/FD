var dbName = "";
$(function () {
    $("[name='ServerName1']").change(function (e) {
        dbName = $(this).children('option:selected').val();
        if (dbName != null && dbName != "") {
            $("#db2").show();
        } else {
            $("#db2").hide();
        }
    });
});

//开始创建数据库表和内容
function toCreateTables() {
    var uiData = getUIData();
    uiData.Action = "Create";
    uiData.DBName = dbName;
    $.post("api.aspx", uiData, function (data) {
        var o = $.parseJSON(data);
        if (o.Success == 0) {
            alert(o.ErrorMessage);
            return;
        }
        alert(data);
    });
}

function getUIData() {
    var object = {
        Action: "GetDBs",
        ServerName: $("[name='ServerName']").val(),
        ServerUID: $("[name='ServerUID']").val(),
        ServerPWD: $("[name='ServerPWD']").val(),
    }
    return object;
}