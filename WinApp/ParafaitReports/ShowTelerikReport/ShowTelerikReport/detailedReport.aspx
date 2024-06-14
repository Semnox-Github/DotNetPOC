<%@ Page Language="C#" AutoEventWireup="true" CodeFile="detailedReport.aspx.cs" Inherits="detailedReport" %>
<%@ Register assembly="Telerik.ReportViewer.WebForms, Version=14.1.20.513, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" namespace="Telerik.ReportViewer.WebForms" tagprefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="float:right; width=100%;">
            <asp:Button runat="server" ID="btnClose" OnClick="btnClose_click" Text="X" BackColor="MidnightBlue" ForeColor="White" ToolTip="Close" Width="40px" Height="40px" />
        </div>
        <br /><br />
        <div id="content">
            
            <telerik:ReportViewer ID="rptViewer" runat="server" Skin="Orange" SkinsPath="Skins" Resources-SessionHasExpiredMessage="Session Expired due to inactivity. <a href='../login.aspx'>Click Here</a> to login."
                Width="100%" Height="100%" />
        </div>
    </form>
</body>
</html>
