Public Class FrmLoadAbsensi
    Inherits System.Web.UI.Page
    Dim Conn As New Data.SqlClient.SqlConnection
    Dim TmpDt As New DataTable()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("User") = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        Else
            Dim UserId As String = Session("User").ToString.Split("|")(1)
            If CheckAkses1(UserId, "LoadAbsensi") = False Then
                Response.Redirect("Default.aspx")
                Exit Sub
            End If
        End If

        Conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnStr").ToString
        Conn.Open()

    End Sub

    Private Sub Page_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        Conn.Close()
        Conn.Dispose()
    End Sub

    Private Sub BtnUpload_Click(sender As Object, e As System.EventArgs) Handles BtnUpload.Click
        If FileUpload1.HasFile = False Then
            LblErr.Text = "File Excel belum dipilih."
            ErrMsg.ShowOnPageLoad = True
            Exit Sub
        End If
        If System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName) <> ".xlsx" And
           System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName) <> ".xls" Then
            LblErr.Text = "File yang di-upload bukan Excel."
            ErrMsg.ShowOnPageLoad = True
            Exit Sub
        End If

        TmpDt.Columns.AddRange(New DataColumn() {New DataColumn("NIK", GetType(String)), _
                                                 New DataColumn("Nama", GetType(String)), _
                                                 New DataColumn("Tanggal", GetType(String)), _
                                                 New DataColumn("Scan Masuk", GetType(String)), _
                                                 New DataColumn("Scan Pulang", GetType(String))})

        Dim FileName As String = "ABSENSI_" & Format(Now, "yyyy-MM-dd") & "_" & Format(Now, "hhmmss") & _
                    System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName)
        Dim FilePath As String = Server.MapPath("~/TMP/") + FileName

        FileUpload1.PostedFile.SaveAs(FilePath)

        Dim conStr As String
        If System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName) = ".xlsx" Then
            conStr = ConfigurationManager.ConnectionStrings("Excel07ConString").ConnectionString()
        Else
            conStr = ConfigurationManager.ConnectionStrings("Excel03ConString").ConnectionString()
        End If

        conStr = String.Format(conStr, FilePath, "Yes")

        Dim connExcel As New Data.OleDb.OleDbConnection(conStr)
        Dim cmdExcel As New Data.OleDb.OleDbCommand()
        Dim oda As New Data.OleDb.OleDbDataAdapter()

        cmdExcel.Connection = connExcel

        'Get the name of First Sheet
        connExcel.Open()

        Dim dtExcelSchema As DataTable
        dtExcelSchema = connExcel.GetOleDbSchemaTable(Data.OleDb.OleDbSchemaGuid.Tables, Nothing)
        Dim SheetName As String = dtExcelSchema.Rows(0)("TABLE_NAME").ToString()
        connExcel.Close()
        
        'Read Data from First Sheet
        connExcel.Open()
        cmdExcel.CommandText = "SELECT * From [" & SheetName & "]"
        oda.SelectCommand = cmdExcel
        oda.Fill(TmpDt)
        connExcel.Close()

        'Bind Data to GridView
        GridData.Caption = System.IO.Path.GetFileName(FilePath)
        GridData.DataSource = TmpDt
        GridData.DataBind()

        cmdExcel.Dispose()
        oda.Dispose()

        If System.IO.File.Exists(FilePath) Then
            System.IO.File.Delete(FilePath)
        End If

        BtnSave.Visible = True

        Session("TmpDt") = TmpDt
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As System.EventArgs) Handles BtnSave.Click
        Dim Count As Integer = 0

        TmpDt = Session("TmpDt")
        'For Each row As DataRow In TmpDt.Rows
        '    If row("NIK").ToString = "" Then Continue For
        '    If row("Scan Masuk").ToString = "" And row("Scan Pulang").ToString = "" Then Continue For
        '    If row("Scan Masuk").ToString = "" Or row("Scan Pulang").ToString = "" Then
        '        LblErr.Text = "Data absensi untuk " + row("Nama") + " belum lengkap. <br/>" + _
        '                      "Tanggal : " + row("Tanggal") + " <br /> " + _
        '                      "Scan Masuk : " + row("Scan Masuk") + " <br /> " + _
        '                      "Scan Pulang : " + row("Scan Pulang")
        '        ErrMsg.ShowOnPageLoad = True
        '        Exit Sub
        '    End If
        'Next

        For Each row As DataRow In TmpDt.Rows
            If row("NIK").ToString = "" Then Continue For
            'If row("Scan Masuk").ToString = "" And row("Scan Pulang").ToString = "" Then Continue For
            If CheckHariLibur(row("Tanggal")) = True Then Continue For
            Using CmdFind As New Data.SqlClient.SqlCommand
                With CmdFind
                    .Connection = Conn
                    .CommandType = CommandType.Text
                    .CommandText = "SELECT * FROM Absensi WHERE NIK=@P1 AND TglMasuk=@P2"
                    .Parameters.AddWithValue("@P1", row("NIK"))
                    .Parameters.AddWithValue("@P2", row("Tanggal"))
                End With
                Using RsFind As Data.SqlClient.SqlDataReader = CmdFind.ExecuteReader
                    If RsFind.Read = False Then
                        Using CmdInsert As New Data.SqlClient.SqlCommand
                            CmdInsert.Parameters.Clear()
                            With CmdInsert
                                .Connection = Conn
                                .CommandType = CommandType.Text
                                .CommandText = "INSERT INTO Absensi(NIK,TglMasuk,JamMasuk,JamKeluar,Status,UserEntry,TimeEntry) " & _
                                    "VALUES(@P1,@P2,@P3,@P4,@P5,@P6,@P7)"
                                .Parameters.AddWithValue("@P1", row("NIK"))
                                .Parameters.AddWithValue("@P2", row("Tanggal"))
                                .Parameters.AddWithValue("@P3", row("Scan Masuk"))
                                .Parameters.AddWithValue("@P4", row("Scan Pulang"))
                                If Not row("Scan Masuk").ToString = "" Then
                                    If TimeSpan.Parse(row("Scan Masuk")) > TimeSpan.Parse("09:30") Then
                                        .Parameters.AddWithValue("@P5", "Ijin 1/2 Hari")
                                    Else
                                        .Parameters.AddWithValue("@P5", "Masuk")
                                    End If
                                Else
                                    .Parameters.AddWithValue("@P5", "Masuk")
                                End If
                                .Parameters.AddWithValue("@P6", Session("User").ToString.Split("|")(0))
                                .Parameters.AddWithValue("@P7", Now)
                                Try
                                    .ExecuteNonQuery()
                                Catch
                                    LblErr.Text = Err.Description
                                    ErrMsg.ShowOnPageLoad = True
                                    Exit Sub
                                End Try
                            End With
                            Count += 1
                        End Using
                    End If
                End Using
            End Using
        Next

        LblErr.Text = CStr(Count) + " records saved."
        ErrMsg.ShowOnPageLoad = True
        TmpDt.Rows.Clear()
        GridData.Caption = ""
        GridData.DataSource = TmpDt
        GridData.DataBind()
        BtnSave.Visible = False

    End Sub

    Private Sub GridData_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridData.PageIndexChanging
        TmpDt = Session("TmpDt")

        GridData.DataSource = TmpDt
        GridData.PageIndex = e.NewPageIndex
        GridData.DataBind()
    End Sub

    Private Function CheckHariLibur(ByVal Tanggal As Date) As Boolean
        Using CmdFind As New Data.SqlClient.SqlCommand
            With CmdFind
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT TglLibur FROM HariLibur WHERE TglLibur=@P1"
                .Parameters.AddWithValue("@P1", Tanggal.Date)
            End With
            Using RsFind As Data.SqlClient.SqlDataReader = CmdFind.ExecuteReader
                If RsFind.Read Then
                    Return True
                End If
            End Using
        End Using

        Return False
    End Function
End Class