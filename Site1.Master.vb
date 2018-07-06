Imports DevExpress.Web

Public Class Site1
    Inherits System.Web.UI.MasterPage
    Dim Conn As New System.Data.SqlClient.SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("User") = "" Then Response.Redirect("Default.aspx")

        Conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnStr").ToString
        Conn.Open()
        Call BindMenu()

    End Sub

    Private Sub BindMenu()
        Dim CmdLogin As New System.Data.SqlClient.SqlCommand
        With CmdLogin
            .Connection = Conn
            .CommandType = CommandType.Text
            .CommandText = "SELECT * FROM Login " & _
                            "WHERE UserID='" & Session("User").ToString.Split("|")(1) & "'"
        End With
        Dim RsLogin As System.Data.SqlClient.SqlDataReader = CmdLogin.ExecuteReader
        If RsLogin.Read Then
            lblMaster.Text = "Login sebagai " & UCase(RsLogin("UserName")) & " pada hari ini " & Format(Now, "dd MMMM yyyy")

            On Error Resume Next
            Dim TmpMenuDaftar As Byte = 0
            Dim TmpMenuEntry As Byte = 0
            Dim TmpSubMenuEntry1 As Byte = 0 'SubMenu Entry > Procurement
            Dim TmpSubMenuEntry2 As Byte = 0 'SubMenu Entry > PD/PJ
            Dim TmpMenuRpt As Byte = 0
            Dim TmpSubMenuRpt1 As Byte = 0 'SubMenu Report > Procurement
            Dim TmpSubMenuRpt2 As Byte = 0 'SubMenu Report > PD/PJ
            Dim TmpMenuTools As Byte = 0
            Dim Item As DevExpress.Web.MenuItem

            If RsLogin("AksesMenu").ToString <> "*" Then
                'Menu Daftar

                If RsLogin("DataKaryawan") = "0" Then
                    TmpMenuDaftar = TmpMenuDaftar + 1
                    Item = Menu1.Items.FindByName("FrmKaryawan.aspx")
                    If Item IsNot Nothing Then
                        Item.Visible = False
                    End If
                End If

                If RsLogin("HariLibur") = "0" Then
                    TmpMenuDaftar = TmpMenuDaftar + 1
                    Item = Menu1.Items.FindByName("FrmHariLibur.aspx")
                    If Item IsNot Nothing Then
                        Item.Visible = False
                    End If
                End If

                If TmpMenuDaftar = 2 Then
                    Item = Menu1.Items.FindByName("#Daftar")
                    If Item IsNot Nothing Then
                        Item.Visible = False
                    End If
                End If
                '-------------------

                'Menu Entry               
                If RsLogin("LoadAbsensi") = "0" Then
                    TmpMenuEntry = TmpMenuEntry + 1
                    Item = Menu1.Items.FindByName("FrmLoadAbsensi.aspx")
                    If Item IsNot Nothing Then
                        Item.Visible = False
                    End If
                End If

                If RsLogin("EntryAbsensi") = "0" Then
                    TmpMenuEntry = TmpMenuEntry + 1
                    Item = Menu1.Items.FindByName("FrmAbsensi.aspx")
                    If Item IsNot Nothing Then
                        Item.Visible = False
                    End If
                End If

                If TmpMenuEntry = 2 Then
                    Item = Menu1.Items.FindByName("#Entry")
                    If Item IsNot Nothing Then
                        Item.Visible = False
                    End If
                End If
                '-------------------

                'Menu Report
                If RsLogin("RekapAbsensi") = "0" Then
                    TmpMenuRpt = TmpMenuRpt + 1
                    Item = Menu1.Items.FindByName("FrmRekapAbsensi.aspx")
                    If Item IsNot Nothing Then
                        Item.Visible = False
                    End If
                End If

                If RsLogin("AbsensiKaryawan") = "0" Then
                    TmpMenuRpt = TmpMenuRpt + 1
                    Item = Menu1.Items.FindByName("FrmAbsensiKaryawan.aspx")
                    If Item IsNot Nothing Then
                        Item.Visible = False
                    End If
                End If

                If RsLogin("RekapStatusAbsensi") = "0" Then
                    TmpMenuRpt = TmpMenuRpt + 1
                    Item = Menu1.Items.FindByName("FrmRekapStatusAbsensi.aspx")
                    If Item IsNot Nothing Then
                        Item.Visible = False
                    End If
                End If

                If RsLogin("RekapStatusAbsensi") = "0" Then
                    TmpMenuRpt = TmpMenuRpt + 1
                    Item = Menu1.Items.FindByName("FrmRekapStatusAbsensi1.aspx")
                    If Item IsNot Nothing Then
                        Item.Visible = False
                    End If
                End If

                If TmpMenuRpt = 4 Then
                    Item = Menu1.Items.FindByName("#Report")
                    If Item IsNot Nothing Then
                        Item.Visible = False
                    End If
                End If
                '-------------------


                'Menu Tools
                If RsLogin("SetupAkses") = "0" Then
                    TmpMenuTools = TmpMenuTools + 1
                    Item = Menu1.Items.FindByName("FrmSetupAkses.aspx")
                    If Item IsNot Nothing Then
                        Item.Visible = False
                    End If
                End If

                If RsLogin("ChangePasswd") = "0" Then
                    TmpMenuTools = TmpMenuTools + 1
                    Item = Menu1.Items.FindByName("FrmChangePasswd.aspx")
                    If Item IsNot Nothing Then
                        Item.Visible = False
                    End If
                End If

                If TmpMenuTools = 2 Then
                    Item = Menu1.Items.FindByName("#Tools")
                    If Item IsNot Nothing Then
                        Item.Visible = False
                    End If
                End If
                '-------------------

            End If
        End If
        RsLogin.Close()
        CmdLogin.Dispose()
    End Sub

    Private Sub Page_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        Conn.Close()
        Conn.Dispose()
    End Sub

    'Private Sub Menu1_ItemClick(source As Object, e As DevExpress.Web.MenuItemEventArgs) Handles Menu1.ItemClick
    '    'Not Parent/Sub Parent Menu

    '    If Left(e.Item.Name, 1) <> "#" Then
    '        Dim User = Session("User")
    '        Session.RemoveAll()
    '        Session("User") = User

    '        Response.Redirect(e.Item.Name)
    '    End If

    'End Sub

    Private Sub Menu1_Load(sender As Object, e As System.EventArgs) Handles Menu1.Load
        Dim menu As ASPxMenu = TryCast(sender, ASPxMenu)
        CorrectItem(menu.RootItem)

    End Sub

    Private Sub CorrectItem(ByVal item As MenuItem)
        If item Is Nothing Then
            Return
        End If
        If item.HasChildren Then
            item.NavigateUrl = Nothing
            For Each subItem As MenuItem In item.Items
                CorrectItem(subItem)
            Next subItem
        End If
    End Sub

End Class