Imports Spire.Pdf
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
    Dim l_rating As SByte
    Dim l_status As SByte
    Dim l_image As Image


#Region "Constructors"
    Public Sub New(l_id As UInteger, l_title As String, l_autor As String, l_begin_date As Date, l_finish_date As Date, l_notes As String, l_description As String, l_page As Short, l_pages As Short, l_path As String, l_rating As SByte, l_status As SByte)
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

    Public Sub New(path As String, doc As XDocument)
        Dim fs As New IO.FileStream(path, IO.FileMode.Open)
        'Read header from file
        Dim binnary_reader As New IO.BinaryReader(fs)
        fs.Position = 0
        Dim header As Byte() = binnary_reader.ReadBytes(8)
        'MessageBox.Show(Hex(header(0)) & "/" & Hex(header(1)) & "/" & Hex(header(2)) & "/" & Hex(header(3)) & "/" & Hex(header(4)) & "/" & Hex(header(5)) & "/" & Hex(header(6)) & "/" & Hex(header(7)))
        binnary_reader.Close()
        fs.Dispose()
        'Check if header valid and get ID
        If header(0) = &H25 And header(1) = &HBB And header(7) = &H10 Then
            Me.l_id = BitConverter.ToUInt32(header, 3)
        Else
            Me.l_id = BitConverter.ToUInt32(getNewId(doc), 0) 'db.getNewBookId()
            assignIdToBook(Me.l_id, path)
        End If

        Dim hb As Byte() = BitConverter.GetBytes(Me.l_id)
        'MessageBox.Show("ID: " & Me.l_id & " HEX: " & Hex(hb(0)) & " " & Hex(hb(1)) & " " & Hex(hb(2)) & " " & Hex(hb(3)))

        loadBookDataOff(doc)
        loadBookData(path)

    End Sub


#End Region

#Region "Properties"
    Public Property id As UInteger
        Get
            Return l_id
        End Get
        Set(value As UInteger)
            l_id = value
        End Set
    End Property

    Public Property title As String
        Get
            Return l_title
        End Get
        Set(value As String)
            l_title = value
        End Set
    End Property

    Public Property autor As String
        Get
            Return l_autor
        End Get
        Set(value As String)
            l_autor = value
        End Set
    End Property

    Public Property begin_date As Date
        Get
            Return l_begin_date
        End Get
        Set(value As Date)
            l_begin_date = value
        End Set
    End Property

    Public Property finish_date As Date
        Get
            Return l_finish_date
        End Get
        Set(value As Date)
            l_finish_date = value
        End Set
    End Property

    Public Property notes As String
        Get
            Return l_notes
        End Get
        Set(value As String)
            l_notes = value
        End Set
    End Property

    Public Property description As String
        Get
            Return l_description
        End Get
        Set(value As String)
            l_description = value
        End Set
    End Property

    Public Property page As Short
        Get
            Return l_page
        End Get
        Set(value As Short)
            l_page = value
        End Set
    End Property

    Public Property pages As Short
        Get
            Return l_pages
        End Get
        Set(value As Short)
            l_pages = value
        End Set
    End Property

    Public Property path As String
        Get
            Return l_path
        End Get
        Set(value As String)
            l_path = value
        End Set
    End Property

    Public Property rating As SByte
        Get
            Return l_rating
        End Get
        Set(value As SByte)
            l_rating = value
        End Set
    End Property

    Public Property status As SByte
        Get
            Return l_status
        End Get
        Set(value As SByte)
            l_status = value
        End Set
    End Property

    Public Property image As Image
        Get
            Return l_image
        End Get
        Set(value As Image)
            l_image = value
        End Set
    End Property

#End Region

#Region "Methods"
    Public Function getStartDate() As String
        If begin_date = Nothing Then
            Return "-"
        Else
            Return Format(begin_date, "d.m.yyyy")
        End If
    End Function
    Public Function getFinishDate() As String
        If finish_date = Nothing Then
            Return "-"
        Else
            Return Format(finish_date, "d.m.yyyy")
        End If
    End Function
    Public Function getCategory() As String
        Return path.Split("/")(path.Count(Function(c As Char) c = "/"))
    End Function
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
    Private Function getNewId(doc As XDocument) As Byte()
        'Get highest value
        Dim maxId As UInteger = doc.Root.Element("HighestId").Value
        'Check for overflow
        'If b_id(0) = &HFF And b_id(1) = &HFF And b_id(2) = &HFF And b_id(3) = &HFF Then

        'End If
        maxId += 1
        'check validity
        Dim b_id As Byte() = BitConverter.GetBytes(maxId)
        If b_id(0) = &H10 Then
            b_id(0) += 1
        End If
        If b_id(1) = &H10 Then
            b_id(1) += 1
        End If
        If b_id(2) = &H10 Then
            b_id(2) += 1
        End If
        If b_id(3) = &H10 Then
            b_id(3) += 1
        End If


        doc.Root.Element("HighestId").Value = BitConverter.ToUInt32(b_id, 0) ' save new highest id
        Return b_id
    End Function
    Private Sub loadBookData(path As String)
        Dim doc As New PdfDocument()
        doc.LoadFromFile(path)

        Dim fileName As String = System.IO.Path.GetFileName(path)
        Me.l_title = fileName.Split("-")(0).Substring(0, fileName.Split("-")(0).Length - 1)
        Me.l_autor = fileName.Split("-")(1).Substring(1, fileName.Split(".")(0).Split("-")(1).Length - 5) '4 from '.pdf' 1 for beggining
        'Me.l_begin_date = Nothing
        'Me.l_finish_date = Nothing
        'Me.l_notes = ""
        Me.l_description = "" 'doc.Pages.Item(0). TODO
        'Me.l_page = 0
        Me.l_pages = doc.Pages.Count()
        Me.l_path = path
        'Me.l_rating = 0
        'Me.l_status = 0
        Me.l_image = doc.SaveAsImage(0)
    End Sub

    Private Sub loadBookDataOff(doc As XDocument)
        'add new book to container
        If doc.Root.Element("Books").Element("ID" & id) Is Nothing Then
            doc.Root.Element("Books").Add(New XElement("ID" & id,
                New XElement("BeginDate", "-"),
                New XElement("FinishDate", "-"),
                New XElement("Page", 0),
                New XElement("Rating", 0),
                New XElement("Status", 0),
                New XElement("Notes", "")
            ))

            doc.Save("DataContainer.xml")
        End If

        'load data from container
        If doc.Root.Element("Books").Element("ID" & id).Element("BeginDate").Value = "-" Then
            begin_date = Nothing
        Else
            begin_date = Convert.ToDateTime(doc.Root.Element("Books").Element("ID" & id).Element("BeginDate").Value)
        End If

        If doc.Root.Element("Books").Element("ID" & id).Element("FinishDate").Value = "-" Then
            finish_date = Nothing
        Else
            finish_date = Convert.ToDateTime(doc.Root.Element("Books").Element("ID" & id).Element("FinishDate").Value)
        End If

        page = doc.Root.Element("Books").Element("ID" & id).Element("Page").Value
        rating = doc.Root.Element("Books").Element("ID" & id).Element("Rating").Value
        status = doc.Root.Element("Books").Element("ID" & id).Element("Status").Value
        notes = doc.Root.Element("Books").Element("ID" & id).Element("Notes").Value
    End Sub


#End Region
End Class
