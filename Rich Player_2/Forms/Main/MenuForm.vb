Imports System.Runtime.InteropServices
Imports Rich_Player.CsWinFormsBlackApp
Imports WindowsInput.KeyboardSimulator
Imports WindowsInput.InputSimulator
Imports WindowsInput
Imports WindowsInput.Native
Imports Transitions

Public Class MenuForm
#Region " Animate Window"

    Dim animationSpeed As Integer = 600
    Sub AnimateObject(ByVal ObjectToAnimate As Object, ByVal showObject As Boolean, ByVal IsControl As Boolean, ByVal AnimateUpwards As Boolean)
        If showObject Then
            If IsControl Then
                If AnimateUpwards Then
                    ObjectToAnimate.Top = Me.Height
                    Transition.run(ObjectToAnimate, "Top", 78, (New TransitionType_EaseInEaseOut(animationSpeed)))
                Else
                    ObjectToAnimate.Left = Me.Width
                    Transition.run(ObjectToAnimate, "Left", 17, (New TransitionType_EaseInEaseOut(animationSpeed)))
                End If
            Else
                Me.Width = 0
                Transition.run(ObjectToAnimate, "Width", 208, (New TransitionType_EaseInEaseOut(animationSpeed)))
            End If
        Else
            If IsControl Then
                If AnimateUpwards Then
                    Transition.run(ObjectToAnimate, "Top", Me.Height, (New TransitionType_EaseInEaseOut(animationSpeed)))
                Else
                    Transition.run(ObjectToAnimate, "Left", Me.Width, (New TransitionType_EaseInEaseOut(animationSpeed)))
                End If
            Else
                Dim t As New Transition(New TransitionType_EaseInEaseOut(animationSpeed)) : AddHandler t.TransitionCompletedEvent, AddressOf TransitionCompleted
                t.add(ObjectToAnimate, "Width", 0) : t.run()

            End If
        End If
    End Sub
    Dim MenuCommand As String = ""
    Public Sub TransitionCompleted()
        Me.Close()
        IsFormClosing = True
        Select Case MenuCommand
            Case "label_QuickOpen"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.VK_Q)
            Case "label_Audio"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.F15)
            Case "label_Subtitles"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.F14)

            Case Else
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.F13)
        End Select

    End Sub

#End Region

#Region " Menu Load |   Activated"

    Private Sub PreVentFlicker()
        With Me
            .SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            .SetStyle(ControlStyles.UserPaint, True)
            .SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            .UpdateStyles()
        End With
    End Sub
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

    Private Sub MenuForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PreVentFlicker()
        Me.Width = 0
       
        Me.Left = Form1.FormLeft + 1
        Me.Top = Form1.FormTop + 1

        Set_EnableEditingState()
        Set_MiniModeState()
        Set_SimplisticModeState()
        Set_PlaylistSearchState()
        Set_UseShadowsState()
        Set_VolumeControlState()
        Set_LyricsState()
        Set_MediaKeysState()
        Set_PlayFavoritesState()

        spin_RowHeight.EditValue = My.Settings.PlaylistRowHeight
    End Sub
    Private Sub MenuForm_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        If Not IsOnScreen(Me) Then
            Me.Top = Screen.FromControl(Me).WorkingArea.Bottom - Me.Height
        End If
        AnimateObject(Me, True, False, False)
        Me.TopMost = True
        '  Me.Focus()
    End Sub

#End Region

#Region " Menu Close    |   Deactivated"
    Dim IsFormClosing As Boolean = False
    Public Sub PerfromClose()
        If IsFormClosing Then Return
        Try
            AnimateObject(Me, False, False, False)
            Form1.PopupOpen = False
            IsFormClosing = True
        Catch ex As Exception : End Try


        Form1.UnRegisterHamburgerMenu()
    End Sub
    Private Sub MenuForm_Deactivate(sender As Object, e As EventArgs) Handles MyBase.Deactivate
        If IsFormClosing Then Return
        PerfromClose()
    End Sub

    Private Sub MenuForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        IsFormClosing = True
    End Sub
    Private Sub MenuForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        SendClick({Alt, Shift}, VirtualKeyCode.VK_U)
        Me.TopMost = False
        Me.Dispose()
    End Sub


#End Region


#Region " Menu Clicks"

    Dim CurMenu As String
    Private Sub label_Songs_Click(sender As Object, e As EventArgs) Handles pb_Songs.Click, label_Songs.Click
        CurMenu = "Songs"
        'animateWin(panel_OpenedSongs.Handle, True, True)
        AnimateObject(panel_OpenedSongs, True, True, False)
        panel_OpenedSongs.BringToFront()
        ' panel_OpenedSongs.Visible = True
    End Sub
    Private Sub label_Playlists_Click(sender As Object, e As EventArgs) Handles pb_Playlists.Click, label_Playlists.Click
        CurMenu = "Playlists"
        '  animateWin(panel_OpenedPlaylists.Handle, True, True)
        AnimateObject(panel_OpenedPlaylists, True, True, False)

        panel_OpenedPlaylists.BringToFront()
        '  panel_OpenedPlaylists.Visible = True
    End Sub
    Private Sub label_Media_Click(sender As Object, e As EventArgs) Handles pb_Media.Click, label_Media.Click
        CurMenu = "Media"
        ' animateWin(panel_OpenedMedia.Handle, True, True)
        AnimateObject(panel_OpenedMedia, True, True, False)
        panel_OpenedMedia.BringToFront()
        ' panel_OpenedMedia.Visible = True
    End Sub
    Private Sub label_Edit_Click(sender As Object, e As EventArgs) Handles pb_Edit.Click, label_Edit.Click
        CurMenu = "Edit"
        ' animateWin(panel_OpenedEdit.Handle, True, True)
        AnimateObject(panel_OpenedEdit, True, True, False)
        panel_OpenedEdit.BringToFront()
        ' panel_OpenedEdit.Visible = True
    End Sub
    Private Sub label_View_Click(sender As Object, e As EventArgs) Handles pb_View.Click, label_View.Click
        CurMenu = "View"
        '  animateWin(panel_OpenedView.Handle, True, True)
        AnimateObject(panel_OpenedView, True, True, False)
        panel_OpenedView.BringToFront()
        ' panel_OpenedView.Visible = True
    End Sub
    Private Sub label_ViewMore_Click(sender As Object, e As EventArgs) Handles label_ViewMore.Click
        CurMenu = "View More"
        'animateWin(panel_OpenedViewMore.Handle, True, True)
        '  AnimateWindow(panel_OpenedViewMore.Handle, 200, AnimateWindowFlags.AW_SLIDE Or AnimateWindowFlags.AW_VER_NEGATIVE)
        AnimateObject(panel_OpenedViewMore, True, True, True)
        panel_OpenedViewMore.BringToFront()
        ' panel_OpenedView.Visible = True
    End Sub
    Private Sub label_Settings_Click(sender As Object, e As EventArgs) Handles pb_Settings.Click, label_Settings.Click
        CurMenu = "Settings"
        ' animateWin(panel_OpenedSettings.Handle, True, True)
        AnimateObject(panel_OpenedSettings, True, True, False)
        panel_OpenedSettings.BringToFront()
        ' panel_OpenedSettings.Visible = True
    End Sub
    Private Sub label_Close_Click(sender As Object, e As EventArgs) Handles pb_Close.Click, label_Close.Click
        CurMenu = "Close"
        ' animateWin(panel_OpenedClose.Handle, True, True)
        AnimateObject(panel_OpenedClose, True, True, False)
        panel_OpenedClose.BringToFront()
        ' panel_OpenedClose.Visible = True
    End Sub
#End Region

