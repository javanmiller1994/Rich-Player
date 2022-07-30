Imports System.Windows.Forms
Imports Rich_Player.CsWinFormsBlackApp
Imports Un4seen.Bass

Public Class OptionsForm1
    Private Sub optionsform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PreVentFlicker()
        For i As Integer = 0 To Bass.BASS_GetDeviceCount() - 1
            Dim device = Bass.BASS_GetDeviceInfo(i)
            If device.IsEnabled Then
                DeviceBox.Properties.Items.Add(String.Format("{0} - {1}", i, device.name))
            End If
        Next
        If My.Settings.DefaultDevice = 0 Then
            DeviceBox.SelectedIndex = 1
        ElseIf My.Settings.DefaultDevice = -1 Then
            DeviceBox.SelectedIndex = 1
        Else

            DeviceBox.SelectedIndex = My.Settings.DefaultDevice
        End If
        Select Case My.Settings.AudioOptimization
            Case 0
                rb_Pitch.Checked = True
            Case 1
                rb_Speed.Checked = True
        End Select


        Me.BackgroundImage = Form1.ChangeOpacity(My.Resources.form_options, 0.5)

        StartupCheckbox.Checked = My.Settings.StartupCheckBoxState
        PlayCheckBox.Checked = My.Settings.PlayOnStart
        CheckEditTouch.Checked = My.Settings.TouchFriendly
        My.Settings.SkinChanged = False
        AutoSaveCheck.Checked = My.Settings.AutoSave

       



    End Sub
    Private Sub OptionsForm1_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Try
            If Web_Update.Retry >= 2 Then
                Label1.Text = "Current Version: " & Application.ProductVersion

            Else
                Web_Update.GetVersionNumber()
                If Web_Update.InternetConnection = False Then
                    Label1.Text = "Current Version: " & Application.ProductVersion & Environment.NewLine & "Web Version: Unknown"
                Else
                    Label1.Text = "Current Version: " & Application.ProductVersion & Environment.NewLine & "Web Version: " & Web_Update.VersionNumber
                End If
                '      Me.Text = Web_Update.VersionNumber
            End If

        Catch ex As Exception
            '  MsgBox(ex.ToString)
            Label1.Text = "Current Version: " & Application.ProductVersion
        End Try
    End Sub
    Private Sub PreVentFlicker()
        With Me
            .SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            .SetStyle(ControlStyles.UserPaint, True)
            .SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            .UpdateStyles()
        End With
    End Sub
    Private Sub optionsform_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        My.Settings.StartupCheckBoxStateCurrent = StartupCheckbox.Checked
        My.Settings.PlayOnStart = PlayCheckBox.Checked
        My.Settings.TouchFriendly = CheckEditTouch.Checked
        My.Settings.AutoSave = AutoSaveCheck.Checked

        If rb_Pitch.Checked Then My.Settings.AudioOptimization = 0
        If rb_Speed.Checked Then My.Settings.AudioOptimization = 1

        My.Settings.Save()
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles but_Update.Click
        Web_Update.Main2()
    End Sub

    Private Sub CheckEditTouch_CheckedChanged(sender As Object, e As EventArgs) Handles CheckEditTouch.CheckedChanged
        My.Settings.SkinChanged = True
    End Sub

    Private Sub but_ReportProblem_Click(sender As Object, e As EventArgs) Handles but_ReportProblem.Click
        Form1.ReportProblem()
    End Sub

  
    Private Sub DeviceBox_Click(sender As Object, e As EventArgs) Handles DeviceBox.Click
        DeviceBox.ShowPopup()
    End Sub

    Private Sub but_ManuallyUpdate_Click(sender As Object, e As EventArgs) Handles but_ManuallyUpdate.Click
        Dim result = MessageBox.Show("Remember to close Rich Player before installing update.", "Close Rich Player", MessageBoxButtons.OKCancel)

        If result = Forms.DialogResult.OK Then
            Process.Start("https://www.dropbox.com/s/b4djfuown4xhxus/Rich%20Player%20-%20Upgrade.exe?dl=1")
        End If

    End Sub

    Private Sub but_VisitWebsite_Click(sender As Object, e As EventArgs) Handles but_VisitWebsite.Click
        Process.Start("https://www.rexfordrich.com/rich-player.html")
    End Sub
End Class