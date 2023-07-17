Public Class MainForm
    Dim user As User
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        user = New User()
        MessageBox.Show(user.lib_path)
    End Sub
End Class
