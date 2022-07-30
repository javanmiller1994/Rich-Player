Public Class Hueform 

    Dim imgOriginal As Image = My.Resources.Blank


    Private Sub Hueform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PreVentFlicker()
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

    Private Sub HueTrackBar_EditValueChanged(sender As Object, e As EventArgs) Handles HueTrackBar.EditValueChanged, SatTrackBar.EditValueChanged, LightTrackBar.EditValueChanged

        Dim imgFilter As HSLFilter = New HSLFilter()
        imgFilter.Hue = HueTrackBar.Value
        imgFilter.Saturation = SatTrackBar.Value
        imgFilter.Lightness = LightTrackBar.Value


        'Execute filters
        Dim imgFiltered As System.Drawing.Image = imgFilter.ExecuteFilter(imgOriginal)


        ColorBox.BackgroundImage = imgFiltered
    End Sub

  
End Class