#Region " Switch Back    |   Previous Menu"

    Private Sub But_SettingsPic_Click(sender As Object, e As EventArgs) Handles pb_SwitchBack.Click
        Select Case CurMenu
            Case "Songs"
                AnimateObject(panel_OpenedSongs, False, True, False)
                CurMenu = ""
            Case "Playlists"
                AnimateObject(panel_OpenedPlaylists, False, True, False)
                CurMenu = ""
            Case "Quick Open" ' Bring up normal popup menu ??
            Case "Media"
                AnimateObject(panel_OpenedMedia, False, True, False)
                CurMenu = ""
            Case "Other Media"

            Case "Subtitles" ' Bring up normal popup menu ??
            Case "Audio"     ' Bring up normal popup menu ??

            Case "Edit"
                AnimateObject(panel_OpenedEdit, False, True, False)
                CurMenu = ""
            Case "View"
                AnimateObject(panel_OpenedView, False, True, False)
                CurMenu = ""
            Case "View More"
                AnimateObject(panel_OpenedViewMore, False, True, True)
                CurMenu = "View"
            Case "Opacity"
            Case "Enhanced Skins"
            Case "Controls Background"
            Case "HD"
            Case "Tiled"
            Case "Spectrum Colors"
            Case "Windows Colors"
            Case "Orientation"
            Case "Settings"
                AnimateObject(panel_OpenedSettings, False, True, False)

                CurMenu = ""
            Case "Rich Player Bounds"
            Case "About"
            Case "Close"
                AnimateObject(panel_OpenedClose, False, True, False)
                CurMenu = ""
            Case Else
                CurMenu = ""
                PerfromClose()
        End Select
    End Sub


#End Region


#Region " Graphics  |   Hover"

#Region " Main Menus"

    Dim SongsHover As Boolean = False
    Private Sub label_Songs_MouseEnter(sender As Object, e As EventArgs) Handles pb_Songs.MouseEnter, label_Songs.MouseEnter
        panel_Songs.BackColor = Color.FromArgb(100, 100, 100)
        SongsHover = True
    End Sub
    Private Sub label_Songs_MouseLeave(sender As Object, e As EventArgs) Handles pb_Songs.MouseLeave, label_Songs.MouseLeave
        panel_Songs.BackColor = Color.Transparent
        SongsHover = False
    End Sub
    Private Sub label_Songs_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_Songs.MouseDown, label_Songs.MouseDown
        panel_Songs.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_Songs_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_Songs.MouseUp, label_Songs.MouseUp
        If SongsHover Then
            panel_Songs.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_Songs.BackColor = Color.Transparent
        End If
    End Sub

    Dim PlaylistsHover As Boolean = False
    Private Sub label_Playlists_MouseEnter(sender As Object, e As EventArgs) Handles pb_Playlists.MouseEnter, label_Playlists.MouseEnter
        panel_Playlists.BackColor = Color.FromArgb(100, 100, 100)
        PlaylistsHover = True
    End Sub
    Private Sub label_Playlists_MouseLeave(sender As Object, e As EventArgs) Handles pb_Playlists.MouseLeave, label_Playlists.MouseLeave
        panel_Playlists.BackColor = Color.Transparent
        PlaylistsHover = False
    End Sub
    Private Sub label_Playlists_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_Playlists.MouseDown, label_Playlists.MouseDown
        panel_Playlists.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_Playlists_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_Playlists.MouseUp, label_Playlists.MouseUp
        If PlaylistsHover Then
            panel_Playlists.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_Playlists.BackColor = Color.Transparent
        End If
    End Sub

    Dim MediaHover As Boolean = False
    Private Sub label_Media_MouseEnter(sender As Object, e As EventArgs) Handles pb_Media.MouseEnter, label_Media.MouseEnter
        panel_Media.BackColor = Color.FromArgb(100, 100, 100)
        MediaHover = True
    End Sub
    Private Sub label_Media_MouseLeave(sender As Object, e As EventArgs) Handles pb_Media.MouseLeave, label_Media.MouseLeave
        panel_Media.BackColor = Color.Transparent
        MediaHover = False
    End Sub
    Private Sub label_Media_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_Media.MouseDown, label_Media.MouseDown
        panel_Media.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_Media_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_Media.MouseUp, label_Media.MouseUp
        If MediaHover Then
            panel_Media.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_Media.BackColor = Color.Transparent
        End If
    End Sub

    Dim EditHover As Boolean = False
    Private Sub label_Edit_MouseEnter(sender As Object, e As EventArgs) Handles pb_Edit.MouseEnter, label_Edit.MouseEnter
        panel_Edit.BackColor = Color.FromArgb(100, 100, 100)
        EditHover = True
    End Sub
    Private Sub label_Edit_MouseLeave(sender As Object, e As EventArgs) Handles pb_Edit.MouseLeave, label_Edit.MouseLeave
        panel_Edit.BackColor = Color.Transparent
        EditHover = False
    End Sub
    Private Sub label_Edit_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_Edit.MouseDown, label_Edit.MouseDown
        panel_Edit.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_Edit_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_Edit.MouseUp, label_Edit.MouseUp
        If EditHover Then
            panel_Edit.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_Edit.BackColor = Color.Transparent
        End If
    End Sub

    Dim ViewHover As Boolean = False
    Private Sub label_View_MouseEnter(sender As Object, e As EventArgs) Handles pb_View.MouseEnter, label_View.MouseEnter
        panel_View.BackColor = Color.FromArgb(100, 100, 100)
        ViewHover = True
    End Sub
    Private Sub label_View_MouseLeave(sender As Object, e As EventArgs) Handles pb_View.MouseLeave, label_View.MouseLeave
        panel_View.BackColor = Color.Transparent
        ViewHover = False
    End Sub
    Private Sub label_View_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_View.MouseDown, label_View.MouseDown
        panel_View.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_View_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_View.MouseUp, label_View.MouseUp
        If ViewHover Then
            panel_View.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_View.BackColor = Color.Transparent
        End If
    End Sub

    Dim SettingsHover As Boolean = False
    Private Sub label_Settings_MouseEnter(sender As Object, e As EventArgs) Handles pb_Settings.MouseEnter, label_Settings.MouseEnter
        panel_Settings.BackColor = Color.FromArgb(100, 100, 100)
        SettingsHover = True
    End Sub
    Private Sub label_Settings_MouseLeave(sender As Object, e As EventArgs) Handles pb_Settings.MouseLeave, label_Settings.MouseLeave
        panel_Settings.BackColor = Color.Transparent
        SettingsHover = False
    End Sub
    Private Sub label_Settings_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_Settings.MouseDown, label_Settings.MouseDown
        panel_Settings.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_Settings_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_Settings.MouseUp, label_Settings.MouseUp
        If SettingsHover Then
            panel_Settings.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_Settings.BackColor = Color.Transparent
        End If
    End Sub

    Dim CloseHover As Boolean = False
    Private Sub label_Close_MouseEnter(sender As Object, e As EventArgs) Handles pb_Close.MouseEnter, label_Close.MouseEnter
        panel_Close.BackColor = Color.FromArgb(100, 100, 100)
        CloseHover = True
    End Sub
    Private Sub label_Close_MouseLeave(sender As Object, e As EventArgs) Handles pb_Close.MouseLeave, label_Close.MouseLeave
        panel_Close.BackColor = Color.Transparent
        CloseHover = False
    End Sub
    Private Sub label_Close_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_Close.MouseDown, label_Close.MouseDown
        panel_Close.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_Close_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_Close.MouseUp, label_Close.MouseUp
        If CloseHover Then
            panel_Close.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_Close.BackColor = Color.Transparent
        End If
    End Sub

#End Region

