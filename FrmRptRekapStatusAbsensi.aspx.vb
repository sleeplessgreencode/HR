Public Class FrmRptRekapStatusAbsensi
    Inherits System.Web.UI.Page
    Dim Conn As New Data.SqlClient.SqlConnection
    Dim Rpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("User") = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        Else
            Dim UserId As String = Session("User").ToString.Split("|")(1)
            If CheckAkses1(UserId, "RekapStatusAbsensi") = False Then
                Response.Redirect("Default.aspx")
                Exit Sub
            End If
        End If

        Conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnStr").ToString
        Conn.Open()

        Call BindReport()

    End Sub

    Private Sub Page_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        Rpt.Close()
        Rpt.Dispose()
        Conn.Close()
        Conn.Dispose()
    End Sub

    Private Sub BindReport()
        Dim TmpDt As New DataTable()
        TmpDt.Columns.AddRange(New DataColumn() {New DataColumn("NIK", GetType(String)), _
                                                 New DataColumn("Nama", GetType(String)), _
                                                 New DataColumn("TtlTelat", GetType(Integer)), _
                                                 New DataColumn("TtlIjin", GetType(String)), _
                                                 New DataColumn("TtlCuti", GetType(Integer)), _
                                                 New DataColumn("TtlSakit", GetType(Integer)), _
                                                 New DataColumn("TtlDinas", GetType(Integer)), _
                                                 New DataColumn("TtlIjinStgh", GetType(Integer)), _
                                                 New DataColumn("TtlAlpa", GetType(Integer)), _
                                                 New DataColumn("TtlTdkAbsen", GetType(Integer)), _
                                                 New DataColumn("WorkingHour", GetType(String))})

        Dim PrdAwal As Date = Session("Print").ToString.Split("|")(0)
        Dim PrdAkhir As Date = Session("Print").ToString.Split("|")(1)
        Dim HakCuti As Integer = 12, TtlTelat As Integer = 0, TtlIjin As Integer = 0, _
            TtlCuti As Integer = 0, TtlSakit As Integer = 0, TtlDinas As Integer = 0, _
            TtlIjinStgh As Decimal = 0, TtlAlpa As Integer = 0, TtlTdkAbsen As Integer = 0
        Dim WorkingHour As String

        Using CmdReport As New Data.SqlClient.SqlCommand
            With CmdReport
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT NIK, Nama FROM Karyawan WHERE Active='1'"
            End With
            Using RsLoad As Data.SqlClient.SqlDataReader = CmdReport.ExecuteReader
                While RsLoad.Read
                    Using CmdFind As New Data.SqlClient.SqlCommand
                        With CmdFind
                            .Connection = Conn
                            .CommandType = CommandType.Text
                            .CommandText = "SELECT * FROM Absensi WHERE NIK=@P1 AND " & _
                                           "TglMasuk>=@P2 AND TglMasuk<=@P3"
                            .Parameters.AddWithValue("@P1", RsLoad("NIK"))
                            .Parameters.AddWithValue("@P2", PrdAwal.Date)
                            .Parameters.AddWithValue("@P3", PrdAkhir.Date)
                        End With
                        Using RsFind As Data.SqlClient.SqlDataReader = CmdFind.ExecuteReader
                            While RsFind.Read
                                If RsFind("JamMasuk") > TimeSpan.Parse("08:30") Then TtlTelat += 1
                                If RsFind("Status") = "Ijin" Then TtlIjin += 1
                                If RsFind("Status") = "Cuti" Then TtlCuti += 1
                                If RsFind("Status") = "Sakit" Then TtlSakit += 1
                                If RsFind("Status") = "Dinas Proyek" Then TtlDinas += 1
                                If RsFind("Status") = "Ijin 1/2 Hari" Then TtlIjinStgh += 1
                                If RsFind("Status") = "Alpa" Then TtlAlpa += 1
                                If (RsFind("JamMasuk") = TimeSpan.Parse("00:00") Or
                                   RsFind("JamKeluar") = TimeSpan.Parse("00:00")) And RsFind("Status") = "Masuk" Then TtlTdkAbsen += 1
                            End While
                        End Using
                    End Using

                    Using CmdFind As New Data.SqlClient.SqlCommand
                        With CmdFind
                            .Connection = Conn
                            .CommandType = CommandType.Text
                            .CommandText = "SELECT CONVERT(VARCHAR,SUM(DATEDIFF(MINUTE,JamMasuk,JamKeluar))/60) + ':' + " & _
                                           "FORMAT(SUM(DATEDIFF(MINUTE,JamMasuk,JamKeluar))%60,'00') AS 'TtlJamKerja' " & _
                                           "FROM Absensi WHERE NIK=@P1 AND " & _
                                           "TglMasuk>=@P2 AND TglMasuk<=@P3 AND JamMasuk <> '00:00' AND JamKeluar <> '00:00' AND (Status=@P4 OR Status=@P5 OR Status=@P6)"
                            .Parameters.AddWithValue("@P1", RsLoad("NIK"))
                            .Parameters.AddWithValue("@P2", PrdAwal.Date)
                            .Parameters.AddWithValue("@P3", PrdAkhir.Date)
                            .Parameters.AddWithValue("@P4", "Masuk")
                            .Parameters.AddWithValue("@P5", "Dinas Proyek")
                            .Parameters.AddWithValue("@P6", "Ijin 1/2 Hari")
                        End With
                        Using RsFind As Data.SqlClient.SqlDataReader = CmdFind.ExecuteReader
                            If RsFind.Read Then
                                WorkingHour = RsFind("TtlJamKerja").ToString
                            Else
                                WorkingHour = "00:00"
                            End If
                        End Using
                    End Using

                    TmpDt.Rows.Add(RsLoad("NIK"), RsLoad("Nama"), TtlTelat, TtlIjin, TtlCuti, TtlSakit, TtlDinas, TtlIjinStgh, TtlAlpa, TtlTdkAbsen, WorkingHour)
                    TtlTelat = 0
                    TtlIjin = 0
                    TtlCuti = 0
                    TtlSakit = 0
                    TtlDinas = 0
                    TtlIjinStgh = 0
                    TtlAlpa = 0
                    TtlTdkAbsen = 0
                    WorkingHour = "00:00"
                End While
            End Using

            Dim TmpPath As String = Server.MapPath("~/Report/RptRekapStatusAbsensi.rpt")
            Rpt.Load(TmpPath)
            CRViewer.ReportSource = Rpt
            CRViewer.Zoom(100)
            With Rpt
                .SetDataSource(TmpDt)
                .SetParameterValue("@Periode", Format(PrdAwal.Date, "dd-MMM-yyyy") + " s.d. " + Format(PrdAkhir.Date, "dd-MMM-yyyy"))
                .SetParameterValue("@HakCuti", HakCuti)
                .SetParameterValue("@UserEntry", Session("User").ToString.Split("|")(0))
                .SetParameterValue("@PrintInfo", "Printed On " + Format(Now, "dd-MMM-yyyy HH:mm") + " By " + Session("User").ToString.Split("|")(0))
                .ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, False, "")
            End With

            TmpDt.Dispose()

        End Using

    End Sub

End Class