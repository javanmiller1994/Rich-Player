Public Class BorderColorFrm 

    Private Sub BorderColorFrm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PreVentFlicker()
        ColorDialog1.Color = My.Settings.FormBorder
        cb_EnableBorders.Checked = My.Settings.FormBorderUse
        If cb_EnableBorders.Checked = False Then
            but_ChangeColor.Enabled = False
        Else
            but_ChangeColor.Enabled = True
        End If
    End Sub
    Private Sub PreVentFlicker()
        With Me
            .SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            .SetStyle(ControlStyles.UserPaint, True)
            .SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            .UpdateStyles()
        End With
    End Sub

    Private Sub cb_EnableBorders_CheckedChanged(sender As Object, e As EventArgs) Handles cb_EnableBorders.CheckedChanged
        Select Case cb_EnableBorders.Checked
            Case True
                My.Settings.FormBorderUse = True
                but_ChangeColor.Enabled = True
            Case False
                My.Settings.FormBorderUse = False
                but_ChangeColor.Enabled = False
        End Select
        My.Settings.Save()
    End Sub

    Private Sub but_ChangeColor_Click(sender As Object, e As EventArgs) Handles but_ChangeColor.Click
        If ColorDialog1.ShowDialog = Forms.DialogResult.OK Then
            My.Settings.FormBorder = ColorDialog1.Color
            My.Settings.Save()
        End If
    End Sub





    Private Sub BorderColorFrm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        My.Settings.Save()
    End Sub


 
End Class