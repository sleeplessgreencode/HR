<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="FrmEntrySetup.aspx.vb" Inherits="HR_ASPNET.FrmEntrySetup" %>
<%@ Register assembly="DevExpress.Web.v17.2, Version=17.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="msgBox" namespace="BunnyBear" tagprefix="cc1" %>

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

<div class="font1">
<table>
<tr>
    <td colspan="4" style="border-bottom:2px; border-bottom-style:solid; border-bottom-color:Black; padding-bottom:5px;font-size:30px; text-decoration:underline; font-family:Segoe UI Light">Setup Akses</td>
    <td colspan="3" align="right" style="border-bottom:2px; border-bottom-style:solid; border-bottom-color:Black; padding-bottom:5px;">
        <asp:TextBox ID="TxtAction" runat="server" Width="35px"  Visible="False" 
            Text="NEW"></asp:TextBox>
        <dx:ASPxButton ID="BtnSave" runat="server" Text="SIMPAN" 
            Width="75px" Theme="MetropolisBlue" TabIndex="23">
        </dx:ASPxButton>     
        <dx:ASPxButton ID="BtnCancel" runat="server" Text="BATAL" 
            Theme="MetropolisBlue" Width="75px" TabIndex="24" CausesValidation="False">
        </dx:ASPxButton>          
    </td>
</tr>
<tr>
    <td>User ID</td>
    <td>:</td>
    <td> 
        <dx:ASPxTextBox ID="TxtUserID" runat="server" Width="250px" MaxLength="20" TabIndex="1">
            <ValidationSettings Display="Dynamic">
                <RequiredField IsRequired="True"/>
            </ValidationSettings>            
        </dx:ASPxTextBox>
    </td>    
    <td></td>
    <td>User Name</td>
    <td>:</td>
    <td> 
        <dx:ASPxTextBox ID="TxtUserName" runat="server" Width="250px" MaxLength="30" 
            TabIndex="2">
            <ValidationSettings Display="Dynamic">
                <RequiredField IsRequired="True"/>
            </ValidationSettings>            
        </dx:ASPxTextBox>
    </td>    
</tr>
<tr>
    <td>Password</td>
    <td>:</td>
    <td> 
        <dx:ASPxTextBox ID="TxtPassword" runat="server" Width="250px" MaxLength="15" TabIndex="3" Password="True">            
        </dx:ASPxTextBox>
    </td>    
    <td></td>
    <td>Confirm Password</td>
    <td>:</td>
    <td> 
        <dx:ASPxTextBox ID="TxtConfirmPw" runat="server" Width="250px" MaxLength="15" TabIndex="4" Password="True">            
        </dx:ASPxTextBox>
    </td>   
</tr>
<tr>
    <td colspan="2"></td>
    <td>*Kosongkan jika tidak ingin merubah password</td>
    <td></td>
    <td colspan="2"></td>
    <td></td>
</tr>
<tr><td></td></tr>
<tr>
<td colspan="7">
<table>
<tr>
    <td colspan="7" align="left" style="padding-top:10px">    
        <dx:ASPxButton ID="BtnAllMenu" runat="server" Text="PILIH SEMUA MENU" 
            Width="75px" Theme="MetropolisBlue">
        </dx:ASPxButton>  
    </td>
    <td></td>
</tr>
<tr>
    <td align="center" colspan="7" style="background-color:Silver; height:20px; font-weight:bold" >AKSES MENU BASIC DATA</td>
    <td></td>
</tr>
<tr>
    <td colspan="2"></td>
    <td>
        <asp:CheckBox ID="CbDataKaryawan" runat="server" Text="Data Karyawan" />
    </td>
    <td></td>
    <td colspan="2"></td>
    <td>
        <asp:CheckBox ID="CbHariLibur" runat="server" Text="Hari Libur" />
    </td>
</tr>
<tr>
    <td align="center" colspan="7" style="background-color:Silver; height:20px; font-weight:bold" >AKSES MENU ENTRY</td>
</tr>
<tr>
    <td colspan="2"></td>
    <td>
        <asp:CheckBox ID="CbLoadAbsensi" runat="server" Text="Load Absensi" />
    </td>
    <td></td>
    <td colspan="2"></td>
    <td>
        <asp:CheckBox ID="CbEntryAbsensi" runat="server" Text="Entry Absensi" />
    </td>    
</tr>
<tr>
    <td align="center" colspan="7" style="background-color:Silver; height:20px; font-weight:bold" >AKSES MENU REPORT</td>
</tr>
<tr>
    <td colspan="2"></td>
    <td>
        <asp:CheckBox ID="CbRekapAbsensi" runat="server" Text="Rekap Absensi" />
    </td>
    <td></td>
    <td colspan="2"></td>
    <td>
        <asp:CheckBox ID="CbAbsensiKaryawan" runat="server" Text="Absensi Karyawan" />
    </td>
</tr>
<tr>
    <td colspan="2"></td>
    <td>
        <asp:CheckBox ID="CbRekapStatusAbsensi" runat="server" Text="Rekap Status Absensi" />
    </td>
    <td></td>
    <td colspan="2"></td>
</tr>
<tr>
    <td align="center" colspan="7" style="background-color:Silver; height:20px; font-weight:bold" >AKSES MENU TOOLS</td>
</tr>
<tr>
    <td colspan="2"></td>
    <td>
        <asp:CheckBox ID="CbSetup" runat="server" Text="Setup Akses" />
    </td>
    <td></td>
    <td colspan="2"></td>
    <td>
        <asp:CheckBox ID="CbPasswd" runat="server" Text="Change Password" />
    </td>
</tr>
</table>
</td>
</tr>
</table>
<cc1:msgBox ID="msgBox1" runat="server" /> 
</div>
</asp:Content>