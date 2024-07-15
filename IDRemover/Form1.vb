Imports FxResources
Imports PdfSharp.Pdf
Imports PdfSharp.Pdf.IO
Imports System.IO
Public Class Form1
    Dim doc As PdfDocument
    Dim path As String
    Dim folder_path As String
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            doc = PdfReader.Open(OpenFileDialog1.FileName)
            path = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim props As PdfDictionary.DictionaryElements = doc.Info.Elements

        Label1.Text = ""
        For Each kvp As KeyValuePair(Of String, PdfItem) In props
            Label1.Text &= kvp.Key & " : " & kvp.Value.ToString() & vbNewLine
        Next
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        doc.Info.Elements.Add(New KeyValuePair(Of String, PdfItem)(TextBox1.Text, New PdfString(TextBox2.Text)))
        doc.Save(path)
    End Sub

#Region "ID-remover"
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If FolderBrowserDialog1.ShowDialog() = DialogResult.OK Then
            folder_path = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Not folder_path = "" Then
            If ComboBox1.SelectedIndex = 0 Then
                removeV1Ids(folder_path)
            Else
                MessageBox.Show("No selected version.")
            End If
        Else
            MessageBox.Show("No path selected")
        End If
    End Sub

    Private Sub removeV1Ids(path As String)
        Dim local_books As List(Of String)

        local_books = getAllFiles(path)
        Dim bookCount As Integer = local_books.Count
        Dim processedBooks As Integer = 0
        Label2.Text = "0/" & bookCount

        For Each s As String In local_books
            'Read header from file
            Dim bytes As Byte() = File.ReadAllBytes(s)
            'Check if header valid v1 ID
            If bytes(0) = &H25 And bytes(1) = &HBB And bytes(2) = &H12 And bytes(7) = &H10 Then
                Dim newBytes(bytes.Length - 9) As Byte
                Buffer.BlockCopy(bytes, 8, newBytes, 0, bytes.Length - 8)
                File.WriteAllBytes(s, newBytes)
            End If

            processedBooks += 1
            Label2.Text = processedBooks & "/" & bookCount
        Next
    End Sub

    Private Function getAllFiles(path As String) As List(Of String)
        Dim files As New List(Of String)
        If System.IO.Directory.Exists(path) Then
            For Each f As String In System.IO.Directory.GetFiles(path)
                If f.EndsWith(".pdf") Then
                    files.Add(f)
                End If
            Next
            For Each d As String In System.IO.Directory.GetDirectories(path)
                files.AddRange(getAllFiles(d))
            Next
        Else
            MessageBox.Show("Non valid library path:""" & path & """.")
        End If
        Return files
    End Function

#End Region
End Class
