Public Class FrmAbsensi
    Inherits System.Web.UI.Page
    Dim Conn As New Data.SqlClient.SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("User") = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        Else
            Dim UserId As String = Session("User").ToString.Split("|")(1)
            If CheckAkses1(UserId, "AbsensiKaryawan") = False Then
                Response.Redirect("Default.aspx")
                Exit Sub
            End If
        End If

        Conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnStr").ToString
        Conn.Open()

        If IsPostBack = False Then
            If Month(Today) = 1 Then
                PrdAwal.Date = DateSerial(Year(Today) - 1, 12, 1)
            Else
                PrdAwal.Date = DateSerial(Year(Today), Month(Today) - 1, 1)
            End If

            PrdAkhir.Date = DateSerial(Year(Today), Month(Today), 1).AddDays(-1)

            Call BindGrid()
        End If
    End Sub

    Private Sub Page_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        Conn.Close()
        Conn.Dispose()
    End Sub

    Private Sub BindGrid()
        Using CmdGrid As New Data.SqlClient.SqlCommand
            With CmdGrid
                .Connection = Conn
                .CommandType = CommandType.Text
                If GetFilter1() <> "" Or GetFilter2() <> "" Then
                    .CommandText = "SELECT A.*, B.Nama FROM Absensi A JOIN Karyawan B ON A.NIK=B.NIK WHERE TglMasuk>=@P1 AND TglMasuk<=@P2" + GetFilter1() + GetFilter2() + " ORDER BY A.TglMasuk DESC"
                Else
                    .CommandText = "SELECT A.*, B.Nama FROM Absensi A JOIN Karyawan B ON A.NIK=B.NIK WHERE TglMasuk>=@P1 AND TglMasuk<=@P2 ORDER BY TglMasuk DESC"
                End If
                .Parameters.AddWithValue("@P1", PrdAwal.Date)
                .Parameters.AddWithValue("@P2", PrdAkhir.Date)
            End With
            'LblErr.Text = CmdGrid.CommandText
            'ErrMsg.ShowOnPageLoad = True
            'Exit Sub
            Using DaGrid As New Data.SqlClient.SqlDataAdapter
                DaGrid.SelectCommand = CmdGrid
                Using DtGrid As New Data.DataTable
                    DaGrid.Fill(DtGrid)
                    GridData.DataSource = DtGrid
                    GridData.DataBind()
                End Using
            End Using
        End Using
    End Sub

    Private Sub GridData_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridData.PageIndexChanging
        Call BindGrid()
        GridData.PageIndex = e.NewPageIndex
        GridData.DataBind()
    End Sub

    Private Function GetFilter1() As String
        If DDLField1.Value <> "0" Then
            Return " AND " + DDLField1.Value + GetFilterFunction1()
        End If

        Return ""
    End Function

    Private Function GetFilterFunction1() As String
        If DDLFilterBy1.Value = "0" Then
            If DDLField1.Value = "TglMasuk" Then
                Return "='" + TglFilter1.Date + "'"
            ElseIf DDLField1.Value = "JamMasuk" Or DDLField1.Value = "JamKeluar" Then
                Return "='" + TimeFilter1.Value + "'"
            Else
                Return "='" + TxtFind1.Value + "'"
            End If
        ElseIf DDLFilterBy1.Value = "1" Then
            Return " LIKE '%" + TxtFind1.Text + "%'"
        ElseIf DDLFilterBy1.Value = "2" Then
            If DDLField1.Value = "TglMasuk" Then
                Return ">='" + TglFilter1.Date + "'"
            ElseIf DDLField1.Value = "JamMasuk" Or DDLField1.Value = "JamKeluar" Then
                Return ">='" + TimeFilter1.Value + "'"
            Else
                Return ">='" + TxtFind1.Value + "'"
            End If
        ElseIf DDLFilterBy1.Value = "3" Then
            If DDLField1.Value = "TglMasuk" Then
                Return "<='" + TglFilter1.Date + "'"
            ElseIf DDLField1.Value = "JamMasuk" Or DDLField1.Value = "JamKeluar" Then
                Return "<='" + TimeFilter1.Value + "'"
            Else
                Return "<='" + TxtFind1.Value + "'"
            End If
        End If

        Return ""
    End Function

    Protected Sub DDLField1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDLField1.SelectedIndexChanged
        TxtFind1.Text = ""
        TglFilter1.Text = ""
        TimeFilter1.Text = ""
        DDLFilterBy1.Items.Clear()
        TxtFind1.Visible = True
        TglFilter1.Visible = False
        TimeFilter1.Visible = False
        If DDLField1.Value = "TglMasuk" Then
            TxtFind1.Visible = False
            TglFilter1.Visible = True
            DDLFilterBy1.Items.Add("Equals", "0")
            DDLFilterBy1.Items.Add("Is Greather Than Or Equal To", "2")
            DDLFilterBy1.Items.Add("Is Less Than Or Equal To", "3")
        ElseIf DDLField1.Value = "JamMasuk" Or DDLField1.Value = "JamKeluar" Then
            TxtFind1.Visible = False
            TimeFilter1.Visible = True
            DDLFilterBy1.Items.Add("Equals", "0")
            DDLFilterBy1.Items.Add("Is Greather Than Or Equal To", "2")
            DDLFilterBy1.Items.Add("Is Less Than Or Equal To", "3")
        Else
            DDLFilterBy1.Items.Add("Equals", "0")
            DDLFilterBy1.Items.Add("Contains", "1")
        End If
        DDLFilterBy1.Value = "0"
    End Sub

    Protected Sub DDLField2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDLField2.SelectedIndexChanged
        TxtFind2.Text = ""
        TglFilter2.Text = ""
        TimeFilter2.Text = ""
        DDLFilterBy2.Items.Clear()
        TxtFind2.Visible = True
        TglFilter2.Visible = False
        TimeFilter2.Visible = False
        If DDLField2.Value = "TglMasuk" Then
            TxtFind2.Visible = False
            TglFilter2.Visible = True
            DDLFilterBy2.Items.Add("Equals", "0")
            DDLFilterBy2.Items.Add("Is Greather Than Or Equal To", "2")
            DDLFilterBy2.Items.Add("Is Less Than Or Equal To", "3")
        ElseIf DDLField2.Value = "JamMasuk" Or DDLField2.Value = "JamKeluar" Then
            TxtFind2.Visible = False
            TimeFilter2.Visible = True
            DDLFilterBy2.Items.Add("Equals", "0")
            DDLFilterBy2.Items.Add("Is Greather Than Or Equal To", "2")
            DDLFilterBy2.Items.Add("Is Less Than Or Equal To", "3")
        Else
            DDLFilterBy2.Items.Add("Equals", "0")
            DDLFilterBy2.Items.Add("Contains", "1")
        End If
        DDLFilterBy2.Value = "0"
    End Sub

    Private Function GetFilter2() As String
        If DDLField2.Value <> "0" Then
            Return " AND " + DDLField2.Value + " " + GetFilterFunction2()
        End If

        Return ""
    End Function

    Private Function GetFilterFunction2() As String
        If DDLFilterBy2.Value = "0" Then
            If DDLField2.Value = "TglMasuk" Then
                Return "='" + TglFilter2.Date + "'"
            ElseIf DDLField2.Value = "JamMasuk" Or DDLField2.Value = "JamKeluar" Then
                Return "='" + TimeFilter2.Value + "'"
            Else
                Return "='" + TxtFind2.Value + "'"
            End If
        ElseIf DDLFilterBy2.Value = "1" Then
            Return " LIKE '%" + TxtFind2.Text + "%'"
        ElseIf DDLFilterBy2.Value = "2" Then
            If DDLField2.Value = "TglMasuk" Then
                Return ">='" + TglFilter2.Date + "'"
            ElseIf DDLField2.Value = "JamMasuk" Or DDLField2.Value = "JamKeluar" Then
                Return ">='" + TimeFilter2.Value + "'"
            Else
                Return ">='" + TxtFind2.Value + "'"
            End If
        ElseIf DDLFilterBy2.Value = "3" Then
            If DDLField2.Value = "TglMasuk" Then
                Return "<='" + TglFilter2.Date + "'"
            ElseIf DDLField2.Value = "JamMasuk" Or DDLField2.Value = "JamKeluar" Then
                Return "<='" + TimeFilter2.Value + "'"
            Else
                Return "<='" + TxtFind2.Value + "'"
            End If
        End If

        Return ""
    End Function

    Private Sub BtnFind_Click(sender As Object, e As System.EventArgs) Handles BtnFind.Click
        Call BindGrid()
    End Sub

    Private Sub BtnAdd_Click(sender As Object, e As System.EventArgs) Handles BtnAdd.Click
        TxtAction.Text = "NEW"
        Call BindKaryawan()
        DDLKaryawan.Value = "0"
        DDLKaryawan.Enabled = True
        TglAbsen.Date = Today
        TglAbsen.Enabled = True
        DDLStatus.Value = "Masuk"
        JamMasuk.Text = "00:00"
        JamPulang.Text = "00:00"
        TxtKeterangan.Text = ""

        PopEntry.ShowOnPageLoad = True
    End Sub

    Private Sub BindKaryawan()
        DDLKaryawan.Items.Clear()
        DDLKaryawan.Items.Add("Pilih salah satu", "0")

        Using CmdBind As New Data.SqlClient.SqlCommand
            With CmdBind
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT NIK, Nama FROM Karyawan"
            End With
            Using RsBind As Data.SqlClient.SqlDataReader = CmdBind.ExecuteReader
                While RsBind.Read
                    DDLKaryawan.Items.Add(RsBind("NIK") & " - " & RsBind("Nama"), RsBind("NIK"))
                End While
            End Using
        End Using
    End Sub

    Private Sub GridData_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridData.RowCommand
        If e.CommandName = "BtnUpdate" Then
            Dim SelectRecord As GridViewRow = GridData.Rows(e.CommandArgument)

            TxtAction.Text = "UPD"
            Call BindKaryawan()
            Using CmdFind As New Data.SqlClient.SqlCommand
                With CmdFind
                    .Connection = Conn
                    .CommandType = CommandType.Text
                    .CommandText = "SELECT *, CONVERT(VARCHAR(5),JamMasuk) AS 'Jam1', CONVERT(VARCHAR(5),JamKeluar) AS 'Jam2' FROM Absensi WHERE NIK=@P1 AND TglMasuk=@P2"
                    .Parameters.AddWithValue("@P1", SelectRecord.Cells(0).Text)
                    .Parameters.AddWithValue("@P2", SelectRecord.Cells(2).Text)
                End With
                Using RsFind As Data.SqlClient.SqlDataReader = CmdFind.ExecuteReader
                    If RsFind.Read Then
                        DDLKaryawan.Value = RsFind("NIK")
                        DDLKaryawan.Enabled = False
                        DDLStatus.Value = RsFind("Status").ToString
                        TglAbsen.Date = RsFind("TglMasuk")
                        TglAbsen.Enabled = False
                        JamMasuk.Text = RsFind("Jam1")
                        JamPulang.Text = RsFind("Jam2")
                        TxtKeterangan.Text = RsFind("Keterangan").ToString
                        PopEntry.ShowOnPageLoad = True
                    End If
                End Using
            End Using

        End If
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As System.EventArgs) Handles BtnSave.Click
        If DDLKaryawan.Value = "0" Then
            LblErr.Text = "NIK belum dipilih."
            ErrMsg.ShowOnPageLoad = True
            Exit Sub
        End If
        If JamPulang.Value < JamMasuk.Value Then
            LblErr.Text = "Jam pulang < jam masuk."
            ErrMsg.ShowOnPageLoad = True
            Exit Sub
        End If

        If TxtAction.Text = "NEW" Then
            Using CmdFind As New Data.SqlClient.SqlCommand
                With CmdFind
                    .Connection = Conn
                    .CommandType = CommandType.Text
                    .CommandText = "SELECT * FROM Absensi WHERE NIK=@P1 AND TglMasuk=@P2"
                    .Parameters.AddWithValue("@P1", DDLKaryawan.Value)
                    .Parameters.AddWithValue("@P2", TglAbsen.Value)
                End With
                Using RsFind As Data.SqlClient.SqlDataReader = CmdFind.ExecuteReader
                    If RsFind.Read Then
                        LblErr.Text = "Sudah ada absensi untuk <br />" + DDLKaryawan.SelectedItem.Text + "<br /> pd tanggal " + _
                                      Format(TglAbsen.Value, "dd-MMM-yyyy") + "."
                        ErrMsg.ShowOnPageLoad = True
                        Exit Sub
                    End If
                End Using
            End Using

            Using CmdInsert As New Data.SqlClient.SqlCommand
                With CmdInsert
                    .Connection = Conn
                    .CommandType = CommandType.Text
                    .CommandText = "INSERT INTO Absensi (NIK,TglMasuk,JamMasuk,JamKeluar,Status,Keterangan,UserEntry,TimeEntry) VALUES(@P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8)"
                    .Parameters.AddWithValue("@P1", DDLKaryawan.Value)
                    .Parameters.AddWithValue("@P2", TglAbsen.Value)
                    .Parameters.AddWithValue("@P3", JamMasuk.Text)
                    .Parameters.AddWithValue("@P4", JamPulang.Text)
                    .Parameters.AddWithValue("@P5", DDLStatus.Value)
                    .Parameters.AddWithValue("@P6", TxtKeterangan.Text)
                    .Parameters.AddWithValue("@P7", Session("User").ToString.Split("|")(0))
                    .Parameters.AddWithValue("@P8", Now)
                    Try
                        .ExecuteNonQuery()
                    Catch ex As Exception
                        LblErr.Text = Err.Description
                        ErrMsg.ShowOnPageLoad = True
                        Exit Sub
                    End Try
                End With
            End Using
        Else
            Using CmdEdit As New Data.SqlClient.SqlCommand
                With CmdEdit
                    .Connection = Conn
                    .CommandType = CommandType.Text
                    .CommandText = "UPDATE Absensi SET JamMasuk=@P1,JamKeluar=@P2,Status=@P3,Keterangan=@P4,UserEntry=@P5,TimeEntry=@P6 WHERE " & _
                                   "NIK=@P7 AND TglMasuk=@P8"
                    .Parameters.AddWithValue("@P1", JamMasuk.Text)
                    .Parameters.AddWithValue("@P2", JamPulang.Text)
                    .Parameters.AddWithValue("@P3", DDLStatus.Value)
                    .Parameters.AddWithValue("@P4", TxtKeterangan.Text)
                    .Parameters.AddWithValue("@P5", Session("User").ToString.Split("|")(0))
                    .Parameters.AddWithValue("@P6", Now)
                    .Parameters.AddWithValue("@P7", DDLKaryawan.Value)
                    .Parameters.AddWithValue("@P8", TglAbsen.Date)
                    Try
                        .ExecuteNonQuery()
                    Catch ex As Exception
                        LblErr.Text = Err.Description
                        ErrMsg.ShowOnPageLoad = True
                        Exit Sub
                    End Try
                End With
            End Using
        End If

        PopEntry.ShowOnPageLoad = False
        Call BindGrid()
    End Sub

    Private Sub PrdAwal_ValueChanged(sender As Object, e As System.EventArgs) Handles PrdAwal.ValueChanged
        Call BindGrid()
    End Sub

    Private Sub PrdAkhir_ValueChanged(sender As Object, e As System.EventArgs) Handles PrdAkhir.ValueChanged
        Call BindGrid()
    End Sub

    Private Sub DDLStatus_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLStatus.SelectedIndexChanged
        If DDLStatus.Value = "Dinas Proyek" Then
            JamMasuk.Text = "08:30"
            JamPulang.Text = "17:00"        
        End If
    End Sub

End Class
