Imports System.Net
Imports System.Net.Security
Imports System.Net.Sockets
Imports System.Security.Cryptography.X509Certificates

Public Class SyncManager
    Dim Client As TcpClient
    Dim NetStream As SslStream
    Dim ip As String
    Dim port As Integer

#Region "Constructors"
    Public Sub New(ip As String, port As Integer)
        Me.ip = ip
        Me.port = port
    End Sub
#End Region

#Region "Properties"

#End Region

#Region "Methods"
    Private Function ValidateServerCertificate(sender As Object,
              certificate As X509Certificate,
              chain As X509Chain,
              sslPolicyErrors As SslPolicyErrors)

        If sslPolicyErrors = SslPolicyErrors.None Then
            Return True
        End If

        Return False
    End Function
    Public Function Connect() As Integer
        Client = New TcpClient(ip, port)
        NetStream = New SslStream(
            Me.Client.GetStream(),
            False,
            New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate),
            Nothing
            )

        Try
            NetStream.AuthenticateAsClient("ServerName")

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return 1 'Failed to connect
        End Try

        Return 0
    End Function
    Public Sub Disconnect()
        Client.Close()
    End Sub
    Public Function Pull(token As Guid) As Tuple(Of Integer, Byte())
        Dim message As Byte()

        'Message:  | 0x02 | GUID(16B) | 0xFF 0x25 0x55 0x66 |


    End Function
    Public Function Push(token As Guid, data As Byte()) As Tuple(Of Integer, Byte())

        'Message: | 0x03 | GUID(16B) | DATA(UNDEF) | 0xFF 0x25 0x55 0x66 |

    End Function
    Public Function GetToken(password As String) As Guid

        'Message: | 0x01 | SHA256_PASSWORD(32B) | 0xFF 0x25 0x55 0x66 |

    End Function
#End Region


End Class
