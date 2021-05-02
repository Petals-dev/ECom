<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Start.aspx.cs" Inherits="ZohoIntegration.Start" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Welcome To Zoho POC

            <asp:Button ID="btnAuthenticate" runat="server" Text="Authenticate" OnClick="btnAuthenticate_Click" BackColor="#666699" Font-Bold="True" ForeColor="White" />&nbsp;
 <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
        </div>
    </form>
</body>
</html>
