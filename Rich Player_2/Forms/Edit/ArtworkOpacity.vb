Public Class ArtworkOpacity 


    Private Sub but_35_Click(sender As Object, e As EventArgs) Handles but_75.Click, but_50.Click, but_35.Click
        My.Settings.ArtworkTransparency = sender.tag / 100
        My.Settings.Save()
    End Sub

    Private Sub Textbox_CustomOpacity_EditValueChanged(sender As Object, e As EventArgs) Handles Textbox_CustomOpacity.EditValueChanged
   
    End Sub

    Public Sub ArtworkTransparencyChangeError()
        MyMsgBox.Show("Invalid value! Please use only 2-3 digits", "", True)
        Textbox_CustomOpacity.EditValue = My.Settings.ArtworkTransparency * 100
        DoCancel = True
    End Sub
    Dim DoCancel As Boolean = False
    Private Sub ArtworkOpacity_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If DoCancel Then
            DoCancel = False
            e.Cancel = True

        End If
    End Sub

    Private Sub ArtworkOpacity_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RichLabel1.Location = New Drawing.Point(57, 9)
        RichLabel2.Location = New Drawing.Point(153, 135)
        Textbox_CustomOpacity.Text = My.Settings.ArtworkTransparency * 100
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Try
            If Textbox_CustomOpacity.EditValue.ToString.Count = 2 Then
            ElseIf Textbox_CustomOpacity.EditValue.ToString.Count = 3 Then
                If Textbox_CustomOpacity.EditValue = 100 Then
                Else
                    ArtworkTransparencyChangeError()
                End If
            Else
                ArtworkTransparencyChangeError()
            End If
        Catch
            ArtworkTransparencyChangeError()
        End Try
        If DoCancel Then Return
        My.Settings.ArtworkTransparency = Textbox_CustomOpacity.EditValue / 100
        My.Settings.Save()
    End Sub

  
End Class