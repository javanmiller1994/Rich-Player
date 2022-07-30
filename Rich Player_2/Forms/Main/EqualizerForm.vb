Imports Rich_Player.CsWinFormsBlackApp
Imports Un4seen.Bass
Imports Rich_Player.AudioController
Imports BassUn4SeenEQualizer.BassEQualizer

Public Class EqualizerForm
    Dim LoadFirst As Boolean = True

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

    Private Sub EqualizerForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PreVentFlicker()
        Do Until IsOnScreen(Me)
            Dim i As Integer = Me.Top - 1
            Me.Top = i
        Loop


        LoadSettings()
        strm = AudioPlayer.Instance.CurrentAudioHandle.CurrentHandle

        LoadFirst = False
        paramEQ = New BASS_DX8_PARAMEQ

        fxEQ = New Integer(9 - 0) {}
        BASS_TableEQ()
    End Sub
    Private Sub PreVentFlicker()
        With Me
            .SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            .SetStyle(ControlStyles.UserPaint, True)
            .SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            .UpdateStyles()
        End With
    End Sub


    ' Close window
    Private Sub EqualizerForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Hide()
    End Sub
    Private Sub but_ok_Click(sender As Object, e As EventArgs) Handles but_ok.Click
        SaveSettings()
        Me.Hide()
    End Sub

    Private fDlg As System.Windows.Forms.OpenFileDialog
    Private sPan As System.TimeSpan
    Friend WithEvents progressTime As System.Windows.Forms.Timer
    Friend WithEvents activeStateTime As System.Windows.Forms.Timer
    'bass.net directX param eq
    Private paramEQ As BASS_DX8_PARAMEQ
    'array number param eq for eq slider volume
    Private fxEQ As Integer()
    Private strm As Integer



    Private Sub tb_80z_Scroll(sender As Object) Handles tb_80z.ValueChanged, tb_120z.ValueChanged, tb_250z.ValueChanged, tb_500z.ValueChanged, tb_1k.ValueChanged, _
        tb_1_8k.ValueChanged, tb_3_5k.ValueChanged, tb_7k.ValueChanged, tb_10k.ValueChanged, tb_14k.ValueChanged
        Select Case sender.Name
            Case tb_80z.Name
                Try
                    ' eq library function
                    BASS_updateEQ(paramEQ, fxEQ, 0, CSng(tb_80z.Value))
                Catch ex As Exception
                End Try

            Case tb_120z.Name
                Try
                    BASS_updateEQ(paramEQ, fxEQ, 1, CSng(tb_120z.Value))
                Catch ex As Exception
                End Try

            Case tb_250z.Name
                Try
                    BASS_updateEQ(paramEQ, fxEQ, 2, CSng(tb_250z.Value))
                Catch ex As Exception
                End Try

            Case tb_500z.Name
                Try
                    BASS_updateEQ(paramEQ, fxEQ, 3, CSng(tb_500z.Value))
                Catch ex As Exception
                End Try

            Case tb_1k.Name
                Try
                    BASS_updateEQ(paramEQ, fxEQ, 4, CSng(tb_1k.Value))
                Catch ex As Exception
                End Try
            Case tb_1_8k.Name
                Try
                    BASS_updateEQ(paramEQ, fxEQ, 5, CSng(tb_1_8k.Value))
                Catch ex As Exception
                End Try

            Case tb_3_5k.Name
                Try
                    BASS_updateEQ(paramEQ, fxEQ, 6, CSng(tb_3_5k.Value))
                Catch ex As Exception
                End Try

            Case tb_7k.Name
                Try
                    BASS_updateEQ(paramEQ, fxEQ, 7, CSng(tb_7k.Value))
                Catch ex As Exception
                End Try

            Case tb_10k.Name
                Try
                    BASS_updateEQ(paramEQ, fxEQ, 8, CSng(tb_10k.Value))
                Catch ex As Exception
                End Try

            Case tb_14k.Name
                Try
                    BASS_updateEQ(paramEQ, fxEQ, 9, CSng(tb_14k.Value))
                Catch ex As Exception
                End Try
        End Select
        ' SaveSettings()
    End Sub

    'created functions eq 10 bands
    Public Sub BASS_TableEQ()
        LoadSettings()
        'eq library function
        strm = AudioPlayer.Instance.CurrentAudioHandle.CurrentHandle
        BASS_ChannelSetFXEQ(strm, paramEQ, fxEQ, 9)
        BASS_FXSetParametersEQ(paramEQ, fxEQ, 80.0!, 0)
        BASS_FXSetParametersEQ(paramEQ, fxEQ, 120.0!, 1)
        BASS_FXSetParametersEQ(paramEQ, fxEQ, 250.0!, 2)
        BASS_FXSetParametersEQ(paramEQ, fxEQ, 500.0!, 3)
        BASS_FXSetParametersEQ(paramEQ, fxEQ, 1000.0!, 4)
        BASS_FXSetParametersEQ(paramEQ, fxEQ, 1800.0!, 5)
        BASS_FXSetParametersEQ(paramEQ, fxEQ, 3500.0!, 6)
        BASS_FXSetParametersEQ(paramEQ, fxEQ, 7000.0!, 7)
        BASS_FXSetParametersEQ(paramEQ, fxEQ, 10000.0!, 8)
        BASS_FXSetParametersEQ(paramEQ, fxEQ, 14000.0!, 9)
        UpdateAll()
    End Sub

    Public Sub UpdateAll()
        BASS_updateEQ(paramEQ, fxEQ, 0, CSng(tb_80z.Value))
        BASS_updateEQ(paramEQ, fxEQ, 1, CSng(tb_120z.Value))
        BASS_updateEQ(paramEQ, fxEQ, 2, CSng(tb_250z.Value))
        BASS_updateEQ(paramEQ, fxEQ, 3, CSng(tb_500z.Value))
        BASS_updateEQ(paramEQ, fxEQ, 4, CSng(tb_1k.Value))
        BASS_updateEQ(paramEQ, fxEQ, 5, CSng(tb_1_8k.Value))
        BASS_updateEQ(paramEQ, fxEQ, 6, CSng(tb_3_5k.Value))
        BASS_updateEQ(paramEQ, fxEQ, 7, CSng(tb_7k.Value))
        BASS_updateEQ(paramEQ, fxEQ, 8, CSng(tb_10k.Value))
        BASS_updateEQ(paramEQ, fxEQ, 9, CSng(tb_14k.Value))
    End Sub
    Public Sub LoadSettings()
        If Not Forms.Application.OpenForms().OfType(Of EqualizerForm).Any Then Return
        tb_80z.Value = My.Settings.EQ0
        tb_120z.Value = My.Settings.EQ1
        tb_250z.Value = My.Settings.EQ2
        tb_500z.Value = My.Settings.EQ3
        tb_1k.Value = My.Settings.EQ4
        tb_1_8k.Value = My.Settings.EQ5
        tb_3_5k.Value = My.Settings.EQ6
        tb_7k.Value = My.Settings.EQ7
        tb_10k.Value = My.Settings.EQ8
        tb_14k.Value = My.Settings.EQ9
    End Sub
    Public Sub SaveSettings()
        My.Settings.EQ0 = tb_80z.Value
        My.Settings.EQ1 = tb_120z.Value
        My.Settings.EQ2 = tb_250z.Value
        My.Settings.EQ3 = tb_500z.Value
        My.Settings.EQ4 = tb_1k.Value
        My.Settings.EQ5 = tb_1_8k.Value
        My.Settings.EQ6 = tb_3_5k.Value
        My.Settings.EQ7 = tb_7k.Value
        My.Settings.EQ8 = tb_10k.Value
        My.Settings.EQ9 = tb_14k.Value
        My.Settings.Save()
    End Sub

    ' Reset
    Private Sub tb_80z_MouseClick(sender As Object, e As MouseEventArgs) Handles tb_80z.MouseClick, tb_120z.MouseClick, tb_250z.MouseClick, tb_500z.MouseClick, tb_1k.MouseClick, _
        tb_1_8k.MouseClick, tb_3_5k.MouseClick, tb_7k.MouseClick, tb_10k.MouseClick, tb_14k.MouseClick
        If e.Button = Forms.MouseButtons.Right Then
            sender.Value = 0
        End If



    End Sub

