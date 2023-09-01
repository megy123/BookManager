﻿Imports System.Xml
Public Class User
    Public Event favouriteChanged()

    Dim l_last_sync As Date
    Dim l_last_read As Date
    Dim l_startup_sync As Boolean
    Dim l_confirm_sync As Boolean
    Dim l_lib_path As String
    Dim l_favourite As List(Of Book)
    Dim l_books As List(Of Book)



#Region "Constructors"
    Public Sub New()
        Dim container As XDocument = getDataContainer()
        loadUserData(container)
        loadUserBooks(container)
        loadUserFavouriteBooks(container)

    End Sub

    Public Sub New(l_last_sync As Date, l_last_read As Date, l_startup_sync As Boolean, l_confirm_sync As Boolean, l_lib_path As String, l_favourite As List(Of Book), l_books As List(Of Book))
        Me.l_last_sync = l_last_sync
        Me.l_last_read = l_last_read
        Me.l_startup_sync = l_startup_sync
        Me.l_confirm_sync = l_confirm_sync
        Me.l_lib_path = l_lib_path
        Me.l_favourite = l_favourite
        Me.l_books = l_books
    End Sub

#End Region

#Region "Properties"
    Public Property last_sync As Date
        Get
            Return l_last_sync
        End Get
        Set(value As Date)
            l_last_sync = value
        End Set
    End Property

    Public Property last_read As Date
        Get
            Return l_last_read
        End Get
        Set(value As Date)
            l_last_read = value
        End Set
    End Property

    Public Property startup_sync As Boolean
        Get
            Return l_startup_sync
        End Get
        Set(value As Boolean)
            l_startup_sync = value
        End Set
    End Property

    Public Property confirm_sync As Boolean
        Get
            Return l_confirm_sync
        End Get
        Set(value As Boolean)
            l_confirm_sync = value
        End Set
    End Property

    Public Property lib_path As String
        Get
            Return l_lib_path
        End Get
        Set(value As String)
            l_lib_path = value
        End Set
    End Property

    Public Property favourite As List(Of Book)
        Get
            Return l_favourite
        End Get
        Set(value As List(Of Book))
            l_favourite = value
        End Set
    End Property

    Public Property books As List(Of Book)
        Get
            Return l_books
        End Get
        Set(value As List(Of Book))
            l_books = value
        End Set
    End Property

#End Region

