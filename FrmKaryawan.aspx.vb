Imports System.IO
Imports DevExpress.Web
Public Class FrmKaryawan
    Inherits System.Web.UI.Page
    Dim Conn As New SqlClient.SqlConnection

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

        If Request.Params("EmpNonAktif") = 1 Then
            Using CmdEdit As New SqlClient.SqlCommand
                With CmdEdit
                    .Connection = Conn
                    .CommandType = CommandType.Text
                    .CommandText = "UPDATE Karyawan SET Active=@P1,UserEntry=@P2,TimeEntry=@P3 WHERE NIK=@P4"
                    .Parameters.AddWithValue("@P1", "0")
                    .Parameters.AddWithValue("@P2", Session("User").ToString.Split("|")(0))
                    .Parameters.AddWithValue("@P3", Now)
                    .Parameters.AddWithValue("@P4", Session("NIK"))
                    .ExecuteNonQuery()
                End With
            End Using
            GridMaster.DataBind()

        End If

        If IsPostBack = False Then
            GridMaster.DataBind()
            GridMaster.Columns(2).Visible = False
            GridMaster.Columns(3).Visible = False
            GridMaster.Columns(4).Visible = False
            GridMaster.Columns(9).Visible = False
            GridMaster.Columns(10).Visible = False
            GridMaster.Columns(11).Visible = False
            GridMaster.Columns(12).Visible = False
            GridMaster.Columns(13).Visible = False
            GridMaster.Columns(14).Visible = False
        End If

        If Session("MasterPage") IsNot Nothing Then
            GridMaster.PageIndex = Session("MasterPage")
        End If

    End Sub
    Protected Sub GridMaster_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        If GridMaster.Columns("UpdateColumn") Is Nothing Then
            Dim col = New DevExpress.Web.GridViewCommandColumn()
            col.Name = "UpdateColumn"
            Dim btnUpdate = New DevExpress.Web.GridViewCommandColumnCustomButton()
            btnUpdate.ID = "UpdateBaris"
            btnUpdate.Text = "UPDATE"
            col.CustomButtons.Add(btnUpdate)
            GridMaster.Columns.Add(col)
        End If
        If GridMaster.Columns("StatusColumn") Is Nothing Then
            Dim col = New DevExpress.Web.GridViewCommandColumn()
            col.Name = "StatusColumn"
            Dim btnStat = New DevExpress.Web.GridViewCommandColumnCustomButton()
            btnStat.ID = "StatusBaris"
            btnStat.Text = "NON AKTIF"
            col.CustomButtons.Add(btnStat)
            GridMaster.Columns.Add(col)
        End If
    End Sub

    Protected Sub GridMaster_CustomButtonCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs)
        If e.ButtonID = "UpdateBaris" Then
            Session("Karyawan") = "UPD"
            Session("NIK") = GridMaster.GetRowValues(e.VisibleIndex, "NIK")
            'Using CmdFind As New SqlClient.SqlCommand
            '    With CmdFind
            '        .Connection = Conn
            '        .CommandType = CommandType.Text
            '        .CommandText = "SELECT * FROM Karyawan WHERE NIK=@P1"
            '        .Parameters.AddWithValue("@P1", Session("NIK"))
            '    End With
            '    Using RsFind As SqlClient.SqlDataReader = CmdFind.ExecuteReader
            '        If RsFind.Read Then
            '            If RsFind("Active") = "0" Then
            '                msgBox1.alert(RsFind("Nama") & " sudah non aktif.")
            '                Exit Sub
            '            End If
            '        End If
            '    End Using
            'End Using

            'DevExpress.Web.ASPxWebControl.RedirectOnCallback("FrmEntryKaryawan.aspx")
            Response.Redirect("FrmEntryKaryawan.aspx")
        End If
        If e.ButtonID = "StatusBaris" Then
            Session("NIK") = GridMaster.GetRowValues(e.VisibleIndex, "NIK")
            Using CmdFind As New SqlClient.SqlCommand
                With CmdFind
                    .Connection = Conn
                    .CommandType = CommandType.Text
                    .CommandText = "SELECT * FROM Karyawan WHERE NIK=@P1"
                    .Parameters.AddWithValue("@P1", Session("NIK"))
                End With
                Using RsFind As SqlClient.SqlDataReader = CmdFind.ExecuteReader
                    If RsFind.Read Then
                        If RsFind("Active") = "0" Then
                            msgBox1.alert(RsFind("Nama") & " sudah non aktif.")
                            Exit Sub
                        End If
                        msgBox1.confirm("Konfirmasi non aktif untuk " & RsFind("Nama") & " ?", "EmpNonAktif")
                    End If
                End Using
            End Using
        End If

    End Sub
    'Private Sub GridMaster_EditCommand(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) Handles GridMaster.EditCommand
    '    Session("Karyawan") = "UPD"
    '    Dim NilaiNIK As Object = TryCast(GridMaster.GetRowValues(1, "NIK"), Object())
    '    Session("NIK") = NilaiNIK
    '    Response.Redirect("FrmEntryKaryawan.aspx")
    'End Sub

    Private Sub Grid_DataBinding(sender As Object, e As System.EventArgs) Handles GridMaster.DataBinding
        Using CmdGrid As New SqlClient.SqlCommand
            With CmdGrid
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT " & _
                               "Karyawan.NIK, " & _
                               "Karyawan.Nama, " & _
                               "Karyawan.Alamat, " & _
                               "Karyawan.NoTelp, " & _
                               "Karyawan.Email, " & _
                               "Karyawan.LokasiKerja as [Lokasi Kerja], " & _
                               "Karyawan.Divisi, " & _
                               "Karyawan.Subdivisi as [Sub Divisi], " & _
                               "Karyawan.Jabatan, " & _
                               "Karyawan.Golongan, " & _
                               "Karyawan.Grade, " & _
                               "Karyawan.UraianPekerjaan as [Uraian Pekerjaan], " & _
                               "Karyawan.Foto, " & _
                               "FORMAT(Karyawan.PrdAwal,'dd-MMM-yyyy') as [PrdAwal], " & _
                               "Karyawan.Active " & _
                               "FROM Karyawan ORDER BY NIK DESC"
            End With
            Using DaGrid As New SqlClient.SqlDataAdapter
                DaGrid.SelectCommand = CmdGrid
                Using DsGrid As New DataSet
                    DaGrid.Fill(DsGrid)
                    With GridMaster
                        .DataSource = DsGrid
                    End With
                End Using
            End Using
        End Using

    End Sub

    Protected Sub BtnTambah_Click(sender As Object, e As EventArgs) Handles BtnTambah.Click
        Session("Karyawan") = "NEW"
        Response.Redirect("FrmEntryKaryawan.aspx")
    End Sub

    Protected Sub GridMinarta_CustomCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs)
        'Dim masterKey As Integer = GridMaster.GetRowValues(0, "NIK")
        Dim masterKey As Integer = GridMaster.GetRowValues(Convert.ToInt32(e.Parameters), "NIK")
        'Session("masterKey") = masterKey
        Using CmdGrid As New SqlClient.SqlCommand
            With CmdGrid
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT " & _
                               "EmpPekerjaanMinarta.NIK, " & _
                               "EmpPekerjaanMinarta.PrdAwal as [Tanggal Dimulai], " & _
                               "EmpPekerjaanMinarta.PrdAkhir as [Tanggal Berakhir], " & _
                               "EmpPekerjaanMinarta.Divisi, " & _
                               "EmpPekerjaanMinarta.Subdivisi, " & _
                               "EmpPekerjaanMinarta.Jabatan, " & _
                               "EmpPekerjaanMinarta.Golongan, " & _
                               "EmpPekerjaanMinarta.Grade as Status, " & _
                               "EmpPekerjaanMinarta.LokasiKerja as [Lokasi Kerja], " & _
                               "EmpPekerjaanMinarta.KPI, " & _
                               "EmpPekerjaanMinarta.Atasan " & _
                               "FROM EmpPekerjaanMinarta " & _
                               "WHERE NIK=@P1"
                .Parameters.AddWithValue("@P1", masterKey)
            End With
            Using DaGrid As New SqlClient.SqlDataAdapter
                DaGrid.SelectCommand = CmdGrid
                Using DsGrid As New DataSet
                    DaGrid.Fill(DsGrid)
                    With GridMinarta
                        .DataSource = DsGrid
                        .DataBind()
                        .Columns(0).Visible = False
                    End With
                End Using
            End Using
        End Using
    End Sub
    Protected Sub GridRwytPekerjaan_CustomCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs)
        'Dim masterKey As Integer = GridMaster.GetRowValues(0, "NIK")
        Dim masterKey As Integer = GridMaster.GetRowValues(Convert.ToInt32(e.Parameters), "NIK")
        'Session("masterKey") = masterKey
        Using CmdGrid As New SqlClient.SqlCommand
            With CmdGrid
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT " & _
                               "EmpPekerjaanH.NIK, " & _
                               "EmpPekerjaanH.PrdAwal as [Tanggal Dimulai], " & _
                               "EmpPekerjaanH.PrdAkhir as [Tanggal Berakhir], " & _
                               "EmpPekerjaanH.Perusahaan, " & _
                               "EmpPekerjaanH.Alamat, " & _
                               "EmpPekerjaanH.Industri, " & _
                               "EmpPekerjaanH.Jabatan, " & _
                               "EmpPekerjaanH.UraianPekerjaan as [Uraian Pekerjaan] " & _
                               "FROM EmpPekerjaanH " & _
                               "WHERE NIK=@P1"
                .Parameters.AddWithValue("@P1", masterKey)
            End With
            Using DaGrid As New SqlClient.SqlDataAdapter
                DaGrid.SelectCommand = CmdGrid
                Using DsGrid As New DataSet
                    DaGrid.Fill(DsGrid)
                    With GridRwytPekerjaan
                        .DataSource = DsGrid
                        .DataBind()
                        .Columns(0).Visible = False
                    End With
                End Using
            End Using
        End Using
    End Sub

    'Protected Sub BtnSimpan_Click(sender As Object, e As EventArgs) Handles BtnSimpan.Click
    '    'Menyimpan input data karyawan ke dalam database
    'End Sub
    Protected Sub GridPendidikan_CustomCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs)
        'Dim masterKey As Integer = GridMaster.GetRowValues(0, "NIK")
        Dim masterKey As Integer = GridMaster.GetRowValues(Convert.ToInt32(e.Parameters), "NIK")
        'Session("masterKey") = masterKey
        Using CmdGrid As New SqlClient.SqlCommand
            With CmdGrid
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT " & _
                               "EmpPendidikan.NIK, " & _
                               "EmpPendidikan.TgkPendidikan as [Tingkat Pendidikan], " & _
                               "EmpPendidikan.PrdAwal as [Tanggal Dimulai], " & _
                               "EmpPendidikan.PrdAkhir as [Tanggal Berakhir], " & _
                               "EmpPendidikan.Institusi, " & _
                               "EmpPendidikan.Alamat, " & _
                               "EmpPendidikan.Jurusan, " & _
                               "EmpPendidikan.LlsTdkLls as Status, " & _
                               "EmpPendidikan.IPK as Nilai, " & _
                               "EmpPendidikan.NoIjazah as [Nomor Ijazah] " & _
                               "FROM EmpPendidikan " & _
                               "WHERE NIK=@P1"
                .Parameters.AddWithValue("@P1", masterKey)
            End With
            Using DaGrid As New SqlClient.SqlDataAdapter
                DaGrid.SelectCommand = CmdGrid
                Using DsGrid As New DataSet
                    DaGrid.Fill(DsGrid)
                    With GridPendidikan
                        .DataSource = DsGrid
                        .DataBind()
                        .Columns(0).Visible = False
                    End With
                End Using
            End Using
        End Using
    End Sub
    Protected Sub GridKetrampilan_CustomCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs)
        'Dim masterKey As Integer = GridMaster.GetRowValues(0, "NIK")
        Dim masterKey As Integer = GridMaster.GetRowValues(Convert.ToInt32(e.Parameters), "NIK")
        'Session("masterKey") = masterKey
        Using CmdGrid As New SqlClient.SqlCommand
            With CmdGrid
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT " & _
                               "EmpKetrampilan.NIK, " & _
                               "EmpKetrampilan.Nama as [Nama Ketrampilan], " & _
                               "EmpKetrampilan.PrdAwal as [Tanggal Dimulai], " & _
                               "EmpKetrampilan.PrdAkhir as [Tanggal Berakhir], " & _
                               "EmpKetrampilan.Sertifikat as [Nama Sertifikat], " & _
                               "EmpKetrampilan.Grade as [Level / Grade], " & _
                               "EmpKetrampilan.NoSertifikat as [No. Sertifikat], " & _
                               "EmpKetrampilan.Institusi as [Diterbitkan Oleh] " & _
                               "FROM EmpKetrampilan " & _
                               "WHERE NIK=@P1"
                .Parameters.AddWithValue("@P1", masterKey)
            End With
            Using DaGrid As New SqlClient.SqlDataAdapter
                DaGrid.SelectCommand = CmdGrid
                Using DsGrid As New DataSet
                    DaGrid.Fill(DsGrid)
                    With GridKetrampilan
                        .DataSource = DsGrid
                        .DataBind()
                        .Columns(0).Visible = False
                    End With
                End Using
            End Using
        End Using
    End Sub
    Protected Sub GridIdentitas_CustomCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs)
        'Dim masterKey As Integer = GridMaster.GetRowValues(0, "NIK")
        Dim masterKey As Integer = GridMaster.GetRowValues(Convert.ToInt32(e.Parameters), "NIK")
        'Session("masterKey") = masterKey
        Using CmdGrid As New SqlClient.SqlCommand
            With CmdGrid
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT " & _
                               "EmpID.NIK, " & _
                               "EmpID.JenisID as [Jenis ID], " & _
                               "EmpID.NoID as [No ID], " & _
                               "EmpID.PrdAwal as [Tanggal Mulai Berlaku], " & _
                               "EmpID.PrdAkhir as [Tanggal Akhir Berlaku], " & _
                               "EmpID.DiterbitkanOleh as [Diterbitkan Oleh] " & _
                               "FROM EmpID " & _
                               "WHERE NIK=@P1"
                .Parameters.AddWithValue("@P1", masterKey)
            End With
            Using DaGrid As New SqlClient.SqlDataAdapter
                DaGrid.SelectCommand = CmdGrid
                Using DsGrid As New DataSet
                    DaGrid.Fill(DsGrid)
                    With GridIdentitas
                        .DataSource = DsGrid
                        .DataBind()
                        .Columns(0).Visible = False
                    End With
                End Using
            End Using
        End Using
    End Sub
    Protected Sub GridKeluarga_CustomCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs)
        'Dim masterKey As Integer = GridMaster.GetRowValues(0, "NIK")
        Dim masterKey As Integer = GridMaster.GetRowValues(Convert.ToInt32(e.Parameters), "NIK")
        'Session("masterKey") = masterKey
        Using CmdGrid As New SqlClient.SqlCommand
            With CmdGrid
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT " & _
                               "EmpKeluarga.NIK, " & _
                               "EmpKeluarga.Hubungan, " & _
                               "EmpKeluarga.Nama, " & _
                               "EmpKeluarga.Kelamin as [Jenis Kelamin], " & _
                               "EmpKeluarga.TglLahir as [Tanggal Lahir], " & _
                               "EmpKeluarga.Pekerjaan, " & _
                               "EmpKeluarga.Perusahaan " & _
                               "FROM EmpKeluarga " & _
                               "WHERE NIK=@P1"
                .Parameters.AddWithValue("@P1", masterKey)
            End With
            Using DaGrid As New SqlClient.SqlDataAdapter
                DaGrid.SelectCommand = CmdGrid
                Using DsGrid As New DataSet
                    DaGrid.Fill(DsGrid)
                    With GridKeluarga
                        .DataSource = DsGrid
                        .DataBind()
                        .Columns(0).Visible = False
                    End With
                End Using
            End Using
        End Using
    End Sub
    Protected Sub GridMinarta_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim grid = TryCast(sender, ASPxGridView)
        For i As Integer = 0 To grid.Columns.Count - 1
            If grid.DataColumns(i).FieldName = "Tanggal Dimulai" _
               Or grid.DataColumns(i).FieldName = "Tanggal Berakhir" Then
                grid.DataColumns(i).PropertiesEdit.DisplayFormatString = "dd-MMM-yyyy"
            End If
        Next
    End Sub
    Protected Sub GridRwytPekerjaan_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim grid = TryCast(sender, ASPxGridView)
        For i As Integer = 0 To grid.Columns.Count - 1
            If grid.DataColumns(i).FieldName = "Tanggal Dimulai" _
               Or grid.DataColumns(i).FieldName = "Tanggal Berakhir" Then
                grid.DataColumns(i).PropertiesEdit.DisplayFormatString = "dd-MMM-yyyy"
            End If
        Next
    End Sub
    Protected Sub GridPendidikan_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim grid = TryCast(sender, ASPxGridView)
        For i As Integer = 0 To grid.Columns.Count - 1
            If grid.DataColumns(i).FieldName = "Tanggal Dimulai" _
               Or grid.DataColumns(i).FieldName = "Tanggal Berakhir" Then
                grid.DataColumns(i).PropertiesEdit.DisplayFormatString = "dd-MMM-yyyy"
            End If
        Next
    End Sub
    Protected Sub GridKetrampilan_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim grid = TryCast(sender, ASPxGridView)
        For i As Integer = 0 To grid.Columns.Count - 1
            If grid.DataColumns(i).FieldName = "Tanggal Dimulai" _
               Or grid.DataColumns(i).FieldName = "Tanggal Berakhir" Then
                grid.DataColumns(i).PropertiesEdit.DisplayFormatString = "dd-MMM-yyyy"
            End If
        Next
    End Sub
    Protected Sub GridIdentitas_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim grid = TryCast(sender, ASPxGridView)
        For i As Integer = 0 To grid.Columns.Count - 1
            If grid.DataColumns(i).FieldName = "Tanggal Mulai Berlaku" _
               Or grid.DataColumns(i).FieldName = "Tanggal Akhir Berlaku" Then
                grid.DataColumns(i).PropertiesEdit.DisplayFormatString = "dd-MMM-yyyy"
            End If
        Next
    End Sub

    Protected Sub GridKeluarga_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim grid = TryCast(sender, ASPxGridView)
        For i As Integer = 0 To grid.Columns.Count - 1
            If grid.DataColumns(i).FieldName = "Tanggal Lahir" Then
                grid.DataColumns(i).PropertiesEdit.DisplayFormatString = "dd-MMM-yyyy"
            End If
        Next

    End Sub

    Protected Sub GridKeluarga_CustomColumnDisplayText(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs) Handles GridKeluarga.CustomColumnDisplayText
        If e.Column.FieldName = "Jenis Kelamin" Then
            If e.Value = "L" Then
                e.DisplayText = "Pria"
            Else
                e.DisplayText = "Wanita"
            End If
        End If
    End Sub

    Private Sub GridMaster_HtmlRowPrepared(sender As Object, e As DevExpress.Web.ASPxGridViewTableRowEventArgs) Handles GridMaster.HtmlRowPrepared
        If e.RowType <> GridViewRowType.Data Then
            Return
        End If        
        If e.GetValue("Active") = "0" Then
            e.Row.Font.Strikeout = True
            e.Row.Font.Italic = True
            e.Row.ForeColor = System.Drawing.Color.IndianRed
        End If
    End Sub

    Private Sub GridMaster_PageIndexChanged1(sender As Object, e As System.EventArgs) Handles GridMaster.PageIndexChanged
        Session("MasterPage") = TryCast(sender, ASPxGridView).PageIndex
    End Sub

    Private Sub GridMaster_PreRender(sender As Object, e As System.EventArgs) Handles GridMaster.PreRender
        GridMaster.FocusedRowIndex = -1
    End Sub
    Protected Sub GridDokumenPendukung_CustomCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs)
        GridDokumenPendukung.JSProperties("cpOpenWindow") = Nothing
        Dim masterKey As String = GridMaster.GetRowValues(Convert.ToInt32(e.Parameters), "NIK")
        Session("NIK") = masterKey
        Dim TmpDtDP As New DataTable()
        TmpDtDP.Clear()
        If Directory.Exists(Server.MapPath("/PDF/Employee/" & masterKey & "/")) Then
            TmpDtDP.Columns.AddRange(New DataColumn() {New DataColumn("NamaFile", GetType(String))})
            For Each FileDP As String In Directory.GetFiles(Server.MapPath("/PDF/Employee/" & masterKey & "/"))
                Dim NamaFileDP() As String = FileDP.Split("\")
                Dim arrayAkhir As Integer = NamaFileDP.Length()
                If Mid(NamaFileDP(arrayAkhir - 1), 1, 6) = masterKey Then Continue For
                TmpDtDP.Rows.Add(NamaFileDP(arrayAkhir - 1))
            Next
        End If
        With GridDokumenPendukung
            .DataSource = TmpDtDP
            .DataBind()
        End With
    End Sub
    Protected Sub GridDokumenPendukung_CustomButtonCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs)
        If e.ButtonID = "ViewBaris" Then
            Session.Remove("ViewJPG")
            Session.Remove("ViewPDF")
            Dim ViewNIK As String = Session("NIK")

            Dim ViewFile As String = GridDokumenPendukung.GetRowValues(e.VisibleIndex, "NamaFile")
            Dim FileSplit() As String = ViewFile.Split(".")
            Dim arrayAkhir As Integer = FileSplit.Length - 1
            If FileSplit(arrayAkhir).ToLower = "pdf" Then
                Session("ViewPDF") = "/PDF/Employee/" & ViewNIK & "/" & _
                                        ViewFile
            Else
                Session("ViewJPG") = "/PDF/Employee/" & ViewNIK & "/" & _
                                        ViewFile
            End If

            GridDokumenPendukung.JSProperties("cpOpenWindow") = "FrmView.aspx"

        End If
    End Sub

End Class