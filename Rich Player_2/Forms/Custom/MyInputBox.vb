Public Class MyInputBox 

    Private Sub TextEdit1_KeyUp(sender As Object, e As KeyEventArgs) Handles TextEdit1.KeyUp
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Me.DialogResult = DialogResult.OK

        End If

    End Sub

    
End Class