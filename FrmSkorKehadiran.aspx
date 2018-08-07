<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="FrmSkorKehadiran.aspx.vb" Inherits="HR_ASPNET.FrmSkorKehadiran" %>
<%@ Register assembly="DevExpress.Web.v17.2, Version=17.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="msgBox" namespace="BunnyBear" tagprefix="cc1" %>
<%@ Register Assembly="MetaBuilders.WebControls" Namespace="MetaBuilders.WebControls"
   TagPrefix="mb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div>
    <dx:ASPxPopupControl ID="PopEntScoring" runat="server" CloseAction="CloseButton" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="PopEntScoring"
        HeaderText="Poin Nilai" PopupAnimationType="Fade" 
            Width="300px" PopupElementID="PopEntry" CloseOnEscape="True" 
        Height="200px" Theme="MetropolisBlue" AllowDragging="True" ShowPageScrollbarWhenModal="true">
        <ClientSideEvents EndCallback="function(s, e) { PopEntKeluarga.Show(); }" />
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <div align="center">
                    <table>
                    <tr>
                        <td align="left" width="200px">Masuk</td>
                        <td>:</td>
                        <td align="left" width="70px">
                            <dx:ASPxTextBox ID="TxtNilaiMasuk" runat="server" Width="30px" 
                                ClientInstanceName="TxtNilaiMasuk">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>       
                    <tr>
                        <td align="left">Telat</td>
                        <td>:</td>
                        <td align="left">
                            <dx:ASPxTextBox ID="TxtNilaiTelat" runat="server" Width="30px" 
                                ClientInstanceName="TxtNilaiTelat">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Pulang Cepat</td>
                        <td>:</td>
                        <td align="left">
                            <dx:ASPxTextBox ID="TxtNilaiPulangCepat" runat="server" Width="30px" 
                                ClientInstanceName="TxtNilaiPulangCepat">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Ijin</td>
                        <td>:</td>
                        <td align="left">
                            <dx:ASPxTextBox ID="TxtNilaiIjin" runat="server" Width="30px" 
                                ClientInstanceName="TxtNilaiIjin">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Cuti</td>
                        <td>:</td>
                        <td align="left">
                            <dx:ASPxTextBox ID="TxtNilaiCuti" runat="server" Width="30px" 
                                ClientInstanceName="TxtNilaiCuti">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Sakit</td>
                        <td>:</td>
                        <td align="left">
                            <dx:ASPxTextBox ID="TxtNilaiSakit" runat="server" Width="30px" 
                                ClientInstanceName="TxtNilaiSakit">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Dinas Proyek</td>
                        <td>:</td>
                        <td align="left">
                            <dx:ASPxTextBox ID="TxtNilaiDinasProyek" runat="server" Width="30px" 
                                ClientInstanceName="TxtNilaiDinasProyek">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Ijin 1/2 Hari</td>
                        <td>:</td>
                        <td align="left">
                            <dx:ASPxTextBox ID="TxtNilaiIjinSetengahHari" runat="server" Width="30px" 
                                ClientInstanceName="TxtNilaiIjinSetengahHari">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Alpa</td>
                        <td>:</td>
                        <td align="left">
                            <dx:ASPxTextBox ID="TxtNilaiAlpa" runat="server" Width="30px" 
                                ClientInstanceName="TxtNilaiAlpa">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Tidak Absen</td>
                        <td>:</td>
                        <td align="left">
                            <dx:ASPxTextBox ID="TxtNilaiTidakAbsen" runat="server" Width="30px" 
                                ClientInstanceName="TxtNilaiTidakAbsen">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Medical RS</td>
                        <td>:</td>
                        <td align="left">
                            <dx:ASPxTextBox ID="TxtNilaiMedicalRS" runat="server" Width="30px" 
                                ClientInstanceName="TxtNilaiMedicalRS">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Batas Waktu Telat</td>
                        <td>:</td>
                        <td align="left">
                            <dx:ASPxTextBox ID="TxtBatasTelat" runat="server" Width="40px" 
                                ClientInstanceName="TxtBatasTelat">
                                <MaskSettings Mask="99:99" IncludeLiterals="All" />
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Batas Pulang Awal</td>
                        <td>:</td>
                        <td align="left">
                            <dx:ASPxTextBox ID="TxtBatasPulangAwal" runat="server" Width="40px" 
                                ClientInstanceName="TxtBatasPulangAwal">
                                <MaskSettings Mask="99:99" IncludeLiterals="All" />
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr><td></td></tr>
                    <tr>
                        <td colspan="4" align="right" style="border-top:2px; border-top-style:solid; border-top-color:Black; padding-top:10px;">
                            <dx:ASPxButton ID="BtnSave" runat="server" Text="LANJUTKAN" 
                                Theme="MetropolisBlue" Width="80px">
                            </dx:ASPxButton>                       
                            <dx:ASPxButton ID="BtnCancel" runat="server" Text="BATAL" CausesValidation="false"
                                Theme="MetropolisBlue" Width="80px" AutoPostBack="False">
                                <ClientSideEvents Click="function(s, e) { PopEntScoring.Hide();}" />
                            </dx:ASPxButton>   
                        </td>
                    </tr>
                    </table>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</div>
<div style="font-family:Segoe UI Light">
    <table>
        <tr>
            <td style="font-size:30px; text-decoration:underline">Nilai Kehadiran</td>
        </tr>
    </table>
</div>
<div>
    <table>
        <tr>
            <td>Periode</td>
            <td>:</td>
            <td>
                <dx:ASPxDateEdit ID="PrdAwal" runat="server" CssClass="font1" 
                    DisplayFormatString="dd-MMM-yyyy" 
                    Theme="MetropolisBlue" TabIndex="1">
                    <ValidationSettings 
                        Display="Dynamic"
                        ErrorDisplayMode="None" 
                        SetFocusOnError="true" 
                        RequiredField-IsRequired="true">
                    </ValidationSettings>
                </dx:ASPxDateEdit>        
            </td>
            <td>s.d.</td>
            <td>
                <dx:ASPxDateEdit ID="PrdAkhir" runat="server" CssClass="font1" 
                    DisplayFormatString="dd-MMM-yyyy" 
                    Theme="MetropolisBlue" TabIndex="2">
                    <DateRangeSettings StartDateEditID="PrdAwal"></DateRangeSettings>
                    <ValidationSettings 
                        Display="Dynamic"
                        ErrorDisplayMode="None" 
                        SetFocusOnError="true" 
                        RequiredField-IsRequired="true">
                    </ValidationSettings>
                </dx:ASPxDateEdit>
            </td>
        </tr>
        <tr>
            <td colspan="2"></td>
            <td>
                <dx:ASPxButton ID="BtnPrint" runat="server" Text="PRINT" 
                    Theme="MetropolisBlue" Width="75px" TabIndex="3">
                    <%--<ClientSideEvents Click="function(s,e) { OpenNewTab(); }" />--%>
                </dx:ASPxButton>  
            </td>
        </tr>
    </table>
<mb:DialogWindow ID="DialogWindow1" runat="server" CenterWindow="True" 
       Resizable="True" WindowHeight="600px" WindowWidth="900px">
</mb:DialogWindow>
<cc1:msgBox ID="msgBox1" runat="server" />
</div>
</asp:Content>
