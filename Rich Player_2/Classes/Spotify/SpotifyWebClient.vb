
Imports Newtonsoft.Json
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Text
Imports System.Threading.Tasks


Friend Class SpotifyWebClient
    ' Implements IClient
    Public Property JsonSettings() As JsonSerializerSettings
        Get
            Return m_JsonSettings
        End Get
        Set(value As JsonSerializerSettings)
            m_JsonSettings = value
        End Set
    End Property
    Private m_JsonSettings As JsonSerializerSettings

    Private ReadOnly _webClient As WebClient
    Private ReadOnly _encoding As Encoding = Encoding.UTF8

    Public Sub New()
        _webClient = New WebClient() With { _
             .Proxy = Nothing, _
             .Encoding = _encoding _
        }
    End Sub

    Public Sub Dispose()
        _webClient.Dispose()
    End Sub

    Public Function Download(url As String) As String
        Dim response As String
        Try
            response = _encoding.GetString(DownloadRaw(url))
        Catch e As WebException
            Using reader As New StreamReader(e.Response.GetResponseStream())
                response = reader.ReadToEnd()
            End Using
        End Try
        Return response
    End Function

    Public Async Function DownloadAsync(url As String) As Task(Of String)
        Dim response As String
        Try
            response = _encoding.GetString(Await DownloadRawAsync(url))
        Catch e As WebException
            Using reader As New StreamReader(e.Response.GetResponseStream())
                response = reader.ReadToEnd()
            End Using
        End Try
        Return response
    End Function

    Public Function DownloadRaw(url As String) As Byte()
        Return _webClient.DownloadData(url)
    End Function

    Public Async Function DownloadRawAsync(url As String) As Task(Of Byte())
        Using webClient As New WebClient()
            webClient.Proxy = Nothing
            webClient.Encoding = _encoding
            webClient.Headers = _webClient.Headers
            Return Await _webClient.DownloadDataTaskAsync(url)
        End Using
    End Function

    Public Function DownloadJson(Of T)(url As String) As T
        Dim response As String = Download(url)
        Return JsonConvert.DeserializeObject(Of T)(response, JsonSettings)
    End Function

    Public Async Function DownloadJsonAsync(Of T)(url As String) As Task(Of T)
        Dim response As String = Await DownloadAsync(url)
        Return JsonConvert.DeserializeObject(Of T)(response, JsonSettings)
    End Function

    Public Function Upload(url As String, body As String, method As String) As String
        Dim response As String
        Try
            Dim data As Byte() = UploadRaw(url, body, method)
            response = _encoding.GetString(data)
        Catch e As WebException
            Using reader As New StreamReader(e.Response.GetResponseStream())
                response = reader.ReadToEnd()
            End Using
        End Try
        Return response
    End Function

    Public Async Function UploadAsync(url As String, body As String, method As String) As Task(Of String)
        Dim response As String
        Try
            Dim data As Byte() = Await UploadRawAsync(url, body, method)
            response = _encoding.GetString(data)
        Catch e As WebException
            Using reader As New StreamReader(e.Response.GetResponseStream())
                response = reader.ReadToEnd()
            End Using
        End Try
        Return response
    End Function

    Public Function UploadRaw(url As String, body As String, method As String) As Byte()
        Return _webClient.UploadData(url, method, _encoding.GetBytes(body))
    End Function

    Public Async Function UploadRawAsync(url As String, body As String, method As String) As Task(Of Byte())
        Using webClient As New WebClient()
            webClient.Proxy = Nothing
            webClient.Encoding = _encoding
            webClient.Headers = _webClient.Headers
            Return Await _webClient.UploadDataTaskAsync(url, method, _encoding.GetBytes(body))
        End Using
    End Function

    Public Function UploadJson(Of T)(url As String, body As String, method As String) As T
        Dim response As String = Upload(url, body, method)
        Return JsonConvert.DeserializeObject(Of T)(response, JsonSettings)
    End Function

    Public Async Function UploadJsonAsync(Of T)(url As String, body As String, method As String) As Task(Of T)
        Dim response As String = Await UploadAsync(url, body, method)
        Return JsonConvert.DeserializeObject(Of T)(response, JsonSettings)
    End Function

    Public Sub SetHeader(header As String, value As String)
        _webClient.Headers(header) = value
    End Sub

    Public Sub RemoveHeader(header As String)
        If _webClient.Headers(header) IsNot Nothing Then
            _webClient.Headers.Remove(header)
        End If
    End Sub

    Public Function GetHeaders() As List(Of KeyValuePair(Of String, String))
        Return _webClient.Headers.AllKeys.[Select](Function(header) New KeyValuePair(Of String, String)(header, _webClient.Headers(header))).ToList()
    End Function
End Class


