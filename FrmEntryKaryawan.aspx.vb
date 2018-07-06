Imports System.IO
Imports DevExpress.Web

Public Class FrmEntryKaryawan
    Inherits System.Web.UI.Page
    Dim Conn As New SqlClient.SqlConnection
    Dim TmpDtKeluarga As New DataTable()
    Dim TmpDtPendidikan As New DataTable()
    Dim TmpDtKetrampilan As New DataTable()
    Dim TmpDtRwytPekerjaan As New DataTable()
    Dim TmpDtRwytPekerjaanMinarta As New DataTable()
    Dim TmpDtDP As New DataTable()
    Dim TmpDtDelFile As New DataTable()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("User") = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        Else
            If CheckAkses1(Session("User").ToString.Split("|")(1), "DataKaryawan") = False Then
                Response.Redirect("Default.aspx")
                Exit Sub
            End If
        End If

        Conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnStr").ToString
        Conn.Open()

        LblAction.Text = Session("Karyawan")
        Dim UpdateNIK As String = Session("NIK")

        If IsPostBack = False Then
            Call BindGridKeluarga()
            Call BindGridPendidikan()
            Call BindGridKetrampilan()
            Call BindGridRwytPekerjaan()
            Call BindGridRwytPekerjaanMinarta()
            Call BindGridDP()
            Call BindData()
            Call DelFile()

            If Session("Karyawan") = "UPD" Then
                PasFotoDefault.ImageUrl = Session("PasFoto").ToString
            ElseIf Session("Karyawan") = "NEW" And PasFoto.HasFile Then
                Dim FileName As String = TxtNIK.Text & "PasFoto.jpg"
                Session("PasFoto") = Server.MapPath("/PDF/Employee/" + TxtNIK.Text + "/" + FileName)
            Else
                Session("PasFoto") = "/Images/PasFotoDefault.jpg"
            End If

            PasFotoDefault.ImageUrl = Session("PasFoto").ToString
        End If
    End Sub
    Private Sub BindData()
        Dim UpdateNIK As String = Session("NIK")

        If LblAction.Text = "UPD" Then
            TxtNIK.Enabled = False
            Dim CmdDataHdr As New SqlClient.SqlCommand
            With CmdDataHdr
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT * FROM Karyawan where NIK=@P1"
                .Parameters.AddWithValue("@P1", UpdateNIK)
            End With
            Dim DataHdr As SqlClient.SqlDataReader = CmdDataHdr.ExecuteReader
            If DataHdr.Read Then
                TxtNIK.Text = DataHdr("NIK")
                TxtNama.Text = DataHdr("Nama")
                RblJenisKelamin.Value = DataHdr("Kelamin").ToString
                TxtTempatLahir.Text = DataHdr("TmpLahir").ToString
                TxtTanggalLahir.Text = If(IsDBNull(DataHdr("TglLahir")) = True, String.Empty, DataHdr("TglLahir"))
                TxtWN.Text = DataHdr("WN").ToString
                RblStsNikah.Value = DataHdr("StsNikah").ToString
                RblStsNikah_SelectedIndexChanged(RblStsNikah, New EventArgs())
                TxtTglNikah.Text = If(IsDBNull(DataHdr("TglNikah")) = True, String.Empty, DataHdr("TglNikah"))
                RblAgama.Value = DataHdr("Agama").ToString
                DDLDivisi.Value = DataHdr("Divisi").ToString
                TxtSubdivisi.Text = DataHdr("Subdivisi").ToString
                TxtJabatan.Text = DataHdr("Jabatan").ToString
                TxtGolongan.Text = DataHdr("Golongan").ToString
                TxtGrade.Text = DataHdr("Grade").ToString
                TxtPrdAwal.Text = If(IsDBNull(DataHdr("PrdAwal")) = True, String.Empty, DataHdr("PrdAwal"))
                TxtUraian.Text = DataHdr("UraianPekerjaan").ToString
                TxtAlamat.Text = DataHdr("Alamat").ToString
                TxtProvinsi.Text = DataHdr("Provinsi").ToString
                TxtKota.Text = DataHdr("Kota").ToString
                TxtAlamatSurat.Text = DataHdr("AlamatSurat").ToString
                TxtEmail.Text = DataHdr("Email").ToString
                TxtNoTelp.Text = DataHdr("NoTelp").ToString
                TxtLokasiKerja.Text = DataHdr("LokasiKerja").ToString
                Session("PasFoto") = DataHdr("Foto")
                If DataHdr("Active") = "0" Then
                    DisableControls(Form)
                End If
            End If
            CmdDataHdr.Dispose()
            DataHdr.Close()

            Dim CmdDataID As New SqlClient.SqlCommand
            With CmdDataID
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT * FROM EmpID where NIK=@P1"
                .Parameters.AddWithValue("@P1", UpdateNIK)
            End With
            Dim DataID As SqlClient.SqlDataReader = CmdDataID.ExecuteReader
            While DataID.Read
                If DataID("JenisID") = "KTP" Then
                    TxtNoKTP.Text = DataID("NoID")
                    TxtPrdAwalKTP.Text = If(IsDBNull(DataID("PrdAwal")) = True, String.Empty, DataID("PrdAwal"))
                    TxtPrdAkhirKTP.Text = If(IsDBNull(DataID("PrdAkhir")) = True, String.Empty, DataID("PrdAkhir"))
                    TxtDiterbitkanOlehKTP.Text = DataID("DiterbitkanOleh")
                End If
                If DataID("JenisID") = "Passport" Then
                    TxtNoPassport.Text = DataID("NoID")
                    TxtPrdAwalPassport.Text = If(IsDBNull(DataID("PrdAwal")) = True, String.Empty, DataID("PrdAwal"))
                    TxtPrdAkhirPassport.Text = If(IsDBNull(DataID("PrdAkhir")) = True, String.Empty, DataID("PrdAkhir"))
                    TxtDiterbitkanOlehPassport.Text = DataID("DiterbitkanOleh")
                End If
                If DataID("JenisID") = "NPWP" Then
                    TxtNoNPWP.Text = DataID("NoID")
                    TxtPrdAwalNPWP.Text = If(IsDBNull(DataID("PrdAwal")) = True, String.Empty, DataID("PrdAwal"))
                    TxtPrdAkhirNPWP.Text = If(IsDBNull(DataID("PrdAkhir")) = True, String.Empty, DataID("PrdAkhir"))
                    TxtDiterbitkanOlehNPWP.Text = DataID("DiterbitkanOleh")
                End If
                If DataID("JenisID") = "KK" Then
                    TxtNoKK.Text = DataID("NoID")
                    TxtPrdAwalKK.Text = If(IsDBNull(DataID("PrdAwal")) = True, String.Empty, DataID("PrdAwal"))
                    TxtPrdAkhirKK.Text = If(IsDBNull(DataID("PrdAkhir")) = True, String.Empty, DataID("PrdAkhir"))
                    TxtDiterbitkanOlehKK.Text = DataID("DiterbitkanOleh")
                End If
                If DataID("JenisID") = "SIM A" Then
                    TxtNoSIMA.Text = DataID("NoID")
                    TxtPrdAwalSIMA.Text = If(IsDBNull(DataID("PrdAwal")) = True, String.Empty, DataID("PrdAwal"))
                    TxtPrdAkhirSIMA.Text = If(IsDBNull(DataID("PrdAkhir")) = True, String.Empty, DataID("PrdAkhir"))
                    TxtDiterbitkanOlehSIMA.Text = DataID("DiterbitkanOleh")
                End If
                If DataID("JenisID") = "SIM B" Then
                    TxtNoSIMB.Text = DataID("NoID")
                    TxtPrdAwalSIMB.Text = If(IsDBNull(DataID("PrdAwal")) = True, String.Empty, DataID("PrdAwal"))
                    TxtPrdAkhirSIMB.Text = If(IsDBNull(DataID("PrdAkhir")) = True, String.Empty, DataID("PrdAkhir"))
                    TxtDiterbitkanOlehSIMB.Text = DataID("DiterbitkanOleh")
                End If
                If DataID("JenisID") = "SIM C" Then
                    TxtNoSIMC.Text = DataID("NoID")
                    TxtPrdAwalSIMC.Text = If(IsDBNull(DataID("PrdAwal")) = True, String.Empty, DataID("PrdAwal"))
                    TxtPrdAkhirSIMC.Text = If(IsDBNull(DataID("PrdAkhir")) = True, String.Empty, DataID("PrdAkhir"))
                    TxtDiterbitkanOlehSIMC.Text = DataID("DiterbitkanOleh")
                End If
            End While
            DataID.Close()
            CmdDataID.Dispose()

        End If

    End Sub
    Private Sub BindGridKeluarga()
        Dim UpdateNIK As String = Session("NIK")
        TmpDtKeluarga.Columns.AddRange(New DataColumn() {New DataColumn("NoUrutKeluarga", GetType(Integer)), _
                                                         New DataColumn("HubKeluarga", GetType(String)), _
                                                         New DataColumn("NamaKeluarga", GetType(String)), _
                                                         New DataColumn("JenisKelaminKeluarga", GetType(String)), _
                                                         New DataColumn("TglLahirKeluarga", GetType(Date)), _
                                                         New DataColumn("PekerjaanKeluarga", GetType(String)), _
                                                         New DataColumn("PerusahaanKeluarga", GetType(String))})

        If Session("Karyawan") = "UPD" Then
            Dim CmdDataKeluarga As New SqlClient.SqlCommand
            With CmdDataKeluarga
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT * FROM EmpKeluarga WHERE NIK=@P1 ORDER BY NoUrutKeluarga"
                .Parameters.AddWithValue("@P1", UpdateNIK)
            End With
            Dim DataKeluarga As SqlClient.SqlDataReader = CmdDataKeluarga.ExecuteReader
            While DataKeluarga.Read
                TmpDtKeluarga.Rows.Add(DataKeluarga("NoUrutKeluarga"), DataKeluarga("Hubungan"), DataKeluarga("Nama"), DataKeluarga("Kelamin"), DataKeluarga("TglLahir"), DataKeluarga("Pekerjaan"), DataKeluarga("Perusahaan"))
            End While
            DataKeluarga.Close()
            CmdDataKeluarga.Dispose()
        End If

        GridDataKeluarga.DataSource = TmpDtKeluarga
        Session("TmpDtKeluarga") = TmpDtKeluarga

        GridDataKeluarga.DataBind()
    End Sub
    Private Sub BindGridPendidikan()
        Dim UpdateNIK As String = Session("NIK")
        TmpDtPendidikan.Columns.AddRange(New DataColumn() {New DataColumn("NoUrutPendidikan", GetType(Integer)), _
                                                           New DataColumn("TgkPendidikan", GetType(String)), _
                                                           New DataColumn("PrdAwalPendidikan", GetType(Date)), _
                                                           New DataColumn("PrdAkhirPendidikan", GetType(Date)), _
                                                           New DataColumn("InstitusiPendidikan", GetType(String)), _
                                                           New DataColumn("AlamatInstitusiPendidikan", GetType(String)), _
                                                           New DataColumn("JurusanPendidikan", GetType(String)), _
                                                           New DataColumn("LlsTdkLlsPendidikan", GetType(String)), _
                                                           New DataColumn("NilaiPendidikan", GetType(String)), _
                                                           New DataColumn("NoIjazahPendidikan", GetType(String))})
        If Session("Karyawan") = "UPD" Then
            Dim CmdDataPendidikan As New SqlClient.SqlCommand
            With CmdDataPendidikan
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT * FROM EmpPendidikan WHERE NIK=@P1 ORDER BY NoUrutPendidikan"
                .Parameters.AddWithValue("@P1", UpdateNIK)
            End With
            Dim DataPendidikan As SqlClient.SqlDataReader = CmdDataPendidikan.ExecuteReader
            While DataPendidikan.Read
                TmpDtPendidikan.Rows.Add(DataPendidikan("NoUrutPendidikan"), DataPendidikan("TgkPendidikan"), _
                                         DataPendidikan("PrdAwal"), DataPendidikan("PrdAkhir"), _
                                         DataPendidikan("Institusi"), DataPendidikan("Alamat"), _
                                         DataPendidikan("Jurusan"), DataPendidikan("LlsTdkLls"), _
                                         DataPendidikan("IPK"), DataPendidikan("NoIjazah"))
            End While
            DataPendidikan.Close()
            CmdDataPendidikan.Dispose()
        End If
        GridDataPendidikan.DataSource = TmpDtPendidikan
        Session("TmpDtPendidikan") = TmpDtPendidikan

        GridDataPendidikan.DataBind()
    End Sub
    Private Sub BindGridKetrampilan()
        Dim UpdateNIK As String = Session("NIK")
        TmpDtKetrampilan.Columns.AddRange(New DataColumn() {New DataColumn("NoUrutKetrampilan", GetType(Integer)), _
                                                            New DataColumn("NamaKetrampilan", GetType(String)), _
                                                            New DataColumn("PrdAwalKetrampilan", GetType(Date)), _
                                                            New DataColumn("PrdAkhirKetrampilan", GetType(Date)), _
                                                            New DataColumn("NamaSertifikatKetrampilan", GetType(String)), _
                                                            New DataColumn("GradeKetrampilan", GetType(String)), _
                                                            New DataColumn("NoSertifikatKetrampilan", GetType(String)), _
                                                            New DataColumn("InstitusiKetrampilan", GetType(String))})
        If Session("Karyawan") = "UPD" Then
            Dim CmdDataKetrampilan As New SqlClient.SqlCommand
            With CmdDataKetrampilan
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT * FROM EmpKetrampilan WHERE NIK=@P1 ORDER BY NoUrutKetrampilan"
                .Parameters.AddWithValue("@P1", UpdateNIK)
            End With
            Dim DataKetrampilan As SqlClient.SqlDataReader = CmdDataKetrampilan.ExecuteReader
            While DataKetrampilan.Read
                TmpDtKetrampilan.Rows.Add(DataKetrampilan("NoUrutKetrampilan"), DataKetrampilan("Nama"), _
                                          DataKetrampilan("PrdAwal"), DataKetrampilan("PrdAkhir"), _
                                          DataKetrampilan("Sertifikat"), DataKetrampilan("Grade"), _
                                          DataKetrampilan("NoSertifikat"), DataKetrampilan("Institusi"))
            End While
            DataKetrampilan.Close()
            CmdDataKetrampilan.Dispose()
        End If
        GridDataKetrampilan.DataSource = TmpDtKetrampilan
        Session("TmpDtKetrampilan") = TmpDtKetrampilan

        GridDataKetrampilan.DataBind()
    End Sub
    Private Sub BindGridRwytPekerjaan()
        Dim UpdateNIK As String = Session("NIK")
        TmpDtRwytPekerjaan.Columns.AddRange(New DataColumn() {New DataColumn("NoUrutRwytPekerjaan", GetType(Integer)), _
                                                              New DataColumn("PrdAwalRwytPekerjaan", GetType(Date)), _
                                                              New DataColumn("PrdAkhirRwytPekerjaan", GetType(Date)), _
                                                              New DataColumn("PerusahaanRwytPekerjaan", GetType(String)), _
                                                              New DataColumn("AlamatRwytPekerjaan", GetType(String)), _
                                                              New DataColumn("IndustriRwytPekerjaan", GetType(String)), _
                                                              New DataColumn("JabatanRwytPekerjaan", GetType(String)), _
                                                              New DataColumn("LokasiKerjaRwytPekerjaan", GetType(String)), _
                                                              New DataColumn("GajiRwytPekerjaan", GetType(String)), _
                                                              New DataColumn("TunjanganRwytPekerjaan", GetType(String)), _
                                                              New DataColumn("UraianRwytPekerjaan", GetType(String))})
        If Session("Karyawan") = "UPD" Then
            Dim CmdDataRwytPekerjaan As New SqlClient.SqlCommand
            With CmdDataRwytPekerjaan
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT * FROM EmpPekerjaanH WHERE NIK=@P1 ORDER BY NoUrutRwytPekerjaan"
                .Parameters.AddWithValue("@P1", UpdateNIK)
            End With
            Dim DataRwytPekerjaan As SqlClient.SqlDataReader = CmdDataRwytPekerjaan.ExecuteReader
            While DataRwytPekerjaan.Read
                TmpDtRwytPekerjaan.Rows.Add(DataRwytPekerjaan("NoUrutRwytPekerjaan"), DataRwytPekerjaan("PrdAwal"), _
                                            DataRwytPekerjaan("PrdAkhir"), DataRwytPekerjaan("Perusahaan"), _
                                            DataRwytPekerjaan("Alamat"), DataRwytPekerjaan("Industri"), _
                                            DataRwytPekerjaan("Jabatan"), DataRwytPekerjaan("LokasiKerja"), _
                                            Format(DataRwytPekerjaan("GajiPokok"), "N0"), DataRwytPekerjaan("Tunjangan"), _
                                            DataRwytPekerjaan("UraianPekerjaan"))
            End While
            DataRwytPekerjaan.Close()
            CmdDataRwytPekerjaan.Dispose()
        End If
        GridDataRwytPekerjaan.DataSource = TmpDtRwytPekerjaan
        Session("TmpDtRwytPekerjaan") = TmpDtRwytPekerjaan

        GridDataRwytPekerjaan.DataBind()
    End Sub
    Private Sub BindGridRwytPekerjaanMinarta()
        Dim UpdateNIK As String = Session("NIK")
        TmpDtRwytPekerjaanMinarta.Columns.AddRange(New DataColumn() {New DataColumn("NoUrutRwytPekerjaanMinarta", GetType(Integer)), _
                                                                     New DataColumn("PrdAwalRwytPekerjaanMinarta", GetType(Date)), _
                                                                     New DataColumn("PrdAkhirRwytPekerjaanMinarta", GetType(Date)), _
                                                                     New DataColumn("DivisiRwytPekerjaanMinarta", GetType(String)), _
                                                                     New DataColumn("SubdivisiRwytPekerjaanMinarta", GetType(String)), _
                                                                     New DataColumn("LokasiKerjaRwytPekerjaanMinarta", GetType(String)), _
                                                                     New DataColumn("JabatanRwytPekerjaanMinarta", GetType(String)), _
                                                                     New DataColumn("GolonganRwytPekerjaanMinarta", GetType(String)), _
                                                                     New DataColumn("GradeRwytPekerjaanMinarta", GetType(String)), _
                                                                     New DataColumn("GajiRwytPekerjaanMinarta", GetType(String)), _
                                                                     New DataColumn("TunjanganRwytPekerjaanMinarta", GetType(String)), _
                                                                     New DataColumn("KPIRwytPekerjaanMinarta", GetType(String)), _
                                                                     New DataColumn("TesKesehatanRwytPekerjaanMinarta", GetType(String)), _
                                                                     New DataColumn("HasilKesehatanRwytPekerjaanMinarta", GetType(String)), _
                                                                     New DataColumn("TesPsikologiRwytPekerjaanMinarta", GetType(String)), _
                                                                     New DataColumn("HasilPsikologiRwytPekerjaanMinarta", GetType(String)), _
                                                                     New DataColumn("AtasanRwytPekerjaanMinarta", GetType(String)), _
                                                                     New DataColumn("UraianRwytPekerjaanMinarta", GetType(String))})
        If Session("Karyawan") = "UPD" Then
            Dim CmdDataRwytPekerjaanMinarta As New SqlClient.SqlCommand
            With CmdDataRwytPekerjaanMinarta
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT * FROM EmpPekerjaanMinarta WHERE NIK=@P1 ORDER BY NoUrutRwytPekerjaanMinarta"
                .Parameters.AddWithValue("@P1", UpdateNIK)
            End With
            Dim DataRwytPekerjaanMinarta As SqlClient.SqlDataReader = CmdDataRwytPekerjaanMinarta.ExecuteReader
            While DataRwytPekerjaanMinarta.Read
                TmpDtRwytPekerjaanMinarta.Rows.Add(DataRwytPekerjaanMinarta("NoUrutRwytPekerjaanMinarta"), _
                                                   DataRwytPekerjaanMinarta("PrdAwal"), _
                                                   DataRwytPekerjaanMinarta("PrdAkhir"), _
                                                   DataRwytPekerjaanMinarta("Divisi"), _
                                                   DataRwytPekerjaanMinarta("Subdivisi"), _
                                                   DataRwytPekerjaanMinarta("LokasiKerja"), _
                                                   DataRwytPekerjaanMinarta("Jabatan"), _
                                                   DataRwytPekerjaanMinarta("Golongan"), _
                                                   DataRwytPekerjaanMinarta("Grade"), _
                                                   Format(DataRwytPekerjaanMinarta("GajiPokok"), "N0"), _
                                                   DataRwytPekerjaanMinarta("Tunjangan"), _
                                                   DataRwytPekerjaanMinarta("KPI"), _
                                                   DataRwytPekerjaanMinarta("TesKesehatan"), _
                                                   DataRwytPekerjaanMinarta("HslKesehatan"), _
                                                   DataRwytPekerjaanMinarta("TesPsikologi"), _
                                                   DataRwytPekerjaanMinarta("HslPsikologi"), _
                                                   DataRwytPekerjaanMinarta("Atasan"), _
                                                   DataRwytPekerjaanMinarta("UraianPekerjaan"))
            End While
            DataRwytPekerjaanMinarta.Close()
            CmdDataRwytPekerjaanMinarta.Dispose()
        End If
        GridDataRwytPekerjaanMinarta.DataSource = TmpDtRwytPekerjaanMinarta
        Session("TmpDtRwytPekerjaanMinarta") = TmpDtRwytPekerjaanMinarta

        GridDataRwytPekerjaanMinarta.DataBind()
    End Sub

    Protected Sub RblStsNikah_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RblStsNikah.SelectedIndexChanged
        If RblStsNikah.Value <> String.Empty Then
            If RblStsNikah.SelectedItem.Value = "Menikah" Then
                TxtTglNikah.Visible = True
            Else
                TxtTglNikah.Visible = False
            End If
        End If
    End Sub

    Protected Sub BtnPopUpKeluarga_Click(sender As Object, e As EventArgs) Handles BtnPopUpKeluarga.Click
        TxtActKeluarga.Text = "NEW"
        TxtNoUrutKeluarga.Text = ""
        TxtHubKeluarga.Text = ""
        TxtNamaKeluarga.Text = ""
        TxtJenisKelaminKeluarga.Text = ""
        TxtTglLahirKeluarga.Text = ""
        TxtPekerjaanKeluarga.Text = ""
        TxtPerusahaanKeluarga.Text = ""
        PopEntKeluarga.ShowOnPageLoad = True
    End Sub
    Protected Sub BtnPopUpPendidikan_Click(sender As Object, e As EventArgs) Handles BtnPopUpPendidikan.Click
        TxtActPendidikan.Text = "NEW"
        TxtNoUrutPendidikan.Text = ""
        TxtTgkPendidikan.Text = ""
        TxtPrdAwalPendidikan.Text = ""
        TxtPrdAkhirPendidikan.Text = ""
        TxtInstitusiPendidikan.Text = ""
        TxtAlamatInstitusiPendidikan.Text = ""
        TxtJurusanPendidikan.Text = ""
        'TxtLlsTdkLlsPendidikan.Text = ""
        TxtNilaiPendidikan.Text = ""
        TxtNoIjazahPendidikan.Text = ""
        PopEntPendidikan.ShowOnPageLoad = True
    End Sub
    Protected Sub BtnPopUpKetrampilan_Click(sender As Object, e As EventArgs) Handles BtnPopUpKetrampilan.Click
        TxtActKetrampilan.Text = "NEW"
        TxtNoUrutKetrampilan.Text = ""
        TxtNamaKetrampilan.Text = ""
        TxtPrdAwalKetrampilan.Text = ""
        TxtPrdAkhirKetrampilan.Text = ""
        TxtNamaSertifikatKetrampilan.Text = ""
        TxtGradeKetrampilan.Text = ""
        TxtNoSertifikatKetrampilan.Text = ""
        TxtInstitusiKetrampilan.Text = ""
        PopEntKetrampilan.ShowOnPageLoad = True
    End Sub
    Protected Sub BtnPopUpRwytPekerjaan_Click(sender As Object, e As EventArgs) Handles BtnPopUpRwytPekerjaan.Click
        TxtActRwytPekerjaan.Text = "NEW"
        TxtNoUrutRwytPekerjaan.Text = ""
        TxtPrdAwalRwytPekerjaan.Text = ""
        TxtPrdAkhirRwytPekerjaan.Text = ""
        TxtPerusahaanRwytPekerjaan.Text = ""
        TxtAlamatRwytPekerjaan.Text = ""
        TxtIndustriRwytPekerjaan.Text = ""
        TxtJabatanRwytPekerjaan.Text = ""
        TxtLokasiKerjaRwytPekerjaan.Text = ""
        TxtGajiRwytPekerjaan.Text = ""
        TxtTunjanganRwytPekerjaan.Text = ""
        TxtUraianRwytPekerjaan.Text = ""
        PopEntRwytPekerjaan.ShowOnPageLoad = True
    End Sub
    Protected Sub BtnPopUpRwytPekerjaanMinarta_Click(sender As Object, e As EventArgs) Handles BtnPopUpRwytPekerjaanMinarta.Click
        TxtActRwytPekerjaanMinarta.Text = "NEW"
        TxtNoUrutRwytPekerjaanMinarta.Text = ""
        TxtPrdAwalRwytPekerjaanMinarta.Text = ""
        TxtPrdAkhirRwytPekerjaanMinarta.Text = ""
        TxtDivisiRwytPekerjaanMinarta.Text = ""
        TxtSubdivisiRwytPekerjaanMinarta.Text = ""
        TxtLokasiKerjaRwytPekerjaanMinarta.Text = ""
        TxtJabatanRwytPekerjaanMinarta.Text = ""
        TxtGolonganRwytPekerjaanMinarta.Text = ""
        TxtGradeRwytPekerjaanMinarta.Text = ""
        TxtGajiRwytPekerjaanMinarta.Text = ""
        TxtTunjanganRwytPekerjaanMinarta.Text = ""
        TxtKPIRwytPekerjaanMinarta.Text = ""
        'TxtTesKesehatanRwytPekerjaanMinarta.Text = ""
        TxtHasilKesehatanRwytPekerjaanMinarta.Text = ""
        'TxtTesPsikologiRwytPekerjaanMinarta.Text = ""
        TxtHasilPsikologiRwytPekerjaanMinarta.Text = ""
        TxtAtasanRwytPekerjaanMinarta.Text = ""
        TxtUraianRwytPekerjaanMinarta.Text = ""
        PopEntRwytPekerjaanMinarta.ShowOnPageLoad = True
    End Sub

    Protected Sub BtnSaveKeluarga_Click(sender As Object, e As EventArgs) Handles BtnSaveKeluarga.Click
        'If TxtHubKeluarga.Value = "" Then
        '    LblErr.Text = "Hubungan Keluarga belum di-pilih."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If
        'If TxtNamaKeluarga.Text = "" Then
        '    LblErr.Text = "Nama Keluarga belum di-isi."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If
        'If TxtJenisKelaminKeluarga.Value = "" Then
        '    LblErr.Text = "Jenis Kelamin belum di-pilih."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If
        'If TxtTglLahirKeluarga.Text = "" Then
        '    LblErr.Text = "Tanggal Lahir belum di-pilih."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If

        TmpDtKeluarga = Session("TmpDtKeluarga")

        If TxtActKeluarga.Text = "NEW" Then
            Dim Counter As Integer
            Dim Result As DataRow = TmpDtKeluarga.Select("NoUrutKeluarga > 0", "NoUrutKeluarga DESC").FirstOrDefault
            If Result Is Nothing Then
                Counter = 1
            Else
                Counter = Result("NoUrutKeluarga") + 1
            End If
            TmpDtKeluarga.Rows.Add(Counter, TxtHubKeluarga.Value, TxtNamaKeluarga.Text, TxtJenisKelaminKeluarga.Value, TxtTglLahirKeluarga.Text, TxtPekerjaanKeluarga.Text, TxtPerusahaanKeluarga.Text)
        Else
            Dim Result As DataRow = TmpDtKeluarga.Select("NoUrutKeluarga='" & TxtNoUrutKeluarga.Text & "'").FirstOrDefault
            If Result IsNot Nothing Then
                Result("HubKeluarga") = TxtHubKeluarga.Value
                Result("NamaKeluarga") = TxtNamaKeluarga.Text
                Result("JenisKelaminKeluarga") = TxtJenisKelaminKeluarga.Value
                Result("TglLahirKeluarga") = TxtTglLahirKeluarga.Text
                Result("PekerjaanKeluarga") = TxtPekerjaanKeluarga.Text
                Result("PerusahaanKeluarga") = TxtPerusahaanKeluarga.Text
            End If
        End If

        GridDataKeluarga.DataSource = TmpDtKeluarga
        GridDataKeluarga.DataBind()

        Session("TmpDtKeluarga") = TmpDtKeluarga
        PopEntKeluarga.ShowOnPageLoad = False
    End Sub
    Protected Sub BtnSavePendidikan_Click(sender As Object, e As EventArgs) Handles BtnSavePendidikan.Click
        'If TxtTgkPendidikan.Value = "" Then
        '    LblErr.Text = "Tingkat Pendidikan belum di-pilih."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If
        'If TxtPrdAwalPendidikan.Text = "" Then
        '    LblErr.Text = "Tanggal Dimulai belum di-isi."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If
        'If TxtInstitusiPendidikan.Text = "" Then
        '    LblErr.Text = "Nama Institusi belum di-isi."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If
        'If TxtAlamatInstitusiPendidikan.Text = "" Then
        '    LblErr.Text = "Alamat Institusi belum di-isi."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If

        TmpDtPendidikan = Session("TmpDtPendidikan")

        If TxtActPendidikan.Text = "NEW" Then
            Dim Counter As Integer
            Dim Result As DataRow = TmpDtPendidikan.Select("NoUrutPendidikan > 0", "NoUrutPendidikan DESC").FirstOrDefault
            If Result Is Nothing Then
                Counter = 1
            Else
                Counter = Result("NoUrutPendidikan") + 1
            End If
            TmpDtPendidikan.Rows.Add(Counter, TxtTgkPendidikan.Value, TxtPrdAwalPendidikan.Date, _
                                     If(TxtPrdAkhirPendidikan.Text = String.Empty, DBNull.Value, TxtPrdAkhirPendidikan.Date), _
                                     TxtInstitusiPendidikan.Text, TxtAlamatInstitusiPendidikan.Text, TxtJurusanPendidikan.Text, _
                                     TxtLlsTdkLlsPendidikan.Value, TxtNilaiPendidikan.Text, TxtNoIjazahPendidikan.Text)
        Else
            Dim Result As DataRow = TmpDtPendidikan.Select("NoUrutPendidikan='" & TxtNoUrutPendidikan.Text & "'").FirstOrDefault
            If Result IsNot Nothing Then
                Result("TgkPendidikan") = TxtTgkPendidikan.Value
                Result("PrdAwalPendidikan") = TxtPrdAwalPendidikan.Date
                Result("PrdAkhirPendidikan") = If(TxtPrdAkhirPendidikan.Text = String.Empty, DBNull.Value, TxtPrdAkhirPendidikan.Date)
                Result("InstitusiPendidikan") = TxtInstitusiPendidikan.Text
                Result("AlamatInstitusiPendidikan") = TxtAlamatInstitusiPendidikan.Text
                Result("JurusanPendidikan") = TxtJurusanPendidikan.Text
                Result("LlsTdkLlsPendidikan") = TxtLlsTdkLlsPendidikan.Value
                Result("NilaiPendidikan") = TxtNilaiPendidikan.Text
                Result("NoIjazahPendidikan") = TxtNoIjazahPendidikan.Text
            End If
        End If

        GridDataPendidikan.DataSource = TmpDtPendidikan
        GridDataPendidikan.DataBind()

        Session("TmpDtPendidikan") = TmpDtPendidikan
        PopEntPendidikan.ShowOnPageLoad = False
    End Sub
    Protected Sub BtnSaveKetrampilan_Click(sender As Object, e As EventArgs) Handles BtnSaveKetrampilan.Click
        'If TxtNamaKetrampilan.Text = "" Then
        '    LblErr.Text = "Nama Ketramplan belum di-isi."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If
        'If TxtPrdAwalKetrampilan.Text = "" Then
        '    LblErr.Text = "Tanggal Dimulai belum di-isi."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If
        'If TxtNamaSertifikatKetrampilan.Text = "" Then
        '    LblErr.Text = "Nama Sertifikat belum di-isi."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If
        'If TxtGradeKetrampilan.Text = "" Then
        '    LblErr.Text = "Level/Grade Ketrampilan belum di-isi."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If
        'If TxtInstitusiKetrampilan.Text = "" Then
        '    LblErr.Text = "Nama Institusi belum di-isi."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If

        TmpDtKetrampilan = Session("TmpDtKetrampilan")

        If TxtActKetrampilan.Text = "NEW" Then
            Dim Counter As Integer
            Dim Result As DataRow = TmpDtKetrampilan.Select("NoUrutKetrampilan > 0", "NoUrutKetrampilan DESC").FirstOrDefault
            If Result Is Nothing Then
                Counter = 1
            Else
                Counter = Result("NoUrutKetrampilan") + 1
            End If
            TmpDtKetrampilan.Rows.Add(Counter, TxtNamaKetrampilan.Text, TxtPrdAwalKetrampilan.Date, _
                                      If(TxtPrdAkhirKetrampilan.Text = String.Empty, DBNull.Value, TxtPrdAkhirKetrampilan.Date), _
                                      TxtNamaSertifikatKetrampilan.Text, TxtGradeKetrampilan.Text, TxtNoSertifikatKetrampilan.Text, TxtInstitusiKetrampilan.Text)
        Else
            Dim Result As DataRow = TmpDtPendidikan.Select("NoUrutKetrampilan='" & TxtNoUrutKetrampilan.Text & "'").FirstOrDefault
            If Result IsNot Nothing Then
                Result("NamaKetrampilan") = TxtNamaKetrampilan.Text
                Result("PrdAwalKetrampilan") = TxtPrdAwalKetrampilan.Date
                Result("PrdAkhirKetrampilan") = If(TxtPrdAkhirKetrampilan.Text = String.Empty, DBNull.Value, TxtPrdAkhirKetrampilan.Date)
                Result("NamaSertifikatKetrampilan") = TxtNamaSertifikatKetrampilan.Text
                Result("GradeKetrampilan") = TxtGradeKetrampilan.Text
                Result("NoSertifikatKetrampilan") = TxtNoSertifikatKetrampilan.Text
                Result("InstitusiKetrampilan") = TxtInstitusiKetrampilan.Text
            End If
        End If

        GridDataKetrampilan.DataSource = TmpDtKetrampilan
        GridDataKetrampilan.DataBind()

        Session("TmpDtKetrampilan") = TmpDtKetrampilan
        PopEntKetrampilan.ShowOnPageLoad = False
    End Sub
    Protected Sub BtnSaveRwytPekerjaan_Click(sender As Object, e As EventArgs) Handles BtnSaveRwytPekerjaan.Click
        'If TxtPrdAwalRwytPekerjaan.Text = "" Then
        '    LblErr.Text = "Tanggal Dimulai belum di-isi."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If
        'If TxtPrdAkhirRwytPekerjaan.Text = "" Then
        '    LblErr.Text = "Tanggal Berakhir belum di-isi."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If
        'If TxtPerusahaanRwytPekerjaan.Text = "" Then
        '    LblErr.Text = "Nama Perusahaan belum di-isi."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If
        'If TxtAlamatRwytPekerjaan.Text = "" Then
        '    LblErr.Text = "Alamat Perusahaan belum di-isi."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If
        'If TxtIndustriRwytPekerjaan.Text = "" Then
        '    LblErr.Text = "Bidang Industri belum di-isi."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If
        'If TxtJabatanRwytPekerjaan.Text = "" Then
        '    LblErr.Text = "Jabatan belum di-isi."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If
        'If TxtLokasiKerjaRwytPekerjaan.Text = "" Then
        '    LblErr.Text = "Lokasi Kerja belum di-isi."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If
        'If TxtGajiRwytPekerjaan.Text = "" Then
        '    LblErr.Text = "Gaji Pokok belum di-isi."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If
        'If TxtUraianRwytPekerjaan.Text = "" Then
        '    LblErr.Text = "Uraian Pekerjaan belum di-isi."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If

        TmpDtRwytPekerjaan = Session("TmpDtRwytPekerjaan")

        If TxtActRwytPekerjaan.Text = "NEW" Then
            Dim Counter As Integer
            Dim Result As DataRow = TmpDtRwytPekerjaan.Select("NoUrutRwytPekerjaan > 0", "NoUrutRwytPekerjaan DESC").FirstOrDefault
            If Result Is Nothing Then
                Counter = 1
            Else
                Counter = Result("NoUrutRwytPekerjaan") + 1
            End If
            TmpDtRwytPekerjaan.Rows.Add(Counter, TxtPrdAwalRwytPekerjaan.Date, TxtPrdAkhirRwytPekerjaan.Date, _
                                        TxtPerusahaanRwytPekerjaan.Text, TxtAlamatRwytPekerjaan.Text, TxtIndustriRwytPekerjaan.Text, _
                                        TxtJabatanRwytPekerjaan.Text, TxtLokasiKerjaRwytPekerjaan.Text, TxtGajiRwytPekerjaan.Text, _
                                        TxtTunjanganRwytPekerjaan.Text, TxtUraianRwytPekerjaan.Text)
        Else
            Dim Result As DataRow = TmpDtRwytPekerjaan.Select("NoUrutRwytPekerjaan='" & TxtNoUrutRwytPekerjaan.Text & "'").FirstOrDefault
            If Result IsNot Nothing Then
                Result("PrdAwalRwytPekerjaan") = TxtPrdAwalRwytPekerjaan.Date
                Result("PrdAkhirRwytPekerjaan") = TxtPrdAkhirRwytPekerjaan.Date
                Result("PerusahaanRwytPekerjaan") = TxtPerusahaanRwytPekerjaan.Text
                Result("AlamatRwytPekerjaan") = TxtAlamatRwytPekerjaan.Text
                Result("IndustriRwytPekerjaan") = TxtIndustriRwytPekerjaan.Text
                Result("JabatanRwytPekerjaan") = TxtJabatanRwytPekerjaan.Text
                Result("LokasiKerjaRwytPekerjaan") = TxtLokasiKerjaRwytPekerjaan.Text
                Result("GajiRwytPekerjaan") = TxtGajiRwytPekerjaan.Text
                Result("TunjanganRwytPekerjaan") = TxtTunjanganRwytPekerjaan.Text
                Result("UraianRwytPekerjaan") = TxtUraianRwytPekerjaan.Text
            End If
        End If

        GridDataRwytPekerjaan.DataSource = TmpDtRwytPekerjaan
        GridDataRwytPekerjaan.DataBind()

        Session("TmpDtRwytPekerjaan") = TmpDtRwytPekerjaan
        PopEntRwytPekerjaan.ShowOnPageLoad = False
    End Sub
    Protected Sub BtnSaveRwytPekerjaanMinarta_Click(sender As Object, e As EventArgs) Handles BtnSaveRwytPekerjaanMinarta.Click
        If TxtPrdAwalRwytPekerjaanMinarta.Text = "" Then
            LblErr.Text = "Tanggal Dimulai belum di-isi."
            ErrMsg.ShowOnPageLoad = True
            Exit Sub
        End If
        If TxtPrdAkhirRwytPekerjaanMinarta.Text = "" Then
            LblErr.Text = "Tanggal Berakhir belum di-isi."
            ErrMsg.ShowOnPageLoad = True
            Exit Sub
        End If
        If TxtDivisiRwytPekerjaanMinarta.Text = "" Then
            LblErr.Text = "Divisi belum di-isi."
            ErrMsg.ShowOnPageLoad = True
            Exit Sub
        End If
        If TxtSubdivisiRwytPekerjaanMinarta.Text = "" Then
            LblErr.Text = "Subdivisi belum di-isi."
            ErrMsg.ShowOnPageLoad = True
            Exit Sub
        End If
        If TxtLokasiKerjaRwytPekerjaanMinarta.Text = "" Then
            LblErr.Text = "Lokasi Kerja belum di-isi."
            ErrMsg.ShowOnPageLoad = True
            Exit Sub
        End If
        If TxtJabatanRwytPekerjaanMinarta.Text = "" Then
            LblErr.Text = "Jabatan belum di-isi."
            ErrMsg.ShowOnPageLoad = True
            Exit Sub
        End If
        If TxtGolonganRwytPekerjaanMinarta.Text = "" Then
            LblErr.Text = "Golongan Kerja belum di-isi."
            ErrMsg.ShowOnPageLoad = True
            Exit Sub
        End If
        If TxtGradeRwytPekerjaanMinarta.Text = "" Then
            LblErr.Text = "Grade belum di-isi."
            ErrMsg.ShowOnPageLoad = True
            Exit Sub
        End If
        If TxtGajiRwytPekerjaanMinarta.Text = "" Then
            LblErr.Text = "Gaji Pokok belum di-isi."
            ErrMsg.ShowOnPageLoad = True
            Exit Sub
        End If
        If TxtAtasanRwytPekerjaanMinarta.Text = "" Then
            LblErr.Text = "Atasan belum di-isi."
            ErrMsg.ShowOnPageLoad = True
            Exit Sub
        End If
        If TxtUraianRwytPekerjaanMinarta.Text = "" Then
            LblErr.Text = "Uraian Pekerjaan belum di-isi."
            ErrMsg.ShowOnPageLoad = True
            Exit Sub
        End If

        TmpDtRwytPekerjaanMinarta = Session("TmpDtRwytPekerjaanMinarta")

        If TxtActRwytPekerjaanMinarta.Text = "NEW" Then
            Dim Counter As Integer
            Dim Result As DataRow = TmpDtRwytPekerjaanMinarta.Select("NoUrutRwytPekerjaanMinarta > 0", "NoUrutRwytPekerjaanMinarta DESC").FirstOrDefault
            If Result Is Nothing Then
                Counter = 1
            Else
                Counter = Result("NoUrutRwytPekerjaanMinarta") + 1
            End If
            TmpDtRwytPekerjaanMinarta.Rows.Add(Counter, TxtPrdAwalRwytPekerjaanMinarta.Date, TxtPrdAkhirRwytPekerjaanMinarta.Date, _
                                               TxtDivisiRwytPekerjaanMinarta.Text, TxtSubdivisiRwytPekerjaanMinarta.Text, _
                                               TxtLokasiKerjaRwytPekerjaanMinarta.Text, TxtJabatanRwytPekerjaanMinarta.Text, _
                                               TxtGolonganRwytPekerjaanMinarta.Text, TxtGradeRwytPekerjaanMinarta.Text, TxtGajiRwytPekerjaanMinarta.Text, _
                                               TxtTunjanganRwytPekerjaanMinarta.Text, TxtKPIRwytPekerjaanMinarta.Text, TxtTesKesehatanRwytPekerjaanMinarta.Text, _
                                               TxtHasilKesehatanRwytPekerjaanMinarta.Text, TxtTesPsikologiRwytPekerjaanMinarta.Text, _
                                               TxtHasilPsikologiRwytPekerjaanMinarta.Text, TxtAtasanRwytPekerjaanMinarta.Text, TxtUraianRwytPekerjaanMinarta.Text)
        Else
            Dim Result As DataRow = TmpDtRwytPekerjaanMinarta.Select("NoUrutRwytPekerjaanMinarta='" & TxtNoUrutRwytPekerjaanMinarta.Text & "'").FirstOrDefault
            If Result IsNot Nothing Then
                Result("PrdAwalRwytPekerjaanMinarta") = TxtPrdAwalRwytPekerjaanMinarta.Date
                Result("PrdAkhirRwytPekerjaanMinarta") = TxtPrdAkhirRwytPekerjaanMinarta.Date
                Result("DivisiRwytPekerjaanMinarta") = TxtDivisiRwytPekerjaanMinarta.Text
                Result("SubdivisiRwytPekerjaanMinarta") = TxtSubdivisiRwytPekerjaanMinarta.Text
                Result("LokasiKerjaRwytPekerjaanMinarta") = TxtLokasiKerjaRwytPekerjaanMinarta.Text
                Result("JabatanRwytPekerjaanMinarta") = TxtJabatanRwytPekerjaanMinarta.Text
                Result("GolonganRwytPekerjaanMinarta") = TxtGolonganRwytPekerjaanMinarta.Text
                Result("GradeRwytPekerjaanMinarta") = TxtGradeRwytPekerjaanMinarta.Text
                Result("GajiRwytPekerjaanMinarta") = TxtGajiRwytPekerjaanMinarta.Text
                Result("TunjanganRwytPekerjaanMinarta") = TxtTunjanganRwytPekerjaanMinarta.Text
                Result("KPIRwytPekerjaanMinarta") = TxtKPIRwytPekerjaanMinarta.Text
                Result("TesKesehatanRwytPekerjaanMinarta") = TxtTesKesehatanRwytPekerjaanMinarta.Text
                Result("HasilKesehatanRwytPekerjaanMinarta") = TxtHasilKesehatanRwytPekerjaanMinarta.Text
                Result("TesPsikologiRwytPekerjaanMinarta") = TxtTesPsikologiRwytPekerjaanMinarta.Text
                Result("HasilPsikologiRwytPekerjaanMinarta") = TxtTesPsikologiRwytPekerjaanMinarta.Text
                Result("AtasanRwytPekerjaanMinarta") = TxtAtasanRwytPekerjaanMinarta.Text
                Result("UraianRwytPekerjaanMinarta") = TxtUraianRwytPekerjaanMinarta.Text
            End If
        End If

        GridDataRwytPekerjaanMinarta.DataSource = TmpDtRwytPekerjaanMinarta
        GridDataRwytPekerjaanMinarta.DataBind()

        Session("TmpDtRwytPekerjaanMinarta") = TmpDtRwytPekerjaanMinarta
        PopEntRwytPekerjaanMinarta.ShowOnPageLoad = False
    End Sub

    Protected Sub BtnSaveDataEntry_Click(sender As Object, e As System.EventArgs) Handles BtnSaveDataEntry.Click        
        If PasFoto.HasFile Then
            If PasFoto.PostedFile.ContentType.ToLower <> "image/jpeg" Then
                LblErr.Text = "Pas Foto hanya mendukung file dengan ext. JPG/JPEG."
                ErrMsg.ShowOnPageLoad = True
                Exit Sub
            End If
            If PasFoto.PostedFile.ContentLength >= 2000000 Then
                LblErr.Text = "Ukuran file foto maximum 2MB"
                ErrMsg.ShowOnPageLoad = True
                Exit Sub
            End If
        End If
        If FileUpload.HasFile Then
            For Ctr As Integer = 0 To Request.Files.Count - 1
                Dim postedFile As HttpPostedFile = Request.Files(Ctr)
                If postedFile.ContentLength > 0 Then
                    If postedFile.ContentType.ToLower <> "image/jpeg" And _
                    postedFile.ContentType.ToLower <> "application/pdf" Then
                        LblErr.Text = "Pastikan file yang anda upload memiliki ext. JPG/PDF."
                        ErrMsg.ShowOnPageLoad = True
                        Exit Sub
                    End If
                End If
                If postedFile.ContentType.ToLower <> "application/pdf" And postedFile.ContentLength >= 2000000 Then
                    LblErr.Text = postedFile.FileName & " melebihi ukuran file maximum 2MB."
                    ErrMsg.ShowOnPageLoad = True
                    Exit Sub
                End If
            Next
        End If
        'If FileKTP.HasFile And (FileKTP.PostedFile.ContentType.ToLower <> "image/jpeg" And _
        '                        FileKTP.PostedFile.ContentType.ToLower <> "application/pdf") Then
        '    LblErr.Text = "KTP hanya mendukung file dengan ext. JPG/JPEG/PDF."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If
        'If FileKK.HasFile And (FileKK.PostedFile.ContentType.ToLower <> "image/jpeg" And _
        '                       FileKK.PostedFile.ContentType.ToLower <> "application/pdf") Then
        '    LblErr.Text = "Kartu Keluarga hanya mendukung file dengan ext. JPG/JPEG/PDF."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If
        'If FileNPWP.HasFile And (FileNPWP.PostedFile.ContentType.ToLower <> "image/jpeg" And _
        '                         FileNPWP.PostedFile.ContentType.ToLower <> "application/pdf") Then
        '    LblErr.Text = "NPWP hanya mendukung file dengan ext. JPG/JPEG/PDF."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If
        'If FileIjazah.HasFile And (FileIjazah.PostedFile.ContentType.ToLower <> "image/jpeg" And _
        '                           FileIjazah.PostedFile.ContentType.ToLower <> "application/pdf") Then
        '    LblErr.Text = "Ijazah hanya mendukung file dengan ext. JPG/JPEG/PDF."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If
        'If FileTranskripNilai.HasFile And (FileTranskripNilai.PostedFile.ContentType.ToLower <> "image/jpeg" And _
        '                                   FileTranskripNilai.PostedFile.ContentType.ToLower <> "application/pdf") Then
        '    LblErr.Text = "Transkrip Nilai hanya mendukung file dengan ext. JPG/JPEG/PDF."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If

        TmpDtKeluarga = Session("TmpDtKeluarga")
        TmpDtPendidikan = Session("TmpDtPendidikan")
        TmpDtKetrampilan = Session("TmpDtKetrampilan")
        TmpDtRwytPekerjaan = Session("TmpDtRwytPekerjaan")
        TmpDtRwytPekerjaanMinarta = Session("TmpDtRwytPekerjaanMinarta")
        TmpDtDelFile = Session("TmpDtDelFile")

        Dim CmdInsertHdr As New SqlClient.SqlCommand
        With CmdInsertHdr
            .Connection = Conn
            .CommandType = CommandType.Text
            If LblAction.Text = "NEW" Then
                Using CmdFind As New SqlClient.SqlCommand
                    With CmdFind
                        .Connection = Conn
                        .CommandType = CommandType.Text
                        .CommandText = "SELECT NIK FROM Karyawan WHERE NIK=@P1"
                        .Parameters.AddWithValue("@P1", TxtNIK.Text)
                    End With
                    Using RsFind As SqlClient.SqlDataReader = CmdFind.ExecuteReader
                        If RsFind.Read Then
                            LblErr.Text = "NIK " & TxtNIK.Text & " sudah pernah digunakan."
                            ErrMsg.ShowOnPageLoad = True
                            Exit Sub
                        End If
                    End Using
                End Using
                .CommandText = "INSERT INTO Karyawan (NIK, Nama, Kelamin, TmpLahir, TglLahir, WN, StsNikah, TglNikah, " & _
                            "Agama, Alamat, Provinsi, Kota, AlamatSurat, Email, NoTelp, LokasiKerja, Divisi, " & _
                            "Subdivisi, Jabatan, Golongan, Grade, PrdAwal, UraianPekerjaan, Foto, ScanKTP, " & _
                            "ScanKK, ScanNPWP, ScanIjazah, ScanTranskripNilai, ScanSertifikat,UserEntry,TimeEntry) VALUES (@P1, " & _
                            "@P2, @P3, @P4, @P5, @P6, @P7, @P8, @P9, @P10, @P11, @P12, @P13, @P14, @P15, @P16, " & _
                            "@P17, @P18, @P19, @P20, @P21, @P22, @P23, @P24, @P25, @P26, @P27, @P28, @P29, @P30,@P31,@P32)"
                Session("KTP") = DBNull.Value
                Session("KK") = DBNull.Value
                Session("NPWP") = DBNull.Value
                Session("Ijazah") = DBNull.Value
                Session("TranskripNilai") = DBNull.Value
                Session("Sertifikat") = DBNull.Value
            Else
                Dim UpdateNIK As String = Session("NIK")
                Dim CmdDokumenReader As New SqlClient.SqlCommand
                With CmdDokumenReader
                    .Connection = Conn
                    .CommandType = CommandType.Text
                    .CommandText = "SELECT ScanKTP, ScanKK, ScanNPWP, ScanIjazah, ScanTranskripNilai, ScanSertifikat " & _
                                   "FROM Karyawan where NIK=@P1"
                    .Parameters.AddWithValue("@P1", UpdateNIK)
                End With
                Dim DokumenReader As SqlClient.SqlDataReader = CmdDokumenReader.ExecuteReader
                If DokumenReader.Read Then
                    Session("KTP") = DokumenReader("ScanKTP")
                    Session("KK") = DokumenReader("ScanKK")
                    Session("NPWP") = DokumenReader("ScanNPWP")
                    Session("Ijazah") = DokumenReader("ScanIjazah")
                    Session("TranskripNilai") = DokumenReader("ScanTranskripNilai")
                    Session("Sertifikat") = DokumenReader("ScanSertifikat")
                Else
                    Session("KTP") = DBNull.Value
                    Session("KK") = DBNull.Value
                    Session("NPWP") = DBNull.Value
                    Session("Ijazah") = DBNull.Value
                    Session("TranskripNilai") = DBNull.Value
                    Session("Sertifikat") = DBNull.Value
                End If
                CmdDokumenReader.Dispose()
                DokumenReader.Close()
                .CommandText = "UPDATE Karyawan SET Nama=@P2, Kelamin=@P3, TmpLahir=@P4, TglLahir=@P5, WN=@P6, StsNikah=@P7, TglNikah=@P8, " & _
                            "Agama=@P9, Alamat=@P10, Provinsi=@P11, Kota=@P12, AlamatSurat=@P13, Email=@P14, NoTelp=@P15, LokasiKerja=@P16, Divisi=@P17, " & _
                            "Subdivisi=@P18, Jabatan=@P19, Golongan=@P20, Grade=@P21, PrdAwal=@P22, UraianPekerjaan=@P23, Foto=@P24, ScanKTP=@P25, " & _
                            "ScanKK=@P26, ScanNPWP=@P27, ScanIjazah=@P28, ScanTranskripNilai=@P29, ScanSertifikat=@P30,UserEntry=@P31,TimeEntry=@P32 WHERE NIK=@P1"
            End If
            .Parameters.AddWithValue("@P1", TxtNIK.Text)
            .Parameters.AddWithValue("@P2", TxtNama.Text)
            .Parameters.AddWithValue("@P3", If(RblJenisKelamin.Value = String.Empty, DBNull.Value, RblJenisKelamin.Value))
            .Parameters.AddWithValue("@P4", TxtTempatLahir.Text)
            .Parameters.AddWithValue("@P5", If(TxtTanggalLahir.Text = String.Empty, DBNull.Value, TxtTanggalLahir.Text))
            .Parameters.AddWithValue("@P6", TxtWN.Text)
            .Parameters.AddWithValue("@P7", If(RblStsNikah.Value = String.Empty, DBNull.Value, RblStsNikah.Value))
            .Parameters.AddWithValue("@P8", If(RblStsNikah.Value = "Menikah", If(TxtTglNikah.Text = String.Empty, DBNull.Value, TxtTglNikah.Date), DBNull.Value))
            .Parameters.AddWithValue("@P9", If(RblAgama.Value = String.Empty, DBNull.Value, RblAgama.Value))
            .Parameters.AddWithValue("@P10", TxtAlamat.Text)
            .Parameters.AddWithValue("@P11", TxtProvinsi.Text)
            .Parameters.AddWithValue("@P12", TxtKota.Text)
            .Parameters.AddWithValue("@P13", TxtAlamatSurat.Text)
            .Parameters.AddWithValue("@P14", TxtEmail.Text)
            .Parameters.AddWithValue("@P15", TxtNoTelp.Text)
            .Parameters.AddWithValue("@P16", TxtLokasiKerja.Text)
            .Parameters.AddWithValue("@P17", DDLDivisi.Value)
            .Parameters.AddWithValue("@P18", TxtSubdivisi.Text)
            .Parameters.AddWithValue("@P19", TxtJabatan.Text)
            .Parameters.AddWithValue("@P20", TxtGolongan.Text)
            .Parameters.AddWithValue("@P21", TxtGrade.Text)
            .Parameters.AddWithValue("@P22", If(TxtPrdAwal.Text = String.Empty, DBNull.Value, TxtPrdAwal.Date))
            .Parameters.AddWithValue("@P23", TxtUraian.Text)
            Dim FolderPath As String = Server.MapPath("/PDF/Employee/" & TxtNIK.Text)
            If Not Directory.Exists(FolderPath) Then
                Directory.CreateDirectory(FolderPath)
            End If
            FolderPath = FolderPath & "/"
            If PasFoto.HasFile Then
                Dim FileName As String = TxtNIK.Text & "PasFoto.jpg"
                PasFoto.PostedFile.SaveAs(FolderPath & FileName)
                .Parameters.AddWithValue("@P24", "/PDF/Employee/" & TxtNIK.Text & "/" & FileName)
            Else
                .Parameters.AddWithValue("@P24", Session("PasFoto"))
            End If
            .Parameters.AddWithValue("@P25", DBNull.Value)
            .Parameters.AddWithValue("@P26", DBNull.Value)
            .Parameters.AddWithValue("@P27", DBNull.Value)
            .Parameters.AddWithValue("@P28", DBNull.Value)
            .Parameters.AddWithValue("@P29", DBNull.Value)
            
            If FileUpload.HasFile Then
                Dim NmFile As String = String.Empty
                For Ctr As Integer = 0 To Request.Files.Count - 1
                    Dim postedFile As HttpPostedFile = Request.Files(Ctr)
                    If postedFile.ContentLength > 0 Then
                        Dim fileName As String = FolderPath & Path.GetFileName(postedFile.FileName)
                        postedFile.SaveAs(fileName)
                        NmFile = If(NmFile = String.Empty, "/PDF/Employee/" & TxtNIK.Text & "/" & Path.GetFileName(postedFile.FileName), NmFile & "|" & _
                                    "/PDF/Employee/" & TxtNIK.Text & "/" & Path.GetFileName(postedFile.FileName))
                    End If
                Next
                .Parameters.AddWithValue("@P30", NmFile)
            Else
                .Parameters.AddWithValue("@P30", Session("Sertifikat"))
            End If

            'If FileKTP.HasFile Then
            '    Dim FileNameKTP As String = TxtNIK.Text & "KTP." & _
            '                                If(FileKTP.PostedFile.ContentType.ToLower = "image/jpeg", "jpg", "pdf")
            '    FileKTP.PostedFile.SaveAs(Server.MapPath("/PDF/Employee/" + TxtNIK.Text + "/" + FileNameKTP))
            '    .Parameters.AddWithValue("@P25", "/PDF/Employee/" + TxtNIK.Text + "/" + FileNameKTP)
            'Else
            '    .Parameters.AddWithValue("@P25", Session("KTP"))
            'End If
            'If FileKK.HasFile Then
            '    Dim FileNameKK As String = TxtNIK.Text & "KK." & _
            '                                If(FileKK.PostedFile.ContentType.ToLower = "image/jpeg", "jpg", "pdf")
            '    FileKK.PostedFile.SaveAs(Server.MapPath("/PDF/Employee/" + TxtNIK.Text + "/" + FileNameKK))
            '    .Parameters.AddWithValue("@P26", "/PDF/Employee/" + TxtNIK.Text + "/" + FileNameKK)
            'Else
            '    .Parameters.AddWithValue("@P26", Session("KK"))
            'End If
            'If FileNPWP.HasFile Then
            '    Dim FileNameNPWP As String = TxtNIK.Text & "NPWP." & _
            '                                    If(FileNPWP.PostedFile.ContentType.ToLower = "image/jpeg", "jpg", "pdf")
            '    FileNPWP.PostedFile.SaveAs(Server.MapPath("/PDF/Employee/" + TxtNIK.Text + "/" + FileNameNPWP))
            '    .Parameters.AddWithValue("@P27", "/PDF/Employee/" + TxtNIK.Text + "/" + FileNameNPWP)
            'Else
            '    .Parameters.AddWithValue("@P27", Session("NPWP"))
            'End If
            'If FileIjazah.HasFile Then
            '    Dim FileNameIjazah As String = TxtNIK.Text & "Ijazah." & _
            '                                    If(FileIjazah.PostedFile.ContentType.ToLower = "image/jpeg", "jpg", "pdf")
            '    FileIjazah.PostedFile.SaveAs(Server.MapPath("/PDF/Employee/" + TxtNIK.Text + "/" + FileNameIjazah))
            '    .Parameters.AddWithValue("@P28", "/PDF/Employee/" + TxtNIK.Text + "/" + FileNameIjazah)
            'Else
            '    .Parameters.AddWithValue("@P28", Session("Ijazah"))
            'End If
            'If FileTranskripNilai.HasFile Then
            '    Dim FileNameTranskripNilai As String = TxtNIK.Text & "Transkrip." & _
            '                                            If(FileTranskripNilai.PostedFile.ContentType.ToLower = "image/jpeg", "jpg", "pdf")
            '    FileTranskripNilai.PostedFile.SaveAs(Server.MapPath("/PDF/Employee/" + TxtNIK.Text + "/" + FileNameTranskripNilai))
            '    .Parameters.AddWithValue("@P29", "/PDF/Employee/" + TxtNIK.Text + "/" + FileNameTranskripNilai)
            'Else
            '    .Parameters.AddWithValue("@P29", Session("TranskripNilai"))
            'End If

            .Parameters.AddWithValue("@P31", Session("User").ToString.Split("|")(0))
            .Parameters.AddWithValue("@P32", Now)
            .ExecuteNonQuery()
            .Dispose()
        End With

        Dim CmdDeleteID As New SqlClient.SqlCommand
        With CmdDeleteID
            .Connection = Conn
            .CommandType = CommandType.Text
            .CommandText = "DELETE EmpID WHERE NIK=@P1"
            .Parameters.AddWithValue("@P1", TxtNIK.Text)
            .ExecuteNonQuery()
            .Dispose()
        End With

        If TxtNoKTP.Text <> "" Then
            Dim CmdInsertKTP As New SqlClient.SqlCommand
            With CmdInsertKTP
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "INSERT INTO EmpID (NIK, JenisID, NoID, PrdAwal, PrdAkhir, DiterbitkanOleh,UserEntry,TimeEntry) " & _
                                "VALUES (@P1, @P2, @P3, @P4, @P5, @P6,@P7,@P8)"
                .Parameters.AddWithValue("@P1", TxtNIK.Text)
                .Parameters.AddWithValue("@P2", "KTP")
                .Parameters.AddWithValue("@P3", TxtNoKTP.Text)
                .Parameters.AddWithValue("@P4", If(TxtPrdAwalKTP.Text = String.Empty, DBNull.Value, TxtPrdAwalKTP.Date))
                .Parameters.AddWithValue("@P5", If(TxtPrdAkhirKTP.Text = String.Empty, DBNull.Value, TxtPrdAkhirKTP.Date))
                .Parameters.AddWithValue("@P6", TxtDiterbitkanOlehKTP.Text)
                .Parameters.AddWithValue("@P7", Session("User").ToString.Split("|")(0))
                .Parameters.AddWithValue("@P8", Now)
                .ExecuteNonQuery()
                .Dispose()
            End With
        End If

        If TxtNoPassport.Text <> "" Then
            Dim CmdInsertPassport As New SqlClient.SqlCommand
            With CmdInsertPassport
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "INSERT INTO EmpID (NIK, JenisID, NoID, PrdAwal, PrdAkhir, DiterbitkanOleh,UserEntry,TimeEntry) " & _
                                "VALUES (@P1, @P2, @P3, @P4, @P5, @P6,@P7,@P8)"
                .Parameters.AddWithValue("@P1", TxtNIK.Text)
                .Parameters.AddWithValue("@P2", "Passport")
                .Parameters.AddWithValue("@P3", TxtNoPassport.Text)
                .Parameters.AddWithValue("@P4", If(TxtPrdAwalPassport.Text = String.Empty, DBNull.Value, TxtPrdAwalPassport.Date))
                .Parameters.AddWithValue("@P5", If(TxtPrdAkhirPassport.Text = String.Empty, DBNull.Value, TxtPrdAkhirPassport.Date))
                .Parameters.AddWithValue("@P6", TxtDiterbitkanOlehPassport.Text)
                .Parameters.AddWithValue("@P7", Session("User").ToString.Split("|")(0))
                .Parameters.AddWithValue("@P8", Now)
                .ExecuteNonQuery()
                .Dispose()
            End With
        End If

        If TxtNoNPWP.Text <> "" Then
            Dim CmdInsertNPWP As New SqlClient.SqlCommand
            With CmdInsertNPWP
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "INSERT INTO EmpID (NIK, JenisID, NoID, PrdAwal, PrdAkhir, DiterbitkanOleh,UserEntry,TimeEntry) " & _
                                "VALUES (@P1, @P2, @P3, @P4, @P5, @P6,@P7,@P8)"
                .Parameters.AddWithValue("@P1", TxtNIK.Text)
                .Parameters.AddWithValue("@P2", "NPWP")
                .Parameters.AddWithValue("@P3", TxtNoNPWP.Text)
                .Parameters.AddWithValue("@P4", If(TxtPrdAwalNPWP.Text = String.Empty, DBNull.Value, TxtPrdAwalNPWP.Date))
                .Parameters.AddWithValue("@P5", If(TxtPrdAkhirNPWP.Text = String.Empty, DBNull.Value, TxtPrdAkhirNPWP.Date))
                .Parameters.AddWithValue("@P6", TxtDiterbitkanOlehNPWP.Text)
                .Parameters.AddWithValue("@P7", Session("User").ToString.Split("|")(0))
                .Parameters.AddWithValue("@P8", Now)
                .ExecuteNonQuery()
                .Dispose()
            End With
        End If

        If TxtNoKK.Text <> "" Then
            Dim CmdInsertNPWP As New SqlClient.SqlCommand
            With CmdInsertNPWP
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "INSERT INTO EmpID (NIK, JenisID, NoID, PrdAwal, PrdAkhir, DiterbitkanOleh,UserEntry,TimeEntry) " & _
                                "VALUES (@P1, @P2, @P3, @P4, @P5, @P6,@P7,@P8)"
                .Parameters.AddWithValue("@P1", TxtNIK.Text)
                .Parameters.AddWithValue("@P2", "KK")
                .Parameters.AddWithValue("@P3", TxtNoKK.Text)
                .Parameters.AddWithValue("@P4", If(TxtPrdAwalKK.Text = String.Empty, DBNull.Value, TxtPrdAwalKK.Date))
                .Parameters.AddWithValue("@P5", If(TxtPrdAkhirKK.Text = String.Empty, DBNull.Value, TxtPrdAkhirKK.Date))
                .Parameters.AddWithValue("@P6", TxtDiterbitkanOlehKK.Text)
                .Parameters.AddWithValue("@P7", Session("User").ToString.Split("|")(0))
                .Parameters.AddWithValue("@P8", Now)
                .ExecuteNonQuery()
                .Dispose()
            End With
        End If

        If TxtNoSIMA.Text <> "" Then
            Dim CmdInsertSIMA As New SqlClient.SqlCommand
            With CmdInsertSIMA
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "INSERT INTO EmpID (NIK, JenisID, NoID, PrdAwal, PrdAkhir, DiterbitkanOleh,UserEntry,TimeEntry) " & _
                                "VALUES (@P1, @P2, @P3, @P4, @P5, @P6,@P7,@P8)"
                .Parameters.AddWithValue("@P1", TxtNIK.Text)
                .Parameters.AddWithValue("@P2", "SIM A")
                .Parameters.AddWithValue("@P3", TxtNoSIMA.Text)
                .Parameters.AddWithValue("@P4", If(TxtPrdAwalSIMA.Text = String.Empty, DBNull.Value, TxtPrdAwalSIMA.Date))
                .Parameters.AddWithValue("@P5", If(TxtPrdAkhirSIMA.Text = String.Empty, DBNull.Value, TxtPrdAkhirSIMA.Date))
                .Parameters.AddWithValue("@P6", TxtDiterbitkanOlehSIMA.Text)
                .Parameters.AddWithValue("@P7", Session("User").ToString.Split("|")(0))
                .Parameters.AddWithValue("@P8", Now)
                .ExecuteNonQuery()
                .Dispose()
            End With
        End If

        If TxtNoSIMB.Text <> "" Then
            Dim CmdInsertSIMB As New SqlClient.SqlCommand
            With CmdInsertSIMB
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "INSERT INTO EmpID (NIK, JenisID, NoID, PrdAwal, PrdAkhir, DiterbitkanOleh,UserEntry,TimeEntry) " & _
                                "VALUES (@P1, @P2, @P3, @P4, @P5, @P6,@P7,@P8)"
                .Parameters.AddWithValue("@P1", TxtNIK.Text)
                .Parameters.AddWithValue("@P2", "SIM B")
                .Parameters.AddWithValue("@P3", TxtNoSIMB.Text)
                .Parameters.AddWithValue("@P4", If(TxtPrdAwalSIMB.Text = String.Empty, DBNull.Value, TxtPrdAwalSIMB.Date))
                .Parameters.AddWithValue("@P5", If(TxtPrdAkhirSIMB.Text = String.Empty, DBNull.Value, TxtPrdAkhirSIMB.Date))
                .Parameters.AddWithValue("@P6", TxtDiterbitkanOlehSIMB.Text)
                .Parameters.AddWithValue("@P7", Session("User").ToString.Split("|")(0))
                .Parameters.AddWithValue("@P8", Now)
                .ExecuteNonQuery()
                .Dispose()
            End With
        End If

        If TxtNoSIMC.Text <> "" Then
            Dim CmdInsertSIMC As New SqlClient.SqlCommand
            With CmdInsertSIMC
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "INSERT INTO EmpID (NIK, JenisID, NoID, PrdAwal, PrdAkhir, DiterbitkanOleh,UserEntry,TimeEntry) " & _
                                "VALUES (@P1, @P2, @P3, @P4, @P5, @P6,@P7,@P8)"
                .Parameters.AddWithValue("@P1", TxtNIK.Text)
                .Parameters.AddWithValue("@P2", "SIM C")
                .Parameters.AddWithValue("@P3", TxtNoSIMC.Text)
                .Parameters.AddWithValue("@P4", If(TxtPrdAwalSIMC.Text = String.Empty, DBNull.Value, TxtPrdAwalSIMC.Date))
                .Parameters.AddWithValue("@P5", If(TxtPrdAkhirSIMC.Text = String.Empty, DBNull.Value, TxtPrdAkhirSIMC.Date))
                .Parameters.AddWithValue("@P6", TxtDiterbitkanOlehSIMC.Text)
                .Parameters.AddWithValue("@P7", Session("User").ToString.Split("|")(0))
                .Parameters.AddWithValue("@P8", Now)
                .ExecuteNonQuery()
                .Dispose()
            End With
        End If

        Dim CmdDeleteKeluarga As New SqlClient.SqlCommand
        With CmdDeleteKeluarga
            .Connection = Conn
            .CommandType = CommandType.Text
            .CommandText = "DELETE EmpKeluarga WHERE NIK=@P1"
            .Parameters.AddWithValue("@P1", TxtNIK.Text)
            .ExecuteNonQuery()
            .Dispose()
        End With

        Dim Counter As Integer = 0
        Dim CmdInsertKeluarga As New SqlClient.SqlCommand
        For Each row As DataRow In TmpDtKeluarga.Rows
            Counter += 1
            CmdInsertKeluarga.Parameters.Clear()
            With CmdInsertKeluarga
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "INSERT INTO EmpKeluarga (NIK, NoUrutKeluarga, Hubungan, Nama, " & _
                                "Kelamin, TglLahir, Pekerjaan, Perusahaan,UserEntry,TimeEntry) VALUES (@P1, @P2, @P3, @P4, " & _
                                "@P5, @P6, @P7, @P8,@P9,@P10)"
                .Parameters.AddWithValue("@P1", TxtNIK.Text)
                .Parameters.AddWithValue("@P2", Counter)
                .Parameters.AddWithValue("@P3", row("HubKeluarga"))
                .Parameters.AddWithValue("@P4", row("NamaKeluarga"))
                .Parameters.AddWithValue("@P5", row("JenisKelaminKeluarga"))
                .Parameters.AddWithValue("@P6", row("TglLahirKeluarga"))
                .Parameters.AddWithValue("@P7", row("PekerjaanKeluarga"))
                .Parameters.AddWithValue("@P8", row("PerusahaanKeluarga"))
                .Parameters.AddWithValue("@P9", Session("User").ToString.Split("|")(0))
                .Parameters.AddWithValue("@P10", Now)
                .ExecuteNonQuery()
                .Dispose()
            End With
        Next row
        Counter = 0

        Dim CmdDeletePendidikan As New SqlClient.SqlCommand
        With CmdDeletePendidikan
            .Connection = Conn
            .CommandType = CommandType.Text
            .CommandText = "DELETE EmpPendidikan WHERE NIK=@P1"
            .Parameters.AddWithValue("@P1", TxtNIK.Text)
            .ExecuteNonQuery()
            .Dispose()
        End With

        Dim CmdInsertPendidikan As New SqlClient.SqlCommand
        For Each row As DataRow In TmpDtPendidikan.Rows
            Counter += 1
            CmdInsertPendidikan.Parameters.Clear()
            With CmdInsertPendidikan
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "INSERT INTO EmpPendidikan (NIK, NoUrutPendidikan, TgkPendidikan, PrdAwal, PrdAkhir, Institusi, " & _
                                "Alamat, Jurusan, LlsTdkLls, IPK, NoIjazah,UserEntry,TimeEntry) VALUES (@P1, @P2, @P3, @P4, " & _
                                "@P5, @P6, @P7, @P8, @P9, @P10, @P11,@P12,@P13)"
                .Parameters.AddWithValue("@P1", TxtNIK.Text)
                .Parameters.AddWithValue("@P2", Counter)
                .Parameters.AddWithValue("@P3", row("TgkPendidikan"))
                .Parameters.AddWithValue("@P4", row("PrdAwalPendidikan"))
                .Parameters.AddWithValue("@P5", row("PrdAkhirPendidikan"))
                .Parameters.AddWithValue("@P6", row("InstitusiPendidikan"))
                .Parameters.AddWithValue("@P7", row("AlamatInstitusiPendidikan"))
                .Parameters.AddWithValue("@P8", row("JurusanPendidikan"))
                .Parameters.AddWithValue("@P9", row("LlsTdkLlsPendidikan"))
                .Parameters.AddWithValue("@P10", row("NilaiPendidikan"))
                .Parameters.AddWithValue("@P11", row("NoIjazahPendidikan"))
                .Parameters.AddWithValue("@P12", Session("User").ToString.Split("|")(0))
                .Parameters.AddWithValue("@P13", Now)
                .ExecuteNonQuery()
                .Dispose()
            End With
        Next row
        Counter = 0

        Dim CmdDeleteKetrampilan As New SqlClient.SqlCommand
        With CmdDeleteKetrampilan
            .Connection = Conn
            .CommandType = CommandType.Text
            .CommandText = "DELETE EmpKetrampilan WHERE NIK=@P1"
            .Parameters.AddWithValue("@P1", TxtNIK.Text)
            .ExecuteNonQuery()
            .Dispose()
        End With

        Dim CmdInsertKetrampilan As New SqlClient.SqlCommand
        For Each row As DataRow In TmpDtKetrampilan.Rows
            Counter += 1
            CmdInsertKetrampilan.Parameters.Clear()
            With CmdInsertKetrampilan
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "INSERT INTO EmpKetrampilan (NIK, NoUrutKetrampilan, Nama, PrdAwal, PrdAkhir, " & _
                                "Sertifikat, Grade, NoSertifikat, Institusi,UserEntry,TimeEntry) VALUES (@P1, @P2, @P3, " & _
                                "@P4, @P5, @P6, @P7, @P8, @P9,@P10,@P11)"
                .Parameters.AddWithValue("@P1", TxtNIK.Text)
                .Parameters.AddWithValue("@P2", Counter)
                .Parameters.AddWithValue("@P3", row("NamaKetrampilan"))
                .Parameters.AddWithValue("@P4", row("PrdAwalKetrampilan"))
                .Parameters.AddWithValue("@P5", row("PrdAkhirKetrampilan"))
                .Parameters.AddWithValue("@P6", row("NamaSertifikatKetrampilan"))
                .Parameters.AddWithValue("@P7", row("GradeKetrampilan"))
                .Parameters.AddWithValue("@P8", row("NoSertifikatKetrampilan"))
                .Parameters.AddWithValue("@P9", row("InstitusiKetrampilan"))
                .Parameters.AddWithValue("@P10", Session("User").ToString.Split("|")(0))
                .Parameters.AddWithValue("@P11", Now)
                .ExecuteNonQuery()
                .Dispose()
            End With
        Next row
        Counter = 0

        Dim CmdDeleteRwytPekerjaan As New SqlClient.SqlCommand
        With CmdDeleteRwytPekerjaan
            .Connection = Conn
            .CommandType = CommandType.Text
            .CommandText = "DELETE EmpPekerjaanH WHERE NIK=@P1"
            .Parameters.AddWithValue("@P1", TxtNIK.Text)
            .ExecuteNonQuery()
            .Dispose()
        End With

        Dim CmdInsertRwytPekerjaan As New SqlClient.SqlCommand
        For Each row As DataRow In TmpDtRwytPekerjaan.Rows
            Counter += 1
            CmdInsertRwytPekerjaan.Parameters.Clear()
            With CmdInsertRwytPekerjaan
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "INSERT INTO EmpPekerjaanH (NIK, NoUrutRwytPekerjaan, PrdAwal, PrdAkhir, " & _
                                "Perusahaan, Alamat, Industri, Jabatan, GajiPokok, Tunjangan, " & _
                                "UraianPekerjaan,UserEntry,TimeEntry) VALUES (@P1, @P2, @P3, @P4, @P5, @P6, @P7, " & _
                                "@P8, @P9, @P10, @P11,@P12,@P13)"
                .Parameters.AddWithValue("@P1", TxtNIK.Text)
                .Parameters.AddWithValue("@P2", Counter)
                .Parameters.AddWithValue("@P3", row("PrdAwalRwytPekerjaan"))
                .Parameters.AddWithValue("@P4", row("PrdAkhirRwytPekerjaan"))
                .Parameters.AddWithValue("@P5", row("PerusahaanRwytPekerjaan"))
                .Parameters.AddWithValue("@P6", row("AlamatRwytPekerjaan"))
                .Parameters.AddWithValue("@P7", row("IndustriRwytPekerjaan"))
                .Parameters.AddWithValue("@P8", row("JabatanRwytPekerjaan"))
                .Parameters.AddWithValue("@P9", row("GajiRwytPekerjaan"))
                .Parameters.AddWithValue("@P10", row("TunjanganRwytPekerjaan"))
                .Parameters.AddWithValue("@P11", row("UraianRwytPekerjaan"))
                .Parameters.AddWithValue("@P12", Session("User").ToString.Split("|")(0))
                .Parameters.AddWithValue("@P13", Now)
                .ExecuteNonQuery()
                .Dispose()
            End With
        Next row
        Counter = 0

        Dim CmdDeleteRwytPekerjaanMinarta As New SqlClient.SqlCommand
        With CmdDeleteRwytPekerjaanMinarta
            .Connection = Conn
            .CommandType = CommandType.Text
            .CommandText = "DELETE EmpPekerjaanMinarta WHERE NIK=@P1"
            .Parameters.AddWithValue("@P1", TxtNIK.Text)
            .ExecuteNonQuery()
            .Dispose()
        End With

        Dim CmdInsertRwytPekerjaanMinarta As New SqlClient.SqlCommand
        For Each row As DataRow In TmpDtRwytPekerjaanMinarta.Rows
            Counter += 1
            CmdInsertRwytPekerjaanMinarta.Parameters.Clear()
            With CmdInsertRwytPekerjaanMinarta
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "INSERT INTO EmpPekerjaanMinarta (NIK, NoUrutRwytPekerjaanMinarta, " & _
                                "PrdAwal, PrdAkhir, Divisi, Subdivisi, LokasiKerja, Jabatan, " & _
                                "Golongan, Grade, GajiPokok, Tunjangan, KPI, TesKesehatan, HslKesehatan, " & _
                                "TesPsikologi, HslPsikologi, Atasan, UraianPekerjaan,UserEntry,TimeEntry) VALUES " & _
                                "(@P1, @P2, @P3, @P4, @P5, @P6, @P7, @P8, @P9, @P10, @P11, @P12, " & _
                                "@P13, @P14, @P15, @P16, @P17, @P18, @P19,@P20,@P21)"
                .Parameters.AddWithValue("@P1", TxtNIK.Text)
                .Parameters.AddWithValue("@P2", Counter)
                .Parameters.AddWithValue("@P3", row("PrdAwalRwytPekerjaanMinarta"))
                .Parameters.AddWithValue("@P4", row("PrdAkhirRwytPekerjaanMinarta"))
                .Parameters.AddWithValue("@P5", row("DivisiRwytPekerjaanMinarta"))
                .Parameters.AddWithValue("@P6", row("SubdivisiRwytPekerjaanMinarta"))
                .Parameters.AddWithValue("@P7", row("LokasiKerjaRwytPekerjaanMinarta"))
                .Parameters.AddWithValue("@P8", row("JabatanRwytPekerjaanMinarta"))
                .Parameters.AddWithValue("@P9", row("GolonganRwytPekerjaanMinarta"))
                .Parameters.AddWithValue("@P10", row("GradeRwytPekerjaanMinarta"))
                .Parameters.AddWithValue("@P11", row("GajiRwytPekerjaanMinarta"))
                .Parameters.AddWithValue("@P12", row("TunjanganRwytPekerjaanMinarta"))
                .Parameters.AddWithValue("@P13", row("KPIRwytPekerjaanMinarta"))
                .Parameters.AddWithValue("@P14", row("TesKesehatanRwytPekerjaanMinarta"))
                .Parameters.AddWithValue("@P15", row("HasilKesehatanRwytPekerjaanMinarta"))
                .Parameters.AddWithValue("@P16", row("TesPsikologiRwytPekerjaanMinarta"))
                .Parameters.AddWithValue("@P17", row("HasilPsikologiRwytPekerjaanMinarta"))
                .Parameters.AddWithValue("@P18", row("AtasanRwytPekerjaanMinarta"))
                .Parameters.AddWithValue("@P19", row("UraianRwytPekerjaanMinarta"))
                .Parameters.AddWithValue("@P20", Session("User").ToString.Split("|")(0))
                .Parameters.AddWithValue("@P21", Now)
                .ExecuteNonQuery()
                .Dispose()
            End With
        Next row
        Counter = 0

        For Each FileDeleted As DataRow In TmpDtDelFile.Rows
            If File.Exists(Server.MapPath("/PDF/Employee/" & Session("NIK") & "/" & FileDeleted("NamaFile"))) Then
                File.Delete(Server.MapPath("~/PDF/Employee/" & Session("NIK") & "/" & FileDeleted("NamaFile")))
            End If
        Next

        BtnCancelDataEntry_Click(BtnCancelDataEntry, New EventArgs())
    End Sub
    Private Sub GridDataKeluarga_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDataKeluarga.RowCommand
        If e.CommandName = "BtnUpdKeluarga" Then
            Dim SelectRecord As GridViewRow = GridDataKeluarga.Rows(e.CommandArgument)

            TxtActKeluarga.Text = "UPD"
            TxtNoUrutKeluarga.Text = SelectRecord.Cells(0).Text
            TxtHubKeluarga.Value = SelectRecord.Cells(1).Text
            TxtNamaKeluarga.Text = SelectRecord.Cells(2).Text
            TxtJenisKelaminKeluarga.Value = If(SelectRecord.Cells(3).Text = "Pria", "L", "P")
            TxtTglLahirKeluarga.Date = If(SelectRecord.Cells(4).Text = "&nbsp;", DBNull.Value, SelectRecord.Cells(4).Text)
            TxtPekerjaanKeluarga.Text = If(SelectRecord.Cells(5).Text = "&nbsp;", String.Empty, SelectRecord.Cells(5).Text)
            TxtPerusahaanKeluarga.Text = If(SelectRecord.Cells(6).Text = "&nbsp;", String.Empty, SelectRecord.Cells(6).Text)
            PopEntKeluarga.ShowOnPageLoad = True
        ElseIf e.CommandName = "BtnDelKeluarga" Then
            Dim SelectRecord As GridViewRow = GridDataKeluarga.Rows(e.CommandArgument)

            TmpDtKeluarga = Session("TmpDtKeluarga")
            TmpDtKeluarga.Rows(e.CommandArgument).Delete()
            TmpDtKeluarga.AcceptChanges()

            GridDataKeluarga.DataSource = TmpDtKeluarga
            GridDataKeluarga.DataBind()
            Session("TmpDtKeluarga") = TmpDtKeluarga
        End If
    End Sub
    Private Sub GridDataPendidikan_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDataPendidikan.RowCommand
        If e.CommandName = "BtnUpdPendidikan" Then
            Dim SelectRecord As GridViewRow = GridDataPendidikan.Rows(e.CommandArgument)

            TxtActPendidikan.Text = "UPD"
            TxtNoUrutPendidikan.Text = SelectRecord.Cells(0).Text
            TxtTgkPendidikan.Value = SelectRecord.Cells(1).Text
            TxtPrdAwalPendidikan.Date = If(SelectRecord.Cells(2).Text = "&nbsp;", DBNull.Value, SelectRecord.Cells(2).Text)
            TxtPrdAkhirPendidikan.Date = If(SelectRecord.Cells(3).Text = "&nbsp;", DBNull.Value, SelectRecord.Cells(3).Text)
            TxtInstitusiPendidikan.Text = If(SelectRecord.Cells(4).Text = "&nbsp;", String.Empty, SelectRecord.Cells(4).Text)
            TxtAlamatInstitusiPendidikan.Text = If(SelectRecord.Cells(5).Text = "&nbsp;", String.Empty, SelectRecord.Cells(5).Text)
            TxtJurusanPendidikan.Text = If(SelectRecord.Cells(6).Text = "&nbsp;", String.Empty, SelectRecord.Cells(6).Text)
            TxtLlsTdkLlsPendidikan.Value = SelectRecord.Cells(7).Text
            TxtNilaiPendidikan.Text = If(SelectRecord.Cells(8).Text = "&nbsp;", String.Empty, SelectRecord.Cells(8).Text)
            TxtNoIjazahPendidikan.Text = If(SelectRecord.Cells(9).Text = "&nbsp;", String.Empty, SelectRecord.Cells(9).Text)
            PopEntPendidikan.ShowOnPageLoad = True
        ElseIf e.CommandName = "BtnDelPendidikan" Then
            Dim SelectRecord As GridViewRow = GridDataPendidikan.Rows(e.CommandArgument)

            TmpDtPendidikan = Session("TmpDtPendidikan")
            TmpDtPendidikan.Rows(e.CommandArgument).Delete()
            TmpDtPendidikan.AcceptChanges()

            GridDataPendidikan.DataSource = TmpDtPendidikan
            GridDataPendidikan.DataBind()
            Session("TmpDtPendidikan") = TmpDtPendidikan
        End If
    End Sub
    Private Sub GridDataKetrampilan_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDataKetrampilan.RowCommand
        If e.CommandName = "BtnUpdKetrampilan" Then
            Dim SelectRecord As GridViewRow = GridDataKetrampilan.Rows(e.CommandArgument)

            TxtActKetrampilan.Text = "UPD"
            TxtNoUrutKetrampilan.Text = SelectRecord.Cells(0).Text
            TxtNamaKetrampilan.Value = SelectRecord.Cells(1).Text
            TxtPrdAwalKetrampilan.Date = If(SelectRecord.Cells(2).Text = "&nbsp;", DBNull.Value, SelectRecord.Cells(2).Text)
            TxtPrdAkhirKetrampilan.Date = If(SelectRecord.Cells(3).Text = "&nbsp;", DBNull.Value, SelectRecord.Cells(3).Text)
            TxtNamaSertifikatKetrampilan.Text = SelectRecord.Cells(4).Text
            TxtGradeKetrampilan.Text = If(SelectRecord.Cells(5).Text = "&nbsp;", String.Empty, SelectRecord.Cells(5).Text)
            TxtNoSertifikatKetrampilan.Text = If(SelectRecord.Cells(6).Text = "&nbsp;", String.Empty, SelectRecord.Cells(6).Text)
            TxtInstitusiKetrampilan.Text = If(SelectRecord.Cells(7).Text = "&nbsp;", String.Empty, SelectRecord.Cells(7).Text)
            PopEntKetrampilan.ShowOnPageLoad = True
        ElseIf e.CommandName = "BtnDelKetrampilan" Then
            Dim SelectRecord As GridViewRow = GridDataKetrampilan.Rows(e.CommandArgument)

            TmpDtKetrampilan = Session("TmpDtKetrampilan")
            TmpDtKetrampilan.Rows(e.CommandArgument).Delete()
            TmpDtKetrampilan.AcceptChanges()

            GridDataKetrampilan.DataSource = TmpDtKetrampilan
            GridDataKetrampilan.DataBind()
            Session("TmpDtKetrampilan") = TmpDtKetrampilan
        End If
    End Sub
    Private Sub GridDataRwytPekerjaan_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDataRwytPekerjaan.RowCommand
        If e.CommandName = "BtnUpdRwytPekerjaan" Then
            Dim SelectRecord As GridViewRow = GridDataRwytPekerjaan.Rows(e.CommandArgument)

            TxtActRwytPekerjaan.Text = "UPD"
            TxtNoUrutRwytPekerjaan.Text = SelectRecord.Cells(0).Text
            TxtPrdAwalRwytPekerjaan.Date = If(SelectRecord.Cells(1).Text = "&nbsp;", DBNull.Value, SelectRecord.Cells(1).Text)
            TxtPrdAkhirRwytPekerjaan.Date = If(SelectRecord.Cells(2).Text = "&nbsp;", DBNull.Value, SelectRecord.Cells(2).Text)
            TxtPerusahaanRwytPekerjaan.Value = If(SelectRecord.Cells(3).Text = "&nbsp;", String.Empty, SelectRecord.Cells(3).Text)
            TxtAlamatRwytPekerjaan.Text = If(SelectRecord.Cells(4).Text = "&nbsp;", String.Empty, SelectRecord.Cells(4).Text)
            TxtIndustriRwytPekerjaan.Text = If(SelectRecord.Cells(5).Text = "&nbsp;", String.Empty, SelectRecord.Cells(5).Text)
            TxtJabatanRwytPekerjaan.Text = If(SelectRecord.Cells(6).Text = "&nbsp;", String.Empty, SelectRecord.Cells(6).Text)
            TxtLokasiKerjaRwytPekerjaan.Text = If(SelectRecord.Cells(7).Text = "&nbsp;", String.Empty, SelectRecord.Cells(7).Text)
            TxtGajiRwytPekerjaan.Text = If(SelectRecord.Cells(8).Text = "&nbsp;", String.Empty, SelectRecord.Cells(8).Text)
            TxtTunjanganRwytPekerjaan.Text = If(SelectRecord.Cells(9).Text = "&nbsp;", String.Empty, SelectRecord.Cells(9).Text)
            TxtUraianRwytPekerjaan.Text = If(SelectRecord.Cells(10).Text = "&nbsp;", String.Empty, SelectRecord.Cells(10).Text)
            PopEntRwytPekerjaan.ShowOnPageLoad = True
        ElseIf e.CommandName = "BtnDelRwytPekerjaan" Then
            Dim SelectRecord As GridViewRow = GridDataRwytPekerjaan.Rows(e.CommandArgument)

            TmpDtRwytPekerjaan = Session("TmpDtRwytPekerjaan")
            TmpDtRwytPekerjaan.Rows(e.CommandArgument).Delete()
            TmpDtRwytPekerjaan.AcceptChanges()

            GridDataRwytPekerjaan.DataSource = TmpDtRwytPekerjaan
            GridDataRwytPekerjaan.DataBind()
            Session("TmpDtRwytPekerjaan") = TmpDtRwytPekerjaan
        End If
    End Sub
    Private Sub GridDataRwytPekerjaanMinarta_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDataRwytPekerjaanMinarta.RowCommand
        If e.CommandName = "BtnUpdRwytPekerjaanMinarta" Then
            Dim SelectRecord As GridViewRow = GridDataRwytPekerjaanMinarta.Rows(e.CommandArgument)

            TxtActRwytPekerjaanMinarta.Text = "UPD"
            TxtNoUrutRwytPekerjaanMinarta.Text = SelectRecord.Cells(0).Text
            TxtPrdAwalRwytPekerjaanMinarta.Date = If(SelectRecord.Cells(1).Text = "&nbsp;", DBNull.Value, SelectRecord.Cells(1).Text)
            TxtPrdAkhirRwytPekerjaanMinarta.Date = If(SelectRecord.Cells(2).Text = "&nbsp;", DBNull.Value, SelectRecord.Cells(2).Text)
            TxtDivisiRwytPekerjaanMinarta.Value = If(SelectRecord.Cells(3).Text = "&nbsp;", String.Empty, SelectRecord.Cells(3).Text)
            TxtSubdivisiRwytPekerjaanMinarta.Text = If(SelectRecord.Cells(4).Text = "&nbsp;", String.Empty, SelectRecord.Cells(4).Text)
            TxtLokasiKerjaRwytPekerjaanMinarta.Text = If(SelectRecord.Cells(5).Text = "&nbsp;", String.Empty, SelectRecord.Cells(5).Text)
            TxtJabatanRwytPekerjaanMinarta.Text = If(SelectRecord.Cells(6).Text = "&nbsp;", String.Empty, SelectRecord.Cells(6).Text)
            TxtGolonganRwytPekerjaanMinarta.Text = If(SelectRecord.Cells(7).Text = "&nbsp;", String.Empty, SelectRecord.Cells(7).Text)
            TxtGradeRwytPekerjaanMinarta.Text = If(SelectRecord.Cells(8).Text = "&nbsp;", String.Empty, SelectRecord.Cells(8).Text)
            TxtGajiRwytPekerjaanMinarta.Text = If(SelectRecord.Cells(9).Text = "&nbsp;", String.Empty, SelectRecord.Cells(9).Text)
            TxtTunjanganRwytPekerjaanMinarta.Text = If(SelectRecord.Cells(10).Text = "&nbsp;", String.Empty, SelectRecord.Cells(10).Text)
            TxtKPIRwytPekerjaanMinarta.Text = If(SelectRecord.Cells(11).Text = "&nbsp;", String.Empty, SelectRecord.Cells(11).Text)
            TxtTesKesehatanRwytPekerjaanMinarta.Text = SelectRecord.Cells(12).Text
            TxtHasilKesehatanRwytPekerjaanMinarta.Text = If(SelectRecord.Cells(13).Text = "&nbsp;", String.Empty, SelectRecord.Cells(13).Text)
            TxtTesPsikologiRwytPekerjaanMinarta.Text = SelectRecord.Cells(14).Text
            TxtHasilPsikologiRwytPekerjaanMinarta.Text = If(SelectRecord.Cells(15).Text = "&nbsp;", String.Empty, SelectRecord.Cells(15).Text)
            TxtAtasanRwytPekerjaanMinarta.Text = If(SelectRecord.Cells(16).Text = "&nbsp;", String.Empty, SelectRecord.Cells(16).Text)
            TxtUraianRwytPekerjaanMinarta.Text = If(SelectRecord.Cells(17).Text = "&nbsp;", String.Empty, SelectRecord.Cells(17).Text)
            PopEntRwytPekerjaanMinarta.ShowOnPageLoad = True
        ElseIf e.CommandName = "BtnDelRwytPekerjaanMinarta" Then
            Dim SelectRecord As GridViewRow = GridDataRwytPekerjaanMinarta.Rows(e.CommandArgument)

            TmpDtRwytPekerjaanMinarta = Session("TmpDtRwytPekerjaanMinarta")
            TmpDtRwytPekerjaanMinarta.Rows(e.CommandArgument).Delete()
            TmpDtRwytPekerjaanMinarta.AcceptChanges()

            GridDataRwytPekerjaanMinarta.DataSource = TmpDtRwytPekerjaanMinarta
            GridDataRwytPekerjaanMinarta.DataBind()
            Session("TmpDtRwytPekerjaanMinarta") = TmpDtRwytPekerjaanMinarta
        End If
    End Sub

    Protected Sub BtnCancelDataEntry_Click(sender As Object, e As EventArgs) Handles BtnCancelDataEntry.Click
        Session.Remove("Karyawan")
        Session.Remove("NIK")
        Session.Remove("Nama")
        Session.Remove("PasFoto")
        Session.Remove("KTP")
        Session.Remove("KK")
        Session.Remove("NPWP")
        Session.Remove("Ijazah")
        Session.Remove("TranskripNilai")
        Session.Remove("Sertifikat")
        Session.Remove("TmpDtKeluarga")
        Session.Remove("TmpDtPendidikan")
        Session.Remove("TmpDtKetrampilan")
        Session.Remove("TmpDtRwytPekerjaan")
        Session.Remove("TmpDtRwytPekerjaanMinarta")
        Session.Remove("ViewJPG")
        Session.Remove("ViewPDF")
        Session.Remove("TmpDtDelFile")
        Response.Redirect("FrmKaryawan.aspx")
    End Sub

    Private Sub GridDataKeluarga_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDataKeluarga.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(3).Text = "L" Then
                e.Row.Cells(3).Text = "Pria"
            Else
                e.Row.Cells(3).Text = "Wanita"
            End If
        End If
    End Sub
    Private Sub BindGridDP()
        Dim UpdateNIK As String = Session("NIK")
        TmpDtDP.Columns.AddRange(New DataColumn() {New DataColumn("NamaFile", GetType(String))})

        If Session("Karyawan") = "UPD" And _
            Directory.Exists(Server.MapPath("/PDF/Employee/" & Session("NIK") & "/")) Then
            For Each FileDP As String In Directory.GetFiles(Server.MapPath("~/PDF/Employee/" & Session("NIK") & "/"))
                Dim NamaFileDP() As String = FileDP.Split("\")
                Dim arrayAkhir As Integer = NamaFileDP.Length()
                TmpDtDP.Rows.Add(NamaFileDP(arrayAkhir - 1))
            Next
        End If

        GridDokumenPendukung.DataSource = TmpDtDP
        Session("TmpDtDP") = TmpDtDP

        GridDokumenPendukung.DataBind()
    End Sub
    Private Sub GridDokumenPendukung_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDokumenPendukung.RowCommand
        If e.CommandName = "BtnViewFile" Then
            Session.Remove("ViewPDF")
            Session.Remove("ViewJPG")
            Dim SelectRecord As GridViewRow = GridDokumenPendukung.Rows(e.CommandArgument)

            Dim FileSplit() As String = SelectRecord.Cells(1).Text.Split(".")
            Dim arrayAkhir As Integer = FileSplit.Length - 1
            If FileSplit(arrayAkhir).ToLower = "pdf" Then
                Session("ViewPDF") = "/PDF/Employee/" & Session("NIK") & "/" & _
                                        SelectRecord.Cells(1).Text
            Else
                Session("ViewJPG") = "/PDF/Employee/" & Session("NIK") & "/" & _
                                        SelectRecord.Cells(1).Text
            End If

            'If SelectRecord.Cells(1).Text.Split(".")(1) = "jpg" Then
            '    Session("ViewJPG") = "/PDF/Employee/" & Session("NIK") & "/" & _
            '                            SelectRecord.Cells(1).Text
            'ElseIf SelectRecord.Cells(1).Text.Split(".")(1) = "pdf" Then
            '    Session("ViewPDF") = "/PDF/Employee/" & Session("NIK") & "/" & _
            '                            SelectRecord.Cells(1).Text
            'Else
            '    msgBox1.alert("Format file tidak mendukung untuk dilihat.")
            '    Exit Sub
            'End If
            With DialogWindow1
                .TargetUrl = "FrmView.aspx"
                .Open()
            End With
        ElseIf e.CommandName = "BtnDelFile" Then
            Dim SelectRecord As GridViewRow = GridDokumenPendukung.Rows(e.CommandArgument)

            'Menambahkan nama file yang dihapus
            TmpDtDelFile = Session("TmpDtDelFile")
            TmpDtDelFile.Rows.Add(SelectRecord.Cells(1).Text)
            Session("TmpDtDelFile") = TmpDtDelFile

            'Memperbaharui TmpDtDP
            TmpDtDP = Session("TmpDtDP")
            TmpDtDP.Rows(e.CommandArgument).Delete()
            TmpDtDP.AcceptChanges()

            GridDokumenPendukung.DataSource = TmpDtDP
            GridDokumenPendukung.DataBind()
            Session("TmpDtDP") = TmpDtDP
        End If

    End Sub
    Private Sub DelFile()
        TmpDtDelFile.Columns.AddRange(New DataColumn() {New DataColumn("NamaFile", GetType(String))})
        Session("TmpDtDelFile") = TmpDtDelFile
    End Sub

    Private Sub DisableControls(control As System.Web.UI.Control)

        For Each c As System.Web.UI.Control In control.Controls
            ' Get the Enabled property by reflection.
            Dim type As Type = c.GetType
            Dim prop As Reflection.PropertyInfo = type.GetProperty("Enabled")

            ' Set it to False to disable the control.
            If Not prop Is Nothing Then
                prop.SetValue(c, False, Nothing)
            End If

            ' Recurse into child controls.
            If c.Controls.Count > 0 Then
                Me.DisableControls(c)
            End If
        Next

        BtnSaveDataEntry.Visible = False
        BtnCancelDataEntry.Text = "OK"
        BtnCancelDataEntry.Enabled = True        

    End Sub

End Class