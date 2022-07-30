Imports Rich_Player.CsWinFormsBlackApp
Public Class MyMsgBox
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


    Public Function Show(ByVal Message As String, ByVal Title As String, ByVal CenterParent As Boolean) As DialogResult
        Dim SMF As New MyMsgBox

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