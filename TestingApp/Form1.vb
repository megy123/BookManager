Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Xml
Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim t_key As String = "asdfasdfasdfasdf"
        Dim t_iv As String = "asdfasdfasdfasdf"

        Dim doc As XDocument

        Dim MyKey As Aes = Aes.Create()
        MyKey.KeySize = 128
        MyKey.Key = Encoding.ASCII.GetBytes(t_key)
        MyKey.IV = Encoding.ASCII.GetBytes(t_iv)
        Dim encryptor As ICryptoTransform = MyKey.CreateEncryptor(MyKey.Key, MyKey.IV)
        Dim decryptor As ICryptoTransform = MyKey.CreateDecryptor(MyKey.Key, MyKey.IV)

        If (IO.File.Exists("test.xml")) Then

            'load document
            doc = DecryptXml("test.xml", MyKey)
            doc.Root.Value += 1


        Else
            'create New container
            doc = New XDocument(
                New XElement("data", 0)
            )

        End If

        Label1.Text = doc.Root.Value

        EncryptXml("test.xml", MyKey, doc)

    End Sub

    Public Sub Encrypt(filename As String, key As Aes, data As String)
        Try
            Using fs As New FileStream(filename, FileMode.OpenOrCreate)
                Dim iv() As Byte = key.IV

                fs.Write(iv, 0, iv.Length)

                Using cs As New CryptoStream(fs, key.CreateEncryptor(key.Key, key.IV), CryptoStreamMode.Write)
                    Using w As New StreamWriter(cs)
                        w.Write(data)
                    End Using
                End Using

            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Function Decrypt(filename As String, key As Aes) As String
        Try
            Using fs As New FileStream(filename, FileMode.Open)
                Dim buff(key.IV.Length) As Byte
                Dim bytesToRead As Integer = key.IV.Length
                Dim bytesRead As Integer = 0

                While bytesToRead > 0
                    Dim n As Integer = fs.Read(buff, bytesRead, bytesToRead)
                    If n = 0 Then
                        Exit While
                    End If

                    bytesRead += n
                    bytesToRead -= n
                End While

                Using cs As New CryptoStream(fs, key.CreateDecryptor(key.Key, key.IV), CryptoStreamMode.Read)
                    Using r As New StreamReader(cs)
                        Dim decrypted As String = r.ReadToEnd()
                        Return decrypted
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return "Err while decrypting"
        End Try
    End Function

    Public Sub EncryptXml(filename As String, key As Aes, doc As XDocument)
        Try
            Using fs As New FileStream(filename, FileMode.OpenOrCreate)
                Dim iv() As Byte = key.IV

                fs.Write(iv, 0, iv.Length)

                Using cs As New CryptoStream(fs, key.CreateEncryptor(key.Key, key.IV), CryptoStreamMode.Write)
                    doc.Save(cs)
                End Using

            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Function DecryptXml(filename As String, key As Aes) As XDocument
        Try
            Using fs As New FileStream(filename, FileMode.Open)
                Dim buff(key.IV.Length) As Byte
                Dim bytesToRead As Integer = key.IV.Length
                Dim bytesRead As Integer = 0

                While bytesToRead > 0
                    Dim n As Integer = fs.Read(buff, bytesRead, bytesToRead)
                    If n = 0 Then
                        Exit While
                    End If

                    bytesRead += n
                    bytesToRead -= n
                End While

                Using cs As New CryptoStream(fs, key.CreateDecryptor(key.Key, key.IV), CryptoStreamMode.Read)
                    Return XDocument.Load(cs)
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return New XDocument()
        End Try
    End Function
End Class
