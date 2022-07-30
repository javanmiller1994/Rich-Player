Imports System.Windows.Forms
Public Class ReleaseNotes

  

    Private Sub ReleaseNotes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Label2.Text = "Current Version: " & Application.ProductVersion

        Me.Text = "Release Notes -- " & "Current Version: " & Application.ProductVersion
    End Sub
End Class