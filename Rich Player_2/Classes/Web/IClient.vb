
Imports Newtonsoft.Json
Imports System.Collections.Generic
Imports System.Threading.Tasks


Public Interface IClient
    Inherits IDisposable
    Property JsonSettings() As JsonSerializerSettings

    ''' <summary>
    '''     Downloads data from an URL and returns it
    ''' </summary>
    ''' <param name="url">An URL</param>
    ''' <returns></returns>
    Function Download(url As String) As String

    ''' <summary>
    '''     Downloads data async from an URL and returns it
    ''' </summary>
    ''' <param name="url"></param>
    ''' <returns></returns>
    Function DownloadAsync(url As String) As Task(Of String)

    ''' <summary>
    '''     Downloads data from an URL and returns it
    ''' </summary>
    ''' <param name="url">An URL</param>
    ''' <returns></returns>
    Function DownloadRaw(url As String) As Byte()

    ''' <summary>
    '''     Downloads data async from an URL and returns it
    ''' </summary>
    ''' <param name="url"></param>
    ''' <returns></returns>
    Function DownloadRawAsync(url As String) As Task(Of Byte())

    ''' <summary>
    '''     Downloads data from an URL and converts it to an object
    ''' </summary>
    ''' <typeparam name="T">The Type which the object gets converted to</typeparam>
    ''' <param name="url">An URL</param>
    ''' <returns></returns>
    Function DownloadJson(Of T)(url As String) As T

    ''' <summary>
    '''     Downloads data async from an URL and converts it to an object
    ''' </summary>
    ''' <typeparam name="T">The Type which the object gets converted to</typeparam>
    ''' <param name="url">An URL</param>
    ''' <returns></returns>
    Function DownloadJsonAsync(Of T)(url As String) As Task(Of T)

    ''' <summary>
    '''     Uploads data from an URL and returns the response
    ''' </summary>
    ''' <param name="url">An URL</param>
    ''' <param name="body">The Body-Data (most likely a JSON String)</param>
    ''' <param name="method">The Upload-method (POST,DELETE,PUT)</param>
    ''' <returns></returns>
    Function Upload(url As String, body As String, method As String) As String

    ''' <summary>
    '''     Uploads data async from an URL and returns the response
    ''' </summary>
    ''' <param name="url">An URL</param>
    ''' <param name="body">The Body-Data (most likely a JSON String)</param>
    ''' <param name="method">The Upload-method (POST,DELETE,PUT)</param>
    ''' <returns></returns>
    Function UploadAsync(url As String, body As String, method As String) As Task(Of String)

    ''' <summary>
    '''     Uploads data from an URL and returns the response
    ''' </summary>
    ''' <param name="url">An URL</param>
    ''' <param name="body">The Body-Data (most likely a JSON String)</param>
    ''' <param name="method">The Upload-method (POST,DELETE,PUT)</param>
    ''' <returns></returns>
    Function UploadRaw(url As String, body As String, method As String) As Byte()

    ''' <summary>
    '''     Uploads data async from an URL and returns the response
    ''' </summary>
    ''' <param name="url">An URL</param>
    ''' <param name="body">The Body-Data (most likely a JSON String)</param>
    ''' <param name="method">The Upload-method (POST,DELETE,PUT)</param>
    ''' <returns></returns>
    Function UploadRawAsync(url As String, body As String, method As String) As Task(Of Byte())

    ''' <summary>
    '''     Uploads data from an URL and converts the response to an object
    ''' </summary>
    ''' <typeparam name="T">The Type which the object gets converted to</typeparam>
    ''' <param name="url">An URL</param>
    ''' <param name="body">The Body-Data (most likely a JSON String)</param>
    ''' <param name="method">The Upload-method (POST,DELETE,PUT)</param>
    ''' <returns></returns>
    Function UploadJson(Of T)(url As String, body As String, method As String) As T

    ''' <summary>
    '''     Uploads data async from an URL and converts the response to an object
    ''' </summary>
    ''' <typeparam name="T">The Type which the object gets converted to</typeparam>
    ''' <param name="url">An URL</param>
    ''' <param name="body">The Body-Data (most likely a JSON String)</param>
    ''' <param name="method">The Upload-method (POST,DELETE,PUT)</param>
    ''' <returns></returns>
    Function UploadJsonAsync(Of T)(url As String, body As String, method As String) As Task(Of T)

    ''' <summary>
    '''     Sets a specific Header
    ''' </summary>
    ''' <param name="header">Header name</param>
    ''' <param name="value">Header value</param>
    Sub SetHeader(header As String, value As String)

    ''' <summary>
    '''     Removes a specific Header
    ''' </summary>
    ''' <param name="header">Header name</param>
    Sub RemoveHeader(header As String)

    ''' <summary>
    '''     Gets all current Headers
    ''' </summary>
    ''' <returns>A collection of Header KeyValue Pairs</returns>
    Function GetHeaders() As List(Of KeyValuePair(Of String, String))
End Interface


'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
