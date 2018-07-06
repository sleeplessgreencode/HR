Public Class FrmRptRekapAbsensi
    Inherits System.Web.UI.Page
    Dim Conn As New Data.SqlClient.SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("User") = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        Else
            Dim UserId As String = Session("User").ToString.Split("|")(1)
            If CheckAkses1(UserId, "RekapAbsensi") = False Then
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
        Dim TmpDt As New DataTable()
        TmpDt.Columns.AddRange(New DataColumn() {New DataColumn("NIK", GetType(String)), _
                                                 New DataColumn("Nama", GetType(String)), _
                                                 New DataColumn("TtlHariKerja", GetType(Integer)), _
                                                 New DataColumn("TtlJamKerja", GetType(String)), _
                                                 New DataColumn("TtlJamKerja8", GetType(Integer)), _
                                                 New DataColumn("TtlJamKerja9", GetType(Integer)), _
                                                 New DataColumn("TtlAbsen", GetType(Integer))})

        'Dim Bulan As Integer = Session("Print").ToString.Split("|")(0)
        'Dim Tahun As Integer = Session("Print").ToString.Split("|")(1)
        'Dim PrdAwal As Date = DateSerial(Tahun, Bulan, 1)
        'Dim PrdAkhir As Date
        'If Bulan = 12 Then
        '    PrdAkhir = DateSerial(Tahun + 1, 1, 0)
        'Else
        '    PrdAkhir = DateSerial(Tahun, Bulan + 1, 0)
        'End If

        'Dim NmBulan As String() = {"Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "Nopember", "Desember"}
        'Dim BulanIni As String = NmBulan(PrdAwal.Month - 1)

        Dim PrdAwal As Date = Session("Print").ToString.Split("|")(0)
        Dim PrdAkhir As Date = Session("Print").ToString.Split("|")(1)

        Using CmdReport As New Data.SqlClient.SqlCommand
            With CmdReport
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT NIK, Nama FROM Karyawan WHERE Active='1'"
            End With

            Dim TtlJamKerja8 As Integer = 0
            Dim TtlJamKerja9 As Integer = 0
            Dim TtlHariKerja As Integer = JmlHariKerja(PrdAwal.Date, PrdAkhir.Date)
            Dim TtlAbsensi As Integer = 0
            Dim TtlJamKerja As String = "00:00"
            Using RsLoad As Data.SqlClient.SqlDataReader = CmdReport.ExecuteReader
                While RsLoad.Read
                    Using CmdFind As New Data.SqlClient.SqlCommand
                        With CmdFind
                            .Connection = Conn
                            .CommandType = CommandType.Text
                            .CommandText = "SELECT COUNT(NIK) AS 'TtlHariKerja', " & _
                               "CONVERT(VARCHAR,SUM(DATEDIFF(MINUTE,JamMasuk,JamKeluar))/60) + ':' + " & _
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
                                TtlAbsensi = RsFind("TtlHariKerja")
                                TtlJamKerja = RsFind("TtlJamKerja").ToString
                            End If
                        End Using
                    End Using
                    Using CmdFind1 As New Data.SqlClient.SqlCommand
                        With CmdFind1
                            .Connection = Conn
                            .CommandType = CommandType.Text
                            .CommandText = "SELECT COUNT(NIK) AS 'TtlJamKerja8' " & _
                               "FROM Absensi WHERE " & _
                               "NIK=@P1 AND TglMasuk>=@P2 AND TglMasuk<=@P3 AND JamMasuk < '08:00' AND " & _
                               "JamMasuk <> '00:00'"
                            .Parameters.AddWithValue("@P1", RsLoad("NIK"))
                            .Parameters.AddWithValue("@P2", PrdAwal.Date)
                            .Parameters.AddWithValue("@P3", PrdAkhir.Date)
                        End With
                        Using RsFind1 As Data.SqlClient.SqlDataReader = CmdFind1.ExecuteReader
                            If RsFind1.Read Then
                                TtlJamKerja8 = RsFind1("TtlJamKerja8")
                            End If
                        End Using
                    End Using
                    Using CmdFind2 As New Data.SqlClient.SqlCommand
                        With CmdFind2
                            .Connection = Conn
                            .CommandType = CommandType.Text
                            .CommandText = "SELECT COUNT(NIK) AS 'TtlJamKerja9' " & _
                               "FROM Absensi WHERE " & _
                               "NIK=@P1 AND TglMasuk>=@P2 AND TglMasuk<=@P3 AND JamMasuk > '08:30' AND " & _
                               "JamMasuk <> '00:00'"
                            .Parameters.AddWithValue("@P1", RsLoad("NIK"))
                            .Parameters.AddWithValue("@P2", PrdAwal.Date)
                            .Parameters.AddWithValue("@P3", PrdAkhir.Date)
                        End With
                        Using RsFind2 As Data.SqlClient.SqlDataReader = CmdFind2.ExecuteReader
                            If RsFind2.Read Then
                                TtlJamKerja9 = RsFind2("TtlJamKerja9")
                            End If
                        End Using
                    End Using
                    TmpDt.Rows.Add(RsLoad("NIK"), RsLoad("Nama"), TtlAbsensi, TtlJamKerja, TtlJamKerja8, TtlJamKerja9, TtlHariKerja - TtlAbsensi)
                End While
            End Using

            Using Rpt As New RptRekapAbsensi
                With Rpt
                    .SetDataSource(TmpDt)
                    '.SetParameterValue("@Periode", BulanIni + " " + Format(PrdAkhir.Date, "yyyy"))
                    .SetParameterValue("@Periode", Format(PrdAwal.Date, "dd-MMM-yyyy") + " s.d. " + Format(PrdAkhir.Date, "dd-MMM-yyyy"))
                    .SetParameterValue("@TtlHariKerja", TtlHariKerja.ToString + " hari")
                    '.SetParameterValue("@Periode", PrdAwal.Date + " " + PrdAkhir.Date + " " + JmlHariKerja(PrdAwal, PrdAkhir).ToString)
                    .SetParameterValue("@UserEntry", Session("User").ToString.Split("|")(0))
                End With

                CRViewer.ReportSource = Rpt
                Rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, False, "")

            End Using
            TmpDt.Dispose()
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