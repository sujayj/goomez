<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoResults.aspx.cs" Inherits="GoomezSearch.NoResults" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Goomez Search</title>
    <link rel="shortcut icon" href="favicon.ico" />
</head>
<body style="margin: 0px">
    <form id="form1" runat="server">
    <div>
        <table width="100%">
            <tr>
                <td align="Right" valign="top">
                <asp:LinkButton ID="myHistory" runat="server" PostBackUrl="~/History.aspx" Font-Names="Arial" Font-Size="XX-Small" ForeColor="Blue" Visible="true" Text="<%$Resources:Messages, goomezHistory %>"></asp:LinkButton>
                </td>
            </tr>
            <tr align="center">
                <td>
                    <asp:Panel ID="PanelSearch" DefaultButton="btnSearch" runat="server">
                        <asp:ImageButton ID="goomezLogo" runat="server" ImageUrl="~/images/GoomezLogo2.gif" PostBackUrl="~/Default.aspx" ToolTip="<%$ Resources:Messages, goToGoomez%>" /><br/>
                        <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                        <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Messages,search %>" OnClick="btnSearch_Click" />
                        <p/>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="right" style="border-top: blue 0.01cm solid; background-color: #b0e0e6;" >
                    <asp:Label ID="lblResult" runat="server" BackColor="Transparent" ForeColor="Black" Width="100%" Visible="True" Font-Names="Arial" Font-Size="Smaller"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblDidYouMean" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="Medium"></asp:Label>
                    <br />
                    <asp:Label ID="lblGoogle" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="Medium"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <p/>
                    <br />
                    <br />
                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Large"></asp:Label>
                    <br />
                    <asp:Image ID="imageNazi" runat="server" ImageUrl="~/images/soup_nazi.jpg" Visible="false" />
                 </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
