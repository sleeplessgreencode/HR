<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="FrmKaryawan.aspx.vb" Inherits="HR_ASPNET.FrmKaryawan" %>
<%@ Register assembly="DevExpress.Web.v17.2, Version=17.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="msgBox" namespace="BunnyBear" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function OnGridFocusedRowChanged() {
            //            LblDivisi.SetText("Loading...");
            //            LblSubdivisi.SetText("Loading...");
            //            LblJabatan.SetText("Loading...");
            //            LblGolongan.SetText("Loading...");
            //            LblGrade.SetText("Loading...");
            //            LblUraianPekerjaan.SetText("Loading...");
            GridMaster.GetRowValues(GridMaster.GetFocusedRowIndex(), 'NIK;Alamat;NoTelp;Email;Divisi;Sub Divisi;Jabatan;Golongan;Grade;Uraian Pekerjaan;Foto;PrdAwal', OnGetRowValues);
            //            Creates an array to store data (Pendidikan, Ketrampilan dll) based on RowValues NIK
            //            Executes function to SetText based on this array (used the OnGetRowValues() way of doing things)
        }
        function OnGetRowValues(values) {
            LblAlamat.SetText(values[1]);
            LblNoTelp.SetText(values[2]);
            LblEmail.SetText(values[3]);
            LblDivisi.SetText(values[4]);
            LblSubdivisi.SetText(values[5]);
            LblJabatan.SetText(values[6]);
            LblGolongan.SetText(values[7]);
            LblGrade.SetText(values[8]);
            LblUraianPekerjaan.SetText(values[9]);
            LblPasFoto.SetImageUrl(values[10]);
            LblPrdAwal.SetText(values[11]);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="title" style="border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #C0C0C0; margin-bottom: 5px;">
    Data Karyawan
</div>
<div>
    <table>
        <tr>
            <td>
                <dx:ASPxButton ID="BtnTambah" runat="server" Text="TAMBAH" Width="80px" Theme="MetropolisBlue">
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
    <table style="width: 100%">
        <tr>
            <td>
                <dx:ASPxGridView ID="GridMaster" ClientInstanceName="GridMaster" runat="server" KeyFieldName="NIK" PreviewFieldName="Divisi" EnableRowCache="false" AutoGenerateColumns="true" Theme="MetropolisBlue" Width="100%" OnDataBound="GridMaster_DataBound" OnCustomButtonCallback="GridMaster_CustomButtonCallback" EnableCallBacks="false">
                <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                <SettingsSearchPanel Visible="true" ShowApplyButton="true" ShowClearButton="true" SearchInPreview="true" />
                <SettingsPager PageSize="5"></SettingsPager>
                <SettingsBehavior AllowFocusedRow="true" />
                <ClientSideEvents FocusedRowChanged="function(s, e) {OnGridFocusedRowChanged();GridMinarta.PerformCallback(s.GetFocusedRowIndex());GridRwytPekerjaan.PerformCallback(s.GetFocusedRowIndex());GridPendidikan.PerformCallback(s.GetFocusedRowIndex());GridKetrampilan.PerformCallback(s.GetFocusedRowIndex());GridIdentitas.PerformCallback(s.GetFocusedRowIndex());GridKeluarga.PerformCallback(s.GetFocusedRowIndex());GridDokumenPendukung.PerformCallback(s.GetFocusedRowIndex());}" />
                </dx:ASPxGridView>
            </td>
        </tr>
    </table>
    
    <table style="width: 100%; height: 200px">
        <tr>
            <td align="center" style="width:240px; vertical-align:top">
                <dx:ASPxImage runat="server" ID="PasFoto" Width="120px" Height="120px" Theme="MetropolisBlue" ClientInstanceName="LblPasFoto" ImageUrl="~/Images/PasFotoDefault.jpg">
                </dx:ASPxImage>
            </td>
            <td style="width:5px">
            </td>
            <td style="width:200px; vertical-align:top" >
                <table>
                    <tr>
                        <td>
                            <b>Alamat:</b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel1" ClientInstanceName="LblAlamat" runat="server" Text="" Width="150px"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>No Telp:</b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel2" ClientInstanceName="LblNoTelp" runat="server" Text=""></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Email:</b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel3" ClientInstanceName="LblEmail" runat="server" Text=""></dx:ASPxLabel>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width:5px">
            </td>
            <td style="vertical-align: top; width:100%;">
                <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
                    EnableTheming="True" TabPosition="Top" Theme="MetropolisBlue">
                    <TabPages>
                        <dx:TabPage Text="Pekerjaan">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl1" runat="server">
                                <b>Divisi:</b><br />
                                <dx:ASPxLabel ID="ASPxLabel4" ClientInstanceName="LblDivisi" runat="server" Text=""></dx:ASPxLabel>
                                <br /><br />

                                <b>Sub Divisi:</b><br />
                                <dx:ASPxLabel ID="ASPxLabel5" ClientInstanceName="LblSubdivisi" runat="server" Text=""></dx:ASPxLabel>
                                <br /><br />

                                <b>Jabatan:</b><br />
                                <dx:ASPxLabel ID="ASPxLabel6" ClientInstanceName="LblJabatan" runat="server" Text=""></dx:ASPxLabel>
                                <br /><br />

                                <b>Golongan:</b><br />
                                <dx:ASPxLabel ID="ASPxLabel7" ClientInstanceName="LblGolongan" runat="server" Text=""></dx:ASPxLabel>
                                <br /><br />

                                <b>Grade:</b><br />
                                <dx:ASPxLabel ID="ASPxLabel8" ClientInstanceName="LblGrade" runat="server" Text=""></dx:ASPxLabel>
                                <br /><br />
                                
                                <b>Periode:</b><br />
                                <dx:ASPxLabel ID="ASPxLabel10" ClientInstanceName="LblPrdAwal" runat="server" Text=""></dx:ASPxLabel>
                                <br /><br />

                                <b>Uraian Pekerjaan:</b><br />
                                <dx:ASPxLabel ID="ASPxLabel9" ClientInstanceName="LblUraianPekerjaan" runat="server" Text=""></dx:ASPxLabel>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Riwayat di Minarta">
                            <ContentCollection>
                                <dx:ContentControl>
                                    <dx:ASPxGridView ID="GridMinarta" ClientInstanceName="GridMinarta" runat="server" KeyFieldName="NIK" EnableRowCache="true" AutoGenerateColumns="true" Theme="MetropolisBlue" Width="100%" OnCustomCallback="GridMinarta_CustomCallback" OnDataBound="GridMinarta_DataBound">
                                    <SettingsBehavior AllowFocusedRow="true" AllowSort="false"/>
                                    <SettingsSearchPanel Visible="true" ShowApplyButton="true" ShowClearButton="true" />
                                    </dx:ASPxGridView>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Riwayat Pekerjaan">
                            <ContentCollection>
                                <dx:ContentControl>
                                    <dx:ASPxGridView ID="GridRwytPekerjaan" ClientInstanceName="GridRwytPekerjaan" runat="server" KeyFieldName="NIK" EnableRowCache="true" AutoGenerateColumns="true" Theme="MetropolisBlue" Width="100%" OnCustomCallback="GridRwytPekerjaan_CustomCallback" OnDataBound="GridRwytPekerjaan_DataBound">
                                    <SettingsBehavior AllowFocusedRow="true" AllowSort="false"/>
                                    <SettingsSearchPanel Visible="true" ShowApplyButton="true" ShowClearButton="true" />
                                    </dx:ASPxGridView>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Data Pendidikan">
                            <ContentCollection>
                                <dx:ContentControl>
                                    <dx:ASPxGridView ID="GridPendidikan" ClientInstanceName="GridPendidikan" runat="server" KeyFieldName="NIK" EnableRowCache="true" AutoGenerateColumns="true" Theme="MetropolisBlue" Width="100%" OnCustomCallback="GridPendidikan_CustomCallback" OnDataBound="GridPendidikan_DataBound">
                                    <SettingsBehavior AllowFocusedRow="true" AllowSort="false"/>
                                    <SettingsSearchPanel Visible="true" ShowApplyButton="true" ShowClearButton="true" />
                                    </dx:ASPxGridView>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Data Ketrampilan">
                            <ContentCollection>
                                <dx:ContentControl>
                                    <dx:ASPxGridView ID="GridKetrampilan" ClientInstanceName="GridKetrampilan" runat="server" KeyFieldName="NIK" EnableRowCache="true" AutoGenerateColumns="true" Theme="MetropolisBlue" Width="100%" OnCustomCallback="GridKetrampilan_CustomCallback" OnDataBound="GridKetrampilan_DataBound">
                                    <SettingsBehavior AllowFocusedRow="true" AllowSort="false"/>
                                    <SettingsSearchPanel Visible="true" ShowApplyButton="true" ShowClearButton="true" />
                                    </dx:ASPxGridView>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Identitas">
                            <ContentCollection>
                                <dx:ContentControl>
                                    <dx:ASPxGridView ID="GridIdentitas" ClientInstanceName="GridIdentitas" runat="server" KeyFieldName="NIK" EnableRowCache="true" AutoGenerateColumns="true" Theme="MetropolisBlue" Width="100%" OnCustomCallback="GridIdentitas_CustomCallback" OnDataBound="GridIdentitas_DataBound">
                                    <SettingsBehavior AllowFocusedRow="true" AllowSort="false"/>
                                    <SettingsSearchPanel Visible="true" ShowApplyButton="true" ShowClearButton="true" />
                                    </dx:ASPxGridView>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Data Keluarga">
                            <ContentCollection>
                                <dx:ContentControl>
                                    <dx:ASPxGridView ID="GridKeluarga" ClientInstanceName="GridKeluarga" runat="server" KeyFieldName="NIK" EnableRowCache="true" AutoGenerateColumns="true" Theme="MetropolisBlue" Width="100%" OnCustomCallback="GridKeluarga_CustomCallback" OnDataBound="GridKeluarga_DataBound">
                                    <SettingsBehavior AllowFocusedRow="true" AllowSort="false"/>
                                    <SettingsSearchPanel Visible="true" ShowApplyButton="true" ShowClearButton="true" />
                                    </dx:ASPxGridView>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Dokumen Pendukung" NewLine="true">
                            <ContentCollection>
                                <dx:ContentControl>
                                    <dx:ASPxGridView ID="GridDokumenPendukung" ClientInstanceName="GridDokumenPendukung" runat="server" OnCustomCallback="GridDokumenPendukung_CustomCallback" OnCustomButtonCallback="GridDokumenPendukung_CustomButtonCallback" 
                                     Theme="MetropolisBlue" AutoGenerateColumns="False" Width="100%">
                                        <Columns>
                                            <dx:GridViewCommandColumn Width="80">
                                            <CustomButtons>
                                                <dx:GridViewCommandColumnCustomButton ID="ViewBaris" Text="VIEW" />
                                            </CustomButtons>
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataTextColumn FieldName="NamaFile" Width="100%" Caption="Nama File">
                                                <Settings AutoFilterCondition="Contains" HeaderFilterMode="CheckedList" />
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <Settings ShowFilterRow="True" ShowFooter="true" HorizontalScrollBarMode="Visible" ShowHeaderFilterButton="true" />
                                        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                        <SettingsBehavior AllowFocusedRow="true" AllowSort="false"/>                                        
                                        <ClientSideEvents EndCallback="function(s, e) { if (s.cpOpenWindow != null) window.open(s.cpOpenWindow, 'newwindow', 'width=800px, height=600px', 'menubar=no'); }" />
                                    </dx:ASPxGridView>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                    </TabPages>
                </dx:ASPxPageControl>
            </td>
        </tr>
        <%--<tr>
            <td>
                <dx:ASPxMemo runat="server" ID="DetailNotes" ClientInstanceName="DetailNotes" Width="100%" Height="170" ReadOnly="true" />
            </td>
        </tr>--%>    
    </table>
    <cc1:msgBox ID="msgBox1" runat="server" /> 
</div>
</asp:Content>