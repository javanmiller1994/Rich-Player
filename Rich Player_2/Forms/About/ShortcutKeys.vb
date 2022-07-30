Public Class ShortcutKeys


    Private Sub ShortcutKeys_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        If Not System.Windows.Forms.Application.OpenForms().OfType(Of Tutorial).Any Then

            Dim xform As New Tutorial
            xform.StartPosition = FormStartPosition.Manual
            xform.Location = New System.Drawing.Point(Me.Location.X + Me.Width / 2 - xform.Width / 2, Me.Location.Y + Me.Height / 2 - xform.Height / 2)
            xform.Show()
            xform.BringToFront()
            xform.TopMost = True
        End If

    End Sub

    Private Sub but_Close_Click(sender As Object, e As EventArgs) Handles but_Close.Click
        Me.Close()
    End Sub
End Class