#Region " Playlist Menu"

    Dim ShufflePlayHover As Boolean = False
    Private Sub label_ShufflePlay_MouseEnter(sender As Object, e As EventArgs) Handles pb_ShufflePlay.MouseEnter, label_ShufflePlay.MouseEnter
        panel_ShufflePlay.BackColor = Color.FromArgb(100, 100, 100)
        ShufflePlayHover = True
    End Sub
    Private Sub label_ShufflePlay_MouseLeave(sender As Object, e As EventArgs) Handles pb_ShufflePlay.MouseLeave, label_ShufflePlay.MouseLeave
        panel_ShufflePlay.BackColor = Color.Transparent
        ShufflePlayHover = False
    End Sub
    Private Sub label_ShufflePlay_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_ShufflePlay.MouseDown, label_ShufflePlay.MouseDown
        panel_ShufflePlay.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_ShufflePlay_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_ShufflePlay.MouseUp, label_ShufflePlay.MouseUp
        If ShufflePlayHover Then
            panel_ShufflePlay.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_ShufflePlay.BackColor = Color.Transparent
        End If
    End Sub

    Dim YTHover As Boolean = False
    Private Sub label_YT_MouseEnter(sender As Object, e As EventArgs) Handles pb_YT.MouseEnter, label_YT.MouseEnter
        panel_YT.BackColor = Color.FromArgb(100, 100, 100)
        YTHover = True
    End Sub
    Private Sub label_YT_MouseLeave(sender As Object, e As EventArgs) Handles pb_YT.MouseLeave, label_YT.MouseLeave
        panel_YT.BackColor = Color.Transparent
        YTHover = False
    End Sub
    Private Sub label_YT_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_YT.MouseDown, label_YT.MouseDown
        panel_YT.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_YT_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_YT.MouseUp, label_YT.MouseUp
        If YTHover Then
            panel_YT.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_YT.BackColor = Color.Transparent
        End If
    End Sub

    Dim OpenYTHover As Boolean = False
    Private Sub label_OpenYT_MouseEnter(sender As Object, e As EventArgs) Handles label_OpenYT.MouseEnter, pb_YT2.MouseEnter
        panel_OpenYT.BackColor = Color.FromArgb(100, 100, 100)
        OpenYTHover = True
    End Sub
    Private Sub label_OpenYT_MouseLeave(sender As Object, e As EventArgs) Handles label_OpenYT.MouseLeave, pb_YT2.MouseLeave
        panel_OpenYT.BackColor = Color.Transparent
        OpenYTHover = False
    End Sub
    Private Sub label_OpenYT_MouseDown(sender As Object, e As MouseEventArgs) Handles label_OpenYT.MouseDown, pb_YT2.MouseDown
        panel_OpenYT.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_OpenYT_MouseUp(sender As Object, e As MouseEventArgs) Handles label_OpenYT.MouseUp, pb_YT2.MouseUp
        If OpenYTHover Then
            panel_OpenYT.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_OpenYT.BackColor = Color.Transparent
        End If
    End Sub

    Dim RadioHover As Boolean = False
    Private Sub label_Radio_MouseEnter(sender As Object, e As EventArgs) Handles pb_Radio.MouseEnter, label_Radio.MouseEnter
        panel_Radio.BackColor = Color.FromArgb(100, 100, 100)
        RadioHover = True
    End Sub
    Private Sub label_Radio_MouseLeave(sender As Object, e As EventArgs) Handles pb_Radio.MouseLeave, label_Radio.MouseLeave
        panel_Radio.BackColor = Color.Transparent
        RadioHover = False
    End Sub
    Private Sub label_Radio_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_Radio.MouseDown, label_Radio.MouseDown
        panel_Radio.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_Radio_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_Radio.MouseUp, label_Radio.MouseUp
        If RadioHover Then
            panel_Radio.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_Radio.BackColor = Color.Transparent
        End If
    End Sub

    Dim OpenRadioHover As Boolean = False
    Private Sub label_OpenRadio_MouseEnter(sender As Object, e As EventArgs) Handles pb_OpenRadio.MouseEnter, label_OpenRadio.MouseEnter
        panel_OpenRadio.BackColor = Color.FromArgb(100, 100, 100)
        OpenRadioHover = True
    End Sub
    Private Sub label_OpenRadio_MouseLeave(sender As Object, e As EventArgs) Handles pb_OpenRadio.MouseLeave, label_OpenRadio.MouseLeave
        panel_OpenRadio.BackColor = Color.Transparent
        OpenRadioHover = False
    End Sub
    Private Sub label_OpenRadio_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_OpenRadio.MouseDown, label_OpenRadio.MouseDown
        panel_OpenRadio.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_OpenRadio_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_OpenRadio.MouseUp, label_OpenRadio.MouseUp
        If OpenRadioHover Then
            panel_OpenRadio.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_OpenRadio.BackColor = Color.Transparent
        End If
    End Sub


    Dim OpenEqualizerHover As Boolean = False
    Private Sub label_OpenEqualizer_MouseEnter(sender As Object, e As EventArgs) Handles pb_OpenEqualizer.MouseEnter, label_OpenEqualizer.MouseEnter
        panel_OpenEqualizer.BackColor = Color.FromArgb(100, 100, 100)
        OpenEqualizerHover = True
    End Sub
    Private Sub label_OpenEqualizer_MouseLeave(sender As Object, e As EventArgs) Handles pb_OpenEqualizer.MouseLeave, label_OpenEqualizer.MouseLeave
        panel_OpenEqualizer.BackColor = Color.Transparent
        OpenEqualizerHover = False
    End Sub
    Private Sub label_OpenEqualizer_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_OpenEqualizer.MouseDown, label_OpenEqualizer.MouseDown
        panel_OpenEqualizer.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_OpenEqualizer_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_OpenEqualizer.MouseUp, label_OpenEqualizer.MouseUp
        If OpenEqualizerHover Then
            panel_OpenEqualizer.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_OpenEqualizer.BackColor = Color.Transparent
        End If
    End Sub

    Dim OpenHover As Boolean = False
    Dim OpenFolderHover As Boolean = False
    Dim FolderSubHover As Boolean = False
    Dim AddFolderSubHover As Boolean = False

    Private Sub label_Open_MouseEnter(sender As Object, e As EventArgs) Handles label_OpenFolder.MouseEnter, label_Open.MouseEnter, label_FolderSub.MouseEnter, label_AddFolderSub.MouseEnter
        sender.BackColor = Color.FromArgb(100, 100, 100)
        Select Case sender.name
            Case "label_Open"
                OpenHover = True
            Case "label_OpenFolder"
                OpenFolderHover = True
            Case "label_FolderSub"
                FolderSubHover = True
            Case "label_AddFolderSub"
                AddFolderSubHover = True
        End Select
    End Sub

    Private Sub label_Open_MouseLeave(sender As Object, e As EventArgs) Handles label_OpenFolder.MouseLeave, label_Open.MouseLeave, label_FolderSub.MouseLeave, label_AddFolderSub.MouseLeave
        sender.BackColor = Color.Transparent
        Select Case sender.name
            Case "label_Open"
                OpenHover = False
            Case "label_OpenFolder"
                OpenFolderHover = False
            Case "label_FolderSub"
                FolderSubHover = False
            Case "label_AddFolderSub"
                AddFolderSubHover = False
        End Select
    End Sub

    Private Sub label_Open_MouseUp(sender As Object, e As MouseEventArgs) Handles label_OpenFolder.MouseUp, label_Open.MouseUp, label_FolderSub.MouseUp, label_AddFolderSub.MouseUp
        Select Case sender.name
            Case "label_Open"
                If OpenHover Then
                    sender.BackColor = Color.FromArgb(100, 100, 100)
                Else
                    sender.BackColor = Color.Transparent
                End If
            Case "label_OpenFolder"
                If OpenFolderHover Then
                    sender.BackColor = Color.FromArgb(100, 100, 100)
                Else
                    sender.BackColor = Color.Transparent
                End If
            Case "label_FolderSub"
                If FolderSubHover Then
                    sender.BackColor = Color.FromArgb(100, 100, 100)
                Else
                    sender.BackColor = Color.Transparent
                End If
            Case "label_AddFolderSub"
                If AddFolderSubHover Then
                    sender.BackColor = Color.FromArgb(100, 100, 100)
                Else
                    sender.BackColor = Color.Transparent
                End If
        End Select
    End Sub



    Dim SaveHover As Boolean = False
    Dim RenamePlaylistHover As Boolean = False
    Dim RenameHover As Boolean = False
    Dim QuickOpenHover As Boolean = False
    Dim ChooseColorHover As Boolean = False
    Dim AdjustHover As Boolean = False

    Private Sub PlaylistsOpen_MouseEnter(sender As Object, e As EventArgs) Handles label_Save.MouseEnter, label_RenamePlaylist.MouseEnter, label_Rename.MouseEnter, label_QuickOpen.MouseEnter, label_ChooseColor.MouseEnter, label_Adjust.MouseEnter
        sender.BackColor = Color.FromArgb(100, 100, 100)
        Select Case sender.name
            Case "label_Save"
                SaveHover = True
            Case "label_RenamePlaylist"
                RenamePlaylistHover = True
            Case "label_Rename"
                RenameHover = True
            Case "label_QuickOpen"
                QuickOpenHover = True
            Case "label_ChooseColor"
                ChooseColorHover = True
            Case "label_Adjust"
                AdjustHover = True
        End Select
    End Sub
    Private Sub PlaylistsOpen_MouseLeave(sender As Object, e As EventArgs) Handles label_Save.MouseLeave, label_RenamePlaylist.MouseLeave, label_Rename.MouseLeave, label_QuickOpen.MouseLeave, label_ChooseColor.MouseLeave, label_Adjust.MouseLeave
        sender.BackColor = Color.Transparent
        Select Case sender.name
            Case "label_Save"
                SaveHover = False
            Case "label_RenamePlaylist"
                RenamePlaylistHover = False
            Case "label_Rename"
                RenameHover = False
            Case "label_QuickOpen"
                QuickOpenHover = False
            Case "label_ChooseColor"
                ChooseColorHover = False
            Case "label_Adjust"
                AdjustHover = False
        End Select
    End Sub
    Private Sub PlaylistsOpen_MouseUp(sender As Object, e As MouseEventArgs) Handles label_Save.MouseUp, label_RenamePlaylist.MouseUp, label_Rename.MouseUp, label_QuickOpen.MouseUp, label_ChooseColor.MouseUp, label_Adjust.MouseUp
        Select Case sender.name
            Case "label_Save"
                If SaveHover Then
                    sender.BackColor = Color.FromArgb(100, 100, 100)
                Else
                    sender.BackColor = Color.Transparent
                End If
            Case "label_RenamePlaylist"
                If RenamePlaylistHover Then
                    sender.BackColor = Color.FromArgb(100, 100, 100)
                Else
                    sender.BackColor = Color.Transparent
                End If
            Case "label_Rename"
                If RenameHover Then
                    sender.BackColor = Color.FromArgb(100, 100, 100)
                Else
                    sender.BackColor = Color.Transparent
                End If
            Case "label_QuickOpen"
                If QuickOpenHover Then
                    sender.BackColor = Color.FromArgb(100, 100, 100)
                Else
                    sender.BackColor = Color.Transparent
                End If
            Case "label_ChooseColor"
                If ChooseColorHover Then
                    sender.BackColor = Color.FromArgb(100, 100, 100)
                Else
                    sender.BackColor = Color.Transparent
                End If
            Case "label_Adjust"
                If AdjustHover Then
                    sender.BackColor = Color.FromArgb(100, 100, 100)
                Else
                    sender.BackColor = Color.Transparent
                End If
        End Select
    End Sub