#Region " Presets"

    Private Sub but_bass_Click(sender As Object, e As EventArgs) Handles but_bass.Click
        tb_80z.Value = 2
        tb_120z.Value = 4
        tb_250z.Value = -4
        tb_500z.Value = -2
        tb_1k.Value = 0
        tb_1_8k.Value = 0
        tb_3_5k.Value = 0
        tb_7k.Value = 0
        tb_10k.Value = 0
        tb_14k.Value = 0
    End Sub

    Private Sub but_deepbass_Click(sender As Object, e As EventArgs) Handles but_deepbass.Click
        tb_80z.Value = 4
        tb_120z.Value = 8
        tb_250z.Value = -8
        tb_500z.Value = -4
        tb_1k.Value = 0
        tb_1_8k.Value = 0
        tb_3_5k.Value = 0
        tb_7k.Value = 0
        tb_10k.Value = 0
        tb_14k.Value = 0
    End Sub

    Private Sub but_rock_Click(sender As Object, e As EventArgs) Handles but_rock.Click
        tb_80z.Value = 3
        tb_120z.Value = 2
        tb_250z.Value = 1
        tb_500z.Value = 1
        tb_1k.Value = -1
        tb_1_8k.Value = -1
        tb_3_5k.Value = 0
        tb_7k.Value = 1
        tb_10k.Value = 2
        tb_14k.Value = 3
    End Sub

    Private Sub but_balanced_Click(sender As Object, e As EventArgs) Handles but_balanced.Click
        tb_80z.Value = 2
        tb_120z.Value = 1
        tb_250z.Value = 1
        tb_500z.Value = 0
        tb_1k.Value = -1
        tb_1_8k.Value = -2
        tb_3_5k.Value = 0
        tb_7k.Value = 1
        tb_10k.Value = 2
        tb_14k.Value = 2
    End Sub

    Private Sub but_boosted_Click(sender As Object, e As EventArgs) Handles but_boosted.Click
        tb_80z.Value = 3
        tb_120z.Value = 3
        tb_250z.Value = 2
        tb_500z.Value = 0
        tb_1k.Value = 1
        tb_1_8k.Value = 1
        tb_3_5k.Value = 2
        tb_7k.Value = 3
        tb_10k.Value = 2
        tb_14k.Value = 1
    End Sub

    Private Sub but_treble_Click(sender As Object, e As EventArgs) Handles but_treble.Click
        tb_80z.Value = -8
        tb_120z.Value = -7
        tb_250z.Value = -6
        tb_500z.Value = -4
        tb_1k.Value = -3
        tb_1_8k.Value = 0
        tb_3_5k.Value = 0
        tb_7k.Value = 0
        tb_10k.Value = 0
        tb_14k.Value = 0
    End Sub

    Private Sub but_flat_Click(sender As Object, e As EventArgs) Handles but_flat.Click
        tb_80z.Value = 0
        tb_120z.Value = 0
        tb_250z.Value = 0
        tb_500z.Value = 0
        tb_1k.Value = 0
        tb_1_8k.Value = 0
        tb_3_5k.Value = 0
        tb_7k.Value = 0
        tb_10k.Value = 0
        tb_14k.Value = 0
    End Sub

    Private Sub but_rb_Click(sender As Object, e As EventArgs) Handles but_rb.Click
        tb_80z.Value = 2
        tb_120z.Value = 5
        tb_250z.Value = 4
        tb_500z.Value = 1
        tb_1k.Value = -2
        tb_1_8k.Value = -1
        tb_3_5k.Value = 1
        tb_7k.Value = 1
        tb_10k.Value = 2
        tb_14k.Value = 3
    End Sub

    Private Sub but_jazz_Click(sender As Object, e As EventArgs) Handles but_jazz.Click
        tb_80z.Value = 3
        tb_120z.Value = 2
        tb_250z.Value = 1
        tb_500z.Value = 2
        tb_1k.Value = -1
        tb_1_8k.Value = -1
        tb_3_5k.Value = 0
        tb_7k.Value = 1
        tb_10k.Value = 2
        tb_14k.Value = 3
    End Sub

    Private Sub but_classical_Click(sender As Object, e As EventArgs) Handles but_classical.Click
        tb_80z.Value = 3
        tb_120z.Value = 2
        tb_250z.Value = 1
        tb_500z.Value = 1
        tb_1k.Value = -1
        tb_1_8k.Value = -1
        tb_3_5k.Value = 0
        tb_7k.Value = 1
        tb_10k.Value = 2
        tb_14k.Value = 3
    End Sub

