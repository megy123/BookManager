Public Class BookDetail

    Dim user As User
    Dim book As Book
    Public Sub New(b As Book, u As User)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        book = b
        user = u


        'Set up gui
        ComboBox2.Items.AddRange(statusArr)
        ComboBox1.Items.AddRange(ratingArr)
        If b.status = status.None Then
            ReadBookGuiChange(False)
        End If
        If user.isFavourite(book) Then
            Button1.Text = "Remove from favourite"
        Else
            Button1.Text = "Add to favourite"
        End If

        'Insert values

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
        NumericUpDown1.Maximum = book.pages
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
        If user.isFavourite(book) Then
            Button1.Text = "Add to favourite"
            user.removeFromFavourite(book)
        Else
            Button1.Text = "Remove from favourite"
            user.addToFavourite(book)
        End If
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
        book.status = ComboBox2.SelectedIndex
        If ComboBox2.SelectedIndex = status.Reading Then
            ReadBookGuiChange(True)
        Else
            ReadBookGuiChange(False)
        End If
        If ComboBox2.SelectedIndex = status.Completed And IsNothing(book.finish_date) Then
            MessageBox.Show("fin")
            book.finish_date = My.Computer.Clock.LocalTime
            Label8.Text = "Completed on: " & book.getFinishDate()
        End If
        If ComboBox2.SelectedIndex = status.Reading And IsNothing(book.begin_date) Then
            MessageBox.Show("beg")
            book.begin_date = My.Computer.Clock.LocalTime
            Label7.Text = "Started on: " & book.getStartDate()
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        'Rating combobox
        book.rating = ComboBox1.SelectedIndex
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        'Page num
        book.page = NumericUpDown1.Value
        ProgressBar1.Value = NumericUpDown1.Value
        If NumericUpDown1.Value = NumericUpDown1.Maximum Then
            ComboBox2.SelectedIndex = status.Completed
        End If
        If Not NumericUpDown1.Value = 0 And ComboBox2.SelectedIndex = status.None Then
            ComboBox2.SelectedIndex = status.Reading
        End If
    End Sub

    Private Sub RichTextBox1_TextChanged(sender As Object, e As EventArgs) Handles RichTextBox1.TextChanged
        book.notes = RichTextBox1.Text
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim randdate? As Date
        randdate = Nothing

        If randdate Is Nothing Then
            MessageBox.Show("Nohing")

        End If
    End Sub
End Class