<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="GoomezSearch.Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Oooops!</title>
    <link rel="shortcut icon" href="favicon.ico" />
</head>
<body style="margin: 0px">
    <form id="form1" runat="server">
    <div>
        <table width="100%">
            <tr>
                <td align="Right" valign="top">
                    &nbsp;</td>
            </tr>
            <tr align="center">
                <td>
                    <asp:ImageButton ID="goomezLogo" runat="server" ImageUrl="~/images/GoomezLogo2.gif" PostBackUrl="~/Default.aspx" ToolTip="<%$ Resources:Messages, goToGoomez%>" />
                    <p/>
                </td>
            </tr>
            <tr>
                <td align="right" style="border-top: blue 0.01cm solid; background-color: #b0e0e6;" >
                    <asp:Label ID="lblResult" runat="server" BackColor="Transparent" ForeColor="Black" Width="100%" Visible="True" Font-Names="Arial" Font-Size="Smaller"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <p/>
                    <br />
                    <br />
                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Large" Text="<%$ Resources:Messages, errorMessage %>"></asp:Label></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>