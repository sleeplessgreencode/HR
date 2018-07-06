Public Class FrmEntrySetup
    Inherits System.Web.UI.Page
    Dim Conn As New Data.SqlClient.SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("User") = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        Else
            If CheckAkses1(Session("User").ToString.Split("|")(1), "SetupAkses") = False Then
                Response.Redirect("Default.aspx")
                Exit Sub
            End If
        End If

        Conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnStr").ToString
        Conn.Open()

        If IsPostBack = False Then
            Call BindGrid()
        End If

    End Sub

    Private Sub BindGrid()
        If Session("Setup") <> "NEW" Then
            Using CmdFind As New Data.SqlClient.SqlCommand
                With CmdFind
                    .Connection = Conn
                    .CommandType = CommandType.Text
                    .CommandText = "SELECT * FROM Login WHERE UserID=@P1"
                    .Parameters.AddWithValue("@P1", Session("Setup"))
                End With
                Using RsFind As Data.SqlClient.SqlDataReader = CmdFind.ExecuteReader
                    If RsFind.Read Then
                        TxtUserID.Text = RsFind("UserID")
                        TxtUserName.Text = RsFind("UserName")

                        'Menu Daftar
                        CbDataKaryawan.Checked = If(RsFind("DataKaryawan") = "0", False, True)
                        CbHariLibur.Checked = If(RsFind("HariLibur") = "0", False, True)

                        'Menu Entry
                        CbLoadAbsensi.Checked = If(RsFind("LoadAbsensi") = "0", False, True)
                        CbEntryAbsensi.Checked = If(RsFind("EntryAbsensi") = "0", False, True)

                        'Menu Report
                        CbRekapAbsensi.Checked = If(RsFind("RekapAbsensi") = "0", False, True)
                        CbAbsensiKaryawan.Checked = If(RsFind("AbsensiKaryawan") = "0", False, True)
                        CbRekapStatusAbsensi.Checked = If(RsFind("RekapStatusAbsensi") = "0", False, True)

                        'Menu Tools
                        CbSetup.Checked = If(RsFind("SetupAkses") = "0", False, True)
                        CbPasswd.Checked = If(RsFind("ChangePasswd") = "0", False, True)

                    End If
                End Using
            End Using

            TxtUserID.Enabled = False
            TxtAction.Text = "UPD"
        End If

    End Sub

    Protected Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        Session.Remove("Setup")
        Response.Redirect("FrmSetupAkses.aspx")
        Exit Sub
    End Sub

    Private Sub Page_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        Conn.Close()
        Conn.Dispose()
    End Sub

    Protected Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If TxtAction.Text = "NEW" Then
            If String.IsNullOrEmpty(TxtPassword.Text) = True Then
                msgBox1.alert("Password belum diisi.")
                TxtPassword.Focus()
                Exit Sub
            End If
        End If
        If TxtPassword.Text <> TxtConfirmPw.Text Then
            msgBox1.alert("Password dan Confirm Password tidak sama.")
            TxtPassword.Focus()
            Exit Sub
        End If
        If TxtAction.Text = "NEW" Then
            If CheckUserId() = False Then Exit Sub

            Dim CmdInsert As New Data.SqlClient.SqlCommand
            With CmdInsert
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "INSERT INTO Login (UserID,UserName,Password,DataKaryawan,HariLibur,LoadAbsensi,EntryAbsensi," + _
                               "RekapAbsensi,AbsensiKaryawan,UserEntry,TimeEntry,RekapStatusAbsensi,ChangePasswd) " + _
                               "VALUES (@P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8,@P9,@P10,@P11,@P12,@P13)"
                .Parameters.AddWithValue("@P1", TxtUserID.Text)
                .Parameters.AddWithValue("@P2", TxtUserName.Text)
                .Parameters.AddWithValue("@P3", Encrypt(TxtPassword.Text))
                .Parameters.AddWithValue("@P4", If(CbDataKaryawan.Checked = True, "1", "0"))
                .Parameters.AddWithValue("@P5", If(CbHariLibur.Checked = True, "1", "0"))
                .Parameters.AddWithValue("@P6", If(CbLoadAbsensi.Checked = True, "1", "0"))
                .Parameters.AddWithValue("@P7", If(CbEntryAbsensi.Checked = True, "1", "0"))
                .Parameters.AddWithValue("@P8", If(CbRekapAbsensi.Checked = True, "1", "0"))
                .Parameters.AddWithValue("@P9", If(CbAbsensiKaryawan.Checked = True, "1", "0"))
                .Parameters.AddWithValue("@P10", Session("User").ToString.Split("|")(0))
                .Parameters.AddWithValue("@P11", Now)
                .Parameters.AddWithValue("@P12", If(CbRekapStatusAbsensi.Checked = True, "1", "0"))
                .Parameters.AddWithValue("@P13", If(CbPasswd.Checked = True, "1", "0"))
                Try
                    .ExecuteNonQuery()
                Catch
                    msgBox1.alert(Err.Description)
                    Exit Sub
                End Try
            End With
        Else
            Dim CmdEdit As New Data.SqlClient.SqlCommand
            With CmdEdit
                .Connection = Conn
                .CommandType = CommandType.Text
                If String.IsNullOrEmpty(TxtPassword.Text) = False Then
                    .CommandText = "UPDATE Login SET Password=@P1,DataKaryawan=@P2,HariLibur=@P3,LoadAbsensi=@P4," + _
                                   "EntryAbsensi=@P5,RekapAbsensi=@P6,AbsensiKaryawan=@P7,UserEntry=@P8,TimeEntry=@P9, " + _
                                   "RekapStatusAbsensi=@P10,ChangePasswd=@P11 WHERE UserID=@P12"
                Else
                    .CommandText = "UPDATE Login SET DataKaryawan=@P2,HariLibur=@P3,LoadAbsensi=@P4," + _
                                   "EntryAbsensi=@P5,RekapAbsensi=@P6,AbsensiKaryawan=@P7,UserEntry=@P8,TimeEntry=@P9, " + _
                                   "RekapStatusAbsensi=@P10,ChangePasswd=@P11 WHERE UserID=@P12"
                End If
                .Parameters.AddWithValue("@P1", Encrypt(TxtPassword.Text))
                .Parameters.AddWithValue("@P2", If(CbDataKaryawan.Checked = True, "1", "0"))
                .Parameters.AddWithValue("@P3", If(CbHariLibur.Checked = True, "1", "0"))
                .Parameters.AddWithValue("@P4", If(CbLoadAbsensi.Checked = True, "1", "0"))
                .Parameters.AddWithValue("@P5", If(CbEntryAbsensi.Checked = True, "1", "0"))
                .Parameters.AddWithValue("@P6", If(CbRekapAbsensi.Checked = True, "1", "0"))
                .Parameters.AddWithValue("@P7", If(CbAbsensiKaryawan.Checked = True, "1", "0"))
                .Parameters.AddWithValue("@P8", Session("User").ToString.Split("|")(0))
                .Parameters.AddWithValue("@P9", Now)
                .Parameters.AddWithValue("@P10", If(CbRekapStatusAbsensi.Checked = True, "1", "0"))
                .Parameters.AddWithValue("@P11", If(CbPasswd.Checked = True, "1", "0"))
                .Parameters.AddWithValue("@P12", Session("Setup"))
                Try
                    .ExecuteNonQuery()
                Catch
                    msgBox1.alert(Err.Description)
                    Exit Sub
                End Try
                .Dispose()
            End With
        End If

        BtnCancel_Click(BtnCancel, New EventArgs())

    End Sub

    Private Function Encrypt(clearText As String) As String

        Dim EncryptionKey As String = "sKlpxvB8hR43zYt3CQRwO1K94Q39k5m7"
        Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
        Using encryptor As System.Security.Cryptography.Aes = System.Security.Cryptography.Aes.Create()
            Dim pdb As New System.Security.Cryptography.Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, _
             &H65, &H64, &H76, &H65, &H64, &H65, _
             &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New System.IO.MemoryStream()
                Using cs As New System.Security.Cryptography.CryptoStream(ms, encryptor.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write)
                    cs.Write(clearBytes, 0, clearBytes.Length)
                    cs.Close()
                End Using
                clearText = Convert.ToBase64String(ms.ToArray())
            End Using
        End Using
        Return clearText

    End Function

    Private Function Decrypt(cipherText As String) As String

        Dim EncryptionKey As String = "sKlpxvB8hR43zYt3CQRwO1K94Q39k5m7"
        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
        Using encryptor As System.Security.Cryptography.Aes = System.Security.Cryptography.Aes.Create()
            Dim pdb As New System.Security.Cryptography.Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, _
             &H65, &H64, &H76, &H65, &H64, &H65, _
             &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New System.IO.MemoryStream()
                Using cs As New System.Security.Cryptography.CryptoStream(ms, encryptor.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write)
                    cs.Write(cipherBytes, 0, cipherBytes.Length)
                    cs.Close()
                End Using
                cipherText = Encoding.Unicode.GetString(ms.ToArray())
            End Using
        End Using
        Return cipherText

    End Function

    Private Function CheckUserId() As Boolean

        Using CmdFind As New Data.SqlClient.SqlCommand
            With CmdFind
                .Connection = Conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT * FROM Login WHERE UserID=@P1"
                .Parameters.AddWithValue("@P1", Trim(TxtUserID.Text))
            End With
            Using RsFind As Data.SqlClient.SqlDataReader = CmdFind.ExecuteReader
                If RsFind.Read Then
                    msgBox1.alert("User ID " & Trim(TxtUserID.Text) & " sudah ada.")
                    TxtUserID.Focus()
                    Return False
                End If
            End Using
        End Using

        Return True

    End Function

    Private Sub BtnAllMenu_Click(sender As Object, e As System.EventArgs) Handles BtnAllMenu.Click
        'Daftar
        CbDataKaryawan.Checked = True
        CbHariLibur.Checked = True
        'Entry
        CbLoadAbsensi.Checked = True
        CbEntryAbsensi.Checked = True

        'Report
        CbRekapAbsensi.Checked = True
        CbAbsensiKaryawan.Checked = True
        CbRekapStatusAbsensi.Checked = True

        'Tools
        CbSetup.Checked = True
        CbPasswd.Checked = True
    End Sub

End Class