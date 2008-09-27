<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GoomezSearch.Default" UICulture="Auto" %>

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
                <td>
                    <asp:Label ID="ie7Label" runat="server" Font-Names="Arial" Font-Size="XX-Small" ForeColor="Blue" Visible="false" ToolTip="<%$Resources:Messages, searchProviderToolTip %>" ><a id="ie7" href="javascript:window.external.AddSearchProvider('http://fsartech/goomez/provider.xml')" ><asp:Literal ID="literal" runat="server" Text="<%$Resources:Messages, searchProvider %>" /></a></asp:Label>
                    <asp:Label ID="ie8Label" runat="server" Font-Names="Arial" Font-Size="XX-Small" ForeColor="Blue" Visible="false" ToolTip="<%$Resources:Messages, installIE8Activity %>" ><a id="ie8" href="javascript:window.external.AddService('http://fsartech/goomez/GoomezActivity.xml')" ><asp:Literal ID="literal1" runat="server" Text="<%$Resources:Messages, ieActivity %>" /></a></asp:Label>
                    <asp:Label ID="ffLablel" runat="server" Font-Names="Arial" Font-Size="XX-Small" ForeColor="Blue" Visible="false" ToolTip="<%$Resources:Messages, userJsToolTip %>" ><a id="ff3" href="user.js"><asp:Literal ID="literal2" runat="server" Text="<%$Resources:Messages, ftpSupport %>" /></a></asp:Label>
                </td>
                <td align="right" valign="top">
                    <asp:LinkButton ID="store" runat="server" PostBackUrl="http://www.cafepress.com/goomez" Font-Names="Arial" Font-Size="XX-Small" ForeColor="Blue" Visible="true" Text="<%$Resources:Messages, goomezStore %>"/>
                    <asp:LinkButton ID="myHistory" runat="server" PostBackUrl="~/History.aspx" Font-Names="Arial" Font-Size="XX-Small" ForeColor="Blue" Visible="true" Text="<%$Resources:Messages, goomezHistory %>"/>
                </td>
            </tr>
            <tr align="center">
                <td colspan="2">
                    <asp:panel ID="Panel1" defaultbutton="btnSearch" runat="server">
                        <asp:ImageButton ID="imageEscudo" runat="server" ImageUrl="~/images/epearol1.gif" Visible="false" ToolTip="Peñarol en la Wikipedia" PostBackUrl="http://en.wikipedia.org/wiki/C.A._Pe%C3%B1arol#Conmebol_All_Time_Club_Rankings" />
                        <asp:Image ID="imageLogo" runat="server" ImageUrl="~/images/GoomezLogo2.gif" /><br/>
                        <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                        <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Messages,search %>" OnClick="btnSearch_Click" />
                        <p/>
                    </asp:panel>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <asp:Table ID="tableHeader" runat="server" Visible="false" BorderStyle="Solid" Width="100%" BackColor="#b0e0e6" BorderWidth="1" BorderColor="Blue">
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Right">
                                <asp:Label ID="lblResult" runat="server" BackColor="Transparent" ForeColor="Black" Font-Names="Arial" Font-Size="Smaller" Visible="False"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </td>
            </tr>
            <tr>
                <td>
                    <p/>
                    <asp:DataList ID="listResult" runat="server" ShowHeader="true" ShowFooter="true">
                        <ItemTemplate>
                            <asp:Label ID="link" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="Medium" ForeColor="Blue"><a href="file:///<%# DataBinder.Eval(Container.DataItem, "FileFullName") %>"><%# DataBinder.Eval(Container.DataItem, "FileName") %></a></asp:Label>
                            <br/>
                            <asp:Label ID="full" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="Small" ForeColor="Black"><%# DataBinder.Eval(Container.DataItem, "FileFolderName")%></asp:Label>
                            <br/>
                            <asp:Label ID="size" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="Small" ForeColor="Green"><%# DataBinder.Eval(Container.DataItem, "FileSizeinBytes") %> bytes</asp:Label> 
                            &nbsp; 
                            <asp:Label ID="folder" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="Smaller" ForeColor="MediumPurple"><a href="file:///<%# DataBinder.Eval(Container.DataItem, "FileFolderName") %>" target="_blank" style="color:Gray"><asp:Literal ID="open" runat="server" Text="<%$ Resources:Messages,openFolder %>" ></asp:Literal></a></asp:Label>
                            <p/>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2" >
                    <asp:Table ID="tableFooter" runat="server" Visible="false" BorderStyle="Solid" Width="100%" BackColor="#b0e0e6" BorderWidth="1" BorderColor="Blue">
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Center">
                                <asp:Label ID="lblFirst" runat="server" BackColor="Transparent" ForeColor="Black" Font-Names="Arial" Font-Size="Smaller" Visible="False"></asp:Label>&nbsp;
                                <asp:Label ID="lblPrevious" runat="server" BackColor="Transparent" ForeColor="Black" Font-Names="Arial" Font-Size="Smaller" Visible="False"></asp:Label>&nbsp;
                                <asp:Label ID="lblCurrent" runat="server" BackColor="Transparent" ForeColor="Black" Font-Names="Arial" Font-Size="Smaller" Visible="False"></asp:Label>&nbsp;
                                <asp:Label ID="lblNext" runat="server" BackColor="Transparent" ForeColor="Black" Font-Names="Arial" Font-Size="Smaller" Visible="False"></asp:Label>&nbsp;
                                <asp:Label ID="lblLast" runat="server" BackColor="Transparent" ForeColor="Black" Font-Names="Arial" Font-Size="Smaller" Visible="False"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