#End Region

#Region " All Labels"

    Dim AllHovers As Boolean = False
    Private Sub labels_MouseEnter(sender As Object, e As EventArgs) Handles label_Subtitles.MouseEnter, _
           label_SpotifyUse.MouseEnter, label_SpotifyOpen.MouseEnter, label_SpotifyOpenWeb.MouseEnter, label_Audio.MouseEnter, _
           label_CustomizeHotkeys.MouseEnter, label_SaveLyrics.MouseEnter, label_ResetLyrics.MouseEnter, _
           label_ArtworkFilename.MouseEnter, label_ArtworkAlbum.MouseEnter, _
           label_Orientation.MouseEnter, label_Opacity.MouseEnter, label_ControlsBG.MouseEnter, label_ChangeWH.MouseEnter, _
           label_CA.MouseEnter, label_ViewMore.MouseEnter, label_DriveMode.MouseEnter, _
           label_SaveAll.MouseEnter, label_TempDisHotkeys.MouseEnter, label_RichPlayerBounds.MouseEnter, label_Reset.MouseEnter, _
           label_Refresh.MouseEnter, label_Options.MouseEnter, label_OpenAppLoc.MouseEnter, labeL_About.MouseEnter, label_SaveAll.MouseEnter, _
            label_Exit.MouseEnter, label_ForceExit.MouseEnter, label_ViewAccount.MouseEnter, label_Tutorial.MouseEnter, pb_SpotifyRefresh.MouseEnter, _
            label_SaveSettings.MouseEnter, label_RestoreSettings.MouseEnter
        sender.BackColor = Color.FromArgb(100, 100, 100)
        AllHovers = True
    End Sub
    Private Sub labels_MouseLeave(sender As Object, e As EventArgs) Handles label_Subtitles.MouseLeave, _
        label_SpotifyUse.MouseLeave, label_SpotifyOpen.MouseLeave, label_SpotifyOpenWeb.MouseLeave, label_Audio.MouseLeave, _
           label_CustomizeHotkeys.MouseLeave, label_SaveLyrics.MouseLeave, label_ResetLyrics.MouseLeave, _
           label_ArtworkFilename.MouseLeave, label_ArtworkAlbum.MouseLeave, _
           label_Orientation.MouseLeave, label_Opacity.MouseLeave, label_ControlsBG.MouseLeave, label_ChangeWH.MouseLeave, _
           label_CA.MouseLeave, label_ViewMore.MouseLeave, label_DriveMode.MouseLeave, _
           label_SaveAll.MouseLeave, label_TempDisHotkeys.MouseLeave, label_RichPlayerBounds.MouseLeave, label_Reset.MouseLeave, _
           label_Refresh.MouseLeave, label_Options.MouseLeave, label_OpenAppLoc.MouseLeave, labeL_About.MouseLeave, label_SaveAll.MouseLeave, _
            label_Exit.MouseLeave, label_ForceExit.MouseLeave, label_ViewAccount.MouseLeave, label_Tutorial.MouseLeave, pb_SpotifyRefresh.MouseLeave, _
            label_SaveSettings.MouseLeave, label_RestoreSettings.MouseLeave
        sender.BackColor = Color.Transparent
        AllHovers = False
    End Sub
    Private Sub labels_MouseUp(sender As Object, e As MouseEventArgs) Handles _
        label_Subtitles.MouseUp, label_SpotifyUse.MouseUp, label_SpotifyOpen.MouseUp, label_SpotifyOpenWeb.MouseUp, label_Audio.MouseUp, _
           label_CustomizeHotkeys.MouseUp, label_SaveLyrics.MouseUp, label_ResetLyrics.MouseUp, label_ArtworkFilename.MouseUp, _
           label_ArtworkAlbum.MouseUp, _
           label_Orientation.MouseUp, label_Opacity.MouseUp, label_ControlsBG.MouseUp, label_ChangeWH.MouseUp, _
           label_CA.MouseUp, label_ViewMore.MouseUp, label_DriveMode.MouseUp, _
           label_SaveAll.MouseUp, label_TempDisHotkeys.MouseUp, label_RichPlayerBounds.MouseUp, label_Reset.MouseUp, _
           label_Refresh.MouseUp, label_Options.MouseUp, label_OpenAppLoc.MouseUp, labeL_About.MouseUp, label_SaveAll.MouseUp, _
            label_Exit.MouseUp, label_ForceExit.MouseUp, label_ViewAccount.MouseUp, label_Tutorial.MouseUp, pb_SpotifyRefresh.MouseUp, _
            label_SaveSettings.MouseUp, label_RestoreSettings.MouseUp


        If AllHovers Then
            sender.BackColor = Color.FromArgb(100, 100, 100)
        Else
            sender.BackColor = Color.Transparent
        End If
    End Sub
    Private Sub labels_MouseDown(sender As Object, e As MouseEventArgs) Handles label_OpenFolder.MouseDown, label_Open.MouseDown, _
        label_FolderSub.MouseDown, label_AddFolderSub.MouseDown, label_Save.MouseDown, label_RenamePlaylist.MouseDown, label_Rename.MouseDown, _
        label_QuickOpen.MouseDown, label_ChooseColor.MouseDown, label_Adjust.MouseDown, _
        label_Subtitles.MouseDown, label_SpotifyUse.MouseDown, label_SpotifyOpen.MouseDown, label_SpotifyOpenWeb.MouseDown, label_Audio.MouseDown, _
           label_CustomizeHotkeys.MouseDown, label_SaveLyrics.MouseDown, label_ResetLyrics.MouseDown, _
           label_ArtworkFilename.MouseDown, label_ArtworkAlbum.MouseDown, _
           label_Orientation.MouseDown, label_Opacity.MouseDown, label_ControlsBG.MouseDown, label_ChangeWH.MouseDown, _
           label_CA.MouseDown, label_ViewMore.MouseDown, label_DriveMode.MouseDown, _
           label_SaveAll.MouseDown, label_TempDisHotkeys.MouseDown, label_RichPlayerBounds.MouseDown, label_Reset.MouseDown, _
           label_Refresh.MouseDown, label_Options.MouseDown, label_OpenAppLoc.MouseDown, labeL_About.MouseDown, label_SaveAll.MouseDown, _
           label_Exit.MouseDown, label_ForceExit.MouseDown, label_ViewAccount.MouseDown, label_Tutorial.MouseDown, pb_SpotifyRefresh.MouseDown, _
            label_SaveSettings.MouseDown, label_RestoreSettings.MouseDown


        sender.BackColor = Color.FromArgb(70, 70, 70)
    End Sub

