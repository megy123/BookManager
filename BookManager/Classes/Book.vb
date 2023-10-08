﻿Imports Spire.Pdf
Public Class Book
    Public Event readDateChanged()
    Public Event bookRead()

    Dim l_id As UInteger
    Dim l_title As String
    Dim l_autor As String
    Dim l_begin_date As Date?
    Dim l_finish_date As Date?
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
        'Read header from file
        Dim fs As New IO.FileStream(path, IO.FileMode.Open)
        Dim binnary_reader As New IO.BinaryReader(fs)
        fs.Position = 0
        Dim header As Byte() = binnary_reader.ReadBytes(8)
        binnary_reader.Close()
        fs.Dispose()
        'Check if header valid and get ID
        If header(0) = &H25 And header(1) = &HBB And header(2) = &H12 And header(7) = &H10 Then
            Me.l_id = BitConverter.ToUInt32(header, 3)
        Else
            Me.l_id = BitConverter.ToUInt32(getNewId(doc), 0) 'db.getNewBookId()
            assignIdToBook(Me.l_id, path)
        End If

        'load book data
        loadDataFromContainer(doc)
        loadDataFromBook(path)
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
    Public Property begin_date As Date?
        Get
            Return l_begin_date
        End Get
        Set(value As Date?)
            l_begin_date = value
            Dim container As XDocument = XDocument.Load("DataContainer.xml")
            container.Root.Element("Books").Element("ID" & Me.l_id).Element("BeginDate").Value = value
            container.Save("DataContainer.xml")
        End Set
    End Property
    Public Property finish_date As Date?
        Get
            Return l_finish_date
        End Get
        Set(value As Date?)
            l_finish_date = value
            Dim container As XDocument = XDocument.Load("DataContainer.xml")
            container.Root.Element("Books").Element("ID" & Me.l_id).Element("FinishDate").Value = value
            container.Save("DataContainer.xml")
        End Set
    End Property
    Public Property notes As String
        Get
            Return l_notes
        End Get
        Set(value As String)
            l_notes = value
            Dim container As XDocument = XDocument.Load("DataContainer.xml")
            container.Root.Element("Books").Element("ID" & Me.l_id).Element("Notes").Value = value
            container.Save("DataContainer.xml")
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
            RaiseEvent bookRead()
            Dim container As XDocument = XDocument.Load("DataContainer.xml")
            container.Root.Element("Books").Element("ID" & Me.l_id).Element("Page").Value = value
            container.Save("DataContainer.xml")
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
            Dim container As XDocument = XDocument.Load("DataContainer.xml")
            container.Root.Element("Books").Element("ID" & Me.l_id).Element("Rating").Value = value
            container.Save("DataContainer.xml")
        End Set
    End Property
    Public Property status As SByte
        Get
            Return l_status
        End Get
        Set(value As SByte)
            l_status = value
            Dim container As XDocument = XDocument.Load("DataContainer.xml")
            container.Root.Element("Books").Element("ID" & Me.l_id).Element("Status").Value = value
            container.Save("DataContainer.xml")
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
    'Public
    Public Function getStartDate() As String
        If begin_date Is Nothing Then
            Return "-"
        Else
            Return Format(begin_date, "d.M.yyyy")
        End If
    End Function
    Public Function getFinishDate() As String
        If finish_date Is Nothing Then
            Return "-"
        Else
            Return Format(finish_date, "d.M.yyyy")
        End If
    End Function
    Public Function getCategory() As String
        Return path.Split("\")(path.Count(Function(c As Char) c = "\") - 1) 'last directory from path
    End Function
    Public Function getImage() As Image
        Dim doc As New PdfDocument()
        doc.LoadFromFile(path)
        Return doc.SaveAsImage(0)
    End Function
    'Private
    Private Sub assignIdToBook(id As UInteger, path As String)
        'Initilize variables
        Dim file_content As Byte() = My.Computer.FileSystem.ReadAllBytes(path)
        Dim fs As New IO.FileStream(path, IO.FileMode.Open)
        Dim binnary_writer As New IO.BinaryWriter(fs)
        'Write data
        binnary_writer.Write({&H25, &HBB, &H12}) 'Header
        binnary_writer.Write(id) 'Id
        binnary_writer.Write({&H10}) 'ending
        'Write content
        binnary_writer.Write(file_content)
        'Close file
        binnary_writer.Close()
    End Sub
    Private Function getNewId(doc As XDocument) As Byte()
        'Get highest value
        Dim maxId As UInteger = doc.Root.Element("HighestId").Value
        'Check for overflow
        If BitConverter.GetBytes(maxId).SequenceEqual({&HFF, &HFF, &HFF, &HFF}) Then
            MessageBox.Show("Maximum id limit reached")
            Application.Exit()
        End If
        'rise id
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
        'save new highest id
        doc.Root.Element("HighestId").Value = BitConverter.ToUInt32(b_id, 0)
        Return b_id
    End Function
    Private Sub loadDataFromBook(path As String)
        'init
        Try
            PdfDocument.ClearCustomFontsFolders()
            Dim doc As New PdfDocument()
            doc.LoadFromFile(path)

            Me.l_path = path
            Dim fileName As String = System.IO.Path.GetFileName(path)
            'set data
            If fileName.Count(Function(c As Char) c = "-") = 1 Then
                Me.l_title = fileName.Split("-")(0).Substring(0, fileName.Split("-")(0).Length - 1)
                Me.l_autor = fileName.Split("-")(1).Substring(1, fileName.Split("-")(1).Length - 5) '4 from '.pdf' 1 for beggining
            Else
                Me.l_title = fileName.Substring(1, fileName.Length - 5) '4 from '.pdf' 1 for beggining
                Me.l_autor = ""
            End If

            Me.l_description = ""
            'Dim cnt As Integer = doc.Pages.Count
            'If cnt > 20 Then cnt = 20
            'For i As Integer = 0 To 20
            '    If doc.Pages.Item(i).ExtractText().ToLower.Contains("introduction") Then
            '        Me.l_description = doc.Pages(i).ExtractText()
            '        Exit For
            '    End If
            'Next

            Me.l_pages = doc.Pages.Count()

            'Me.l_image = doc.SaveAsImage(0, 10, 10)
            doc.Close()
            doc.Dispose()
        Catch ex As Exception

            MessageBox.Show(path & ex.Message)
        End Try


    End Sub
    Private Sub loadDataFromContainer(doc As XDocument)
        'add new book to container if not exists
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
            l_begin_date = Nothing
        Else
            l_begin_date = Convert.ToDateTime(doc.Root.Element("Books").Element("ID" & id).Element("BeginDate").Value)
        End If

        If doc.Root.Element("Books").Element("ID" & id).Element("FinishDate").Value = "-" Then
            l_finish_date = Nothing
        Else
            l_finish_date = Convert.ToDateTime(doc.Root.Element("Books").Element("ID" & id).Element("FinishDate").Value)
        End If

        l_page = doc.Root.Element("Books").Element("ID" & id).Element("Page").Value
        l_rating = doc.Root.Element("Books").Element("ID" & id).Element("Rating").Value
        l_status = doc.Root.Element("Books").Element("ID" & id).Element("Status").Value
        l_notes = doc.Root.Element("Books").Element("ID" & id).Element("Notes").Value
    End Sub
#End Region
End Class
