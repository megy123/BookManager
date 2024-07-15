﻿Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports PdfSharp.Pdf.IO

Public Class Book
    Public Event readDateChanged()
    Public Event bookRead()

    Dim l_id As Guid
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
    Public Sub New(l_id As Guid, l_title As String, l_autor As String, l_begin_date As Date, l_finish_date As Date, l_notes As String, l_description As String, l_page As Short, l_pages As Short, l_path As String, l_rating As SByte, l_status As SByte)
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
        'Get pdf book ID
        Dim byteBuffer As Byte() = System.IO.File.ReadAllBytes(path)
        Dim byteBufferAsString As String = System.Text.Encoding.UTF8.GetString(byteBuffer)
        Dim offset1 As Int32 = byteBufferAsString.IndexOf("/bmguid")


        If Not offset1 = -1 Then
            'Me.l_id = Guid.Parse(bookFile.Info.Elements.Item("/bmguid").ToString())
        Else
            'Dim bookFile As PdfDocument = PdfReader.Open(path, PdfDocumentOpenMode.Modify)
            'Me.l_id = Guid.NewGuid()
            'bookFile.Info.Elements.Add(New KeyValuePair(Of String, PdfItem)("/bmguid", New PdfString(Me.l_id.ToString())))
            'bookFile.Save(path)
        End If

        'load book data
        loadDataFromContainer(doc)
        loadDataFromBook(path)
    End Sub

#End Region

#Region "Properties"
    Public Property id As Guid
        Get
            Return l_id
        End Get
        Set(value As Guid)
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
            container.Root.Element("Books").Element("ID" & Me.l_id.ToString()).Element("BeginDate").Value = value
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
            container.Root.Element("Books").Element("ID" & Me.l_id.ToString()).Element("FinishDate").Value = value
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
            container.Root.Element("Books").Element("ID" & Me.l_id.ToString()).Element("Notes").Value = value
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
            container.Root.Element("Books").Element("ID" & Me.l_id.ToString()).Element("Page").Value = value
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
            container.Root.Element("Books").Element("ID" & Me.l_id.ToString()).Element("Rating").Value = value
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
            container.Root.Element("Books").Element("ID" & Me.l_id.ToString()).Element("Status").Value = value
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
        Return Nothing
    End Function
    'Private
    Private Sub loadDataFromBook(path As String)
        'init
        Try
            'Dim doc As PdfDocument = PdfReader.Open(path, PdfDocumentOpenMode.Modify)

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

            Me.l_pages = 100 'doc.Pages.Count()

            'Me.l_image = doc.SaveAsImage(0, 10, 10)
            'doc.Close()
            'doc.Dispose()
        Catch ex As Exception

            MessageBox.Show(path & ex.Message)
        End Try


    End Sub
    Private Sub loadDataFromContainer(doc As XDocument)
        'add new book to container if not exists
        If doc.Root.Element("Books").Element("ID" & id.ToString()) Is Nothing Then
            doc.Root.Element("Books").Add(New XElement("ID" & id.ToString(),
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
        If doc.Root.Element("Books").Element("ID" & id.ToString()).Element("BeginDate").Value = "-" Then
            l_begin_date = Nothing
        Else
            l_begin_date = Convert.ToDateTime(doc.Root.Element("Books").Element("ID" & id.ToString()).Element("BeginDate").Value)
        End If

        If doc.Root.Element("Books").Element("ID" & id.ToString()).Element("FinishDate").Value = "-" Then
            l_finish_date = Nothing
        Else
            l_finish_date = Convert.ToDateTime(doc.Root.Element("Books").Element("ID" & id.ToString()).Element("FinishDate").Value)
        End If

        l_page = doc.Root.Element("Books").Element("ID" & id.ToString()).Element("Page").Value
        l_rating = doc.Root.Element("Books").Element("ID" & id.ToString()).Element("Rating").Value
        l_status = doc.Root.Element("Books").Element("ID" & id.ToString()).Element("Status").Value
        l_notes = doc.Root.Element("Books").Element("ID" & id.ToString()).Element("Notes").Value
    End Sub
#End Region
End Class