#End Region

#Region " Panels with pb and label"

    Dim VolumeControlHover As Boolean = False
    Private Sub label_VolumeControl_MouseEnter(sender As Object, e As EventArgs) Handles pb_VolumeControl.MouseEnter, label_VolumeControl.MouseEnter
        panel_VolumeControl.BackColor = Color.FromArgb(100, 100, 100)
        VolumeControlHover = True
    End Sub
    Private Sub label_VolumeControl_MouseLeave(sender As Object, e As EventArgs) Handles pb_VolumeControl.MouseLeave, label_VolumeControl.MouseLeave
        panel_VolumeControl.BackColor = Color.Transparent
        VolumeControlHover = False
    End Sub
    Private Sub label_VolumeControl_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_VolumeControl.MouseDown, label_VolumeControl.MouseDown
        panel_VolumeControl.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_VolumeControl_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_VolumeControl.MouseUp, label_VolumeControl.MouseUp
        If VolumeControlHover Then
            panel_VolumeControl.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_VolumeControl.BackColor = Color.Transparent
        End If
    End Sub


    Dim PlaylistSearchHover As Boolean = False
    Private Sub label_PlaylistSearch_MouseEnter(sender As Object, e As EventArgs) Handles pb_PlaylistSearch.MouseEnter, label_PlaylistSearch.MouseEnter
        panel_PlaylistSearch.BackColor = Color.FromArgb(100, 100, 100)
        PlaylistSearchHover = True
    End Sub
    Private Sub label_PlaylistSearch_MouseLeave(sender As Object, e As EventArgs) Handles pb_PlaylistSearch.MouseLeave, label_PlaylistSearch.MouseLeave
        panel_PlaylistSearch.BackColor = Color.Transparent
        PlaylistSearchHover = False
    End Sub
    Private Sub label_PlaylistSearch_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_PlaylistSearch.MouseDown, label_PlaylistSearch.MouseDown
        panel_PlaylistSearch.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_PlaylistSearch_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_PlaylistSearch.MouseUp, label_PlaylistSearch.MouseUp
        If PlaylistSearchHover Then
            panel_PlaylistSearch.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_PlaylistSearch.BackColor = Color.Transparent
        End If
    End Sub

    Dim EnhancedSkinsHover As Boolean = False
    Private Sub label_EnhancedSkins_MouseEnter(sender As Object, e As EventArgs) Handles pb_EnhancedSkins.MouseEnter, label_EnhancedSkins.MouseEnter
        panel_EnhancedSkins.BackColor = Color.FromArgb(100, 100, 100)
        EnhancedSkinsHover = True
    End Sub
    Private Sub label_EnhancedSkins_MouseLeave(sender As Object, e As EventArgs) Handles pb_EnhancedSkins.MouseLeave, label_EnhancedSkins.MouseLeave
        panel_EnhancedSkins.BackColor = Color.Transparent
        EnhancedSkinsHover = False
    End Sub
    Private Sub label_EnhancedSkins_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_EnhancedSkins.MouseDown, label_EnhancedSkins.MouseDown
        panel_EnhancedSkins.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_EnhancedSkins_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_EnhancedSkins.MouseUp, label_EnhancedSkins.MouseUp
        If EnhancedSkinsHover Then
            panel_EnhancedSkins.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_EnhancedSkins.BackColor = Color.Transparent
        End If
    End Sub

    Dim SpectrumColorsHover As Boolean = False
    Private Sub label_SpectrumColors_MouseEnter(sender As Object, e As EventArgs) Handles pb_SpectrumColors.MouseEnter, label_SpectrumColors.MouseEnter
        panel_SpectrumColors.BackColor = Color.FromArgb(100, 100, 100)
        SpectrumColorsHover = True
    End Sub
    Private Sub label_SpectrumColors_MouseLeave(sender As Object, e As EventArgs) Handles pb_SpectrumColors.MouseLeave, label_SpectrumColors.MouseLeave
        panel_SpectrumColors.BackColor = Color.Transparent
        SpectrumColorsHover = False
    End Sub
    Private Sub label_SpectrumColors_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_SpectrumColors.MouseDown, label_SpectrumColors.MouseDown
        panel_SpectrumColors.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_SpectrumColors_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_SpectrumColors.MouseUp, label_SpectrumColors.MouseUp
        If SpectrumColorsHover Then
            panel_SpectrumColors.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_SpectrumColors.BackColor = Color.Transparent
        End If
    End Sub


    Dim WindowsColorsHover As Boolean = False
    Private Sub label_WindowsColors_MouseEnter(sender As Object, e As EventArgs) Handles pb_WindowsColors.MouseEnter, label_WindowsColors.MouseEnter
        panel_WindowsColors.BackColor = Color.FromArgb(100, 100, 100)
        WindowsColorsHover = True
    End Sub
    Private Sub label_WindowsColors_MouseLeave(sender As Object, e As EventArgs) Handles pb_WindowsColors.MouseLeave, label_WindowsColors.MouseLeave
        panel_WindowsColors.BackColor = Color.Transparent
        WindowsColorsHover = False
    End Sub
    Private Sub label_WindowsColors_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_WindowsColors.MouseDown, label_WindowsColors.MouseDown
        panel_WindowsColors.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_WindowsColors_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_WindowsColors.MouseUp, label_WindowsColors.MouseUp
        If WindowsColorsHover Then
            panel_WindowsColors.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_WindowsColors.BackColor = Color.Transparent
        End If
    End Sub

    Dim UseShadowsHover As Boolean = False
    Private Sub label_UseShadows_MouseEnter(sender As Object, e As EventArgs) Handles pb_UseShadows.MouseEnter, label_UseShadows.MouseEnter
        panel_UseShadows.BackColor = Color.FromArgb(100, 100, 100)
        UseShadowsHover = True
    End Sub
    Private Sub label_UseShadows_MouseLeave(sender As Object, e As EventArgs) Handles pb_UseShadows.MouseLeave, label_UseShadows.MouseLeave
        panel_UseShadows.BackColor = Color.Transparent
        UseShadowsHover = False
    End Sub
    Private Sub label_UseShadows_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_UseShadows.MouseDown, label_UseShadows.MouseDown
        panel_UseShadows.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_UseShadows_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_UseShadows.MouseUp, label_UseShadows.MouseUp
        If UseShadowsHover Then
            panel_UseShadows.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_UseShadows.BackColor = Color.Transparent
        End If
    End Sub

    Dim MiniModeHover As Boolean = False
    Private Sub label_MiniMode_MouseEnter(sender As Object, e As EventArgs) Handles pb_MiniMode.MouseEnter, label_MiniMode.MouseEnter
        panel_MiniMode.BackColor = Color.FromArgb(100, 100, 100)
        MiniModeHover = True
    End Sub
    Private Sub label_MiniMode_MouseLeave(sender As Object, e As EventArgs) Handles pb_MiniMode.MouseLeave, label_MiniMode.MouseLeave
        panel_MiniMode.BackColor = Color.Transparent
        MiniModeHover = False
    End Sub
    Private Sub label_MiniMode_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_MiniMode.MouseDown, label_MiniMode.MouseDown
        panel_MiniMode.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_MiniMode_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_MiniMode.MouseUp, label_MiniMode.MouseUp
        If MiniModeHover Then
            panel_MiniMode.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_MiniMode.BackColor = Color.Transparent
        End If
    End Sub

    Dim SimplisticModeHover As Boolean = False
    Private Sub label_SimplisticMode_MouseEnter(sender As Object, e As EventArgs) Handles pb_SimplisticMode.MouseEnter, label_SimplisticMode.MouseEnter
        panel_SimplisticMode.BackColor = Color.FromArgb(100, 100, 100)
        SimplisticModeHover = True
    End Sub
    Private Sub label_SimplisticMode_MouseLeave(sender As Object, e As EventArgs) Handles pb_SimplisticMode.MouseLeave, label_SimplisticMode.MouseLeave
        panel_SimplisticMode.BackColor = Color.Transparent
        SimplisticModeHover = False
    End Sub
    Private Sub label_SimplisticMode_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_SimplisticMode.MouseDown, label_SimplisticMode.MouseDown
        panel_SimplisticMode.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_SimplisticMode_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_SimplisticMode.MouseUp, label_SimplisticMode.MouseUp
        If SimplisticModeHover Then
            panel_SimplisticMode.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_SimplisticMode.BackColor = Color.Transparent
        End If
    End Sub


    Dim EnableEditingHover As Boolean = False
    Private Sub label_EnableEditing_MouseEnter(sender As Object, e As EventArgs) Handles pb_EnableEditing.MouseEnter, label_EnableEditing.MouseEnter
        panel_EnableEditing.BackColor = Color.FromArgb(100, 100, 100)
        EnableEditingHover = True
    End Sub
    Private Sub label_EnableEditing_MouseLeave(sender As Object, e As EventArgs) Handles pb_EnableEditing.MouseLeave, label_EnableEditing.MouseLeave
        panel_EnableEditing.BackColor = Color.Transparent
        EnableEditingHover = False
    End Sub
    Private Sub label_EnableEditing_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_EnableEditing.MouseDown, label_EnableEditing.MouseDown
        panel_EnableEditing.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_EnableEditing_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_EnableEditing.MouseUp, label_EnableEditing.MouseUp
        If EnableEditingHover Then
            panel_EnableEditing.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_EnableEditing.BackColor = Color.Transparent
        End If
    End Sub

    Dim LyricsHover As Boolean = False
    Private Sub label_Lyrics_MouseEnter(sender As Object, e As EventArgs) Handles pb_lyrics.MouseEnter, label_Lyrics.MouseEnter
        panel_Lyrics.BackColor = Color.FromArgb(100, 100, 100)
        LyricsHover = True
    End Sub
    Private Sub label_Lyrics_MouseLeave(sender As Object, e As EventArgs) Handles pb_lyrics.MouseLeave, label_Lyrics.MouseLeave
        panel_Lyrics.BackColor = Color.Transparent
        LyricsHover = False
    End Sub
    Private Sub label_Lyrics_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_lyrics.MouseDown, label_Lyrics.MouseDown
        panel_Lyrics.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_Lyrics_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_lyrics.MouseUp, label_Lyrics.MouseUp
        If LyricsHover Then
            panel_Lyrics.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_Lyrics.BackColor = Color.Transparent
        End If
    End Sub


    Dim SpotifyOpenWebHover As Boolean = False
    Private Sub label_SpotifyOpenWeb_MouseEnter(sender As Object, e As EventArgs) Handles pb_SpotifyOpenWeb.MouseEnter, label_SpotifyOpenWeb.MouseEnter
        panel_SpotifyOpenWeb.BackColor = Color.FromArgb(100, 100, 100)
        SpotifyOpenWebHover = True
    End Sub
    Private Sub label_SpotifyOpenWeb_MouseLeave(sender As Object, e As EventArgs) Handles pb_SpotifyOpenWeb.MouseLeave, label_SpotifyOpenWeb.MouseLeave
        panel_SpotifyOpenWeb.BackColor = Color.Transparent
        SpotifyOpenWebHover = False
    End Sub
    Private Sub label_SpotifyOpenWeb_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_SpotifyOpenWeb.MouseDown, label_SpotifyOpenWeb.MouseDown
        panel_SpotifyOpenWeb.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_SpotifyOpenWeb_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_SpotifyOpenWeb.MouseUp, label_SpotifyOpenWeb.MouseUp
        If SpotifyOpenWebHover Then
            panel_SpotifyOpenWeb.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_SpotifyOpenWeb.BackColor = Color.Transparent
        End If
    End Sub


    Dim SpotifyOpenHover As Boolean = False
    Private Sub label_SpotifyOpen_MouseEnter(sender As Object, e As EventArgs) Handles pb_SpotifyOpen.MouseEnter, label_SpotifyOpen.MouseEnter
        panel_SpotifyOpen.BackColor = Color.FromArgb(100, 100, 100)
        SpotifyOpenHover = True
    End Sub
    Private Sub label_SpotifyOpen_MouseLeave(sender As Object, e As EventArgs) Handles pb_SpotifyOpen.MouseLeave, label_SpotifyOpen.MouseLeave
        panel_SpotifyOpen.BackColor = Color.Transparent
        SpotifyOpenHover = False
    End Sub
    Private Sub label_SpotifyOpen_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_SpotifyOpen.MouseDown, label_SpotifyOpen.MouseDown
        panel_SpotifyOpen.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_SpotifyOpen_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_SpotifyOpen.MouseUp, label_SpotifyOpen.MouseUp
        If SpotifyOpenHover Then
            panel_SpotifyOpen.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_SpotifyOpen.BackColor = Color.Transparent
        End If
    End Sub

    Dim UseMediaKeysHover As Boolean = False
    Private Sub label_UseMediaKeys_MouseEnter(sender As Object, e As EventArgs) Handles pb_UseMediaKeys.MouseEnter, label_UseMediaKeys.MouseEnter
        panel_UseMediaKeys.BackColor = Color.FromArgb(100, 100, 100)
        UseMediaKeysHover = True
    End Sub
    Private Sub label_UseMediaKeys_MouseLeave(sender As Object, e As EventArgs) Handles pb_UseMediaKeys.MouseLeave, label_UseMediaKeys.MouseLeave
        panel_UseMediaKeys.BackColor = Color.Transparent
        UseMediaKeysHover = False
    End Sub
    Private Sub label_UseMediaKeys_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_UseMediaKeys.MouseDown, label_UseMediaKeys.MouseDown
        panel_UseMediaKeys.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_UseMediaKeys_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_UseMediaKeys.MouseUp, label_UseMediaKeys.MouseUp
        If UseMediaKeysHover Then
            panel_UseMediaKeys.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_UseMediaKeys.BackColor = Color.Transparent
        End If
    End Sub


    Dim PlayFavoritesHover As Boolean = False
    Private Sub label_PlayFavorites_MouseEnter(sender As Object, e As EventArgs) Handles pb_PlayFavorites.MouseEnter, label_PlayFavorites.MouseEnter
        panel_PlayFavorites.BackColor = Color.FromArgb(100, 100, 100)
        PlayFavoritesHover = True
    End Sub
    Private Sub label_PlayFavorites_MouseLeave(sender As Object, e As EventArgs) Handles pb_PlayFavorites.MouseLeave, label_PlayFavorites.MouseLeave
        panel_PlayFavorites.BackColor = Color.Transparent
        PlayFavoritesHover = False
    End Sub
    Private Sub label_PlayFavorites_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_PlayFavorites.MouseDown, label_PlayFavorites.MouseDown
        panel_PlayFavorites.BackColor = Color.FromArgb(70, 70, 70)
    End Sub
    Private Sub label_PlayFavorites_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_PlayFavorites.MouseUp, label_PlayFavorites.MouseUp
        If PlayFavoritesHover Then
            panel_PlayFavorites.BackColor = Color.FromArgb(100, 100, 100)
        Else
            panel_PlayFavorites.BackColor = Color.Transparent
        End If
    End Sub


