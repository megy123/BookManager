Public Class Book
    Dim l_id As UInteger
    Dim l_title As String
    Dim l_autor As String
    Dim l_begin_date As Date
    Dim l_finish_date As Date
    Dim l_notes As String
    Dim l_description As String
    Dim l_page As Short
    Dim l_pages As Short
    Dim l_path As String
    Dim l_rating As String
    Dim l_status As String
    Dim l_image As Image



#Region "Constructors"
    Public Sub New(l_id As UInteger, l_title As String, l_autor As String, l_begin_date As Date, l_finish_date As Date, l_notes As String, l_description As String, l_page As Short, l_pages As Short, l_path As String, l_rating As String, l_status As String)
        Me.l_id = l_id
        Me.l_title = l_title
        Me.l_autor = l_autor
        Me.l_begin_date = l_begin_date
        Me.l_finish_date = l_finish_date
        Me.l_notes = l_notes
        Me.l_description = l_description
        Me.l_page = l_page
        Me.l_pages = l_pages
        Me.l_path = l_path
        Me.l_rating = l_rating
        Me.l_status = l_status
    End Sub

    Public Sub New(path As String)
        Dim db As New DatabaseManager()
        Dim fs As New IO.FileStream(path, IO.FileMode.Open)
        'Read header from file
        Dim binnary_reader As New IO.BinaryReader(fs)
        fs.Position = 0
        Dim header As Byte() = binnary_reader.ReadBytes(8)
        MessageBox.Show(Hex(header(0)) & "/" & Hex(header(1)) & "/" & Hex(header(2)) & "/" & Hex(header(3)) & "/" & Hex(header(4)) & "/" & Hex(header(5)) & "/" & Hex(header(6)) & "/" & Hex(header(7)))
        binnary_reader.Close()
        fs.Dispose()
        'Check if header valid and get ID
        If header(0) = &H25 And header(1) = &HBB And header(7) = &H10 Then
            Me.l_id = BitConverter.ToUInt32(header, 3)
        Else
            Me.l_id = BitConverter.ToUInt32({&H52, &H18, &H9A, &HAA}, 0) 'db.getNewBookId()
            assignIdToBook(Me.l_id, path)
        End If

        Dim hb As Byte() = BitConverter.GetBytes(Me.l_id)
        MessageBox.Show("ID: " & Me.l_id & " HEX: " & Hex(hb(0)) & " " & Hex(hb(1)) & " " & Hex(hb(2)) & " " & Hex(hb(3)))


    End Sub


#End Region

#Region "Properties"

#End Region

#Region "Methods"
    Public Function getProgress() As Byte

    End Function

    'Public Function getState()
    '    Select Case page
    '        Case 0
    '            Return bookState.none
    '        Case pages
    '            Return bookState.finished
    '        Case Else
    '            Return bookState.reading
    '    End Select
    'End Function

    Private Sub assignIdToBook(id As UInteger, path As String)
        'Initilize variables
        Dim file_content As Byte() = My.Computer.FileSystem.ReadAllBytes(path)
        Dim fs As New IO.FileStream(path, IO.FileMode.Open)
        Dim binnary_writer As New IO.BinaryWriter(fs)
        'Write header
        binnary_writer.Write({&H25, &HBB, &H12})
        binnary_writer.Write(id)
        binnary_writer.Write({&H10})
        'Write content
        binnary_writer.Write(file_content)
        'Close file
        binnary_writer.Close()
    End Sub
#End Region
End Class