#End Region

  
 
    


    Private Sub RichLabel2_Click(sender As Object, e As EventArgs) Handles RichLabel2.Click

    End Sub
    Private Sub RichLabel3_Click(sender As Object, e As EventArgs) Handles RichLabel3.Click

    End Sub
    Private Sub RichLabel4_Click(sender As Object, e As EventArgs) Handles RichLabel4.Click

    End Sub
    Private Sub RichLabel5_Click(sender As Object, e As EventArgs) Handles RichLabel5.Click

    End Sub
    Private Sub RichLabel6_Click(sender As Object, e As EventArgs) Handles RichLabel6.Click

    End Sub
    Private Sub RichLabel7_Click(sender As Object, e As EventArgs) Handles RichLabel7.Click

    End Sub
    Private Sub RichLabel8_Click(sender As Object, e As EventArgs) Handles RichLabel8.Click

    End Sub
    Private Sub RichLabel9_Click(sender As Object, e As EventArgs) Handles RichLabel9.Click

    End Sub
    Private Sub RichLabel10_Click(sender As Object, e As EventArgs) Handles RichLabel10.Click

    End Sub
    Private Sub RichLabel11_Click(sender As Object, e As EventArgs) Handles RichLabel11.Click

    End Sub
    Private Sub RichLabel12_Click(sender As Object, e As EventArgs) Handles RichLabel12.Click

    End Sub
    Private Sub RichLabel13_Click(sender As Object, e As EventArgs) Handles RichLabel13.Click

    End Sub
    Private Sub RichLabel14_Click(sender As Object, e As EventArgs) Handles RichLabel14.Click

    End Sub
End Class