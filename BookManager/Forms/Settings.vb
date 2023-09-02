Public Class Settings
    Dim user As User
    Public Sub New(user As User)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.user = user
        Label1.Text = "Library path: " & user.lib_path
        CheckBox1.Checked = user.startup_sync
        CheckBox2.Checked = user.confirm_sync
        If user.last_sync Is Nothing Then
            Label3.Text = "Last sync. :-"
        Else
            Label3.Text = "Last sync. : " & Format(user.last_sync, "d. MMMM yyyy")
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'change library path button
        Dim fd As New FolderBrowserDialog()
        If fd.ShowDialog() = DialogResult.OK Then
            user.lib_path = fd.SelectedPath
            'TODO resetuj usera
        End If
        Label1.Text = "Library path: " & user.lib_path
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Export button


    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'Import button

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        'Synchronize on startup checkbox
        user.startup_sync = CheckBox1.Checked
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        'Confirm sync checkbox
        user.confirm_sync = CheckBox2.Checked
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        'Database button

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        'Sync button
    End Sub
End Class