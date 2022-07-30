Imports System.Drawing

Imports Rich_Player.CsWinFormsBlackApp
Public Class MyFullMsgBox
    Public PosX As Integer
    Public PosY As Integer


    Private Sub MyMsgBox_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Me.DialogResult = DialogResult.OK
        End If
    End Sub

    Public Sub SizeForm()
        Dim NewHeight As Integer = 0 : Dim NewWidth As Integer = 0

        NewWidth = Message.Width + Message.Left
        NewHeight = Message.Height + but_OK.Height + 20

        Me.SetClientSizeCore(NewWidth, NewHeight)

    End Sub

    Public Sub SetLocation()
        Me.Left = PosX
        Me.Top = PosY
    End Sub
    Public Enum CustomButtons
        YesNo
        OkCancel
        RetryCancel
        Ok
        QuickRegular
    End Enum

    Public Function Show(ByVal Message As String, ByVal Title As String, ByVal CenterParent As Boolean, ByVal CustomButtons As CustomButtons) As DialogResult
        Dim SMF As New MyMsgBox
        Dim f As New Font("Segoe UI", 10)

        SMF.but_OK.Font = f
        SMF.but_Cancel.Font = f


        SMF.but_OK.Location = New Point(12, 40)
        SMF.but_Cancel.Visible = True
        Select Case CustomButtons
            Case MyFullMsgBox.CustomButtons.Ok
                SMF.but_OK.Location = New Point(89, 40)
                SMF.but_OK.Text = "Okay"
                SMF.but_Cancel.Visible = False

            Case MyFullMsgBox.CustomButtons.YesNo
                SMF.but_OK.Text = "Yes"
                SMF.but_Cancel.Text = "No"
                SMF.but_OK.DialogResult = Forms.DialogResult.Yes
                SMF.but_Cancel.DialogResult = Forms.DialogResult.No

            Case MyFullMsgBox.CustomButtons.OkCancel
                SMF.but_OK.Text = "Okay"
                SMF.but_Cancel.Text = "Cancel"
                SMF.but_OK.DialogResult = Forms.DialogResult.OK
                SMF.but_Cancel.DialogResult = Forms.DialogResult.Cancel

            Case MyFullMsgBox.CustomButtons.RetryCancel
                SMF.but_OK.Text = "Retry"
                SMF.but_Cancel.Text = "Cancel"
                SMF.but_OK.DialogResult = Forms.DialogResult.Retry
                SMF.but_Cancel.DialogResult = Forms.DialogResult.Cancel

            Case MyFullMsgBox.CustomButtons.QuickRegular '4
                SMF.but_OK.Text = "Quick Update"
                SMF.but_OK.DialogResult = Forms.DialogResult.Yes
                SMF.but_Cancel.Text = "Regular Update" + Environment.NewLine + "(Recommended)"
                SMF.but_Cancel.DialogResult = Forms.DialogResult.No

        End Select

        SMF.Text = Title
        SMF.Message.Text = Message
        SMF.SizeForm()
     
 


        If CenterParent = True Then
            SMF.StartPosition = FormStartPosition.CenterParent
            SMF.PosX = Me.Left + Me.Width / 2
            SMF.PosY = Me.Top + Me.Height / 2
            SMF.SetLocation()
        End If

        Return SMF.ShowDialog
    End Function


End Class