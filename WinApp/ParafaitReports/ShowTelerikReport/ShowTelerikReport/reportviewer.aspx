<%@ Page Language="C#" AutoEventWireup="true" CodeFile="reportviewer.aspx.cs" Inherits="reportviewer" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register assembly="Telerik.ReportViewer.WebForms, Version=14.1.20.513, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" namespace="Telerik.ReportViewer.WebForms" tagprefix="telerik" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<meta http-equiv="X-UA-Compatible" content="IE=edge" />

<html xmlns="http://www.w3.org/1999/xhtml" id="html">
<head runat="server">
    <title>Parafait Reports</title>
    <%--<link rel="shortcut icon" href="~/Images/animated_favicon.gif" />--%>
    <style type="text/css">   
      html#html, body#body, form#form1, div#content, center#center1
      { 
        height: 100%;
      }
    </style>
</head>
<body id="body">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div id="dateSelection">
            <table width=100% style="vertical-align:middle; display:table-cell; height:49px;">
                <tr align="center" style="width:100%;">
                    <td>
                        <asp:Label ID="lblFromdate" runat="server" Text="From Date:" CssClass="lblHomePage" Font-Size="14pt" ForeColor="Black"></asp:Label>
                    </td>
                    <td>
                        <telerik:RadDateTimePicker runat="server" ID="radFromDate" Culture="en-GB" Width="290px" Height="40px"
                            AutoPostBackControl="Both" 
                            onselecteddatechanged="radFromDate_SelectedDateChanged" Skin="Office2007" Font-Overline="False">
                            <TimeView CellSpacing="-1" Culture="en-GB" TimeFormat="hh:mm tt"></TimeView>
                            <TimePopupButton ImageUrl="Images/clock.png" HoverImageUrl="Images/clock-hover.png"></TimePopupButton>
                            <Calendar ID="Calendar2" runat="server" EnableKeyboardNavigation="true"  
                                  Skin="Office2007"></Calendar>
                            <DateInput DisplayDateFormat="dd-MMM-yyyy hh:mm tt" DateFormat="dd-MMM-yyyy hh:mm tt" Font-Size="12pt" LabelWidth="40%" AutoPostBack="True">
                                <EmptyMessageStyle Resize="None"></EmptyMessageStyle>
                                <ReadOnlyStyle Resize="None"></ReadOnlyStyle>
                                <FocusedStyle Resize="None"></FocusedStyle>
                                <DisabledStyle Resize="None"></DisabledStyle>
                                <InvalidStyle Resize="None"></InvalidStyle>
                                <HoveredStyle Resize="None"></HoveredStyle>
                                <EnabledStyle Resize="None"></EnabledStyle>
                            </DateInput>
                            <DatePopupButton ImageUrl="Images/calendar.png" HoverImageUrl="Images/calendar-hover.png"></DatePopupButton>
                        </telerik:RadDateTimePicker>
                    </td>
                    <td>
                        <asp:Label ID="lblTodate" runat="server" Text="To Date:" CssClass="lblHomePage"  Font-Size="14pt" ForeColor="Black"></asp:Label>
                    </td>
                    <td>
                        <telerik:RadDateTimePicker runat="server" ID="radToDate" Culture="en-GB" Width="290px" Height="40px"
                            AutoPostBackControl="Both" 
                            onselecteddatechanged="radToDate_SelectedDateChanged" Skin="Office2007">
                            <TimeView CellSpacing="-1" Culture="en-GB" TimeFormat="hh:mm tt"></TimeView>
                            <TimePopupButton ImageUrl="Images/clock.png" HoverImageUrl="Images/clock-hover.png"></TimePopupButton>
                            <Calendar ID="Calendar1" runat="server" EnableKeyboardNavigation="true" 
                                Skin="Office2007"></Calendar>
                            <DateInput DisplayDateFormat="dd-MMM-yyyy hh:mm tt" Font-Size="12pt" DateFormat="dd-MMM-yyyy hh:mm tt" LabelWidth="40%" AutoPostBack="True">
                                <EmptyMessageStyle Resize="None"></EmptyMessageStyle>
                                <ReadOnlyStyle Resize="None"></ReadOnlyStyle>
                                <FocusedStyle Resize="None"></FocusedStyle>
                                <DisabledStyle Resize="None"></DisabledStyle>
                                <InvalidStyle Resize="None"></InvalidStyle>
                                <HoveredStyle Resize="None"></HoveredStyle>
                                <EnabledStyle Resize="None"></EnabledStyle>
                            </DateInput>
                            <DatePopupButton ImageUrl="Images/calendar.png" HoverImageUrl="Images/calendar-hover.png"></DatePopupButton>
                        </telerik:RadDateTimePicker>
                    </td>
                    <td style=" padding-left:5px;">
                        <center>
                            <asp:ImageButton ID="btnGo" runat="server" ImageUrl="~/Images/refresh-button.png" OnClick="btnGo_Click" ToolTip="Refresh" />
                        </center>
                    </td>
                </tr>
            </table>    
        </div>
        <div id="content">
            <telerik:ReportViewer ID="rptViewer" runat="server" Skin="Orange" SkinsPath="Skins" ShowRefreshButton="false"
                Width="100%" Height="100%" />
        </div>
    </form>
</body>
</html>
