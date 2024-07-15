Public Class LoadErrors
    Public Sub New(errors As List(Of String))

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        For Each err As String In errors
            ListBox1.Items.Add(err)
        Next
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.DialogResult = DialogResult.OK
    End Sub
End Class