Imports System.Windows.Forms
Public Class Sliders

    Dim SliderShape As RichTrackBar.SliderShapes
    Dim _SliderFill As RichTrackBar.SliderFilled
    Dim _ShowText As RichTrackBar.ShowText
    Dim _BorderThickness As Integer
    Dim LoadedSliderShape As Integer '= My.Settings.seekbarShape

    Dim _Color_BarLeft As Color '= My.Settings.seekbarBarLeft
    Dim _Color_BarLeftInactive As Color '= My.Settings.seekbarBarLeftInactive
    Dim _Color_BarRight As Color ' = My.Settings.seekbarBarRight
    Dim _Color_Border As Color '= My.Settings.seekbarBorder
    Dim _Color_Slider As Color '= My.Settings.seekbarSlider
    Dim _Color_Text As Color





    Private Sub Sliders_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PreVentFlicker()
        rb_seekbar.Checked = True
        LoadSettings()

    End Sub
    Private Sub PreVentFlicker()
        With Me
            .SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            .SetStyle(ControlStyles.UserPaint, True)
            .SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            .UpdateStyles()
        End With
    End Sub


    Public Sub LoadSettings()
        Select Case True
            Case rb_seekbar.Checked
                LoadedSliderShape = CInt(My.Settings.seekbarShape)
                _Color_BarLeft = My.Settings.seekbarBarLeft
                _Color_BarLeftInactive = My.Settings.seekbarBarLeftInactive
                _Color_BarRight = My.Settings.seekbarBarRight
                _Color_Border = My.Settings.seekbarBorder
                _Color_Slider = My.Settings.seekbarSlider
                _Color_Text = My.Settings.seekbarTextColor
                If My.Settings.SlidersFilledSeek Then
                    rb_FilledYes.Checked = True
                    _SliderFill = RichTrackBar.SliderFilled.Yes
                Else
                    rb_FilledNo.Checked = True
                    _SliderFill = RichTrackBar.SliderFilled.No
                End If
                If My.Settings.seekbarText Then
                    rb_TextYes.Checked = True
                    _ShowText = RichTrackBar.ShowText.Yes
                Else
                    rb_TextNo.Checked = True
                    _ShowText = RichTrackBar.ShowText.No
                End If
                _BorderThickness = My.Settings.BorderThicknessSeek

            Case rb_volumebar.Checked
                LoadedSliderShape = CInt(My.Settings.volumebarShape)
                _Color_BarLeft = My.Settings.volumebarBarLeft
                _Color_BarLeftInactive = My.Settings.volumebarBarLeftInactive
                _Color_BarRight = My.Settings.volumebarBarRight
                _Color_Border = My.Settings.volumebarBorder
                _Color_Slider = My.Settings.volumebarSlider
                _Color_Text = My.Settings.volumebarTextColor
                If My.Settings.SlidersFilledVolume Then
                    rb_FilledYes.Checked = True
                    _SliderFill = RichTrackBar.SliderFilled.Yes
                Else
                    rb_FilledNo.Checked = True
                    _SliderFill = RichTrackBar.SliderFilled.No
                End If
                If My.Settings.volumebarText Then
                    rb_TextYes.Checked = True
                    _ShowText = RichTrackBar.ShowText.Yes
                Else
                    rb_TextNo.Checked = True
                    _ShowText = RichTrackBar.ShowText.No
                End If
                _BorderThickness = My.Settings.BorderThicknessVolume

            Case rb_speedbar.Checked
                LoadedSliderShape = CInt(My.Settings.speedbarShape)
                _Color_BarLeft = My.Settings.speedbarBarLeft
                _Color_BarLeftInactive = My.Settings.speedbarBarLeftInactive
                _Color_BarRight = My.Settings.speedbarBarRight
                _Color_Border = My.Settings.speedbarBorder
                _Color_Slider = My.Settings.speedbarSlider
                _Color_Text = My.Settings.speedbarTextColor
                If My.Settings.SlidersFilledSpeed Then
                    rb_FilledYes.Checked = True
                    _SliderFill = RichTrackBar.SliderFilled.Yes
                Else
                    rb_FilledNo.Checked = True
                    _SliderFill = RichTrackBar.SliderFilled.No
                End If
                If My.Settings.speedbarText Then
                    rb_TextYes.Checked = True
                    _ShowText = RichTrackBar.ShowText.Yes
                Else
                    rb_TextNo.Checked = True
                    _ShowText = RichTrackBar.ShowText.No
                End If
                _BorderThickness = My.Settings.BorderThicknessSpeed

            Case rb_pitchbar.Checked
                LoadedSliderShape = CInt(My.Settings.pitchbarShape)
                _Color_BarLeft = My.Settings.pitchbarBarLeft
                _Color_BarLeftInactive = My.Settings.pitchbarBarLeftInactive
                _Color_BarRight = My.Settings.pitchbarBarRight
                _Color_Border = My.Settings.pitchbarBorder
                _Color_Slider = My.Settings.pitchbarSlider
                _Color_Text = My.Settings.pitchbarTextColor
                If My.Settings.SlidersFilledPitch Then
                    rb_FilledYes.Checked = True
                    _SliderFill = RichTrackBar.SliderFilled.Yes
                Else
                    rb_FilledNo.Checked = True
                    _SliderFill = RichTrackBar.SliderFilled.No
                End If
                If My.Settings.pitchbarText Then
                    rb_TextYes.Checked = True
                    _ShowText = RichTrackBar.ShowText.Yes
                Else
                    rb_TextNo.Checked = True
                    _ShowText = RichTrackBar.ShowText.No
                End If
                _BorderThickness = My.Settings.BorderThicknessPitch

        End Select

        Select Case LoadedSliderShape
            Case 0
                SliderShape = RichTrackBar.SliderShapes.Circle
                rb_Circle.Checked = True
            Case 1
                SliderShape = RichTrackBar.SliderShapes.RectangleNarrow
                rb_RecNarrow.Checked = True
            Case 2
                SliderShape = RichTrackBar.SliderShapes.RectangleWide
                rb_RecWide.Checked = True
            Case 3
                SliderShape = RichTrackBar.SliderShapes.Square
                rb_Square.Checked = True
            Case 4
                SliderShape = RichTrackBar.SliderShapes.Pentagon
                rb_Pentagon.Checked = True
            Case 5
                SliderShape = RichTrackBar.SliderShapes.RoundedRectangle
                rb_Rounded.Checked = True

        End Select


        mtb_BorderThickness.Text = _BorderThickness


        pb_BarLeft.BackColor = _Color_BarLeft
        pb_BarLeftIA.BackColor = _Color_BarLeftInactive
        pb_BarRight.BackColor = _Color_BarRight
        pb_Border.BackColor = _Color_Border
        pb_Slider.BackColor = _Color_Slider
        pb_TextColor.BackColor = _Color_Text

        TrackBar_Preview.BorderThickness = _BorderThickness
        TrackBar_Preview.SliderFill = _SliderFill
        TrackBar_Preview.SliderShape = SliderShape
        TrackBar_Preview.Color_BarLeft = _Color_BarLeft
        TrackBar_Preview.Color_BarLeftInactive = _Color_BarLeftInactive
        TrackBar_Preview.Color_BarRight = _Color_BarRight
        TrackBar_Preview.Color_Border = _Color_Border
        TrackBar_Preview.Color_Slider = _Color_Slider
        TrackBar_Preview.Color_Text = _Color_Text
        TrackBar_Preview.TextShown = _ShowText

    End Sub

    Public Sub SaveSettings()
        Select Case True
            Case rb_seekbar.Checked
                My.Settings.seekbarShape = CInt(SliderShape)
                My.Settings.seekbarBarLeft = _Color_BarLeft
                My.Settings.seekbarBarLeftInactive = _Color_BarLeftInactive
                My.Settings.seekbarBarRight = _Color_BarRight
                My.Settings.seekbarBorder = _Color_Border
                My.Settings.seekbarSlider = _Color_Slider
                My.Settings.SlidersFilledSeek = rb_FilledYes.Checked
                My.Settings.BorderThicknessSeek = _BorderThickness
                My.Settings.seekbarText = rb_TextYes.Checked
                My.Settings.seekbarTextColor = _Color_Text

            Case rb_volumebar.Checked
                My.Settings.volumebarShape = CInt(SliderShape)
                My.Settings.volumebarBarLeft = _Color_BarLeft
                My.Settings.volumebarBarLeftInactive = _Color_BarLeftInactive
                My.Settings.volumebarBarRight = _Color_BarRight
                My.Settings.volumebarBorder = _Color_Border
                My.Settings.volumebarSlider = _Color_Slider
                My.Settings.SlidersFilledVolume = rb_FilledYes.Checked
                My.Settings.BorderThicknessVolume = _BorderThickness
                My.Settings.volumebarText = rb_TextYes.Checked
                My.Settings.volumebarTextColor = _Color_Text

            Case rb_speedbar.Checked
                My.Settings.speedbarShape = CInt(SliderShape)
                My.Settings.speedbarBarLeft = _Color_BarLeft
                My.Settings.speedbarBarLeftInactive = _Color_BarLeftInactive
                My.Settings.speedbarBarRight = _Color_BarRight
                My.Settings.speedbarBorder = _Color_Border
                My.Settings.speedbarSlider = _Color_Slider
                My.Settings.SlidersFilledSpeed = rb_FilledYes.Checked
                My.Settings.BorderThicknessSpeed = _BorderThickness
                My.Settings.speedbarText = rb_TextYes.Checked
                My.Settings.speedbarTextColor = _Color_Text

            Case rb_pitchbar.Checked
                My.Settings.pitchbarShape = CInt(SliderShape)
                My.Settings.pitchbarBarLeft = _Color_BarLeft
                My.Settings.pitchbarBarLeftInactive = _Color_BarLeftInactive
                My.Settings.pitchbarBarRight = _Color_BarRight
                My.Settings.pitchbarBorder = _Color_Border
                My.Settings.pitchbarSlider = _Color_Slider
                My.Settings.SlidersFilledPitch = rb_FilledYes.Checked
                My.Settings.BorderThicknessPitch = _BorderThickness
                My.Settings.pitchbarText = rb_TextYes.Checked
                My.Settings.pitchbarTextColor = _Color_Text

        End Select


        My.Settings.Save()
    End Sub

    Public Sub DefaultSettings()
        Select Case True
            Case rb_seekbar.Checked
                My.Settings.seekbarShape = 0
                My.Settings.seekbarBarLeft = Color.FromArgb(23, 119, 151)
                My.Settings.seekbarBarLeftInactive = Color.Gray
                My.Settings.seekbarBarRight = Color.FromArgb(60, 60, 60)
                My.Settings.seekbarBorder = Color.FromArgb(35, 35, 35)
                My.Settings.seekbarSlider = Color.Silver
                My.Settings.SlidersFilledSeek = True
                My.Settings.BorderThicknessSeek = 1
                My.Settings.seekbarText = False
                My.Settings.seekbarTextColor = Color.Silver

            Case rb_volumebar.Checked
                My.Settings.volumebarShape = 4
                My.Settings.volumebarBarLeft = Color.FromArgb(23, 119, 151)
                My.Settings.volumebarBarLeftInactive = Color.Gray
                My.Settings.volumebarBarRight = Color.FromArgb(60, 60, 60)
                My.Settings.volumebarBorder = Color.FromArgb(35, 35, 35)
                My.Settings.volumebarSlider = Color.Silver
                My.Settings.SlidersFilledVolume = True
                My.Settings.BorderThicknessVolume = 1
                My.Settings.volumebarText = False
                My.Settings.volumebarTextColor = Color.Silver

            Case rb_speedbar.Checked
                My.Settings.speedbarShape = 4
                My.Settings.speedbarBarLeft = Color.FromArgb(23, 119, 151)
                My.Settings.speedbarBarLeftInactive = Color.Gray
                My.Settings.speedbarBarRight = Color.FromArgb(60, 60, 60)
                My.Settings.speedbarBorder = Color.FromArgb(35, 35, 35)
                My.Settings.speedbarSlider = Color.Silver
                My.Settings.SlidersFilledSpeed = True
                My.Settings.BorderThicknessSpeed = 1
                My.Settings.speedbarText = False
                My.Settings.speedbarTextColor = Color.Silver

            Case rb_pitchbar.Checked
                My.Settings.pitchbarShape = 4
                My.Settings.pitchbarBarLeft = Color.FromArgb(23, 119, 151)
                My.Settings.pitchbarBarLeftInactive = Color.Gray
                My.Settings.pitchbarBarRight = Color.FromArgb(60, 60, 60)
                My.Settings.pitchbarBorder = Color.FromArgb(35, 35, 35)
                My.Settings.pitchbarSlider = Color.Silver
                My.Settings.SlidersFilledPitch = True
                My.Settings.BorderThicknessPitch = 1
                My.Settings.pitchbarText = False
                My.Settings.pitchbarTextColor = Color.Silver

        End Select


        My.Settings.Save()
    End Sub

    Private Sub but_OK_Click(sender As Object, e As EventArgs) Handles but_OK.Click, but_Apply.Click
        SaveSettings()

        Return
        My.Settings.seekbarShape = CInt(SliderShape)

        My.Settings.seekbarBarLeft = _Color_BarLeft
        My.Settings.seekbarBarLeftInactive = _Color_BarLeftInactive
        My.Settings.seekbarBarRight = _Color_BarRight
        My.Settings.seekbarBorder = _Color_Border
        My.Settings.seekbarSlider = _Color_Slider


        My.Settings.Save()
    End Sub

    Private Sub rb_Circle_CheckedChanged(sender As Object, e As EventArgs) Handles rb_Circle.CheckedChanged, rb_Pentagon.CheckedChanged, rb_RecNarrow.CheckedChanged, rb_RecWide.CheckedChanged, rb_Square.CheckedChanged, rb_Rounded.CheckedChanged
        Select Case sender.tag
            Case 1
                SliderShape = RichTrackBar.SliderShapes.Circle
            Case 2
                SliderShape = RichTrackBar.SliderShapes.RectangleNarrow
            Case 3
                SliderShape = RichTrackBar.SliderShapes.RectangleWide
            Case 4
                SliderShape = RichTrackBar.SliderShapes.Square
            Case 5
                SliderShape = RichTrackBar.SliderShapes.Pentagon
            Case 6
                SliderShape = RichTrackBar.SliderShapes.RoundedRectangle
        End Select

        TrackBar_Preview.SliderShape = SliderShape

    End Sub
    Private Sub rb_FilledYes_CheckedChanged(sender As Object, e As EventArgs) Handles rb_FilledYes.CheckedChanged, rb_FilledNo.CheckedChanged
        Select Case sender.tag
            Case 1
                _SliderFill = RichTrackBar.SliderFilled.Yes
            Case 2
                _SliderFill = RichTrackBar.SliderFilled.No
        End Select
        TrackBar_Preview.SliderFill = _SliderFill

    End Sub

    Private Sub rb_TextYes_CheckedChanged(sender As Object, e As EventArgs) Handles rb_TextYes.CheckedChanged, rb_TextNo.CheckedChanged
        Select Case sender.tag
            Case 1
                _ShowText = RichTrackBar.ShowText.Yes
            Case 2
                _ShowText = RichTrackBar.ShowText.No
        End Select
        TrackBar_Preview.TextShown = _ShowText

    End Sub


    Private Sub pb_BarLeft_Click(sender As Object, e As EventArgs) Handles pb_BarLeft.Click, pb_BarLeftIA.Click, pb_BarRight.Click, pb_Border.Click, pb_Slider.Click, pb_TextColor.Click
        Select Case sender.tag
            Case 1
                ColorPicker.Color = _Color_BarLeft
            Case 2
                ColorPicker.Color = _Color_BarLeftInactive
            Case 3
                ColorPicker.Color = _Color_BarRight
            Case 4
                ColorPicker.Color = _Color_Border
            Case 5
                ColorPicker.Color = _Color_Slider
            Case 6
                ColorPicker.Color = _Color_Text
        End Select

        If ColorPicker.ShowDialog = Forms.DialogResult.OK Then
            Select Case sender.tag
                Case 1
                    _Color_BarLeft = ColorPicker.Color
                Case 2
                    _Color_BarLeftInactive = ColorPicker.Color
                Case 3
                    _Color_BarRight = ColorPicker.Color
                Case 4
                    _Color_Border = ColorPicker.Color
                Case 5
                    _Color_Slider = ColorPicker.Color
                Case 6
                    _Color_Text = ColorPicker.Color
            End Select

            pb_BarLeft.BackColor = _Color_BarLeft
            pb_BarLeftIA.BackColor = _Color_BarLeftInactive
            pb_BarRight.BackColor = _Color_BarRight
            pb_Border.BackColor = _Color_Border
            pb_Slider.BackColor = _Color_Slider
            pb_TextColor.BackColor = _Color_Text
            TrackBar_Preview.SliderFill = _SliderFill
            TrackBar_Preview.Color_BarLeft = _Color_BarLeft
            TrackBar_Preview.Color_BarLeftInactive = _Color_BarLeftInactive
            TrackBar_Preview.Color_BarRight = _Color_BarRight
            TrackBar_Preview.Color_Border = _Color_Border
            TrackBar_Preview.Color_Slider = _Color_Slider
            TrackBar_Preview.Color_Text = _Color_Text


        End If
    End Sub

    Public Sub Trackbar1_MouseEnter(sender As Object, e As EventArgs) Handles TrackBar_Preview.MouseEnter
        TrackBar_Preview.IsMouseEnter = True
    End Sub
    Public Sub Trackbar1_MouseLeave(sender As Object, e As EventArgs) Handles TrackBar_Preview.MouseLeave
        TrackBar_Preview.IsMouseEnter = False
    End Sub


    Private Sub but_Default_Click(sender As Object, e As EventArgs) Handles but_Default.Click
        If MessageBox.Show("Are you sure you want to reset the currently selected TrackBar to its default appearence?" + Environment.NewLine + "This cannot be undone.", "Are You Sure?", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            DefaultSettings()
            LoadSettings()



            Return
            My.Settings.seekbarShape = 0

            My.Settings.seekbarBarLeft = Color.FromArgb(23, 119, 151)
            My.Settings.seekbarBarLeftInactive = Color.Gray
            My.Settings.seekbarBarRight = Color.FromArgb(60, 60, 60)
            My.Settings.seekbarBorder = Color.FromArgb(35, 35, 35)
            My.Settings.seekbarSlider = Color.Silver
            My.Settings.Save()

            _Color_BarLeft = My.Settings.seekbarBarLeft
            _Color_BarLeftInactive = My.Settings.seekbarBarLeftInactive
            _Color_BarRight = My.Settings.seekbarBarRight
            _Color_Border = My.Settings.seekbarBorder
            _Color_Slider = My.Settings.seekbarSlider


            LoadedSliderShape = My.Settings.seekbarShape
            LoadSettings()
        End If

    End Sub

    Private Sub but_Cancel_Click(sender As Object, e As EventArgs) Handles but_Cancel.Click

    End Sub

    Private Sub rb_pitchbar_CheckedChanged(sender As Object, e As EventArgs) Handles rb_volumebar.CheckedChanged, rb_speedbar.CheckedChanged, rb_seekbar.CheckedChanged, rb_pitchbar.CheckedChanged
        LoadSettings()
    End Sub

    Private Sub but_DefaultAll_Click(sender As Object, e As EventArgs) Handles but_DefaultAll.Click
        If MessageBox.Show("Are you sure you want to reset all TrackBars to their default appearences?" + Environment.NewLine + "This cannot be undone.", "Are You Sure?", MessageBoxButtons.YesNo) = DialogResult.Yes Then


            My.Settings.seekbarShape = 0
            My.Settings.seekbarBarLeft = Color.FromArgb(23, 119, 151)
            My.Settings.seekbarBarLeftInactive = Color.Gray
            My.Settings.seekbarBarRight = Color.FromArgb(60, 60, 60)
            My.Settings.seekbarBorder = Color.FromArgb(35, 35, 35)
            My.Settings.seekbarSlider = Color.Silver

            My.Settings.volumebarShape = 4
            My.Settings.volumebarBarLeft = Color.FromArgb(23, 119, 151)
            My.Settings.volumebarBarLeftInactive = Color.Gray
            My.Settings.volumebarBarRight = Color.FromArgb(60, 60, 60)
            My.Settings.volumebarBorder = Color.FromArgb(35, 35, 35)
            My.Settings.volumebarSlider = Color.Silver

            My.Settings.speedbarShape = 4
            My.Settings.speedbarBarLeft = Color.FromArgb(23, 119, 151)
            My.Settings.speedbarBarLeftInactive = Color.Gray
            My.Settings.speedbarBarRight = Color.FromArgb(60, 60, 60)
            My.Settings.speedbarBorder = Color.FromArgb(35, 35, 35)
            My.Settings.speedbarSlider = Color.Silver

            My.Settings.pitchbarShape = 4
            My.Settings.pitchbarBarLeft = Color.FromArgb(23, 119, 151)
            My.Settings.pitchbarBarLeftInactive = Color.Gray
            My.Settings.pitchbarBarRight = Color.FromArgb(60, 60, 60)
            My.Settings.pitchbarBorder = Color.FromArgb(35, 35, 35)
            My.Settings.pitchbarSlider = Color.Silver

            My.Settings.SlidersFilledSeek = True
            My.Settings.SlidersFilledVolume = True
            My.Settings.SlidersFilledSpeed = True
            My.Settings.SlidersFilledPitch = True

            My.Settings.BorderThicknessSeek = 1
            My.Settings.BorderThicknessVolume = 1
            My.Settings.BorderThicknessSpeed = 1
            My.Settings.BorderThicknessPitch = 1

            My.Settings.seekbarText = False
            My.Settings.seekbarTextColor = Color.Silver
            My.Settings.volumebarText = False
            My.Settings.volumebarTextColor = Color.Silver
            My.Settings.speedbarText = False
            My.Settings.speedbarTextColor = Color.Silver
            My.Settings.pitchbarText = False
            My.Settings.pitchbarTextColor = Color.Silver

            LoadSettings()
        End If
    End Sub


    Private Sub mtb_BorderThickness_TextChanged(sender As Object, e As EventArgs) Handles mtb_BorderThickness.TextChanged
        Try
            If mtb_BorderThickness.Text = "" Then Return
            If mtb_BorderThickness.Text = 0 Then Return
            If mtb_BorderThickness.Text = Nothing Then Return

            _BorderThickness = CInt(mtb_BorderThickness.Text)
            TrackBar_Preview.BorderThickness = _BorderThickness
        Catch ex As Exception
        End Try
    End Sub


   
    Private Sub TrackBar_Preview_ValueChanged(sender As Object) Handles TrackBar_Preview.ValueChanged
        TrackBar_Preview.Text = TrackBar_Preview.Value
    End Sub

   
End Class