#End Region

#Region " Switch Back"

    Dim SwitchBackImage As System.Drawing.Image = My.Resources.SwitchBack
    Dim SwitchBackHoverImage As System.Drawing.Image = My.Resources.SwitchBack_Hover
    Dim SwitchBackPressImage As System.Drawing.Image = My.Resources.SwitchBack_Press
    Dim SwitchBackhover As Boolean = False

    Public Sub SwitchBack_MouseEnter(sender As Object, e As EventArgs) Handles pb_SwitchBack.MouseEnter
        pb_SwitchBack.BackgroundImage = SwitchBackHoverImage
        SwitchBackhover = True
        Me.Cursor = Cursors.Default
    End Sub
    Public Sub SwitchBack_MouseLeave(sender As Object, e As EventArgs) Handles pb_SwitchBack.MouseLeave
        pb_SwitchBack.BackgroundImage = SwitchBackImage
        SwitchBackhover = False
    End Sub
    Public Sub SwitchBack_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_SwitchBack.MouseDown
        pb_SwitchBack.BackgroundImage = SwitchBackPressImage
    End Sub
    Public Sub SwitchBack_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_SwitchBack.MouseUp
        If SwitchBackhover = True Then
            pb_SwitchBack.BackgroundImage = SwitchBackHoverImage
        Else
            pb_SwitchBack.BackgroundImage = SwitchBackImage
        End If
    End Sub


