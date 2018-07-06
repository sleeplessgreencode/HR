<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="FrmLoadAbsensi.aspx.vb" Inherits="HR_ASPNET.FrmLoadAbsensi" %>
<%@ Register assembly="DevExpress.Web.v17.2, Version=17.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
    $(function () {
        $("[id*=GridData] td").hover(function () {
            $("td", $(this).closest("tr")).addClass("hover_row");
        }, function () {
            $("td", $(this).closest("tr")).removeClass("hover_row");
        });
    });
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
    <dx:ASPxPopupControl ID="ErrMsg" runat="server" CloseAction="CloseButton" CloseOnEscape="True" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="ErrMsg"
        HeaderText="Information" PopupAnimationType="None" EnableViewState="False" Width="500px" Theme="MetropolisBlue" PopupElementID="ErrMsg">
        <ClientSideEvents PopUp="function(s, e) { BtnClose.Focus(); }" />
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <div style="text-align:center; font-size:large; font-family:Segoe UI Light;">
                    <asp:Label ID="LblErr" runat="server" Text=""></asp:Label>
                    <br /> <br />
                    <div align="center">
                        <dx:ASPxButton ID="BtnClose" runat="server" AutoPostBack="False" ClientInstanceName="BtnClose"
                            Text="OK" Theme="MetropolisBlue" Width="75px">
                            <ClientSideEvents Click="function(s, e) { ErrMsg.Hide();}" />
                        </dx:ASPxButton>
                    </div>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</div>

<div style="font-family:Segoe UI Light">
<table>
<tr>
    <td style="font-size:30px; text-decoration:underline">Upload Data Absensi</td>
</tr>
</table>
</div>

<div class="font1">
<table>
<tr>
    <td>File</td>
    <td>:</td>
    <td>
        <asp:FileUpload ID="FileUpload1" runat="server" CssClass="font1" Width="400px" />
    </td>
    <td></td>
</tr>
<tr>
    <td colspan="2"></td>
    <td>
        <dx:ASPxButton ID="BtnUpload" runat="server" Text="UPLOAD" 
            Width="75px" Theme="MetropolisBlue">
        </dx:ASPxButton>     
    </td>
</tr>
</table>

<table style="width: 100%">
<tr>
    <td>  
        <asp:GridView ID="GridData" runat="server" AutoGenerateColumns="False"               
            CellPadding="4" ForeColor="#333333" GridLines="Vertical" 
            ShowHeaderWhenEmpty="True" 
            PageSize="50" ShowFooter="True" 
            AllowPaging="True">
            <Columns>
                <asp:BoundField DataField="NIK" HeaderText="NIK" HeaderStyle-Width="100px" ItemStyle-Width = "100px">                    
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Nama" HeaderText="Nama" HeaderStyle-Width="200px" ItemStyle-Width = "200px">
                </asp:BoundField>
                <asp:BoundField DataField="Tanggal" HeaderText="Tanggal" HeaderStyle-Width="120px" ItemStyle-Width = "120px" 
                ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd-MMM-yyyy}">                    
                </asp:BoundField>
                <asp:BoundField DataField="Scan Masuk" HeaderText="Jam Masuk" HeaderStyle-Width="80px" ItemStyle-Width = "80px"
                    DataFormatString="{0:HH:mm}">                        
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Scan Pulang" HeaderText="Jam Pulang" HeaderStyle-Width="80px" ItemStyle-Width = "80px"
                    DataFormatString="{0:HH:mm}">                        
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="False" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView> 
    </td>    
</tr>    
<tr>
    <td>
        <dx:ASPxButton ID="BtnSave" runat="server" Text="SAVE" 
            Width="75px" Theme="MetropolisBlue" Visible="false">
        </dx:ASPxButton>   
    </td>
</tr>
</table>   

</div>

</asp:Content>
