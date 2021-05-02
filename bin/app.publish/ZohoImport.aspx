<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ZohoImport.aspx.cs" Inherits="ZohoIntegration.ZohoImport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnImport" runat="server" Text="Import" OnClick="btnImport_Click" BackColor="#666699" Font-Bold="True" ForeColor="White"/>&nbsp;
            <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
        </div>

        <div id="divContent">
            <asp:GridView ID="grdItems" runat="server" AllowPaging="true" PageSize="10">

            </asp:GridView>
        </div>
    </form>
</body>
</html>
