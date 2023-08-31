Public Class BookDetail

    Dim book As Book
    Public Sub New(b As Book)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'Set up gui
        ComboBox2.Items.AddRange(statusArr)
        ComboBox1.Items.AddRange(ratingArr)
        If b.status = status.None Then
            ReadBookGuiChange(False)
        End If

        'Insert values
        book = b
        Label4.Text = "Title: " & book.title
        Label5.Text = "Author: " & book.autor
        Label6.Text = "Category: " & book.getCategory()
        Label7.Text = "Started on: " & book.getStartDate()
        Label8.Text = "Completed on: " & book.getFinishDate()
        RichTextBox2.Text = book.description
        RichTextBox1.Text = book.notes
        ProgressBar1.Maximum = book.pages
        ProgressBar1.Value = book.page
        PictureBox1.Image = book.image
        NumericUpDown1.Value = book.page
        Label10.Text = "/" & book.pages
        ComboBox1.SelectedIndex = book.rating
        ComboBox2.SelectedIndex = book.status

    End Sub

    Private Sub ReadBookGuiChange(isRead As Boolean)
        If isRead Then
            Label1.Enabled = True
            Label3.Enabled = True
            NumericUpDown1.Enabled = True
            ComboBox1.Enabled = True
        Else
            Label1.Enabled = False
            Label3.Enabled = False
            NumericUpDown1.Enabled = False
            ComboBox1.Enabled = False
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'add to favourite button
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'open in folder button
        Process.Start(IO.Path.GetDirectoryName(book.path))
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'read button
        Process.Start(book.path)
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        'Status combobox
        If Not ComboBox2.SelectedIndex = status.None Then
            ReadBookGuiChange(True)
        Else
            ReadBookGuiChange(False)
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        'Rating combobox

    End Sub
End Class