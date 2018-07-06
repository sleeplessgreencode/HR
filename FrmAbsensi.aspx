<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="FrmAbsensi.aspx.vb" Inherits="HR_ASPNET.FrmAbsensi" %>
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
<div>
    <dx:ASPxPopupControl ID="PopEntry" runat="server" CloseAction="CloseButton" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="PopEntry"
        HeaderText="Data Entry" PopupAnimationType="Fade" 
            Width="700px" PopupElementID="PopEntry" CloseOnEscape="True" 
        Height="200px" Theme="MetropolisBlue">
        <ClientSideEvents Init="function(s, e) {}" EndCallback="function(s, e) { PopEntry.Show(); }" />
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <div align="center">
                    <table>
                    <tr>
                        <td align="left">
                            <asp:TextBox ID="TxtAction" runat="server" Text="" 
                                BorderColor="White" BorderStyle="None" ForeColor="White" Width="30px"></asp:TextBox>
                        </td>
                    </tr> 
                    <tr>
                        <td align="left">NIK</td>
                        <td>:</td>
                        <td align="left">
                            <dx:ASPxComboBox ID="DDLKaryawan" runat="server" 
                                CssClass="font1" Width="400px" 
                                ClientInstanceName="DDLField1" TabIndex="1" Theme="MetropolisBlue">
                            </dx:ASPxComboBox>
                        </td>    
                    </tr>
                    <tr>
                        <td align="left">Tanggal Absensi</td>
                        <td>:</td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="TglAbsen" runat="server" CssClass="font1" 
                                DisplayFormatString="dd-MMM-yyyy" Width="200px" 
                                Theme="MetropolisBlue" TabIndex="2">
                                <ValidationSettings Display="Dynamic">
                                    <RequiredField IsRequired="True"/>
                                </ValidationSettings>   
                            </dx:ASPxDateEdit>
                        </td>    
                    </tr>
                    <tr>
                        <td align="left">Status</td>
                        <td>:</td>
                        <td align="left">
                            <dx:ASPxComboBox ID="DDLStatus" runat="server" 
                                CssClass="font1" Width="300px" 
                                ClientInstanceName="DDLStatus" TabIndex="3" Theme="MetropolisBlue" AutoPostBack="true">
                                <Items>
                                    <dx:ListEditItem Text="Masuk" Value="Masuk" Selected="True" />
                                    <dx:ListEditItem Text="Ijin" Value="Ijin" />
                                    <dx:ListEditItem Text="Ijin 1/2 Hari" Value="Ijin 1/2 Hari" />
                                    <dx:ListEditItem Text="Sakit" Value="Sakit" />
                                    <dx:ListEditItem Text="Medical RS" Value="Medical RS" />
                                    <dx:ListEditItem Text="Alpa" Value="Alpa" />
                                    <dx:ListEditItem Text="Cuti" Value="Cuti" />
                                    <dx:ListEditItem Text="Dinas Proyek" Value="Dinas Proyek" />
                                </Items>
                            </dx:ASPxComboBox>
                        </td>    
                    </tr>
                    <tr>
                        <td align="left">Jam Masuk</td>
                        <td>:</td>
                        <td align="left">
                            <dx:ASPxTimeEdit ID="JamMasuk" runat="server" 
                                Theme="MetropolisBlue" EditFormat="Custom" EditFormatString="HH:mm" TabIndex="4">
                                <ValidationSettings Display="Dynamic">
                                    <RequiredField IsRequired="True"/>
                                </ValidationSettings>   
                            </dx:ASPxTimeEdit>
                        </td>    
                    </tr>
                    <tr>
                        <td align="left">Jam Pulang</td>
                        <td>:</td>
                        <td align="left">
                            <dx:ASPxTimeEdit ID="JamPulang" runat="server"
                                Theme="MetropolisBlue" EditFormat="Custom" EditFormatString="HH:mm" TabIndex="5">
                                <ValidationSettings Display="Dynamic">
                                    <RequiredField IsRequired="True"/>
                                </ValidationSettings>   
                            </dx:ASPxTimeEdit>
                        </td>    
                    </tr>
                    <tr>
                        <td align="left">Keterangan</td>
                        <td>:</td>
                        <td align="left">
                            <dx:ASPxTextBox ID="TxtKeterangan" runat="server" Width="300px" CssClass="font1" MaxLength="50" TabIndex="6">
                            </dx:ASPxTextBox>
                        </td>    
                    </tr>
                    <tr>
                        <td colspan="3" align="right" style="border-top:2px; border-top-style:solid; border-top-color:Black; padding-top:10px;">
                            <dx:ASPxButton ID="BtnSave" runat="server" Text="SIMPAN"
                                Theme="MetropolisBlue" TabIndex="7" Width="80px">
                            </dx:ASPxButton>                       
                            <dx:ASPxButton ID="BtnCancel" runat="server" Text="BATAL" CausesValidation="false"
                                Theme="MetropolisBlue" TabIndex="8" Width="80px" AutoPostBack="False">
                                <ClientSideEvents Click="function(s, e) { PopEntry.Hide();}" />
                            </dx:ASPxButton>   
                        </td>
                    </tr>
                    <tr>
                        <td></td>
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
    <td style="font-size:30px; text-decoration:underline">Entry Data Absensi</td>
