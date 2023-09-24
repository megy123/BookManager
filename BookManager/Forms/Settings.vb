Public Class Settings
    Dim user As User

#Region "Methods"
    Public Sub guiInit()
        Label1.Text = "Library path: " & user.lib_path
        CheckBox1.Checked = user.startup_sync
        CheckBox2.Checked = user.confirm_sync
        If user.last_sync Is Nothing Then
            Label3.Text = "Last sync. :-"
        Else
            Label3.Text = "Last sync. : " & Format(user.last_sync, "d. MMMM yyyy")
        End If
    End Sub
#End Region

#Region "Controls"
    Public Sub New(user As User)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.user = user
        guiInit()

    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'change library path button
        Dim fd As New FolderBrowserDialog()
        fd.Description = "Change library path."
        If fd.ShowDialog() = DialogResult.OK Then
            user.lib_path = fd.SelectedPath
            Application.Restart() 'restart
        End If
        'Label1.Text = "Library path: " & user.lib_path
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Export button
        Dim fd As New FolderBrowserDialog()
        fd.Description = "Export data."
        If fd.ShowDialog() = DialogResult.OK Then
            IO.File.Copy("DataContainer.xml", fd.SelectedPath & "\BookManager_backup_" & Format(My.Computer.Clock.LocalTime, "yyyy_M_d") & ".xml")
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'Import button
        Dim fd As New OpenFileDialog()
        fd.Title = "Import data."
        fd.Filter = "Book manager backup|BookManager_backup_*.xml"
        fd.InitialDirectory = user.lib_path
        If fd.ShowDialog() = DialogResult.OK Then
            IO.File.Copy(fd.FileName, "DataContainer.xml")
            Application.Restart() 'restart
        End If
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
#End Region
End Class