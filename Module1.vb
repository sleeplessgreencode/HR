Option Explicit On

Module Module1
    Public Function Terbilang(ByVal nilai As Long) As String
        Dim bilangan As String() = {" ", "Satu", "Dua", "Tiga", "Empat", "Lima", _
                                    "Enam", "Tujuh", "Delapan", "Sembilan", "Sepuluh", "Sebelas"}

        If nilai < 12 Then
            Return " " & bilangan(nilai)
        ElseIf nilai < 20 Then
            Return Terbilang(nilai - 10) & " Belas"
        ElseIf nilai < 100 Then
            Return (Terbilang(CInt((nilai \ 10))) & " Puluh") + Terbilang(nilai Mod 10)
        ElseIf nilai < 200 Then
            Return " Seratus" & Terbilang(nilai - 100)
        ElseIf nilai < 1000 Then
            Return (Terbilang(CInt((nilai \ 100))) & " Ratus") + Terbilang(nilai Mod 100)
        ElseIf nilai < 2000 Then
            Return " Seribu" & Terbilang(nilai - 1000)
        ElseIf nilai < 1000000 Then
            Return (Terbilang(CInt((nilai \ 1000))) & " Ribu") + Terbilang(nilai Mod 1000)
        ElseIf nilai < 1000000000 Then
            Return (Terbilang(CInt((nilai \ 1000000))) & " Juta") + Terbilang(nilai Mod 1000000)
        ElseIf nilai < 1000000000000 Then
            Return (Terbilang(CInt((nilai \ 1000000000))) & " Milyar") + Terbilang(nilai Mod 1000000000)
        ElseIf nilai < 1000000000000000 Then
            Return (Terbilang(CInt((nilai \ 1000000000000))) & " Trilyun") + Terbilang(nilai Mod 1000000000000)
        Else
            Return ""
        End If

    End Function

    Public Function CharTanggal(Tgl As Date) As String
        Dim CharMo As String = ""
        Dim CharDay As String = ""

        If Weekday(Tgl) = 1 Then CharDay = "Minggu"
        If Weekday(Tgl) = 2 Then CharDay = "Senin"
        If Weekday(Tgl) = 3 Then CharDay = "Selasa"
        If Weekday(Tgl) = 4 Then CharDay = "Rabu"
        If Weekday(Tgl) = 5 Then CharDay = "Kamis"
        If Weekday(Tgl) = 6 Then CharDay = "Jumat"
        If Weekday(Tgl) = 7 Then CharDay = "Sabtu"

        If Month(Tgl) = 1 Then CharMo = "Januari"
        If Month(Tgl) = 2 Then CharMo = "Februari"
        If Month(Tgl) = 3 Then CharMo = "Maret"
        If Month(Tgl) = 4 Then CharMo = "April"
        If Month(Tgl) = 5 Then CharMo = "Mei"
        If Month(Tgl) = 6 Then CharMo = "Juni"
        If Month(Tgl) = 7 Then CharMo = "Juli"
        If Month(Tgl) = 8 Then CharMo = "Agustus"
        If Month(Tgl) = 9 Then CharMo = "September"
        If Month(Tgl) = 10 Then CharMo = "Oktober"
        If Month(Tgl) = 11 Then CharMo = "November"
        If Month(Tgl) = 12 Then CharMo = "Desember"

        CharTanggal = CharDay & " Tanggal " & Terbilang(Day(Tgl)) & " Bulan " & CharMo & " Tahun " & Terbilang(Year(Tgl))

    End Function

    Public Function CheckAkses1(ByVal UserID As String, ByVal Menu As String) As Boolean
        If UserID = "" Then Return False

        Using Conn As New Data.SqlClient.SqlConnection
            Conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnStr").ToString
            Conn.Open()

            Using CmdFind As New Data.SqlClient.SqlCommand
                With CmdFind
                    .Connection = Conn
                    .CommandType = CommandType.Text
                    .CommandText = "SELECT " + Menu + " FROM Login WHERE UserID=@P1"
                    .Parameters.AddWithValue("@P1", UserID)
                End With
                Using RsFind As Data.SqlClient.SqlDataReader = CmdFind.ExecuteReader
                    If RsFind.Read Then
                        If RsFind(0) = "0" Then Return False
                    End If
                End Using

            End Using
            Conn.Close()
        End Using

        Return True

    End Function
End Module