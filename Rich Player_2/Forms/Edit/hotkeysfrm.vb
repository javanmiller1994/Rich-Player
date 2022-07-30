Imports System.Drawing

Public Class hotkeysfrm

    Public Function IsOnScreen(ByVal form As Form) As Boolean
        Dim screens() As Screen = Screen.AllScreens
        For Each scrn As Screen In screens
            Dim formRectangle As Rectangle = New Rectangle(form.Left, form.Top, form.Width, form.Height)
            If scrn.WorkingArea.Contains(formRectangle) Then
                Return True
            End If
        Next
        Return False
    End Function
    Private Sub hotkeysfrm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PreVentFlicker()
        Do Until IsOnScreen(Me)
            Dim i As Integer = Me.Top - 1
            Me.Top = i
        Loop
        ''Load Comboxes
        '-------------------------
        'Play / Pause
        If My.Settings.PlayPauseKeyCtrl = 2 Then
            PlayPauseCombo.Text = "Ctrl + " + My.Settings.PlayPauseKey.ToString
        Else
            PlayPauseCombo.Text = My.Settings.PlayPauseKey.ToString
        End If
        'Stop
        If My.Settings.StopKeyCtrl = 2 Then
            StopCombo.Text = "Ctrl + " + My.Settings.StopKey.ToString
        Else
            StopCombo.Text = My.Settings.StopKey.ToString
        End If
        'Previous
        If My.Settings.PreviousKeyCtrl = 2 Then
            PreviousCombo.Text = "Ctrl + " + My.Settings.PreviousKey.ToString
        Else
            PreviousCombo.Text = My.Settings.PreviousKey.ToString
        End If
        'Next
        If My.Settings.NextKeyCtrl = 2 Then
            NextCombo.Text = "Ctrl + " + My.Settings.NextKey.ToString
        Else
            NextCombo.Text = My.Settings.NextKey.ToString
        End If
        'Skip Backwards
        If My.Settings.SkipBackwardsKeyCtrl = 2 Then
            SkipBackwardsCombo.Text = "Ctrl + " + My.Settings.SkipBackwardsKey.ToString
        Else
            SkipBackwardsCombo.Text = My.Settings.SkipBackwardsKey.ToString
        End If
        'Skip Forward
        If My.Settings.SkipForwardKeyCtrl = 2 Then
            SkipForwardCombo.Text = "Ctrl + " + My.Settings.SkipForwardKey.ToString
        Else
            SkipForwardCombo.Text = My.Settings.SkipForwardKey.ToString
        End If



        'AB A Repeat
        If My.Settings.ArKeyCtrl = 2 And Not My.Settings.ArKeyAlt = 1 Then
            ArCombo.Text = "Ctrl + " + My.Settings.Arkey.ToString
        ElseIf My.Settings.ArKeyAlt = 1 Then
            ArCombo.Text = "Ctrl + Alt + " + My.Settings.Arkey.ToString
        Else
            ArCombo.Text = My.Settings.Arkey.ToString
        End If
        'AB B Repeat
        If My.Settings.BrKeyCtrl = 2 And Not My.Settings.BrKeyAlt = 1 Then
            BrCombo.Text = "Ctrl + " + My.Settings.Brkey.ToString
        ElseIf My.Settings.BrKeyAlt = 1 Then
            BrCombo.Text = "Ctrl + Alt + " + My.Settings.Brkey.ToString
        Else
            BrCombo.Text = My.Settings.Brkey.ToString
        End If
        'AB Repeat Reset
        If My.Settings.ABrKeyCtrl = 2 And Not My.Settings.ABrKeyAlt = 1 Then
            ABrCombo.Text = "Ctrl + " + My.Settings.ABrkey.ToString
        ElseIf My.Settings.ABrKeyAlt = 1 Then
            ABrCombo.Text = "Ctrl + Alt + " + My.Settings.ABrkey.ToString
        Else
            ABrCombo.Text = My.Settings.ABrkey.ToString
        End If
        'Speed Up
        If My.Settings.FastKeyCtrl = 2 And Not My.Settings.FastKeyAlt = 1 Then
            FastCombo.Text = "Ctrl + " + My.Settings.FastKey.ToString
        ElseIf My.Settings.FastKeyAlt = 1 Then
            FastCombo.Text = "Ctrl + Alt + " + My.Settings.FastKey.ToString
        Else
            FastCombo.Text = My.Settings.FastKey.ToString
        End If
        'Speed Down
        If My.Settings.SlowKeyCtrl = 2 And Not My.Settings.SlowKeyAlt = 1 Then
            SlowCombo.Text = "Ctrl + " + My.Settings.Slowkey.ToString
        ElseIf My.Settings.SlowKeyAlt = 1 Then
            SlowCombo.Text = "Ctrl + Alt + " + My.Settings.Slowkey.ToString
        Else
            SlowCombo.Text = My.Settings.Slowkey.ToString
        End If
        'Speed Reset
        If My.Settings.SpeedNormCtrl = 2 And Not My.Settings.SpeedNormAlt = 1 Then
            SpeedNormCombo.Text = "Ctrl + " + My.Settings.SpeedNormkey.ToString
        ElseIf My.Settings.SpeedNormAlt = 1 Then
            SpeedNormCombo.Text = "Ctrl + Alt + " + My.Settings.SpeedNormkey.ToString
        Else
            SpeedNormCombo.Text = My.Settings.SpeedNormkey.ToString
        End If


        'Pitch
        ' Down
        If My.Settings.PitchDownKeyCtrl = 2 And Not My.Settings.PitchDownKeyAlt = 1 Then
            PitchDownCombo.Text = "Ctrl + " + My.Settings.PitchDownKey.ToString
        ElseIf My.Settings.PitchDownKeyAlt = 1 Then
            PitchDownCombo.Text = "Ctrl + Alt + " + My.Settings.PitchDownKey.ToString
        Else
            PitchDownCombo.Text = My.Settings.PitchDownKey.ToString
        End If
        ' Up
        If My.Settings.PitchUpKeyCtrl = 2 And Not My.Settings.PitchUpKeyAlt = 1 Then
            PitchUpCombo.Text = "Ctrl + " + My.Settings.PitchUpKey.ToString
        ElseIf My.Settings.PitchUpKeyAlt = 1 Then
            PitchUpCombo.Text = "Ctrl + Alt + " + My.Settings.PitchUpKey.ToString
        Else
            PitchUpCombo.Text = My.Settings.PitchUpKey.ToString
        End If
        ' Reset
        If My.Settings.PitchResetKeyCtrl = 2 And Not My.Settings.PitchResetKeyAlt = 1 Then
            PitchResetCombo.Text = "Ctrl + " + My.Settings.PitchResetKey.ToString
        ElseIf My.Settings.PitchResetKeyAlt = 1 Then
            PitchResetCombo.Text = "Ctrl + Alt + " + My.Settings.PitchResetKey.ToString
        Else
            PitchResetCombo.Text = My.Settings.PitchResetKey.ToString
        End If


        ' Fullscreen
        If My.Settings.KeyFullscreenCtrl = 2 And Not My.Settings.KeyFullscreenAlt = 1 Then
            FullscreenCombo.Text = "Ctrl + " + My.Settings.KeyFullscreen.ToString
        ElseIf My.Settings.KeyFullscreenAlt = 1 Then
            FullscreenCombo.Text = "Ctrl + Alt + " + My.Settings.KeyFullscreen.ToString
        Else
            FullscreenCombo.Text = My.Settings.KeyFullscreen.ToString
        End If

        PictureBox1.Location = New Point(234, 47)
        PictureBox1.Size = New Size(2, 242)


        MyBase.Focus()
    End Sub


    Private Sub PreVentFlicker()
        With Me
            .SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            .SetStyle(ControlStyles.UserPaint, True)
            .SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            .UpdateStyles()
        End With
    End Sub


End Class