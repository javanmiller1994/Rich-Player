'...................Taskbar....................
Imports Microsoft.WindowsAPICodePack
Imports Microsoft.WindowsAPICodePack.Shell
Imports Microsoft.WindowsAPICodePack.Taskbar


Imports Rich_Player.CsWinFormsBlackApp
Imports Rich_Player.CsWinFormsBlackApp.Form1
Imports CefSharp.WinForms
Imports System.Globalization
Imports System.Threading.Tasks
Imports Rich_Player.CefSharp.WinForms.Example.Handlers
Imports System.Runtime.InteropServices
Imports DevExpress.XtraEditors
Imports Rich_Player.AudioController
Imports Gecko.GeckoWebBrowser
Imports System.Drawing.Imaging
Imports System.ComponentModel
Imports CoreAudio
Imports Un4seen.Bass
Imports Rich_Player.Spotify
Imports DevExpress.XtraBars
Imports DevExpress.XtraTab
Imports DevExpress
Imports DevExpress.XtraTab.ViewInfo
Imports DevExpress.XtraEditors.Controls
Imports System.Drawing
Imports System.Windows.Forms
Imports SpotifyAPI.Web.Enums


Public Class CarForm

#Region " Declarations"
    <DllImport("user32.dll")> _
    Private Shared Function SendMessage(hWnd As IntPtr, wMsg As Int32, wParam As Boolean, lParam As Int32) As Integer
    End Function
    Private Declare Function RegisterWindowMessageA Lib "User32" (ByVal M As String) As Integer
    Private Declare Function RegisterShellHookWindow Lib "User32" (ByVal hWnd As IntPtr) As Boolean
    <DllImport("user32.dll", SetLastError:=True)> _
    Private Shared Function FindWindow(lpClassName As String, lpWindowName As String) As IntPtr
    End Function
    Private Const WM_SETREDRAW As Integer = 11
    Const WM_KEYDOWN As Integer = &H100
    Private Const GWL_EXSTYLE = (-20)
    Private Const WS_EX_TRANSPARENT = &H20&
    Private Declare Auto Function SetWindowLong Lib "User32.Dll" (ByVal hWnd As IntPtr, ByVal nIndex As Integer, ByVal dwNewLong As Integer) As Integer
    Private Declare Auto Function SetForegroundWindow Lib "user32" (ByVal hWnd As IntPtr) As Boolean
    Private Const WM_NCHITTEST As Integer = &H84
    Private Const HTCLIENT As Integer = &H1
    Private Const HTCAPTION As Integer = &H2
    Public Const WM_NCLBUTTONDBLCLK As Integer = &HA3
    Public Const WM_DoubleClick As Integer = &H203
    Private Const EM_SCROLL As Int32 = &HB5
    Private Const WM_MOUSEWHEEL As Int32 = &HB6
    Private Const SB_LINEDOWN As Int32 = 1
    Private Const SB_LINEUP As Int32 = 0

    Private Const WM_NCLBUTTONDOWN As Integer = &HA1
    Private Const HTBORDER As Integer = 18
    Private Const HTBOTTOM As Integer = 15
    Private Const HTBOTTOMLEFT As Integer = 16
    Private Const HTBOTTOMRIGHT As Integer = 17
    Private Const HTLEFT As Integer = 10
    Private Const HTRIGHT As Integer = 11
    Private Const HTTOP As Integer = 12
    Private Const HTTOPLEFT As Integer = 13
    Private Const HTTOPRIGHT As Integer = 14
    Dim hkr As New HotKeyRegistryClass(Me.Handle)

    <DllImport("User32.dll")>
    Shared Function RegisterHotKey(ByVal hwnd As IntPtr,
                        ByVal id As Integer, ByVal fsModifiers As Integer,
                        ByVal vk As Integer) As Integer
    End Function
    <DllImport("User32.dll")>
    Shared Function UnregisterHotKey(ByVal hwnd As IntPtr,
                        ByVal id As Integer) As Integer
    End Function
    Private Declare Sub keybd_event Lib "user32" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Long, ByVal dwExtraInfo As Long)

    Private Const WM_PAINT As Integer = &HF
    Dim bAllowPaint As Boolean = True
    Private Const WM_SYSCOMMAND As Integer = 274
    Private Const SC_MAXIMIZE As Integer = 61488
    Declare Auto Function SetParent Lib "user32.dll" (ByVal hWndChild As IntPtr, ByVal hWndNewParent As IntPtr) As Integer
    Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer

    Public WithEvents bgWorker_FormLoad As New BackgroundWorker
    Public WithEvents bgWorker_NowPlaying As New BackgroundWorker



    Dim browserPandora As Gecko.GeckoWebBrowser
    Dim browseriHeartRadio As Gecko.GeckoWebBrowser
    Dim browserSpotify As New Gecko.GeckoWebBrowser


    Public Shared PanelPlayback As Panel '= SplitContainer1.Panel1
    Dim PlaylistScroller As SplitterPanel '= SplitContainer1.Panel2
    Public Shared Panel_AllPlayback As New Panel
    Dim lb_PageNavigation As New ListBox
    Dim TrackbarY As Integer


    Public DontSetParents As Boolean = False

    Dim firstsetupspotify As Boolean = True
    Dim firstsetupnowplaying As Boolean = True
    Dim FirstSetupPandora As Boolean = True
    Dim FirstSetupiHeartRadio As Boolean = True

    Dim DoAdd As Boolean = False

    Dim DoClose As Boolean = True
    Public NowPlayingInit = False
    Public PreviousArtOpac As Double



    Dim Timer_Rate As New Timer '= Form1.Timer_Rate
    Dim Timer_LabelConfirmSpeedFadeIn As New Timer
    Dim Timer_LabelConfirmSpeedFadeOut As New Timer
    Dim Timer_AB As New Timer

    Dim ArtworkTransparency As Integer = 0.25

    Public Sub wait(ByVal seconds As Single)
        For i As Integer = 0 To seconds * 10
            System.Threading.Thread.Sleep(10)
            Application.DoEvents()
            'Label_SongName.Text = i
        Next
    End Sub


#End Region

#Region " Time"

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim time As String = Date.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture)
        Dim today As String = Date.Now.DayOfWeek.ToString + " " + Date.Now.Date

        Label_Time.Text = time
        Label_Date.Text = today
    End Sub

#End Region

#Region " Hotkeys"

    Protected Overrides Sub WndProc(ByRef m As Message)
        Try


            If m.Msg = HotKeyRegistryClass.Messages.WM_HOTKEY Then 'NOT THE ACTUAL WINDOWS NAMESPACE
                Dim ID As String = m.WParam.ToString()

                Select Case ID
                    'F9
                    Case 0
                        Form1.PlayHotkeySub()
                    Case 1
                        Form1.PrevHotkeySub()
                        'F11
                    Case 2
                        Form1.NextHotkeySub()
                        'Ctrl + F9
                    Case 3
                        Form1.StopSub()

                        'Ctrl + F10
                    Case 4
                        Form1.Prevrwhover = True
                        Form1.SkipBackwards5Secs()
                        Form1.Prevrwhover = False

                        'Ctrl + f11
                    Case 5
                        Form1.NextButhover = True
                        Form1.SkipForward5Secs()
                        Form1.NextButhover = False

                    Case 6      'A Repeast
                        Form1.A_Repeat()
                    Case 7      'B Repeat
                        Form1.B_Repeat()
                    Case 8      'AB Repeat Reset
                        Form1.Reset_AB_Repeat()
                    Case 9      'Slow Down
                        Form1.Speed_Slow()
                    Case 10     'Speed Up
                        Form1.Speed_Fast()
                    Case 11     'Speed Reset
                        Form1.Speed_Norm()

                    Case 12     'Pitch Down
                        If Form1.IsVideo = False Then
                            If Not Form1.trackbar_Pitch2.Value = -12 Then
                                Form1.trackbar_Pitch2.Value -= 1
                            End If
                        End If
                    Case 13     'Pitch Up
                        If Form1.IsVideo = False Then
                            If Not Form1.trackbar_Pitch2.Value = 12 Then
                                Form1.trackbar_Pitch2.Value += 1
                            End If
                        End If
                    Case 14     'Pitch Reset
                        If Form1.IsVideo = False Then
                            Form1.trackbar_Pitch2.Value = 0
                        End If
                    Case 15
                        Form1.MySpotify.ToggleHide()



                End Select

            Else

                Select Case m.Msg
                    Case WM_NCHITTEST
                        MyBase.WndProc(m)
                        If m.Result = HTCLIENT Then
                            m.Result = HTCAPTION
                        End If
                    Case WM_NCLBUTTONDBLCLK
                        If m.Msg = WM_DoubleClick Then
                            If Me.WindowState = FormWindowState.Maximized Then
                                Me.WindowState = FormWindowState.Normal
                            Else
                                Me.WindowState = FormWindowState.Maximized
                            End If

                        Else
                            If m.Msg = WM_NCLBUTTONDBLCLK Then Return
                        End If


                    Case WM_DoubleClick
                        If Me.WindowState = FormWindowState.Maximized Then
                            Me.WindowState = FormWindowState.Normal
                        Else
                            Me.WindowState = FormWindowState.Maximized
                        End If


                    Case Else
                        Try
                            MyBase.WndProc(m)
                        Catch
                        End Try

                End Select




                '   Try
                ' MyBase.WndProc(m)
                ' Catch
                'End Try

            End If
        Catch
        End Try

        Try
            If (m.Msg <> WM_PAINT) OrElse (bAllowPaint AndAlso m.Msg = WM_PAINT) Then
                MyBase.WndProc(m)
            End If
        Catch
        End Try

        MyBase.WndProc(m)
    End Sub
    Public Sub RefreshHotkeys()

        Form1.hkr.Unregister(0)
        Form1.hkr.Unregister(1)
        Form1.hkr.Unregister(2)
        Form1.hkr.Unregister(3)
        Form1.hkr.Unregister(4)
        Form1.hkr.Unregister(5)
        Form1.hkr.Unregister(6)
        Form1.hkr.Unregister(7)
        Form1.hkr.Unregister(8)
        Form1.hkr.Unregister(9)
        Form1.hkr.Unregister(10)
        Form1.hkr.Unregister(11)
        Form1.hkr.Unregister(12)
        Form1.hkr.Unregister(13)
        Form1.hkr.Unregister(14)

        Setup_Hotkeys()
    End Sub
    Public Sub Setup_Hotkeys()
        Try
            hkr.Register(My.Settings.PlayPauseKeyCtrl, My.Settings.PlayPauseKey).ToString()
            hkr.Register(My.Settings.PreviousKeyCtrl, My.Settings.PreviousKey).ToString()
            hkr.Register(My.Settings.NextKeyCtrl, My.Settings.NextKey).ToString()
            hkr.Register(My.Settings.StopKeyCtrl, My.Settings.StopKey).ToString()
            hkr.Register(My.Settings.SkipBackwardsKeyCtrl, My.Settings.SkipBackwardsKey).ToString()
            hkr.Register(My.Settings.SkipForwardKeyCtrl, My.Settings.SkipForwardKey).ToString()
            'AB and Speed
            hkr.Register(My.Settings.ArKeyCtrl Or My.Settings.ArKeyAlt, My.Settings.Arkey).ToString()
            hkr.Register(My.Settings.BrKeyCtrl Or My.Settings.BrKeyAlt, My.Settings.Brkey).ToString()
            hkr.Register(My.Settings.ABrKeyCtrl Or My.Settings.ABrKeyAlt, My.Settings.ABrkey).ToString()
            hkr.Register(My.Settings.SlowKeyCtrl Or My.Settings.SlowKeyAlt, My.Settings.Slowkey).ToString()
            hkr.Register(My.Settings.FastKeyCtrl Or My.Settings.FastKeyAlt, My.Settings.FastKey).ToString()
            hkr.Register(My.Settings.SpeedNormCtrl Or My.Settings.SpeedNormAlt, My.Settings.SpeedNormkey).ToString()
            'Pitch
            hkr.Register(My.Settings.PitchDownKeyCtrl Or My.Settings.PitchDownKeyAlt, My.Settings.PitchDownKey).ToString()
            hkr.Register(My.Settings.PitchUpKeyCtrl Or My.Settings.PitchUpKeyAlt, My.Settings.PitchUpKey).ToString()
            hkr.Register(My.Settings.PitchResetKeyCtrl Or My.Settings.PitchResetKeyAlt, My.Settings.PitchResetKey).ToString()
            'Hide Spotify
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT, Keys.H)

        Catch
            MyMsgBox.Show("Error 0x32: Error loading Global Hotkeys. To fix, reapply your desired Hotkeys from the Edit menu.", "", True)
        End Try
    End Sub



#End Region

#Region " Load"


    Private Sub CarForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PreVentFlicker()
        FormLoad()

        My.Settings.Save()
    End Sub

    Public Sub FormLoad()
        My.Settings.CarFormOpened = True
        My.Settings.DriveMode = True
        DontSetParents = True

        Form1.xcarform = Me

        '  SuspendDrawing()

        Me.TopLevel = True
        ' PreviousArtOpac = My.Settings.ArtworkTransparency
        ' My.Settings.ArtworkTransparency = 0.25

        PanelPlayback = SplitContainer1.Panel1
        PlaylistScroller = SplitContainer1.Panel2


        ' tab_Pandora.Controls.Add(browserPandora)
        ' tab_iHeartRadio.Controls.Add(browseriHeartRadio)
        browseriHeartRadio = iHeartRadioBrowser
        browserPandora = PandoraBrowser

        lb_PageNavigation.Items.Add(0)

        Me.MaximumSize = Screen.FromRectangle(Me.Bounds).WorkingArea.Size
        Me.WindowState = FormWindowState.Maximized

        '  ResumeDrawing()

        Timer_Rate.Interval = 1
        Timer_LabelConfirmSpeedFadeIn.Interval = 2000
        Timer_LabelConfirmSpeedFadeOut.Interval = 2000
        Timer_AB.Interval = 1

        AddHandler Timer_AB.Tick, AddressOf abtimer_Tick
        AddHandler Timer_Rate.Tick, AddressOf Timerrate_Tick
        AddHandler Timer_LabelConfirmSpeedFadeIn.Tick, AddressOf TimerLabelConfirmSpeed_Tick
        AddHandler Timer_LabelConfirmSpeedFadeOut.Tick, AddressOf TimerLabelConfirmSpeedFadeOut_Tick

    End Sub

    Private Sub PreVentFlicker()
        With Me
            .SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            .SetStyle(ControlStyles.UserPaint, True)
            .SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            .UpdateStyles()
        End With
    End Sub

#End Region


#Region " Home"

    Private Sub pb_HomeBut_Click(sender As Object, e As EventArgs) Handles pb_HomeBut.Click, pb_iHeartRadio.Click, pb_Pandora.Click, pb_Spotify.Click, pb_NowPlaying.Click
        Select Case sender.name
            Case "pb_HomeBut"
                DoAdd = True
                tabs_Car.SelectedTabPageIndex = 0

            Case "pb_NowPlaying"
                DoAdd = True
                DontSetParents = True
                tabs_Car.SelectedTabPageIndex = 1
                Init_NowPlaying()

            Case "pb_Spotify"
                DoAdd = True
                tabs_Car.SelectedTabPageIndex = 2
                Setup_Spotify()

            Case "pb_iHeartRadio"
                DoAdd = True
                tabs_Car.SelectedTabPageIndex = 3
                SetupiHeartRadio()

            Case "pb_Pandora"
                DoAdd = True
                tabs_Car.SelectedTabPageIndex = 4
                SetupPandora()

        End Select
    End Sub

#End Region

