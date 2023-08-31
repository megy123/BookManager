Public Class MainForm
    Dim user As User
    Dim selectedBook As Book
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        selectedBook = Nothing
        user = New User()
        'display data
        If user.last_read = Nothing Then
            Label7.Text = "You have never ever read."
        Else
            Label7.Text = "You read lately on " & Format(user.last_read, "d. mmmm yyyy") & "."
        End If

        For Each b As Book In user.favourite
            ListBox1.Items.Add(b.title)
        Next
        Label2.Text = "Completed: " & user.GetCompleted()
        Label3.Text = "Reading: " & user.GetReading()
        Label4.Text = "Book count: " & user.GetBookCount()
        Label5.Text = "Dropped: " & user.GetDropped()
        Label6.Text = "Read pages: " & user.GetPageCount()

        'Setup tree view
        For Each b As Book In user.books
            Dim path As String = b.path.Substring(user.lib_path.Length + 1)
            Dim node As TreeNode = TreeView1.Nodes.Find("Root", False)(0)

            If path.Contains("\") Then
                Dim i As Integer = path.Count(Function(c As Char) c = "\")
                For c As Integer = 0 To i - 1
                    If node.Nodes.Find(path.Split("\")(c), False).Length = 0 Then
                        node = node.Nodes.Add(path.Split("\")(c), path.Split("\")(c))
                    Else
                        node = node.Nodes.Find(path.Split("\")(c), False)(0)
                    End If
                Next
            End If

            node.Nodes.Add(b.title)
        Next

        'MessageBox.Show(user.lib_path)
        'Dim bok As New Book("C:\Users\domin\Desktop\test.pdf")
        'Dim bok As New Book("C:\Users\domin\Desktop\Ask for More 10 Questions to Negotiate Anything - Alexandra Carter.pdf")
        'Dim bok2 As New Book("C:\Users\domin\Desktop\Neuromancer - William Gibson.pdf")

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'Read button
        If selectedBook IsNot Nothing Then
            MessageBox.Show(user.lib_path & "\" & selectedBook.path & "\" & selectedBook.title & " - " & selectedBook.autor & ".pdf")
            Process.Start(user.lib_path & "\" & selectedBook.path & "\" & selectedBook.title & " - " & selectedBook.autor & ".pdf")
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        'Book info button
        If selectedBook IsNot Nothing Then

        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Settings button
        Settings.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Search button
    End Sub
End Class
