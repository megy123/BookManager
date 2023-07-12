Public Class Book
    Dim l_name As String
    Dim l_autor As String
    Dim l_pages As Short
    Dim l_page As Short
    Dim l_comment As String
    Dim l_description As String
    Dim l_path As String
    Dim l_rating As String
    Dim l_image As Image
#Region "Constructors"
    Public Sub New(name As String, autor As String)
        Me.name = name
        Me.autor = autor
    End Sub

    Public Sub New(name As String, autor As String, pages As Short, page As Short, comment As String, description As String, path As String, image As Image, rating As String)
        Me.name = name
        Me.autor = autor
        Me.pages = pages
        Me.page = page
        Me.comment = comment
        Me.description = description
        Me.path = path
        Me.image = image
        Me.rating = rating
    End Sub
#End Region

#Region "Properties"
    Public Property name As String
        Get
            Return l_name
        End Get
        Set(value As String)
            l_name = value
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

    Public Property pages As Short
        Get
            Return l_pages
        End Get
        Set(value As Short)
            l_pages = value
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

    Public Property comment As String
        Get
            Return l_comment
        End Get
        Set(value As String)
            l_comment = value
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

    Public Property image As Image
        Get
            Return l_image
        End Get
        Set(value As Image)
            l_image = value
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

    Public Property rating As String
        Get
            Return l_rating
        End Get
        Set(value As String)
            l_rating = value
        End Set
    End Property

#End Region

#Region "Methods"
    Public Function getProgress() As Byte
        Return page / pages * 100
    End Function

    Public Function getState()
        Select Case page
            Case 0
                Return bookState.none
            Case pages
                Return bookState.finished
            Case Else
                Return bookState.reading
        End Select
    End Function

#End Region
End Class
