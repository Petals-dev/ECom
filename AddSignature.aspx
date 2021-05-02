<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddSignature.aspx.cs" Inherits="ZohoIntegration.Signature.AddSignature" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <meta name="viewport" content="width=device-width, initial-scale=1, minimum-scale=1, maximum-scale=1, user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <link rel="stylesheet" href="css/signature-pad.css">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <script type="text/javascript">
            var _gaq = _gaq || [];
            _gaq.push(['_setAccount', 'UA-39365077-1']);
            _gaq.push(['_trackPageview']);
            (function () {
                var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
                ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
                var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
            })();
        </script>
        <div id="signature-pad" class="m-signature-pad">    
            <div class="description">Draw your signature here</div>
            <div class="m-signature-pad--body">
                <canvas width="200" height="100"></canvas>
            </div>
            <div class="m-signature-pad--footer">
                
                <button class="button clear" data-action="clear">
                    Clear</button>
                <button type="button" class="button" data-action="change-color">Change color</button>
                <button type="button" class="button" data-action="undo">Undo</button>
                <asp:Button Text="Save" runat="server" CssClass="button save" data-action="save"
                    OnClick="Save" />
            </div>
            <script src="scripts/signature-pad.js"></script>
            <script src="scripts/app.js"></script>
            <input type="hidden" id="hfSign" runat="server" />
        </div>

        <div class="description" style="margin-top:50px">
            Upload signature<br />

            <input type="file" id="fileControl" />
            <asp:Button Text="Save" ID="btnUpload" runat="server" CssClass="button save" />
        </div>
    </form>
</body>
</html>