#Region " Navigation"




    Private Sub tabs_Car_SelectedPageChanged(sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles tabs_Car.SelectedPageChanged
        If DoAdd Then
            If lb_PageNavigation.SelectedIndex <> lb_PageNavigation.Items.Count - 1 Then
                For i As Integer = lb_PageNavigation.SelectedIndex + 1 To lb_PageNavigation.Items.Count - 1
                    Try
                        lb_PageNavigation.Items.RemoveAt(i)
                    Catch ex As Exception
                    End Try

                Next
            End If
            lb_PageNavigation.Items.Add(tabs_Car.SelectedTabPageIndex)
            lb_PageNavigation.SelectedIndex += 1
            Try
                ' lb_PageNavigation.SelectedIndex += 1
            Catch ex As Exception
            End Try
            DoAdd = False
        End If


    End Sub

    Private Sub tabs_Car_SelectedPageChanging(sender As Object, e As DevExpress.XtraTab.TabPageChangingEventArgs) Handles tabs_Car.SelectedPageChanging

    End Sub

    Private Sub pb_BackBut_Click(sender As Object, e As EventArgs) Handles pb_BackBut.Click, pb_ForwardBut.Click

        Select Case sender.name
            Case "pb_BackBut"
                Try

                    lb_PageNavigation.SelectedIndex -= 1
                    tabs_Car.SelectedTabPageIndex = CInt(lb_PageNavigation.SelectedItem)
                Catch ex As Exception
                End Try


            Case "pb_ForwardBut"
                Try
                    lb_PageNavigation.SelectedIndex += 1
                    tabs_Car.SelectedTabPageIndex = CInt(lb_PageNavigation.SelectedItem)
                Catch ex As Exception
                End Try


        End Select

    End Sub



#End Region


#Region " Setup Now Playing"

    Public Sub Init_NowPlaying()
        If firstsetupnowplaying Then
            SetupNowPlaying()
            SkinTrackbars()
            RefreshHotkeys()
            ' SetupPlaylistsTabs()
            Form1.PlaylistTabs = Me.PlaylistTabs
            Select Case My.Settings.DriveMode
                Case True : PlaylistTabs = Form1.PlaylistTabs
            End Select
            Form1.Setup_Playlists()
            PlaylistTabs.Dock = DockStyle.Fill


            Try : If Not PlaylistTabs.SelectedTabPage.PageVisible Then
                    TryCast(PlaylistTabs, XtraTabControl).MakePageVisible(PlaylistTabs.SelectedTabPage)
                    PlaylistTabs.SelectedTabPage.Refresh()
                End If
            Catch : End Try
            firstsetupnowplaying = False

        End If
    End Sub

    Public Sub SetupNowPlaying()
        '   SuspendDrawing()


        Dim bgImg As Image
        Dim use As Boolean = False
        If use Then
            Me.TopLevel = True
            Me.TopMost = True
            Form1.Show()
            Form1.Opacity = 0.1

            If My.Settings.MiniModeOn Then
                Form1.MiniModeOff()
            End If

            Select Case My.Settings.EnhancedSkin : Case "None"
                Case 1 : bgImg = My.Resources.form_bg_1 : Case 2 : bgImg = My.Resources.form_bg_21
                Case 3 : bgImg = My.Resources.form_bg_31 : Case 4 : bgImg = My.Resources.form_bg_41
                Case 5 : bgImg = My.Resources.form_bg_51 : Case 6 : bgImg = My.Resources.form_bg_6
                Case 7 : bgImg = My.Resources.form_bg_71 : Case 8 : bgImg = My.Resources.form_bg_81
                Case 9 : bgImg = My.Resources.form_bg_9 : Case 10 : bgImg = My.Resources.form_bg_10
                Case 11 : bgImg = My.Resources.form_bg_11png : Case 12 : bgImg = My.Resources.form_bg_12
                Case 13 : bgImg = My.Resources.form_bg_13 : Case 14 : bgImg = My.Resources.form_bg_14
                Case 15 : bgImg = My.Resources.form_bg_15 : Case "Custom" : bgImg = (System.Drawing.Image.FromFile(My.Settings.CustomImageFile))
            End Select
            If My.Settings.CustomImageCheckState = True Then
                bgImg = (System.Drawing.Image.FromFile(My.Settings.CustomImageFile))
                bgImg = My.Resources.form_bg_9
            End If : End If
        bgImg = My.Resources.form_bg_9
        PanelPlayback.BackgroundImage = ChangeOpacity(bgImg, 0.75)

        ' ReArrange()

        '  ResumeDrawing()
        NowPlayingInit = True
    End Sub

    Public Sub ReArrange()
        ' ........  Artwork .......
        PanelPlayback.Controls.Add(Form1.Artwork)
        Form1.Artwork.Dock = DockStyle.Fill
        Form1.Artwork.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom

        Panel_AllPlayback.Size = New Size(978, 59)
        Panel_AllPlayback.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        Panel_AllPlayback.Controls.Add(Form1.PlaybackPanel)

        Form1.PlaybackPanel.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        Form1.PlaybackPanel.Location = New Point((PanelPlayback.Width - Form1.PlaybackPanel.Width) / 2, 0)
        Panel_AllPlayback.Controls.Add(Form1.PicBox_PitchTextBox)
        Panel_AllPlayback.Controls.Add(Form1.trackbar_Pitch2)
        Panel_AllPlayback.Controls.Add(Form1.But_B)
        Panel_AllPlayback.Controls.Add(Form1.trackBar_Speed2)
        Panel_AllPlayback.Controls.Add(Form1.But_Stop)
        Panel_AllPlayback.Controls.Add(Form1.Label_SpeedTextbox)
        Panel_AllPlayback.Controls.Add(Form1.But_AB_Reset)
        Panel_AllPlayback.Controls.Add(Form1.But_Shuffle)
        Panel_AllPlayback.Controls.Add(Form1.But_Repeat)
        Panel_AllPlayback.Controls.Add(Form1.But_SpeedDown)
        Panel_AllPlayback.Controls.Add(Form1.But_A)
        Panel_AllPlayback.Controls.Add(Form1.But_PitchUp)
        Panel_AllPlayback.Controls.Add(Form1.But_PitchDown)
        Panel_AllPlayback.Controls.Add(Form1.But_SpeedUp)
        Panel_AllPlayback.Size = New System.Drawing.Size(978, 59)

        Form1.Artwork.Controls.Add(Form1.Label_SongName)
        Form1.Artwork.Controls.Add(Form1.Label_Album)
        Form1.Artwork.Controls.Add(Form1.Label_Artist)
        Form1.Artwork.Controls.Add(Panel_AllPlayback)
        Form1.Artwork.Controls.Add(Form1.TrackBar_PlayerVol2)
        Form1.Artwork.Controls.Add(Form1.totaltimelabel)
        Form1.Artwork.Controls.Add(Form1.timelabel)
        Form1.Artwork.Controls.Add(Form1.TrackBar_Seek2)
        Form1.Artwork.Controls.Add(Form1.LabelConfirmSpeed)
        Form1.Artwork.Controls.Add(Form1.PictureBoxSpec)


        ' Player Volume
        Form1.TrackBar_PlayerVol2.Location = New Point(14, 322)
        Form1.TrackBar_PlayerVol2.Size = New Size(84, 45)

        ' VLC Player
        PanelPlayback.Controls.Add(Form1.VlcPlayer)

        'Form1.VlcPlayer.Dock = DockStyle.Fill
        Form1.VlcPlayer.Location = New System.Drawing.Point(5, 193)
        Form1.VlcPlayer.MinimumSize = New System.Drawing.Size(143, 143)
        Form1.VlcPlayer.Size = New System.Drawing.Size(978, 59)
        Form1.VlcPlayer.Height = PanelPlayback.Height - Panel_AllPlayback.Height
        Form1.Artwork.BringToFront()


        Form1.LabelConfirmSpeed.Location = New System.Drawing.Point(26, 101)


        ' A/B Repeat
        Panel_AllPlayback.Controls.Add(Form1.But_A)
        Panel_AllPlayback.Controls.Add(Form1.But_AB_Reset)
        Panel_AllPlayback.Controls.Add(Form1.But_B)
        Form1.But_A.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Form1.But_A.Location = New System.Drawing.Point(0, 6)
        Form1.But_A.Size = New System.Drawing.Size(36, 36)
        Form1.But_AB_Reset.Location = New System.Drawing.Point(48, 6)
        Form1.But_AB_Reset.Size = New System.Drawing.Size(36, 36)
        Form1.But_B.Location = New System.Drawing.Point(96, 6)
        Form1.But_B.Size = New System.Drawing.Size(36, 36)
        Form1.But_B.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Form1.But_AB_Reset.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left


        ' Shuffle / Repeat
        Panel_AllPlayback.Controls.Add(Form1.But_Shuffle)
        Form1.But_Shuffle.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Form1.But_Shuffle.Location = New System.Drawing.Point(942, 6)
        Form1.But_Shuffle.Size = New System.Drawing.Size(36, 36)
        Panel_AllPlayback.Controls.Add(Form1.But_Repeat)
        Form1.But_Repeat.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Form1.But_Repeat.Location = New System.Drawing.Point(900, 6)
        Form1.But_Repeat.Size = New System.Drawing.Size(36, 36)

        'Playback
        Panel_AllPlayback.Controls.Add(Form1.But_Stop)
        Form1.But_Stop.Location = New System.Drawing.Point(353, 12)
        Form1.But_Stop.Size = New System.Drawing.Size(31, 31)
        Form1.PlaybackPanel.Controls.Add(Form1.But_Previous)
        Form1.But_Previous.Location = New System.Drawing.Point(3, 7)
        Form1.But_Previous.Size = New System.Drawing.Size(40, 40)
        Form1.PlaybackPanel.Controls.Add(Form1.But_PlayPause)
        Form1.But_PlayPause.Location = New System.Drawing.Point(68, 3)
        Form1.But_PlayPause.Size = New System.Drawing.Size(52, 52)
        Form1.PlaybackPanel.Controls.Add(Form1.But_Next)
        Form1.But_Next.Location = New System.Drawing.Point(145, 7)
        Form1.But_Next.Size = New System.Drawing.Size(40, 40)



        'Speed & Pitch
        PanelPlayback.Controls.Add(Form1.PicBox_PitchTextBox)
        Form1.PicBox_PitchTextBox.Location = New System.Drawing.Point(813, 42)
        PanelPlayback.Controls.Add(Form1.Label_SpeedTextbox)
        Form1.Label_SpeedTextbox.Location = New System.Drawing.Point(675, 42)

        PanelPlayback.Controls.Add(Form1.trackBar_Speed2)
        Form1.trackBar_Speed2.Location = New System.Drawing.Point(675, 42)
        PanelPlayback.Controls.Add(Form1.trackbar_Pitch2)
        Form1.trackbar_Pitch2.Location = New System.Drawing.Point(792, 19)

        PanelPlayback.Controls.Add(Form1.But_PitchUp)
        Form1.But_PitchUp.Location = New System.Drawing.Point(872, 17)
        PanelPlayback.Controls.Add(Form1.But_PitchDown)
        Form1.But_PitchDown.Location = New System.Drawing.Point(771, 17)

        PanelPlayback.Controls.Add(Form1.But_SpeedUp)
        Form1.But_SpeedUp.Location = New System.Drawing.Point(737, 17)

        PanelPlayback.Controls.Add(Form1.But_SpeedDown)
        Form1.But_SpeedDown.Location = New System.Drawing.Point(634, 17)


        'Seek
        Form1.Artwork.Controls.Add(Form1.TrackBar_Seek2)
        TrackbarY = (PanelPlayback.Height - Panel_AllPlayback.Height - 65)
        Form1.TrackBar_Seek2.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        Form1.TrackBar_Seek2.MaximumSize = New System.Drawing.Size(0, 23)
        Form1.TrackBar_Seek2.MinimumSize = New System.Drawing.Size(PanelPlayback.Width - 40, 23)
        Form1.TrackBar_Seek2.Size = New System.Drawing.Size(PanelPlayback.Width - 40, 23)
        Form1.TrackBar_Seek2.Location = New System.Drawing.Point((PanelPlayback.Width - Form1.TrackBar_Seek2.Width) / 2, TrackbarY)
        Form1.TrackBar_Seek2.BringToFront()

        ' Time Label - Current
        PanelPlayback.Controls.Add(Form1.timelabel)
        Form1.timelabel.Location = New System.Drawing.Point(3, 279)
        Form1.timelabel.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Form1.timelabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Form1.timelabel.ForeColor = System.Drawing.Color.Silver
        Form1.timelabel.Location = New System.Drawing.Point(18, TrackbarY + 25)
        Form1.timelabel.MinimumSize = New System.Drawing.Size(80, 17)
        Form1.timelabel.Size = New System.Drawing.Size(80, 17)
        Form1.timelabel.BringToFront()

        ' Time Label - Total
        PanelPlayback.Controls.Add(Form1.totaltimelabel)
        Form1.totaltimelabel.Location = New System.Drawing.Point(905, 279)
        '  Form1.TrackBar_Seek2.SendToBack()
        Form1.totaltimelabel.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        Form1.totaltimelabel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Form1.totaltimelabel.ForeColor = System.Drawing.Color.Silver
        Form1.totaltimelabel.Location = New System.Drawing.Point(PanelPlayback.Width - Form1.totaltimelabel.Width - 10, TrackbarY + 25)
        Form1.totaltimelabel.MaximumSize = New System.Drawing.Size(450, 0)
        Form1.totaltimelabel.MinimumSize = New System.Drawing.Size(41, 17)
        Form1.totaltimelabel.Size = New System.Drawing.Size(80, 17)
        Form1.totaltimelabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Form1.totaltimelabel.BringToFront()


        ' Panel_AllPlayback
        Panel_AllPlayback.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        Panel_AllPlayback.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Panel_AllPlayback.Controls.Add(Form1.PlaybackPanel)
        Panel_AllPlayback.Controls.Add(Form1.PicBox_PitchTextBox)
        Panel_AllPlayback.Controls.Add(Form1.trackbar_Pitch2)
        Panel_AllPlayback.Controls.Add(Form1.But_B)
        Panel_AllPlayback.Controls.Add(Form1.TrackBar_PlayerVol2)
        Panel_AllPlayback.Controls.Add(Form1.trackBar_Speed2)
        Panel_AllPlayback.Controls.Add(Form1.But_Stop)
        Panel_AllPlayback.Controls.Add(Form1.Label_SpeedTextbox)
        Panel_AllPlayback.Controls.Add(Form1.But_AB_Reset)
        Panel_AllPlayback.Controls.Add(Form1.But_Shuffle)
        Panel_AllPlayback.Controls.Add(Form1.But_Repeat)
        Panel_AllPlayback.Controls.Add(Form1.But_SpeedDown)
        Panel_AllPlayback.Controls.Add(Form1.But_A)
        Panel_AllPlayback.Controls.Add(Form1.But_PitchUp)
        Panel_AllPlayback.Controls.Add(Form1.But_PitchDown)
        Panel_AllPlayback.Controls.Add(Form1.But_SpeedUp)

        ' PlaybackPanel
        Form1.PlaybackPanel.Anchor = AnchorStyles.None
        Form1.PlaybackPanel.Controls.Add(Form1.But_Next)
        Form1.PlaybackPanel.Controls.Add(Form1.But_PlayPause)
        Form1.PlaybackPanel.Controls.Add(Form1.But_Previous)
        Form1.PlaybackPanel.Location = New Point((Panel_AllPlayback.Width - Form1.PlaybackPanel.Width) / 2, 0)
        Form1.PlaybackPanel.Size = New System.Drawing.Size(185, 58)
        Form1.PlaybackPanel.BringToFront()

        ' But_Next
        Form1.But_Next.Location = New System.Drawing.Point(142, 7)
        Form1.But_Next.Size = New System.Drawing.Size(40, 40)
        Form1.But_Next.Anchor = AnchorStyles.None

        ' But_PlayPause
        Form1.But_PlayPause.Location = New System.Drawing.Point(65, 3)
        Form1.But_PlayPause.Size = New System.Drawing.Size(52, 52)
        Form1.But_PlayPause.Anchor = AnchorStyles.None

        ' But_Previous
        Form1.But_Previous.Location = New System.Drawing.Point(0, 7)
        Form1.But_Previous.Size = New System.Drawing.Size(40, 40)
        Form1.But_Previous.Anchor = AnchorStyles.None

        ' But_Stop
        Form1.But_Stop.Location = New System.Drawing.Point(353, 12)
        Form1.But_Stop.Size = New System.Drawing.Size(31, 31)
        Form1.But_Stop.Anchor = AnchorStyles.None

        ' But_SpeedDown
        Form1.But_SpeedDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Form1.But_SpeedDown.Location = New System.Drawing.Point(634, 17)
        Form1.But_SpeedDown.Size = New System.Drawing.Size(22, 22)

        ' But_PitchUp
        Form1.But_PitchUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Form1.But_PitchUp.Location = New System.Drawing.Point(872, 17)
        Form1.But_PitchUp.Size = New System.Drawing.Size(22, 22)

        ' But_PitchDown
        Form1.But_PitchDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Form1.But_PitchDown.Location = New System.Drawing.Point(771, 17)
        Form1.But_PitchDown.Size = New System.Drawing.Size(22, 22)

        ' But_SpeedUp
        Form1.But_SpeedUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Form1.But_SpeedUp.Location = New System.Drawing.Point(737, 17)
        Form1.But_SpeedUp.Size = New System.Drawing.Size(22, 22)

        ' Song Labels
        ' Label_Album
        Form1.Label_Album.Anchor = AnchorStyles.Top
        Form1.Label_Album.Font = New System.Drawing.Font("Century Gothic", 12.0!)
        Form1.Label_Album.ForeColor = System.Drawing.Color.Silver
        Form1.Label_Album.Size = New System.Drawing.Size(988, 19)
        Form1.Label_Album.MinimumSize = New System.Drawing.Size(988, 19)
        Form1.Label_Album.Location = New Point((Form1.Artwork.Parent.Width - Form1.Label_Album.Width) \ 2, 15)
        Form1.Label_Album.TextAlign = System.Drawing.ContentAlignment.MiddleCenter

        ' Label_SongName
        Form1.Label_SongName.Anchor = AnchorStyles.Top
        Form1.Label_SongName.Font = New System.Drawing.Font("Century Gothic", 30.0!, System.Drawing.FontStyle.Bold)
        Form1.Label_SongName.ForeColor = System.Drawing.Color.Silver
        Form1.Label_SongName.Size = New System.Drawing.Size(988, 47)
        Form1.Label_SongName.MinimumSize = New System.Drawing.Size(988, 47)
        Form1.Label_SongName.Location = New Point((Form1.Artwork.Parent.Width - Form1.Label_SongName.Width) \ 2, 49)
        Form1.Label_SongName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter

        ' Label_Artist
        Form1.Label_Artist.Anchor = AnchorStyles.Top
        Form1.Label_Artist.Font = New System.Drawing.Font("Century Gothic", 12.0!)
        Form1.Label_Artist.ForeColor = System.Drawing.Color.Silver
        Form1.Label_Artist.Size = New System.Drawing.Size(988, 19)
        Form1.Label_Artist.MinimumSize = New System.Drawing.Size(988, 19)
        Form1.Label_Artist.Location = New Point((Form1.Artwork.Parent.Width - Form1.Label_Artist.Width) \ 2, 111)
        Form1.Label_Artist.TextAlign = System.Drawing.ContentAlignment.MiddleCenter

        ' PicBox_PitchTextBox
        Form1.PicBox_PitchTextBox.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        Form1.PicBox_PitchTextBox.Location = New System.Drawing.Point(813, 42)
        Form1.PicBox_PitchTextBox.Size = New System.Drawing.Size(35, 16)

        ' trackbar_Pitch2
        Form1.trackbar_Pitch2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Form1.trackbar_Pitch2.Location = New System.Drawing.Point(792, 19)
        Form1.trackbar_Pitch2.MaximumSize = New System.Drawing.Size(2000, 23)

        ' TrackBar_PlayerVol2
        Form1.TrackBar_PlayerVol2.Location = New System.Drawing.Point(12, 44)
        Form1.TrackBar_PlayerVol2.MaximumSize = New System.Drawing.Size(2000, 23)
        Form1.TrackBar_PlayerVol2.Size = New System.Drawing.Size(116, 23)
        Form1.TrackBar_PlayerVol2.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Form1.TrackBar_PlayerVol2.BringToFront()

        ' trackBar_Speed2
        Form1.trackBar_Speed2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Form1.trackBar_Speed2.Location = New System.Drawing.Point(656, 19)
        Form1.trackBar_Speed2.MaximumSize = New System.Drawing.Size(2000, 23)
        Form1.trackBar_Speed2.Size = New System.Drawing.Size(80, 23)

        ' Label_SpeedTextbox
        Form1.Label_SpeedTextbox.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        Form1.Label_SpeedTextbox.Location = New System.Drawing.Point(675, 42)
        Form1.Label_SpeedTextbox.MaximumSize = New System.Drawing.Size(25, 16)
        Form1.Label_SpeedTextbox.MinimumSize = New System.Drawing.Size(35, 16)
        Form1.Label_SpeedTextbox.Size = New System.Drawing.Size(35, 16)


        Panel_AllPlayback.Size = New System.Drawing.Size(PanelPlayback.Width, 59)
        Panel_AllPlayback.Location = New System.Drawing.Point((PanelPlayback.Width - Panel_AllPlayback.Width) / 2, (PanelPlayback.Height - Panel_AllPlayback.Height - 20))

        ' Spectrum
        PanelPlayback.Controls.Add(Form1.PictureBoxSpec)
        Form1.PictureBoxSpec.Dock = DockStyle.None
        Form1.PictureBoxSpec.Size = New System.Drawing.Size(350, 100)
        Form1.PictureBoxSpec.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        Form1.PictureBoxSpec.Location = New System.Drawing.Point((Form1.Artwork.Width - Form1.PictureBoxSpec.Width) / 2, TrackbarY - 115)

        Form1.PlaybackPanel.Location = New Point((Panel_AllPlayback.Width - Form1.PlaybackPanel.Width) / 2, 0)

    End Sub
    Public Sub SetupPlaylistsTabs()
        'SuspendLayout()
        Me.TopLevel = True
        Me.TopMost = True

        SuspendDrawing()


        Form1.Show()
        Form1.Opacity = 100
        If My.Settings.MiniModeOn Then
            Form1.MiniModeOff()
        End If
        Dim use As Boolean = False
        If use Then
            PlaylistScroller.Controls.Add(PlaylistTabs)
            PlaylistTabs.Location = New Point(0, 0)
            PlaylistTabs.BringToFront()
            PlaylistTabs.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            PlaylistTabs.Dock = DockStyle.Fill
        End If



        Form1.Opacity = 0
        Form1.ShowInTaskbar = False
        Form1.WindowState = FormWindowState.Minimized

        ResumeDrawing()

        Me.TopMost = False

        '  ResumeLayout()

        ' Form1.PictureBoxSpec.Location = New System.Drawing.Point((Form1.Artwork.Width - Form1.PictureBoxSpec.Width) / 2, TrackbarY - 115)
        '  Form1.PlaybackPanel.Location = New Point((Panel_AllPlayback.Width - Form1.PlaybackPanel.Width) / 2, 0)
    End Sub




    Public Sub SkinTrackbars()
        TrackBar_Seek2.Maximum = SeekBarMaxVal
        TrackBar_PlayerVol2.Value = Form1.TrackBar_PlayerVol2.Value

        Dim LoadedSliderShape As Integer = My.Settings.seekbarShape
        Select Case LoadedSliderShape
            Case 0
                TrackBar_Seek2.SliderShape = RichTrackBar.SliderShapes.Circle
            Case 1
                TrackBar_Seek2.SliderShape = RichTrackBar.SliderShapes.RectangleNarrow
            Case 2
                TrackBar_Seek2.SliderShape = RichTrackBar.SliderShapes.RectangleWide
            Case 3
                TrackBar_Seek2.SliderShape = RichTrackBar.SliderShapes.Square
            Case 4
                TrackBar_Seek2.SliderShape = RichTrackBar.SliderShapes.Pentagon
            Case 5
                TrackBar_Seek2.SliderShape = RichTrackBar.SliderShapes.RoundedRectangle
        End Select

        TrackBar_Seek2.Color_BarLeft = My.Settings.seekbarBarLeft
        TrackBar_Seek2.Color_BarLeftInactive = My.Settings.seekbarBarLeftInactive
        TrackBar_Seek2.Color_BarRight = My.Settings.seekbarBarRight
        TrackBar_Seek2.Color_Border = My.Settings.seekbarBorder
        TrackBar_Seek2.Color_Slider = My.Settings.seekbarSlider
        TrackBar_Seek2.BorderThickness = My.Settings.BorderThicknessSeek
        TrackBar_Seek2.Color_Text = My.Settings.seekbarTextColor

        If My.Settings.SlidersFilledSeek Then
            TrackBar_Seek2.SliderFill = RichTrackBar.SliderFilled.Yes
        Else
            TrackBar_Seek2.SliderFill = RichTrackBar.SliderFilled.No
        End If
        If My.Settings.seekbarText Then
            TrackBar_Seek2.TextShown = RichTrackBar.ShowText.Yes
        Else
            TrackBar_Seek2.TextShown = RichTrackBar.ShowText.No
        End If


        Dim volumeLoadedSliderShape As Integer = My.Settings.volumebarShape
        Select Case volumeLoadedSliderShape
            Case 0
                TrackBar_PlayerVol2.SliderShape = RichTrackBar.SliderShapes.Circle
            Case 1
                TrackBar_PlayerVol2.SliderShape = RichTrackBar.SliderShapes.RectangleNarrow
            Case 2
                TrackBar_PlayerVol2.SliderShape = RichTrackBar.SliderShapes.RectangleWide
            Case 3
                TrackBar_PlayerVol2.SliderShape = RichTrackBar.SliderShapes.Square
            Case 4
                TrackBar_PlayerVol2.SliderShape = RichTrackBar.SliderShapes.Pentagon
            Case 5
                TrackBar_PlayerVol2.SliderShape = RichTrackBar.SliderShapes.RoundedRectangle
        End Select

        TrackBar_PlayerVol2.Color_BarLeft = My.Settings.volumebarBarLeft
        TrackBar_PlayerVol2.Color_BarLeftInactive = My.Settings.volumebarBarLeftInactive
        TrackBar_PlayerVol2.Color_BarRight = My.Settings.volumebarBarRight
        TrackBar_PlayerVol2.Color_Border = My.Settings.volumebarBorder
        TrackBar_PlayerVol2.Color_Slider = My.Settings.volumebarSlider
        TrackBar_PlayerVol2.BorderThickness = My.Settings.BorderThicknessVolume
        TrackBar_PlayerVol2.Color_Text = My.Settings.volumebarTextColor

        If My.Settings.SlidersFilledVolume Then
            TrackBar_PlayerVol2.SliderFill = RichTrackBar.SliderFilled.Yes
        Else
            TrackBar_PlayerVol2.SliderFill = RichTrackBar.SliderFilled.No
        End If
        If My.Settings.volumebarText Then
            TrackBar_PlayerVol2.TextShown = RichTrackBar.ShowText.Yes
        Else
            TrackBar_PlayerVol2.TextShown = RichTrackBar.ShowText.No
        End If

        Dim speedbarLoadedSliderShape As Integer = My.Settings.speedbarShape
        Select Case speedbarLoadedSliderShape
            Case 0
                trackBar_Speed2.SliderShape = RichTrackBar.SliderShapes.Circle
            Case 1
                trackBar_Speed2.SliderShape = RichTrackBar.SliderShapes.RectangleNarrow
            Case 2
                trackBar_Speed2.SliderShape = RichTrackBar.SliderShapes.RectangleWide
            Case 3
                trackBar_Speed2.SliderShape = RichTrackBar.SliderShapes.Square
            Case 4
                trackBar_Speed2.SliderShape = RichTrackBar.SliderShapes.Pentagon
            Case 5
                trackBar_Speed2.SliderShape = RichTrackBar.SliderShapes.RoundedRectangle
        End Select

        trackBar_Speed2.Color_BarLeft = My.Settings.speedbarBarLeft
        trackBar_Speed2.Color_BarLeftInactive = My.Settings.speedbarBarLeftInactive
        trackBar_Speed2.Color_BarRight = My.Settings.speedbarBarRight
        trackBar_Speed2.Color_Border = My.Settings.speedbarBorder
        trackBar_Speed2.Color_Slider = My.Settings.speedbarSlider
        trackBar_Speed2.BorderThickness = My.Settings.BorderThicknessSpeed
        trackBar_Speed2.Color_Text = My.Settings.speedbarTextColor

        If My.Settings.SlidersFilledSpeed Then
            trackBar_Speed2.SliderFill = RichTrackBar.SliderFilled.Yes
        Else
            trackBar_Speed2.SliderFill = RichTrackBar.SliderFilled.No
        End If
        If My.Settings.speedbarText Then
            trackBar_Speed2.TextShown = RichTrackBar.ShowText.Yes
        Else
            trackBar_Speed2.TextShown = RichTrackBar.ShowText.No
        End If

        Dim pitchbarLoadedSliderShape As Integer = My.Settings.pitchbarShape
        Select Case pitchbarLoadedSliderShape
            Case 0
                trackbar_Pitch2.SliderShape = RichTrackBar.SliderShapes.Circle
            Case 1
                trackbar_Pitch2.SliderShape = RichTrackBar.SliderShapes.RectangleNarrow
            Case 2
                trackbar_Pitch2.SliderShape = RichTrackBar.SliderShapes.RectangleWide
            Case 3
                trackbar_Pitch2.SliderShape = RichTrackBar.SliderShapes.Square
            Case 4
                trackbar_Pitch2.SliderShape = RichTrackBar.SliderShapes.Pentagon
            Case 5
                trackbar_Pitch2.SliderShape = RichTrackBar.SliderShapes.RoundedRectangle
        End Select

        trackbar_Pitch2.Color_BarLeft = My.Settings.pitchbarBarLeft
        trackbar_Pitch2.Color_BarLeftInactive = My.Settings.pitchbarBarLeftInactive
        trackbar_Pitch2.Color_BarRight = My.Settings.pitchbarBarRight
        trackbar_Pitch2.Color_Border = My.Settings.pitchbarBorder
        trackbar_Pitch2.Color_Slider = My.Settings.pitchbarSlider
        trackbar_Pitch2.BorderThickness = My.Settings.BorderThicknessPitch
        trackbar_Pitch2.Color_Text = My.Settings.pitchbarTextColor

        If My.Settings.SlidersFilledPitch Then
            trackbar_Pitch2.SliderFill = RichTrackBar.SliderFilled.Yes
        Else
            trackbar_Pitch2.SliderFill = RichTrackBar.SliderFilled.No
        End If
        If My.Settings.pitchbarText Then
            trackbar_Pitch2.TextShown = RichTrackBar.ShowText.Yes
        Else
            trackbar_Pitch2.TextShown = RichTrackBar.ShowText.No
        End If

    End Sub

#End Region

#Region " Setup Browsers"

    Public Sub SetupPandora()
        If FirstSetupPandora Then : Try
                browserPandora.Navigate("https://www.pandora.com/")
            Catch : End Try

            FirstSetupPandora = False : End If
    End Sub

    Public Sub SetupiHeartRadio()
        If FirstSetupiHeartRadio Then : Try
                browseriHeartRadio.Navigate("https://www.iheart.com/live")
            Catch : End Try

            FirstSetupiHeartRadio = False : End If
    End Sub


#End Region

#Region " Browsers Navigation Buttons"

    Private Sub pbBackNav_Click(sender As Object, e As EventArgs) Handles pbBackNav_ihr.Click
        browseriHeartRadio.GoBack()
    End Sub
    Private Sub pbForwardNav_Click(sender As Object, e As EventArgs) Handles pbForwardNav_ihr.Click
        browseriHeartRadio.GoForward()
    End Sub
    Private Sub pbRefreshNav_Click(sender As Object, e As EventArgs) Handles pbRefreshNav_ihr.Click
        browseriHeartRadio.Reload()
    End Sub

    Private Sub pbBackNav_p_Click(sender As Object, e As EventArgs) Handles pbBackNav_p.Click
        browserPandora.GoBack()
        'browserPandora.GetBrowser.GoBack()
    End Sub
    Private Sub pbForwardNav_p_Click(sender As Object, e As EventArgs) Handles pbForwardNav_p.Click
        browserPandora.GoForward()
        ' browserPandora.GetBrowser.GoForward()
    End Sub
    Private Sub pbRefreshNaV_p_Click(sender As Object, e As EventArgs) Handles pbRefreshNaV_p.Click
        browserPandora.Reload()
        ' browserPandora.GetBrowser.Reload()
    End Sub

    Private Sub pbBackNav_s_Click(sender As Object, e As EventArgs) Handles pbBackNav_s.Click
        browserSpotify.GoBack()
    End Sub
    Private Sub pbForwardNav_s_Click(sender As Object, e As EventArgs) Handles pbForwardNav_s.Click
        browserSpotify.GoForward()
    End Sub
    Private Sub pbRefreshNaV_s_Click(sender As Object, e As EventArgs) Handles pbRefreshNav_s.Click
        browserSpotify.Reload()
    End Sub


#End Region

#Region " Spotify"

    Public Sub Setup_Spotify()

        If firstsetupspotify Then
            Dim SpotfiyProc As Process
            If IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\Spotify\Spotify.exe") Then
                SpotfiyProc = Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\Spotify\Spotify.exe")
                SpotfiyProc.WaitForInputIdle()
                System.Threading.Thread.Sleep(3000)
                SetParent(SpotfiyProc.MainWindowHandle, tab_Spotify.Handle)
                System.Threading.Thread.Sleep(3000)
                SendMessage(SpotfiyProc.MainWindowHandle, WM_SYSCOMMAND, SC_MAXIMIZE, 0)
            End If
            firstsetupspotify = False

            If NowPlayingInit = False Then
                DoAdd = True
                DontSetParents = True
                tabs_Car.SelectedTabPageIndex = 1
                If firstsetupnowplaying Then
                    SetupNowPlaying()
                    SetupPlaylistsTabs()
                    RefreshHotkeys()
                    firstsetupnowplaying = False
                End If
            End If

            Form1.BarCheckBox_UseSpotifyLocal.Checked = True

        End If

    End Sub

    Private Sub pb_SpotifyOff_Click(sender As Object, e As EventArgs) Handles pb_SpotifyOff.Click
        If NowPlayingInit Then
            Select Case Form1.BarCheckBox_UseSpotifyLocal.Checked
                Case True
                    Form1.BarCheckBox_UseSpotifyLocal.Checked = False
                Case False
                    Form1.BarCheckBox_UseSpotifyLocal.Checked = True
            End Select
        End If


    End Sub


#End Region

#Region " Maps"

    Private Sub pb_Maps_Click(sender As Object, e As EventArgs) Handles pb_Maps.Click
        Process.Start("shell:Appsfolder\Microsoft.WindowsMaps_8wekyb3d8bbwe!App")
    End Sub

#End Region

#Region " WhatsApp"

    Private Sub pb_WhatsApp_Click(sender As Object, e As EventArgs) Handles pb_WhatsApp.Click
        Try
            Process.Start(IO.Path.Combine(Environment.SpecialFolder.LocalApplicationData, "WhatsApp\WhatsApp.exe"))
        Catch ex As Exception

        End Try
    End Sub

#End Region






#Region " PLAYBACK"


#Region " PLAY | PAUSE | STOP | PREV | NEXT"
#Region " Varibles for Playback Controls & Seek"
    Dim TotalTime As Long
    Dim CurTime As Long
    Dim currentIndex As Integer = 0
#End Region

    'Play
    Public Sub New_Play()
        Form1.Timer_Meta_and_Artwork.Start()

        Form1.CheckIfVideo()


        If UsingSpotify Then
            '  Form1.MySpotify.RefreshConnection()
            '  MySpotify.PlayPause()
            If SpotifyPlaying Then
                _SpotifyNew.PausePlayback()
                Form1.SetTaskbarState(TaskbarState.Paused)
            Else
                For i As Integer = 0 To _SpotifyNew.GetDevices.Devices.Count - 1
                    Try
                        If _SpotifyNew.GetDevices.Devices(i).IsActive Then

                            _SpotifyNew.ResumePlayback(_SpotifyNew.GetDevices.Devices(i).Id, _SpotifyNew.GetPlayback.Item.Id, Nothing, 0)
                        End If
                    Catch ex As Exception
                    End Try
                Next

                Form1.SetTaskbarState(TaskbarState.Playing)
            End If

        Else
            If IsVideo = False Then

                VlcPlayer.playlist.stop() 'VlcPlayer.Playlist.Stop() 'VlcPlayer.playlist.stop()
                Form1.VLCclearPlaylists() : ResetVLC()
                Dim currentAction As BASSActive = AudioPlayer.Instance.GetStreamStatus()

                Select Case currentAction
                    Case BASSActive.BASS_ACTIVE_PLAYING     'Was Playing
                        AudioPlayer.Instance.Pause()
                        Exit Select

                    Case BASSActive.BASS_ACTIVE_PAUSED      'Was Paused
                        UpdatePitchByCurrentValue()

                        AudioPlayer.Instance.Play(False)
                        UpdatePitchByCurrentValue()
                        Form1.Bass_InitEQ()
                        Exit Select

                    Case Else                               'Was Stopped
                        If AudioPlayer.Instance.TrackList.Tracks.Count > 0 Then
                            If AudioPlayer.Instance.CurrentTrack Is Nothing Then
                                AudioPlayer.Instance.CurrentTrackIndex = 0
                            End If
                            AudioPlayer.Instance.Play(True)
                            UpdatePitchByCurrentValue()
                            UpdateSpeedByCurrentValue()

                            AudioPlayer.Instance.Pause()
                            AudioPlayer.Instance.Play(False)

                            trackbar_Pitch2.Enabled = True
                            But_PitchDown.Enabled = True
                            But_PitchUp.Enabled = True
                            But_PitchDown.Visible = True
                            But_PitchUp.Visible = True
                            trackBar_Speed2.Enabled = True
                            Form1.SongStartOver = True
                            Form1.Bass_InitEQ()
                        End If
                        If Form1.InitiatePlayOnStart = False Then
                            Try
                                AudioPlayer.Instance.Position = My.Settings.LastPlayedPositions.Item(PlaylistTabs.SelectedTabPageIndex)
                            Catch ex As Exception
                            End Try

                            Form1.InitiatePlayOnStart = True
                        End If
                        If Form1.InitiatePlayOnStartCurrent = False Then
                            Try
                                AudioPlayer.Instance.Position = My.Settings.LastPlayedPositions.Item(PlaylistTabs.SelectedTabPageIndex)
                            Catch ex As Exception
                            End Try

                            Form1.InitiatePlayOnStartCurrent = True
                        End If
                        Exit Select
                End Select
            Else
                VlcPlayer.playlist.stop() 'VlcPlayer.playlist.stop()
                Form1.VLCclearPlaylists() : ResetVLC()
                Form1.isVLCplaying = False
                If Form1.InitiatePlayOnStart = True Then
                    PlaySub()
                End If

            End If
        End If
    End Sub
    'Double Click to PLAY
    Public Sub playlistboxedit_MouseDoubleClick(sender As Object, e As MouseEventArgs)
        Form1.BarCheckBox_UseSpotifyold.Checked = False
        Form1.BarCheckBox_YoutubeUse.Checked = False

        DoubleClickPlay()
    End Sub
    Public Sub DoubleClickPlay()
        Form1.InitiatePlayOnStart = True
        Dim Playlist As GridPlaylist
        Dim Scroller As Scroller
        For Each c As Control In PlaylistTabs.SelectedTabPage.Controls
            If c.GetType Is GetType(Scroller) Then
                Scroller = c
                For Each c2 As Control In Scroller.Controls
                    If c2.GetType Is GetType(GridPlaylist) Then
                        Playlist = c2
                    End If
                Next
            End If
        Next

        If Playlist.CurrentCell.RowIndex >= 0 AndAlso Playlist.CurrentCell.ColumnIndex >= 0 Then
            Dim row As Integer
            For i As Integer = 0 To Playlist.SelectedCells.Count - 1
                row = Playlist.SelectedCells.Item(i).RowIndex
            Next

            Dim SongFilename As String = Playlist.Item(6, row).Value.ToString

            Form1.Timer_Seek.Stop()
            Form1.Timer_Meta_and_Artwork.Stop()
            AudioPlayer.Instance.ResetTrackList()
            Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
            AudioPlayer.Instance.TrackList.Tracks.Add(track)
            If AudioPlayer.Instance.TrackList.Tracks.Count > 0 Then
                If AudioPlayer.Instance.CurrentTrack Is Nothing Then
                    AudioPlayer.Instance.CurrentTrackIndex = 0
                End If
                New_Play()
                Form1.SongStartOver = True
            End If
            Form1.Timer_Meta_and_Artwork.Start() ' Set Artwork
            Form1.Timer_Seek.Start()
        End If
    End Sub
    Public Sub GridPlaylist_MouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs)
        Form1.InitiatePlayOnStart = True
        Dim Playlist As GridPlaylist
        Dim Scroller As Scroller
        For Each c As Control In PlaylistTabs.SelectedTabPage.Controls
            If c.GetType Is GetType(Scroller) Then
                Scroller = c
                For Each c2 As Control In Scroller.Controls
                    If c2.GetType Is GetType(GridPlaylist) Then
                        Playlist = c2
                    End If
                Next
            End If

        Next

        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Dim row As Integer
            For i As Integer = 0 To Playlist.SelectedCells.Count - 1
                row = Playlist.SelectedCells.Item(i).RowIndex
            Next

            Dim SongFilename As String = Playlist.Item(6, row).Value.ToString

            Form1.Timer_Seek.Stop()
            Form1.Timer_Meta_and_Artwork.Stop()

            AudioPlayer.Instance.ResetTrackList()
            Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
            AudioPlayer.Instance.TrackList.Tracks.Add(track)

            If AudioPlayer.Instance.TrackList.Tracks.Count > 0 Then
                If AudioPlayer.Instance.CurrentTrack Is Nothing Then
                    AudioPlayer.Instance.CurrentTrackIndex = 0
                End If
                New_Play()
                Form1.SongStartOver = True
            End If


            Form1.Timer_Meta_and_Artwork.Start() ' Set Artwork
            Form1.Timer_Seek.Start()

        End If
    End Sub

    'VLC Play
    Public Sub PlaySub()
        Form1.Timer_Meta_and_Artwork.Start()

        AudioPlayer.Instance.Stop()
        AudioPlayer.Instance.ResetTrackList()


        If UsingSpotify Then
            MySpotify.RefreshConnection()
            MySpotify.PlayPause()
        Else
            If VlcPlayer.playlist.isPlaying = True Then 'If VlcPlayer.playlist.isPlaying = True Then
                VlcPlayer.playlist.togglePause() 'VlcPlayer.playlist.togglePause()
                Form1.isVLCplaying = True
                Try
                    Form1.SetTaskbarState("Paused")
                Catch
                End Try

                But_PlayPause.BackgroundImage = Form1.PlayImage

            Else
                If Form1.isVLCplaying Then
                    VlcPlayer.playlist.play() 'VlcPlayer.playlist.play()
                    Form1.isVLCplaying = True
                    Try
                        Form1.SetTaskbarState("Normal")
                    Catch
                    End Try
                    But_PlayPause.BackgroundImage = Form1.PauseImage
                Else
                    Form1.isVLCplaying = False
                    VLC_Play()
                    VlcPlayer.playlist.play() 'VlcPlayer.playlist.play()
                    Form1.isVLCplaying = True
                    Try
                        Form1.SetTaskbarState("Normal")
                    Catch
                    End Try
                    But_PlayPause.BackgroundImage = Form1.PauseImage
                End If
                If Form1.InitiatePlayOnStart = False Then
                    VlcPlayer.input.position = My.Settings.LastPlayedPositions.Item(PlaylistTabs.SelectedTabPageIndex) 'VlcPlayer.Input.Position 'VlcPlayer.Input.Position = My.Settings.LastPlayedPositions.Item(PlaylistTabs.SelectedTabPageIndex)
                    Form1.InitiatePlayOnStart = True
                End If
                If Form1.InitiatePlayOnStartCurrent = False Then
                    VlcPlayer.input.position = My.Settings.LastPlayedPositions.Item(PlaylistTabs.SelectedTabPageIndex) 'VlcPlayer.Input.Position 'VlcPlayer.Input.Position = My.Settings.LastPlayedPositions.Item(PlaylistTabs.SelectedTabPageIndex)
                    Form1.InitiatePlayOnStartCurrent = True
                End If

            End If
            Form1.ReApplySubtitles()
        End If

    End Sub
    Public Sub VLC_Play()
        Form1.BarCheckBox_UseSpotifyold.Checked = False


        Dim Playlist As GridPlaylist
        Dim Scroller As Scroller
        For Each c As Control In PlaylistTabs.SelectedTabPage.Controls
            If c.GetType Is GetType(Scroller) Then
                Scroller = c
                For Each c2 As Control In Scroller.Controls
                    If c2.GetType Is GetType(GridPlaylist) Then
                        Playlist = c2

                        Form1.Timer_Seek.Stop()
                        Form1.Timer_Meta_and_Artwork.Stop()

                        Form1.VLCclearPlaylists() : ResetVLC()

                        Dim row As Integer

                        For i As Integer = 0 To Playlist.SelectedCells.Count - 1
                            row = Playlist.SelectedCells.Item(i).RowIndex
                        Next
                        Dim SongFilename As String = Playlist.Item(6, row).Value.ToString
                        VlcPlayer.playlist.add("file:///" & SongFilename)
                        ' VlcPlayer.SetMedia(SongFilename)

                        VlcPlayer.playlist.play() 'VlcPlayer.playlist.play()

                        Form1.Timer_Seek.Start()
                        Form1.Timer_Meta_and_Artwork.Start() ' Set Artwork

                        Return
                    End If
                Next
            End If

        Next


    End Sub
    'Youtube Play
    Public Sub PlayYoutube()
        If VlcPlayer.playlist.isPlaying = True Then 'If VlcPlayer.playlist.isPlaying = True Then
            VlcPlayer.playlist.togglePause() 'VlcPlayer.playlist.togglePause()
            Form1.isVLCplaying = True
            Try
                Form1.SetTaskbarState("Paused")
            Catch
            End Try

            But_PlayPause.BackgroundImage = Form1.PlayImage

        Else
            If Form1.isVLCplaying Then
                VlcPlayer.playlist.play() 'VlcPlayer.playlist.play()
                Form1.isVLCplaying = True
                Try
                    Form1.SetTaskbarState("Normal")
                Catch
                End Try
                But_PlayPause.BackgroundImage = Form1.PauseImage
            Else
                Form1.isVLCplaying = False
                Form1.Timer_Seek.Stop()
                Form1.Timer_Meta_and_Artwork.Stop()
                VlcPlayer.playlist.stop() 'VlcPlayer.playlist.stop()
                Form1.VLCclearPlaylists() : ResetVLC()

                Try
                    Form1.SetTaskbarState("Normal")
                Catch
                End Try
                But_PlayPause.BackgroundImage = Form1.PauseImage
            End If
            Form1.isVLCplaying = False
        End If

    End Sub

    Public Sub PlayALL()
        Form1.CheckIfVideo()
        If IsVideo = False Then
            If AudioPlayer.Instance.TrackList.Tracks.Count > 0 Then
                New_Play()
            End If
        Else
            PlaySub()
        End If
    End Sub

    'Folder
    Dim firstplay As Boolean = True
    Public Sub PlayHotkeySub()
        If IsVideo = False Then
            If AudioPlayer.Instance.TrackList.Tracks.Count > 0 Then
                New_Play()
            End If
        Else
            PlaySub()
        End If

    End Sub


    Public Sub ResetVLC()


        Form1.VlcPlayer.Dock = DockStyle.Fill
        Form1.VlcPlayer.Location = New System.Drawing.Point(5, 193)
        Form1.VlcPlayer.MinimumSize = New System.Drawing.Size(143, 143)
        Form1.VlcPlayer.Size = New System.Drawing.Size(978, 59)
    End Sub


    'Stop
    Public Sub stopbut2_Click(sender As Object, e As EventArgs) Handles But_Stop.Click
        StopSub()
        SwipingUp = False
        AddingFile = False
    End Sub
    Public Sub Artwork_TwoFingerTapGesture(sender As Object, e As Telerik.WinControls.GestureEventArgs) 'Handles Artwork.TwoFingerTapGesture
        If e.GestureType = Telerik.WinControls.GestureType.TwoFingerTap Then
            If AudioPlayer.Instance.TrackList.Tracks.Count = 0 Then
                StopSub()
                If Not AudioPlayer.Instance.GetStreamStatus = BASSActive.BASS_ACTIVE_PLAYING Then
                    But_PlayPause.BackgroundImage = Form1.PlayImage
                Else
                    But_PlayPause.BackgroundImage = Form1.PauseImage
                End If
            End If
        End If

    End Sub
    Public Sub StopSub()
        If UsingSpotify Then
            _SpotifyNew.PausePlayback()
        Else
            If IsVideo = False Then
                If Not AudioPlayer.Instance.TrackList.Tracks.Count = 0 Then
                    AudioPlayer.Instance.Stop()
                    If Not AudioPlayer.Instance.GetStreamStatus = BASSActive.BASS_ACTIVE_PLAYING Then
                        But_PlayPause.BackgroundImage = Form1.PlayImage
                    Else
                        But_PlayPause.BackgroundImage = Form1.PauseImage
                    End If
                End If

            Else
                If VlcPlayer.playlist() IsNot Nothing Then 'If VlcPlayer.playlist IsNot Nothing Then
                    VlcPlayer.playlist.stop() 'VlcPlayer.playlist.stop()
                    Form1.isVLCplaying = False
                    If VlcPlayer.playlist.isPlaying = False Then ' If VlcPlayer.playlist.isPlaying = False Then
                        But_PlayPause.BackgroundImage = Form1.PlayImage
                    Else
                        But_PlayPause.BackgroundImage = Form1.PauseImage
                    End If
                End If
            End If
        End If
    End Sub

    'Next   
    Public Sub NextBut_MouseUp(sender As Object, e As MouseEventArgs) Handles But_Next.MouseUp
        If e.Button = MouseButtons.Left Then

            If UsingSpotify Then
                NextItem()
            Else
                If NextButhover Then
                    '  form1.CheckIfVideo()
                    If IsVideo Then
                        If VlcPlayer.input.chapter.count > 1 Then
                            'If VlcPlayer.Input.Chapter.count > 1 Then ' If VlcPlayer.Input.Chapter. 'VlcPlayer.Input.Chapter.count > 1 Then
                            If VlcPlayer.input.chapter.Current = VlcPlayer.input.chapter.count - 1 Then
                                ' If VlcPlayer.Input.Chapter.Current= VlcPlayer.Input.Chapter.count - 1 Then 'If VlcPlayer.Input.Chapter. 'VlcPlayer.Input.Chapter.track = VlcPlayer.Input.Chapter. 'VlcPlayer.Input.Chapter.count - 1 Then
                                NextItem()
                            Else
                                VlcPlayer.input.chapter.next()
                                ' VlcPlayer.Input.Chapter. 'VlcPlayer.Input.Chapter.next()

                                ' VLCChapterMarks.SelectedIndex = VlcPlayer.Input.Chapter. 'VlcPlayer.Input.Chapter.track
                                VLCChapterMarks.SelectedIndex = VlcPlayer.input.chapter.Current
                            End If
                        Else
                            NextItem()
                        End If
                    Else
                        NextItem()
                    End If
                End If

            End If

            '   Next Item
        ElseIf e.Button = MouseButtons.Right Then
            SkipForward5Secs()

        End If
        If NextButhover = True Then
            If NextButdisabled = False Then
                But_Next.BackgroundImage = NextHoverImage
                ' ArtworkNextBut.BackgroundImage = RightHoverImage
            End If
        Else
            If NextButdisabled = False Then
                But_Next.BackgroundImage = NextImage
                ' ArtworkNextBut.BackgroundImage = RightImage
            End If
        End If
    End Sub
    Public Sub NextItem()
        If UsingSpotify Then
            _SpotifyNew.SkipPlaybackToNext()
            Exit Sub


            'Playlists
        Else
            Dim Playlist As GridPlaylist
            Dim Scroller As Scroller
            For Each c As Control In PlaylistTabs.SelectedTabPage.Controls
                If c.GetType Is GetType(Scroller) Then
                    Scroller = c
                    For Each c2 As Control In Scroller.Controls
                        If c2.GetType Is GetType(GridPlaylist) Then
                            Playlist = c2
                        End If
                    Next
                End If

            Next

            Dim Row As Integer = Playlist.CurrentCell.RowIndex
            Dim RowCount As Integer = Playlist.RowCount
            'Dim SongFilename As String = Playlist.Item(6, row).Value.ToString

            If Playlist.RowCount > 0 Then
                If IsShuffle Then
                    Dim random As New System.Random
                    Dim Index As System.Int32 = random.Next(0, Playlist.RowCount - 1)
                    Row = Index
                    '     Playlist.CurrentCell = Playlist(0, Row)
                    Playlist.CurrentCell = Playlist(0, Row)
                    Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString

                    Form1.CheckIfVideo()
                    If IsVideo Then
                        Form1.VLCclearPlaylists() : ResetVLC()
                        VlcPlayer.playlist.add("file:///" & SongFilename)
                        VlcPlayer.playlist.play() 'VlcPlayer.playlist.play()
                        New_Play()
                        Form1.Timer_Meta_and_Artwork.Start()
                    Else
                        AudioPlayer.Instance.ResetTrackList()
                        Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                        AudioPlayer.Instance.TrackList.Tracks.Add(track)
                        New_Play()
                        Form1.Timer_Meta_and_Artwork.Start()

                    End If
                Else

                    Form1.Timer_Seek.Stop()
                    If Row = RowCount - 1 Then
                        If Form1.repeat Then
                            Row = 0
                            Playlist.CurrentCell = Playlist(0, Row)
                        End If
                    Else
                        Row += 1
                        Playlist.CurrentCell = Playlist(0, Row)
                    End If
                    Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                    Form1.CheckIfVideo()
                    If IsVideo Then
                        Form1.VLCclearPlaylists() : ResetVLC()
                        VlcPlayer.playlist.add("file:///" & SongFilename)
                        VlcPlayer.playlist.play() 'VlcPlayer.playlist.play()
                        New_Play()
                        Form1.Timer_Meta_and_Artwork.Start()
                    Else
                        AudioPlayer.Instance.ResetTrackList()
                        Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                        AudioPlayer.Instance.TrackList.Tracks.Add(track)
                        New_Play()
                        Form1.Timer_Meta_and_Artwork.Start()

                    End If



                End If
                Form1.Timer_Seek.Start()
                Reset_AB_Repeat()

                Return
            End If

        End If
        Reset_AB_Repeat()
    End Sub
    Public Sub SkipForward5Secs()
        If UsingSpotify Then
            MySpotify.PlayForward()
        Else
            If NextButhover Then
                If IsVideo = False Then
                    AudioPlayer.Instance.Position += CLng(CDbl(AudioPlayer.Instance.CurrentAudioHandle.GetElapsedTime) + 1600000)  'CLng(35000 * (AudioPlayer.Instance.CurrentAudioHandle.LengthInSeconds / 5))
                Else
                    ' VlcPlayer.input.time = VlcPlayer.input.time + 5000
                    VlcPlayer.input.time = VlcPlayer.input.time + 5000
                End If
            End If
            'Skip 5 secs
        End If
    End Sub
    Public Sub NextHotkeySub()
        If UsingSpotify Then
            NextItem()
        Else
            If IsVideo Then
                If VlcPlayer.input.chapter.count > 1 Then
                    'If VlcPlayer.Input.Chapter.count > 1 Then ' If VlcPlayer.Input.Chapter. 'VlcPlayer.Input.Chapter.count > 1 Then
                    If VlcPlayer.input.chapter.Current = VlcPlayer.input.chapter.count - 1 Then
                        ' If VlcPlayer.Input.Chapter.Current= VlcPlayer.Input.Chapter.count - 1 Then 'If VlcPlayer.Input.Chapter. 'VlcPlayer.Input.Chapter.track = VlcPlayer.Input.Chapter. 'VlcPlayer.Input.Chapter.count - 1 Then
                        NextItem()
                    Else
                        VlcPlayer.input.chapter.next()
                        ' VlcPlayer.Input.Chapter. 'VlcPlayer.Input.Chapter.next()


                        ' VLCChapterMarks.SelectedIndex = VlcPlayer.Input.Chapter. 'VlcPlayer.Input.Chapter.track
                        VLCChapterMarks.SelectedIndex = VlcPlayer.input.chapter.Current
                    End If
                Else
                    NextItem()
                End If
            Else
                NextItem()
            End If
        End If
    End Sub

    'Previous     
    Public Sub prevrwbut2_MouseUp(sender As Object, e As MouseEventArgs) Handles But_Previous.MouseUp

        If e.Button = MouseButtons.Left Then
            If UsingSpotify Then
                PrevItem()
            Else

                If Prevrwhover Then
                    ' form1.CheckIfVideo()
                    If IsVideo Then
                        If VlcPlayer.input.chapter.count > 1 Then
                            'If VlcPlayer.Input.Chapter.count > 1 Then ' If VlcPlayer.Input.Chapter. 'VlcPlayer.Input.Chapter.count > 1 Then
                            If VlcPlayer.input.chapter.Current = 0 Then
                                'If VlcPlayer.Input.Chapter. 'VlcPlayer.Input.Chapter.track = 0 Then
                                PrevItem()
                            Else
                                VlcPlayer.input.chapter.Previous()
                                VLCChapterMarks.SelectedIndex = VlcPlayer.input.chapter.Current
                                ' VlcPlayer.Input.Chapter. 'VlcPlayer.Input.Chapter.prev()
                                'VLCChapterMarks.SelectedIndex = VlcPlayer.Input.Chapter. 'VlcPlayer.Input.Chapter.track
                            End If

                        Else
                            PrevItem()
                        End If



                    Else

                        PrevItem()
                    End If
                End If
            End If
        ElseIf e.Button = MouseButtons.Right Then
            SkipBackwards5Secs()
        End If

        'Graphics
        If Prevrwhover = True Then
            If Prevrwdisabled = False Then
                But_Previous.BackgroundImage = PreviousHoverImage
                ' ArtworkPrevBut.BackgroundImage = LeftHoverImage
            End If
        Else
            If Prevrwdisabled = False Then
                But_Previous.BackgroundImage = PreviousImage
                ' ArtworkPrevBut.BackgroundImage = LeftImage
            End If
        End If
    End Sub
    Public Sub PrevItem()

        If UsingSpotify Then
            '   MySpotify.RefreshConnection()
            '  MySpotify.PlayPrev()
            _SpotifyNew.SkipPlaybackToPrevious()
            Exit Sub
        Else
            Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next

            Dim Row As Integer = Playlist.CurrentCell.RowIndex
            Dim RowCount As Integer = Playlist.RowCount


            If Playlist.RowCount > 0 Then
                If IsShuffle Then
                    Dim random As New System.Random
                    Dim Index As System.Int32 = random.Next(0, Playlist.RowCount - 1)
                    Row = Index
                    Playlist.CurrentCell = Playlist(0, Row)
                    Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                    AudioPlayer.Instance.ResetTrackList()
                    Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                    AudioPlayer.Instance.TrackList.Tracks.Add(track)
                    New_Play()
                    Form1.Timer_Meta_and_Artwork.Start()
                Else
                    If Row < 0 Then
                        New_Play()
                        Form1.Timer_Meta_and_Artwork.Start()
                    ElseIf Row <> 0 Then
                        Row -= 1
                        Playlist.CurrentCell = Playlist(0, Row)
                        Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                        AudioPlayer.Instance.ResetTrackList()
                        Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                        AudioPlayer.Instance.TrackList.Tracks.Add(track)
                        New_Play()
                        Form1.Timer_Meta_and_Artwork.Start()
                    ElseIf repeatAll = True Or repeatOne = True Then
                        If Row = 0 Then
                            Row = RowCount - 1
                            Playlist.CurrentCell = Playlist(0, Row)
                            Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                            AudioPlayer.Instance.ResetTrackList()
                            Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                            AudioPlayer.Instance.TrackList.Tracks.Add(track)
                            New_Play()
                            Form1.Timer_Meta_and_Artwork.Start()
                        End If
                    End If

                End If

                Return
            End If


        End If
    End Sub
    Public Sub SkipBackwards5Secs()
        If UsingSpotify Then
            MySpotify.PlayBackwards()
        Else
            If Prevrwhover Then
                If IsVideo = False Then
                    If AudioPlayer.Instance.Position >= 1600000 Then
                        AudioPlayer.Instance.Position += CLng(CDbl(AudioPlayer.Instance.CurrentAudioHandle.GetElapsedTime) - 1600000) 'CLng(35000 * (AudioPlayer.Instance.CurrentAudioHandle.LengthInSeconds / 5))
                    Else
                        AudioPlayer.Instance.Position = 0
                    End If

                Else
                    ' If VlcPlayer.input.time > 5000 Then
                    'VlcPlayer.input.time = VlcPlayer.input.time - 5000
                    ' Else
                    ' VlcPlayer.input.time = 0
                    ' End If
                    If VlcPlayer.input.time > 5000 Then
                        VlcPlayer.input.time = VlcPlayer.input.time - 5000
                    Else
                        VlcPlayer.input.time = 0
                    End If
                End If

            End If
        End If
    End Sub
    Public Sub PrevHotkeySub()

        If UsingSpotify Then
            PrevItem()
        Else
            If IsVideo Then

                If VlcPlayer.input.chapter.count > 1 Then
                    VlcPlayer.input.chapter.Previous()
                    VLCChapterMarks.SelectedIndex = VlcPlayer.input.chapter.Current
                    ' If VlcPlayer.Input.Chapter.count > 1 Then ' If VlcPlayer.Input.Chapter. 'VlcPlayer.Input.Chapter.count > 1 Then
                    'VlcPlayer.Input.Chapter. 'VlcPlayer.Input.Chapter.prev()
                    ' VLCChapterMarks.SelectedIndex = VlcPlayer.Input.Chapter. 'VlcPlayer.Input.Chapter.track
                Else
                    PrevItem()
                End If
            Else
                PrevItem()
            End If
        End If

    End Sub


    'Key Presses
    Public Sub MyBaseKeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        Select Case e.KeyCode
            Case Keys.Space
                If e.Control Then
                    StopSub()
                Else
                    If UseSpaceBar = False Then Return
                    '.................Play / Pause.................
                    If IsVideo Then
                        PlaySub()
                    Else
                        New_Play()
                    End If
                End If

            Case Keys.Left
                If e.Control Then
                    '.................Skip Backwards.................
                    Prevrwhover = True
                    SkipBackwards5Secs()
                    Prevrwhover = False
                Else
                    If UseSpaceBar = False Then Return
                    '..............Previous.................
                    If UsingSpotify Then
                        PrevItem()
                    Else
                        If IsVideo Then
                            If VlcPlayer.input.chapter.count > 1 Then ' If VlcPlayer.Input.Chapter.count > 1 Then ' If VlcPlayer.Input.Chapter. 'VlcPlayer.Input.Chapter.count > 1 Then
                                VlcPlayer.input.chapter.Previous() 'VlcPlayer.Input.Chapter.prev()
                                VLCChapterMarks.SelectedIndex = VlcPlayer.input.chapter.Current 'VlcPlayer.Input.Chapter.track
                            Else
                                PrevItem()
                            End If
                        Else
                            PrevItem()
                        End If
                    End If
                End If

            Case Keys.Right
                If e.Control Then
                    '.................Skip Forward.................
                    NextButhover = True
                    SkipForward5Secs()
                    NextButhover = False
                Else
                    If UseSpaceBar = False Then Return
                    '.................Skip.................
                    If UsingSpotify Then
                        NextItem()
                    Else
                        If IsVideo Then
                            If VlcPlayer.input.chapter.count > 1 Then ' If VlcPlayer.Input.Chapter. 'VlcPlayer.Input.Chapter.count > 1 Then
                                If VlcPlayer.input.chapter.Current = VlcPlayer.input.chapter.count - 1 Then 'If VlcPlayer.Input.Chapter. 'VlcPlayer.Input.Chapter.track = VlcPlayer.Input.Chapter. 'VlcPlayer.Input.Chapter.count - 1 Then
                                    NextItem()
                                Else
                                    VlcPlayer.input.chapter.next() 'VlcPlayer.Input.Chapter.next()
                                    VLCChapterMarks.SelectedIndex = VlcPlayer.input.chapter.Current 'VlcPlayer.Input.Chapter.track
                                End If
                            Else
                                NextItem()
                            End If
                        Else
                            NextItem()
                        End If
                    End If
                End If

            Case Keys.L
                If e.Control AndAlso e.Shift Then
                    Me.Location = New Point(0, 0)
                End If
        End Select
    End Sub


