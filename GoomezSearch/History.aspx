<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="History.aspx.cs" Inherits="GoomezSearch.History" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Goomez Search History</title>
    <link rel="shortcut icon" href="favicon.ico" />
</head>
<body style="margin: 0px">
    <form id="form1" runat="server">
    <div>
        <table width="100%">
            <tr valign="top" align="left">
                <td>
                    <asp:ImageButton ID="goomezLogo" runat="server" ImageUrl="~/images/GoomezLogo2.bmp" PostBackUrl="~/Default.aspx" ToolTip="<%$ Resources:Messages, goToGoomez%>" />
                </td>
                <td valign="middle">
                    <asp:Panel ID="Panel1" DefaultButton="btnSearchHistory" runat="server" >
                        <asp:TextBox ID="searchText" runat="server" />
                        <asp:Button ID="btnSearchHistory" runat="server" Text="<%$ Resources:Messages, historySearch %>" OnClick="btnSearchHistory_Click" />
                        <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Messages, goomezSearch %>" OnClick="btnSearch_Click" />
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="left" style="border-top: blue 0.01cm solid; background-color: #b0e0e6;" colspan="2" >
                    <asp:Label ID="lblResult" runat="server" BackColor="Transparent" ForeColor="Black" Width="100%" Visible="True" Font-Names="Arial" Font-Size="Smaller" Font-Bold="true"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="#3366CC" BorderWidth="1px" CellPadding="1" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="#003399" Height="200px" Width="220px" OnSelectionChanged="Calendar1_SelectionChanged">
                        <SelectedDayStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                        <TodayDayStyle BackColor="#99CCCC" ForeColor="White" />
                        <SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
                        <WeekendDayStyle BackColor="#CCCCFF" />
                        <OtherMonthDayStyle ForeColor="#999999" />
                        <NextPrevStyle Font-Size="8pt" ForeColor="#CCCCFF" />
                        <DayHeaderStyle BackColor="#99CCCC" ForeColor="#336666" Height="1px" />
                        <TitleStyle BackColor="#003399" BorderColor="#3366CC" BorderWidth="1px" Font-Bold="True"
                            Font-Size="10pt" ForeColor="#CCCCFF" Height="25px" />
                    </asp:Calendar>
                </td>
                <td valign="top">
                    <asp:DataList ID="DataList1" runat="server" ShowHeader="true" ShowFooter="true">
                        <ItemTemplate>
                            <asp:Label ID="dateTime" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="Small" ForeColor="Gray"><%# (DateTime.ParseExact(DataBinder.Eval(Container.DataItem, "DateTicks").ToString(), "yyyyMMddHHmmss", System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat)).ToString()%></asp:Label>
                            <asp:Label ID="searched" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="Medium" ForeColor="Blue"><a href="Default.aspx?search=<%# DataBinder.Eval(Container.DataItem, "SearchedText") %>"><%# DataBinder.Eval(Container.DataItem, "SearchedText")%></a></asp:Label></br>
                            <p/>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