</tr>
</table>
</div>

<div class="font1">
<table>
<tr>
    <td>Tanggal Absensi</td>
    <td>:</td>
    <td>
        <dx:ASPxDateEdit ID="PrdAwal" runat="server" CssClass="font1" 
            DisplayFormatString="dd-MMM-yyyy" 
            Theme="MetropolisBlue" TabIndex="1" AutoPostBack="True">
        </dx:ASPxDateEdit>        
    </td>
    <td>s.d.</td>
    <td>
        <dx:ASPxDateEdit ID="PrdAkhir" runat="server" CssClass="font1" 
            DisplayFormatString="dd-MMM-yyyy" 
            Theme="MetropolisBlue" TabIndex="2" AutoPostBack="True">
            <DateRangeSettings StartDateEditID="PrdAwal"></DateRangeSettings>
        </dx:ASPxDateEdit>
    </td>
</tr>
</table>
<table>
<tr>
    <td>Filter by</td>
    <td>:</td>
    <td>
        <dx:ASPxComboBox ID="DDLField1" runat="server" 
            CssClass="font1" Width="300px" 
            ClientInstanceName="DDLField1" TabIndex="2" Theme="MetropolisBlue" 
            AutoPostBack="True">
            <Items>
                <dx:ListEditItem Text="Pilih salah satu" Value="0" Selected="True" />
                <dx:ListEditItem Text="NIK" Value="A.NIK" />
                <dx:ListEditItem Text="Nama" Value="Nama" />
                <%--<dx:ListEditItem Text="Tanggal Absensi" Value="TglMasuk" />--%>
                <dx:ListEditItem Text="Jam Masuk" Value="JamMasuk" />
                <dx:ListEditItem Text="Jam Pulang" Value="JamKeluar" />
                <dx:ListEditItem Text="Status" Value="Status" />
            </Items>
        </dx:ASPxComboBox>
    </td>
    <td>
        <dx:ASPxComboBox ID="DDLFilterBy1" runat="server" ValueType="System.String" 
            CssClass="font1" Width="200px" 
            ClientInstanceName="DDLFilterBy1" Theme="MetropolisBlue">
            <Items>
                <dx:ListEditItem Text="Equals" Value="0" Selected="True" />
            </Items>
        </dx:ASPxComboBox>
    </td>
    <td>
        <dx:ASPxTextBox ID="TxtFind1" runat="server" Width="200px" CssClass="font1">
        </dx:ASPxTextBox>
        <dx:ASPxDateEdit ID="TglFilter1" runat="server" CssClass="font1" 
            DisplayFormatString="dd-MMM-yyyy" Width="200px" 
            Theme="MetropolisBlue" Visible="False">
        </dx:ASPxDateEdit>
        <dx:ASPxTimeEdit ID="TimeFilter1" runat="server" Visible="false" 
            Theme="MetropolisBlue" EditFormat="Custom" EditFormatString="HH:mm">
        </dx:ASPxTimeEdit>
    </td>
    <td rowspan="2" valign="bottom">
        <dx:ASPxButton ID="BtnFind" runat="server" Text="FILTER" 
            Theme="MetropolisBlue" TabIndex="4">
        </dx:ASPxButton>   
    </td>   
