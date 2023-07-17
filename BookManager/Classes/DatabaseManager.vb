Imports System.Data.SqlClient
Imports System.Data
Public Class DatabaseManager
    Const connection_string As String = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\dominik\Programovanie\Projekty\Small projects & utilities\BookManager\BookManager\BookManagerDatabase.mdf"";Integrated Security=True"
    Dim connection As SqlConnection

    Public Sub New()
        Try
            connection = New SqlConnection(connection_string)
            connection.Open()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Function getUserData(u As User)
        Try
            Dim command As SqlCommand = connection.CreateCommand()
            Dim reader As SqlDataReader
            command.CommandText = "EXECUTE getUserData"
            reader = command.ExecuteReader()
            reader.Read()
            u.confirm_sync = reader.GetBoolean(2)
            u.startup_sync = reader.GetBoolean(1)
            u.last_read = reader.GetDateTime(4)
            u.last_sync = reader.GetDateTime(3)
            u.lib_path = reader.GetString(5)


            reader.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Function

    Public Function getUserBooks(books As List(Of Book))
        Try
            Dim command As SqlCommand = connection.CreateCommand()
            Dim reader As SqlDataReader
            command.CommandText = "EXECUTE getUserBooks"
            reader = command.ExecuteReader()
            While reader.Read()
                Dim book As New Book()

                'TODO dopíš content book

                books.Add(book)
            End While


            reader.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Function

    Public Function getUserFavBooks(favs As List(Of Book))
        Try
            Dim command As SqlCommand = connection.CreateCommand()
            Dim reader As SqlDataReader
            command.CommandText = "EXECUTE getUserFavs"
            reader = command.ExecuteReader()
            While reader.Read()
                Dim book As New Book()

                'TODO dopíš content book

                favs.Add(book)
            End While


            reader.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Function
End Class
