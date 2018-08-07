Public Class FrmSkorKehadiran
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

        Session.Remove("PrdAwal")
        Session.Remove("PrdAkhir")
    End Sub
    Protected Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        TxtNilaiMasuk.Text = "10"
        TxtNilaiTelat.Text = "-10"
        TxtNilaiIjin.Text = "-10"
        TxtNilaiCuti.Text = "0"
        TxtNilaiSakit.Text = "-10"
        TxtNilaiDinasProyek.Text = "10"
        TxtNilaiIjinSetengahHari.Text = "-5"
        TxtNilaiAlpa.Text = "-10"
        TxtNilaiTidakAbsen.Text = "-10"
        TxtNilaiMedicalRS.Text = "0"
        TxtNilaiPulangCepat.Text = "-10"
        TxtBatasTelat.Text = "08:30"
        TxtBatasPulangAwal.Text = "17:00"
        PopEntScoring.ShowOnPageLoad = True
        'Session("PrdAwal") = PrdAwal.Date
        'Session("PrdAkhir") = PrdAkhir.Date
        'Dim Url As String = "FrmRptSkorKehadiran.aspx"

        'With DialogWindow1
        '    .TargetUrl = Url
        '    .Open()
        'End With
    End Sub
    Protected Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        Call GenerateReport()
    End Sub
    Private Sub GenerateReport()
        Dim TmpDt As New DataTable()
        TmpDt.Columns.AddRange(New DataColumn() {New DataColumn("NIK", GetType(String)), _
                                                 New DataColumn("Nama", GetType(String)), _
                                                 New DataColumn("TotalHariKerja", GetType(Integer)), _
                                                 New DataColumn("TotalMasuk", GetType(Integer)), _
                                                 New DataColumn("TotalSakit", GetType(Integer)), _
                                                 New DataColumn("TotalDinas", GetType(Integer)), _
                                                 New DataColumn("TotalIjinSetengah", GetType(Integer)), _
                                                 New DataColumn("TotalCuti", GetType(Integer)), _
                                                 New DataColumn("TotalAlpa", GetType(Integer)), _
                                                 New DataColumn("TotalIjin", GetType(Integer)), _
                                                 New DataColumn("TotalMedicalRS", GetType(Integer)), _
                                                 New DataColumn("TotalTelat", GetType(Integer)), _
                                                 New DataColumn("TotalPulangCepat", GetType(Integer)), _
                                                 New DataColumn("TotalTdkAbsen", GetType(Integer)), _
                                                 New DataColumn("TotalScore", GetType(Decimal))})

        Dim TotalHariKerja = 0, TotalMasuk = 0, TotalSakit = 0, TotalDinas = 0, TotalIjinSetengah = 0, TotalCuti = 0, TotalAlpa = 0, _
            TotalIjin = 0, TotalMedicalRS = 0, TotalTelat = 0, TotalPulangCepat = 0, TotalTdkAbsen = 0, TotalScore = 0

        Using CmdKaryawan As New Data.SqlClient.SqlCommand
            With CmdKaryawan
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT NIK, Nama FROM KARYAWAN WHERE Active='1'"
            End With
            Using RsReadKaryawan As Data.SqlClient.SqlDataReader = CmdKaryawan.ExecuteReader
                While RsReadKaryawan.Read
                    Using CmdAbsensi As New Data.SqlClient.SqlCommand
                        With CmdAbsensi
                            .Connection = Conn
                            .CommandType = CommandType.Text
                            .CommandText = "SELECT * FROM Absensi WHERE NIK=@P1 AND TglMasuk>=@P2 AND TglMasuk<=@P3"
                            .Parameters.AddWithValue("@P1", RsReadKaryawan("NIK"))
                            .Parameters.AddWithValue("@P2", PrdAwal.Date)
                            .Parameters.AddWithValue("@P3", PrdAkhir.Date)
                        End With
                        Using RsReadAbsensi As Data.SqlClient.SqlDataReader = CmdAbsensi.ExecuteReader
                            While RsReadAbsensi.Read
                                TotalHariKerja += 1
                                If RsReadAbsensi("JamMasuk") > TimeSpan.Parse(TxtBatasTelat.Text) And _
                                    (RsReadAbsensi("Status") <> "Dinas Proyek" And RsReadAbsensi("Status") <> "Ijin 1/2 Hari") Then
                                    TotalTelat += 1
                                End If
                                TotalTelat += 1
                                If RsReadAbsensi("JamKeluar") < TimeSpan.Parse(TxtBatasPulangAwal.Text) And _
                                    (RsReadAbsensi("Status") <> "Dinas Proyek" And RsReadAbsensi("Status") <> "Ijin 1/2 Hari") Then
                                    TotalPulangCepat += 1
                                End If
                                If (RsReadAbsensi("JamMasuk") = TimeSpan.Parse("00:00") Or RsReadAbsensi("JamKeluar") = TimeSpan.Parse("00:00")) _
                                    And CheckHariLibur(RsReadAbsensi("TglMasuk")) = False Then
                                    TotalTdkAbsen += 1
                                End If
                                Select Case RsReadAbsensi("Status")
                                    Case "Masuk"
                                        TotalMasuk += 1
                                    Case "Sakit"
                                        TotalSakit += 1
                                    Case "Dinas Proyek"
                                        TotalDinas += 1
                                    Case "Ijin 1/2 Hari"
                                        TotalIjinSetengah += 1
                                    Case "Cuti"
                                        TotalCuti += 1
                                    Case "Alpa"
                                        TotalAlpa += 1
                                    Case "Ijin"
                                        TotalIjin += 1
                                    Case "Medical RS"
                                        TotalMedicalRS += 1
                                End Select
                            End While
                            TotalScore = (TotalHariKerja * TxtNilaiMasuk.Text) + (TotalSakit * TxtNilaiSakit.Text) + _
                                         (TotalDinas * TxtNilaiDinasProyek.Text) + (TotalIjinSetengah * TxtNilaiIjinSetengahHari.Text) + _
                                         (TotalCuti * TxtNilaiCuti.Text) + (TotalAlpa * TxtNilaiAlpa.Text) + (TotalIjin * TxtNilaiIjin.Text) + _
                                         (TotalMedicalRS * TxtNilaiMedicalRS.Text) + (TotalTelat * TxtNilaiTelat.Text) + _
                                         (TotalPulangCepat * TxtNilaiPulangCepat.Text)
                            TmpDt.Rows.Add(RsReadKaryawan("NIK"), RsReadKaryawan("Nama"), TotalMasuk, TotalSakit, TotalDinas, TotalIjinSetengah, _
                                           TotalCuti, TotalAlpa, TotalIjin, TotalMedicalRS, TotalTelat, TotalPulangCepat, TotalScore)
                            TotalHariKerja = 0
                            TotalMasuk = 0
                            TotalSakit = 0
                            TotalDinas = 0
                            TotalIjinSetengah = 0
                            TotalCuti = 0
                            TotalAlpa = 0
                            TotalIjin = 0
                            TotalMedicalRS = 0
                            TotalTelat = 0
                            TotalPulangCepat = 0
                            TotalTdkAbsen = 0
                            TotalScore = 0
                        End Using
                    End Using
                End While
            End Using
        End Using
        ' Sort DataTable berdasar skor
        TmpDt.DefaultView.Sort = "TotalScore DESC, TotalAlpa DESC, TotalIjin DESC"
        TmpDt = TmpDt.DefaultView.ToTable

        Dim TmpPath As String = Server.MapPath("~/Report/RptSkorKehadiran.rpt")
        Rpt.Load(TmpPath)
        With Rpt
            .SetDataSource(TmpDt)
            .SetParameterValue("@Periode", Format(PrdAwal.Date, "dd-MMM-yyyy") + " s.d. " + Format(PrdAkhir.Date, "dd-MMM-yyyy"))
            .SetParameterValue("@UserEntry", Session("User").ToString.Split("|")(0))
            .SetParameterValue("@PrintInfo", "Printed On " + Format(Now, "dd-MMM-yyyy HH:mm") + " By " + Session("User").ToString.Split("|")(0))
        End With
        Session("RptSkorKehadiran") = Rpt

        'With CRViewer
        '    .ReportSource = Rpt
        '    .Zoom(100)
        'End With

        TmpDt.Dispose()
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