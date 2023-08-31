Public Class BookDetail

    Dim book As Book
    Public Sub New(b As Book)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ComboBox2.Items.AddRange([Enum].GetValues(GetType(status)))
        ComboBox1.Items.AddRange(ratingArr)

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

    Private Sub BookDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'add to favourite button
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'open in folder button
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'read button
    End Sub
End Class