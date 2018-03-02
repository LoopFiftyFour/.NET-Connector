<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="Default.aspx.cs" Inherits="Loop54.Test.AspNet.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Loop54 .NET Connector ASP.NET test</title>
</head>
<body>
    <h1>Loop54 .NET Connector ASP.NET test</h1>
    <form id="SearchForm" runat="server" DefaultButton="SubmitButton">
        <div>
            <label for="QueryText">Search query</label>
            <asp:TextBox id="QueryText" runat="server" autofocus="" />
            <asp:Button id="SubmitButton" runat="server" Text="Search" OnClick="SubmitButton_Click" />
        </div>
    </form>
    <hr />
    <div>
        <asp:Label id="RequestLabel" runat="server"/>
    </div>
    <hr />
    <div>
        <asp:Label id="ResponseLabel" runat="server"/>
    </div>
</body>
</html>
