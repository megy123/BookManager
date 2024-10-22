﻿Public Class MainForm
    Dim user As User
    Dim selectedBook As Book
    Dim WithEvents searchBox As ListBox

#Region "Methods"
    Private Sub favouriteChanged()
        ListBox1.Items.Clear()
        If user.favourite IsNot Nothing Then
            For Each b As Book In user.favourite
                ListBox1.Items.Add(b.title)
            Next
        End If
    End Sub

    Private Sub lastReadUpdated()
        Label7.Text = "You read lately on " & Format(user.last_read, "d. MMMM yyyy") & "."
    End Sub

    Private Sub bookSelectionChange(isBook As Boolean)
        If isBook = True And selectedBook IsNot Nothing Then
            GroupBox2.Enabled = True
            Label9.Text = "Title: " & selectedBook.title
            Label10.Text = "Author: " & selectedBook.autor
            Label11.Text = "Satus: " & [Enum].GetName(GetType(status), selectedBook.status)
        ElseIf isBook = False Then
            GroupBox2.Enabled = False
                Label9.Text = "Title:"
                Label10.Text = "Author:"
            Label11.Text = "Satus:"
        End If
    End Sub

    Public Sub guiInit()
        'display data
        If user.last_read Is Nothing Then
            Label7.Text = "You have never ever read."
        Else
            Label7.Text = "You read lately on " & Format(user.last_read, "d. MMMM yyyy") & "."
        End If

        'For Each b As Book In user.favourite
        '    ListBox1.Items.Add(b.title)
        'Next
        Label2.Text = "Completed: " & user.GetCompleted()
        Label3.Text = "Reading: " & user.GetReading()
        Label4.Text = "Book count: " & user.GetBookCount()
        Label5.Text = "Dropped: " & user.GetDropped()
        Label6.Text = "Read pages: " & user.GetPageCount()

        'Setup tree view
        Invoke(Sub()
                   TreeView1.Nodes.Clear()
                   TreeView1.Nodes.Add("Root", "Bookshelf", 0)
               End Sub)

        For Each b As Book In user.books
            Dim path As String = b.path.Substring(user.lib_path.Length + 1)
            Dim node As TreeNode = TreeView1.Nodes.Find("Root", False)(0)

            If path.Contains("\") Then
                Dim i As Integer = path.Count(Function(c As Char) c = "\")
                For c As Integer = 0 To i - 1
                    Dim index As Integer = c
                    If node.Nodes.Find(path.Split("\")(index), False).Length = 0 Then
                        Invoke(Sub()
                                   node = node.Nodes.Add(path.Split("\")(index), path.Split("\")(index))
                                   node.ImageIndex = 0 ' folder image
                               End Sub)

                    Else
                        Invoke(Sub() node = node.Nodes.Find(path.Split("\")(index), False)(0))
                    End If
                Next
            End If

            Invoke(Sub()
                       node = node.Nodes.Add(b.title)
                       'add image
                       'ImageList1.Images.Add(b.image)
                       node.ImageIndex = 1 'ImageList1.Images.Count - 1
                       node.SelectedImageIndex = ImageList1.Images.Count - 1
                   End Sub)

        Next

        'TreeView1.ExpandAll()
        TreeView1.Nodes.Find("Root", False)(0).Expand()
        bookSelectionChange(False)
        favouriteChanged()
        Label1.Text = "Status: Up to date"
    End Sub

    Private Sub bookLoaded()
        If user.getLoadedBooks().Split("/")(0) = user.getLoadedBooks().Split("/")(1) Then
            Label1.Text = "Status: Up to date"
            guiInit()
        Else
            Label1.Text = "Loading books:" & user.getLoadedBooks()
        End If

        'add book to treeview
        Dim path As String = user.books(user.books.Count - 1).path.Substring(user.lib_path.Length + 1)
        Dim node As TreeNode = TreeView1.Nodes.Find("Root", False)(0)

        If path.Contains("\") Then
            Dim i As Integer = path.Count(Function(c As Char) c = "\")
            For c As Integer = 0 To i - 1
                Dim index As Integer = c
                If node.Nodes.Find(path.Split("\")(index), False).Length = 0 Then
                    Me.Invoke(Sub()
                                  node = node.Nodes.Add(path.Split("\")(index), path.Split("\")(index))
                                  node.ImageIndex = 0 ' folder image
                              End Sub)

                Else
                    node = node.Nodes.Find(path.Split("\")(index), False)(0)
                End If
            Next
        End If

        Me.Invoke(Sub()
                      node = node.Nodes.Add(user.books(user.books.Count - 1).title)

                      'add image
                      'ImageList1.Images.Add(b.image)
                      node.ImageIndex = 1 'ImageList1.Images.Count - 1
                      node.SelectedImageIndex = ImageList1.Images.Count - 1
                  End Sub)

    End Sub

    Private Sub FailedBookLoad(books As List(Of String))
        Label1.Text = "Status: Up to date"

        Dim le As New LoadErrors(books)
        If le.ShowDialog() = DialogResult.OK Then

        End If
    End Sub

    Private Function getPassword() As String
        Dim passwordDialog As New PasswordPromtForm
        If passwordDialog.ShowDialog() = DialogResult.OK Then
            Return passwordDialog.TextBox1.Text
        Else
            Return Nothing
        End If
        Return Nothing
    End Function

#End Region
#Region "Controls"
    Public Sub New()
        CheckForIllegalCrossThreadCalls = False
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            If Not System.IO.File.Exists("DataContainer.xml") Then
                My.Settings.encryption = False
                My.Settings.Save()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try



        selectedBook = Nothing
        Try
            If My.Settings.encryption Then
                Dim pass As String = Nothing
                'While pass Is Nothing
                pass = getPassword()
                'End While

                user = New User(My.Settings.lib_path, My.Settings.encryption, pass)

            Else
                user = New User(My.Settings.lib_path, My.Settings.encryption, String.Empty)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        'Handlers
        AddHandler user.failedLoadAllBooks, AddressOf FailedBookLoad
        AddHandler user.bookLoad, AddressOf bookLoaded
        AddHandler user.favouriteChanged, AddressOf favouriteChanged
        AddHandler user.lastReadUpdated, AddressOf lastReadUpdated
        Try
            user.Load()
        Catch ex As Exception
            MessageBox.Show("Could not open Data file: " & ex.Message)
            Application.Exit()
        End Try

        'Search box setup
        searchBox = New ListBox()
        Me.Controls.Add(searchBox)
        searchBox.Hide()
        searchBox.BringToFront()
        searchBox.Location = New Point(TextBox1.Location.X, TextBox1.Location.Y + TextBox1.Size.Height)
        searchBox.Size = New Size(TextBox1.Size.Width, 1000)

        guiInit()

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
            Dim bookinfo As New BookDetail(selectedBook, user)
            bookinfo.Show()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Settings button
        Dim setin As New Settings(user)
        setin.Show()
    End Sub

    Private Sub TreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect
        'Book selecion frontend changes
        ListBox1.SelectedIndex = -1
        If TreeView1.SelectedNode.Nodes.Count = 0 Then
            selectedBook = user.getBookByTitle(TreeView1.SelectedNode.Text)
            bookSelectionChange(True)
        Else
            selectedBook = Nothing
            bookSelectionChange(False)
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        'Favourite books
        If ListBox1.SelectedIndex > -1 Then
            selectedBook = user.getBookByTitle(ListBox1.SelectedItem)
            bookSelectionChange(True)
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        'Search
        If TextBox1.Text = "" Then
            searchBox.Hide()
        Else
            Dim searchedBooks As List(Of Book) = user.searchBooksByTitle(TextBox1.Text)
            If searchedBooks.Count = 0 Then
                searchBox.Hide()
            Else
                searchBox.Items.Clear()
                For Each b As Book In searchedBooks
                    searchBox.Items.Add(b.title)
                Next
                searchBox.Size = New Size(searchBox.Size.Width, (searchedBooks.Count + 1) * searchBox.ItemHeight + 2)
                searchBox.SelectedIndex = 0
                searchBox.Show()
            End If
        End If
    End Sub

    Private Sub searchbox_select() Handles searchBox.SelectedIndexChanged
        If searchBox.SelectedIndex > -1 Then
            selectedBook = user.getBookByTitle(searchBox.SelectedItem)
            bookSelectionChange(True)
        End If
    End Sub

    Private Sub searchbox_keyboard(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Down Then
            If searchBox.SelectedIndex < searchBox.Items.Count - 1 Then
                searchBox.SelectedIndex += 1
            End If
        ElseIf e.KeyCode = Keys.Up Then
            If searchBox.SelectedIndex > 0 Then
                searchBox.SelectedIndex -= 1
            End If
        ElseIf e.KeyCode = Keys.Enter Then
            TextBox1.Text = ""
            searchBox.Items.Clear()
        End If
    End Sub

    Private Sub TextBox1_Leave(sender As Object, e As EventArgs) Handles TextBox1.Leave
        searchBox.Hide()
    End Sub

#End Region
End Class
