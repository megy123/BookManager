﻿Public Class MainForm
    Dim user As User
    Dim selectedBook As Book
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'user = New User()
        ''display data
        'Label7.Text = "You read lately on " & Format(user.last_read, "d. mmmm yyyy")
        'For Each b As Book In user.favourite
        '    ListBox1.Items.Add(b.title)
        'Next
        'Label2.Text = "Completed: " & user.GetCompleted()
        'Label3.Text = "Reading: " & user.GetReading()
        'Label4.Text = "Book count: " & user.GetBookCount()
        'Label5.Text = "Dropped: " & user.GetDropped()
        'Label6.Text = "Read pages: " & user.GetPageCount()

        user = New User(False)
        selectedBook = Nothing

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