#End Region '                     <<<<<<<<<<<<                          CONTROLS                                                        >>>>>>>>>>>>


#Region " SEEK"
    Dim firsttime_TimeLabel As Boolean = True


    'VLC
    Public Sub VLCplayer_MediaPlayerPositionChanged(sender As Object, e As DVLCEvents_MediaPlayerPositionChangedEvent) ' Handles VlcPlayer.MediaPlayerPositionChanged
        If VLCChapterMarks.Visible = True Then
            If VLCChapterMarks_IsClicking = False Then
                VLCChapterMarks.SelectedIndex = VlcPlayer.input.chapter.Current 'VlcPlayer.Input.Chapter.track
            End If

        End If

        If VLCmediaOpened Then
            VLCTimerSeekCode()
        End If
        Try
            '//////////////Taskbar///////////////
            'ProgressBar
            Dim pos As Double = VlcPlayer.input.position 'VlcPlayer.Input.Position
            If My.Settings.DriveMode Then
                Dim hWnd As IntPtr = FindWindow(Nothing, Me.Text)
                Taskbar.TaskbarManager.Instance.SetProgressValue(pos * 100, 100, hWnd)
            Else
                Taskbar.TaskbarManager.Instance.SetProgressValue(pos * 100, 100)
            End If

            If Not VlcPlayer.playlist.isPlaying = True Then 'If Not VlcPlayer.playlist.isPlaying = True Then
                Form1.SetTaskbarState("Paused")
            Else
                Form1.SetTaskbarState("Normal")
            End If
            '/////////////End Taskbar///////////
        Catch
        End Try
    End Sub
    Public Sub VLCTimerSeekCode()
        Try
            If SeekBarDown Then Return
            If VlcPlayer.playlist.isPlaying = False Then Return ' If VlcPlayer.playlist.isPlaying = False Then Return
            If Form1.isVLCplaying = False Then Return

            ' Dim total As TimeSpan = TimeSpan.FromMilliseconds(VlcPlayer.Input.Length)
            Dim total As TimeSpan = TimeSpan.FromMilliseconds(VlcPlayer.input.length)
            Dim pos As Double = VlcPlayer.input.position 'VlcPlayer.Input.Position


            Setup_Time_LabelsVLC()
            Try

                TrackBar_Seek2.Maximum = SeekBarMaxVal
                TrackBar_Seek2.Value = pos * SeekBarMaxVal


            Catch

            End Try

            Try
                If VlcPlayer.input.position > 0.999999 Then 'VlcPlayer.Input.Position > 0.999999 Then
                    SubtitleMenu_Clear()
                    Form1.AudioMenu_Clear()
                End If
                If VlcPlayer.input.position = 0 Then 'VlcPlayer.Input.Position = 0 Then
                    SubtitleMenu_Clear()
                    Form1.AudioMenu_Clear()
                End If
                If VlcPlayer.input.position > 0 Then 'VlcPlayer.Input.Position > 0 Then
                    If SubtitlesAdded Then Return

                    RefreshSubtitles()
                    Form1.RefreshAudios()

                    SubtitlesAdded = True
                    AudiosAdded = True
                End If
            Catch

            End Try


            Try
                '//////////////Taskbar///////////////
                'ProgressBar
                If My.Settings.DriveMode Then
                    Dim hWnd As IntPtr = FindWindow(Nothing, Me.Text)
                    Taskbar.TaskbarManager.Instance.SetProgressValue(pos * 100, 100, hWnd)
                Else
                    Taskbar.TaskbarManager.Instance.SetProgressValue(pos * 100, 100)
                End If

                If Not VlcPlayer.playlist.isPlaying = True Then 'If Not VlcPlayer.playlist.isPlaying = True Then
                    Form1.SetTaskbarState("Paused")
                Else
                    Form1.SetTaskbarState("Normal")
                End If
                '/////////////End Taskbar///////////
            Catch
            End Try
        Catch

        End Try
    End Sub
    Public Sub Setup_Time_LabelsVLC()
        'Time Labels
        Dim total As TimeSpan = TimeSpan.FromMilliseconds(VlcPlayer.input.length)

        If total.Hours >= 1 Then
            timelabel.Text = TimeSpan.FromMilliseconds(VlcPlayer.input.time).ToString().Substring(0, 8)
            totaltimelabel.Text = TimeSpan.FromMilliseconds(VlcPlayer.input.length).ToString().Substring(0, 8)

        Else
            timelabel.Text = TimeSpan.FromMilliseconds(VlcPlayer.input.time).ToString().Substring(3, 5)
            totaltimelabel.Text = TimeSpan.FromMilliseconds(VlcPlayer.input.length).ToString().Substring(3, 5)
        End If
    End Sub

    'BASS.NET
    Public Sub TimerSeekCode()
        If IsVideo = False Then
            If SeekBarDown Then Return

            Setup_Time_Labels()

            If AudioPlayer.Instance.Position > 0.999999 Then
                SubtitleMenu_Clear()
                Form1.AudioMenu_Clear()
            End If
            If AudioPlayer.Instance.Position = 0 Then
                SubtitleMenu_Clear()
                Form1.AudioMenu_Clear()
            End If
            If AudioPlayer.Instance.Position > 0 Then
                If SubtitlesAdded Then Return
                RefreshSubtitles()
                Form1.RefreshAudios()
                SubtitlesAdded = True
                AudiosAdded = True
            End If
        Else
            VLCTimerSeekCode()
        End If
    End Sub
    Public Sub Setup_Time_Labels()
        'Time Labels
        Dim current As String = AudioPlayer.Instance.GetElapsedTime
        Dim total As String = AudioPlayer.Instance.GetTotalTime

        timelabel.Text = current
        totaltimelabel.Text = total

    End Sub
    Public Function Getseekbar(pos As Long, len As Long) As Short
        Dim val As Short = CShort(pos * SeekBarMaxVal \ len)
        Return val
    End Function
    Public Function GetTrackPosition(barpos As Short, len As Long) As Long
        Dim pos As Long = CLng(len * barpos / 100)
        Return pos
    End Function

    'Trackbar
    Dim SeekBarDown As Boolean = False
    Public Sub seekbar_MouseDown(sender As Object, e As MouseEventArgs) Handles TrackBar_Seek2.MouseDown, TrackBar_Seek2.MouseMove
        If sender.name = "TrackBar_Seek2" Then
            Try

                If IsVideo = False Then
                    If e.Button = MouseButtons.Left Then
                        timerUpdate.Stop()
                        Form1.Timer_Meta_and_Artwork.Stop()
                        'Move to mouse position
                        Dim dblValue As Double

                        If TrackBar_Seek2.Value > (SeekBarMaxVal / 2) Then
                            dblValue = (Convert.ToDouble(e.X) / Convert.ToDouble(TrackBar_Seek2.Width - 10)) * (TrackBar_Seek2.Maximum)
                        Else
                            dblValue = (Convert.ToDouble(e.X) / Convert.ToDouble(TrackBar_Seek2.Width)) * (TrackBar_Seek2.Maximum)
                        End If

                        Try
                            TrackBar_Seek2.Value = Convert.ToInt32(dblValue)
                        Catch ex As Exception
                            If ex.ToString.Contains("System.OverflowException") Then
                                TrackBar_Seek2.Value = SeekBarMaxVal
                            End If
                        End Try
                        SeekBarDown = True
                    End If
                Else

                    If e.Button = MouseButtons.Left Then

                        Form1.Timer_Seek.Stop()
                        Form1.Timer_Meta_and_Artwork.Stop()
                        'Move to mouse position
                        Dim dblValue As Double

                        If TrackBar_Seek2.Value > (SeekBarMaxVal / 2) Then
                            dblValue = (Convert.ToDouble(e.X) / Convert.ToDouble(TrackBar_Seek2.Width - 10)) * (TrackBar_Seek2.Maximum)
                        Else
                            dblValue = (Convert.ToDouble(e.X) / Convert.ToDouble(TrackBar_Seek2.Width)) * (TrackBar_Seek2.Maximum)
                        End If

                        Try
                            TrackBar_Seek2.Value = Convert.ToInt32(dblValue)
                        Catch ex As Exception
                            If ex.ToString.Contains("System.OverflowException") Then
                                TrackBar_Seek2.Value = SeekBarMaxVal
                            End If
                        End Try

                        SeekBarDown = True
                    End If
                End If
            Catch
            End Try
        End If


    End Sub
    Public Sub seekbar_MouseUp(sender As Object, e As MouseEventArgs) Handles TrackBar_Seek2.MouseUp
        If sender.name = "TrackBar_Seek2" Then
            If e.Button = MouseButtons.Right Then
                Form1.CustomizeSliders()

                Return
            End If

            If IsVideo = False Then
                If SeekBarDown = True Then
                    If waveForm Is Nothing Then
                        Return
                    End If
                    ghostCursorPosition = -1

                    If e.Button = MouseButtons.Left Then
                        Dim pos As Long = (TrackBar_Seek2.Value / (SeekBarMaxVal)) * AudioPlayer.Instance.CurrentAudioHandle.LengthInBytes  'waveForm.GetBytePositionFromX(e.X, Me.SeekBar.Width, -1, -1)
                        Bass.BASS_ChannelSetPosition(AudioPlayer.Instance.CurrentAudioHandle.CurrentHandle, pos)

                    End If
                    timerUpdate.Start()
                End If
                SeekBarDown = False

            Else
                If SeekBarDown = True Then
                    VlcPlayer.input.position = TrackBar_Seek2.Value / (SeekBarMaxVal) 'VlcPlayer.Input.Position = TrackBar_Seek2.Value / (SeekBarMaxVal)
                    Form1.Timer_Seek.Start()
                End If
                SeekBarDown = False
            End If

        End If

    End Sub



