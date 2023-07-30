Public Class MainForm
    Dim user As User
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        user = New User()
        MessageBox.Show(user.lib_path)
        'Dim bok As New Book("C:\Users\domin\Desktop\test.pdf")
        Dim bok As New Book("D:\dominik\Dokumenty\knihy\Psychológia\Deep Work - Cal Newport.pdf")
    End Sub
End Class