#End Region

#End Region

#Region " Menu buttons & checkbox"

    ' Lyric Editing
    Private Sub label_EnableEditing_Click(sender As Object, e As EventArgs) Handles pb_EnableEditing.Click, label_EnableEditing.Click
        My.Settings.CheckStateLyricsCustomize = Not My.Settings.CheckStateLyricsCustomize
        Set_EnableEditingState()

        PerfromClose() : Dim k As New InputSimulator
        k.Keyboard.ModifiedKeyStroke({Ctrl, Alt, Shift}, VirtualKeyCode.F24)
        'My.Settings.Save()
    End Sub
    Public Sub Set_EnableEditingState()
        Select Case My.Settings.CheckStateLyricsCustomize
            Case False
                pb_EnableEditing.BackgroundImage = My.Resources.MenuIcon_cb
            Case True
                pb_EnableEditing.BackgroundImage = My.Resources.MenuIcon_cb_Checked
        End Select
    End Sub
    ' Lyrics
    Private Sub label_lyrics_Click(sender As Object, e As EventArgs) Handles pb_lyrics.Click, label_Lyrics.Click
        My.Settings.CheckStateLyrics = Not My.Settings.CheckStateLyrics
        Set_LyricsState()

        My.Settings.Save()
    End Sub
    Public Sub Set_LyricsState()
        Select Case My.Settings.CheckStateLyrics
            Case False
                pb_lyrics.BackgroundImage = My.Resources.MenuIcon_cb
            Case True
                pb_lyrics.BackgroundImage = My.Resources.MenuIcon_cb_Checked
        End Select
    End Sub

    ' Player Volume Enable Checkbox
    Private Sub label_VolumeControl_Click(sender As Object, e As EventArgs) Handles pb_VolumeControl.Click, label_VolumeControl.Click
        Set_VolumeControlState()

        PerfromClose() : Dim k As New InputSimulator
        k.Keyboard.ModifiedKeyStroke({VirtualKeyCode.CONTROL, VirtualKeyCode.MENU}, VirtualKeyCode.VK_V)

    End Sub
    Public Sub Set_VolumeControlState()
        Select Case My.Settings.EnablePlayerVolumeCheckState
            Case False
                pb_VolumeControl.BackgroundImage = My.Resources.MenuIcon_cb
            Case True
                pb_VolumeControl.BackgroundImage = My.Resources.MenuIcon_cb_Checked
        End Select
    End Sub

    ' Search Box Enable
    Private Sub label_PlaylistSearch_Click(sender As Object, e As EventArgs) Handles pb_PlaylistSearch.Click, label_PlaylistSearch.Click
        Set_PlaylistSearchState()

        PerfromClose() : Dim k As New InputSimulator
        k.Keyboard.ModifiedKeyStroke({VirtualKeyCode.CONTROL, VirtualKeyCode.MENU, VirtualKeyCode.SHIFT}, VirtualKeyCode.VK_S)
    End Sub
    Public Sub Set_PlaylistSearchState()
        Select Case My.Settings.PlaylistSearchBoxCheckState
            Case False
                pb_PlaylistSearch.BackgroundImage = My.Resources.MenuIcon_cb
            Case True
                pb_PlaylistSearch.BackgroundImage = My.Resources.MenuIcon_cb_Checked
        End Select
    End Sub

    ' Icons use Shadows Checkbox
    Private Sub label_UseShadows_Click(sender As Object, e As EventArgs) Handles pb_UseShadows.Click, label_UseShadows.Click
        Set_UseShadowsState()

        PerfromClose() : Dim k As New InputSimulator
        k.Keyboard.ModifiedKeyStroke({VirtualKeyCode.CONTROL, VirtualKeyCode.MENU, VirtualKeyCode.SHIFT}, VirtualKeyCode.VK_I)
    End Sub
    Public Sub Set_UseShadowsState()
        Select Case My.Settings.UseIconShadows
            Case False
                pb_UseShadows.BackgroundImage = My.Resources.MenuIcon_cb
            Case True
                pb_UseShadows.BackgroundImage = My.Resources.MenuIcon_cb_Checked
        End Select
    End Sub

    ' Mini Mode
    Dim MiniModeClick As Boolean = False
    Private Sub label_MiniMode_Click(sender As Object, e As EventArgs) Handles pb_MiniMode.Click, label_MiniMode.Click
        Set_MiniModeState()

        PerfromClose() : Dim k As New InputSimulator
        k.Keyboard.ModifiedKeyStroke({VirtualKeyCode.CONTROL, VirtualKeyCode.MENU}, VirtualKeyCode.VK_M)
    End Sub
    Public Sub Set_MiniModeState()
        Select Case My.Settings.MiniModeOn
            Case False
                pb_MiniMode.BackgroundImage = My.Resources.MenuIcon_cb

            Case True
                pb_MiniMode.BackgroundImage = My.Resources.MenuIcon_cb_Checked

        End Select
    End Sub


    Dim SimplisticModeClick As Boolean = False
    Private Sub label_SimplisticMode_Click(sender As Object, e As EventArgs) Handles pb_SimplisticMode.Click, label_SimplisticMode.Click
		If My.Settings.SimplisticMode Then Return
		Set_SimplisticModeState()

		PerfromClose() : Dim k As New InputSimulator
        k.Keyboard.ModifiedKeyStroke({VirtualKeyCode.CONTROL, VirtualKeyCode.MENU}, VirtualKeyCode.VK_T)
    End Sub
    Public Sub Set_SimplisticModeState()
        Select Case My.Settings.SimplisticMode
            Case False
                pb_SimplisticMode.BackgroundImage = My.Resources.MenuIcon_cb

            Case True
                pb_SimplisticMode.BackgroundImage = My.Resources.MenuIcon_cb_Checked

        End Select
    End Sub


    Public Sub Set_MediaKeysState()
        If My.Settings.UseMediaKeys Then
            pb_UseMediaKeys.BackgroundImage = My.Resources.MenuIcon_cb_Checked
        Else
            pb_UseMediaKeys.BackgroundImage = My.Resources.MenuIcon_cb
        End If
    End Sub

    Public Sub Set_PlayFavoritesState()
        If My.Settings.PlayFavorites Then
            pb_PlayFavorites.BackgroundImage = My.Resources.MenuIcon_cb_Checked
        Else
            pb_PlayFavorites.BackgroundImage = My.Resources.MenuIcon_cb
        End If
    End Sub


