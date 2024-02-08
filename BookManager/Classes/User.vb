Imports System.ComponentModel
Imports System.Reflection.Emit
Imports System.Threading
Imports System.Xml
Public Class User
    Public Event favouriteChanged()
    Public Event lastReadUpdated()
    Public Event bookLoad()

    Dim l_last_sync As Date?
    Dim l_last_read As Date?
    Dim l_startup_sync As Boolean
    Dim l_confirm_sync As Boolean
    Dim l_lib_path As String
    Dim l_favourite As List(Of Book)
    Dim l_books As List(Of Book)

    Dim l_booksCount As Integer
    Dim l_lbooks As Integer


#Region "Constructors"
    Public Sub New()
        AddHandler bookLoad, AddressOf BooksLoaded
        Dim container As XDocument = getDataContainer()
        loadUserData(container)
        If lib_path = "" Then 'set library path to default
            lib_path = My.Application.Info.DirectoryPath
        End If

        Dim tThread1 As New Thread(Sub() loadUserBooks(container))
        tThread1.Start()
        'loadUserFavouriteBooks(container) <-- toto je v BooksLoaded() funkcii
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
    Public Property last_sync As Date?
        Get
            Return l_last_sync
        End Get
        Set(value As Date?)
            l_last_sync = value
            Dim container As XDocument = getDataContainer()
            If value Is Nothing Then
                container.Root.Element("LastSynchronization").Value = "-"
            Else
                container.Root.Element("LastSynchronization").Value = value
            End If

            container.Save("DataContainer.xml")
        End Set
    End Property
    Public Property last_read As Date?
        Get
            Return l_last_read
        End Get
        Set(value As Date?)
            l_last_read = value
            Dim container As XDocument = getDataContainer()
            If value Is Nothing Then
                container.Root.Element("LastRead").Value = "-"
            Else
                container.Root.Element("LastRead").Value = value
            End If

            container.Save("DataContainer.xml")
        End Set
    End Property
    Public Property startup_sync As Boolean
        Get
            Return l_startup_sync
        End Get
        Set(value As Boolean)
            l_startup_sync = value
            Dim container As XDocument = getDataContainer()
            container.Root.Element("StartupSynchronization").Value = value
            container.Save("DataContainer.xml")
        End Set
    End Property
    Public Property confirm_sync As Boolean
        Get
            Return l_confirm_sync
        End Get
        Set(value As Boolean)
            l_confirm_sync = value
            Dim container As XDocument = getDataContainer()
            container.Root.Element("ConfirmSynchronization").Value = value
            container.Save("DataContainer.xml")
        End Set
    End Property
    Public Property lib_path As String
        Get
            Return l_lib_path
        End Get
        Set(value As String)
            l_lib_path = value
            Dim container As XDocument = getDataContainer()
            container.Root.Element("LibraryPath").Value = value
            container.Save("DataContainer.xml")
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
    Public Sub sync()
        last_sync = My.Computer.Clock.LocalTime
    End Sub
    Public Function searchBooksByTitle(title As String) As List(Of Book)
        Dim b As New List(Of Book)

        For Each book As Book In books
            If book.title.StartsWith(title) Then
                b.Add(book)
            End If
        Next

        Return b
    End Function
    Public Sub addToFavourite(b As Book)
        favourite.Add(b)
        'save to container
        Dim container As XDocument = getDataContainer()
        container.Root.Element("FavouriteBooks").Add(New XElement(b.id))
        container.Save("DataContainer.xml")

        RaiseEvent favouriteChanged()
    End Sub
    Public Sub removeFromFavourite(b As Book)
        favourite.Remove(b)
        'save to container
        Dim container As XDocument = getDataContainer()
        container.Root.Element("FavouriteBooks").Element(b.id).Remove()
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
        'skus ptm prerobit
        'PTM PREROB -- Return WorkOrders.Find(Function(wo As clsWorkOrders) wo.WoNumber = woNumber) IsNot Nothing
        For Each b As Book In books
            If b.title = title Then Return b
        Next
        Return Nothing
    End Function
    Public Function getBookById(id As String) As Book
        'wiederholen
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
    Public Function getLoadedBooks() As String
        Return l_lbooks & "/" & l_booksCount
    End Function
    'Private methods
    Private Function getAllFiles(path As String) As List(Of String)
        Dim files As New List(Of String)
        If IO.Directory.Exists(path) Then
            For Each f As String In IO.Directory.GetFiles(path)
                If f.EndsWith(".pdf") Then
                    files.Add(f)
                End If
            Next
            For Each d As String In IO.Directory.GetDirectories(path)
                files.AddRange(getAllFiles(d))
            Next
        Else
            MessageBox.Show("Non valid library path:""" & path & """.")
        End If
        Return files
    End Function
    Private Function getDataContainer() As XDocument
        'load data container, create if not exists
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
        'loads data from container
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
        l_booksCount = 0
        l_lbooks = 0
    End Sub
    Private Sub loadUserBooks(doc As XDocument)
        Dim prevBooks As New List(Of String)
        Dim newBooks As New List(Of String)

        For Each el As XElement In doc.Root.Element("Books").Elements
            prevBooks.Add(el.Name.LocalName)
        Next

        books = New List(Of Book)

        Dim local_books As List(Of String)
        local_books = getAllFiles(lib_path)
        l_booksCount = local_books.Count

        For Each s As String In local_books
            newBooks.Add(IO.Path.GetFileName(s).Replace(" ", "_").Replace(",", "_"))
            Dim b As New Book(s, doc)
            AddHandler b.bookRead, AddressOf lastReadUpdatedHandler
            books.Add(b)
            l_lbooks += 1
            RaiseEvent bookLoad()
        Next


        'get all books
        Dim allBooksCnt As Integer = newBooks.Count

        'get new books
        Dim newBooksCnt As Integer = 0
        Dim addedBooks As New List(Of String)
        For Each s As String In newBooks
            If Not prevBooks.ToArray().Contains(s) Then
                newBooksCnt += 1
                addedBooks.Add(s)
            End If
        Next

        'get removed books
        Dim removedBooksCnt As Integer = 0
        Dim removedBooks As New List(Of String)
        For Each s As String In prevBooks
            If Not newBooks.ToArray().Contains(s) Then
                removedBooksCnt += 1
                removedBooks.Add(s)
            End If
        Next


        'MessageBox.Show("old book:" & prevBooks(0) & vbCrLf & "new book:" & newBooks(0))
        If newBooksCnt Or removedBooksCnt Then
            Dim syncDialog As New Syncing(allBooksCnt, newBooksCnt, removedBooksCnt)
            If syncDialog.ShowDialog() = DialogResult.OK Then

            Else

            End If

            'remove missing books
            For Each title As String In removedBooks
                RemoveBook(doc, title)
            Next
        End If

    End Sub
    Private Sub loadUserFavouriteBooks(doc As XDocument)
        favourite = New List(Of Book)
        For Each fb As XElement In doc.Root.Element("FavouriteBooks").Elements
            Dim book As Book = getBookById(fb.Name.LocalName)
            If Not IsNothing(book) Then
                favourite.Add(book)
            End If
        Next
    End Sub
    Private Sub lastReadUpdatedHandler()
        last_read = My.Computer.Clock.LocalTime
        RaiseEvent lastReadUpdated()
    End Sub 'Handler function

    Private Sub BooksLoaded()
        If getLoadedBooks().Split("/")(0) = getLoadedBooks().Split("/")(1) Then
            Dim container As XDocument = getDataContainer()
            loadUserFavouriteBooks(container)
        End If
    End Sub

    Private Sub RemoveBook(doc As XDocument, bookTitle As String)
        'remove book from container
        My.Computer.FileSystem.WriteAllText("deletedBooks.log", doc.Root.Element("Books").Element(bookTitle).ToString(), True)
        doc.Root.Element("Books").Element(bookTitle).Remove()
        doc.Save("DataContainer.xml")

        'remove from favourites
        If doc.Root.Element("FavouriteBooks").Element(bookTitle) IsNot Nothing Then
            doc.Root.Element("FavouriteBooks").Element(bookTitle).Remove()
        End If
    End Sub
#End Region
End Class