#End Region '                                                  <<<<<<<<<<<<                          SEEK                                                            >>>>>>>>>>>>

#Region " Repeat | Shuffle"
    Dim Alabel As String = "00:00"
    Dim Alabel2 As String
    Dim Blabel As String = "00:00"
    Dim Blabel2 As String

#Region " AB Repeat"

    '.....................A

    Dim A_Postion As Long
    Dim B_Postion As Long
    Public Sub abut2_MouseUp(sender As Object, e As MouseEventArgs) Handles But_A.MouseUp
        If abut2hover = True Then
            But_A.BackgroundImage = My.Resources.New_Design_A_Hover
        Else
            But_A.BackgroundImage = My.Resources.New_Design_A
        End If
        If abut2hover = False Then Return

        If e.Button = MouseButtons.Left Then
            A_Repeat()
        ElseIf e.Button = MouseButtons.Right Then
            A_Postion = CLng(0)
            Alabel2 = "00:00"
            Alabel = "00:00"
        End If
    End Sub
    Public Sub A_Repeat()
        If IsVideo Then
            Dim current As TimeSpan = TimeSpan.FromMilliseconds(VlcPlayer.input.time)
            CurTime = VlcPlayer.input.position 'VlcPlayer.Input.Position
            If VlcPlayer.playlist.isPlaying Then 'If VlcPlayer.playlist.isPlaying Then
                Alabel = String.Format("{0}:{1:D2}", current.Minutes, current.Seconds)
                Alabel2 = VlcPlayer.input.position 'VlcPlayer.Input.Position
            End If
        Else
            CurTime = AudioPlayer.Instance.Position
            If AudioPlayer.Instance.GetStreamStatus = BASSActive.BASS_ACTIVE_PLAYING Then
                A_Postion = AudioPlayer.Instance.Position
                Alabel = AudioPlayer.Instance.GetElapsedTime
                Alabel2 = TrackBar_Seek2.Value
            End If
        End If
    End Sub
    '.....................B
    Public Sub Bbut_MouseUp(sender As Object, e As MouseEventArgs) Handles But_B.MouseUp
        If Bbuthover = True Then
            But_B.BackgroundImage = My.Resources.New_Design_B_Hover
        Else
            But_B.BackgroundImage = My.Resources.New_Design_B
        End If
        If Bbuthover = False Then Return
        If e.Button = MouseButtons.Left Then
            B_Repeat()

        ElseIf e.Button = MouseButtons.Right Then
            B_Postion = CLng(0)
            Blabel2 = "00:00"
            Blabel = "00:00"
        End If
    End Sub
    Public Sub B_Repeat()
        If IsVideo Then
            Dim current As TimeSpan = TimeSpan.FromMilliseconds(VlcPlayer.input.time)
            CurTime = VlcPlayer.input.position 'VlcPlayer.Input.Position
            If VlcPlayer.playlist.isPlaying Then 'If VlcPlayer.playlist.isPlaying Then
                Blabel = String.Format("{0}:{1:D2}", current.Minutes, current.Seconds)
                If Alabel2 = "00:00" Then
                    Blabel2 = "00:00"
                Else
                    Blabel2 = VlcPlayer.input.position 'VlcPlayer.Input.Position
                End If
                If Not Alabel2 = "00:00" Then
                    VlcPlayer.input.position = Alabel2 'VlcPlayer.Input.Position = Alabel2
                End If
            End If
        Else


            If AudioPlayer.Instance.GetStreamStatus = BASSActive.BASS_ACTIVE_PLAYING Then
                '      blabel.Text = String.Format("{0}:{1:D2}", current.Minutes, current.Seconds)
                If A_Postion = CLng(0) Then
                    B_Postion = CLng(0)
                    Blabel2 = 0
                Else
                    B_Postion = AudioPlayer.Instance.Position
                    Blabel = AudioPlayer.Instance.GetElapsedTime
                    Blabel2 = TrackBar_Seek2.Value
                End If

                If Not A_Postion = CLng(0) Then
                    AudioPlayer.Instance.Position = A_Postion
                End If
            End If
        End If
        Timer_AB.Start()
    End Sub
    Dim ABLength As Integer
    'Activate A B repeat
    Public Sub abtimer_Tick(sender As Object, e As EventArgs) ' Handles Timer_AB.Tick
        If Form1.FormClosingval Then Return
        If AppOpenFinished = False Then Return
        If IsVideo Then
            If VlcPlayer.playlist.isPlaying = False Then Return ' If VlcPlayer.playlist.isPlaying = False Then Return
            If Not Blabel2 = "00:00" Then 'Not Label1.Text = "00:00" And 
                If Blabel2 > Alabel2 Then
                    If VlcPlayer.input.position Then 'VlcPlayer.Input.Position >= Blabel2 Then
                        VlcPlayer.input.position = Alabel2 'VlcPlayer.Input.Position = Alabel2
                        ABLength = (Blabel2 * 100) - (Alabel2 * 100)
                    End If
                End If
            End If
        Else
            If Not (AudioPlayer.Instance.GetStreamStatus = BASSActive.BASS_ACTIVE_PLAYING) Then Return

            If Not B_Postion = CLng(0) Then
                If B_Postion > A_Postion Then
                    If AudioPlayer.Instance.Position >= B_Postion Then
                        AudioPlayer.Instance.Position = A_Postion
                        ABLength = (B_Postion) - (A_Postion)
                    End If
                End If
            End If
        End If
    End Sub
    'Reset all A B
    Public Sub abresetbut2_Click(sender As Object, e As EventArgs) Handles But_AB_Reset.Click
        Reset_AB_Repeat()
    End Sub
    Public Sub Reset_AB_Repeat()
        A_Postion = CLng(0)
        B_Postion = CLng(0)
        Alabel = "00:00"
        Blabel = "00:00"
        Alabel2 = 0
        Blabel2 = 0
    End Sub
    Public Sub abut2_MouseHover(sender As Object, e As EventArgs) Handles But_A.MouseHover
        ToolTip1.SetToolTip(But_A, Alabel)
    End Sub
    Public Sub Bbut_MouseHover(sender As Object, e As EventArgs) Handles But_B.MouseHover
        ToolTip1.SetToolTip(But_B, Blabel)
    End Sub

