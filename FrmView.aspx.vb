Public Class FrmView
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("ViewJPG") <> "" Then
            Image1.ImageUrl = Session("ViewJPG")
        Else
            Image1.Visible = False
        End If
    End Sub
End Class