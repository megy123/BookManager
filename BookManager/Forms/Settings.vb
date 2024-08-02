Public Class Settings
    Dim user As User
    Dim setup As Boolean

#Region "Methods"
    Public Sub guiInit()
        setup = False

        Label1.Text = "Library path: " & user.lib_path
        CheckBox1.Checked = user.startup_sync
        CheckBox2.Checked = user.confirm_sync
        CheckBox3.Checked = user.encrytion
        If user.last_sync Is Nothing Then
            Label3.Text = "Last sync. :-"
        Else
            Label3.Text = "Last sync. : " & Format(user.last_sync, "d. MMMM yyyy")
        End If
        If user.encrytion Then
            Button6.Enabled = True
        Else
            Button6.Enabled = False
        End If

        setup = True
    End Sub
    Private Function getPassword() As String
        Dim passwordDialog As New PasswordPromtForm
        If passwordDialog.ShowDialog() = DialogResult.OK Then
            Return passwordDialog.TextBox1.Text
        Else
            Return Nothing
        End If
    End Function
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

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        If setup Then
            'Encryption checkbox
            user.encrytion = CheckBox3.Checked
            Dim pass As String = Nothing

            If user.encrytion Then
                While pass Is Nothing
                    pass = getPassword()
                End While
            End If

            user.setPassword(pass)
            user.Save()
            guiInit()
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        'Password set button
        Dim pass As String = Nothing
        While pass Is Nothing
            pass = getPassword()
        End While

        user.setPassword(pass)
        user.Save()
        guiInit()
    End Sub
#End Region
End Class