#End Region
#Region " Shuffle"
    'shuffle2
    Public Shared IsShuffle As Boolean = False
    Public Shared shuffle2hover As Boolean = False
    Public Shared ShuffleButdisabled As Boolean = False
    Public Sub shuffle2_Click(sender As Object, e As EventArgs) Handles But_Shuffle.Click
        If UsingSpotify Then
            For i As Integer = 0 To _SpotifyNew.GetDevices.Devices.Count - 1
                Try
                    If _SpotifyNew.GetDevices.Devices(i).IsActive Then
                        _SpotifyNew.SetShuffle(Not _SpotifyNew.GetPlayback.ShuffleState, _SpotifyNew.GetDevices.Devices(i).Id)
                    End If
                Catch ex As Exception
                End Try
            Next


            IsShuffle = _SpotifyNew.GetPlayback.ShuffleState
            ' MySpotify.Shuffle()
            Exit Sub
        End If
        If ShuffleButdisabled = False Then
            If IsShuffle = False Then
                IsShuffle = True
                But_Shuffle.BackgroundImage = ShuffleImage
                'TouchShuffleBut.BackgroundImage = ShuffleImage
                My.Settings.Shuffle = 1
            Else
                IsShuffle = False
                But_Shuffle.BackgroundImage = ShuffleDisabledImage
                'TouchShuffleBut.BackgroundImage = ShuffleDisabledImage
                My.Settings.Shuffle = 0
            End If
        End If
    End Sub
