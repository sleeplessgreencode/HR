Public Class FrmHariLibur
    Inherits System.Web.UI.Page
    Dim Conn As New Data.SqlClient.SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("User") = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        Else
            Dim UserId As String = Session("User").ToString.Split("|")(1)
            If CheckAkses1(UserId, "HariLibur") = False Then
                Response.Redirect("Default.aspx")
                Exit Sub
            End If
        End If

        Conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnStr").ToString
        Conn.Open()

        If IsPostBack = False Then
            TxtTahun.Value = Today.Year
            Call BindGrid()
        End If

    End Sub

    Private Sub Page_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        Conn.Close()
        Conn.Dispose()
    End Sub

    Private Sub BindGrid()
        Dim PrdAwal As Date = DateSerial(TxtTahun.Value, 1, 1)
        Dim PrdAkhir As Date = DateSerial(TxtTahun.Value + 1, 1, 0)
        
        Using CmdGrid As New Data.SqlClient.SqlCommand
            With CmdGrid
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT * FROM HariLibur WHERE TglLibur>=@P1 AND TglLibur<=@P2"
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

    Private Sub TxtTahun_ValueChanged(sender As Object, e As System.EventArgs) Handles TxtTahun.ValueChanged
        Call BindGrid()
    End Sub

    Private Sub BtnAdd_Click(sender As Object, e As System.EventArgs) Handles BtnAdd.Click
        TxtAction.Text = "NEW"
        Tgl1.Date = Today
        CheckMultiple.Value = False
        CheckMultiple_CheckedChanged(CheckMultiple, New EventArgs)
        CheckMultiple.Enabled = True
        TxtKeterangan.Text = ""

        PopEntry.ShowOnPageLoad = True
    End Sub

    Private Sub GridData_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridData.RowCommand
        If e.CommandName = "BtnUpdate" Then
            Dim SelectRecord As GridViewRow = GridData.Rows(e.CommandArgument)

            Session("Libur") = CDate(SelectRecord.Cells(1).Text)
            TxtAction.Text = "UPD"
            Tgl1.Date = SelectRecord.Cells(1).Text
            TxtKeterangan.Text = SelectRecord.Cells(2).Text
            CheckMultiple.Value = False
            CheckMultiple_CheckedChanged(CheckMultiple, New EventArgs)
            CheckMultiple.Enabled = False
            PopEntry.ShowOnPageLoad = True
        ElseIf e.CommandName = "BtnDelete" Then
            Dim SelectRecord As GridViewRow = GridData.Rows(e.CommandArgument)

            Session("Libur") = CDate(SelectRecord.Cells(1).Text)
            Using CmdDelete As New Data.SqlClient.SqlCommand
                With CmdDelete
                    .Connection = Conn
                    .CommandType = CommandType.Text
                    .CommandText = "DELETE FROM HariLibur WHERE TglLibur=@P1"
                    .Parameters.AddWithValue("@P1", Session("Libur"))
                    Try
                        .ExecuteNonQuery()
                    Catch
                        LblErr.Text = Err.Description
                        ErrMsg.ShowOnPageLoad = True
                        Exit Sub
                    End Try
                    .Dispose()
                End With
            End Using

            Session.Remove("Libur")
            Call BindGrid()


        End If
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As System.EventArgs) Handles BtnSave.Click
        If CheckMultiple.Value = True And (Tgl2.Value < Tgl1.Value) Then
            LblErr.Text = "Periode invalid."
            ErrMsg.ShowOnPageLoad = True
            Exit Sub
        End If

        If TxtKeterangan.Text = "" Then
            LblErr.Text = "Keterangan belum diisi."
            ErrMsg.ShowOnPageLoad = True
            Exit Sub
        End If

        If TxtAction.Text = "NEW" Then
            If Validasi(Tgl1.Date) = False Then
                LblErr.Text = "Sudah ada hari libur tanggal " + Format(Tgl1.Date, "dd-MMM-yyy") + "."
                ErrMsg.ShowOnPageLoad = True
                Exit Sub
            End If

            If CheckMultiple.Value = False Then
                Using CmdInsert As New Data.SqlClient.SqlCommand
                    With CmdInsert
                        .Connection = Conn
                        .CommandType = CommandType.Text
                        .CommandText = "INSERT INTO HariLibur (TglLibur,Keterangan,UserEntry,TimeEntry) VALUES(@P1,@P2,@P3,@P4)"
                        .Parameters.AddWithValue("@P1", Tgl1.Date)
                        .Parameters.AddWithValue("@P2", TxtKeterangan.Text)
                        .Parameters.AddWithValue("@P3", Session("User").ToString.Split("|")(0))
                        .Parameters.AddWithValue("@P4", Now)
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
                Dim TglLibur As Date = Tgl1.Date

                While TglLibur.Date <= Tgl2.Date
                    If Validasi(TglLibur) = True Then
                        Using CmdInsert As New Data.SqlClient.SqlCommand
                            With CmdInsert
                                .Connection = Conn
                                .CommandType = CommandType.Text
                                .CommandText = "INSERT INTO HariLibur (TglLibur,Keterangan,UserEntry,TimeEntry) VALUES(@P1,@P2,@P3,@P4)"
                                .Parameters.AddWithValue("@P1", TglLibur.Date)
                                .Parameters.AddWithValue("@P2", TxtKeterangan.Text)
                                .Parameters.AddWithValue("@P3", Session("User").ToString.Split("|")(0))
                                .Parameters.AddWithValue("@P4", Now)
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
                    TglLibur = TglLibur.AddDays(1)
                End While
            End If
            
        Else
            If Tgl1.Date <> Session("Libur") Then
                If Validasi(Tgl1.Date) = False Then
                    Session.Remove("Libur")
                    LblErr.Text = "Sudah ada hari libur tanggal " + Format(Tgl1.Date, "dd-MMM-yyy") + "."
                    ErrMsg.ShowOnPageLoad = True
                    Exit Sub
                End If
            End If
            Using CmdEdit As New Data.SqlClient.SqlCommand
                With CmdEdit
                    .Connection = Conn
                    .CommandType = CommandType.Text
                    .CommandText = "UPDATE HariLibur SET TglLibur=@P1,Keterangan=@P2,UserEntry=@P3,TimeEntry=@P4 WHERE " & _
                                   "TglLibur=@P5"
                    .Parameters.AddWithValue("@P1", Tgl1.Date)
                    .Parameters.AddWithValue("@P2", TxtKeterangan.Text)
                    .Parameters.AddWithValue("@P3", Session("User").ToString.Split("|")(0))
                    .Parameters.AddWithValue("@P4", Now)
                    .Parameters.AddWithValue("@P5", Session("Libur"))
                    Try
                        .ExecuteNonQuery()
                    Catch ex As Exception
                        LblErr.Text = Err.Description
                        ErrMsg.ShowOnPageLoad = True
                        Exit Sub
                    End Try
                End With
            End Using
            Session.Remove("Libur")
        End If

        PopEntry.ShowOnPageLoad = False
        Call BindGrid()
    End Sub

    Private Sub CheckMultiple_CheckedChanged(sender As Object, e As System.EventArgs) Handles CheckMultiple.CheckedChanged
        If CheckMultiple.Value = True Then
            LblTgl2.Visible = True
            Tgl2.Visible = True
        Else
            LblTgl2.Visible = False
            Tgl2.Visible = False
        End If
    End Sub

    Private Function Validasi(ByVal Tgl1 As Date) As Boolean
        Using CmdFind As New Data.SqlClient.SqlCommand
            With CmdFind
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT * FROM HariLibur WHERE TglLibur=@P1"
                .Parameters.AddWithValue("@P1", Tgl1.Date)
            End With
            Using RsFind As Data.SqlClient.SqlDataReader = CmdFind.ExecuteReader
                If RsFind.Read Then Return False
            End Using
        End Using

        Return True

    End Function
End Class