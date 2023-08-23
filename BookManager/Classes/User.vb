Public Class User
    Dim l_last_sync As Date
    Dim l_last_read As Date
    Dim l_startup_sync As Boolean
    Dim l_confirm_sync As Boolean
    Dim l_lib_path As String
    Dim l_favourite As List(Of Book)
    Dim l_books As List(Of Book)



#Region "Constructors"
    Public Sub New()
        Dim db As New DatabaseManager()
        db.getUserData(Me)
        db.getUserBooks(Me.books)
        db.getUserFavBooks(Me.favourite)
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
    Public Sub addBook()

    End Sub
    Public Sub removeBook()

    End Sub
    Public Sub editBook()

    End Sub
    Public Sub addToFavourite()

    End Sub
    Public Sub removeFromFavourite()

    End Sub
    Public Sub sync()

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
#End Region
End Class
