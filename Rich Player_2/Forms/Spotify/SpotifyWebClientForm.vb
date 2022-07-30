Public Class SpotifyWebClientForm 

    Private Async Sub SpotifyWebClientForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim xform As New SpotifyWebClientUserControl
        Me.Controls.Add(xform)
        xform.Dock = DockStyle.Fill
        Me.BringToFront()
        Me.Activate()


        BringMeToFocus()

    End Sub

    Public Sub BringMeToFocus()

       
        Me.BringToFront()
        Me.Activate()
        Me.Show()
        Me.Focus()
    End Sub

    Private Sub SpotifyWebClientForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

  

 
    Private Sub SpotifyWebClientForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        BringMeToFocus()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        BringMeToFocus()
        Timer1.Stop()
    End Sub
End Class