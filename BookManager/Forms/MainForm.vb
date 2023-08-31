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
                        node.ImageIndex = 0 ' folder image

                    Else
                        node = node.Nodes.Find(path.Split("\")(c), False)(0)
                    End If
                Next
            End If

            node = node.Nodes.Add(b.title)
            'add image
            ImageList1.Images.Add(b.image)
            node.ImageIndex = ImageList1.Images.Count - 1
            node.SelectedImageIndex = ImageList1.Images.Count - 1

        Next

        TreeView1.ExpandAll()
        bookSelectionChange(False)
        Label1.Text = "Status: Up to date"
        'MessageBox.Show(user.lib_path)
        'Dim bok As New Book("C:\Users\domin\Desktop\test.pdf")
        'Dim bok As New Book("C:\Users\domin\Desktop\Ask for More 10 Questions to Negotiate Anything - Alexandra Carter.pdf")
        'Dim bok2 As New Book("C:\Users\domin\Desktop\Neuromancer - William Gibson.pdf")

    End Sub

    Private Sub bookSelectionChange(isBook As Boolean)
        If isBook = False Then
            GroupBox2.Enabled = False
            Label9.Text = "Title:"
            Label10.Text = "Author:"
            Label11.Text = "Satus:"

        Else
            GroupBox2.Enabled = True
            Label9.Text = "Title: " & selectedBook.title
            Label10.Text = "Author: " & selectedBook.autor
            Label11.Text = "Satus: " & [Enum].GetName(GetType(status), selectedBook.status)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'Read button
        If selectedBook IsNot Nothing Then
            Process.Start(selectedBook.path)
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        'Book info button
        If selectedBook IsNot Nothing Then
            Dim bookinfo As New BookDetail(selectedBook)
            bookinfo.Show()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Settings button
        Settings.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Search button
    End Sub

    Private Sub TreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect
        'Book selecion frontend changes
        If TreeView1.SelectedNode.Nodes.Count = 0 Then
            selectedBook = user.getBookByTitle(TreeView1.SelectedNode.Text)
            bookSelectionChange(True)
        Else
            selectedBook = Nothing
            bookSelectionChange(False)
        End If
    End Sub
End Class
