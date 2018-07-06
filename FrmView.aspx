<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FrmView.aspx.vb" Inherits="HR_ASPNET.FrmView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <div>
    <asp:Image ID="Image1" runat="server" ImageUrl='<%=Session("ViewJPG")%>'/>
    <object data='<%=Session("ViewPDF")%>' type="application/pdf" style="height:calc(100vh - 15px)" width="100%">      
    </object>
    </div>
</body>
</html>
