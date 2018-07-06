Public Class FrmAbsensiKaryawan
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
            'PrdAwal.Date = DateSerial(Year(Today), Month(Today), 1)
            'PrdAkhir.Date = DateAdd(DateInterval.Month, 1, DateSerial(Year(Today), Month(Today), 0))
            If Month(Today) = 1 Then
                PrdAwal.Date = DateSerial(Year(Today) - 1, 12, 1)
            Else
                PrdAwal.Date = DateSerial(Year(Today), Month(Today) - 1, 1)
            End If

            PrdAkhir.Date = DateSerial(Year(Today), Month(Today), 1).AddDays(-1)

            Call BindKaryawan()
        End If

    End Sub

    Private Sub Page_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        Conn.Close()
        Conn.Dispose()
    End Sub

    Private Sub BtnPrint_Click(sender As Object, e As System.EventArgs) Handles BtnPrint.Click
        If PrdAkhir.Date < PrdAwal.Date Then
            LblErr.Text = "Periode invalid."
            ErrMsg.ShowOnPageLoad = True
            Exit Sub
        End If

        Session("Print") = PrdAwal.Date & "|" & PrdAkhir.Date & "|" & DDLKaryawan.Text
        Response.Redirect("FrmRptAbsensiKaryawan.aspx")
    End Sub

    Private Sub BindKaryawan()
        DDLKaryawan.Items.Clear()
        DDLKaryawan.Text = ""

        Using CmdFind As New Data.SqlClient.SqlCommand
            With CmdFind
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT * FROM Karyawan ORDER BY Nama"
            End With
            Using RsFind As Data.SqlClient.SqlDataReader = CmdFind.ExecuteReader
                While RsFind.Read
                    DDLKaryawan.Items.Add(RsFind("NIK") + " - " + RsFind("Nama"), RsFind("NIK"))
                End While
            End Using
        End Using

    End Sub

End Class