#Region "Methods"

    'Public methods
    'Public Sub addBook()

    'End Sub
    'Public Sub removeBook()

    'End Sub
    'Public Sub editBook()

    'End Sub


    'Public Sub sync()
    '    Dim local_books As List(Of String)
    '    local_books = getAllFiles(lib_path)
    '    Dim bookIndex As Integer = 1

    '    'syncform init
    '    Dim sync_form As Syncing = New Syncing()
    '    sync_form.Label2.Text = "0/" & local_books.Count
    '    sync_form.Label3.Text = ""
    '    sync_form.Label4.Text = ""
    '    sync_form.Button1.Enabled = False
    '    sync_form.Button2.Enabled = False
    '    sync_form.Show()


    '    For Each s As String In local_books
    '        Dim fileName As String = System.IO.Path.GetFileName(s)
    '        sync_form.Label2.Text = bookIndex & "/" & local_books.Count
    '        sync_form.Label3.Text = fileName.Split("-")(0).Substring(0, fileName.Split("-")(0).Length - 1)
    '        sync_form.Label4.Text = fileName.Split("-")(1).Substring(1, fileName.Split("-")(1).Length)
    '        bookIndex += 1

    '        Dim dm As New DatabaseManager()
    '        If dm.checkBook = False Then

    '        End If
    '    Next
    'End Sub

    Public Sub addToFavourite(b As Book)
        favourite.Add(b)
        Dim container As XDocument = getDataContainer()
        container.Root.Element("FavouriteBooks").Add(New XElement("ID" & b.id))
        container.Save("DataContainer.xml")
        RaiseEvent favouriteChanged()
    End Sub
    Public Sub removeFromFavourite(b As Book)
        favourite.Remove(b)
        Dim container As XDocument = getDataContainer()
        container.Root.Element("FavouriteBooks").Element("ID" & b.id).Remove()
        container.Save("DataContainer.xml")
        RaiseEvent favouriteChanged()
    End Sub
    Public Function GetCompleted() As Integer
        Dim count As Integer = 0
        For Each b As Book In books
            If b.status = status.Completed Then count += 1
        Next
        Return count
    End Function
    Public Function GetReading() As Integer
        Dim count As Integer = 0
        For Each b As Book In books
            If b.status = status.Reading Then count += 1
        Next
        Return count
    End Function
    Public Function GetBookCount() As Integer
        Return books.Count
    End Function
    Public Function GetDropped() As Integer
        Dim count As Integer = 0
        For Each b As Book In books
            If b.status = status.Dropped Then count += 1
        Next
        Return count
    End Function
    Public Function GetPageCount() As Integer
        Dim count As Integer = 0
        For Each b As Book In books
            count += b.page
        Next
        Return count
    End Function

    Public Function getBookByTitle(title As String) As Book
        For Each b As Book In books
            If b.title = title Then Return b
        Next
        Return Nothing
    End Function

    Public Function getBookById(id As Integer) As Book
        For Each b As Book In books
            If b.id = id Then Return b
        Next
        Return Nothing
    End Function

    Public Function isFavourite(b As Book) As Boolean
        For Each book As Book In favourite
            If book Is b Then Return True
        Next
        Return False
    End Function

    'Private methods
    Private Function getAllFiles(path As String) As List(Of String)
        Dim files As New List(Of String)
        For Each f As String In IO.Directory.GetFiles(path)
            files.Add(f)
        Next
        For Each d As String In IO.Directory.GetDirectories(path)
            files.AddRange(getAllFiles(d))
        Next

        Return files
    End Function
    Private Function getDataContainer() As XDocument
        If (IO.File.Exists("DataContainer.xml")) Then
            Return XDocument.Load("DataContainer.xml")
        Else
            Dim doc As New XDocument(
                New XComment("User data."),
                New XElement("User",
                    New XElement("LastSynchronization", "-"),
                    New XElement("LastRead", "-"),
                    New XElement("StartupSynchronization", True),
                    New XElement("ConfirmSynchronization", False),
                    New XElement("LibraryPath", ""),
                    New XElement("HighestId", 0),
                    New XComment("User books."),
                    New XElement("Books"),
                    New XComment("User favourite books."),
                    New XElement("FavouriteBooks")
                )
            )

            doc.Save("DataContainer.xml")

            Return doc
        End If
    End Function
    Private Sub loadUserData(doc As XDocument)
        If doc.Root.Element("LastSynchronization").Value = "-" Then
            last_sync = Nothing
        Else
            last_sync = Convert.ToDateTime(doc.Root.Element("LastSynchronization").Value)
        End If

        If doc.Root.Element("LastRead").Value = "-" Then
            last_read = Nothing
        Else
            last_read = Convert.ToDateTime(doc.Root.Element("LastRead").Value)
        End If

        startup_sync = doc.Root.Element("StartupSynchronization").Value
        confirm_sync = doc.Root.Element("ConfirmSynchronization").Value
        lib_path = doc.Root.Element("LibraryPath").Value
    End Sub

    Private Sub loadUserBooks(doc As XDocument)
        books = New List(Of Book)

        Dim local_books As List(Of String)
        local_books = getAllFiles(lib_path)

        For Each s As String In local_books
            Dim b As New Book(s, doc)
            books.Add(b)
        Next
    End Sub

    Private Sub loadUserFavouriteBooks(doc As XDocument)
        favourite = New List(Of Book)
        For Each fb As XElement In doc.Root.Element("FavouriteBooks").Elements
            Dim book As Book = getBookById(Integer.Parse(fb.Name.LocalName.Substring(2)))
            If Not IsNothing(book) Then
                favourite.Add(book)
            End If
        Next
    End Sub
#End Region
End Class