#Region " Labels"
    Public Shared RefreshSpotify As Boolean = False

    Private Sub label_Open_Click(sender As Object, e As EventArgs) Handles label_Open.Click, labeL_About.Click, label_AddFolderSub.Click, label_Adjust.Click, label_ArtworkAlbum.Click, _
        label_ArtworkFilename.Click, label_CA.Click, label_ChangeWH.Click, label_ChooseColor.Click, label_ControlsBG.Click, label_CustomizeHotkeys.Click, _
        label_DriveMode.Click, label_Exit.Click, label_ForceExit.Click, label_FolderSub.Click, label_Opacity.Click, label_OpenAppLoc.Click, _
        label_OpenEqualizer.Click, label_OpenFolder.Click, label_Options.Click, label_Orientation.Click, label_QuickOpen.Click, label_Radio.Click, pb_Radio.Click, label_OpenRadio.Click, pb_OpenRadio.Click, _
        label_Refresh.Click, label_Rename.Click, label_RenamePlaylist.Click, label_Reset.Click, label_ResetLyrics.Click, _
        label_Save.Click, label_SaveAll.Click, label_SaveLyrics.Click, label_ShufflePlay.Click, pb_ShufflePlay.Click, _
         label_SpotifyUse.Click, pb_SpotifyRefresh.Click, label_SpotifyOpen.Click, label_SpotifyOpenWeb.Click, pb_SpotifyOpen.Click, pb_SpotifyOpenWeb.Click, label_TempDisHotkeys.Click, _
         label_WindowsColors.Click, label_YT.Click, pb_YT.Click, label_OpenYT.Click, pb_YT2.Click, label_Lyrics.Click, pb_lyrics.Click, label_EnhancedSkins.Click, pb_EnhancedSkins.Click, _
        label_SpectrumColors.Click, pb_SpectrumColors.Click, label_WindowsColors.Click, pb_WindowsColors.Click, label_RichPlayerBounds.Click, _
        label_Audio.Click, label_Subtitles.Click, label_ViewAccount.Click, label_Tutorial.Click, _
        pb_UseMediaKeys.Click, label_UseMediaKeys.Click, _
        pb_PlayFavorites.Click, label_PlayFavorites.Click, _
        label_SaveSettings.Click, label_RestoreSettings.Click

        MenuCommand = ""

        Select Case sender.name
            Case "label_Open"
                My.Settings.YouTubeURL = ""
                My.Settings.RadioURL = ""
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.F1)
            Case "label_OpenFolder"
                SendClick({Ctrl, Alt}, VirtualKeyCode.VK_O)
            Case "label_FolderSub"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.VK_O)
            Case "label_AddFolderSub"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.VK_A)
            Case "label_ShufflePlay"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.VK_P)
            Case "pb_ShufflePlay"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.VK_P)
            Case "label_YT"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.VK_Y)
            Case "pb_yt"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.VK_Y)

            Case "label_OpenYT"

                ' Open YT Browser
                My.Settings.YouTubeURL = "FirstOpened"
                My.Settings.Save()
                '  SendClick({Ctrl, Alt}, VirtualKeyCode.ADD)
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.F1)
            Case "pb_yt2"
                ' Open YT Browser
                My.Settings.YouTubeURL = "FirstOpened"
                My.Settings.Save()
                '  SendClick({Ctrl, Alt}, VirtualKeyCode.ADD)
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.F1)
            Case "label_Radio"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.VK_R)
            Case "pb_Radio"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.VK_R)

            Case "label_OpenRadio"
                ' Open Radio Browser
                My.Settings.RadioURL = "FirstOpened"
                My.Settings.Save()
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.F1)
              '  SendClick({Ctrl, Alt}, VirtualKeyCode.ADD)
            Case "pb_OpenRadio"
                ' Open Radio Browser
                My.Settings.RadioURL = "FirstOpened"
                My.Settings.Save()
                '    SendClick({Ctrl, Alt}, VirtualKeyCode.ADD)
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.F1)

            Case "label_Save"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.F22)
            Case "label_RenamePlaylist"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.F2)
            Case "label_Rename"
                SendClick({Ctrl, Alt}, VirtualKeyCode.F2)

            Case "label_QuickOpen"
                MenuCommand = "label_QuickOpen"
                PerfromClose()

            Case "label_ChooseColor"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.VK_C)
            Case "label_Adjust"
                SendClick({Ctrl, Alt}, VirtualKeyCode.VK_A)
            Case "label_OpenEqualizer"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.VK_E)

            Case "label_UseMediaKeys"
                My.Settings.UseMediaKeys = Not My.Settings.UseMediaKeys
                If My.Settings.UseMediaKeys Then
                    pb_UseMediaKeys.BackgroundImage = My.Resources.MenuIcon_cb_Checked
                Else
                    pb_UseMediaKeys.BackgroundImage = My.Resources.MenuIcon_cb
                End If
            Case "pb_UseMediaKeys"
                My.Settings.UseMediaKeys = Not My.Settings.UseMediaKeys
                Set_MediaKeysState()


            Case "label_SpotifyOpen"
                SendClick({Ctrl, Alt}, VirtualKeyCode.VK_S)
            Case "label_SpotifyOpenWeb"
                Try
                    Dim p As New ProcessStartInfo
                    p.FileName = "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"
                    p.Arguments = "--app=https://open.spotify.com/browse/"
                    ' Start the process
                    Process.Start(p)

                    ' Process.Start("""C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"" --app=https://open.spotify.com/browse/")
                Catch ex As Exception
                    Process.Start("https://open.spotify.com/browse")
                End Try

            Case "label_SpotifyUse"
                RefreshSpotify = False
                SendClick({Ctrl, Shift}, VirtualKeyCode.VK_S)

            Case "pb_SpotifyRefresh"
                RefreshSpotify = True
                SendClick({Ctrl, Shift}, VirtualKeyCode.VK_S)

            Case "label_ViewAccount"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.F12)

            Case "label_Tutorial"
                SendClick({Ctrl, Shift}, VirtualKeyCode.VK_Y)

            Case "label_CustomizeHotkeys"
                SendClick({Ctrl, Shift}, VirtualKeyCode.VK_C)
            Case "label_SaveLyrics"
                SendClick({Ctrl, Shift}, VirtualKeyCode.VK_L)
            Case "label_ResetLyrics"
                SendClick({Ctrl, Shift}, VirtualKeyCode.VK_R)
            Case "label_ArtworkFilename"
                SendClick({Ctrl, Shift}, VirtualKeyCode.VK_E)
            Case "label_ArtworkAlbum"
                SendClick({Ctrl, Alt}, VirtualKeyCode.VK_E)
            Case "label_Lyrics"
                SendClick({Ctrl, Shift, Alt}, VirtualKeyCode.VK_W)
            Case "pb_lyrics"
                SendClick({Ctrl, Shift, Alt}, VirtualKeyCode.VK_W)
            Case "label_Opacity"
                SendClick({Ctrl, Shift}, VirtualKeyCode.VK_O)
            Case "label_EnhancedSkins"
                SendClick({Ctrl}, VirtualKeyCode.VK_E)
            Case "label_ControlsBG"
                SendClick({Ctrl}, VirtualKeyCode.VK_C)
            Case "label_ChangeWH"
                SendClick({Ctrl}, VirtualKeyCode.VK_W)
            Case "label_CA"
                SendClick({Ctrl}, VirtualKeyCode.VK_T)
            Case "label_DriveMode"
                SendClick({Ctrl}, VirtualKeyCode.VK_D)
            Case "label_Options"
                SendClick({Ctrl}, VirtualKeyCode.VK_P)
            Case "label_Refresh"
                SendClick({Alt}, VirtualKeyCode.VK_R)
            Case "label_SaveAll"
                SendClick({Alt}, VirtualKeyCode.VK_A)
            Case "label_Reset"
                SendClick({Alt, Shift}, VirtualKeyCode.VK_R)
            Case "label_OpenAppLoc"
                SendClick({Alt, Shift}, VirtualKeyCode.VK_O)
            Case "label_TempDisHotkeys"
                SendClick({Alt, Shift}, VirtualKeyCode.VK_T)
            Case "label_SpectrumColors"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.F23)
            Case "pb_SpectrumColors"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.F23)
            Case "label_WindowsColors"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.F21)
            Case "pb_WindowsColors"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.F21)
            Case "label_Orientation"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.F20)
            Case "label_RichPlayerBounds"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.F19)
            Case "labeL_About"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.F18)

                'Close App
            Case "label_Exit"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.F17)
            Case "label_ForceExit"
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.F16)

            Case "label_Audio"
                MenuCommand = "label_Audio"
                PerfromClose()
            Case "label_Subtitles"
                MenuCommand = "label_Subtitles"
                PerfromClose()

            Case "pb_PlayFavorites"
                My.Settings.PlayFavorites = Not My.Settings.PlayFavorites
                Set_PlayFavoritesState()

            Case "label_PlayFavorites"
                My.Settings.PlayFavorites = Not My.Settings.PlayFavorites
                Set_PlayFavoritesState()

            Case "label_SaveSettings"
                PerfromClose()
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.F11)


            Case "label_RestoreSettings"
                PerfromClose()
                SendClick({Ctrl, Alt, Shift}, VirtualKeyCode.F10)



        End Select
    End Sub


#End Region


#End Region

#Region " Send Clicks"

#Region " Declare Send Clicks"
    Dim Ctrl = VirtualKeyCode.CONTROL
    Dim Alt = VirtualKeyCode.MENU
    Dim Shift = VirtualKeyCode.SHIFT



#End Region

    Public Function SendClick(m As System.Collections.Generic.IEnumerable(Of VirtualKeyCode), k As VirtualKeyCode)
        If IsFormClosing = False Then
            '   PerfromClose()
        End If

        Dim keyboard As New InputSimulator
        keyboard.Keyboard.ModifiedKeyStroke(m, k)
    End Function

#End Region


    Private Sub spin_RowHeight_EditValueChanged(sender As Object, e As EventArgs) Handles spin_RowHeight.ValueChanged
        My.Settings.PlaylistRowHeight = spin_RowHeight.Value
        My.Settings.Save()
    End Sub


#Region " Settings"

 


#End Region

End Class