<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="step1.aspx.cs" Inherits="FD.Core.Test.Web.install.step1" %>

<%@ Register Src="Include.ascx" TagPrefix="uc1" TagName="Include" %>
<%@ Register Src="~/install/Navigator.ascx" TagPrefix="uc1" TagName="Navigator" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <uc1:Include runat="server" ID="Include" />
    <script src="step1.js"></script>
    <script>
        function createDBPanel(n) {
            if (n == 1) {
                $("#s1").hide();
                $("#s2").show();
            }
            if (n == 0) {
                $("#s2").hide();
                $("#s1").show();
            }
        }

        

        //链接数据库并返回数据库中的表
        function toConnection() {
//            var object = {
//                Action: "GetDBs",
//                ServerName: $("[name='ServerName']").val(),
//                ServerUID: $("[name='ServerUID']").val(),
//                ServerPWD: $("[name='ServerPWD']").val(),
//            };
            $.post("api.aspx", getUIData(), function (data) {
                var o = $.parseJSON(data);
                if (o.Success == 0) {
                    alert(o.ErrorMessage);
                    return;
                }

                var o1 = $.parseJSON(o.Data);
                $("[name='ServerName1']").empty();
                $.each(o1, function (index, item) {
                    $("[name='ServerName1']").append("<option value=" + item + ">" + item + "</option>");
                });
                $("#db1").show();
                
            });
        }
    </script>
</head>
<body>
    <div>
        <uc1:Navigator CurrentStep="1" runat="server" ID="Navigator" />
    </div>
    <form id="form1" runat="server">
        <table width="300" align="center">
            <tr>
                <td align="center" colspan="2">数据库服务器设置</td>
            </tr>
            <tr>
                <td>服务器:</td>
                <td>
                    <input type="text" name="ServerName" value="192.168.32.2" /></td>
            </tr>
            <tr>
                <td>登陆名:</td>
                <td>
                    <input type="text" name="ServerUID" value="sa" /></td>
            </tr>
            <tr>
                <td>密码:</td>
                <td>
                    <input type="text" name="ServerPWD" value="levcn_2008" /></td>
            </tr>
            <tr>
                <td colspan="2"><a href="javascript:toConnection()">连接数据库</a></td>
            </tr>
            <tr id="db1" style="display: none;">
                <td>数据库:</td>
                <td>
                    <span id="s1" style="display: none;">
                        <input type="text" name="ServerName" /><a href="javascript:createDBPanel(1)">选择</a>
                    </span>
                    <span id="s2">
                        <select name="ServerName1">
                        </select>
                        <a style="display: none;" href="javascript:createDBPanel(0)">新建</a>
                    </span>
                </td>
            </tr>
            <tr id="db2" style="display: none;">
                <td>
                    <a href="javascript:toCreateTables()">确定</a>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
