Public Class Syncing
    Public Sub New(bookCnt As Integer, newBookCnt As Integer, removedBookCnt As Integer)
        ' This call is required by the designer.
        InitializeComponent()

        'setup gui
        Label2.Text = bookCnt & " books were synced" & vbCrLf &
                      newBookCnt & " new books found " & vbCrLf &
                      removedBookCnt & " books were removed"

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'ok button
        Me.DialogResult = DialogResult.Cancel
        Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'manage button
        Me.DialogResult = DialogResult.OK
        Close()
    End Sub
End Class