#End Region


#End Region
#Region " Speed  |   Pitch  |   Equalizer"

#Region " Speed / Pitch Controls"


    'Speed Trackbar
    Public Sub trackBarSpeed_Scroll() Handles trackBar_Speed2.ValueChanged
        UpdateSpeedByCurrentValue()
    End Sub
    Dim DoSnapTrackbarSpeed As Boolean = False
    Public Sub trackBarSpeed_MouseDown(sender As Object, e As MouseEventArgs) Handles trackBar_Speed2.MouseDown, trackBar_Speed2.MouseMove

        If e.Button = MouseButtons.Left Then
            Dim numberTicks As Integer = (trackBar_Speed2.Maximum - trackBar_Speed2.Minimum) / (trackBar_Speed2.TickInterval)
            Dim percent As Single = CSng(e.X) / CSng(trackBar_Speed2.Size.Width)
            Dim newValue As Integer = CInt(CSng(numberTicks) * CSng(percent))
            Dim val As Integer = trackBar_Speed2.Value
            If (val <= 0) And (val > -6) Then
                trackBar_Speed2.Value = 0
                DoSnapTrackbarSpeed = True
            ElseIf (val >= 0) And (val < 6) Then
                trackBar_Speed2.Value = 0
                DoSnapTrackbarSpeed = True
            Else
                DoSnapTrackbarSpeed = False
            End If
        End If
    End Sub
    Public Sub trackBarSpeed_MouseUp(sender As Object, e As MouseEventArgs) Handles trackBar_Speed2.MouseUp
        If e.Button = MouseButtons.Right Then
            Form1.CustomizeSliders()

            Return
        End If
        If DoSnapTrackbarSpeed Then
            trackBar_Speed2.Value = 0
        End If
        DoSnapTrackbarSpeed = False
    End Sub
    'Update Speed
    Public Sub UpdateSpeedByCurrentValue()
        Dim trackbarValue As Integer = trackBar_Speed2.Value
        Dim speed As Single = trackbarValue


        If IsVideo Then
            VlcPlayer.input.rate = speed / 50

        Else

            AudioPlayer.Instance.CurrentAudioHandle.SetTempo(speed)
        End If






        Timer_Rate.Start()
    End Sub

    'Slow
    Public Sub slowbut2_Click(sender As Object, e As EventArgs) Handles But_SpeedDown.Click
        Speed_Slow()
    End Sub
    Public Sub Speed_Slow()
        Try
            If IsVideo Then
                If VlcPlayer.input.rate = 1 Then
                    VlcPlayer.input.rate = 0.9
                Else
                    VlcPlayer.input.rate -= 0.1
                End If
            Else
                trackBar_Speed2.Value -= 1
                Timer_Rate.Start()
            End If
        Catch
        End Try
    End Sub
    'Normal
    Public Sub normbut2_Click(sender As Object, e As EventArgs) Handles Label_SpeedTextbox.Click
        Speed_Norm()
    End Sub
    Public Sub Speed_Norm()
        Try
            If IsVideo Then
                VlcPlayer.input.rate = 1
            Else
                trackBar_Speed2.Value = 0
                Timer_Rate.Start()
            End If
        Catch
        End Try
    End Sub
    'Fast
    Public Sub fastbut2_Click(sender As Object, e As EventArgs) Handles But_SpeedUp.Click
        Speed_Fast()
    End Sub
    Public Sub Speed_Fast()
        Try
            If IsVideo Then
                VlcPlayer.input.rate += 0.1
            Else
                trackBar_Speed2.Value += 1
                Timer_Rate.Start()
            End If
        Catch
        End Try
    End Sub
    'Speed Label
    Public Sub Timerrate_Tick(sender As Object, e As EventArgs) ' Handles Timer_Rate.Tick
        If AppOpenFinished = False Then Return
        Try
            If IsVideo Then
                If VlcPlayer.input.rate = 1 Or 2 Or 3 Or 4 Or 5 Then
                    Label_SpeedTextbox.Text = VlcPlayer.input.rate & ".0"
                Else
                    Label_SpeedTextbox.Text = VlcPlayer.input.rate
                End If
            Else
                Label_SpeedTextbox.Text = trackBar_Speed2.Value & "%"
            End If
        Catch
        End Try
    End Sub




    'Pitch Trackbar
    Public Sub trackBarPitch_Scroll() Handles trackbar_Pitch2.ValueChanged
        UpdatePitchByCurrentValue()
        PicBox_PitchTextBox.Text = trackbar_Pitch2.Value
    End Sub
    'Update Pitch
    Public Sub UpdatePitchByCurrentValue()
        AudioPlayer.Instance.CurrentAudioHandle.SetPitch(CSng(trackbar_Pitch2.Value))
        PicBox_PitchTextBox.Text = trackbar_Pitch2.Value
    End Sub

    'Low
    Public Sub PitchDownBut_Click(sender As Object, e As EventArgs) Handles But_PitchDown.Click
        If Not trackbar_Pitch2.Value = -12 Then
            trackbar_Pitch2.Value -= 1
        End If
    End Sub
    'High
    Public Sub PitchUpBut_Click(sender As Object, e As EventArgs) Handles But_PitchUp.Click
        If Not trackbar_Pitch2.Value = 12 Then
            trackbar_Pitch2.Value += 1
        End If
    End Sub
    'Pitch Label
    Public Sub PitchTextBox_Click(sender As Object, e As EventArgs) Handles PicBox_PitchTextBox.Click
        trackbar_Pitch2.Value = 0
    End Sub

    ' Mouse Up
    Public Sub trackbar_Pitch2_MouseUp(sender As Object, e As MouseEventArgs) Handles trackbar_Pitch2.MouseUp
        If e.Button = MouseButtons.Right Then
            Form1.CustomizeSliders()

            Return
        End If
    End Sub




#End Region
#Region " Player Speed Change"
    Public Sub playerspeed_TextChanged(sender As Object, e As EventArgs) Handles Label_SpeedTextbox.TextChanged
        LabelConfirmSpeed.Text = trackBar_Speed2.Value & "%"
        LabelConfirmSpeed.Visible = True
        Timer_LabelConfirmSpeedFadeIn.Interval = 2000
        Timer_LabelConfirmSpeedFadeIn.Stop()
        Timer_LabelConfirmSpeedFadeIn.Start()
    End Sub

    Public Sub TimerLabelConfirmSpeed_Tick(sender As Object, e As EventArgs) 'Handles Timer_LabelConfirmSpeedFadeIn.Tick
        If AppOpenFinished = False Then Return
        LabelConfirmSpeed.Visible = False
    End Sub

    Public Sub TimerLabelConfirmSpeedFadeOut_Tick(sender As Object, e As EventArgs) 'Handles Timer_LabelConfirmSpeedFadeOut.Tick
        LabelConfirmSpeed.Text = Label_SpeedTextbox.Text.Substring(0, 3)
        Static aa As Integer
        If aa <= 0 Then
            LabelConfirmSpeed.Visible = False
            Timer_LabelConfirmSpeedFadeIn.Stop() 'finished fade-in
            Timer_LabelConfirmSpeedFadeOut.Stop()
        End If
    End Sub
#End Region



#End Region

#Region " Volume"

#Region " Player Volume"

    ' Mouse Up
    Public Sub TrackBar_PlayerVol2_MouseUp(sender As Object, e As MouseEventArgs) Handles TrackBar_PlayerVol2.MouseUp
        If e.Button = MouseButtons.Right Then
            Form1.CustomizeSliders()

            Return
        End If
    End Sub
    'Player Volume
    Public Sub TrackBarControl1_EditValueChanged() Handles TrackBar_PlayerVol2.ValueChanged
        If Not UsingSpotify Then
            Try
                Dim Playlist As GridPlaylist
                Dim Scroller As Scroller
                For Each c As Control In PlaylistTabs.SelectedTabPage.Controls
                    If c.GetType Is GetType(Scroller) Then
                        Scroller = c
                        For Each c2 As Control In Scroller.Controls
                            If c2.GetType Is GetType(GridPlaylist) Then
                                Playlist = c2
                            End If
                        Next
                    End If

                Next

                If Not Playlist.Rows.Count = 0 Then
                    If IsVideo Then

                    Else
                        SetPlayerVolume()
                    End If
                End If
            Catch

            End Try
        Else
            If TrackBar_PlayerVol2.Maximum = 200 Then
                _SpotifyNew.SetVolume(TrackBar_PlayerVol2.Value / 2)
            Else
                _SpotifyNew.SetVolume(TrackBar_PlayerVol2.Value)
            End If
        End If
    End Sub
    'Volume
    Public Sub SetPlayerVolume()
        Dim trackbarValue As Integer = TrackBar_PlayerVol2.Value
        Dim volume As Single
        Dim multiplier As Integer
        Dim isModule As Boolean = AudioPlayer.Instance.CurrentAudioHandle.IsModule()

        isModule = False

        If isModule Then
            Const maxVolumeValueIT As Integer = 128
            Const maxVolumeValueMOD As Integer = 64

            Dim maxVolume As Integer = If(AudioPlayer.Instance.CurrentAudioHandle.ChannelInfo.[ctype] = BASSChannelType.BASS_CTYPE_MUSIC_IT, maxVolumeValueIT, maxVolumeValueMOD)
            multiplier = maxVolume
        Else
            multiplier = 1
        End If

        volume = (CSng(trackbarValue) * multiplier / 100)
        AudioPlayer.Instance.CurrentAudioHandle.SetVolume(volume)
    End Sub



#End Region

#End Region '




    Public Sub SeekBar2_ValueChanged() Handles TrackBar_Seek2.ValueChanged

        If SongStartOver = True Then
            If IsVideo = True Then
                AudioPlayer.Instance.ResetTrackList()
            Else
                VlcPlayer.playlist.stop() 'VlcPlayer.playlist.stop()
                Form1.VLCclearPlaylists() : ResetVLC()
                Form1.AudioPlayer_Song_Opening()
            End If

            SongStartOver = False
        End If

        Try
            If My.Settings.DriveMode Then
                Dim hWnd As IntPtr = FindWindow(Nothing, Me.Text)
                Taskbar.TaskbarManager.Instance.SetProgressValue(TrackBar_Seek2.Value, TrackBar_Seek2.Maximum, hWnd)
            Else
                Taskbar.TaskbarManager.Instance.SetProgressValue(TrackBar_Seek2.Value, TrackBar_Seek2.Maximum)
            End If

        Catch

        End Try
    End Sub




#Region " Artwork   |   Fullscreen"
    'Double Click to Enlarge Picture
    Dim pictureform As New Form
    Public Sub Zoom()
        pictureform.FormBorderStyle = FormBorderStyle.None
        pictureform.BackgroundImage = ChangeOpacity(Artwork.BackgroundImage, 100)
        pictureform.BackgroundImageLayout = ImageLayout.Zoom
        pictureform.BackColor = Color.Black
        pictureform.ShowInTaskbar = False
        pictureform.Show()
        pictureform.Location = Me.Location
        pictureform.WindowState = FormWindowState.Maximized
    End Sub
    Public Sub PictureBox1_MouseDoubleClick(sender As Object, e As MouseEventArgs)  'PictureBox1.MouseDoubleClick
        If Not e.Button = MouseButtons.Left Then Return
        Zoom()
    End Sub
    Public Sub Artwork_ZoomGesture(sender As Object, e As Telerik.WinControls.ZoomGestureEventArgs) 'Handles Artwork.ZoomGesture
        Zoom()
    End Sub
    Public Sub pictureform_keyup()
        pictureform.WindowState = FormWindowState.Normal
        pictureform.Hide()
    End Sub

#End Region
#Region " Artwork   |   Gestures"
    Dim XTemp As Integer
    Dim YTemp As Integer
    Public Shared SwipingUp As Boolean = False
    Public Shared AddingFile As Boolean = False
    Public Sub PictureBox1_MouseDown(sender As Object, e As MouseEventArgs) Handles Artwork.MouseDown
        XTemp = e.Location.X
        YTemp = e.Location.Y
    End Sub
    Public Sub PictureBox1_MouseMove(sender As Object, e As MouseEventArgs) Handles Artwork.MouseMove
        If Not e.Button = MouseButtons.Left Then Return
        If Form1.LyricsMoved2 Then
            Return
        End If
        If e.Location.Y >= (YTemp + 50) Then
            SwipingUp = True
        ElseIf e.Location.Y <= (YTemp - 50) Then
            SwipingUp = True
        Else
            SwipingUp = False
        End If

        If SwipingUp = False Then
            If e.Location.X >= (XTemp + 50) Then
                But_Previous.BackgroundImage = PreviousHoverImage
                But_Next.BackgroundImage = NextImage
                But_Next.BackgroundImage = NextImage
            ElseIf e.Location.X <= (XTemp - 50) Then
                But_Next.BackgroundImage = NextHoverImage
                But_Next.BackgroundImage = NextHoverImage
                But_Previous.BackgroundImage = PreviousImage
            Else
                But_Next.BackgroundImage = NextImage
                But_Next.BackgroundImage = NextImage

                But_Previous.BackgroundImage = PreviousImage
            End If
        End If
    End Sub
    Public Sub PictureBox1_MouseUp(sender As Object, e As MouseEventArgs) Handles Artwork.MouseUp
        If AddingFile = True Then Exit Sub
        Form1.LyricsDown = False
        If e.Button = MouseButtons.Right Then
            AudioPlayer.Instance.Stop()
            But_PlayPause.BackgroundImage = PlayImage
            But_PlayPause.BackgroundImage = PlayImage
        End If
        If Not e.Button = MouseButtons.Left Then Return
        But_Previous.BackgroundImage = PreviousImage
        But_Next.BackgroundImage = NextImage

        If Form1.LyricsMoved2 Then
            Form1.LyricsMoved2 = False
            Form1.LyricsMoved = False
            Return
        End If

        'Skip Songs
        If SwipingUp = False Then
            If e.Location.X >= (XTemp + 50) Then
                PrevItem()
                But_Previous.BackgroundImage = PreviousImage
                Return
            ElseIf e.Location.X <= (XTemp - 50) Then
                NextItem()
                But_Next.BackgroundImage = NextImage
                But_Next.BackgroundImage = NextImage
                Return
            End If
        End If
        If Form1.LyricsMoved Then
            Form1.LyricsMoved = False
            Form1.LyricsMoved2 = False
            Return
        End If
        'Control Speed
        If Form1.MaximumSizeChanged = False Then
            If e.Location.Y >= (YTemp + 75) Then

                Speed_Slow()
            ElseIf e.Location.Y <= (YTemp - 75) Then

                Speed_Fast()
            Else
                New_Play()
            End If
        End If
        Form1.MaximumSizeChanged = False
    End Sub


    Public Sub Artwork_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles Artwork.MouseDoubleClick
        Zoom()
    End Sub
#End Region

#End Region

#Region " Spectrum"

    Public Sub PictureBoxSpec_Click(sender As Object, e As MouseEventArgs) Handles PictureBoxSpec.MouseClick, PictureBoxSpec.Click
        If e.Button = MouseButtons.Left Then
            Select Case My.Settings.SpectrumLineWidth
                Case 2
                    My.Settings.SpectrumLineWidth = 4
                Case 4
                    My.Settings.SpectrumLineWidth = 6
                Case 6
                    My.Settings.SpectrumLineWidth = 8
                Case 8
                    My.Settings.SpectrumLineWidth = 10
                Case 10
                    My.Settings.SpectrumLineWidth = 12
                Case 12
                    My.Settings.SpectrumLineWidth = 2
            End Select
        ElseIf e.Button = MouseButtons.Right Then
            Select Case My.Settings.SpectrumType
                Case "Line"
                    My.Settings.SpectrumType = "Dot"
                Case "Dot"
                    My.Settings.SpectrumType = "Line Peak"
                Case "Line Peak"
                    My.Settings.SpectrumType = "Bean"
                Case "Bean"
                    My.Settings.SpectrumType = "Ellipse"
                Case "Ellipse"
                    My.Settings.SpectrumType = "Text"
                Case "Text"
                    My.Settings.SpectrumType = "Wave"
                Case "Wave"
                    My.Settings.SpectrumType = "Full"
                Case "Full"
                    My.Settings.SpectrumType = "Line"
                Case Else
                    My.Settings.SpectrumType = "Line"


            End Select
        End If

    End Sub


