Public Class Book
    Dim l_id As Integer
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
    Public Sub New(l_id As Integer, l_title As String, l_autor As String, l_begin_date As Date, l_finish_date As Date, l_notes As String, l_description As String, l_page As Short, l_pages As Short, l_path As String, l_rating As String, l_status As String)
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
        Dim fs As New IO.FileStream(path, IO.FileMode.Open)
        Dim binnary_reader As New IO.BinaryReader(fs)
        fs.Position = 0
        Dim header As Byte() = binnary_reader.ReadBytes(3)
        MessageBox.Show(Hex(header(0)) & "/" & Hex(header(1)) & "/" & Hex(header(2)))
        binnary_reader.Close()
        fs.Dispose()
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

#End Region
End Class
