Public Class FrmRptAbsensiKaryawan
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

        Call BindReport()

    End Sub

    Private Sub Page_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        Conn.Close()
        Conn.Dispose()
    End Sub

    Private Sub BindReport()
        Dim PrdAwal As Date = Session("Print").ToString.Split("|")(0)
        Dim PrdAkhir As Date = Session("Print").ToString.Split("|")(1)
        Dim Karyawan As String = Session("Print").ToString.Split("|")(2)

        Using CmdReport As New Data.SqlClient.SqlCommand
            With CmdReport
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT * FROM Absensi WHERE NIK=@P1 AND TglMasuk>=@P2 AND TglMasuk<=@P3"
                .Parameters.AddWithValue("@P1", Trim(Karyawan.Split("-")(0)))
                .Parameters.AddWithValue("@P2", PrdAwal)
                .Parameters.AddWithValue("@P3", PrdAkhir)
                Using DaReport As New Data.SqlClient.SqlDataAdapter
                    DaReport.SelectCommand = CmdReport
                    Using DtReport As New Data.DataTable
                        DaReport.Fill(DtReport)

                        Using Rpt As New RptAbsensiKaryawan
                            With Rpt
                                .SetDataSource(DtReport)
                                .SetParameterValue("@Periode", Format(PrdAwal.Date, "dd-MMM-yyyy") + " s.d. " + Format(PrdAkhir.Date, "dd-MMM-yyyy"))
                                .SetParameterValue("@TtlHariKerja", JmlHariKerja(PrdAwal.Date, PrdAkhir.Date).ToString + " hari")
                                .SetParameterValue("@NIK", Karyawan)
                                .SetParameterValue("@UserEntry", Session("User").ToString.Split("|")(0))
                            End With

                            CRViewer.ReportSource = Rpt
                            Rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, False, "")

                        End Using
                    End Using
                End Using
            End With
            
        End Using

    End Sub

    Private Function JmlHariKerja(ByVal PrdAwal As Date, ByVal PrdAkhir As Date) As Integer
        Dim start As Date = PrdAwal.Date
        Dim [end] As Date = PrdAkhir.Date.AddDays(1)
        Dim workingDays As Integer = 0

        While start < [end]
            If start.DayOfWeek <> DayOfWeek.Saturday AndAlso start.DayOfWeek <> DayOfWeek.Sunday Then
                workingDays += 1
            End If
            start = start.AddDays(1)
        End While

        Using CmdFind As New Data.SqlClient.SqlCommand
            With CmdFind
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT TglLibur FROM HariLibur WHERE TglLibur>=@P1 AND TglLibur<=@P2"
                .Parameters.AddWithValue("@P1", PrdAwal.Date)
                .Parameters.AddWithValue("@P2", PrdAkhir.Date)
            End With
            Using RsFind As Data.SqlClient.SqlDataReader = CmdFind.ExecuteReader
                While RsFind.Read
                    If RsFind("TglLibur").DayOfWeek <> DayOfWeek.Saturday AndAlso RsFind("TglLibur").DayOfWeek <> DayOfWeek.Sunday Then
                        workingDays -= 1
                    End If
                End While
            End Using
        End Using

        Return workingDays

    End Function

End Class