#End Region









#Region " Switch Back  |  Close"
    'Closing

    Private Sub pb_switchback_Click(sender As Object, e As EventArgs) Handles pb_switchback.Click
        ' My.Settings.ArtworkTransparency = PreviousArtOpac

        My.Settings.DriveMode = False
        My.Settings.CarFormOpened = False
        My.Settings.Save()
        DoClose = False
        Form1.Show()
        ' Form1.Splitter.Panel2.Controls.Add(form1.PlaylistTabs)
        ' Form1.Setup_Playlists()
        If My.Settings.MiniModeOn Then
            Form1.MiniModeOn()
        Else
            ' Form1.MiniModeOff()
        End If
        Form1.Opacity = 100
        Form1.WindowState = FormWindowState.Normal
        Form1.Location = My.Settings.OriginalLocation
        Me.Hide()
        Me.Hide()



    End Sub

    Public Sub oldSwitchBackcode()
        'My.Settings.ArtworkTransparency = PreviousArtOpac

        My.Settings.DriveMode = False
        My.Settings.CarFormOpened = False
        DoClose = False
        Form1.Show()
        Form1.Opacity = 100
        Form1.WindowState = FormWindowState.Normal
        Form1.Location = My.Settings.OriginalLocation
        Try
            For Each c As Control In PanelPlayback.Controls
                Form1.Controls.Add(c)
            Next
            For Each c As Control In PlaylistScroller.Controls
                Form1.Controls.Add(c)
            Next

        Catch ex As Exception

        End Try

        Application.Restart()
        Form1.Close()


        Return


        Form1.Show()
        Form1.Opacity = 1
        Form1.WindowState = FormWindowState.Normal
        Form1.ShowInTaskbar = True


        If My.Settings.MiniModeOn Then
            Form1.MiniModeOn()
        Else
            Form1.MiniModeOff()
        End If
        Me.Dispose()
    End Sub
    Private Sub CarForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        '  My.Settings.ArtworkTransparency = PreviousArtOpac

        If DoClose Then
            My.Settings.CarFormOpened = False
            Form1.Close()
        End If
    End Sub
    Private Sub But_TitleBar_Close_Click(sender As Object, e As EventArgs) Handles But_TitleBar_Close.Click
        ' My.Settings.ArtworkTransparency = PreviousArtOpac

        If NowPlayingInit = False Then
            Dim use As Boolean = False
            If use Then
                DoAdd = True
                DontSetParents = True
                tabs_Car.SelectedTabPageIndex = 1
                If firstsetupnowplaying Then
                    SetupNowPlaying()
                    SetupPlaylistsTabs()
                    RefreshHotkeys()
                    firstsetupnowplaying = False
                End If
            End If

        End If

        DoClose = True
        'My.Settings.DriveMode = False
        My.Settings.CarFormOpened = False
        My.Settings.Save()
        Form1.Close()
    End Sub

    Private Sub CarForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' My.Settings.ArtworkTransparency = PreviousArtOpac
        If NowPlayingInit = False Then
            Dim use As Boolean = False
            If use Then
                DoAdd = True
                DontSetParents = True
                tabs_Car.SelectedTabPageIndex = 1
                If firstsetupnowplaying Then
                    SetupNowPlaying()
                    SetupPlaylistsTabs()
                    RefreshHotkeys()
                    firstsetupnowplaying = False
                End If
            End If

        End If

        If NowPlayingInit Then
            Dim use As Boolean = False
            If use Then
                Form1.Show()
                Form1.Opacity = 100
                Form1.WindowState = FormWindowState.Normal
                Form1.Location = My.Settings.OriginalLocation
            End If


            FormClosing()
        End If


        My.Settings.CarFormOpened = False
        My.Settings.Save()
    End Sub

    Public Sub FormClosing()
        Dim excuteCode As Boolean = False
        ' Me.Opacity = 100
        My.Settings.PlaylistsCount = PlaylistTabs.TabPages.Count
        My.Settings.LastPlayedSongs.Clear()
        '...................Save Playlist Items...............
        For num As Integer = 0 To My.Settings.PlaylistsCount - 1
            Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.TabPages(num).Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next
            If num = 0 Then : If Playlist.RowCount <> 0 Then : My.Settings.Playlist0HasFiles = True : My.Settings.FirstTimeSetup = False
                Else : My.Settings.Playlist0HasFiles = False : End If : End If

            Try : Dim Row As Integer = Playlist.CurrentCell.RowIndex
                Dim RowCount As Integer = Playlist.RowCount : Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                If RowCount > 0 Then
                    System.IO.File.WriteAllText(Form1.TempPlaylistNew & num & ".rpl", "") : Form1.SaveGridData(Playlist, Form1.TempPlaylistNew & num & ".rpl")
                    My.Settings.LastPlayedSongs.Insert(num, Row)
                End If
            Catch : Form1.ClosingErrors = Form1.ClosingErrors + "Unable to save playlist(s) items! You may have blank playlists." + Environment.NewLine : End Try
        Next

        '.......................Save Position of Active Tab....................
        Try : Dim num As Integer = PlaylistTabs.SelectedTabPageIndex
            If Form1.IsVideo Then
                My.Settings.LastPlayedPositions(num) = Form1.VlcPlayer.input.position 'VlcPlayer.Input.Position
            Else
                My.Settings.LastPlayedPositions(num) = AudioPlayer.Instance.Position.ToString
            End If : Catch ex As Exception : End Try


        '.......................Last Played Item...................
        Try
            For num As Integer = 0 To My.Settings.PlaylistsCount - 1
                If PlaylistTabs.TabPages(num).Text <> "Spotify" Then
                    Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.TabPages(num).Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next
                    Try
                        Dim Row As Integer = Playlist.CurrentCell.RowIndex
                        Dim RowCount As Integer = Playlist.RowCount
                        Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                        If RowCount > 0 Then
                            My.Settings.LastPlayedSongs.Item(num) = Row
                        End If
                    Catch : Form1.ClosingErrors = Form1.ClosingErrors + "Unable to save playlist(s) last played songs! Possibly no items in playlists?" + Environment.NewLine
                    End Try : End If : Next : Catch
            Form1.ClosingErrors = Form1.ClosingErrors + "Error 0x22: Unable to save Playlists' Last Played item! (Playlists cannot be blank when saving/closing app!)" + Environment.NewLine
            ' MyMsgBox.Show("Error 0x22: Unable to save other Playlists' Last Played item!")
        End Try


        '..............Playlist Selected................
        Try : My.Settings.PlaylistsSelected = PlaylistTabs.SelectedTabPageIndex
        Catch : Form1.ClosingErrors = Form1.ClosingErrors + "Error 0x25: Unable to save Selected Playlist status" + Environment.NewLine : End Try


        '.......................Playlist Names..............
        If Form1.ClosingErrors = "" Then : For num As Integer = 0 To PlaylistTabs.TabPages.Count - 1
                My.Settings.PlaylistNames.Item(num) = PlaylistTabs.TabPages(num).Text.ToString : Next : End If





        '...................WINDOW LOCATION................

        If Not excuteCode Then
            Try
                If My.Settings.DriveMode = False Then
                    If Me.WindowState = FormWindowState.Maximized Then
                        My.Settings.PlayerLocation = Form1.CurrentPoint : My.Settings.FormSize = Form1.CurrentSize
                    Else : My.Settings.PlayerLocation = Me.Location
                        My.Settings.FormSize = Me.Size
                        '   If My.Settings.MiniModeOn = False Then My.Settings.OriginalSize = Me.Size
                    End If
                Else
                    '  Me.Show()
                    '  Me.Location = My.Settings.PlayerLocation ': 
                    My.Settings.PlayerLocation = My.Settings.OriginalLocation '  CurrentPoint
                    My.Settings.FormSize = Form1.CurrentSize

                End If

            Catch : Form1.ClosingErrors = Form1.ClosingErrors + "Error 0x26: Unable to save App's position on screen!" + Environment.NewLine : End Try
        End If


        '.......................Trackbar Values.........................
        My.Settings.BASS_Speed = Form1.trackBar_Speed2.Value
        My.Settings.BASS_Pitch = Form1.trackbar_Pitch2.Value
        My.Settings.Player_Volume = Form1.TrackBar_PlayerVol2.Value
        My.Settings.EnablePlayerVolumeCheckState = Form1.BarCheckBox_Player_Volume.Checked


        '................Video Last Played Positions................
        Try 'If Form1.VlcPlayer.playlist.items.count > 0 Then : End If
        Catch : End Try

        My.Settings.PlaylistsCount = PlaylistTabs.TabPages.Count

        If Not Form1.IsAutoSaving Then : If Not Form1.SaveALLBut Then : My.Settings.PlaylistsCount = PlaylistTabs.TabPages.Count : My.Settings.Save() : End If : End If


        '......................SAVE SETTINGS........................
        My.Settings.Save()

        If Form1.SaveALLBut = True Then : If Not Form1.IsAutoSaving Then : If Form1.ClosingErrors = "" Then : MyMsgBox.Show("Current settings and playlists saved!", "", True) : End If : End If : End If
    End Sub






#End Region






#Region "  Graphics"


    Public Shared Function ChangeOpacity(ByVal img As System.Drawing.Image, ByVal opacityvalue As Single) As Bitmap
        Dim bmp As New Bitmap(img.Width, img.Height)
        Dim graphics__1 As Graphics = Graphics.FromImage(bmp)
        Dim colormatrix As New ColorMatrix
        colormatrix.Matrix33 = opacityvalue
        Dim imgAttribute As New ImageAttributes
        imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.[Default], ColorAdjustType.Bitmap)
        graphics__1.DrawImage(img, New Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, _
         GraphicsUnit.Pixel, imgAttribute)
        graphics__1.Dispose()
        Return bmp
    End Function



    Dim TitleBarCloseImage As System.Drawing.Image = My.Resources.TitleBar_Close_NewDesign
    Dim TitleBarCloseHoverImage As System.Drawing.Image = My.Resources.TitleBar_Close_NewDesign_Hover
    Dim TitleBarClosePressImage As System.Drawing.Image = My.Resources.TitleBar_Close_NewDesign_Press
    Dim TitleBar_Closehover As Boolean = False



    Public Sub TitleBar_Close2_MouseEnter(sender As Object, e As EventArgs) Handles But_TitleBar_Close.MouseEnter
        But_TitleBar_Close.BackgroundImage = TitleBarCloseHoverImage
        TitleBar_Closehover = True
        Me.Cursor = Cursors.Default
    End Sub
    Public Sub TitleBar_Close2_MouseLeave(sender As Object, e As EventArgs) Handles But_TitleBar_Close.MouseLeave
        But_TitleBar_Close.BackgroundImage = TitleBarCloseImage
        TitleBar_Closehover = False
    End Sub
    Public Sub TitleBar_Close2_MouseDown(sender As Object, e As MouseEventArgs) Handles But_TitleBar_Close.MouseDown
        But_TitleBar_Close.BackgroundImage = TitleBarClosePressImage
    End Sub
    Public Sub TitleBar_Close2_MouseUp(sender As Object, e As MouseEventArgs) Handles But_TitleBar_Close.MouseUp
        If TitleBar_Closehover = True Then
            But_TitleBar_Close.BackgroundImage = TitleBarCloseHoverImage
        Else
            But_TitleBar_Close.BackgroundImage = TitleBarCloseImage
        End If
    End Sub

    Dim SwitchBackImage As System.Drawing.Image = My.Resources.SwitchBack
    Dim SwitchBackHoverImage As System.Drawing.Image = My.Resources.SwitchBack_Hover
    Dim SwitchBackPressImage As System.Drawing.Image = My.Resources.SwitchBack_Press
    Dim SwitchBackhover As Boolean = False

    Public Sub SwitchBack_MouseEnter(sender As Object, e As EventArgs) Handles pb_switchback.MouseEnter
        pb_switchback.BackgroundImage = SwitchBackHoverImage
        SwitchBackhover = True
        Me.Cursor = Cursors.Default
    End Sub
    Public Sub SwitchBack_MouseLeave(sender As Object, e As EventArgs) Handles pb_switchback.MouseLeave
        pb_switchback.BackgroundImage = SwitchBackImage
        SwitchBackhover = False
    End Sub
    Public Sub SwitchBack_MouseDown(sender As Object, e As MouseEventArgs) Handles pb_switchback.MouseDown
        pb_switchback.BackgroundImage = SwitchBackPressImage
    End Sub
    Public Sub SwitchBack_MouseUp(sender As Object, e As MouseEventArgs) Handles pb_switchback.MouseUp
        If SwitchBackhover = True Then
            pb_switchback.BackgroundImage = SwitchBackHoverImage
        Else
            pb_switchback.BackgroundImage = SwitchBackImage
        End If
    End Sub


#End Region

#Region " GRAPHICS"