</tr>
<tr>
    <td colspan="2"></td>
    <td>
        <dx:ASPxComboBox ID="DDLField2" runat="server" 
            CssClass="font1" Width="300px" 
            ClientInstanceName="DDLField2" TabIndex="2" Theme="MetropolisBlue" 
            AutoPostBack="True">
            <Items>
                <dx:ListEditItem Text="Pilih salah satu" Value="0" Selected="True" />
                <dx:ListEditItem Text="NIK" Value="A.NIK" />
                <dx:ListEditItem Text="Nama" Value="Nama" />
                <%--<dx:ListEditItem Text="Tanggal Absensi" Value="TglMasuk" />--%>
                <dx:ListEditItem Text="Jam Masuk" Value="JamMasuk" />
                <dx:ListEditItem Text="Jam Pulang" Value="JamKeluar" />
                <dx:ListEditItem Text="Status" Value="Status" />
            </Items>
        </dx:ASPxComboBox>
    </td>
    <td>
        <dx:ASPxComboBox ID="DDLFilterBy2" runat="server" ValueType="System.String" 
            CssClass="font1" Width="200px" 
            ClientInstanceName="DDLFilterBy2" Theme="MetropolisBlue">
            <Items>
                <dx:ListEditItem Text="Equals" Value="0" Selected="True" />
            </Items>
        </dx:ASPxComboBox>
    </td>
    <td>
        <dx:ASPxTextBox ID="TxtFind2" runat="server" Width="200px" CssClass="font1">
        </dx:ASPxTextBox>
        <dx:ASPxDateEdit ID="TglFilter2" runat="server" CssClass="font1" 
            DisplayFormatString="dd-MMM-yyyy" Width="200px" 
            Theme="MetropolisBlue" Visible="False">
        </dx:ASPxDateEdit>
        <dx:ASPxTimeEdit ID="TimeFilter2" runat="server" Visible="false" 
            Theme="MetropolisBlue" EditFormat="Custom" EditFormatString="HH:mm">
        </dx:ASPxTimeEdit>
    </td>
</tr>
</table>

<table style="width: 100%">
<tr>
<td style="padding-top:10px">
    <dx:ASPxButton ID="BtnAdd" runat="server" Text="TAMBAH" 
        Theme="MetropolisBlue" TabIndex="5">
    </dx:ASPxButton>
</td>
</tr>
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
                <asp:BoundField DataField="TglMasuk" HeaderText="Tanggal Absensi" HeaderStyle-Width="120px" ItemStyle-Width = "120px" 
                ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd-MMM-yyyy}">                    
                </asp:BoundField>
                <asp:BoundField DataField="JamMasuk" HeaderText="Jam Masuk" HeaderStyle-Width="80px" ItemStyle-Width = "80px" DataFormatString="{0:hh\:mm}">                        
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="JamKeluar" HeaderText="Jam Pulang" HeaderStyle-Width="80px" ItemStyle-Width = "80px" DataFormatString="{0:hh\:mm}">
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-Width="120px" ItemStyle-Width = "120px">
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Keterangan" HeaderText="Keterangan" HeaderStyle-Width="200px" ItemStyle-Width = "200px">
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:ButtonField CommandName="BtnUpdate" Text="SELECT"  HeaderStyle-Width="45px" />  
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
</table>   

</div>

</asp:Content>
