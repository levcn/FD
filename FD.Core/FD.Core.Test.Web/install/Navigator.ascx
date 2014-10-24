<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Navigator.ascx.cs" Inherits="FD.Core.Test.Web.install.Navigator" %>
<style>
    .current {
        color: red;
    }
</style>
<div>
    <div class="<% =CurrentStep==1?"current":"" %>" >第一步</div>
    <div class="<% =CurrentStep==2?"current":"" %>" >第二步</div>
    <div class="<% =CurrentStep==3?"current":"" %>" >第三步</div>
    <div class="<% =CurrentStep==4?"current":"" %>" >第四步</div>
    <div class="<% =CurrentStep==5?"current":"" %>" >第五步</div>
</div>