#Region " Buttons"
    'Play/Pause
    Public Shared playhover As Boolean = False
    Public Shared Playdown As Boolean = False

    Public Sub Playpausebut_MouseEnter(sender As Object, e As EventArgs) Handles But_PlayPause.MouseEnter
        If Playdown Then Return
        If UsingSpotify Then
            If SpotifyPlaying Then

                But_PlayPause.BackgroundImage = PauseHoverImage
            Else
                But_PlayPause.BackgroundImage = PlayHoverImage
            End If

            playhover = True

            Return
        End If

        If IsVideo = False Then
            If AudioPlayer.Instance.GetStreamStatus = BASSActive.BASS_ACTIVE_PLAYING Then
                But_PlayPause.BackgroundImage = PauseHoverImage
            Else
                But_PlayPause.BackgroundImage = PlayHoverImage
            End If
        Else

            If VlcPlayer.playlist.isPlaying = True Then
                But_PlayPause.BackgroundImage = PauseHoverImage
            ElseIf VlcPlayer.playlist.isPlaying = False Then ' If VlcPlayer.playlist.isPlaying = False Then

                But_PlayPause.BackgroundImage = PlayHoverImage
            End If


        End If
        playhover = True
    End Sub
    Public Sub Playpausebut_MouseLeave(sender As Object, e As EventArgs) Handles But_PlayPause.MouseLeave
        If UsingSpotify Then
            If SpotifyPlaying Then
                But_PlayPause.BackgroundImage = PauseImage
            Else
                But_PlayPause.BackgroundImage = PlayImage
            End If

            playhover = False

            Return
        End If

        If IsVideo = False Then
            If AudioPlayer.Instance.GetStreamStatus = BASSActive.BASS_ACTIVE_PLAYING Then
                But_PlayPause.BackgroundImage = PauseImage
            Else
                But_PlayPause.BackgroundImage = PlayImage
            End If
        Else

            If VlcPlayer.playlist.isPlaying = True Then
                But_PlayPause.BackgroundImage = PauseImage
            ElseIf VlcPlayer.playlist.isPlaying = False Then ' If VlcPlayer.playlist.isPlaying = False Then

                But_PlayPause.BackgroundImage = PlayImage
            End If


        End If
        playhover = False
    End Sub
    Public Sub Playpausebut_MouseDown(sender As Object, e As MouseEventArgs) Handles But_PlayPause.MouseDown
        Playdown = True
        If UsingSpotify Then
            If SpotifyPlaying Then
                But_PlayPause.BackgroundImage = PausePressImage
            Else
                But_PlayPause.BackgroundImage = PlayPressImage
            End If

            Return
        End If


        If IsVideo = False Then
            If AudioPlayer.Instance.GetStreamStatus = BASSActive.BASS_ACTIVE_PLAYING Then
                But_PlayPause.BackgroundImage = PausePressImage
            Else
                But_PlayPause.BackgroundImage = PlayPressImage
            End If
        Else

            If VlcPlayer.playlist.isPlaying = True Then
                But_PlayPause.BackgroundImage = PausePressImage
            ElseIf VlcPlayer.playlist.isPlaying = False Then ' If VlcPlayer.playlist.isPlaying = False Then
                But_PlayPause.BackgroundImage = PlayPressImage
            End If


        End If

    End Sub
    Public Sub Playpausebut_MouseUp(sender As Object, e As MouseEventArgs) Handles But_PlayPause.MouseUp

        Playdown = False
        If UsingSpotify Then
            If playhover = True Then
                If SpotifyPlaying Then : But_PlayPause.BackgroundImage = PauseHoverImage
                Else : But_PlayPause.BackgroundImage = PlayHoverImage : End If
                New_Play()
            Else
                If SpotifyPlaying Then : But_PlayPause.BackgroundImage = PauseImage
                Else : But_PlayPause.BackgroundImage = PlayImage : End If
            End If

            Return
        End If

        If IsVideo = False Then
            If playhover = True Then
                If AudioPlayer.Instance.GetStreamStatus = BASSActive.BASS_ACTIVE_PLAYING Then
                    But_PlayPause.BackgroundImage = PauseHoverImage
                Else
                    But_PlayPause.BackgroundImage = PlayHoverImage
                End If
            Else
                If AudioPlayer.Instance.GetStreamStatus = BASSActive.BASS_ACTIVE_PLAYING Then
                    But_PlayPause.BackgroundImage = PauseImage
                Else
                    But_PlayPause.BackgroundImage = PlayImage
                End If
            End If
        Else
            If playhover = True Then
                If VlcPlayer.playlist.isPlaying = True Then 'If VlcPlayer.playlist.isPlaying = True Then
                    But_PlayPause.BackgroundImage = PauseHoverImage
                ElseIf VlcPlayer.playlist.isPlaying = False Then ' If VlcPlayer.playlist.isPlaying = False Then
                    But_PlayPause.BackgroundImage = PlayHoverImage
                End If
            Else
                If VlcPlayer.playlist.isPlaying = True Then 'If VlcPlayer.playlist.isPlaying = True Then
                    But_PlayPause.BackgroundImage = PauseImage
                ElseIf VlcPlayer.playlist.isPlaying = False Then ' If VlcPlayer.playlist.isPlaying = False Then
                    But_PlayPause.BackgroundImage = PlayImage
                    But_PlayPause.BackgroundImage = PlayImage
                End If
            End If
        End If
        If IsVideo = False Then
            If AudioPlayer.Instance.TrackList.Tracks.Count > 0 Then
                If playhover Then
                    New_Play()
                End If
            End If
        Else
            PlaySub()
        End If
        Me.Focus()
    End Sub

    'stop
    Public Shared stophover As Boolean = False
    Public Shared stopdisabled As Boolean = False
    Public Sub stopbut2_MouseEnter(sender As Object, e As EventArgs) Handles But_Stop.MouseEnter
        If stopdisabled = False Then
            But_Stop.BackgroundImage = StopHoverImage
            stophover = True
        End If
    End Sub
    Public Sub stopbut2_MouseLeave(sender As Object, e As EventArgs) Handles But_Stop.MouseLeave
        If stopdisabled = False Then
            But_Stop.BackgroundImage = StopImage
            stophover = False
        End If
    End Sub
    Public Sub stopbut2_MouseDown(sender As Object, e As MouseEventArgs) Handles But_Stop.MouseDown
        If stopdisabled = False Then
            But_Stop.BackgroundImage = StopPressImage
        End If
    End Sub
    Public Sub stopbut2_MouseUp(sender As Object, e As MouseEventArgs) Handles But_Stop.MouseUp
        If stophover = True Then
            If stopdisabled = False Then
                But_Stop.BackgroundImage = StopHoverImage
            End If
        Else
            If stopdisabled = False Then
                But_Stop.BackgroundImage = StopImage
            End If
        End If
        MyBase.Focus()
    End Sub
    'previous
    Public Shared Prevrwhover As Boolean = False

    Public Shared Prevrwdisabled As Boolean = False
    Public Sub prevrwbut2_MouseEnter(sender As Object, e As EventArgs) Handles But_Previous.MouseEnter
        If Prevrwdisabled = False Then
            But_Previous.BackgroundImage = PreviousHoverImage
            ' ArtworkPrevBut.BackgroundImage = LeftHoverImage
        End If
        Prevrwhover = True
    End Sub
    Public Sub prevrwbut2_MouseLeave(sender As Object, e As EventArgs) Handles But_Previous.MouseLeave
        If Prevrwdisabled = False Then
            But_Previous.BackgroundImage = PreviousImage
            ' ArtworkPrevBut.BackgroundImage = LeftImage
        End If
        Prevrwhover = False
    End Sub
    Public Sub prevrwbut2_MouseDown(sender As Object, e As MouseEventArgs) Handles But_Previous.MouseDown
        But_Previous.BackgroundImage = PreviousPressImage
        ' ArtworkPrevBut.BackgroundImage = LeftPressImage
    End Sub
    'Mouse up mentioned ealier
    'Next
    Public Shared NextButhover As Boolean = False
    Public Shared NextButdisabled As Boolean = False
    Public Sub NextButbut_MouseEnter(sender As Object, e As EventArgs) Handles But_Next.MouseEnter
        If NextButdisabled = False Then
            But_Next.BackgroundImage = NextHoverImage
            ' ArtworkNextBut.BackgroundImage = RightHoverImage
            NextButhover = True
        End If
    End Sub
    Public Sub NextButbut_MouseLeave(sender As Object, e As EventArgs) Handles But_Next.MouseLeave
        If NextButdisabled = False Then
            But_Next.BackgroundImage = NextImage
            ' ArtworkNextBut.BackgroundImage = RightImage
            NextButhover = False
        End If
    End Sub
    Public Sub NextButbut_MouseDown(sender As Object, e As MouseEventArgs) Handles But_Next.MouseDown
        But_Next.BackgroundImage = NextPressImage
        ' ArtworkNextBut.BackgroundImage = RightPressImage
    End Sub
    'repeat
    Public Shared repeatbut2hover As Boolean = False
    Public Shared repeatbuthover As Boolean = False
    Public Sub repeatbut2_MouseEnter(sender As Object, e As EventArgs) Handles But_Repeat.MouseEnter
        If repeatAll Then
            But_Repeat.BackgroundImage = RepeatAllHoverImage
        ElseIf repeatOne Then
            But_Repeat.BackgroundImage = RepeatOneHoverImage
        ElseIf Form1.repeat = False Then
            But_Repeat.BackgroundImage = RepeatOffHoverImage
        End If
        repeatbuthover = True
    End Sub
    Public Sub repeatbut2_MouseLeave(sender As Object, e As EventArgs) Handles But_Repeat.MouseLeave
        If repeatAll Then
            But_Repeat.BackgroundImage = RepeatAllImage
        ElseIf repeatOne Then
            But_Repeat.BackgroundImage = RepeatOneImage
        ElseIf Form1.repeat = False Then
            But_Repeat.BackgroundImage = RepeatOffImage
        End If
        repeatbuthover = False
    End Sub
    Public Sub repeatbut2_MouseDown(sender As Object, e As MouseEventArgs) Handles But_Repeat.MouseDown
        If repeatAll Then
            But_Repeat.BackgroundImage = RepeatAllPressImage
        ElseIf repeatOne Then
            But_Repeat.BackgroundImage = RepeatOnePressImage
        ElseIf Form1.repeat = False Then
            But_Repeat.BackgroundImage = RepeatOffPressImage
        End If
    End Sub
    Public Sub repeatbut2_MouseUp(sender As Object, e As MouseEventArgs) Handles But_Repeat.MouseUp
        '  If UsingSpotify Then
        ' MySpotify.Repeat()
        ' Exit Sub
        '  End If

        If repeatbuthover = True Then
            If repeatAll Then
                But_Repeat.BackgroundImage = RepeatAllHoverImage
            ElseIf repeatOne Then
                But_Repeat.BackgroundImage = RepeatOneHoverImage
            ElseIf Form1.repeat = False Then
                But_Repeat.BackgroundImage = RepeatOffHoverImage
            End If
        Else
            '' Standard Version
            If repeatAll Then
                But_Repeat.BackgroundImage = RepeatAllImage
            ElseIf repeatOne Then
                But_Repeat.BackgroundImage = RepeatOneImage
            ElseIf Form1.repeat = False Then
                But_Repeat.BackgroundImage = RepeatOffImage
            End If
        End If

        '' Standard Version
        If repeatOne = False And repeatAll = False Then
            Form1.repeat = True
            repeatOne = True
            If UsingSpotify Then
                _SpotifyNew.SetRepeatMode(RepeatState.Track)
            Else
                My.Settings.Repeat = 1
            End If
            repeatAll = False
            But_Repeat.BackgroundImage = RepeatOneImage

        ElseIf repeatOne = True And repeatAll = False Then
            Form1.repeat = True
            repeatOne = False
            repeatAll = True
            If UsingSpotify Then
                _SpotifyNew.SetRepeatMode(RepeatState.Context)
            Else
                My.Settings.Repeat = 2
            End If
            But_Repeat.BackgroundImage = RepeatAllImage

        ElseIf repeatOne = False And repeatAll = True Then
            repeatAll = False
            Form1.repeat = False
            If UsingSpotify Then
                _SpotifyNew.SetRepeatMode(RepeatState.Off)
            Else
                My.Settings.Repeat = 0
            End If
            But_Repeat.BackgroundImage = RepeatOffImage

        End If

        firstopen = False
    End Sub
    'shuffle2
    Public Sub IsShuffleut_MouseEnter(sender As Object, e As EventArgs) Handles But_Shuffle.MouseEnter

        If ShuffleButdisabled = False Then
            But_Shuffle.BackgroundImage = ShuffleHoverImage
            'TouchShuffleBut.BackgroundImage = ShuffleHoverImage
            shuffle2hover = True
        End If
    End Sub
    Public Sub IsShuffleut_MouseLeave(sender As Object, e As EventArgs) Handles But_Shuffle.MouseLeave
        If ShuffleButdisabled = False Then
            If IsShuffle Then
                But_Shuffle.BackgroundImage = ShuffleImage
                'TouchShuffleBut.BackgroundImage = ShuffleImage
            Else
                But_Shuffle.BackgroundImage = ShuffleDisabledImage
                'TouchShuffleBut.BackgroundImage = ShuffleDisabledImage
            End If
            shuffle2hover = False
        End If
    End Sub
    Public Sub IsShuffleut_MouseDown(sender As Object, e As MouseEventArgs) Handles But_Shuffle.MouseDown
        If ShuffleButdisabled = False Then
            But_Shuffle.BackgroundImage = ShufflePressImage
            'TouchShuffleBut.BackgroundImage = ShufflePressImage
        End If
    End Sub
    Public Sub IsShuffleut_MouseUp(sender As Object, e As MouseEventArgs) Handles But_Shuffle.MouseUp
        If ShuffleButdisabled = False Then
            If shuffle2hover = True Then
                If ShuffleButdisabled = False Then
                    But_Shuffle.BackgroundImage = ShuffleHoverImage
                    'TouchShuffleBut.BackgroundImage = ShuffleHoverImage
                End If
            Else
                If ShuffleButdisabled = False Then
                    If IsShuffle Then
                        But_Shuffle.BackgroundImage = ShuffleImage
                        'TouchShuffleBut.BackgroundImage = ShuffleImage
                    Else
                        But_Shuffle.BackgroundImage = ShuffleDisabledImage
                        'TouchShuffleBut.BackgroundImage = ShuffleDisabledImage
                    End If
                End If
            End If
        End If
    End Sub
    'abut2
    Public Shared abut2hover As Boolean = False
    Public Shared abuthover As Boolean = False
    Public Sub a_MouseEnter(sender As Object, e As EventArgs) Handles But_A.MouseEnter
        But_A.BackgroundImage = AHoverImage
        abut2hover = True
    End Sub
    Public Sub a_MouseLeave(sender As Object, e As EventArgs) Handles But_A.MouseLeave
        But_A.BackgroundImage = AImage
        abut2hover = False
    End Sub
    Public Sub a_MouseDown(sender As Object, e As MouseEventArgs) Handles But_A.MouseDown
        But_A.BackgroundImage = APressImage
    End Sub
    'ab reset button
    Public Shared abresethover As Boolean = False
    Public Sub abr_MouseEnter(sender As Object, e As EventArgs) Handles But_AB_Reset.MouseEnter
        But_AB_Reset.BackgroundImage = ResetHoverImage
        abresethover = True
    End Sub
    Public Sub aabr_MouseLeave(sender As Object, e As EventArgs) Handles But_AB_Reset.MouseLeave
        But_AB_Reset.BackgroundImage = ResetImage
        abresethover = False
    End Sub
    Public Sub abr_MouseDown(sender As Object, e As MouseEventArgs) Handles But_AB_Reset.MouseDown
        But_AB_Reset.BackgroundImage = ResetPressImage
    End Sub
    Public Sub abr_MouseUp(sender As Object, e As MouseEventArgs) Handles But_AB_Reset.MouseUp
        If abresethover = True Then
            But_AB_Reset.BackgroundImage = ResetHoverImage
        Else
            But_AB_Reset.BackgroundImage = ResetImage
        End If
    End Sub
    'Bbut
    Public Shared Bbuthover As Boolean = False
    Public Sub b_MouseEnter(sender As Object, e As EventArgs) Handles But_B.MouseEnter
        But_B.BackgroundImage = BHoverImage
        Bbuthover = True
    End Sub
    Public Sub ab_MouseLeave(sender As Object, e As EventArgs) Handles But_B.MouseLeave
        But_B.BackgroundImage = BImage
        Bbuthover = False
    End Sub
    Public Sub b_MouseDown(sender As Object, e As MouseEventArgs) Handles But_B.MouseDown
        But_B.BackgroundImage = BPressImage
    End Sub
    '........speed
    'slow
    Public Shared slowbut2hover As Boolean = False
    Public Shared SlowButhover As Boolean = False
    Public Sub slowbut2_MouseEnter(sender As Object, e As EventArgs) Handles But_SpeedDown.MouseEnter
        But_SpeedDown.BackgroundImage = SlowHoverImage ' My.Resources.Slow_Arrow_Hover
        slowbut2hover = True
    End Sub
    Public Sub slowbut2_MouseLeave(sender As Object, e As EventArgs) Handles But_SpeedDown.MouseLeave
        But_SpeedDown.BackgroundImage = SlowImage ' My.Resources.Slow_Arrow
        slowbut2hover = False
    End Sub
    Public Sub slowbut2_MouseDown(sender As Object, e As MouseEventArgs) Handles But_SpeedDown.MouseDown
        But_SpeedDown.BackgroundImage = SlowPressImage ' My.Resources.Slow_Arrow_Press
    End Sub
    Public Sub slowbut2_MouseUp(sender As Object, e As MouseEventArgs) Handles But_SpeedDown.MouseUp
        If slowbut2hover = True Then
            But_SpeedDown.BackgroundImage = SlowHoverImage '  My.Resources.Slow_Arrow_Hover
        Else
            But_SpeedDown.BackgroundImage = SlowImage ' My.Resources.Slow_Arrow
        End If
    End Sub
    'fast
    Public Shared fastbut2hover As Boolean = False
    Public Shared FastButhover As Boolean = False
    Public Sub fastbut2_MouseEnter(sender As Object, e As EventArgs) Handles But_SpeedUp.MouseEnter
        But_SpeedUp.BackgroundImage = FastHoverImage ' My.Resources.Fast_Arrow_Hover
        fastbut2hover = True
    End Sub
    Public Sub fastbut2_MouseLeave(sender As Object, e As EventArgs) Handles But_SpeedUp.MouseLeave
        But_SpeedUp.BackgroundImage = FastImage ' My.Resources.Fast_Arrow
        fastbut2hover = False
    End Sub
    Public Sub fastbut2_MouseDown(sender As Object, e As MouseEventArgs) Handles But_SpeedUp.MouseDown
        But_SpeedUp.BackgroundImage = FastPressImage 'My.Resources.Fast_Arrow_Press
    End Sub
    Public Sub fastbut2_MouseUp(sender As Object, e As MouseEventArgs) Handles But_SpeedUp.MouseUp
        If fastbut2hover = True Then
            But_SpeedUp.BackgroundImage = FastHoverImage ' My.Resources.Fast_Arrow_Hover
        Else
            But_SpeedUp.BackgroundImage = FastImage ' My.Resources.Fast_Arrow
        End If
    End Sub
    Public Sub ToolTip1_Draw(sender As Object, e As DrawToolTipEventArgs) Handles ToolTip1.Draw
        e.DrawBackground()
        Dim greenPen As New Pen(Color.FromArgb(61, 61, 61))
        e.Graphics.DrawLines(
          greenPen, New Point() {
          New Point(0, e.Bounds.Height - 1),
          New Point(0, 0),
          New Point(e.Bounds.Width - 1, 0)})
        e.Graphics.DrawLines(
            greenPen, New Point() {
            New Point(0, e.Bounds.Height - 1),
            New Point(e.Bounds.Width - 1, e.Bounds.Height - 1),
            New Point(e.Bounds.Width - 1, 0)})
        e.DrawText()
    End Sub
    'Pitch
    'PitchDown
    Public Shared PitchUpButHover As Boolean = False
    Public Shared PitchDownButhover As Boolean = False
    Public Sub PitchDownbut2_MouseEnter(sender As Object, e As EventArgs) Handles But_PitchDown.MouseEnter
        But_PitchDown.BackgroundImage = SlowHoverImage ' My.Resources.Slow_Arrow_Hover
        PitchDownButhover = True
    End Sub
    Public Sub PitchDownbut2_MouseLeave(sender As Object, e As EventArgs) Handles But_PitchDown.MouseLeave
        But_PitchDown.BackgroundImage = SlowImage ' My.Resources.Slow_Arrow
        PitchDownButhover = False
    End Sub
    Public Sub PitchDownbut2_MouseDown(sender As Object, e As MouseEventArgs) Handles But_PitchDown.MouseDown
        But_PitchDown.BackgroundImage = SlowPressImage ' My.Resources.Slow_Arrow_Press
    End Sub
    Public Sub PitchDownbut2_MouseUp(sender As Object, e As MouseEventArgs) Handles But_PitchDown.MouseUp
        If PitchDownButhover = True Then
            But_PitchDown.BackgroundImage = SlowHoverImage ' My.Resources.Slow_Arrow_Hover
        Else
            But_PitchDown.BackgroundImage = SlowImage ' My.Resources.Slow_Arrow
        End If
    End Sub
    'Pitch Up
    Public Sub PitchUpBut2_MouseEnter(sender As Object, e As EventArgs) Handles But_PitchUp.MouseEnter
        But_PitchUp.BackgroundImage = FastHoverImage ' My.Resources.Fast_Arrow_Hover
        PitchUpButHover = True
    End Sub
    Public Sub PitchUpBut2_MouseLeave(sender As Object, e As EventArgs) Handles But_PitchUp.MouseLeave
        But_PitchUp.BackgroundImage = FastImage ' My.Resources.Fast_Arrow
        PitchUpButHover = False
    End Sub
    Public Sub PitchUpBut2_MouseDown(sender As Object, e As MouseEventArgs) Handles But_PitchUp.MouseDown
        But_PitchUp.BackgroundImage = FastPressImage 'My.Resources.Fast_Arrow_Press
    End Sub
    Public Sub PitchUpBut2_MouseUp(sender As Object, e As MouseEventArgs) Handles But_PitchUp.MouseUp
        If PitchUpButHover = True Then
            But_PitchUp.BackgroundImage = FastHoverImage ' My.Resources.Fast_Arrow_Hover
        Else
            But_PitchUp.BackgroundImage = FastImage ' My.Resources.Fast_Arrow
        End If
    End Sub





#End Region
    ' Use Shadows or not


#Region " Title, Artist, Album Hover"

    'Title
    Dim SongnameZOrder As Integer
    Public Sub SongName_MouseEnter(sender As Object, e As EventArgs) Handles Label_SongName.MouseEnter
        Try
            If My.Settings.MiniModeOn = False Then Return

            Label_SongName.Parent.Controls.GetChildIndex(Label_SongName, SongnameZOrder)
            Label_SongName.AutoSize = True

            Label_SongName.Parent.BringToFront()
            Label_SongName.BringToFront()
            '  ResizeBut2.BringToFront()
        Catch

        End Try

    End Sub
    Public Sub SongName_MouseLeave(sender As Object, e As EventArgs) Handles Label_SongName.MouseLeave
        Try
            If My.Settings.MiniModeOn = False Then Return
            Label_SongName.AutoSize = False
            Label_SongName.Parent.Controls.SetChildIndex(Label_SongName, SongnameZOrder)
            ' ResizeBut2.BringToFront()
        Catch

        End Try

    End Sub


    'Artist
    Dim ArtistZOrder As Integer
    Public Sub Artist_MouseEnter(sender As Object, e As EventArgs) Handles Label_Artist.MouseEnter
        Try
            If My.Settings.MiniModeOn = False Then Return
            Controls.GetChildIndex(Label_Artist, ArtistZOrder)
            Label_Artist.AutoSize = True
            '  ResizeBut2.BringToFront()
        Catch

        End Try

    End Sub
    Public Sub Artist_MouseLeave(sender As Object, e As EventArgs) Handles Label_Artist.MouseLeave
        Try
            If My.Settings.MiniModeOn = False Then Return
            Label_Artist.AutoSize = False
            Controls.SetChildIndex(Label_Artist, ArtistZOrder)
            'ResizeBut2.BringToFront()
        Catch

        End Try

    End Sub


    'Album
    Dim AlbumZOrder As Integer
    Public Sub Album_MouseEnter(sender As Object, e As EventArgs) Handles Label_Album.MouseEnter
        Try
            If My.Settings.MiniModeOn = False Then Return
            Controls.GetChildIndex(Label_Album, AlbumZOrder)
            Label_Album.AutoSize = True
            'ResizeBut2.BringToFront()
        Catch

        End Try

    End Sub
    Public Sub Album_MouseLeave(sender As Object, e As EventArgs) Handles Label_Album.MouseLeave
        Try
            If My.Settings.MiniModeOn = False Then Return
            Label_Album.AutoSize = False
            Controls.SetChildIndex(Label_Album, AlbumZOrder)
            'ResizeBut2.BringToFront()
        Catch

        End Try

    End Sub

#End Region


#End Region



End Class