Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Security
Imports System.Security.Cryptography
Imports System.Text

Public NotInheritable Class CryptoManager
    Public Shared Function DerivateKey(password As String, iterations As Integer, keyByteLength As Integer) As Byte()
        ''getSalt
        'Dim salt(8) As Byte
        'Using csp As New RNGCryptoServiceProvider
        '    csp.GetBytes(salt)
        'End Using
        'salt = {25, 16, 25, 45, 86, 250, 85, 211}

        'Try
        '    Dim rfc = New Rfc2898DeriveBytes(password, salt, iterations)
        '    Return rfc.GetBytes(keyByteLength)
        'Catch ex As Exception
        '    MessageBox.Show(ex.Message)
        'End Try
        Dim s As SHA256 = SHA256Managed.Create()
        Return s.ComputeHash(Encoding.UTF8.GetBytes(password))


        Return Nothing
    End Function

    'Public Shared Function DerivateKey(password As SecureString, salt As Byte(), iterations As Integer, keyByteLength As Integer) As Byte()
    '    Dim prt As IntPtr = Marshal.SecureStringToBSTR(password)
    '    Dim passwordByteArray() As Byte = Nothing

    '    Try
    '        Dim len As Integer = Marshal.ReadInt32(ptr, -4)
    '        ReDim passwordByteArray(len)
    '        Dim handle As GCHandle = GCHandle.Alloc(passwordByteArray, GCHandleType.Pinned)
    '        Try
    '            For i As Integer = 0 To len
    '                passwordByteArray(i) = Marshal.ReadByte(ptr, i)
    '            Next

    '            Using rfc2898 As New Rfc2898DeriveBytes(passwordByteArray, salt, iterations)
    '                Return rfc2898.GetBytes(keyByteLength)
    '            End Using
    '        Catch ex As Exception
    '            MessageBox.Show(ex.Message)
    '        Finally
    '            Array.Clear(passwordByteArray, 0, passwordByteArray.Length)
    '            handle.Free()
    '        End Try
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message)
    '    Finally
    '        Marshal.ZeroFreeBSTR(ptr)
    '    End Try
    'End Function


    Public Shared Sub EncryptXmlAES(filename As String, key As Aes, doc As XDocument)
        Try
            Using fs As New FileStream(filename, FileMode.OpenOrCreate)

                Dim iv() As Byte = key.IV
                fs.Write(iv, 0, iv.Length)
                'key.IV = Encoding.ASCII.GetBytes("asdfasdfasdfasdf")

                Using cs As New CryptoStream(fs, key.CreateEncryptor(key.Key, key.IV), CryptoStreamMode.Write)
                    doc.Save(cs)
                End Using

            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Shared Function DecryptXmlAES(filename As String, key As Aes) As XDocument
        'key.IV = Encoding.ASCII.GetBytes("asdfasdfasdfasdf")
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

                key.IV = buff

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
