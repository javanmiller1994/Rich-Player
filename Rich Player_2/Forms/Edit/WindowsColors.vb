Imports System.Drawing.Drawing2D

Public Class WindowsColors
    Dim imgOriginal As Image = My.Resources.Blank


    Private Sub pb_Border_Paint(sender As Object, e As PaintEventArgs) Handles pb_Border.Paint
        ControlPaint.DrawBorder(e.Graphics, pb_Border.ClientRectangle, ColorDialog1.Color, ButtonBorderStyle.Solid)
    End Sub

    Private Sub WindowsColors_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        PreVentFlicker()
        ColorDialog1.Color = My.Settings.FormBorder
        cb_EnableBorders.Checked = My.Settings.FormBorderUse
        If cb_EnableBorders.Checked = False Then
            but_ChangeColor.Enabled = False
        Else
            but_ChangeColor.Enabled = True
        End If


        HueTrackBar.Value = My.Settings.Hue
        SatTrackBar.Value = My.Settings.Saturation
        LightTrackBar.Value = My.Settings.Lightness

        Dim imgFilter As HSLFilter = New HSLFilter()
        imgFilter.Hue = HueTrackBar.Value
        imgFilter.Saturation = SatTrackBar.Value
        imgFilter.Lightness = LightTrackBar.Value

        'Execute filters
        Dim imgFiltered As System.Drawing.Image = imgFilter.ExecuteFilter(imgOriginal)

        ColorBox.BackgroundImage = imgFiltered

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
            pb_Border.Refresh()

        End If
    End Sub

    Private Sub HueTrackBar_EditValueChanged(sender As Object, e As EventArgs) Handles HueTrackBar.EditValueChanged, SatTrackBar.EditValueChanged, LightTrackBar.EditValueChanged

        Dim imgFilter As HSLFilter = New HSLFilter()
        imgFilter.Hue = HueTrackBar.Value
        imgFilter.Saturation = SatTrackBar.Value
        imgFilter.Lightness = LightTrackBar.Value


        'Execute filters
        Dim imgFiltered As System.Drawing.Image = imgFilter.ExecuteFilter(imgOriginal)


        ColorBox.BackgroundImage = imgFiltered
    End Sub







    Private Sub Ok_But_Click(sender As Object, e As EventArgs) Handles Ok_But.Click
        My.Settings.Hue = HueTrackBar.Value
        My.Settings.Saturation = SatTrackBar.Value
        My.Settings.Lightness = LightTrackBar.Value

        My.Settings.FormBorder = ColorDialog1.Color
        My.Settings.Save()
    End Sub

    Private Sub WindowsColors_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Dim s As Drawing.Size = CsWinFormsBlackApp.Form1.Size
        Dim bm As New Bitmap(s.Width, s.Height)

        CsWinFormsBlackApp.Form1.DrawToBitmap(bm, New Rectangle(0, 0, s.Width, s.Height))
        pb_Border.BackgroundImage = bm.Clone
        bm.Dispose()
    End Sub

 
End Class