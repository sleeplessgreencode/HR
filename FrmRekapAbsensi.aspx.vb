Public Class FrmRekapAbsensi
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

        If IsPostBack = False Then
            'Call BindPeriode()
            'DDLBulan.Value = Today.Month.ToString
            'TxtTahun.Value = Today.Year
            'PrdAwal.Date = DateSerial(Year(Today), Month(Today), 1)
            'PrdAkhir.Date = DateAdd(DateInterval.Month, 1, DateSerial(Year(Today), Month(Today), 0))
            If Month(Today) = 1 Then
                PrdAwal.Date = DateSerial(Year(Today) - 1, 12, 1)
            Else
                PrdAwal.Date = DateSerial(Year(Today), Month(Today) - 1, 1)
            End If

            PrdAkhir.Date = DateSerial(Year(Today), Month(Today), 1).AddDays(-1)
        End If

    End Sub

    Private Sub Page_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        Conn.Close()
        Conn.Dispose()
    End Sub

    'Private Sub BindPeriode()
    '    DDLBulan.Items.Clear()
    '    DDLBulan.Items.Add("Januari", "1")
    '    DDLBulan.Items.Add("Februari", "2")
    '    DDLBulan.Items.Add("Maret", "3")
    '    DDLBulan.Items.Add("April", "4")
    '    DDLBulan.Items.Add("Mei", "5")
    '    DDLBulan.Items.Add("Juni", "6")
    '    DDLBulan.Items.Add("Juli", "7")
    '    DDLBulan.Items.Add("Agustus", "8")
    '    DDLBulan.Items.Add("September", "9")
    '    DDLBulan.Items.Add("Oktober", "10")
    '    DDLBulan.Items.Add("November", "11")
    '    DDLBulan.Items.Add("Desember", "12")
    'End Sub

    Private Sub BtnPrint_Click(sender As Object, e As System.EventArgs) Handles BtnPrint.Click
        'If TxtTahun.Text = "0" Then
        '    LblErr.Text = "Tahun belum diisi."
        '    ErrMsg.ShowOnPageLoad = True
        '    Exit Sub
        'End If

        If PrdAkhir.Date < PrdAwal.Date Then
            LblErr.Text = "Periode invalid"
            ErrMsg.ShowOnPageLoad = True
            Exit Sub

        End If
        'Session("Print") = DDLBulan.Value & "|" & TxtTahun.Value
        Session("Print") = PrdAwal.Date & "|" & PrdAkhir.Date
        Response.Redirect("FrmRptRekapAbsensi.aspx")
    End Sub

End Class