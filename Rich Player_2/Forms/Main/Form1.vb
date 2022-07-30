#Region " Imports etc."
Option Infer On
Option Explicit On

'...................Taskbar....................
Imports Microsoft.WindowsAPICodePack
Imports Microsoft.WindowsAPICodePack.Taskbar
Imports Microsoft.WindowsAPICodePack.Dialogs

'..............Audio...............
Imports CoreAudio
Imports Ewk.SoundCloud
Imports Ewk.Extensions

'..............System...............
Imports System
Imports System.ComponentModel
Imports System.Security.Permissions
Imports System.IO
Imports System.IO.Ports
Imports System.Xml.Serialization
Imports System.Runtime.InteropServices
Imports System.Drawing.Imaging
' Imports System.Net
Imports System.Web.HttpUtility
Imports System.Management
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Drawing
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Forms
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
' Imports System.Web
Imports System.Xml.XPath
Imports System.Text
Imports System.Reflection
Imports System.Net.Mail


'............Dev Express.............
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraTab
Imports DevExpress.Skins
Imports DevExpress.LookAndFeel
Imports DevExpress.UserSkins
Imports DevExpress
Imports DevExpress.XtraBars

Imports Rich_Player.DXSample


'....................AUDIO........................
Imports HundredMilesSoftware.UltraID3Lib
Imports Rich_Player.AudioController
Imports Un4seen.Bass
Imports Un4seen.Bass.AddOn.Fx
Imports Rich_Player.PitchAndShiftAudio
Imports Un4seen.Bass.Misc
Imports DevExpress.XtraTab.ViewInfo
Imports Un4seen.Bass.AddOn.Enc
Imports MediaInfoNET

Imports SpotifyAPI.Web
Imports SpotifyAPI.Local
Imports SpotifyAPI.Web.Enums
Imports SpotifyAPI.Web.Models
Imports SpotifyAPI.Web.Auth

Imports Unosquare.Labs.EmbedIO
Imports Unosquare.Labs.EmbedIO.Constants
Imports Unosquare.Labs.EmbedIO.Modules


'......................YOUTUBE........................
Imports Google.Apis.Services
Imports Google.Apis.YouTube.v3
Imports Google.Apis.YouTube.v3.Data
Imports System.Text.RegularExpressions

#End Region

Namespace CsWinFormsBlackApp
    Partial Public Class Form1 : Inherits DevExpress.XtraEditors.XtraForm
#Region " Initialize"
        Public xcarform As New CarForm

        Public Sub New()
            InitializeComponent()
            'Artwork.EnableGesture(Telerik.WinControls.GestureType.All)
        End Sub
        <STAThread>
        Shared Sub Main()

            Dim instanceCountOne As Boolean = False

            Dim AppName As String = "RichPlayer" & IO.Path.GetFileName(Application.StartupPath)
            Using mtex As Threading.Mutex = New Threading.Mutex(True, AppName, instanceCountOne)
                If instanceCountOne Then


                    Application.EnableVisualStyles()
                    BonusSkins.Register()
                    SkinManager.EnableFormSkins()
                    UserLookAndFeel.Default.SetSkinStyle("DevExpress Style Dark")

                    ' Disable BASS.NET splash screen
                    BassNet.Registration(My.Settings.BassEmail, My.Settings.BassCode)

                    ' init BASS using the default output device 
                    If My.Settings.DefaultDevice = 0 Then
                        My.Settings.DefaultDevice = -1
                    End If
                    Dim activeform As Form = Form1
                    Bass.BASS_Init(My.Settings.DefaultDevice, 48000, BASSInit.BASS_DEVICE_DEFAULT, activeform.Handle)

                    If My.Settings.FirstTimeSetup Then
                        Try
                            Application.Run(New SetupForm())
                        Catch ex As Exception
                            SetupForm.Show()
                        End Try
                    Else
                        '  If AppOpen = False Then
                        Application.Run(New Form1())
                        '  End If
                    End If



                    mtex.ReleaseMutex()
                Else
                    MessageBox.Show("Rich Player is already running")
                End If
            End Using


        End Sub



#End Region
#Region " Declarations"

        <DllImport("user32.dll", SetLastError:=True)> _
        Private Shared Function FindWindow(lpClassName As String, lpWindowName As String) As IntPtr
        End Function
        <DllImport("user32.dll")> _
        Private Shared Function SendMessage(hWnd As IntPtr, wMsg As Int32, wParam As Boolean, lParam As Int32) As Integer
        End Function
        Private Declare Function RegisterWindowMessageA Lib "User32" (ByVal M As String) As Integer
        Private Declare Function RegisterShellHookWindow Lib "User32" (ByVal hWnd As IntPtr) As Boolean

        'Main Stuff
        Private Declare Auto Function SetForegroundWindow Lib "user32" (ByVal hWnd As IntPtr) As Boolean

        'Form
        Public Shared AppOpen As Boolean = True
        Public Shared AppOpenFinished As Boolean = False
        Public Shared AppFirstLoaded As Boolean = True
        Public Shared AppFullyLoaded As Boolean = False
        Public Shared FormRestarting As Boolean = False

        Public origWidth As Integer = Me.Width
        Public origHeight As Integer = Me.Height
        Public Shared LocationChanged As Boolean = False
        Public Shared FormActivated As Boolean = False
        Public Shared xpos As Integer
        Public Shared LastSize As Size
        Public Shared AllowSizeChange As Boolean = False
        Public Shared MaximumSizeChanged As Boolean = False
        Public Shared ctrlpress As Boolean = False
        Public Shared shiftpress As Boolean = False
        Public Shared temp_index As Integer

        'Media
        Public Shared isVLCplaying As Boolean
        Public Shared VLCplayer_buffering As Boolean
        Public Shared VLCplayer_firstbuffer As Boolean = True
        Public Shared VLCmediaOpened As Boolean = False
        Public Shared VLCChapterMarks_IsClicking As Boolean = False
        Public Shared SongStartOver As Boolean = False

        Public Shared MP3Tag As New UltraID3

        'VLC
        Public Shared VlcPlayer As New AxAXVLC.AxVLCPlugin2
        Public Shared VLCtimer As New Timer
        Public Shared VLCChapterMarks As New RichListBox
        ' Public Shared VlcPlayer As New VlcControl

        'BASS.net
        Public Shared timerUpdate As New Timer
        Public Shared SpectrumTimer As New Timer

        'Radio
        Dim RadioPlayer As New Gecko.GeckoWebBrowser


        'Equalizer
        Public Shared EQ_FirstTimeOpen As Boolean = True

        'File Browsers
        Public Shared OpenFolder As New CommonOpenFileDialog
        Public Shared OpenProgressBar As New XtraEditors.ProgressBarControl
        Public Shared OpenProgressBarBackgroundWorker As New BackgroundWorker

        'Seek
        Public Shared Property ProgressValue As Double
        Public Shared totaltimeduration As TimeSpan
        Public Shared SeekBarMaxVal As Integer = 1999999999
        Public Shared SubtitlesAdded As Boolean = False
        Public Shared AudiosAdded As Boolean = False

        'Repeat
        Public Shared repeat As Boolean = False
        Public Shared repeatOne As Boolean = False
        Public Shared repeatAll As Boolean = False

        'Add Controls
        Public Shared Panellyrics As New Panel
        Public Shared LabelLyrics As New RichLabel
        Public Shared TextboxLyrics As New TextBox
        Public Shared AutoSaveTimer As New Timer
        Public Meta_and_Artwork_Timer2 As New Timers.Timer
        Public PlaylistTabs As XtraTabControl

        'Playlists
        Public Shared IsVideoFile As Boolean = False
        Public Shared IsFolder As Boolean = False
        Public Shared firstopen As Boolean = True
        Public Shared firstopen2 As Boolean = True

        'Locations of Playlist in App Folder
        Public Shared TempPlaylist As String = Application.StartupPath & "\Playlists\Default Playlist.richplaylist"
        Public Shared TempPlaylistNew As String = Application.StartupPath & "\Playlists\Temp_Playlist"

        'YouTube Playlists
        Public Shared TempYouTubePlaylist As String = Application.StartupPath & "\Playlists\Youtube\TempYouTubePlaylist.rplyt" 'richyoutubeplaylist"
        Public Shared TempYouTubePlaylistImages As String = Application.StartupPath & "\Playlists\Youtube\Images\"

        Public Shared TempYouTubePlaylistFull As String = Application.StartupPath & "\Playlists\Youtube\TempYouTubePlaylistFull.richyoutubeplaylist"
        Public Shared TempYouTubeArtistPlaylist As String = Application.StartupPath & "\Playlists\Youtube\TempYouTubeArtistPlaylist.richyoutubeplaylist"
        Public Shared TempYouTubeAlbumPlaylist As String = Application.StartupPath & "\Playlists\Youtube\TempYouTubeAlbumPlaylist.richyoutubeplaylist"


        'Lyrics
        Public Shared TempLyrics As String = Application.StartupPath & "\Lyrics\temp_Lyrics.dat"
        Public Shared LyricsTimer As New Timer


        'Settings
        Public Shared INI_SettingsFile As String = Application.StartupPath + "\Settings.ini"
        Public Shared DoResetSettings As Boolean = False


        'Command Line
        Public Shared AddedPlaylistFromComamandLine As Boolean = False

#Region "Graphics"
        Public Shared PlayImage As System.Drawing.Image = My.Resources.Play_3
        Public Shared PlayHoverImage As System.Drawing.Image = My.Resources.Play_3_Hover
        Public Shared PlayPressImage As System.Drawing.Image = My.Resources.Play_3_Press

        Public Shared PauseImage As System.Drawing.Image = My.Resources.Pause_3
        Public Shared PauseHoverImage As System.Drawing.Image = My.Resources.Pause_3_Hover
        Public Shared PausePressImage As System.Drawing.Image = My.Resources.Pause_3_Press

        Public Shared StopImage As System.Drawing.Image = My.Resources.Stop_3
        Public Shared StopHoverImage As System.Drawing.Image = My.Resources.Stop_3_Hover
        Public Shared StopPressImage As System.Drawing.Image = My.Resources.Stop_3_Press

        Public Shared NextImage As System.Drawing.Image = My.Resources.Next_3
        Public Shared NextHoverImage As System.Drawing.Image = My.Resources.Next_3_Hover
        Public Shared NextPressImage As System.Drawing.Image = My.Resources.Next_3_Press

        Public Shared PreviousImage As System.Drawing.Image = My.Resources.Previous_3
        Public Shared PreviousHoverImage As System.Drawing.Image = My.Resources.Previous_3_Hover
        Public Shared PreviousPressImage As System.Drawing.Image = My.Resources.Previous_3_Press

        Public Shared RepeatAllImage As System.Drawing.Image = My.Resources.New_Design_RepeatAll_Active
        Public Shared RepeatAllHoverImage As System.Drawing.Image = My.Resources.New_Design_RepeatAll_Hover
        Public Shared RepeatAllPressImage As System.Drawing.Image = My.Resources.New_Design_RepeatAll_Press

        Public Shared RepeatOneImage As System.Drawing.Image = My.Resources.New_Design_RepeatOne_Active
        Public Shared RepeatOneHoverImage As System.Drawing.Image = My.Resources.New_Design_RepeatOne_Hover
        Public Shared RepeatOnePressImage As System.Drawing.Image = My.Resources.New_Design_RepeatOne_Press

        Public Shared RepeatOffImage As System.Drawing.Image = My.Resources.New_Design_RepeatOff
        Public Shared RepeatOffHoverImage As System.Drawing.Image = My.Resources.New_Design_RepeatOff_Hover
        Public Shared RepeatOffPressImage As System.Drawing.Image = My.Resources.New_Design_RepeatOff_Press

        Public Shared ShuffleImage As System.Drawing.Image = My.Resources.New_Design_Shuffle_Active
        Public Shared ShuffleDisabledImage As System.Drawing.Image = My.Resources.New_Design_Shuffle
        Public Shared ShuffleHoverImage As System.Drawing.Image = My.Resources.New_Design_Shuffle_Hover
        Public Shared ShufflePressImage As System.Drawing.Image = My.Resources.New_Design_Shuffle_Press

        Public Shared ResetImage As System.Drawing.Image = My.Resources.New_Design_reset
        Public Shared ResetHoverImage As System.Drawing.Image = My.Resources.New_Design_reset_Hover
        Public Shared ResetPressImage As System.Drawing.Image = My.Resources.New_Design_reset_Press

        Public Shared TitleBarImage As System.Drawing.Image = Nothing ' My.Resources.TitleBar_Gradient  'My.Resources.RichPlayer_Design_Titlebar
        Public Shared AppNameActiveImage As System.Drawing.Image = My.Resources.AppName_Colored_Active 'My.Resources.RichPlayer_Design_AppName_Active_trans
        Public Shared AppNameImage As System.Drawing.Image = My.Resources.AppName_Active_Colored_Deactive ' My.Resources.RichPlayer_Design_AppName_trans

        Public Shared ControlsBgImage As System.Drawing.Image = My.Resources.RichPlayer_Design_ControlsBG_2

        Public Shared TitleBarCloseImage As System.Drawing.Image = My.Resources.TitleBar_Close
        Public Shared TitleBarCloseHoverImage As System.Drawing.Image = My.Resources.TitleBar_Close_Hover
        Public Shared TitleBarClosePressImage As System.Drawing.Image = My.Resources.TitleBar_Close_Press

        Public Shared TitleBarMaxImage As System.Drawing.Image = My.Resources.TitleBar_Max
        Public Shared TitleBarMaxHoverImage As System.Drawing.Image = My.Resources.TitleBar_Max_Hover
        Public Shared TitleBarMaxPressImage As System.Drawing.Image = My.Resources.TitleBar_Max_Press

        Public Shared TitleBarMinImage As System.Drawing.Image = My.Resources.TitleBar_Min
        Public Shared TitleBarMinHoverImage As System.Drawing.Image = My.Resources.TitleBar_Min_Hover
        Public Shared TitleBarMinPressImage As System.Drawing.Image = My.Resources.TitleBar_Min_Press

        Public Shared FormOutlineImage As System.Drawing.Image = My.Resources.form_outline

        Public Shared SlowImage As System.Drawing.Image = My.Resources.Prev_3
        Public Shared SlowHoverImage As System.Drawing.Image = My.Resources.Prev_3_Hover
        Public Shared SlowPressImage As System.Drawing.Image = My.Resources.Prev_3_Press

        Public Shared FastImage As System.Drawing.Image = My.Resources.Next_4
        Public Shared FastHoverImage As System.Drawing.Image = My.Resources.Next_4_Hover
        Public Shared FastPressImage As System.Drawing.Image = My.Resources.Next_4_Press

        Public Shared AImage As System.Drawing.Image = My.Resources.New_Design_A
        Public Shared AHoverImage As System.Drawing.Image = My.Resources.New_Design_A_Hover
        Public Shared APressImage As System.Drawing.Image = My.Resources.New_Design_A_Press

        Public Shared BImage As System.Drawing.Image = My.Resources.New_Design_B
        Public Shared BHoverImage As System.Drawing.Image = My.Resources.New_Design_B_Hover
        Public Shared BPressImage As System.Drawing.Image = My.Resources.New_Design_B_Press
#End Region

        Public SpectrumColor1 As Color
        Public SpectrumColor2 As Color


        'Spotify
        ' Public Shared _spotify As New SpotifyLocalAPI
        '  Public _currentTrack As SpotifyAPI.Local.Models.Track
        Public _spotifyWeb As New SpotifyWebAPI
        '  Public _auth As New Auth.ImplicitGrantAuth
        Public _profile As New SpotifyAPI.Web.Models.PrivateProfile
        Public _savedTracks As New List(Of SpotifyAPI.Web.Models.FullTrack)
        Public _playlists As New List(Of SpotifyAPI.Web.Models.SimplePlaylist)
        Public SpotifyTimer As New Timer


        'Prepare For Lyrics
        Public Function Parse(ByVal Html As String, ByVal Before As String, ByVal After As String, ByVal Offset As Integer) As String

            If String.IsNullOrEmpty(Html) Then Return String.Empty
            If Html.Contains(Before) Then
                Dim Returned As String = Html.Substring(Html.IndexOf(Before) + Offset)
                If Returned.Contains(After) AndAlso Not String.IsNullOrEmpty(After) Then Returned = Returned.Substring(0, Returned.IndexOf(After))
                Return Returned
            Else
                Return String.Empty
            End If

        End Function

        'Opacity
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

        Public Sub wait(ByVal seconds As Single)
            For i As Integer = 0 To seconds * 10
                System.Threading.Thread.Sleep(10)
                Application.DoEvents()
            Next
        End Sub


        Public SearchBox_Panel As New Panel
        Public but_Refresh As New PictureBox



#End Region
#Region " Minimize on taskbar item click"
        Protected Overrides ReadOnly Property CreateParams As CreateParams
            Get
                Const WS_MINIMIZEBOX As Integer = &H20000
                Dim cp As CreateParams = MyBase.CreateParams
                cp.Style = cp.Style Or WS_MINIMIZEBOX
                Return cp
            End Get
        End Property
#End Region


#Region " FORM"


#Region " Form  |   Load"
#Region " Add Handles"
        Public Sub AddAllHandles()

            AddHandler bgw.DoWork, AddressOf bgw_DoWork
            AddHandler bgw.ProgressChanged, AddressOf bgw_ProgressChanged
            AddHandler bgw.RunWorkerCompleted, AddressOf bgw_RunWorkerCompleted
            AddHandler Meta_and_Artwork_Timer2.Elapsed, AddressOf Meta_and_Artwork_Timer_Tick
            AddHandler OpenProgressBarBackgroundWorker.DoWork, AddressOf OpenProgressBarBackgroundWorker_DoWork
            AddHandler OpenProgressBarBackgroundWorker.ProgressChanged, AddressOf OpenProgressBarBackgroundWorker_ProgressChanged
            AddHandler OpenProgressBarBackgroundWorker.RunWorkerCompleted, AddressOf OpenProgressBarBackgroundWorker_RunWorkerCompleted

            AddHandler Microsoft.Win32.SystemEvents.SessionEnding, AddressOf Handler_SessionEnding

        

            AddHandler LyricsTimer.Tick, AddressOf LyricsTimer_Tick
            AddHandler AutoSaveTimer.Tick, AddressOf AutoSaveTimer_Tick

            AddHandler RadioPlayer.Navigated, AddressOf RadioPlayer_Navigated
            AddHandler RadioTimer.Tick, AddressOf RadioTimer_Tick

            RadioTimer.Interval = 1000


        End Sub
#End Region


        'Main
        Public Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load



            LoadApp()
            ' wait(2)
            If My.Settings.DriveMode Then
                If My.Settings.CarFormOpened = False Then
                    InitDriveMode()
                End If
            End If


            'RefreshPlaylists()
        End Sub

        Public Sub LoadApp()

            Me.BringToFront()
            PreVentFlicker()
            '  Me.Opacity = 0.1
            My.Settings.DriveMode = False
            Me.KeyPreview = True
            Select Case My.Settings.DriveMode
                Case True : PlaylistTabs = xcarform.PlaylistTabs
                Case False : PlaylistTabs = Me.PlaylistTabss
            End Select

            AddHandler PlaylistTabs.CustomHeaderButtonClick, AddressOf XtraTabControl1_CustomHeaderButtonClick
            AddHandler PlaylistTabs.TabMiddleClick, AddressOf XtraTabControl1_TabMiddleClick
            AddHandler PlaylistTabs.DragDrop, AddressOf OnDragDrop
            AddHandler PlaylistTabs.DragLeave, AddressOf OnDragLeave
            AddHandler PlaylistTabs.DragOver, AddressOf OnDragOver
            AddHandler PlaylistTabs.MouseDown, AddressOf OnMouseDown
            AddHandler PlaylistTabs.MouseMove, AddressOf OnMouseMove
            AddHandler PlaylistTabs.MouseUp, AddressOf XtraTabControl1_MouseUp
            AddHandler PlaylistTabs.GiveFeedback, AddressOf OnGiveFeedback
            AddHandler PlaylistTabs.TabMiddleClick, AddressOf XtraTabControl1_TabMiddleClick
            AddHandler PlaylistTabs.SelectedPageChanging, AddressOf XtraTabControl1_SelectedPageChanging
            AddHandler PlaylistTabs.SelectedPageChanged, AddressOf XtraTabControl1_SelectedPageChanged

            '  AddHandler VlcPlayer.VlcLibDirectoryNeeded, AddressOf checkdir2

            If PlaylistsImported Then Return
            SetupTrackBars()
            BarCheckBox_RemoveTBGrad.Checked = True
            SuspendDrawing()
            Try
                AppFirstLoaded = True : firstopen = True : NeedResizeRefresh = True
                Me.ShowInTaskbar = True : Control.CheckForIllegalCrossThreadCalls = False
                AddAllHandles()

                FontDialog1.Font = My.Settings.PlaylistsFont
                Setup_Players() : Setup_Playlists() : BringLastPlaylist_to_Focus() : OpenWith_CommandLine()

                BarCheckbox_UseShadows.Checked = My.Settings.UseIconShadows
                Refresh_ButtonGraphics()
                Setup_Visuals() : AddHandles() : AddAllOtherStuff() : AppOpen = True : SetupPlaylistTabsOrientation()

                AppOpen = False : firstopen = False

                If My.Settings.DriveMode = False Then PlayOnStart()
                Try : If Not PlaylistTabs.SelectedTabPage.PageVisible Then
                        TryCast(PlaylistTabs, XtraTabControl).MakePageVisible(PlaylistTabs.SelectedTabPage)
                    End If
                Catch : End Try

                AppFirstLoaded = False
            Catch ex As Exception : MyMsgBox.Show(ex.ToString, "Error loading app", True) : End Try

            Me.KeyPreview = True
            Me.Refresh() : RefreshApp()

            If My.Settings.FirstTimeSetup Then
                AppOpenFinished = True : Me.Refresh() : RefreshApp()
                SkinChange_Playlists_Standard()
            End If

            '...................................Enhanced Skins......................................
            Try
                Select Case My.Settings.CustomImageCheckState
                    Case True

                        If My.Settings.hasbackground Then
                            HasEnahancedBackground = True
                        End If
                        BarCheckBox_UseCustomImage.Checked = False
                        BarCheckBox_UseCustomImage.Checked = True
                        Me.BackgroundImageLayout = My.Settings.BackgroundImageLayout
                    Case False

                        Setup_Enhanced_Skins()
                End Select
            Catch : MyMsgBox.Show("Error: 0x5: Error Loading skin. To fix, re-apply your desired settings.", "", True) : End Try

            ResumeDrawing()
            TitleBarImage = Nothing
            Window_Titlebar.BackgroundImage = Nothing
            SuspendDrawing()
            If My.Settings.MiniModeOn = False Then
                Me.Size = New Size(My.Settings.FormSize)
            End If

            Me.MaximumSize = Screen.FromRectangle(Me.Bounds).WorkingArea.Size
            pictureform.MaximumSize = Screen.FromRectangle(Me.Bounds).WorkingArea.Size
            ResumeDrawing()
            Splitter.SplitterPosition = My.Settings.SplitterVal


            SetupArtworkDragandDrop()
            AddHandler but_Refresh.Click, AddressOf RefreshApp
            ToolTip1.SetToolTip(but_Refresh, "Refresh App")
            but_Refresh.Size = New Size(18, 18)
            but_Refresh.BackgroundImage = My.Resources.Browser_Refresh
            but_Refresh.BackgroundImageLayout = ImageLayout.Zoom
            but_Refresh.BackColor = TextBox_PlaylistSearch.BackColor
            SearchBox_Panel.Height = 21
            PlaylistTabs.TabPages(PlaylistTabs.SelectedTabPageIndex).Controls.Add(SearchBox_Panel)
            SearchBox_Panel.Dock = DockStyle.Top
            SearchBox_Panel.Controls.Add(TextBox_PlaylistSearch)
            TextBox_PlaylistSearch.MinimumSize = New Size(0, 0)
            TextBox_PlaylistSearch.MaximumSize = New Size(0, 0)
            TextBox_PlaylistSearch.Dock = DockStyle.Fill
            ' PlaylistTabs.TabPages(PlaylistTabs.SelectedTabPageIndex).Controls.Add(TextBox_PlaylistSearch)
            ' TextBox_PlaylistSearch.Dock = DockStyle.Top
            TextBox_PlaylistSearch.Controls.Add(But_Search)
            But_Search.Dock = DockStyle.Right
            But_Search.MaximumSize = New Size(0, TextBox_PlaylistSearch.Height - 6)
            But_Search.BringToFront()
            But_Search.Top = 3

            SearchBox_Panel.Controls.Add(but_Refresh)
            but_Refresh.Dock = DockStyle.Left
            TextBox_PlaylistSearch.Padding = New Padding(10, 3, 3, 3)
        End Sub
        Public Sub Form1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
            AppShown()
        End Sub
        Public Sub AppShown()
            SuspendDrawing()
            If AppOpen = False Then
                Timer_PlaylistsSizes.Start()
                Splitter.SplitterPosition = My.Settings.SplitterVal : Splitter.Size = New Size(Splitter.Size.Width, Me.Height - ControlsBGPanel.Height - 40 - 20 - 30 + 30)
            End If

            For Each c As Control In Me.Controls : AddHandler c.KeyUp, AddressOf MyBaseKeyUp : Next
            Me.KeyPreview = True
            '  RefreshPlaylists()

            PlaylistTabs.MakePageVisible(PlaylistTabs.SelectedTabPage)
            '  Me.Refresh() : RefreshApp()

            ' Me.FormBorderStyle = FormBorderStyle.None
            '  Me.FormBorderEffect = XtraEditors.FormBorderEffect.Shadow
            RefreshTaskbarIcon()
            ResumeDrawing()

            TimerSimple.Start()

            On Error Resume Next
        End Sub

        Private Sub TimerSimple_Tick(sender As Object, e As EventArgs) Handles TimerSimple.Tick
            SuspendDrawing()
            If My.Settings.SimplisticMode Then
                Simplistic_Mode_On()
            End If
            ResumeDrawing()
            TimerSimple.Stop()
        End Sub

        Public Sub RefreshTaskbarIcon()
            ' SuspendDrawing()
            SendMessage(Me.Handle, &H112, &HF020, 0)  ' Minimize Window

            TimerStartup.Interval = 1
            TimerStartup.Start()

        End Sub

        Dim startupCounter As Integer = 0
        Private Sub TimerStartup_Tick(sender As Object, e As EventArgs) Handles TimerStartup.Tick
            Select Case startupCounter
                Case 0
                    SendMessage(Me.Handle, &H112, &HF120, 0) ' Restore Window to force Taskbar Icon to Appear
                    startupCounter += 1

                Case 1
                    startupCounter += 1

                Case 2
                    '     ResumeDrawing()
                    TimerStartup.Stop()

            End Select

        End Sub



        Public Sub RefreshPlaylists()
            If Not Application.OpenForms().OfType(Of Form1).Any Then
                RefreshPlaylists()
            End If

            Select Case My.Settings.DriveMode
                Case True : PlaylistTabs = xcarform.PlaylistTabs
                Case False : PlaylistTabs = Me.PlaylistTabss
            End Select
            AddHandler PlaylistTabs.CustomHeaderButtonClick, AddressOf XtraTabControl1_CustomHeaderButtonClick
            AddHandler PlaylistTabs.TabMiddleClick, AddressOf XtraTabControl1_TabMiddleClick
            AddHandler PlaylistTabs.DragDrop, AddressOf OnDragDrop
            AddHandler PlaylistTabs.DragLeave, AddressOf OnDragLeave
            AddHandler PlaylistTabs.DragOver, AddressOf OnDragOver
            AddHandler PlaylistTabs.MouseDown, AddressOf OnMouseDown
            AddHandler PlaylistTabs.MouseMove, AddressOf OnMouseMove
            AddHandler PlaylistTabs.MouseUp, AddressOf XtraTabControl1_MouseUp
            AddHandler PlaylistTabs.GiveFeedback, AddressOf OnGiveFeedback
            AddHandler PlaylistTabs.TabMiddleClick, AddressOf XtraTabControl1_TabMiddleClick
            AddHandler PlaylistTabs.SelectedPageChanging, AddressOf XtraTabControl1_SelectedPageChanging
            AddHandler PlaylistTabs.SelectedPageChanged, AddressOf XtraTabControl1_SelectedPageChanged
        End Sub

#Region " Setup  |   Players"

        Public Sub Setup_Players()
            '.......................BASS.NET......................
            Try
                BASS_LoadPlugins()
                LoadAudioSettings()
            Catch ex As Exception
                MyMsgBox.Show("Error: 0x1: Error loading BASS.NET Audio Player! Please check your references. Visit www.rexfordrich.com/files.html for references.zip download.", "", True)
            End Try
            '...............................VLC..........................

            Try
                Setup_VLC()
            Catch ex As Exception
                Dim message As String
                message = ("Error: 0x2: VLC should be installed for best video use!" & vbCrLf & vbCrLf & "Do you wish to install now?" & vbCrLf & vbCrLf) '& ex.Message)
                Dim result As Integer = MyFullMsgBox.Show(message, "New version of VLC must be installed!", True, MyFullMsgBox.CustomButtons.YesNo)
                If result = DialogResult.No Then
                    'Application.Exit()
                    ' Application.ExitThread()
                ElseIf result = DialogResult.Yes Then
                    Process.Start("http://www.rexfordrich.com/vlc.html")
                    Application.Exit()
                    Application.ExitThread()
                End If
            End Try
        End Sub
        Public Sub BASS_LoadPlugins()
            Dim targetPath As String = Application.StartupPath
            If Utils.Is64Bit Then
                targetPath = Path.Combine(targetPath, "lib/x64")
            Else
                targetPath = Path.Combine(targetPath, "lib/x86")
            End If

            If PSAudioUtils.IsWindowsOS() Then
                Bass.LoadMe(targetPath)
                BassFx.LoadMe(targetPath)
                BassEnc.LoadMe(targetPath)
            End If
            Dim files() As String = IO.Directory.GetFiles(targetPath)
            For Each file As String In files
                Bass.BASS_PluginLoad(file)
            Next
        End Sub
        Dim VersionNumber As String
        Dim version As Integer
        Dim VLC_installed As Boolean = False
        Public Sub Setup_VLC()

            If IO.File.Exists("C:\Program Files\VideoLAN\VLC\vlc.exe") Then
                VLC_installed = True
            ElseIf IO.File.Exists("C:\Program Files (x86)\VideoLAN\VLC\vlc.exe") Then
                VLC_installed = True
            End If
            If VLC_installed Then
                If My.Settings.DriveMode Then
                    CarForm.PanelPlayback.Controls.Add(VlcPlayer)
                    CarForm.ResetVLC()
                Else
                    Splitter.Panel1.Controls.Add(VlcPlayer)
                    If vlcFailed Then
                        If My.Settings.MiniModeOn Then
                            Try
                                VlcPlayer.Location = Window_Titlebar.PointToClient(Me.VlcPlayer.Parent.PointToScreen(Me.VlcPlayer.Location))
                            Catch : End Try
                            Try
                                VlcPlayer.Parent = Window_Titlebar
                            Catch : End Try
                            Try
                                VlcPlayer.BringToFront()
                            Catch : End Try
                            Try
                                VlcPlayer.Anchor = AnchorStyles.Top Or AnchorStyles.Left
                            Catch : End Try
                            Try
                                VlcPlayer.Visible = True
                            Catch : End Try
                            Try
                                VlcPlayer.Location = New System.Drawing.Point(12, 78)
                            Catch : End Try
                            Try
                                VlcPlayer.MinimumSize = New System.Drawing.Size(143, 143)
                            Catch : End Try
                            Try
                                VlcPlayer.Size = New System.Drawing.Size(143, 143)
                            Catch : End Try
                        Else
                            Try
                                Me.VlcPlayer.Location = Me.Splitter.Panel1.PointToClient(Me.VlcPlayer.Parent.PointToScreen(Me.VlcPlayer.Location))
                            Catch : End Try
                            Try
                                Me.VlcPlayer.Parent = Splitter.Panel1
                            Catch : End Try
                            Try
                                Artwork.BringToFront()
                            Catch : End Try
                            Try
                                VlcPlayer.Top = Artwork.Top
                            Catch : End Try
                            Try
                                VlcPlayer.Size = Artwork.Size
                            Catch : End Try
                            Try
                                VlcPlayer.Location = Artwork.Location
                            Catch : End Try
                        End If
                    End If
                End If

                Try
                    VlcPlayer.Anchor = (AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right)
                Catch : End Try

                Try
                    VlcPlayer.AutoLoop = False
                    VlcPlayer.AutoPlay = True
                Catch ex As Exception
                End Try
                Try
                    VlcPlayer.CtlVisible = False
                Catch : End Try
                Try
                    VlcPlayer.FullscreenEnabled = True
                Catch : End Try
                Try
                    VlcPlayer.Toolbar = False
                Catch : End Try
                Try
                    VlcPlayer.Visible = False
                Catch : End Try
                Try
                    VlcPlayer.volume = 100
                Catch : End Try
                Try
                    VlcPlayer.audio.volume = 100
                Catch : End Try
                Try
                    VlcPlayer.BringToFront()
                Catch : End Try
                If vlcFailed = False Then
                    Splitter.Panel2.Controls.Add(VLCChapterMarks)
                    VLCChapterMarks.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left ' AnchorStyles.Right
                    VLCChapterMarks.Location = New Point(0, 0) '(VlcPlayer.Location.X + VlcPlayer.Width, VlcPlayer.Location.Y)
                    VLCChapterMarks.Size = New Size(110, VlcPlayer.Height) '46
                    VLCChapterMarks.Font = New Font("Segoe UI", 8, FontStyle.Regular)
                    VLCChapterMarks.Visible = False
                    VLCChapterMarks.BorderStyle = BorderStyles.NoBorder
                    VLCChapterMarks.BackColor = Color.Gray
                    VLCChapterMarks.ForeColor = Color.Gainsboro
                    VLCChapterMarks.Location = Me.Splitter.Panel2.PointToClient(Me.VLCChapterMarks.Parent.PointToScreen(Me.VLCChapterMarks.Location))
                    VLCChapterMarks.Parent = Splitter.Panel2
                    VLCChapterMarks.BringToFront()

                    AddHandler VLCChapterMarks.MouseClick, AddressOf VLCChapterMarks_Click
                    AddHandler VLCChapterMarks.MouseDown, AddressOf VLCChapterMarks_MouseDown
                    AddHandler VLCChapterMarks.MouseUp, AddressOf VLCChapterMarks_MouseUp
                    AddHandler VLCChapterMarks.SelectedIndexChanged, AddressOf VLCChapterMarks_SelectedIndexChanged
                    AddHandler VLCChapterMarks.VisibleChanged, AddressOf VLCChapterMarks_VisiblityChanged
                    AddHandler Splitter.Panel2.SizeChanged, AddressOf VLCChapterMarks_VisiblityChanged
                    AddHandler AudioPlayer.Instance.AudioHandleLoaded, AddressOf Prepare
                    AddHandler timerUpdate.Tick, AddressOf timerUpdate_Tick
                    timerUpdate.Interval = 1
                    timerUpdate.Start()

                    AddHandler SpectrumTimer.Tick, AddressOf SpectrumTimer_Tick
                    SpectrumTimer.Interval = 50
                    SpectrumTimer.Start()

                    YoutubeBrowser.SendToBack()

                    VersionNumber = VlcPlayer.VersionInfo.Substring(0, 5)
                    version = CInt(VersionNumber.Replace(".", ""))
                End If
                AddHandler VlcPlayer.MediaPlayerBuffering, AddressOf VLCplayer_MediaPlayerBuffering
                AddHandler VlcPlayer.MediaPlayerEndReached, AddressOf VLCplayer_MediaPlayerEndReached
                AddHandler VlcPlayer.MediaPlayerOpening, AddressOf VLCplayer_MediaPlayerOpening
                AddHandler VlcPlayer.MediaPlayerPaused, AddressOf VLCplayer_MediaPlayerPaused
                AddHandler VlcPlayer.MediaPlayerPlaying, AddressOf VLCplayer_MediaPlayerPlaying
                AddHandler VlcPlayer.MediaPlayerStopped, AddressOf VLCplayer_MediaPlayerStopped
                AddHandler VlcPlayer.MediaPlayerPositionChanged, AddressOf VLCplayer_MediaPlayerPositionChanged
                AddHandler VlcPlayer.MediaPlayerTitleChanged, AddressOf VLCplayer_TitleChanged
                AddHandler VlcPlayer.VisibleChanged, AddressOf VLCplayer_VisibleChanged
                AddHandler VlcPlayer.SizeChanged, AddressOf Artwork_SizeChanged
                vlcFailed = False







            End If
            AddHandler Splitter.Panel2.SizeChanged, AddressOf VLCChapterMarks_VisiblityChanged
            AddHandler AudioPlayer.Instance.AudioHandleLoaded, AddressOf Prepare
            AddHandler timerUpdate.Tick, AddressOf timerUpdate_Tick
            timerUpdate.Interval = 1
            timerUpdate.Start()

            AddHandler SpectrumTimer.Tick, AddressOf SpectrumTimer_Tick
            SpectrumTimer.Interval = 50
            SpectrumTimer.Start()

            YoutubeBrowser.SendToBack()




        End Sub
        Dim vlcFailed As Boolean = False
        Public Sub VLCclearPlaylists()
            Try
                VlcPlayer.playlist.items.clear()
            Catch ex As Exception
                If version >= 222 AndAlso version <= 226 Then
                    VlcPlayer.playlist.items.clear()
                Else
                    'Try
                    Try


                        SuspendDrawing()
                        '  VlcPlayer.Visible = False
                        VlcPlayer.Dispose()
                        vlcFailed = True
                        VlcPlayer = New AxAXVLC.AxVLCPlugin2
                        Setup_VLC()
                        ResumeDrawing()
                        VlcPlayer.Refresh()
                        VlcPlayer.Invalidate()
                        Refresh_VLC_volspeed()
                    Catch 'ex As Exception

                    End Try
                    'Catch ex As Exception : MyMsgBox.Show("VLC plugin failed, it may be incompatible. (versions 2.2.2 to 2.2.6 work the best)" & Environment.NewLine & Environment.NewLine & ex.ToString) : End Try
                End If
            End Try

        End Sub
        Public Function IsVLCRightVer() As Boolean
            If version >= 222 AndAlso version <= 226 Then Return True
            Return False
        End Function
 

#End Region

#Region " Setup |   Skin & Visuals"
        Private Sub PreVentFlicker()
            With Me
                .SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
                .SetStyle(ControlStyles.UserPaint, True)
                .SetStyle(ControlStyles.AllPaintingInWmPaint, True)
                .UpdateStyles()
            End With
        End Sub
#Region " Taskbar Thumbnail Declartions"
        Dim FirstSetupTaskbar As Boolean = False
        Dim TaskbarStopBut As New ThumbnailToolBarButton(Icon.FromHandle(My.Resources.Stop_3.GetHicon), "Stop")
        Dim TaskbarPreviousBut As New ThumbnailToolBarButton(Icon.FromHandle(My.Resources.Prev_icon.GetHicon), "Previous")
        Dim TaskbarPlayPauseBut As New ThumbnailToolBarButton(Icon.FromHandle(My.Resources.PlayPause_Icon.GetHicon), "Play / Pause")
        Dim TaskbarNextBut As New ThumbnailToolBarButton(Icon.FromHandle(My.Resources.Next_Icon.GetHicon), "Next")
#End Region

        'Visual / App Layout
        Public Sub Setup_Visuals()
            Try
                Load_Location()
                Setup_Hotkeys()
                TrackBar_Seek2.Maximum = SeekBarMaxVal

                Setup_Controls_OnArtwork()
                LabelConfirmSpeed.ForeColor = Color.FromArgb(5, 192, 192, 192)

                HamburgerMenu.MenuAppearance.AppearanceMenu.SetFont(New Font("Segoe UI", 12))
                Setup_Menus()

                OpenProgressBar.Properties.LookAndFeel.UseDefaultLookAndFeel = False
                OpenProgressBar.Properties.LookAndFeel.SkinMaskColor = Color.FromArgb(128, 255, 128)
                OpenProgressBar.Properties.LookAndFeel.SkinName = "Visual Studio 2013 Dark"

                ViewOtherControlsCheckbox.Checked = My.Settings.WasChecked
                Checkbox_TouchFriendly()
                My.Settings.SkinChanged = False

                Set_Parents()

                But_SpeedDown.BackgroundImage = SlowImage
                But_SpeedUp.BackgroundImage = FastImage
                But_PitchDown.BackgroundImage = SlowImage
                But_PitchUp.BackgroundImage = FastImage

                If My.Settings.MiniModeOn = False Then
                    Label_Update.Location = New Point(PicBox_Window_AppName.Location.X + PicBox_Window_AppName.Width + 12, 18)
                End If
            Catch ex As Exception
                MyMsgBox.Show("Error in sub: Setup_Visuals" & Environment.NewLine & Environment.NewLine & ex.ToString, "", True)
            End Try

            '...................................HD Presets..............................
            Try
                LastSize = New Size(814, 410)
                Me.Size = New Size(My.Settings.FormSize)
                Select Case Me.Size
                    Case New Size(814, 410)
                        BarEdit_WindowSizePresets.EditValue = 1
                    Case New Size(903, 463)
                        BarEdit_WindowSizePresets.EditValue = 2
                    Case New Size(964, 494)
                        BarEdit_WindowSizePresets.EditValue = 3
                    Case New Size(1074, 556)
                        BarEdit_WindowSizePresets.EditValue = 4
                    Case New Size(1168, 608)
                        BarEdit_WindowSizePresets.EditValue = 5
                    Case New Size(1263, 662)
                        BarEdit_WindowSizePresets.EditValue = 6
                End Select
            Catch
                MyMsgBox.Show("Error: 0x3: Error loading last saved window size.", "", True)
            End Try

            Try
                BarCheckBox_ViewLyrics.Checked = My.Settings.CheckStateLyrics
                BarCheckBox_CustomizeLyrics.Checked = My.Settings.CheckStateLyricsCustomize
                Panellyrics.HorizontalScroll.Visible = False
                Panellyrics.HorizontalScroll.Enabled = False
                ShowScrollBar(Panellyrics.Handle, SB_HORZ, False)
            Catch ex As Exception
                MyMsgBox.Show("Error: 0x4: Error Loading some saved settings with artwork. To fix, re-apply your desired settings." & Environment.NewLine & "(" & ex.ToString & ")", "", True)
            End Try
            LyricsTimer.Start()

            SpectrumColor1 = My.Settings.SpectrumColor1
            SpectrumColor2 = My.Settings.SpectrumColor2

            Playlist_Rowheight = My.Settings.PlaylistRowHeight
            BarEdit_PlaylistRowHeightChanger.EditValue = Playlist_Rowheight
            Splitter.BringToFront()
            TitleBarCloseImage = My.Resources.TitleBar_Close
            TitleBarCloseHoverImage = My.Resources.TitleBar_Close_Hover
            TitleBarClosePressImage = My.Resources.TitleBar_Close_Press
            TitleBarMaxImage = My.Resources.TitleBar_Max
            TitleBarMaxHoverImage = My.Resources.TitleBar_Max_Hover
            TitleBarMaxPressImage = My.Resources.TitleBar_Max_Press
            TitleBarMinImage = My.Resources.TitleBar_Min
            TitleBarMinHoverImage = My.Resources.TitleBar_Min_Hover
            TitleBarMinPressImage = My.Resources.TitleBar_Min_Press
            But_TitleBar_Close.BackgroundImage = TitleBarCloseImage
            But_TitleBarMax.BackgroundImage = TitleBarMaxImage
            But_TitleBarMin.BackgroundImage = TitleBarMinImage
            FormOutlineImage = Nothing
            PicBox_Window_AppName.Visible = True
            PicBox_Window_AppName.Parent = Window_Titlebar
            PicBox_Window_AppName.Size = New Size(92, 20)
            PicBox_Window_AppName.Location = New Point(64, 20)
            But_TitleBar_Close.Visible = True
            But_TitleBarMax.Visible = True
            But_TitleBarMin.Visible = True
            Window_Titlebar.Visible = True
            Window_Titlebar.BackColor = Color.Transparent
            Window_Titlebar.BackgroundImage = Nothing
            Window_Titlebar.BackgroundImageLayout = ImageLayout.Stretch
            Dim TitleBarButtons As New Panel
            Window_Titlebar.Controls.Add(TitleBarButtons)
            TitleBarButtons.Location = New Point(Me.Width - 166, 12)
            TitleBarButtons.Visible = True
            TitleBarButtons.Size = New Size(147, 33)
            TitleBarButtons.Controls.AddRange({But_TitleBar_Close, But_TitleBarMax, But_TitleBarMin})
            TitleBarButtons.BringToFront()
            TitleBarButtons.Anchor = AnchorStyles.Top Or AnchorStyles.Right
            But_TitleBar_Close.Size = New Size(45, 33)
            But_TitleBar_Close.Location = New Point(102, 0)
            But_TitleBarMax.Size = New Size(45, 33)
            But_TitleBarMax.Location = New Point(51, 0)
            But_TitleBarMin.Size = New Size(45, 33)
            But_TitleBarMin.Location = New Point(0, 0)
            Me.But_SettingsPic.Parent = Window_Titlebar
            But_SettingsPic.Location = New Point(13, 14)
            But_SettingsPic.Visible = True
            But_SettingsPic.BringToFront()
        End Sub






        'Menus
        Public Sub Setup_Menus()
            Try
                Dim BoldFont As New Font("Segoe UI", 12, FontStyle.Bold)
                Dim RegularFont As New Font("Segoe UI", 12, FontStyle.Regular)
                TEXT_Main.ItemAppearance.SetFont(BoldFont)
                TEXT_OpenSave.ItemAppearance.SetFont(BoldFont)
                TEXT_Playlist.ItemAppearance.SetFont(BoldFont)
                TEXT_SavePlaylist.ItemAppearance.SetFont(BoldFont)
                TEXT_PlaylistPositions.ItemAppearance.SetFont(BoldFont)
                TEXT_Hotkeys.ItemAppearance.SetFont(BoldFont)
                TEXT_Lyrics.ItemAppearance.SetFont(BoldFont)
                TEXT_Artwork.ItemAppearance.SetFont(BoldFont)
                TEXT_Skin.ItemAppearance.SetFont(BoldFont)
                TEXT_WindowSize.ItemAppearance.SetFont(BoldFont)
                TEXT_WindowLocation.ItemAppearance.SetFont(BoldFont)
                TEXT_BetaFeatures.ItemAppearance.SetFont(BoldFont)
                PlaylistBeta_TEXT.ItemAppearance.SetFont(BoldFont)
                TEXT_ChangeOpacitiy.ItemAppearance.SetFont(BoldFont)
                TEXT_EditArtwork.ItemAppearance.SetFont(BoldFont)
                TEXT_Favorites.ItemAppearance.SetFont(BoldFont)
                TEXT_PlaylistFont.ItemAppearance.SetFont(BoldFont)
                TEXT_Playlist_Tabs.ItemAppearance.SetFont(BoldFont)
                TEXT_CustomizeTrackbars.ItemAppearance.SetFont(BoldFont)
                TEXT_Icons.ItemAppearance.SetFont(BoldFont)
                TEXT_Settings.ItemAppearance.SetFont(BoldFont)
                TEXT_Settings.ItemInMenuAppearance.SetFont(BoldFont)
                TEXT_Other.ItemAppearance.SetFont(BoldFont)
                TEXT_YouTube.ItemAppearance.SetFont(BoldFont)
                TEXT_Radio.ItemAppearance.SetFont(BoldFont)
                '...Menus..
                'File
                NEW_MENU_Songs.ItemAppearance.SetFont(RegularFont)
                NEW_MENU_Songs.MenuAppearance.AppearanceMenu.SetFont(RegularFont)

                MENU_Media.ItemAppearance.SetFont(RegularFont)
                MENU_Media.MenuAppearance.AppearanceMenu.SetFont(RegularFont)

                FileMenu.ItemAppearance.SetFont(RegularFont)
                FileMenu.MenuAppearance.AppearanceMenu.SetFont(RegularFont)

                'Playlist
                NEW_MENU_Playlists.ItemAppearance.SetFont(RegularFont)
                NEW_MENU_Playlists.MenuAppearance.AppearanceMenu.SetFont(RegularFont)

                MENU_QuickOpen.ItemAppearance.SetFont(RegularFont)
                MENU_QuickOpen.MenuAppearance.AppearanceMenu.SetFont(RegularFont)

                'Edit
                NEW_MENU_Edit.ItemAppearance.SetFont(RegularFont)
                NEW_MENU_Edit.MenuAppearance.AppearanceMenu.SetFont(RegularFont)
                NEW_MENU_Extra.ItemAppearance.SetFont(RegularFont)
                NEW_MENU_Extra.MenuAppearance.AppearanceMenu.SetFont(RegularFont)

                'View, etc.
                NEW_MENU_View.ItemAppearance.SetFont(RegularFont)
                NEW_MENU_View.MenuAppearance.AppearanceMenu.SetFont(RegularFont)

                Menu_ArtworkTransparency.ItemAppearance.SetFont(RegularFont)
                Menu_ArtworkTransparency.MenuAppearance.AppearanceMenu.SetFont(RegularFont)

                Menu_EnhancedSkins.ItemAppearance.SetFont(RegularFont)
                Menu_EnhancedSkins.MenuAppearance.AppearanceMenu.SetFont(RegularFont)
                Menu_EnhancedSkins.ItemAppearance.SetFont(RegularFont)

                Menu_Tiled.MenuAppearance.AppearanceMenu.SetFont(RegularFont)
                Menu_Tiled.ItemAppearance.SetFont(BoldFont)

                Menu_HD.MenuAppearance.AppearanceMenu.SetFont(RegularFont)
                Menu_HD.ItemAppearance.SetFont(BoldFont)

                Menu_Scaling.MenuAppearance.AppearanceMenu.SetFont(RegularFont)
                Menu_Scaling.ItemAppearance.SetFont(RegularFont)

                MENU_ControlsBG.MenuAppearance.AppearanceMenu.SetFont(RegularFont)
                MENU_ControlsBG.ItemAppearance.SetFont(RegularFont)

                Menu_SpectrumColor.MenuAppearance.AppearanceMenu.SetFont(RegularFont)
                Menu_SpectrumColor.ItemAppearance.SetFont(RegularFont)

                Menu_WindowsColors.MenuAppearance.AppearanceMenu.SetFont(RegularFont)
                Menu_WindowsColors.ItemAppearance.SetFont(RegularFont)

                Menu_TabOrientation.MenuAppearance.AppearanceMenu.SetFont(RegularFont)
                Menu_TabOrientation.ItemAppearance.SetFont(RegularFont)


                'Settings
                NEW_MENU_SettingsMenu.ItemAppearance.SetFont(RegularFont)
                NEW_MENU_SettingsMenu.MenuAppearance.AppearanceMenu.SetFont(RegularFont)

                'Extras, etc.
                Menu_RichPlayer.ItemAppearance.SetFont(RegularFont)
                Menu_RichPlayer.MenuAppearance.AppearanceMenu.SetFont(RegularFont)

                Menu_Spotify.ItemAppearance.SetFont(RegularFont)
                Menu_Spotify.MenuAppearance.AppearanceMenu.SetFont(RegularFont)
                MENU_ClientControl.ItemAppearance.SetFont(RegularFont)
                MENU_ClientControl.MenuAppearance.AppearanceMenu.SetFont(RegularFont)
                MENU_FullControl.ItemAppearance.SetFont(RegularFont)
                MENU_FullControl.MenuAppearance.AppearanceMenu.SetFont(RegularFont)

                Menu_YouTube.ItemAppearance.SetFont(RegularFont)
                Menu_YouTube.MenuAppearance.AppearanceMenu.SetFont(RegularFont)

                Menu_YouTubeBroswer.ItemAppearance.SetFont(RegularFont)
                Menu_YouTubeBroswer.MenuAppearance.AppearanceMenu.SetFont(RegularFont)

                'About
                NEW_MENU_About.ItemAppearance.SetFont(RegularFont)
                NEW_MENU_About.MenuAppearance.AppearanceMenu.SetFont(RegularFont)

                MENU_Close.ItemAppearance.SetFont(RegularFont)
                MENU_Close.MenuAppearance.AppearanceMenu.SetFont(RegularFont)


                Dim use As Boolean = False
                If use Then
                    'Subtitles
                    SubtitleMenu.Caption = "Subtitles"
                    MENU_Media.AddItem(SubtitleMenu)
                    SubtitleMenu.ItemAppearance.SetFont(RegularFont)
                    SubtitleMenu.MenuAppearance.AppearanceMenu.SetFont(RegularFont)

                    BarBut_EnableSubtitles.Visibility = BarItemVisibility.Never

                    Popup_PlaylistTab.MenuAppearance.AppearanceMenu.SetFont(RegularFont)

                    SubtitleMenu.AddItem(Subtitle1But)
                    AddHandler Subtitle1But.CheckedChanged, AddressOf Subtitle1But_ItemClick

                    SubtitleMenu.AddItem(Subtitle2But)
                    AddHandler Subtitle2But.CheckedChanged, AddressOf Subtitle2But_ItemClick

                    SubtitleMenu.AddItem(Subtitle3But)
                    AddHandler Subtitle3But.CheckedChanged, AddressOf Subtitle3But_ItemClick

                    SubtitleMenu.AddItem(Subtitle4But)
                    AddHandler Subtitle4But.CheckedChanged, AddressOf Subtitle4But_ItemClick


                    'Audio
                    AudioMenu.Caption = "Audio"
                    MENU_Media.AddItem(AudioMenu)
                    AudioMenu.ItemAppearance.SetFont(RegularFont)
                    AudioMenu.MenuAppearance.AppearanceMenu.SetFont(RegularFont)

                    Popup_PlaylistTab.MenuAppearance.AppearanceMenu.SetFont(RegularFont)

                    AudioMenu.AddItem(Audio1But)
                    AddHandler Audio1But.CheckedChanged, AddressOf Audio1But_ItemClick

                    AudioMenu.AddItem(Audio2But)
                    AddHandler Audio2But.CheckedChanged, AddressOf Audio2But_ItemClick

                    AudioMenu.AddItem(Audio3But)
                    AddHandler Audio3But.CheckedChanged, AddressOf Audio3But_ItemClick

                    AudioMenu.AddItem(Audio4But)
                    AddHandler Audio4But.CheckedChanged, AddressOf Audio4But_ItemClick
                End If





                NewMenus()


                'Quick Open
                QuickOpenMenu.MenuAppearance.AppearanceMenu.SetFont(RegularFont)



            Catch ex As Exception
                MyMsgBox.Show("Error in sub: Setup_Menus" & Environment.NewLine & Environment.NewLine & ex.ToString, "", True)
            End Try
        End Sub
        Public Sub NewMenus()
            Dim BoldFont As New Font("Segoe UI", 12, FontStyle.Bold)
            Dim RegularFont As New Font("Segoe UI", 12, FontStyle.Regular)

            MENUNEW_Subtitle.MenuAppearance.AppearanceMenu.SetFont(RegularFont)

            MENUNEW_Subtitle.AddItem(Subtitle1But)
            AddHandler Subtitle1But.CheckedChanged, AddressOf Subtitle1But_ItemClick

            MENUNEW_Subtitle.AddItem(Subtitle2But)
            AddHandler Subtitle2But.CheckedChanged, AddressOf Subtitle2But_ItemClick

            MENUNEW_Subtitle.AddItem(Subtitle3But)
            AddHandler Subtitle3But.CheckedChanged, AddressOf Subtitle3But_ItemClick

            MENUNEW_Subtitle.AddItem(Subtitle4But)
            AddHandler Subtitle4But.CheckedChanged, AddressOf Subtitle4But_ItemClick

            MENUNEW_Audio.MenuAppearance.AppearanceMenu.SetFont(RegularFont)

            MENUNEW_Audio.AddItem(Audio1But)
            AddHandler Audio1But.CheckedChanged, AddressOf Audio1But_ItemClick

            MENUNEW_Audio.AddItem(Audio2But)
            AddHandler Audio2But.CheckedChanged, AddressOf Audio2But_ItemClick

            MENUNEW_Audio.AddItem(Audio3But)
            AddHandler Audio3But.CheckedChanged, AddressOf Audio3But_ItemClick

            MENUNEW_Audio.AddItem(Audio4But)
            AddHandler Audio4But.CheckedChanged, AddressOf Audio4But_ItemClick
        End Sub


        'Prepare Skin Layout    |   PARENTS
        Public Sub Set_Parents() ' LOCATIONS and PARENTS
            If xcarform.DontSetParents Then Exit Sub
            Try
                Window_Titlebar.BackgroundImage = TitleBarImage
                Window_Titlebar.BringToFront()
                PicBox_Window_AppName.BringToFront()
                ControlsBGPanel.BringToFront()
                Try
                    PicBox_Window_AppName.Location = Window_Titlebar.PointToClient(PicBox_Window_AppName.Parent.PointToScreen(PicBox_Window_AppName.Location))
                    PicBox_Window_AppName.Parent = Window_Titlebar
                    PicBox_Window_AppName.BringToFront()
                Catch

                End Try


                Label_Update.Location = Window_Titlebar.PointToClient(Label_Update.Parent.PointToScreen(Label_Update.Location))
                Label_Update.Parent = Window_Titlebar
                Label_Update.BringToFront()

                But_TitleBar_Close.Location = Window_Titlebar.PointToClient(But_TitleBar_Close.Parent.PointToScreen(But_TitleBar_Close.Location))
                But_TitleBar_Close.Parent = Window_Titlebar
                But_TitleBar_Close.BringToFront()

                But_TitleBarMax.Location = Window_Titlebar.PointToClient(But_TitleBarMax.Parent.PointToScreen(But_TitleBarMax.Location))
                But_TitleBarMax.Parent = Window_Titlebar
                But_TitleBarMax.BringToFront()

                But_TitleBarMin.Location = Window_Titlebar.PointToClient(But_TitleBarMin.Parent.PointToScreen(But_TitleBarMin.Location))
                But_TitleBarMin.Parent = Window_Titlebar
                But_TitleBarMin.BringToFront()

                'YouTube
                Me.YouTubePanel.Location = Me.Splitter.Panel1.PointToClient(Me.YouTubePanel.Parent.PointToScreen(Me.YouTubePanel.Location))
                Me.YouTubePanel.Parent = Splitter.Panel1
                Me.YoutubeBrowser.Location = Me.Splitter.Panel1.PointToClient(Me.YoutubeBrowser.Parent.PointToScreen(Me.YoutubeBrowser.Location))
                Me.YoutubeBrowser.Parent = Splitter.Panel1
                YoutubeMoveDown.BringToFront()
                YoutubeMoveUp.BringToFront()
                YoutubeMoveTop.BringToFront()
                YoutubeMoveBottom.BringToFront()

                Me.SpotifyBrowser.Location = Me.Splitter.Panel1.PointToClient(Me.SpotifyBrowser.Parent.PointToScreen(Me.SpotifyBrowser.Location))
                Me.SpotifyBrowser.Parent = Splitter.Panel1

                'VLC
                If VLC_installed Then
                    Me.VlcPlayer.Location = Me.Splitter.Panel1.PointToClient(Me.VlcPlayer.Parent.PointToScreen(Me.VlcPlayer.Location))
                    Me.VlcPlayer.Parent = Splitter.Panel1
                End If


                'Artwork
                Me.Artwork.Location = Me.Splitter.Panel1.PointToClient(Me.Artwork.Parent.PointToScreen(Me.Artwork.Location))
                Me.Artwork.Parent = Splitter.Panel1
                Artwork.BringToFront()
                If VLC_installed Then
                    VlcPlayer.Top = Artwork.Top
                    VlcPlayer.Size = Artwork.Size
                    VlcPlayer.Location = Artwork.Location
                End If


                'Speed Confirm Label
                Me.LabelConfirmSpeed.Location = Me.Artwork.PointToClient(Me.LabelConfirmSpeed.Parent.PointToScreen(Me.LabelConfirmSpeed.Location))
                Me.LabelConfirmSpeed.Parent = Artwork
                LabelConfirmSpeed.BringToFront()

                'Lyrics
                Me.Panellyrics.Location = Me.Artwork.PointToClient(Me.Panellyrics.Parent.PointToScreen(Me.Panellyrics.Location))
                Me.Panellyrics.Parent = Artwork
                Panellyrics.BringToFront()

                'Metadata Labels
                Me.Label_SongName.Location = Me.Splitter.Panel1.PointToClient(Me.Label_SongName.Parent.PointToScreen(Me.Label_SongName.Location))
                Me.Label_SongName.Parent = Splitter.Panel1
                Label_SongName.BringToFront()
                Me.Label_Artist.Location = Me.Splitter.Panel1.PointToClient(Me.Label_Artist.Parent.PointToScreen(Me.Label_Artist.Location))
                Me.Label_Artist.Parent = Splitter.Panel1
                Label_Artist.BringToFront()
                Me.Label_Album.Location = Me.Splitter.Panel1.PointToClient(Me.Label_Album.Parent.PointToScreen(Me.Label_Album.Location))
                Me.Label_Album.Parent = Splitter.Panel1
                Label_Album.BringToFront()
                Me.SongNameUnderline.Location = Me.Splitter.Panel1.PointToClient(Me.SongNameUnderline.Parent.PointToScreen(Me.SongNameUnderline.Location))
                Me.SongNameUnderline.Parent = Splitter.Panel1
                SongNameUnderline.BringToFront()

                'Spectrum
                xpos = (Splitter.Panel1.Width / 2) - (PictureBoxSpec.Width / 2)
                PictureBoxSpec.Location = New Point(xpos, PictureBoxSpec.Location.Y)
                PictureBoxSpec.Location = Me.Splitter.Panel1.PointToClient(PictureBoxSpec.Parent.PointToScreen(PictureBoxSpec.Location))
                PictureBoxSpec.Parent = Splitter.Panel1
                PictureBoxSpec.BringToFront()

                'Time Labels
                Me.timelabel.Location = Me.ControlsBGPanel.PointToClient(Me.timelabel.Parent.PointToScreen(Me.timelabel.Location))
                Me.timelabel.Parent = ControlsBGPanel
                timelabel.BringToFront()
                Me.totaltimelabel.Location = Me.ControlsBGPanel.PointToClient(Me.totaltimelabel.Parent.PointToScreen(Me.totaltimelabel.Location))
                Me.totaltimelabel.Parent = ControlsBGPanel
                totaltimelabel.BringToFront()

                'Playlists
                Me.PlaylistTabs.Location = Splitter.Panel2.PointToClient(Me.PlaylistTabs.Parent.PointToScreen(Me.PlaylistTabs.Location))
                Me.PlaylistTabs.Parent = Splitter.Panel2
                PlaylistTabs.BringToFront()

                'Seek Bar
                Me.TrackBar_Seek2.Location = Me.ControlsBGPanel.PointToClient(Me.TrackBar_Seek2.Parent.PointToScreen(Me.TrackBar_Seek2.Location))
                Me.TrackBar_Seek2.Parent = ControlsBGPanel

                'Pause/Play
                Me.PlaybackPanel.Location = Me.ControlsBGPanel.PointToClient(Me.PlaybackPanel.Parent.PointToScreen(Me.PlaybackPanel.Location))
                Me.PlaybackPanel.Parent = ControlsBGPanel
                PlaybackPanel.BringToFront()


                'AB Repeat, Repeat, Shuffle
                Panel_ABRepeatControlsBG.Location = ControlsBGPanel.PointToClient(Panel_ABRepeatControlsBG.Parent.PointToScreen(Panel_ABRepeatControlsBG.Location))
                Panel_ABRepeatControlsBG.Parent = ControlsBGPanel
                Panel_ABRepeatControlsBG.BringToFront()



                'Pitch Trackbar
                Me.trackbar_Pitch2.Location = Me.ControlsBGPanel.PointToClient(Me.trackbar_Pitch2.Parent.PointToScreen(Me.trackbar_Pitch2.Location))
                Me.trackbar_Pitch2.Parent = ControlsBGPanel
                trackbar_Pitch2.BringToFront()
                'Pitch Textbox
                Me.PicBox_PitchTextBox.Location = Me.ControlsBGPanel.PointToClient(Me.PicBox_PitchTextBox.Parent.PointToScreen(Me.PicBox_PitchTextBox.Location))
                Me.PicBox_PitchTextBox.Parent = ControlsBGPanel
                Me.PicBox_PitchTextBox.BringToFront()
                PicBox_PitchText.Location = ControlsBGPanel.PointToClient(PicBox_PitchText.Parent.PointToScreen(PicBox_PitchText.Location))
                PicBox_PitchText.Parent = ControlsBGPanel
                PicBox_PitchText.BringToFront()

                'Speed Trackbar
                Me.trackBar_Speed2.Location = Me.ControlsBGPanel.PointToClient(Me.trackBar_Speed2.Parent.PointToScreen(Me.trackBar_Speed2.Location))
                Me.trackBar_Speed2.Parent = ControlsBGPanel
                trackBar_Speed2.BringToFront()

                'Speed Control
                Me.But_SpeedDown.Location = Me.ControlsBGPanel.PointToClient(Me.But_SpeedDown.Parent.PointToScreen(Me.But_SpeedDown.Location))
                Me.But_SpeedDown.Parent = ControlsBGPanel
                But_SpeedDown.BringToFront()
                Me.But_SpeedUp.Location = Me.ControlsBGPanel.PointToClient(Me.But_SpeedUp.Parent.PointToScreen(Me.But_SpeedUp.Location))
                Me.But_SpeedUp.Parent = ControlsBGPanel
                But_SpeedUp.BringToFront()
                Me.SpeedResetBut.Location = Me.ControlsBGPanel.PointToClient(Me.SpeedResetBut.Parent.PointToScreen(Me.SpeedResetBut.Location))
                Me.SpeedResetBut.Parent = ControlsBGPanel
                SpeedResetBut.BringToFront()
                'Speed Textbox
                Me.Label_SpeedTextbox.Location = Me.ControlsBGPanel.PointToClient(Me.Label_SpeedTextbox.Parent.PointToScreen(Me.Label_SpeedTextbox.Location))
                Me.Label_SpeedTextbox.Parent = ControlsBGPanel
                Me.Label_SpeedTextbox.BringToFront()
                PicBox_SpeedText.Location = ControlsBGPanel.PointToClient(PicBox_SpeedText.Parent.PointToScreen(PicBox_SpeedText.Location))
                PicBox_SpeedText.Parent = ControlsBGPanel
                PicBox_SpeedText.BringToFront()

                'Volume
                Me.TrackBar_PlayerVol2.Location = Me.ControlsBGPanel.PointToClient(Me.TrackBar_PlayerVol2.Parent.PointToScreen(Me.TrackBar_PlayerVol2.Location))
                Me.TrackBar_PlayerVol2.Parent = ControlsBGPanel
                TrackBar_PlayerVol2.BringToFront()

                ControlsBGPanel.SendToBack()
                But_ResizeGrabber.BringToFront()

                PictureBoxWF.BringToFront()
                ProgressBarControl1.BringToFront()
            Catch ex As Exception
                MyMsgBox.Show("Error in: Set_Parents" & Environment.NewLine & Environment.NewLine & ex.ToString, "", True)
            End Try


        End Sub

        'Hue Button_Click
        Public Sub ChangeHueBut_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_ChangeHue.ItemClick
            Dim xform As New Hueform

            If xform.ShowDialog = DialogResult.OK Then

                My.Settings.Hue = xform.HueTrackBar.Value
                My.Settings.Saturation = xform.SatTrackBar.Value
                My.Settings.Lightness = xform.LightTrackBar.Value

                ChangeHue()
            End If
        End Sub
        'Hue
        Public Sub ChangeHue()
            Try
                'BackgroundImage Change Hue

                'Load images
                Dim imgOriginal As System.Drawing.Image = TitleBarImage
                Dim imgOriginal2 As System.Drawing.Image = AppNameActiveImage
                Dim imgOriginal3 As System.Drawing.Image = AppNameImage

                Dim imgOriginal6 As System.Drawing.Image = ControlsBgImage
                Dim imgOriginal7 As System.Drawing.Image = My.Resources.AB_Repeat_BG

                Dim imgOriginal10 As System.Drawing.Image = FormOutlineImage


                'Define filter and parameters.
                Dim imgFilter As HSLFilter = New HSLFilter()
                imgFilter.Hue = My.Settings.Hue
                imgFilter.Saturation = My.Settings.Saturation
                imgFilter.Lightness = My.Settings.Lightness


                'Execute filters
                If Not TitleBarImage Is Nothing Then
                    Dim imgFiltered As System.Drawing.Image = imgFilter.ExecuteFilter(imgOriginal)
                    Window_Titlebar.BackgroundImage = imgFiltered
                End If
                Dim imgFiltered2 As System.Drawing.Image = imgFilter.ExecuteFilter(imgOriginal2)
                Dim imgFiltered3 As System.Drawing.Image = imgFilter.ExecuteFilter(imgOriginal3)

                Dim imgFiltered6 As System.Drawing.Image = imgFilter.ExecuteFilter(imgOriginal6)
                Dim imgFiltered7 As System.Drawing.Image = imgFilter.ExecuteFilter(imgOriginal7)

                If Not FormOutlineImage Is Nothing Then
                    Dim imgFiltered10 As System.Drawing.Image = imgFilter.ExecuteFilter(imgOriginal10)
                    Form_Outline.BackgroundImage = imgFiltered10
                End If


                '  Window_BorderLineLeft.BackgroundImage = imgFiltered4
                ' Window_BorderLineRight.BackgroundImage = imgFiltered5
                ControlsBGPanel.BackgroundImage = ChangeOpacity(imgFiltered6, My.Settings.ControlsBGOpacity / 100)
                Panel_ABRepeatControlsBG.BackgroundImage = ChangeOpacity(imgFiltered7, My.Settings.ControlsBGOpacity / 100)
                ' Window_LLCorner.BackgroundImage = imgFiltered8
                ' Window_LRCorner.BackgroundImage = imgFiltered9

                If FormActivated Then
                    If My.Settings.MiniModeOn Then
                        PicBox_Window_AppName.BackgroundImage = imgFiltered2
                    Else
                        PicBox_Window_AppName.BackgroundImage = AppNameActiveImage
                    End If

                Else
                    PicBox_Window_AppName.BackgroundImage = imgFiltered3
                End If


            Catch ex As Exception
                MyMsgBox.Show("Error in sub: ChangeHue" & Environment.NewLine & Environment.NewLine & ex.ToString, "", True)
            End Try
        End Sub


        'Controls Background Opacity
        Public Sub But_ControlsBGOpacity_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_ControlsBGOpacity.ItemClick
            ControlsBG()
        End Sub
        Public Sub ControlsBG()
            Dim TempName As String = My.Settings.ControlsBGOpacity
            Dim message, title, defaultValue As String
            Dim myValue As Object
            message = "Value for opactity must be from 10-100"
            title = "Enter a Vaild Value"
            defaultValue = TempName


            Dim xform As New MyInputBox
            xform.Text = title
            xform.Label1.Text = message
            xform.TextEdit1.Text = defaultValue
            Try
                If xform.ShowDialog = DialogResult.OK Then
                    myValue = xform.TextEdit1.Text

                End If
            Catch

            End Try

            If myValue Is "" Then myValue = defaultValue
            Try
                If myValue >= 10 And myValue <= 100 Then
                    My.Settings.ControlsBGOpacity = CInt(myValue)
                    ChangeHue()
                End If
            Catch ex As Exception
            End Try
        End Sub

        'Enahanced Skins
        Public Sub Setup_Enhanced_Skins()
            'Enhanced Skin

            Try

                Me.BackgroundImageLayout = My.Settings.BackgroundImageLayout
                Select Case My.Settings.EnhancedSkin
                    Case "None"
                        EnhancedSkin1_Checkbox.Checked = False
                        EnhancedSkin2_Checkbox.Checked = False
                        EnhancedSkin3_Checkbox.Checked = False
                        EnhancedSkin4_Checkbox.Checked = False
                        EnhancedSkin5_Checkbox.Checked = False
                        EnhancedSkin6_Checkbox.Checked = False
                        EnhancedSkin7_Checkbox.Checked = False
                        EnhancedSkin8_Checkbox.Checked = False
                        EnhancedSkin9_Checkbox.Checked = False
                        EnhancedSkin10_Checkbox.Checked = False
                        EnhancedSkin11_Checkbox.Checked = False
                        EnhancedSkin12_Checkbox.Checked = False
                        EnhancedSkin13_Checkbox.Checked = False
                        EnhancedSkin14_Checkbox.Checked = False
                        EnhancedSkin15_Checkbox.Checked = False
                    Case 1
                        EnhancedSkin1_Checkbox.Checked = True
                    Case 2
                        EnhancedSkin2_Checkbox.Checked = True
                    Case 3
                        EnhancedSkin3_Checkbox.Checked = True
                    Case 4
                        EnhancedSkin4_Checkbox.Checked = True
                    Case 5
                        EnhancedSkin5_Checkbox.Checked = True
                    Case 6
                        EnhancedSkin6_Checkbox.Checked = True
                    Case 7
                        EnhancedSkin7_Checkbox.Checked = True
                    Case 8
                        EnhancedSkin8_Checkbox.Checked = True
                    Case 9
                        EnhancedSkin9_Checkbox.Checked = True
                    Case 10
                        EnhancedSkin10_Checkbox.Checked = True
                    Case 11
                        EnhancedSkin11_Checkbox.Checked = True
                    Case 12
                        EnhancedSkin12_Checkbox.Checked = True
                    Case 13
                        EnhancedSkin13_Checkbox.Checked = True
                    Case 14
                        EnhancedSkin14_Checkbox.Checked = True
                    Case 15
                        EnhancedSkin15_Checkbox.Checked = True
                    Case "Custom"
                        My.Settings.CustomImageCheckState = True


                End Select


            Catch ex As Exception
                MyMsgBox.Show("Error: 0x11: Error Loading background image. To fix, please re-apply a background image (View >> Enhanced Skins ...)" And _
                       Environment.NewLine And "(" And ex.ToString And ")", "", True)
            End Try

        End Sub

        'Lyrics Panel
        Public Sub Setup_Controls_OnArtwork()
            Try
                Artwork.AllowDrop = True
                Splitter.Panel1.Controls.Add(Panellyrics)

                Panellyrics.Location = New Point(28, Label_SongName.Location.Y + Label_SongName.Height) '50)
                Panellyrics.Size = Artwork.Size 'New Size(143, 143)
                Panellyrics.Anchor = (AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right)
                Panellyrics.AutoScroll = True
                Panellyrics.Font = New Font("Segoe UI", 9.75)
                Panellyrics.Visible = False


                'Add Handles
                AddHandler Panellyrics.MouseDown, AddressOf PictureBox1_MouseDown
                AddHandler Panellyrics.MouseMove, AddressOf PictureBox1_MouseMove
                AddHandler Panellyrics.MouseUp, AddressOf PictureBox1_MouseUp
                AddHandler Panellyrics.Scroll, AddressOf PanelLyrics_Scroll

                Panellyrics.Controls.Add(LabelLyrics)

                LabelLyrics.AutoSize = True
                LabelLyrics.BackColor = Color.Transparent
                LabelLyrics.Font = New Font("Segoe UI", 9.75, FontStyle.Bold)
                LabelLyrics.Location = New Point(0, 0)
                LabelLyrics.Size = New Size(143, 30)
                LabelLyrics.MinimumSize = New Size(143, 0)
                LabelLyrics.MaximumSize = New Size(143, 0)
                LabelLyrics.Padding = New Padding(0, 5, 15, 5)
                LabelLyrics.Text = "Lyrics"
                LabelLyrics.TextAlign = ContentAlignment.TopCenter
                LabelLyrics.Visible = True


                'Add Handles
                AddHandler LabelLyrics.MouseDown, AddressOf LabelLyrics_MouseDown_1
                AddHandler LabelLyrics.MouseDown, AddressOf PictureBox1_MouseDown
                AddHandler LabelLyrics.MouseMove, AddressOf LabelLyrics_MouseMove
                AddHandler LabelLyrics.MouseMove, AddressOf PictureBox1_MouseMove
                AddHandler LabelLyrics.MouseUp, AddressOf PictureBox1_MouseUp
                AddHandler LabelLyrics.MouseDoubleClick, AddressOf labels_MouseDoubleClick


                Panellyrics.Controls.Add(TextboxLyrics)

                TextboxLyrics.BackColor = Color.FromArgb(80, 80, 80)
                TextboxLyrics.BorderStyle = BorderStyle.None
                TextboxLyrics.Dock = DockStyle.Top
                TextboxLyrics.Font = New Font("Segoe UI", 9.75, FontStyle.Bold)
                TextboxLyrics.ForeColor = Color.FromArgb(235, 235, 235)
                TextboxLyrics.Location = New Point(0, 0)
                TextboxLyrics.Size = New Size(143, 0)
                TextboxLyrics.MinimumSize = New Size(143, 0)
                TextboxLyrics.MaximumSize = New Size(143, 0)
                TextboxLyrics.TextAlign = HorizontalAlignment.Center
                ToolTip1.SetToolTip(TextboxLyrics, "Make desired adjustments to lyrics, then use Edit >> Save Lyrics (Current) to keep changes upon re-opening of this file")
                TextboxLyrics.Visible = False


                'Add Handles
                AddHandler TextboxLyrics.KeyUp, AddressOf TextBox1_KeyUp
                AddHandler TextboxLyrics.MouseDoubleClick, AddressOf Textbox1_MouseDoubleClick
            Catch ex As Exception
                MyMsgBox.Show("Error in sub: Setup_Controls_OnArtwork" & Environment.NewLine & Environment.NewLine & ex.ToString, "", True)
            End Try
        End Sub

        'Title Bar Gradient
        Public Sub BarCheck_RemoveTBGrad_CheckedChanged(sender As Object, e As ItemClickEventArgs) Handles BarCheckBox_RemoveTBGrad.CheckedChanged
            If My.Settings.MiniModeOn = False Then Return
            Select Case BarCheckBox_RemoveTBGrad.Checked
                Case True
                    TitleBarImage = Nothing
                    Window_Titlebar.BackgroundImage = TitleBarImage
                    My.Settings.TBGradientCheckState = True
                Case False
                    TitleBarImage = My.Resources.TitleBar_Gradient
                    Window_Titlebar.BackgroundImage = TitleBarImage
                    My.Settings.TBGradientCheckState = False
                    ChangeHue()
            End Select
        End Sub

#End Region

#Region " Setup | Playlists"

#Region " Playlists Declartions  |   Handles"
        Dim ErrorLoadingPlaylists As String
        Dim isOldPlaylist As Boolean = False

        Public Sub AddHandles()
            Dim Playlist As GridPlaylist
            Dim Scroller As Scroller

            For i As Integer = 1 To PlaylistTabs.TabPages.Count - 1
                For Each c As Control In PlaylistTabs.SelectedTabPage.Controls
                    If c.GetType Is GetType(Scroller) Then
                        Scroller = c
                        For Each c2 As Control In Scroller.Controls
                            If c2.GetType Is GetType(GridPlaylist) Then
                                Playlist = c2
                                AddHandler Playlist.KeyUp, AddressOf playlistboxedit_KeyUp
                                AddHandler Playlist.KeyDown, AddressOf playlistboxedit_KeyDown
                                Utilities.ElevatedDragDropManager.Instance.EnableDragDrop(Playlist.Handle)
                                
                                AddHandler Playlist.SelectionChanged, AddressOf GridPlaylist_SelectionChanged
                                '  'Playlist.AllowDrop = False
                            End If
                        Next
                    End If
                Next
            Next

            AddHandler SizeTimer.Tick, AddressOf SizeTimer_tick
            AddHandler pictureform.KeyUp, AddressOf pictureform_keyup
            AddHandler pictureform.Click, AddressOf pictureform_keyup
            Try
                For i As Integer = 0 To PlaylistTabs.TabPages.Count - 1
                    AddHandler PlaylistTabs.TabPages(i).MouseClick, AddressOf TabPage1_MouseClick
                Next
            Catch
            End Try
        End Sub

#End Region



        Public Sub Setup_Playlists()


            Try
                '.............................Playlists....................................
                If IO.File.Exists(Application.StartupPath & "\Playlists\temp_playlist0.rpl") Then
                    Setup_GridPlaylists()
                    RefreshPlaylistFavorites()

                Else
                    Dim use As Boolean = False
                    If use Then
                        MyMsgBox.Show("IMPORTANT! PLEASE READ: App now has new playlists in .rpl format. You need re-add your playlists." + Environment.NewLine + "Sorry for any inconvience." _
                                                 + Environment.NewLine + "You may still re-create playlists from your saved .richplaylist files though, and when the app closes, it will automatically save the playlists in new the format." _
                                                 + Environment.NewLine + "If you did not save your playlists, you can still access the Default Playlist / temp_playlist# .richplaylist files in your app directory." _
                                                 + Environment.NewLine + "Don't forget to re-save backups of your playlists with the new format once you re-create them (for ease of use in the future).", "", True)
                    End If


                    Dim Scroller As New Scroller
                    Dim Playlist As New GridPlaylist
                    Scroller.Controls.Add(Playlist)

                    PlaylistTabs.TabPages(0).Controls.Add(SongArrangePanel)
                    PlaylistTabs.TabPages(0).Controls.Add(OpenProgressBar)
                    OpenProgressBar.Location = SongArrangePanel.Location
                    OpenProgressBar.Size = SongArrangePanel.Size
                    OpenProgressBar.Visible = False
                    PlaylistTabs.TabPages(0).Controls.Add(Scroller) '(Playlist)
                    Scroller.BringToFront()
                    AddHandler Playlist.CellMouseDoubleClick, AddressOf DoubleClickPlay
                    AddHandler Playlist.SelectionChanged, AddressOf GridPlaylist_SelectionChanged
                    Utilities.ElevatedDragDropManager.Instance.EnableDragDrop(Playlist.Handle)
                    
                    'Playlist.AllowDrop = False
                    isOldPlaylist = True
                    RefreshApp()
                    Timer_Refresh.Start()
                End If
            Catch ex As Exception
                MyMsgBox.Show("Error in sub: Setup_Playlists" & Environment.NewLine & Environment.NewLine & ex.ToString, "", True)
            End Try

        End Sub
        Public Sub Setup_GridPlaylists()


            FavoriteColorChooser.Color = My.Settings.FavColor
            If PlaylistsImported Then Return
            Try
                If My.Settings.PlaylistsCount <> 0 Then My.Settings.FirstTimeSetup = False
                If My.Settings.FirstTimeSetup = False Then
                    Dim Number As Integer = My.Settings.PlaylistsCount
                    If My.Settings.PlaylistsCount = 0 Then Number = 1
                    For num As Integer = 0 To Number - 1
                        If num <> 0 Then
                            PlaylistTabs.TabPages.Add(My.Settings.PlaylistNames.Item(num).ToString)
                        Else
                            If My.Settings.PlaylistNames.Count <> 0 Then
                                If My.Settings.PlaylistNames.Item(num).ToString <> "" Then
                                    PlaylistTabs.TabPages(num).Text = ((My.Settings.PlaylistNames.Item(num).ToString))
                                Else
                                    PlaylistTabs.TabPages(num).Text = "Default Playlist"
                                End If
                            End If
                        End If

                        PlaylistTabs.TabPages(num).AllowTouchScroll = True

                        If My.Settings.PlaylistNames.Item(num).ToString <> "Spotify" Then
                            Dim Scroller As New Scroller
                            Dim Playlist As New GridPlaylist
                            Scroller.Controls.Add(Playlist)
                            Playlist.CreateControl()
                            SongArrangePanel.CreateControl()
                            PlaylistTabs.TabPages(num).Controls.Add(Scroller)

                            AddHandler Playlist.CellMouseDoubleClick, AddressOf DoubleClickPlay
                            AddHandler Playlist.SelectionChanged, AddressOf GridPlaylist_SelectionChanged
                            AddHandler Playlist.CellMouseUp, AddressOf GridPlaylist_CellMouseDown
                            Utilities.ElevatedDragDropManager.Instance.EnableDragDrop(Playlist.Handle)
                            
                            'Playlist.AllowDrop = False
                            Dim row As Integer
                            Dim RowCount As Integer = Playlist.Rows.Count

                            Scroller.BringToFront()

                            Dim playlistFile As String = Application.StartupPath & "\Playlists\temp_playlist" & num & ".rpl"
                            If IO.File.Exists(playlistFile) Then
                                OpenFile.FileName = playlistFile
                                LoadGridData(Playlist, OpenFile.FileName)
                            Else
                                isOldPlaylist = True
                                If num = 0 Then
                                    OpenFile.FileName = Application.StartupPath & "\Playlists\Default Playlist.richplaylist"
                                    TabPlaylists_AddRichPlaylists_old()
                                Else
                                    Try
                                        OpenFile.FileName = Application.StartupPath & "\Playlists\temp_playlist" & num & ".richplaylist"
                                        TabPlaylists_AddRichPlaylists_old()
                                    Catch ex As Exception
                                        '  MyMsgBox.Show("Playlist not found!", "", True)
                                    End Try
                                End If
                            End If
                            Timer_Meta_and_Artwork.Stop()
                            If Playlist.Rows.Count = 0 Then Return
                            'Start Fresh
                            If Playlist.RowCount <> 0 Then
                                Playlist.Rows(0).Selected = True
                                row = 0
                                Playlist.CurrentCell = Playlist(0, row)
                            End If
                            Try
                                row = My.Settings.LastPlayedSongs.Item(num)
                            Catch
                                row = 0
                            End Try
                            If Playlist.RowCount <> 0 Then
                                Playlist.Rows(row).Selected = True
                                Playlist.CurrentCell = Playlist.Rows(row).Cells(0)
                            End If
                            Scroller.BringToFront()
                        End If
                    Next

                    If ErrorLoadingPlaylists <> "" Then
                        If isOldPlaylist = False Then
                            MyMsgBox.Show(ErrorLoadingPlaylists, "", True)
                        End If
                    End If
                    AppOpen = False
                Else
                    SetupGridPlaylistFirstTimeSetup()
                End If

            Catch ex As Exception
                MyMsgBox.Show(ex.ToString, "", True)
            End Try
        End Sub
        Public Sub SetupGridPlaylistFirstTimeSetup()


            Dim Scroller As New Scroller
            Dim Playlist As New GridPlaylist
            Dim num As Integer = 0

            Scroller.Controls.Add(Playlist)
            Playlist.CreateControl()
            SongArrangePanel.CreateControl()
            PlaylistTabs.TabPages(num).Controls.Add(Scroller)

            AddHandler Playlist.CellMouseDoubleClick, AddressOf DoubleClickPlay
            AddHandler Playlist.SelectionChanged, AddressOf GridPlaylist_SelectionChanged
            AddHandler Playlist.CellMouseUp, AddressOf GridPlaylist_CellMouseDown
            Utilities.ElevatedDragDropManager.Instance.EnableDragDrop(Playlist.Handle)
            
            'Playlist.AllowDrop = False
            Dim row As Integer
            Dim RowCount As Integer = Playlist.Rows.Count
            Dim playlistFile As String = Application.StartupPath & "\Playlists\temp_playlist" & num & ".rpl"
            IO.File.Create(playlistFile).Dispose()
            Scroller.BringToFront()
            Timer_Meta_and_Artwork.Stop()
        End Sub

        Public Sub BringLastPlaylist_to_Focus()


            Try
                PlaylistTabs.SelectedTabPageIndex = My.Settings.PlaylistsSelected
                Dim i As Integer = PlaylistTabs.SelectedTabPageIndex
                PlaylistTabs.TabPages(i).Controls.Add(SongArrangePanel)
                PlaylistTabs.TabPages(i).Controls.Add(OpenProgressBar)
                OpenProgressBar.Location = SongArrangePanel.Location
                OpenProgressBar.Size = SongArrangePanel.Size
                OpenProgressBar.Visible = False
                SongArrangePanel.BringToFront()
                OpenProgressBar.BringToFront()
                If PlaylistTabs.TabPages(i).Text = "Spotify" Then Return
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
                Scroller.Controls.Add(Playlist)
                If Playlist.Rows.Count = 0 Then Return
                Dim row As Integer = Playlist.CurrentCell.RowIndex
                Dim num As Integer = My.Settings.PlaylistsSelected

                CheckIfVideo()
                If InitiatePlayOnStart Then
                    Dim use As Boolean = False
                    If use Then
                        If IsVideo Then
                            If VLC_installed Then
                                VideoPlay()
                                VlcPlayer.input.position = My.Settings.LastPlayedPositions.Item(num)
                            Else
                                Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                                Artwork.BackgroundImage = DrawText("Videos unsupported. Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)
                            End If

                        Else
                            Try
                                VideoPlay()
                                AudioPlayer.Instance.Position = My.Settings.LastPlayedPositions.Item(num)
                            Catch
                            End Try
                        End If
                    End If
                End If
                PlaylistTabs.MakePageVisible(PlaylistTabs.SelectedTabPage)
                AppOpenFinished = True

                ' PlaylistTabs.TabPages(i).Controls.Add(TextBox_PlaylistSearch)
                ' TextBox_PlaylistSearch.Dock = DockStyle.Top
                ' TextBox_PlaylistSearch.Controls.Add(But_Search)
                ' But_Search.BringToFront()
                ' But_Search.Top = 1
                PlaylistTabs.TabPages(i).Controls.Add(SearchBox_Panel)

                PlaylistTabs.MakePageVisible(PlaylistTabs.SelectedTabPage)
                PlaylistTabs.MakePageVisible(PlaylistTabs.SelectedTabPage)
                PlaylistTabs.MakePageVisible(PlaylistTabs.SelectedTabPage)
            Catch
            End Try
        End Sub

        Public Sub RefreshPlaylistFavorites()


            Try
                For num As Integer = 0 To My.Settings.PlaylistsCount - 1
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
                    Dim RowCount As Integer
                    If RowCount > 0 Then
                        For i As Integer = 0 To RowCount - 1
                            If Playlist.Item(4, i).Value = "" Then
                                Playlist.Item(4, i).Value = "False"
                            End If
                        Next
                    End If
                Next
            Catch
            End Try
        End Sub

        Public Sub SetupPlaylistTabsOrientation()


            Select Case My.Settings.TabOrientation
                Case 0
                    PlaylistTabs.HeaderOrientation = TabOrientation.Horizontal
                Case 1
                    PlaylistTabs.HeaderOrientation = TabOrientation.Vertical
            End Select
            PlaylistTabs.TabPageWidth = My.Settings.TabWidth
        End Sub


        Public Sub LoadLastPlayedPos()


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
            Dim row As Integer = Playlist.CurrentCell.RowIndex
            Dim SongFilename As String = Playlist.Item(6, row).Value.ToString
            AudioPlayer.Instance.ResetTrackList()
            Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
            AudioPlayer.Instance.TrackList.Tracks.Add(track)
            AudioPlayer.Instance.Position = My.Settings.LastPlayedPositions(PlaylistTabs.SelectedTabPageIndex)
        End Sub

#End Region
#Region " Command Line"

        Public Sub OpenWith_CommandLine()


            If My.Application.CommandLineArgs.Count > 0 Then
                'Open With
                Dim extension As String
                For a As Integer = 0 To My.Application.CommandLineArgs.Count - 1
                    extension = System.IO.Path.GetExtension(My.Application.CommandLineArgs(a))
                Next
                Select Case extension
                    Case ".rpl"
                        Try
                            AddedPlaylistFromComamandLine = True
                            AddNewPlaylist()
                            OpenFile.FileName = (My.Application.CommandLineArgs(0).ToString())
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
                            LoadGridData(Playlist, OpenFile.FileName)

                            Dim Row As Integer = Playlist.CurrentCell.RowIndex
                            Dim RowCount As Integer = Playlist.RowCount
                            Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                            Playlist.Rows(0).Selected = True

                            CheckIfVideo()
                            If IsVideo Then
                                If VLC_installed Then
                                    VLCclearPlaylists()
                                    VlcPlayer.playlist.add("file:///" & SongFilename)

                                    ' VlcPlayer.SetMedia(New IO.FileInfo (SongFilename))
                                    VlcPlayer.playlist.play()

                                Else
                                    Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                                    Artwork.BackgroundImage = DrawText("Videos unsupported." + Environment.NewLine + "Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)
                                End If

                            Else
                                Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                                AudioPlayer.Instance.TrackList.Tracks.Add(track)
                                AudioPlayer.Instance.Play(True)
                                SongStartOver = True
                            End If

                            'Refresh Artwork
                            Timer_Meta_and_Artwork.Start()

                            'Setup Names
                            PlaylistTabs.SelectedTabPage.Text = Path.GetFileNameWithoutExtension(OpenFile.FileName)
                        Catch
                            MyMsgBox.Show("Error: 0x8: Error loading playlist! Playlist may be corrupted. Try to re-create Playlist.", "", True)
                        End Try
                    Case ".mp3"

                        AddedPlaylistFromComamandLine = True
                        AddNewPlaylist()

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

                        For Each x As String In (My.Application.CommandLineArgs)
                            OpenFile.FileName = x
                            For Each i As String In OpenFile.FileNames
                                Playlist.Rows.Add()
                                Playlist.Item(6, Playlist.Rows.Count - 1).Value = i
                                Playlist.Item(0, Playlist.Rows.Count - 1).Value = Path.GetFileNameWithoutExtension(i)
                                Playlist.Item(1, Playlist.Rows.Count - 1).Value = ""
                                Playlist.Item(2, Playlist.Rows.Count - 1).Value = ""
                                Playlist.Item(3, Playlist.Rows.Count - 1).Value = ""
                                Playlist.Item(4, Playlist.Rows.Count - 1).Value = "False"
                                Playlist.Item(5, Playlist.Rows.Count - 1).Value = 0
                            Next
                            Dim row As Integer = 0
                            Dim SongFilename As String = Playlist.Item(6, row).Value.ToString
                            CheckIfVideo()
                            If IsVideo Then
                                If VLC_installed Then
                                    VLCclearPlaylists()
                                    VlcPlayer.playlist.add("file:///" & SongFilename)
                                    ' VlcPlayer.SetMedia(New IO.FileInfo (SongFilename))
                                Else
                                    Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                                    Artwork.BackgroundImage = DrawText("Videos unsupported." + Environment.NewLine + "Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)
                                End If

                            Else
                                Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                                AudioPlayer.Instance.TrackList.Tracks.Add(track)
                            End If

                            Timer_Meta_and_Artwork.Start()  'Set Artwork

                            If firstopen Then
                                Timer_Seek.Stop()
                                CheckIfVideo()
                                If IsVideo Then
                                    If VLC_installed Then
                                        '  VlcPlayer.Playlist.Play() 
                                        VlcPlayer.playlist.play()
                                    Else
                                        Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                                        Artwork.BackgroundImage = DrawText("Videos unsupported." + Environment.NewLine + "Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)
                                    End If

                                Else
                                    AudioPlayer.Instance.Play(True)
                                End If
                                Timer_Seek.Start()
                                Timer_Meta_and_Artwork.Start() ' Set Artwork
                                firstopen = False
                            End If
                        Next
                End Select
            End If
        End Sub

#End Region

      
        'Other Stuff
        Public Sub AddAllOtherStuff()


            Try
                Repeat_Shuffle_State()


                If Not My.Settings.SaveItemPosition = 0 Then
                    BarCheckBox_AllowSaveItemPosition.Checked = True
                End If


                BarCheckBox_Player_Volume.Checked = My.Settings.EnablePlayerVolumeCheckState
                BarCheckBox_PlaylistSearchbox.Checked = My.Settings.PlaylistSearchBoxCheckState


                PlaylistTabs.TabMiddleClickFiringMode = DevExpress.XtraTab.TabMiddleClickFiringMode.MouseUp


                If AddedPlaylistFromComamandLine Then
                    PlaylistTabs.SelectedTabPageIndex = 0
                End If


                Setup_ArtworkFinder()


                Timer_PlaylistsSizes.Start()


                BarCheckBox_AllowSaveItemPosition.Checked = True
                SetupControlLocations()


                ChangeHue()


                AutoSaveTimer.Interval = 600000
                AutoSaveTimer.Enabled = True


                If My.Settings.MiniModeOn = True Then
                    BarCheckBox_MiniMode.Checked = True
                Else
                    Me.Size = My.Settings.OriginalSize
                End If
                BarCheckBox_RemoveTBGrad.Checked = My.Settings.TBGradientCheckState


                '/////////////////////////////////  OPEN FILE  ///////////////////////////
                OpenFile.Multiselect = True
                OpenFile.Title = "Add Music Files"
                '  OpenFile.Filter = "Audio Files|*.wav;*.ogg;*.flac;*.aiff;*.aac;*.ape;*.m4a;*.wma;*.mp3;*.cda|Rich Player Playlists|*.richplaylist|Video Files|*.aaf;*.3gp;*.gif;*.asf;*.avchd;*.avi;*.cam;*.dat;*.dsh;*.flv;*.m1v;*.mpg;*.mp4;*.mpeg;*.mpe;*.mpv;*.fla;*.flr;*.sol;*.m4v;*.mkv;*.wrap;*.mng;*.mov;*.mxf;*.roq;*.off;*.rm;*.svi;*.smi;*.swf;*.wmv|Image Files|*.BMP;*.DIB;*.RLE;*.JPG;*.JPEG;*.JPE;*.JFIF;*.GIF;*.TIF;*.TIFF;*.PNG"
                OpenFile.Filter = "Media Files|*.*|Rich Player Playlists|*.richplaylist"
                SaveFile.Title = "Save as Playlist"
                SaveFile.Filter = "Rich Player Playlists|*.rpl"
                '////////////////////////////////    END     /////////////////////////////
                OpenFolder.IsFolderPicker = True


                SpotifyTimer.Start()
            Catch ex As Exception
                MyMsgBox.Show("Error in sub: AddAllOtherStuff" & Environment.NewLine & Environment.NewLine & ex.ToString, "", True)
            End Try
            My.Settings.Save()
        End Sub
        Public Sub Repeat_Shuffle_State()
            Try
                Select Case My.Settings.Repeat
                    Case 0
                        repeat = False
                        repeatOne = False
                        repeatAll = False
                    Case 1
                        repeat = True
                        repeatOne = True
                        repeatAll = False
                        But_Repeat.BackgroundImage = RepeatOneImage
                    Case 2
                        repeat = True
                        repeatOne = False
                        repeatAll = True
                        But_Repeat.BackgroundImage = RepeatAllImage
                End Select
            Catch
                MyMsgBox.Show("Error: 0x9: Error loading Repeat state", "", True)
            End Try
            Try
                Select Case My.Settings.Shuffle
                    Case 1
                        IsShuffle = True
                        But_Shuffle.BackgroundImage = ShuffleImage
                End Select
            Catch
                MyMsgBox.Show("Error: 0x10: Error loading Shuffle state", "", True)
            End Try

        End Sub

     

#End Region

#Region " Form  |   Closing"
#Region " Declarations"
        Public SaveALLBut As Boolean = False
        Public ClosingErrors As String
        Public FormClosingval As Boolean = False
        Public ForceClose As Boolean = False
        Public FormClosingNow As Boolean = False
#End Region

        'Closing
        Public Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing


            If ForceClose Then Return
            Try
                hkr.Unregister(0) : hkr.Unregister(1) : hkr.Unregister(2) : hkr.Unregister(3) : hkr.Unregister(4) : hkr.Unregister(5)
                hkr.Unregister(6) : hkr.Unregister(7) : hkr.Unregister(8) : hkr.Unregister(9) : hkr.Unregister(10) : hkr.Unregister(11)
                hkr.Unregister(12) : hkr.Unregister(13) : hkr.Unregister(14) : hkr.Unregister(15) : hkr.Unregister(16)
            Catch : MyMsgBox.Show("Error 0x19: Unable to unregister hotkeys", "", True) : End Try
            My.Settings.CarFormOpened = False
            FormClosing()
            FormClosingNow = True
            If ClosingErrors <> "" Then
                If MyFullMsgBox.Show("Close? You have the following errors you can choose to correct." + Environment.NewLine + Environment.NewLine + ClosingErrors, _
                                   "Close Rich Player?", True, MyFullMsgBox.CustomButtons.YesNo) = DialogResult.No Then
                    e.Cancel = True : ClosingErrors = ""
                Else
                    My.Settings.PlaylistsCount = PlaylistTabs.TabPages.Count
                    If My.Settings.Playlist0HasFiles Then : My.Settings.FirstTimeSetup = False : End If
                    My.Settings.Save() : RefreshApp() : My.Settings.Save() : Application.ExitThread() : Application.Exit() : If FormRestarting = False Then Process.GetCurrentProcess.Kill()
                End If
            Else
                My.Settings.Save() : RefreshApp() : My.Settings.Save() : Application.ExitThread() : Application.Exit() : If FormRestarting = False Then Process.GetCurrentProcess.Kill()
            End If
        End Sub
        'Code
        Public Sub FormClosing()


            If My.Settings.DriveMode Then
                If CarForm.NowPlayingInit = False Then Return
            End If

            If ForceClose Then Return
            FormClosingval = True : If DoResetSettings = True Then Exit Sub
            My.Settings.PlaylistsCount = PlaylistTabs.TabPages.Count
            My.Settings.LastPlayedSongs.Clear()
            '...................Save Playlist Items...............
            For num As Integer = 0 To My.Settings.PlaylistsCount - 1
                Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.TabPages(num).Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next
                If num = 0 Then : If Playlist.RowCount <> 0 Then : My.Settings.Playlist0HasFiles = True : My.Settings.FirstTimeSetup = False
                    Else : My.Settings.Playlist0HasFiles = False : End If : End If

                Try : If Playlist.RowCount > 0 Then
                        Dim Row As Integer = Playlist.CurrentCell.RowIndex
                        Dim RowCount As Integer = Playlist.RowCount : Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                        If RowCount > 0 Then
                            System.IO.File.WriteAllText(TempPlaylistNew & num & ".rpl", "") : SaveGridData(Playlist, TempPlaylistNew & num & ".rpl")
                            My.Settings.LastPlayedSongs.Insert(num, Row)
                        End If
                    End If
                Catch : ClosingErrors = ClosingErrors + "Unable to save playlist(s)! You may have blank playlists. Having Blank playlists" + _
                    " may result in app opening with errors and/or loss of other playlists!!" + Environment.NewLine : End Try
            Next

            '.......................Save Position of Active Tab....................
            Try : Dim num As Integer = PlaylistTabs.SelectedTabPageIndex
                If IsVideo Then
                    Try
                        My.Settings.LastPlayedPositions(num) = VlcPlayer.input.position
                    Catch ex As Exception
                    End Try

                Else
                    My.Settings.LastPlayedPositions(num) = AudioPlayer.Instance.Position.ToString
                End If : Catch ex As Exception : End Try


            '.......................Last Played Item...................
            Try
                For num As Integer = 0 To My.Settings.PlaylistsCount - 1
                    If PlaylistTabs.TabPages(num).Text <> "Spotify" Then
                        Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.TabPages(num).Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next
                        Try
                            If Playlist.RowCount > 0 Then
                                Dim Row As Integer = Playlist.CurrentCell.RowIndex
                                Dim RowCount As Integer = Playlist.RowCount
                                Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                                If RowCount > 0 Then
                                    My.Settings.LastPlayedSongs.Item(num) = Row
                                End If
                            End If
                        Catch : ClosingErrors = ClosingErrors + "Unable to save playlist(s) last played songs! Possibly no items in playlists? Having Blank playlists" + _
                    " may result in app opening with errors and/or loss of other playlists!!" + Environment.NewLine
                        End Try : End If : Next : Catch
                ClosingErrors = ClosingErrors + "Error 0x22: Unable to save Playlists' Last Played item! (Playlists cannot be blank when saving/closing app!)" + Environment.NewLine
                ' MyMsgBox.Show("Error 0x22: Unable to save other Playlists' Last Played item!")
            End Try


            '..............Playlist Selected................
            Try : My.Settings.PlaylistsSelected = PlaylistTabs.SelectedTabPageIndex
            Catch : ClosingErrors = ClosingErrors + "Error 0x25: Unable to save Selected Playlist status" + Environment.NewLine : End Try


            '.......................Playlist Names..............
            If ClosingErrors = "" Then : For num As Integer = 0 To PlaylistTabs.TabPages.Count - 1
                    My.Settings.PlaylistNames.Item(num) = PlaylistTabs.TabPages(num).Text.ToString : Next : End If


            '...................WINDOW LOCATION................
            Try
                If My.Settings.DriveMode = False Then
                    If Me.WindowState = FormWindowState.Maximized Then
                        My.Settings.PlayerLocation = CurrentPoint : My.Settings.FormSize = CurrentSize
                    Else : My.Settings.PlayerLocation = Me.Location
                        My.Settings.FormSize = Me.Size
                    End If
                Else
                    My.Settings.PlayerLocation = My.Settings.OriginalLocation '  CurrentPoint
                    My.Settings.FormSize = CurrentSize

                End If

            Catch : ClosingErrors = ClosingErrors + "Error 0x26: Unable to save App's position on screen!" + Environment.NewLine : End Try

            '.......................Trackbar Values.........................
            My.Settings.BASS_Speed = trackBar_Speed2.Value
            My.Settings.BASS_Pitch = trackbar_Pitch2.Value
            My.Settings.Player_Volume = TrackBar_PlayerVol2.Value
            My.Settings.EnablePlayerVolumeCheckState = BarCheckBox_Player_Volume.Checked


            '................Video Last Played Positions................
            Try 'If VlcPlayer.playlist.items.count > 0 Then : End If
            Catch : End Try

            My.Settings.PlaylistsCount = PlaylistTabs.TabPages.Count

            If Not IsAutoSaving Then : If Not SaveALLBut Then : My.Settings.PlaylistsCount = PlaylistTabs.TabPages.Count : My.Settings.Save() : End If : End If


            '......................SAVE SETTINGS........................
            My.Settings.Save()

            If SaveALLBut = True Then : If Not IsAutoSaving Then : If ClosingErrors = "" Then : MyMsgBox.Show("Current settings and playlists saved!", "", True) : End If : End If : End If
        End Sub


        'Save All
        Public Sub SaveAllSettings_MenuBut_ItemClick(sender As Object, e As XtraBars.ItemClickEventArgs) Handles BarBut_SaveAllSettings.ItemClick
            SaveAll()
        End Sub
        Public Sub SaveAll()
            SaveALLBut = True : FormClosing() : SaveALLBut = False
        End Sub

        'Windows is Shutting Down
        Public Sub Handler_SessionEnding(ByVal sender As Object, ByVal e As Microsoft.Win32.SessionEndingEventArgs)
            If e.Reason = Microsoft.Win32.SessionEndReasons.SystemShutdown Then : Application.Exit() : End If
        End Sub

#End Region

#Region " Form  |   Management"
#Region " Declarations"
        Public NeedResizeRefresh As Boolean = False
        Public FirstOpenSize As Boolean = True
        Public SizeChanged As Boolean = False
        Public CurrentSize As Size
        Public CurrentPoint As Point
        Public SizeTimer As New Timer
        Dim screenWidth As Integer = Screen.PrimaryScreen.Bounds.Width
        Dim screenHeight As Integer = Screen.PrimaryScreen.Bounds.Height
#End Region



#Region " Resize    |   Main"

        'Form Resize
        Public Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
            Try
                If IsBuffering Then Return
                If WaitMiniMode Then Return
                If xcarform.DontSetParents Then Return
                If My.Settings.DriveMode Then Return
                If AppOpenFinished = False Then Return
                If AppOpen = False Then
                    Try
                        Splitter = Me.Splitter
                        Splitter.Size = New Size(Splitter.Size.Width, Me.Height - ControlsBGPanel.Height - 40 - 20)
                    Catch ex As Exception
                    End Try

                    Timer_PlaylistsSizes.Start()

                    PlaybackPanel.Location = New Point((ControlsBGPanel.Width / 2) - (PlaybackPanel.Width / 2) - 38, PlaybackPanel.Location.Y)
                    xpos = (Splitter.Panel1.Width / 2) - (PictureBoxSpec.Width / 2)
                    If My.Settings.MiniModeOn = False Then
                        PictureBoxSpec.Location = New Point(xpos, PictureBoxSpec.Location.Y)
                    End If
                End If
                NeedResizeRefresh = True
                resizeDir = ResizeDirection.None
            Catch ex As Exception

            End Try


        End Sub
        Public Sub AppResizeEnd() Handles MyBase.ResizeEnd
            SetupControlLocations()
            resizeDir = ResizeDirection.None
        End Sub
        Public Sub SetupControlLocations()
            If xcarform.DontSetParents Then Return
            If My.Settings.DriveMode Then Return
            Splitter.Size = New Size(Splitter.Size.Width, Me.Height - ControlsBGPanel.Height - 40 - 20)
            PlaybackPanel.Location = New Point((ControlsBGPanel.Width / 2) - (PlaybackPanel.Width / 2) - 38, PlaybackPanel.Location.Y)
            xpos = (Splitter.Panel1.Width / 2) - (PictureBoxSpec.Width / 2)
            If My.Settings.MiniModeOn Then Return
            PictureBoxSpec.Location = New Point(xpos, PictureBoxSpec.Location.Y)
        End Sub
        Public Sub SplitterResize() Handles Splitter.SplitterPositionChanged
            If xcarform.DontSetParents Then Return
            If My.Settings.DriveMode Then Return
            Timer_PlaylistsSizes.Start()
            My.Settings.SplitterVal = Splitter.SplitterPosition
            xpos = (Splitter.Panel1.Width / 2) - (PictureBoxSpec.Width / 2)
            If My.Settings.MiniModeOn = False Then
                PictureBoxSpec.Location = New Point(xpos, PictureBoxSpec.Location.Y)
            End If
            NeedResizeRefresh = True
        End Sub

        'Manually Change Size
        Public Sub SubBarPlayerSizeWidth_EditValueChanged(sender As Object, e As EventArgs) Handles BarEdit_PlayerSizeWidthTextbox.EditValueChanged, BarEdit_PlayerSizeHeightTextbox.EditValueChanged
            Try
                If Not AppOpen And Not SizeChanged Then
                    If AllowSizeChange Then
                        Me.Size = New Size(BarEdit_PlayerSizeWidthTextbox.EditValue.ToString, Me.Height)
                        Me.Size = New Size(Me.Size.Width, BarEdit_PlayerSizeHeightTextbox.EditValue.ToString)
                    End If
                End If
                SizeChanged = False
            Catch
                MyMsgBox.Show("Error 0x31: Error changing size! Please try again.", "", True)
            End Try
        End Sub
        Public Sub SubBarPlayerSizeWidth_ItemClick(sender As Object, e As EventArgs) Handles BarEdit_PlayerSizeWidthTextbox.ItemClick, BarEdit_PlayerSizeHeightTextbox.ItemClick
            AllowSizeChange = True
        End Sub
        Public Sub BarEditItem1_EditValueChanged(sender As Object, e As EventArgs) Handles BarEdit_WindowSizePresets.EditValueChanged, BarEdit_WindowSizePresets.ItemClick
            '  AllowSizeChange = True
            If BarEdit_WindowSizePresets.EditValue = 1 Then
                Me.Size = New Size(814, 410)
            ElseIf BarEdit_WindowSizePresets.EditValue = 2 Then
                Me.Size = New Size(903, 463)
            ElseIf BarEdit_WindowSizePresets.EditValue = 3 Then
                Me.Size = New Size(964, 494)
            ElseIf BarEdit_WindowSizePresets.EditValue = 4 Then
                Me.Size = New Size(1074, 556)
            ElseIf BarEdit_WindowSizePresets.EditValue = 5 Then
                Me.Size = New Size(1168, 608)
            ElseIf BarEdit_WindowSizePresets.EditValue = 6 Then
                Me.Size = New Size(1263, 662)
            End If
        End Sub




#Region " Declarations"

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


        Private Const BorderWidth As Integer = 6
        Private _resizeDir As ResizeDirection = ResizeDirection.None


#End Region
#Region " Change Cursors when hover     unused"





        ' Mouse over Splitter
        Dim SplitterOver As Boolean = False
        Public Sub Splitter_MouseMove(ByVal sender As System.Object, ByVal e As MouseEventArgs) Handles Splitter.MouseMove
            Dim x As Integer = e.X
            Dim y As Integer = e.Y

            With Splitter
                If (x < 0) Or (y < 0) Or (x > .Width) Or (y > .Height) Then
                    ReleaseCapture()
                    SplitterOver = False
                Else
                    If Not SplitterOver Then
                        SplitterOver = True
                    End If
                End If
            End With
        End Sub


#End Region


#End Region
#Region " Resize    |   Grabber"
#Region " Graphics"
        Dim ResizeGrabberhover As Boolean = False




        Public Sub ResizeGrabber_MouseEnter(sender As Object, e As EventArgs) Handles But_ResizeGrabber.MouseEnter

            But_ResizeGrabber.BackgroundImage = My.Resources.Resize_Grabber_New_Hover 'My.Resources.Resizer_Grabber_Hover
            ResizeGrabberhover = True
        End Sub
        Public Sub ResizeGrabber_MouseLeave(sender As Object, e As EventArgs) Handles But_ResizeGrabber.MouseLeave

            But_ResizeGrabber.BackgroundImage = My.Resources.Resize_Grabber_New_Norm 'My.Resources.Resizer_Grabber
            ResizeGrabberhover = False
        End Sub
#End Region
        Dim GripDrag As Boolean
        Dim InitialSizeX As Integer
        Dim InitialSizeY As Integer


        Public Sub ResizeGrabber_MouseDown(sender As Object, e As MouseEventArgs) Handles But_ResizeGrabber.MouseDown

            But_ResizeGrabber.BackgroundImage = My.Resources.Resize_Grabber_New_Press  'My.Resources.Resizer_Grabber_Press
            If e.Button = MouseButtons.Left Then 'If the control is being left-clicked
                GripDrag = True 'Confirms the grip is ready to be dragged
                InitialSizeX = Me.Width 'Sets the initial width
                InitialSizeY = Me.Height 'Sets the initial height
            End If


        End Sub
        Public Sub ResizeGrabber_MouseUp(sender As Object, e As MouseEventArgs) Handles But_ResizeGrabber.MouseUp
            Dim use As Boolean = True

            If GripDrag = True Then

                If use Then
                    Me.Width = InitialSizeX + (Cursor.Position.X - (Me.Width + Me.Location.X))  'Increases the width of the form by the amount the grip has been dragged towards the right
                    Me.Height = InitialSizeY + (Cursor.Position.Y - (Me.Height + Me.Location.Y))  'Increases the height of the form by the amount the grip has been dragged downward

                    InitialSizeX = Me.Width 'Resets the value to the form's current width
                    InitialSizeY = Me.Height 'Resets the value to the form's current height

                    Me.Refresh()
                End If


            End If
            GripDrag = False
            If ResizeGrabberhover = True Then
                But_ResizeGrabber.BackgroundImage = My.Resources.Resize_Grabber_New_Hover  ' My.Resources.Resizer_Grabber_Hover
            Else
                But_ResizeGrabber.BackgroundImage = My.Resources.Resize_Grabber_New_Norm ' My.Resources.Resizer_Grabber
            End If
        End Sub
        Public Sub ResizeGrabber_MouseMove(sender As Object, e As MouseEventArgs) Handles But_ResizeGrabber.MouseMove
            Return
            SuspendDrawing()
            '    Return
            If GripDrag = True Then
                Me.Width = InitialSizeX + (Cursor.Position.X - (Me.Width + Me.Location.X))  'Increases the width of the form by the amount the grip has been dragged towards the right
                Me.Height = InitialSizeY + (Cursor.Position.Y - (Me.Height + Me.Location.Y))  'Increases the height of the form by the amount the grip has been dragged downward

                InitialSizeX = Me.Width 'Resets the value to the form's current width
                InitialSizeY = Me.Height 'Resets the value to the form's current height
                Me.Refresh()
            End If
            ResumeDrawing()
        End Sub


#End Region
#Region " Resize    |   SizeChanged"

        Public Sub Form1_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
            Try
                If Not Me.WindowState = FormWindowState.Maximized Then
                    CurrentSize = Me.Size
                    CurrentPoint = Me.Location
                Else
                    MaximumSizeChanged = True
                End If
                SizeChanged = True
                If AppOpen = False Then
                    BarEdit_PlayerSizeWidthTextbox.EditValue = Me.Size.Width
                    BarEdit_PlayerSizeHeightTextbox.EditValue = Me.Size.Height
                Else
                    SizeTimer.Start()
                End If

                ResetThumbnail()
            Catch ex As Exception
            End Try
        End Sub
        Public Sub SizeTimer_tick()
            If AppOpenFinished = False Then Return
            If AppOpen = False Then
                BarEdit_PlayerSizeWidthTextbox.EditValue = Me.Size.Width
                BarEdit_PlayerSizeHeightTextbox.EditValue = Me.Size.Height
                SizeTimer.Stop()
            Else
                SizeTimer.Start()
            End If
        End Sub

#End Region





        ' not used
        Public Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
            Return
            If e.Button = MouseButtons.Left And Me.WindowState <> FormWindowState.Maximized Then
                ResizeForm(Me, resizeDir)
            End If
        End Sub
        Public Sub MoveForm(frm As Form)
            ReleaseCapture()
            SendMessage(frm.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0)
        End Sub

        Public Sub ResizeForm(ByVal frm As Form, ByVal direction As ResizeDirection)
            Dim dir As Integer = -1

            Select Case direction
                Case ResizeDirection.Left
                    dir = HTLEFT
                Case ResizeDirection.TopLeft
                    dir = HTTOPLEFT
                Case ResizeDirection.Top
                    dir = HTTOP
                Case ResizeDirection.TopRight
                    dir = HTTOPRIGHT
                Case ResizeDirection.Right
                    dir = HTRIGHT
                Case ResizeDirection.BottomRight
                    dir = HTBOTTOMRIGHT
                Case ResizeDirection.Bottom
                    dir = HTBOTTOM
                Case ResizeDirection.BottomLeft
                    dir = HTBOTTOMLEFT
            End Select
            If dir <> -1 Then
                ' Resize form
                ReleaseCapture()
                SendMessage(frm.Handle, WM_NCLBUTTONDOWN, dir, 0)

            End If
        End Sub
        Public Sub SetDirection()

            Dim C_Location = Me.PointToClient(Cursor.Position)

            'Calculate which direction to resize based on mouse position
            If C_Location.X < BorderWidth And C_Location.Y < BorderWidth Then
                resizeDir = ResizeDirection.TopLeft
            ElseIf C_Location.X < BorderWidth And C_Location.Y > Me.Height - BorderWidth Then
                resizeDir = ResizeDirection.BottomLeft
            ElseIf C_Location.X > Me.Width - BorderWidth And C_Location.Y > Me.Height - BorderWidth Then
                resizeDir = ResizeDirection.BottomRight
            ElseIf C_Location.X > Me.Width - BorderWidth And C_Location.Y < BorderWidth Then
                resizeDir = ResizeDirection.TopRight
            ElseIf C_Location.X < BorderWidth Then
                resizeDir = ResizeDirection.Left
            ElseIf C_Location.X > Me.Width - BorderWidth Then
                resizeDir = ResizeDirection.Right
            ElseIf C_Location.Y < BorderWidth Then
                resizeDir = ResizeDirection.Top
            ElseIf C_Location.Y > Me.Height - BorderWidth Then
                resizeDir = ResizeDirection.Bottom
            Else
                resizeDir = ResizeDirection.None
            End If

        End Sub



#Region " Resize to Screen Height"

        Public Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles ControlsBGPanel.MouseMove, Window_Titlebar.MouseMove, MyBase.MouseMove
            If My.Settings.MiniModeOn Then Return
            If e.Location.X < BorderWidth And e.Location.Y < BorderWidth Then
                ' resizeDir = ResizeDirection.TopLeft

            ElseIf e.Location.X < BorderWidth And e.Location.Y > Me.Height - BorderWidth Then
                ' resizeDir = ResizeDirection.BottomLeft

            ElseIf e.Location.X > Me.Width - BorderWidth And e.Location.Y > Me.Height - BorderWidth Then
                ' resizeDir = ResizeDirection.BottomRight

            ElseIf e.Location.X > Me.Width - BorderWidth And e.Location.Y < BorderWidth Then
                '  resizeDir = ResizeDirection.TopRight

            ElseIf e.Location.X < BorderWidth Then
                If sender.name = "Form1" Then
                    '     resizeDir = ResizeDirection.Left
                End If

            ElseIf e.Location.X > Me.Width - BorderWidth Then
                If sender.name = "Form1" Then
                    '      resizeDir = ResizeDirection.Right
                End If

            ElseIf e.Location.Y < BorderWidth Then
                If sender.name = "Window_Titlebar" Then
                    resizeDir = ResizeDirection.Top
                End If

            ElseIf e.Location.Y > sender.Height - BorderWidth Then
                If sender.name <> "Window_Titlebar" Then
                    resizeDir = ResizeDirection.Bottom
                End If

            Else
                resizeDir = ResizeDirection.None
            End If
        End Sub


        Public Sub Form1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles Window_Titlebar.DoubleClick, Me.DoubleClick, ControlsBGPanel.DoubleClick

            If resizeDir = ResizeDirection.Top Then
                AppFillScreenHeight()
                Return
            ElseIf resizeDir = ResizeDirection.Bottom Then
                AppFillScreenHeight()
                Return
            ElseIf resizeDir = ResizeDirection.None Then
                If sender.name = "Window_Titlebar" Then
                    If e.Button = MouseButtons.Left Then
                        If Me.WindowState.Maximized Then
                            Me.WindowState = FormWindowState.Normal
                        Else
                            Me.WindowState = FormWindowState.Maximized
                        End If
                    End If
                End If
            End If
        End Sub

        Dim oldHeight As Integer
        Dim oldTop As Integer
        Public Sub AppFillScreenHeight()

            If Me.Height <> Screen.FromControl(Me).WorkingArea.Height Then
                oldHeight = Me.Height
                oldTop = Me.Top

                Me.Height = Screen.FromControl(Me).WorkingArea.Height
                Me.Top = Screen.FromControl(Me).WorkingArea.Top
                If IsOnScreen(Me) = False Then
                    Me.Top = 0
                End If
                resizeDir = ResizeDirection.None
            Else
                Try
                    Me.Height = oldHeight
                    Me.Top = oldTop
                    resizeDir = ResizeDirection.None
                Catch ex As Exception : End Try
            End If

        End Sub

#End Region

        ' App On Screen ??
        Public Function IsOnScreen(ByVal form As Form) As Boolean
            If My.Settings.SimplisticMode Then Exit Function
            If WasSimplisticOn Then Exit Function
            Dim screens() As Screen = Screen.AllScreens
            For Each scrn As Screen In screens
                Dim formRectangle As Rectangle = New Rectangle(form.Left, form.Top, form.Width, form.Height)
                If scrn.WorkingArea.Contains(formRectangle) Then
                    Return True
                End If
            Next
            Return False
        End Function

        ' Focus Me
        Public Sub BringMeToFocus()
            Me.Activate()
            Me.Show()
            Me.Focus()
        End Sub


#Region " App   |   Location"

        Public Sub Load_Location()
            Try
                Me.Location = New Point(My.Settings.PlayerLocation)
                BarEdit_LocationXTextbox.EditValue = Me.Location.X
                BarEdit_LocationYTextbox.EditValue = Me.Location.Y
            Catch
                MyMsgBox.Show("Error 0x30: Error loading last window location", "", True)
            End Try

        End Sub

        Public Sub Form1_LocationChanged(sender As Object, e As EventArgs) Handles MyBase.LocationChanged
            If Me.WindowState = FormWindowState.Maximized Then Return
            CurrentPoint = Me.Location
            If AppOpen = False Then
                LocationChanged = True
                BarEdit_LocationXTextbox.EditValue = Me.Location.X
                BarEdit_LocationYTextbox.EditValue = Me.Location.Y
            End If
        End Sub

        'Change App Location
        Public Sub LocationTextboxs_EditValueChanged(sender As Object, e As EventArgs) Handles BarEdit_LocationXTextbox.EditValueChanged, BarEdit_LocationYTextbox.EditValueChanged
            If AppOpen = False Then
                If LocationChanged = False Then
                    If BarEdit_LocationXTextbox.EditValue >= 0 And BarEdit_LocationYTextbox.EditValue >= 0 Then
                        If BarEdit_LocationXTextbox.EditValue < (FullScreenSize.X + Me.Size.Width) And BarEdit_LocationYTextbox.EditValue < (FullScreenSize.Y - Me.Size.Height) Then
                            Me.Location = New Point(BarEdit_LocationXTextbox.EditValue, BarEdit_LocationYTextbox.EditValue)
                        End If
                    Else
                        BarEdit_LocationXTextbox.EditValue = Me.Location.X
                        BarEdit_LocationYTextbox.EditValue = Me.Location.Y
                    End If
                End If
                LocationChanged = False
            End If
        End Sub
        Public Function FullScreenSize() As Point
            Dim rect As New Rectangle(Integer.MaxValue, Integer.MaxValue, Integer.MinValue, Integer.MinValue)
            For Each screen__1 As Screen In Screen.AllScreens
                rect = Rectangle.Union(rect, screen__1.Bounds)
            Next
            Console.WriteLine("(width, height) = ({0}, {1})", rect.Width, rect.Height)
            FullScreenSize = New Point(rect.Width, rect.Height)
            Return FullScreenSize
        End Function

#End Region

#Region " App   |   Activate/Deactivate"

        'Activated
        Public Sub Form1_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated


            FormActivated = True
            ChangeHue()
            Try
                Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next
                Playlist.Focus()
                Scroller.Focus()
                Playlist.Focus()
            Catch
            End Try
            Me.Refresh()
            If My.Settings.MiniModeOn Then
                Me.FormBorderEffect = XtraEditors.FormBorderEffect.Shadow
            End If

        End Sub

        'Deactivated
        Public Sub Form1_Deactivate(sender As Object, e As EventArgs) Handles MyBase.Deactivate
            FormActivated = False
            ChangeHue()
        End Sub
        Public Sub Form1_MouseUp(sender As Object, e As MouseEventArgs) Handles MyBase.MouseUp
            ReleaseCapture()
            resizeDir = ResizeDirection.None
          
        End Sub
        Public Sub Window_Titlebar_MouseUp(sender As Object, e As MouseEventArgs) Handles Window_Titlebar.MouseUp
            resizeDir = ResizeDirection.None
        End Sub


#End Region

#Region " App   |   Drag Form   |   Snap to Edges (MyBase_Moved)"




        Public Enum ResizeDirection
            None = 0
            Left = 1
            TopLeft = 2
            Top = 3
            TopRight = 4
            Right = 5
            BottomRight = 6
            Bottom = 7
            BottomLeft = 8
        End Enum

        Public Property resizeDir() As ResizeDirection
            Get
                Return _resizeDir
            End Get
            Set(ByVal value As ResizeDirection)
                _resizeDir = value
                Select Case value
                    Case ResizeDirection.Left
                        Me.Cursor = Cursors.SizeWE
                    Case ResizeDirection.Right
                        Me.Cursor = Cursors.SizeWE
                    Case ResizeDirection.Top
                        Dim ms As New System.IO.MemoryStream(My.Resources.RZVertical1)
                        Dim ptrCur As IntPtr = My.Resources.RZVertical.GetHicon
                        Dim NewNS As New System.Windows.Forms.Cursor(ptrCur)
                        Me.Cursor = NewNS
                    Case ResizeDirection.Bottom
                        Dim ms As New System.IO.MemoryStream(My.Resources.RZVertical1)
                        Dim NewNS As New System.Windows.Forms.Cursor(My.Resources.RZVertical.GetHicon)
                        Me.Cursor = NewNS
                    Case ResizeDirection.BottomLeft
                        Me.Cursor = Cursors.SizeNESW
                    Case ResizeDirection.TopRight
                        Me.Cursor = Cursors.SizeNESW
                    Case ResizeDirection.BottomRight
                        Me.Cursor = Cursors.SizeNWSE
                    Case ResizeDirection.TopLeft
                        Me.Cursor = Cursors.SizeNWSE
                    Case Else
                        Me.Cursor = Cursors.Default
                End Select
            End Set
        End Property


        'Declare the variables
        <DllImportAttribute("user32.dll")> _
        Public Shared Function SendMessage(hWnd As IntPtr, Msg As Integer, wParam As Integer, lParam As Integer) As Integer
        End Function
        <DllImportAttribute("user32.dll")> Public Shared Function ReleaseCapture() As Boolean
        End Function

        Public Sub Window_Titlebar_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) _
            Handles Window_Titlebar.MouseDown, PicBox_Window_AppName.MouseDown, ControlsBGPanel.MouseDown ', MyBase.MouseDown
            Const WM_NCLBUTTONDOWN As Integer = &HA1
            Const HT_CAPTION As Integer = &H2

            If e.Button = MouseButtons.Left And Me.WindowState <> FormWindowState.Maximized Then

                If resizeDir = ResizeDirection.None Then
                    ReleaseCapture()
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0)
                    ' ReleaseCapture()
                End If



            End If
        End Sub

#Region " Snap Window to screen edge    |   MyBase.Move"

        Dim SNAP_SIZE As Integer = 8
        Public Const SnapDist As Integer = 8
        Public Function DoSnap(pos As Integer, edge As Integer) As Boolean
            Dim delta As Integer = pos - edge
            Return delta > 0 AndAlso delta <= SnapDist
        End Function


        Public Sub Form1_Move(sender As Object, e As EventArgs) Handles MyBase.Move
            If AppOpen = True Then Return

            Dim scn As Screen = Screen.FromPoint(Me.Location)
            If DoSnap(Me.Left, scn.WorkingArea.Left) Then
                Me.Left = scn.WorkingArea.Left
            End If
            If DoSnap(Me.Top, scn.WorkingArea.Top) Then
                Me.Top = scn.WorkingArea.Top
            End If
            If DoSnap(scn.WorkingArea.Right, Me.Right) Then
                Me.Left = scn.WorkingArea.Right - Me.Width
            End If
            If DoSnap(scn.WorkingArea.Bottom, Me.Bottom) Then
                Me.Top = scn.WorkingArea.Bottom - Me.Height
            End If
        End Sub

#End Region


#End Region

#Region " App   |   Force Exit"
        Public Sub ForceExitBut_ItemClick(sender As Object, e As XtraBars.ItemClickEventArgs) Handles BarBut_ForceExit.ItemClick
            Dim result As Integer = MyFullMsgBox.Show("This will force the app to close. Settings will NOT be saved!", "Are you sure you want to force exit?", True, MessageBoxButtons.OKCancel)
            If result = DialogResult.OK Then
                Process.GetCurrentProcess.Kill()
            End If
        End Sub
#End Region



#Region " Paint Border"

        Dim DriveModeLoadedOnce As Boolean = False
        Public Sub Form1_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
            If My.Settings.FormBorderUse Then
                ControlPaint.DrawBorder(e.Graphics, Me.ClientRectangle, My.Settings.FormBorder, ButtonBorderStyle.Solid)
            End If
        End Sub

        Public Sub BarBut_BorderColor_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_BorderColor.ItemClick
            Dim xform As New BorderColorFrm
            xform.ShowDialog()
            If xform.DialogResult = DialogResult.OK Then

            End If
        End Sub

#End Region

#End Region


#Region " Mini Mode"
#Region " Declarations"
        Dim StopAnimation As Integer = 350
        Dim StopBackAnimation As Integer = 440
        Dim StopRightAnimation As Integer = 400
        Dim StopBackRightAnimation As Integer = 750

        Dim ResizeGrowImage As System.Drawing.Image = My.Resources.ExtendArrow
        Dim ResizeGrowHoverImage As System.Drawing.Image = My.Resources.ExtendArrow_Hover
        Dim ResizeGrowPressImage As System.Drawing.Image = My.Resources.ExtendArrow_Press
        Dim ResizeShrinkImage As System.Drawing.Image = My.Resources.ExtendArrow_Shrink
        Dim ResizeShrinkHoverImage As System.Drawing.Image = My.Resources.ExtendArrow_Hover
        Dim ResizeShrinkPressImage As System.Drawing.Image = My.Resources.ExtendArrow_Press

        Dim ResizeRightGrowImage As System.Drawing.Image = My.Resources.ExtendArrow
        Dim ResizeRightGrowHoverImage As System.Drawing.Image = My.Resources.ExtendArrowRight_Hover
        Dim ResizeRightGrowPressImage As System.Drawing.Image = My.Resources.ExtendArrowRight_Press
        Dim ResizeRightShrinkImage As System.Drawing.Image = My.Resources.ExtendArrow_Shrink
        Dim ResizeRightShrinkHoverImage As System.Drawing.Image = My.Resources.ExtendArrowRight_Hover
        Dim ResizeRightShrinkPressImage As System.Drawing.Image = My.Resources.ExtendArrowRight_Press

        Dim Form_Outline As New PictureBox
        Dim ControlsList As New Collection
        Dim ResizeBut2 As New PictureBox
        Public Shared MiniModeOffCanceled As Boolean = False
        Dim FormWasOffBottom As Boolean = False
        Dim FormWasOffRight As Boolean = False

#End Region
#Region " Declare Control Sizes/Locations/etc..."

        Dim FormSize As Size
        Dim FormMinSize As Size

        Dim Window_Titlebar_Image As System.Drawing.Image
        Dim Window_Titlebar_Width As Integer
        Dim Window_Titlebar_Location As Point
        Dim Window_Titlebar_BackColor As Color
        Dim Window_Titlebar_Anchor As AnchorStyles
        Dim Window_Titlebar_BackgroundImageLayout As ImageLayout
        Dim Window_Titlebar_Size As Size
        Dim Window_Titlebar_Dock As DockStyle

        Dim But_TitleBar_Close_Size As Size
        Dim But_TitleBar_Close_Location As Point
        Dim But_TitleBarMax_Size As Size
        Dim But_TitleBarMax_Location As Point
        Dim But_TitleBarMin_Size As Size
        Dim But_TitleBarMin_Location As Point



        Dim PicBox_Window_AppName_Visible As Boolean
        Dim PicBox_Window_AppName_Parent As Control
        Dim PicBox_Window_AppName_Size As Size
        Dim PicBox_Window_AppName_Location As Point

        Dim PicBox_AppIcon_Visible As Boolean

        Dim TrackBar_PlayerVol2_Location As Point
        Dim TrackBar_PlayerVol2_Size As Size

        Dim Artwork_VLC_Location As Point
        Dim Artwork_VLC_Size As Size
        Dim Artwork_VLC_MinSize As Size

        Dim LabelConfirmSpeed_Location As Point

        Dim PictureBoxSpec_Dock As DockStyle
        Dim PictureBoxSpec_Anchor As AnchorStyles
        Dim PictureBoxSpec_BackColor As Color
        Dim PictureBoxSpec_Location As Point
        Dim PictureBoxSpec_MinSize As Size
        Dim PictureBoxSpec_MaxSize As Size
        Dim PictureBoxSpec_Size As Size

        Dim But_A_Location As Point
        Dim But_B_Location As Point
        Dim But_AB_Reset_Location As Point

        Dim Panel_ABRepeatControlsBG_Location As Point
        Dim Panel_ABRepeatControlsBG_Visible As Boolean


        Dim But_Shuffle_Location As Point
        Dim But_Repeat_Location As Point


        Dim PlaybackPanel_Location As Point
        Dim PlaybackPanel_Location_Anchor As AnchorStyles
        Dim But_Stop_Location As Point
        Dim But_Previous_Location As Point
        Dim But_PlayPause_Location As Point
        Dim But_Next_Location As Point

        Dim PicBox_PitchText_Location As Point
        Dim PicBox_SpeedText_Location As Point

        Dim trackBar_Speed2_Location As Point
        Dim trackBar_Speed2_Anchor As AnchorStyles
        Dim trackbar_Pitch2_Location As Point
        Dim trackbar_Pitch2_Anchor As AnchorStyles

        Dim But_PitchUp_Location As Point
        Dim But_PitchDown_Location As Point
        Dim But_PitchUp_Anchor As AnchorStyles
        Dim But_PitchDown_Anchor As AnchorStyles


        Dim But_SpeedUp_Location As Point
        Dim But_SpeedDown_Location As Point

        Dim Label_SpeedTextbox_Location As Point
        Dim Label_SpeedTextbox_Anchor As AnchorStyles
        Dim PicBox_SpeedText_Anchor As AnchorStyles
        Dim PicBox_PitchTextBox_Location As Point

        Dim TrackBar_Seek2_MaxSize As Size
        Dim TrackBar_Seek2_MinSize As Size
        Dim TrackBar_Seek2_Size As Size
        Dim TrackBar_Seek2_Location As Point



        Dim timelabel_Location As Point
        Dim totaltimelabel_Location As Point

        Dim Label_SongName_Location As Point
        Dim Label_SongName_Width As Integer
        Dim Label_SongName_TextAlign As ContentAlignment

        Dim Label_Artist_Location As Point
        Dim Label_Artist_Width As Integer
        Dim Label_Artist_TextAlign As ContentAlignment

        Dim Label_Album_Location As Point
        Dim Label_Album_Width As Integer
        Dim Label_Album_TextAlign As ContentAlignment

        Dim VLCChapterMarks_Location As Point
        Dim VLCChapterMarks_Dock As DockStyle
        Dim VLCChapterMarks_Anchor As AnchorStyles
        Dim VLCChapterMarks_Visible As Boolean
        Dim VLCChapterMarks_Size As Size

        Dim PlaylistTabs_Location As Point
        Dim PlaylistTabs_Dock As DockStyle
        Dim PlaylistTabs_Anchor As AnchorStyles
        Dim PlaylistTabs_Visible As Boolean
        Dim PlaylistTabs_Size As Size

        Dim YoutubeTabControl_Location As Point
        Dim YoutubeTabControl_Dock As DockStyle
        Dim YoutubeTabControl_Anchor As AnchorStyles
        Dim YoutubeTabControl_Visible As Boolean
        Dim YoutubeTabControl_Size As Size

        Dim But_Search_Location As Point

        Dim PreviousSize As Size
        Dim PlayPauseSize As Size
        Dim NextSize As Size
        Dim ArtworkSize As Size


#End Region

#Region " Resize Buttons"
#Region " Graphics"

        Dim ResizeButtonHover As Boolean = False
        Public Sub ResizeBut_MouseEnter(sender As Object, e As EventArgs) Handles But_ResizeDown.MouseEnter

            If Me.Height = 440 Then
                But_ResizeDown.BackgroundImage = ResizeShrinkHoverImage
            Else
                But_ResizeDown.BackgroundImage = ResizeGrowHoverImage
            End If
            ResizeButtonHover = True

        End Sub
        Public Sub ResizeBut_MouseLeave(sender As Object, e As EventArgs) Handles But_ResizeDown.MouseLeave

            If Me.Height = 440 Then
                But_ResizeDown.BackgroundImage = ResizeShrinkImage
            Else
                But_ResizeDown.BackgroundImage = ResizeGrowImage
            End If
            ResizeButtonHover = False

        End Sub
        Public Sub ResizeBut_MouseDown(sender As Object, e As MouseEventArgs) Handles But_ResizeDown.MouseDown

            If Me.Height = 440 Then
                But_ResizeDown.BackgroundImage = ResizeShrinkPressImage
            Else
                But_ResizeDown.BackgroundImage = ResizeGrowPressImage
            End If

        End Sub
        Public Sub ResizeBut_MouseUp(sender As Object, e As MouseEventArgs) Handles But_ResizeDown.MouseUp
            If Me.Height = 440 Then
                If ResizeButtonHover = True Then
                    But_ResizeDown.BackgroundImage = ResizeShrinkHoverImage
                Else
                    But_ResizeDown.BackgroundImage = ResizeShrinkImage
                End If
            Else
                If ResizeButtonHover = True Then
                    But_ResizeDown.BackgroundImage = ResizeGrowHoverImage
                Else
                    But_ResizeDown.BackgroundImage = ResizeGrowImage
                End If
            End If


        End Sub


        Dim ResizeBut2Hover As Boolean = False
        Public Sub ResizeBut2_MouseEnter(sender As Object, e As EventArgs)

            If Me.Width = 750 Then
                ResizeBut2.BackgroundImage = ResizeRightShrinkHoverImage
            Else
                ResizeBut2.BackgroundImage = ResizeRightGrowHoverImage
            End If
            ResizeBut2Hover = True

        End Sub
        Public Sub ResizeBut2_MouseLeave(sender As Object, e As EventArgs)

            If Me.Width = 750 Then
                ResizeBut2.BackgroundImage = ResizeRightShrinkImage
            Else
                ResizeBut2.BackgroundImage = ResizeRightGrowImage
            End If
            ResizeBut2Hover = False

        End Sub
        Public Sub ResizeBut2_MouseDown(sender As Object, e As MouseEventArgs)

            If Me.Width = 750 Then
                ResizeBut2.BackgroundImage = ResizeRightShrinkPressImage
            Else
                ResizeBut2.BackgroundImage = ResizeRightGrowPressImage
            End If

        End Sub
        Public Sub ResizeBut2_MouseUp(sender As Object, e As MouseEventArgs)
            If Me.Width = 750 Then
                If ResizeBut2Hover = True Then
                    ResizeBut2.BackgroundImage = ResizeRightShrinkHoverImage
                Else
                    ResizeBut2.BackgroundImage = ResizeRightShrinkImage
                End If
            Else
                If ResizeBut2Hover = True Then
                    ResizeBut2.BackgroundImage = ResizeRightGrowHoverImage
                Else
                    ResizeBut2.BackgroundImage = ResizeRightGrowImage
                End If
            End If


        End Sub



#End Region
#Region " Prepare Resize button images"
        Public Sub Preferences(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            ' Fix Resize Right button images
            ResizeRightGrowImage.RotateFlip(RotateFlipType.Rotate270FlipNone)

            ResizeRightShrinkImage.RotateFlip(RotateFlipType.Rotate270FlipNone)
            ResizeRightShrinkHoverImage.RotateFlip(RotateFlipType.RotateNoneFlipX)
            ResizeRightShrinkPressImage.RotateFlip(RotateFlipType.RotateNoneFlipX)

            ResizeShrinkHoverImage.RotateFlip(RotateFlipType.Rotate180FlipNone)
            ResizeShrinkPressImage.RotateFlip(RotateFlipType.Rotate180FlipNone)
        End Sub
#End Region


        ' Extend Down
        Public Sub ResizeBut_Click(sender As Object, e As EventArgs) Handles But_ResizeDown.Click
            ExtendDown()
        End Sub
        Public Sub ExtendDown()
            If My.Settings.SimplisticMode Then Return
            SuspendDrawing()
            'SuspendLayout()


            Select Case Me.Height

                Case Is < 440
                    'Expand

                    For i As Integer = 0 To 90
                        If Me.Height < StopBackAnimation Then
                            But_ResizeDown.Enabled = False
                            Me.Height += i
                        Else
                            Me.Height = 440
                            But_ResizeDown.Enabled = True
                            But_ResizeDown.BackgroundImage = ResizeShrinkImage
                            If IsOnScreen(Me) = False Then
                                If My.Settings.MiniModeOn = True Then
                                    FormWasOffBottom = True
                                    Me.Top = Me.Top - (440 - 350)
                                    If IsOnScreen(Me) = False Then
                                        Me.Top = 0
                                        FormWasOffBottom = False
                                        Return
                                    End If
                                End If
                            End If
                        End If
                    Next i
                    My.Settings.IsExtendedDown = True
                    My.Settings.FullSize = True


                Case Else
                    'Shrink

                    For i As Integer = 0 To 90
                        If Me.Height > StopAnimation Then
                            But_ResizeDown.Enabled = False
                            Me.Height -= i
                        Else
                            But_ResizeDown.Enabled = True
                            But_ResizeDown.BackgroundImage = ResizeGrowImage
                            If FormWasOffBottom Then
                                Me.Top = Me.Top + (440 - 350)

                                FormWasOffBottom = False
                            End If
                        End If
                    Next i
                    My.Settings.IsExtendedDown = False
                    My.Settings.FullSize = False

            End Select

            My.Settings.Save()
            'ResumeLayout()
            ResumeDrawing()
            ResizeBut2.BringToFront()
        End Sub


        ' Extend Right for Playlist
        Public Sub ResizeBut2_Click(sender As Object, e As EventArgs) ' Handles ResizeBut2.Click

            ExtendRight()
        End Sub
        Public Sub ExtendRight()
            If My.Settings.SimplisticMode Then Return
            SuspendDrawing()
            'SuspendLayout()


            Select Case Me.Width
                Case Is < 750
                    'Expand
                    For i As Integer = 0 To 5000
                        If Me.Width < StopBackRightAnimation Then
                            ResizeBut2.Enabled = False
                            Me.Width += i
                        Else
                            Me.Width = 750
                            ResizeBut2.Enabled = True
                            ResizeBut2.BackgroundImage = ResizeRightShrinkImage
                            If IsOnScreen(Me) = False Then

                                If My.Settings.MiniModeOn = True Then
                                    FormWasOffRight = True
                                    Me.Left = Me.Left - (750 - 400)
                                    If IsOnScreen(Me) = False Then
                                        Me.Left = 0
                                        FormWasOffRight = False
                                        Return
                                    End If
                                End If
                            End If

                        End If
                    Next i

                    My.Settings.IsExpandedRight = True
                Case Else
                    'Shrink
                    For i As Integer = 0 To 50
                        If Me.Width > StopRightAnimation Then
                            ResizeBut2.Enabled = False
                            Me.Width -= i
                        Else
                            ResizeBut2.Enabled = True
                            ResizeBut2.BackgroundImage = ResizeRightGrowImage
                            If FormWasOffRight Then
                                Me.Left = Me.Left + (750 - 400)

                                FormWasOffRight = False
                            End If
                        End If
                    Next i

                    My.Settings.IsExpandedRight = False
            End Select

            My.Settings.Save()
            'ResumeLayout()
            ResumeDrawing()
            ResizeBut2.BringToFront()
            Return
        End Sub


#End Region

        ' Enable/Disable Mini Mode
        Public Sub MiniModeOn()

            Set_Parents()
            PrepareBeforeMiniMode()

            Me.FormBorderEffect = XtraEditors.FormBorderEffect.Shadow
            SuspendDrawing()

            'Change Size
            If My.Settings.MiniModeOn = False Then
                My.Settings.OriginalSize = Me.Size
                My.Settings.OriginalLocation = Me.Location
                My.Settings.MiniModeOn = True
            End If

            MyBase.MinimumSize = New Size(400, 350)
            MyBase.Size = New Size(400, 350)


            Me.Controls.Add(ResizeBut2)
            AddHandler ResizeBut2.Click, AddressOf ResizeBut2_Click
            AddHandler ResizeBut2.MouseUp, AddressOf ResizeBut2_MouseUp
            AddHandler ResizeBut2.MouseDown, AddressOf ResizeBut2_MouseDown
            AddHandler ResizeBut2.MouseEnter, AddressOf ResizeBut2_MouseEnter
            AddHandler ResizeBut2.MouseLeave, AddressOf ResizeBut2_MouseLeave


            Reset_Parents()

            ResizeBut2.Anchor = AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Bottom
            Dim x As Integer = Me.Width - 17
            Dim y As Integer = (Me.Height / 2) '- ResizeBut2.Height '+ Window_Titlebar.Height
            ResizeBut2.Location = New Point(x, y)
            ResizeBut2.Size = New Size(15, 30)
            ResizeBut2.Visible = True
            ResizeBut2.BackgroundImage = ResizeRightGrowImage
            ResizeBut2.BackgroundImageLayout = ImageLayout.Center
            ResizeBut2.BackColor = Color.Transparent

            Me.Controls.Add(Form_Outline)
            Form_Outline.Dock = DockStyle.Fill
            Form_Outline.BackColor = Color.Transparent
            Form_Outline.BackgroundImage = Nothing 'My.Resources.form_outline
            ''   Form_Outline.BackgroundImageLayout = ImageLayout.None

            But_ResizeDown.BackgroundImage = ResizeGrowImage
            But_ResizeDown.Visible = True
            Me.But_ResizeDown.Location = New System.Drawing.Point(185, 334)

            Label_Update.Left = PicBox_Window_AppName.Left - 3
            Label_Update.Top = PicBox_Window_AppName.Top + 20

            ChangeHue()

            ControlsBGPanel.Visible = False
            Splitter.Visible = False
            PictureBoxWF.Visible = False

            TrackBar_PlayerVol2.Location = New Point(14, 310) '322
            TrackBar_PlayerVol2.Size = New Size(84, 50) '45

            'Artwork / VLC
            Me.Artwork.Location = New System.Drawing.Point(12, 78)
            Me.Artwork.MinimumSize = New System.Drawing.Size(143, 143)
            Me.Artwork.Size = New System.Drawing.Size(143, 143)
            If VLC_installed Then
                VlcPlayer.Location = New System.Drawing.Point(12, 78)
                VlcPlayer.MinimumSize = New System.Drawing.Size(143, 143)
                VlcPlayer.Size = New System.Drawing.Size(143, 143)
            Else
                ' Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                ' Artwork.BackgroundImage = DrawText("Videos unsupported. Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)
            End If

            Artwork.BringToFront()

            Me.LabelConfirmSpeed.Location = New System.Drawing.Point(26, 101)

            'Spectrum
            PictureBoxSpec.Dock = DockStyle.None
            PictureBoxSpec.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            PictureBoxSpec.BackColor = Color.Transparent
            PictureBoxSpec.Location = New Point(169, 179)
            PictureBoxSpec.MinimumSize = New Size(0, 0)
            PictureBoxSpec.MaximumSize = New Size(0, 0)
            PictureBoxSpec.Size = New System.Drawing.Size(219, 42)
            PictureBoxSpec.Location = New Point(169, 179)


            'A/B Repeat
            Me.But_A.Location = New System.Drawing.Point(8, 295)
            Me.But_AB_Reset.Location = New System.Drawing.Point(38, 295)
            Me.But_B.Location = New System.Drawing.Point(70, 295)


            'Shuffle / Repeat
            Me.But_Shuffle.Location = New System.Drawing.Point(362, 295)
            Me.But_Repeat.Location = New System.Drawing.Point(330, 295)


            'Playback
            Me.But_Stop.Location = New System.Drawing.Point(112, 297)
            Me.But_Previous.Location = New System.Drawing.Point(142, 292)
            Me.But_PlayPause.Location = New System.Drawing.Point(180, 288)
            Me.But_Next.Location = New System.Drawing.Point(226, 292)


            'Speed & Pitch
            Me.PicBox_PitchText.Location = New System.Drawing.Point(270, 360)
            Me.PicBox_SpeedText.Location = New System.Drawing.Point(97, 360)

            Me.trackBar_Speed2.Location = New System.Drawing.Point(80, 373)
            Me.trackbar_Pitch2.Location = New System.Drawing.Point(248, 373)

            Me.But_PitchUp.Location = New System.Drawing.Point(327, 372)
            Me.But_PitchDown.Location = New System.Drawing.Point(226, 372)

            Me.But_SpeedUp.Location = New System.Drawing.Point(160, 372)
            Me.But_SpeedDown.Location = New System.Drawing.Point(57, 372)

            Me.Label_SpeedTextbox.Location = New System.Drawing.Point(98, 397)
            Me.PicBox_PitchTextBox.Location = New System.Drawing.Point(269, 397)


            'Seek

            TrackBar_Seek2.MaximumSize = New Size(0, 0)
            TrackBar_Seek2.MinimumSize = New Size(0, 0)
            Me.TrackBar_Seek2.Location = New System.Drawing.Point(12, 227)
            Me.TrackBar_Seek2.Size = New System.Drawing.Size(379, 23)


            Me.timelabel.Location = New System.Drawing.Point(9, 254)
            Me.totaltimelabel.Location = New System.Drawing.Point(308, 254)


            'Labels
            Me.Label_SongName.Location = New System.Drawing.Point(165, 103)
            Label_SongName.Width = 222
            Me.Label_SongName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft


            Me.Label_Artist.Location = New System.Drawing.Point(166, 136)
            Label_Artist.Width = 222
            Me.Label_Artist.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

            Me.Label_Album.Location = New System.Drawing.Point(166, 155)
            Label_Album.Width = 222
            Me.Label_Album.TextAlign = System.Drawing.ContentAlignment.MiddleLeft


            Form_Outline.SendToBack()
            ResizeBut2.BringToFront()
            But_ResizeDown.BringToFront()

            ResumeDrawing()

            If My.Settings.MiniModeOn = True Then
                If My.Settings.IsExpandedRight Then
                    ExtendRight()
                End If
                If My.Settings.IsExtendedDown Then
                    ExtendDown()
                End If
            End If
            My.Settings.MiniModeOn = True

            Return
        End Sub
        Public Sub MiniModeOff()
            Set_Parents()
            ResetFromMiniMode()
            If My.Settings.MiniModeOn Then
                My.Settings.FormSize = My.Settings.OriginalSize
                My.Settings.PlayerLocation = My.Settings.OriginalLocation
            Else
                Me.Size = My.Settings.FormSize
            End If

            My.Settings.MiniModeOn = False
            BarCheckBox_MiniMode.Checked = False
            Label_Update.Location = New Point(168, 20)



            Set_Parents()
            Me.Controls.Remove(ResizeBut2)
            Me.Controls.Remove(Form_Outline)


            NeedResizeRefresh = True
            RefreshApp()
            Me.Refresh()
            Splitter.BringToFront()
            Dim SplitPos As Integer = Splitter.SplitterPosition
            Splitter.SplitterPosition = SplitPos + 1
            Splitter.SplitterPosition = SplitPos
          
        End Sub

        Public Sub PrepareBeforeMiniMode()

            FormMinSize = MyBase.MinimumSize
            FormSize = MyBase.Size

            Window_Titlebar_Image = Window_Titlebar.BackgroundImage
            Window_Titlebar_Width = Window_Titlebar.Width
            Window_Titlebar_Location = Window_Titlebar.Location

            TrackBar_PlayerVol2_Location = TrackBar_PlayerVol2.Location
            TrackBar_PlayerVol2_Size = TrackBar_PlayerVol2.Size

            'Artwork / VLC
            Artwork_VLC_Location = Me.Artwork.Location
            Artwork_VLC_MinSize = Me.Artwork.MinimumSize
            Artwork_VLC_Size = Me.Artwork.Size


            LabelConfirmSpeed_Location = LabelConfirmSpeed.Location

            'Spectrum
            PictureBoxSpec_Dock = PictureBoxSpec.Dock
            PictureBoxSpec_Anchor = PictureBoxSpec.Anchor
            PictureBoxSpec_BackColor = PictureBoxSpec.BackColor
            PictureBoxSpec_Location = PictureBoxSpec.Location
            PictureBoxSpec_MinSize = PictureBoxSpec.MinimumSize
            PictureBoxSpec_MaxSize = PictureBoxSpec.MaximumSize
            PictureBoxSpec_Size = PictureBoxSpec.Size


            'A/B Repeat
            But_A_Location = But_A.Location
            But_AB_Reset_Location = But_AB_Reset.Location
            But_B_Location = But_B.Location


            'Shuffle / Repeat
            But_Shuffle_Location = But_Shuffle.Location
            But_Repeat_Location = But_Repeat.Location


            'Playback
            PlaybackPanel_Location_Anchor = PlaybackPanel.Anchor
            PlaybackPanel_Location = PlaybackPanel.Location
            But_Stop_Location = But_Stop.Location
            But_Previous_Location = But_Previous.Location
            But_PlayPause_Location = But_PlayPause.Location
            But_Next_Location = But_Next.Location


            'Speed & Pitch
            PicBox_PitchText_Location = PicBox_PitchText.Location
            PicBox_SpeedText_Location = PicBox_SpeedText.Location

            trackBar_Speed2_Location = trackBar_Speed2.Location
            trackbar_Pitch2_Location = trackbar_Pitch2.Location

            But_PitchUp_Anchor = But_PitchUp.Anchor
            But_PitchUp_Location = But_PitchUp.Location
            But_PitchDown_Location = But_PitchDown.Location
            But_PitchDown_Anchor = But_PitchDown.Anchor

            But_SpeedUp_Location = But_SpeedUp.Location
            But_SpeedDown_Location = But_SpeedDown.Location

            Label_SpeedTextbox_Location = Label_SpeedTextbox.Location
            PicBox_PitchTextBox_Location = PicBox_PitchTextBox.Location


            'Seek
            TrackBar_Seek2_MaxSize = TrackBar_Seek2.MaximumSize
            TrackBar_Seek2_MinSize = TrackBar_Seek2.MinimumSize
            TrackBar_Seek2_Location = TrackBar_Seek2.Location
            TrackBar_Seek2_Size = TrackBar_Seek2.Size


            timelabel_Location = timelabel.Location
            totaltimelabel_Location = totaltimelabel.Location


            'Labels
            Label_SongName_Location = Label_SongName.Location
            Label_SongName_Width = Label_SongName.Width
            Label_SongName_TextAlign = Label_SongName.TextAlign

            Label_Artist_Location = Label_Artist.Location
            Label_Artist_Width = Label_Artist.Width
            Label_Artist_TextAlign = Label_Artist.TextAlign

            Label_Album_Location = Label_Album.Location
            Label_Album_Width = Label_Album.Width
            Label_Album_TextAlign = Label_Album.TextAlign



            Window_Titlebar.SendToBack()
            ' TitleBarImage = My.Resources.TitleBar_Gradient
            AppNameActiveImage = My.Resources.AppName_Colored_Active
            AppNameImage = My.Resources.AppName_Active_Colored_Deactive
          
            But_TitleBar_Close.BackgroundImage = TitleBarCloseImage
            But_TitleBarMax.BackgroundImage = TitleBarMaxImage
            But_TitleBarMin.BackgroundImage = TitleBarMinImage
            FormOutlineImage = Nothing

            PicBox_Window_AppName_Visible = PicBox_Window_AppName.Visible
            PicBox_Window_AppName_Parent = PicBox_Window_AppName.Parent
            PicBox_Window_AppName_Size = PicBox_Window_AppName.Size
            PicBox_Window_AppName_Location = PicBox_Window_AppName.Location



            But_TitleBar_Close.Visible = True
            But_TitleBarMax.Visible = True
            But_TitleBarMin.Visible = True

            Window_Titlebar.Visible = True
            Window_Titlebar_BackColor = Window_Titlebar.BackColor
            Window_Titlebar_Anchor = Window_Titlebar.Anchor

            Window_Titlebar_BackgroundImageLayout = Window_Titlebar.BackgroundImageLayout
            Window_Titlebar_Size = Window_Titlebar.Size
            Window_Titlebar_Location = Window_Titlebar.Location
            Window_Titlebar_Dock = Window_Titlebar.Dock
           
            But_TitleBar_Close_Size = But_TitleBar_Close.Size
            But_TitleBar_Close_Location = But_TitleBar_Close.Location
            But_TitleBarMax_Size = But_TitleBarMax.Size
            But_TitleBarMax_Location = But_TitleBarMax.Location
            But_TitleBarMin_Size = But_TitleBarMin.Size
            But_TitleBarMin_Location = But_TitleBarMin.Location


            'AB Repeat, Repeat, Shuffle
            Panel_ABRepeatControlsBG_Location = Panel_ABRepeatControlsBG.Location
            Panel_ABRepeatControlsBG_Visible = Panel_ABRepeatControlsBG.Visible


            'Speed Trackbar
            trackBar_Speed2_Anchor = trackBar_Speed2.Anchor


            'Speed Textbox
            Label_SpeedTextbox_Anchor = Label_SpeedTextbox.Anchor
            Label_SpeedTextbox_Location = Label_SpeedTextbox.Location
            PicBox_SpeedText_Anchor = PicBox_SpeedText.Anchor
            PicBox_SpeedText_Location = PicBox_SpeedText.Location


            VLCChapterMarks_Dock = VLCChapterMarks.Dock
            VLCChapterMarks_Anchor = VLCChapterMarks.Anchor
            VLCChapterMarks_Visible = VLCChapterMarks.Visible
            VLCChapterMarks_Size = VLCChapterMarks.Size
            VLCChapterMarks_Location = VLCChapterMarks.Location


            PlaylistTabs_Dock = PlaylistTabs.Dock
            PlaylistTabs_Anchor = PlaylistTabs.Anchor
            PlaylistTabs_Visible = PlaylistTabs.Visible
            PlaylistTabs_Size = PlaylistTabs.Size
            PlaylistTabs_Location = PlaylistTabs.Location


            But_Search_Location = But_Search.Location

            PreviousSize = But_Previous.Size
            PlayPauseSize = But_PlayPause.Size
            NextSize = But_Next.Size
            ArtworkSize = Artwork.Size


        End Sub

        Public Sub ResetFromMiniMode()
            For Each c As Control In Me.Controls
                If c.GetType Is GetType(DevExpress.XtraEditors.TrackBarControl) Then
                    c.Visible = False
                End If
            Next
            For Each c As Control In ControlsBGPanel.Controls
                If c.GetType Is GetType(DevExpress.XtraEditors.TrackBarControl) Then
                    c.Visible = False
                End If
            Next



            But_TitleBarMax.Enabled = True

            ChangeHue()
            PictureBoxWF.Visible = False
            ControlsBGPanel.Visible = True
            Splitter.Visible = True
            But_TitleBar_Close.Visible = True
            But_TitleBarMax.Visible = True
            But_TitleBarMin.Visible = True

            Window_Titlebar.Visible = True

            MyBase.MinimumSize = FormMinSize
            MyBase.Size = FormSize
            Window_Titlebar.BackgroundImage = Window_Titlebar_Image
            Window_Titlebar.Width = Window_Titlebar_Width
            Window_Titlebar.Location = Window_Titlebar_Location

            TrackBar_PlayerVol2.Location = TrackBar_PlayerVol2_Location
            TrackBar_PlayerVol2.Size = TrackBar_PlayerVol2_Size

            'Artwork / VLC
            Me.Artwork.Location = Artwork_VLC_Location
            Me.Artwork.MinimumSize = Artwork_VLC_MinSize
            Me.Artwork.Size = Artwork_VLC_Size
            If VLC_installed Then
                Me.VlcPlayer.Location = Artwork_VLC_Location
                Me.VlcPlayer.MinimumSize = Artwork_VLC_MinSize
                Me.VlcPlayer.Size = Artwork_VLC_Size
            End If



            LabelConfirmSpeed.Location = LabelConfirmSpeed_Location

            'Spectrum
            PictureBoxSpec.Dock = PictureBoxSpec_Dock
            PictureBoxSpec.Anchor = PictureBoxSpec_Anchor
            PictureBoxSpec.BackColor = PictureBoxSpec_BackColor
            PictureBoxSpec.Location = PictureBoxSpec_Location
            PictureBoxSpec.MinimumSize = PictureBoxSpec_MinSize
            PictureBoxSpec.MaximumSize = PictureBoxSpec_MaxSize
            PictureBoxSpec.Size = PictureBoxSpec_Size

            'AB Repeat, Repeat, Shuffle
            Panel_ABRepeatControlsBG.Parent = ControlsBGPanel
            Panel_ABRepeatControlsBG.Location = Panel_ABRepeatControlsBG_Location
            Panel_ABRepeatControlsBG.Visible = Panel_ABRepeatControlsBG_Visible

            PlaybackPanel.Parent = ControlsBGPanel
            PlaybackPanel.Anchor = AnchorStyles.Top Or AnchorStyles.Left 'PlaybackPanel_Location_Anchor
            PlaybackPanel.Location = PlaybackPanel_Location
            PlaybackPanel.Location = New Point((ControlsBGPanel.Width / 2) - (But_Stop.Width / 2), PlaybackPanel.Location.Y) ' PlaybackPanel_Location
            PlaybackPanel.Visible = True


            'A/B Repeat
            But_A.Parent = Panel_ABRepeatControlsBG
            But_AB_Reset.Parent = Panel_ABRepeatControlsBG
            But_B.Parent = Panel_ABRepeatControlsBG
            But_A.Location = But_A_Location
            But_AB_Reset.Location = But_AB_Reset_Location
            But_B.Location = But_B_Location


            'Shuffle / Repeat
            But_Shuffle.Parent = Panel_ABRepeatControlsBG
            But_Repeat.Parent = Panel_ABRepeatControlsBG
            But_Shuffle.Location = But_Shuffle_Location
            But_Repeat.Location = But_Repeat_Location

            But_Stop.Parent = PlaybackPanel
            But_Previous.Parent = PlaybackPanel
            But_PlayPause.Parent = PlaybackPanel
            But_Next.Parent = PlaybackPanel

            'Playback
            But_Stop.Location = New Point(0, 14) ' But_Stop_Location
            But_Previous.Location = New Point(30, 9) 'But_Previous_Location
            But_PlayPause.Location = New Point(68, 5) ' But_PlayPause_Location
            But_Next.Location = New Point(114, 9) ' But_Next_Location


            'Speed & Pitch
            PicBox_PitchText.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right '
            PicBox_PitchTextBox.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right '
            PicBox_SpeedText.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right '
            PicBox_PitchText.Location = PicBox_PitchText_Location
            PicBox_SpeedText.Location = PicBox_SpeedText_Location


            trackbar_Pitch2.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right '
            trackBar_Speed2.Anchor = (AnchorStyles.Bottom Or AnchorStyles.Right) '
            trackBar_Speed2.Location = trackBar_Speed2_Location
            trackbar_Pitch2.Location = trackbar_Pitch2_Location

            But_PitchUp.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right 'But_PitchUp_Anchor
            But_PitchDown.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right ' But_PitchDown_Anchor
            But_PitchUp.Parent = ControlsBGPanel
            But_PitchDown.Parent = ControlsBGPanel

            But_PitchUp.Location = But_PitchUp_Location
            But_PitchDown.Location = But_PitchDown_Location


            But_SpeedUp.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right '
            But_SpeedDown.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right '
            But_SpeedUp.Location = But_SpeedUp_Location
            But_SpeedDown.Location = But_SpeedDown_Location


            Label_SpeedTextbox.Location = Label_SpeedTextbox_Location
            PicBox_PitchTextBox.Location = PicBox_PitchTextBox_Location


            'Seek
            TrackBar_Seek2.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
            TrackBar_Seek2.MaximumSize = TrackBar_Seek2_MaxSize
            TrackBar_Seek2.MinimumSize = TrackBar_Seek2_MinSize
            TrackBar_Seek2.Location = TrackBar_Seek2_Location
            TrackBar_Seek2.Size = TrackBar_Seek2_Size


            timelabel.Location = timelabel_Location
            totaltimelabel.Anchor = AnchorStyles.Top Or AnchorStyles.Right
            totaltimelabel.Location = totaltimelabel_Location


            'Labels
            Label_SongName.Location = Label_SongName_Location
            Label_SongName.Width = Label_SongName_Width
            Try
                Label_SongName.TextAlign = Label_SongName_TextAlign
            Catch ex As Exception

            End Try


            Label_Artist.Location = Label_Artist_Location
            Label_Artist.Width = Label_Artist_Width
            Try
                Label_Artist.TextAlign = Label_Artist_TextAlign
            Catch ex As Exception

            End Try


            Label_Album.Location = Label_Album_Location
            Label_Album.Width = Label_Album_Width
            Try

                Label_Album.TextAlign = Label_Album_TextAlign

            Catch ex As Exception

            End Try


            Window_Titlebar.SendToBack()

            FormOutlineImage = Nothing
            ' Form_Outline.Parent = Window_Titlebar


            PicBox_Window_AppName.Visible = PicBox_Window_AppName_Visible
            PicBox_Window_AppName.Parent = PicBox_Window_AppName_Parent
            PicBox_Window_AppName.Size = PicBox_Window_AppName_Size
            PicBox_Window_AppName.Location = PicBox_Window_AppName_Location



            Window_Titlebar.BackColor = Window_Titlebar_BackColor
            Window_Titlebar.Anchor = Window_Titlebar_Anchor

            Window_Titlebar.BackgroundImageLayout = Window_Titlebar_BackgroundImageLayout
            Window_Titlebar.Size = Window_Titlebar_Size
            Window_Titlebar.Location = Window_Titlebar_Location
            Window_Titlebar.Dock = Window_Titlebar_Dock
            '  Window_Titlebar.Dock = DockStyle.Fill

            ' Dim TitleBarButtons As New Panel
            ' Window_Titlebar.Controls.Add(TitleBarButtons)
            ' TitleBarButtons.Location = New Point(242, 12)
            ' TitleBarButtons.Visible = True
            ' TitleBarButtons.Size = New Size(147, 33)
            ' TitleBarButtons.Controls.AddRange({But_TitleBar_Close, But_TitleBarMax, But_TitleBarMin})
            ' TitleBarButtons.BringToFront()
            But_TitleBar_Close.Size = But_TitleBar_Close_Size
            But_TitleBar_Close.Location = But_TitleBar_Close_Location
            But_TitleBarMax.Size = But_TitleBarMax_Size
            But_TitleBarMax.Location = But_TitleBarMax_Location
            But_TitleBarMin.Size = But_TitleBarMin_Size
            But_TitleBarMin.Location = But_TitleBarMin_Location





            'Speed Trackbar
            trackBar_Speed2.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right ' trackBar_Speed2_Anchor


            'Speed Textbox
            Label_SpeedTextbox.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right ' Label_SpeedTextbox_Anchor
            Label_SpeedTextbox.Location = Label_SpeedTextbox_Location
            PicBox_SpeedText.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right ' PicBox_SpeedText_Anchor
            PicBox_SpeedText.Location = PicBox_SpeedText_Location


            VLCChapterMarks.Dock = VLCChapterMarks_Dock
            VLCChapterMarks.Anchor = VLCChapterMarks_Anchor
            VLCChapterMarks.Visible = VLCChapterMarks_Visible
            VLCChapterMarks.Size = VLCChapterMarks_Size
            VLCChapterMarks.Location = VLCChapterMarks_Location

            Me.Controls.Add(PlaylistTabs)
            Me.PlaylistTabs.Location = Me.PointToClient(Me.PlaylistTabs.Parent.PointToScreen(Me.PlaylistTabs.Location))
            PlaylistTabs.Parent = Me
            PlaylistTabs.Dock = PlaylistTabs_Dock
            PlaylistTabs.Anchor = PlaylistTabs_Anchor
            PlaylistTabs.Visible = PlaylistTabs_Visible
            PlaylistTabs.Size = PlaylistTabs_Size
            PlaylistTabs.Location = PlaylistTabs_Location


            But_Search.Location = But_Search_Location

            PlaylistTabs.BringToFront()
            PlaybackPanel.BringToFront()

        End Sub



        ' Initiate
        Public Shared WaitMiniMode As Boolean = False
        Public Sub MiniModeCheckbox_CheckedChanged() Handles BarCheckBox_MiniMode.CheckedChanged
            'Public Sub MiniModeCheckbox_CheckedChanged(sender As Object, e As ItemClickEventArgs) Handles BarCheckBox_MiniMode.CheckedChanged
            If WaitMiniMode Then Return
            MiniModeCheckChanged()
        End Sub
        Public Sub MiniModeCheckChanged()
            If WaitMiniMode Then
                Select Case MyMsgBox.Show("This will toggle mini mode between on/off. App will restart. Continue?", MsgBoxStyle.YesNo, "Switch Mini Mode?")
                    Case MsgBoxResult.Yes
                        WaitMiniMode = False
                        '  BarCheckBox_MiniMode.Checked = Not BarCheckBox_MiniMode.Checked
                        My.Settings.MiniModeOn = Not My.Settings.MiniModeOn
                        My.Settings.Save()

                        System.Diagnostics.Process.Start(Application.ExecutablePath)
                        Application.ExitThread()
                        Application.Exit()
                        Me.Close()

                        Return
                    Case MsgBoxResult.No
                        Return
                End Select
            End If
            Select Case BarCheckBox_MiniMode.Checked
                Case True
                    If MiniModeOffCanceled = False Then
                        MiniModeOn()
                    End If

                    '   My.Settings.MiniModeOn = True
                    MiniModeOffCanceled = False
                Case False
                    MiniModeOff()
            End Select

        End Sub
        'Setup Mini Mode Layout
        Public Sub Reset_Parents() ' LOCATIONS and PARENTS
            For Each c As Control In Me.Controls
                c.Visible = False
            Next
            Window_Titlebar.SendToBack()
            ' TitleBarImage = My.Resources.TitleBar_Gradient
            AppNameActiveImage = My.Resources.AppName_Active
            AppNameImage = My.Resources.AppNameDeactive
            TitleBarCloseImage = My.Resources.TitleBar_Close
            TitleBarCloseHoverImage = My.Resources.TitleBar_Close_Hover
            TitleBarClosePressImage = My.Resources.TitleBar_Close_Press
            TitleBarMaxImage = My.Resources.TitleBar_Max
            TitleBarMaxHoverImage = My.Resources.TitleBar_Max_Hover
            TitleBarMaxPressImage = My.Resources.TitleBar_Max_Press
            TitleBarMinImage = My.Resources.TitleBar_Min
            TitleBarMinHoverImage = My.Resources.TitleBar_Min_Hover
            TitleBarMinPressImage = My.Resources.TitleBar_Min_Press
            But_TitleBar_Close.BackgroundImage = TitleBarCloseImage
            But_TitleBarMax.BackgroundImage = TitleBarMaxImage
            But_TitleBarMin.BackgroundImage = TitleBarMinImage
            FormOutlineImage = Nothing

            Form_Outline.Parent = Window_Titlebar

            PicBox_Window_AppName.Visible = True
            PicBox_Window_AppName.Parent = Window_Titlebar
            PicBox_Window_AppName.Size = New Size(92, 20)
            PicBox_Window_AppName.Location = New Point(64, 20)

            But_TitleBar_Close.Visible = True
            But_TitleBarMax.Visible = True
            But_TitleBarMin.Visible = True

            Window_Titlebar.Visible = True
            Window_Titlebar.BackColor = Color.Transparent
            Window_Titlebar.Anchor = AnchorStyles.Top Or AnchorStyles.Left

            ' Window_Titlebar.BackgroundImage = My.Resources.TitleBar_Gradient
            Window_Titlebar.BackgroundImageLayout = ImageLayout.Stretch
            Window_Titlebar.Size = New Size(750, 440)
            Window_Titlebar.Location = New Point(0, 0)
            '  Window_Titlebar.Dock = DockStyle.Fill

            Dim TitleBarButtons As New Panel
            Window_Titlebar.Controls.Add(TitleBarButtons)
            TitleBarButtons.Location = New Point(242, 12)
            TitleBarButtons.Visible = True
            TitleBarButtons.Size = New Size(147, 33)
            TitleBarButtons.Controls.AddRange({But_TitleBar_Close, But_TitleBarMax, But_TitleBarMin})
            TitleBarButtons.BringToFront()
            But_TitleBar_Close.Size = New Size(45, 33)
            But_TitleBar_Close.Location = New Point(102, 0)
            But_TitleBarMax.Size = New Size(45, 33)
            But_TitleBarMax.Location = New Point(51, 0)
            But_TitleBarMin.Size = New Size(45, 33)
            But_TitleBarMin.Location = New Point(0, 0)




            Me.But_SettingsPic.Parent = Window_Titlebar
            But_SettingsPic.Location = New Point(13, 14)
            ' Me.SettingsPic.Location = Me.PointToClient(Me.SettingsPic.Parent.PointToScreen(Me.SettingsPic.Location))
            But_SettingsPic.Visible = True
            But_SettingsPic.BringToFront()


            Me.But_ResizeDown.Location = Window_Titlebar.PointToClient(Me.But_ResizeDown.Parent.PointToScreen(Me.But_ResizeDown.Location))
            Me.But_ResizeDown.Parent = Window_Titlebar
            But_ResizeDown.Visible = True
            But_ResizeDown.BringToFront()

            Me.ResizeBut2.Parent = Me
            '  Me.ResizeBut2.Location = Window_Titlebar.PointToClient(Me.ResizeBut2.Parent.PointToScreen(Me.ResizeBut2.Location))
            Me.ResizeBut2.Parent = Me
            ResizeBut2.Visible = True
            ResizeBut2.BringToFront()


            'VLC
            If VLC_installed Then
                VlcPlayer.Location = Window_Titlebar.PointToClient(Me.VlcPlayer.Parent.PointToScreen(Me.VlcPlayer.Location))
                VlcPlayer.Parent = Window_Titlebar
                VlcPlayer.BringToFront()
                VlcPlayer.Anchor = AnchorStyles.Top Or AnchorStyles.Left
                VlcPlayer.Visible = True
            End If



            'Artwork
            Me.Artwork.Location = Window_Titlebar.PointToClient(Me.Artwork.Parent.PointToScreen(Me.Artwork.Location))
            Me.Artwork.Parent = Window_Titlebar
            Artwork.BringToFront()
            Artwork.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            Artwork.Visible = True



            Me.TrackBar_PlayerVol2.Location = Window_Titlebar.PointToClient(Me.TrackBar_PlayerVol2.Parent.PointToScreen(Me.TrackBar_PlayerVol2.Location))
            Me.TrackBar_PlayerVol2.Parent = Window_Titlebar
            TrackBar_PlayerVol2.BringToFront()
            TrackBar_PlayerVol2.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            If My.Settings.EnablePlayerVolumeCheckState = True Then
                TrackBar_PlayerVol2.Visible = True

            End If


            'Metadata Labels
            Me.Label_SongName.Location = Window_Titlebar.PointToClient(Me.Label_SongName.Parent.PointToScreen(Me.Label_SongName.Location))
            Me.Label_SongName.Parent = Window_Titlebar
            Label_SongName.BringToFront()
            Label_SongName.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            Label_SongName.Visible = True

            Me.Label_Artist.Location = Window_Titlebar.PointToClient(Me.Label_Artist.Parent.PointToScreen(Me.Label_Artist.Location))
            Me.Label_Artist.Parent = Window_Titlebar
            Label_Artist.BringToFront()
            Label_Artist.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            Label_Artist.Visible = True

            Me.Label_Album.Location = Window_Titlebar.PointToClient(Me.Label_Album.Parent.PointToScreen(Me.Label_Album.Location))
            Me.Label_Album.Parent = Window_Titlebar
            Label_Album.BringToFront()
            Label_Album.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            Label_Album.Visible = True

            'Spectrum
            PictureBoxSpec.Parent = Window_Titlebar
            PictureBoxSpec.Location = Window_Titlebar.PointToClient(PictureBoxSpec.Parent.PointToScreen(PictureBoxSpec.Location))
            PictureBoxSpec.Parent = Window_Titlebar
            PictureBoxSpec.BringToFront()
            PictureBoxSpec.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            PictureBoxSpec.Visible = True


            Me.TrackBar_Seek2.Location = Window_Titlebar.PointToClient(Me.TrackBar_Seek2.Parent.PointToScreen(Me.TrackBar_Seek2.Location))
            Me.TrackBar_Seek2.Parent = Window_Titlebar
            TrackBar_Seek2.BringToFront()
            TrackBar_Seek2.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            TrackBar_Seek2.Visible = True

            'Time Labels
            Me.timelabel.Location = Window_Titlebar.PointToClient(Me.timelabel.Parent.PointToScreen(Me.timelabel.Location))
            Me.timelabel.Parent = Window_Titlebar
            timelabel.BringToFront()
            timelabel.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            timelabel.Visible = True

            Me.totaltimelabel.Location = Window_Titlebar.PointToClient(Me.totaltimelabel.Parent.PointToScreen(Me.totaltimelabel.Location))
            Me.totaltimelabel.Parent = Window_Titlebar
            totaltimelabel.BringToFront()
            totaltimelabel.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            totaltimelabel.Visible = True

            'Seek Bar

            Me.TrackBar_Seek2.Location = Window_Titlebar.PointToClient(Me.TrackBar_Seek2.Parent.PointToScreen(Me.TrackBar_Seek2.Location))
            Me.TrackBar_Seek2.Parent = Window_Titlebar
            TrackBar_Seek2.Visible = True


            'Pause/Play
            Me.PlaybackPanel.Location = Window_Titlebar.PointToClient(Me.PlaybackPanel.Parent.PointToScreen(Me.PlaybackPanel.Location))
            Me.PlaybackPanel.Parent = Window_Titlebar
            PlaybackPanel.BringToFront()
            PlaybackPanel.Visible = False

            But_PlayPause.Location = Window_Titlebar.PointToClient(Me.PlaybackPanel.Parent.PointToScreen(Me.PlaybackPanel.Location))
            But_PlayPause.Parent = Window_Titlebar
            But_PlayPause.BringToFront()
            But_PlayPause.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            But_PlayPause.Visible = True

            But_Next.Location = Window_Titlebar.PointToClient(Me.PlaybackPanel.Parent.PointToScreen(Me.PlaybackPanel.Location))
            But_Next.Parent = Window_Titlebar
            But_Next.BringToFront()
            But_Next.Visible = True

            But_Previous.Location = Window_Titlebar.PointToClient(Me.PlaybackPanel.Parent.PointToScreen(Me.PlaybackPanel.Location))
            But_Previous.Parent = Window_Titlebar
            But_Previous.BringToFront()
            But_Previous.Visible = True

            But_Stop.Location = Window_Titlebar.PointToClient(Me.PlaybackPanel.Parent.PointToScreen(Me.PlaybackPanel.Location))
            But_Stop.Parent = Window_Titlebar
            But_Stop.BringToFront()
            But_Stop.Visible = True


            'AB Repeat, Repeat, Shuffle
            Panel_ABRepeatControlsBG.Location = Window_Titlebar.PointToClient(Panel_ABRepeatControlsBG.Parent.PointToScreen(Panel_ABRepeatControlsBG.Location))
            Panel_ABRepeatControlsBG.Parent = Window_Titlebar
            Panel_ABRepeatControlsBG.BringToFront()
            Panel_ABRepeatControlsBG.Visible = False

            But_A.Location = Window_Titlebar.PointToClient(Me.PlaybackPanel.Parent.PointToScreen(Me.PlaybackPanel.Location))
            But_A.Parent = Window_Titlebar
            But_A.BringToFront()
            But_A.Visible = True

            But_AB_Reset.Location = Window_Titlebar.PointToClient(Me.PlaybackPanel.Parent.PointToScreen(Me.PlaybackPanel.Location))
            But_AB_Reset.Parent = Window_Titlebar
            But_AB_Reset.BringToFront()
            But_AB_Reset.Visible = True

            But_B.Location = Window_Titlebar.PointToClient(Me.PlaybackPanel.Parent.PointToScreen(Me.PlaybackPanel.Location))
            But_B.Parent = Window_Titlebar
            But_B.BringToFront()
            But_B.Visible = True


            But_Shuffle.Location = Window_Titlebar.PointToClient(Me.PlaybackPanel.Parent.PointToScreen(Me.PlaybackPanel.Location))
            But_Shuffle.Parent = Window_Titlebar
            But_Shuffle.BringToFront()
            But_Shuffle.Visible = True

            But_Repeat.Location = Window_Titlebar.PointToClient(Me.PlaybackPanel.Parent.PointToScreen(Me.PlaybackPanel.Location))
            But_Repeat.Parent = Window_Titlebar
            But_Repeat.BringToFront()
            But_Repeat.Visible = True


            'Pitch Trackbar
            trackbar_Pitch2.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            Me.trackbar_Pitch2.Location = Window_Titlebar.PointToClient(Me.trackbar_Pitch2.Parent.PointToScreen(Me.trackbar_Pitch2.Location))
            Me.trackbar_Pitch2.Parent = Window_Titlebar
            trackbar_Pitch2.BringToFront()
            trackbar_Pitch2.Visible = True

            'Pitch Control
            But_PitchDown.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            Me.But_PitchDown.Location = Window_Titlebar.PointToClient(Me.But_PitchDown.Parent.PointToScreen(Me.But_PitchDown.Location))
            Me.But_PitchDown.Parent = Window_Titlebar
            But_PitchDown.BringToFront()
            But_PitchDown.Visible = True

            But_PitchUp.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            Me.But_PitchUp.Location = Window_Titlebar.PointToClient(Me.But_PitchUp.Parent.PointToScreen(Me.But_PitchUp.Location))
            Me.But_PitchUp.Parent = Window_Titlebar
            But_PitchUp.BringToFront()
            But_PitchUp.Visible = True

            'Pitch Textbox
            PicBox_PitchTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            Me.PicBox_PitchTextBox.Location = Window_Titlebar.PointToClient(Me.PicBox_PitchTextBox.Parent.PointToScreen(Me.PicBox_PitchTextBox.Location))
            Me.PicBox_PitchTextBox.Parent = Window_Titlebar
            Me.PicBox_PitchTextBox.BringToFront()
            PicBox_PitchTextBox.Visible = True

            PicBox_PitchText.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            PicBox_PitchText.Location = Window_Titlebar.PointToClient(PicBox_PitchText.Parent.PointToScreen(PicBox_PitchText.Location))
            PicBox_PitchText.Parent = Window_Titlebar
            PicBox_PitchText.BringToFront()
            PicBox_PitchText.Visible = True
            PicBox_PitchText.BackColor = Color.Transparent


            'Speed Trackbar
            trackBar_Speed2.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            Me.trackBar_Speed2.Location = Window_Titlebar.PointToClient(Me.trackBar_Speed2.Parent.PointToScreen(Me.trackBar_Speed2.Location))
            Me.trackBar_Speed2.Parent = Window_Titlebar
            trackBar_Speed2.BringToFront()
            trackBar_Speed2.Visible = True

            'Speed Control
            But_SpeedDown.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            Me.But_SpeedDown.Location = Window_Titlebar.PointToClient(Me.But_SpeedDown.Parent.PointToScreen(Me.But_SpeedDown.Location))
            Me.But_SpeedDown.Parent = Window_Titlebar
            But_SpeedDown.BringToFront()
            But_SpeedDown.Visible = True

            But_SpeedUp.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            Me.But_SpeedUp.Location = Window_Titlebar.PointToClient(Me.But_SpeedUp.Parent.PointToScreen(Me.But_SpeedUp.Location))
            Me.But_SpeedUp.Parent = Window_Titlebar
            But_SpeedUp.BringToFront()
            But_SpeedUp.Visible = True


            'Speed Textbox
            Label_SpeedTextbox.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            Me.Label_SpeedTextbox.Location = Window_Titlebar.PointToClient(Me.Label_SpeedTextbox.Parent.PointToScreen(Me.Label_SpeedTextbox.Location))
            Me.Label_SpeedTextbox.Parent = Window_Titlebar
            Me.Label_SpeedTextbox.BringToFront()
            Label_SpeedTextbox.Visible = True

            PicBox_SpeedText.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            PicBox_SpeedText.Location = Window_Titlebar.PointToClient(PicBox_SpeedText.Parent.PointToScreen(PicBox_SpeedText.Location))
            PicBox_SpeedText.Parent = Window_Titlebar
            PicBox_SpeedText.BringToFront()
            PicBox_SpeedText.Visible = True
            PicBox_SpeedText.BackColor = Color.Transparent

            Me.VLCChapterMarks.Parent = Window_Titlebar
            Me.VLCChapterMarks.Location = Window_Titlebar.PointToClient(Me.PlaylistTabs.Parent.PointToScreen(Me.PlaylistTabs.Location))
            Me.VLCChapterMarks.Parent = Window_Titlebar
            Me.VLCChapterMarks.BringToFront()
            VLCChapterMarks.Dock = DockStyle.None
            VLCChapterMarks.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Bottom
            VLCChapterMarks.Visible = False
            VLCChapterMarks.Size = New Size(110, Me.Height - 8)
            VLCChapterMarks.Location = New Point(404, 4)

            Me.PlaylistTabs.Parent = Window_Titlebar
            Me.PlaylistTabs.Location = Window_Titlebar.PointToClient(Me.PlaylistTabs.Parent.PointToScreen(Me.PlaylistTabs.Location))
            Me.PlaylistTabs.Parent = Window_Titlebar
            Me.PlaylistTabs.BringToFront()
            PlaylistTabs.Dock = DockStyle.None
            PlaylistTabs.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Bottom
            PlaylistTabs.Visible = True
            PlaylistTabs.Size = New Size(331, Me.Height - 8)
            PlaylistTabs.Location = New Point(404, 4)

            

            But_Search.Location = New Point(TextBox_PlaylistSearch.Location.X + TextBox_PlaylistSearch.Width - 21, But_Search.Location.Y)


            ProgressBarControl1.BringToFront()
        End Sub


#End Region


#Region " Drive Mode"



        Public Sub InitDriveMode()
            ' If AppFullyLoaded = False Then Return
            '   Dim xform As New CarForm

            Me.TopLevel = False
            Me.TopMost = False


            Me.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.None
            '  Me.Opacity = 0
            ' Me.WindowState = FormWindowState.Minimized
            Me.ShowInTaskbar = False

            IsAutoSaving = True
            SaveALLBut = True
            FormClosing()
            SaveALLBut = False
            IsAutoSaving = False
            AudioPlayer.Instance.Pause()

            xcarform.Show()
            xcarform.DontSetParents = True


        End Sub

        Private Sub TimerDriveMode_Tick(sender As Object)
            If My.Settings.DriveMode Then
                InitDriveMode()
            End If

            '  DriveModeTimer.Dispose()

        End Sub
        Public Sub BarBut_Car_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_Car.ItemClick
            InitDriveMode()
        End Sub


#End Region


#Region " Simplistic Mode"
#Region " Declarations"
        Dim Simpleform As New Simplistic_Mode
        Dim but_Back As New PictureBox
        Dim but_ViewPlaylists As New PictureBox
        ' Dim label_FullTime As New RichLabel
        Dim but_Menu As New PictureBox
        Dim panel_Playlists As New Panel

        Dim Playlists_Clicked As Boolean = False

        Public WasSimplisticOn As Boolean = False
#End Region


        Public Sub Simplistic_Mode_On()
            My.Settings.SimplisticMode = True
            Set_Parents()
            PrepareBeforeMiniMode()
			For Each c As Control In Me.Controls
				If c.Name <> "But_SettingsPic" Then
					c.Visible = False
				End If

			Next


			AddHandler MyBase.MouseDown, AddressOf Window_Titlebar_MouseDown
            AddHandler Label_SongName.MouseDown, AddressOf Window_Titlebar_MouseDown
            AddHandler Label_Artist.MouseDown, AddressOf Window_Titlebar_MouseDown

            AddHandler but_Back.Click, AddressOf but_Back_Click
            AddHandler but_Back.MouseEnter, AddressOf but_Back_MouseEnter
            AddHandler but_Back.MouseLeave, AddressOf but_Back_Mouseleave
            AddHandler but_Back.MouseUp, AddressOf but_Back_MouseUp
            AddHandler but_Back.MouseDown, AddressOf but_Back_MouseDown
            AddHandler but_ViewPlaylists.Click, AddressOf but_ViewPlaylists_Click
            AddHandler but_ViewPlaylists.MouseEnter, AddressOf but_ViewPlaylists_MouseEnter
            AddHandler but_ViewPlaylists.MouseLeave, AddressOf but_ViewPlaylists_Mouseleave
            AddHandler but_ViewPlaylists.MouseUp, AddressOf but_ViewPlaylists_MouseUp
            AddHandler but_ViewPlaylists.MouseDown, AddressOf but_ViewPlaylists_MouseDown
            AddHandler but_Menu.Click, AddressOf but_Menu_Click
            AddHandler but_Menu.MouseEnter, AddressOf but_Menu_MouseEnter
            AddHandler but_Menu.MouseLeave, AddressOf but_Menu_MouseLeave
            AddHandler but_Menu.MouseUp, AddressOf but_Menu_MouseUp
            AddHandler but_Menu.MouseDown, AddressOf but_Menu_MouseDown


            Me.MinimumSize = New Size(0, 0)
            Me.MaximumSize = New Size(0, 0)
            Me.Size = Simpleform.Size
            Me.Controls.AddRange({but_Back, but_ViewPlaylists, panel_Playlists, But_Next, But_PlayPause, But_Previous, but_Menu})
        

            but_Back.Location = Simpleform.but_Back.Location
            but_Back.Size = Simpleform.but_Back.Size
            but_Back.BackColor = Color.Transparent
            but_Back.BackgroundImage = Simpleform.but_Back.BackgroundImage
            but_Back.BackgroundImageLayout = ImageLayout.Zoom
            but_Back.Visible = True

            Me.Controls.Add(Label_SongName)
            Label_SongName.Size = Simpleform.label_SongTitle.Size
            Label_SongName.MinimumSize = New Size(250, 25)
            Label_SongName.MaximumSize = New Size(0, 25)
            Label_SongName.Font = Simpleform.label_SongTitle.Font
            Label_SongName.TextAlign = ContentAlignment.MiddleLeft
            Label_SongName.Location = Simpleform.label_SongTitle.Location
            Label_SongName.Visible = True

            Me.Controls.Add(Label_Artist)
            Label_Artist.Size = Simpleform.label_Artist.Size
            Label_Artist.MinimumSize = New Size(300, 18)
            Label_Artist.MaximumSize = New Size(0, 18)
            Label_Artist.Font = Simpleform.label_Artist.Font
            Label_Artist.TextAlign = ContentAlignment.MiddleLeft
            Label_Artist.Location = Simpleform.label_Artist.Location
            Label_Artist.Visible = True

            Me.Controls.Add(Label_Album)
            Label_Album.Size = Simpleform.label_Album.Size
            Label_Album.MinimumSize = New Size(300, 18)
            Label_Album.MaximumSize = New Size(0, 18)
            Label_Album.Font = Simpleform.label_Album.Font
            Label_Album.TextAlign = ContentAlignment.MiddleLeft
            Label_Album.Location = Simpleform.label_Album.Location
            Label_Album.Visible = True

            Me.Controls.Add(timelabel)
            Me.Controls.Add(totaltimelabel)
            timelabel.Font = Simpleform.label_Time.Font
            timelabel.Location = Simpleform.label_Time.Location
            timelabel.Visible = True
            timelabel.BringToFront()
            totaltimelabel.Size = Simpleform.label_Duration.Size
            totaltimelabel.Font = Simpleform.label_Duration.Font
            totaltimelabel.Location = Simpleform.label_Duration.Location
            totaltimelabel.Visible = True
            totaltimelabel.BringToFront()

			Me.Controls.Add(VlcPlayer)
			VlcPlayer.MinimumSize = New Size(0, 0)
			VlcPlayer.Location = Simpleform.pb_Artwork.Location
			VlcPlayer.Size = Simpleform.pb_Artwork.Size
			'  VlcPlayer.Visible = True

			Me.Controls.Add(Artwork)
            Artwork.MinimumSize = New Size(0, 0)
            Artwork.Location = Simpleform.pb_Artwork.Location
            Artwork.Size = Simpleform.pb_Artwork.Size
            Artwork.Visible = True



			Me.Controls.Add(But_TitleBarMin)
            But_TitleBarMin.Location = Simpleform.But_WinMin.Location
            But_TitleBarMin.Visible = True

            Me.Controls.Add(But_TitleBar_Close)
            But_TitleBar_Close.Location = Simpleform.But_WinClose.Location
            But_TitleBar_Close.Visible = True

            Me.Controls.Add(TrackBar_Seek2)
            TrackBar_Seek2.Location = Simpleform.tb_Seek.Location
            TrackBar_Seek2.Size = Simpleform.tb_Seek.Size
            ' TrackBar_Seek2.Visible = True




            But_Previous.Location = Simpleform.but_Previous.Location
            But_Previous.Size = Simpleform.but_Previous.Size
            But_Previous.Visible = True

            But_PlayPause.Location = Simpleform.but_PlayPause.Location
            But_PlayPause.Size = Simpleform.but_PlayPause.Size
            But_PlayPause.Visible = True

            But_Next.Location = Simpleform.but_Next.Location
            But_Next.Size = Simpleform.but_Next.Size
            But_Next.Visible = True

            ' Me.Controls.Add(but_Menu)
            but_Menu.Location = Simpleform.but_Menu.Location
            but_Menu.Size = Simpleform.but_Menu.Size
            but_Menu.BackColor = Color.Transparent
            but_Menu.BackgroundImage = Simpleform.but_Menu.BackgroundImage
            but_Menu.BackgroundImageLayout = ImageLayout.Zoom
            but_Menu.Visible = True

            but_ViewPlaylists.Location = Simpleform.but_Playlists.Location
            but_ViewPlaylists.Size = Simpleform.but_Playlists.Size
            but_ViewPlaylists.BackColor = Color.Transparent
            but_ViewPlaylists.BackgroundImage = Simpleform.but_Playlists.BackgroundImage
            but_ViewPlaylists.BackgroundImageLayout = ImageLayout.Zoom
            but_ViewPlaylists.Visible = True

            panel_Playlists.SendToBack()
            panel_Playlists.Size = Simpleform.panel_Playlists.Size
            panel_Playlists.Location = Simpleform.panel_Playlists.Location
            panel_Playlists.BackColor = Color.Transparent
            panel_Playlists.Controls.Add(PlaylistTabs)
            PlaylistTabs.Visible = True
            PlaylistTabs.Dock = DockStyle.Fill
            panel_Playlists.Visible = False

            For Each c As Control In Me.Controls
                If c.Visible = True Then
                    c.BringToFront()
                End If
            Next

			but_Back.SendToBack()

			but_ViewPlaylists.BringToFront()
            but_Menu.BringToFront()
            Label_SongName.SendToBack()
            But_TitleBarMin.BringToFront()
			But_TitleBar_Close.BringToFront()
			Me.Controls.Add(But_SettingsPic)
			But_SettingsPic.Visible = True
			But_SettingsPic.SendToBack()
			But_SettingsPic.Size = but_Back.Size
			But_SettingsPic.Location = but_Back.Location
		End Sub

        Public Sub but_Back_Click()
            If Playlists_Clicked = False Then
                WasSimplisticOn = True
                But_Previous.Size = PreviousSize
                But_PlayPause.Size = PlayPauseSize
                But_Next.Size = NextSize
                Artwork.Size = ArtworkSize
                VlcPlayer.Size = ArtworkSize
                totaltimelabel.MaximumSize = New Size(450, 0)
                totaltimelabel.MinimumSize = New Size(41, 17)
                totaltimelabel.MaximumSize = New Size(80, 17)
                totaltimelabel.TextAlign = ContentAlignment.MiddleRight

                If My.Settings.MiniModeOn Then
                    MiniModeOn()
                Else
                    MiniModeOff()
                End If

                My.Settings.SimplisticMode = False
                TimerSimpleOff.Start()
            Else
				But_SettingsPic.BringToFront()
				TrackBar_Seek2.Visible = True
                timelabel.Visible = True
                totaltimelabel.Visible = True

                panel_Playlists.SendToBack()
                panel_Playlists.Visible = False
                Playlists_Clicked = False
            End If

        End Sub
        Private Sub TimerSimpleOff_Tick(sender As Object, e As EventArgs) Handles TimerSimpleOff.Tick
            If My.Settings.SimplisticMode = False Then
                WasSimplisticOn = False
                TimerSimpleOff.Stop()
            End If
        End Sub


        Public Shared but_BackHover As Boolean = False
        Public Sub but_Back_MouseEnter(sender As Object, e As EventArgs)
            but_Back.BackgroundImage = My.Resources.SwitchBack_Hover
			but_BackHover = True

			TimerSettingsBack.Start()
		End Sub
        Public Sub but_Back_Mouseleave(sender As Object, e As EventArgs)
            but_Back.BackgroundImage = My.Resources.SwitchBack
			but_BackHover = False
			TimerSettingsBack.Stop()
		End Sub
        Public Sub but_Back_MouseDown(sender As Object, e As MouseEventArgs)
            but_Back.BackgroundImage = My.Resources.SwitchBack_Press
        End Sub
        Public Sub but_Back_MouseUp(sender As Object, e As MouseEventArgs)
            If but_BackHover = True Then
                but_Back.BackgroundImage = My.Resources.SwitchBack_Hover
            Else
                but_Back.BackgroundImage = My.Resources.SwitchBack
            End If
            MyBase.Focus()
        End Sub

        Public Sub but_Menu_Click()
            Popup_Playlist.ShowPopup(MousePosition)

        End Sub

		Private Sub TimerSettingsBack_Tick(sender As Object, e As EventArgs) Handles TimerSettingsBack.Tick
			If but_BackHover Then
				But_SettingsPic.BringToFront()
			ElseIf SettingsPicHover Then
				but_Back.BringToFront()
			End If



		End Sub

		Public Shared but_MenuHover As Boolean = False
        Public Sub but_Menu_MouseEnter(sender As Object, e As EventArgs)
            but_Menu.BackgroundImage = My.Resources.Three_Dots_Hover
            but_MenuHover = True
        End Sub
        Public Sub but_Menu_Mouseleave(sender As Object, e As EventArgs)
            but_Menu.BackgroundImage = My.Resources.Three_Dots
            but_MenuHover = False
        End Sub
        Public Sub but_Menu_MouseDown(sender As Object, e As MouseEventArgs)
            but_Menu.BackgroundImage = My.Resources.Three_Dots_Press
        End Sub
        Public Sub but_Menu_MouseUp(sender As Object, e As MouseEventArgs)
            If but_MenuHover = True Then
                but_Menu.BackgroundImage = My.Resources.Three_Dots_Hover
            Else
                but_Menu.BackgroundImage = My.Resources.Three_Dots
            End If
            MyBase.Focus()
        End Sub

        Public Sub but_ViewPlaylists_Click()
            Playlists_Clicked = True
			but_Back.BringToFront()

			'panel_Playlists.Controls.Add(PlaylistTabs)
			' PlaylistTabs.Visible = True
			Label_Artist.SendToBack()
            panel_Playlists.BringToFront()
            panel_Playlists.Visible = True

            TrackBar_Seek2.Visible = False
            timelabel.Visible = False
            totaltimelabel.Visible = False

        End Sub

        Public Shared but_ViewPlaylistsHover As Boolean = False
        Public Sub but_ViewPlaylists_MouseEnter(sender As Object, e As EventArgs)
            but_ViewPlaylists.BackgroundImage = My.Resources.Playlists_2_Hover
            but_ViewPlaylistsHover = True
        End Sub
        Public Sub but_ViewPlaylists_Mouseleave(sender As Object, e As EventArgs)
            but_ViewPlaylists.BackgroundImage = My.Resources.Playlists_2
            but_ViewPlaylistsHover = False
        End Sub
        Public Sub but_ViewPlaylists_MouseDown(sender As Object, e As MouseEventArgs)
            but_ViewPlaylists.BackgroundImage = My.Resources.Playlists_2_Press
        End Sub
        Public Sub but_ViewPlaylists_MouseUp(sender As Object, e As MouseEventArgs)
            If but_ViewPlaylistsHover = True Then
                but_ViewPlaylists.BackgroundImage = My.Resources.Playlists_2_Hover
            Else
                but_ViewPlaylists.BackgroundImage = My.Resources.Playlists_2
            End If
            MyBase.Focus()
        End Sub


#End Region

#End Region

#Region " REFRESH APP   &   Auto Save  &  Auto Update"
#Region " Prepare    |   Refresh Skin   |   Other Declartions"
        Public UpdateCanceled As Boolean = False

        Public Sub Prepare()
            If AppOpenFinished = False Then Return
            ResetThumbnail()

        End Sub
        Public Sub RefreshSkin()


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
            Try
                If Not Playlist Is Nothing Then
                    If PlaylistClosing Then
                        GC.Collect()
                        PlaylistClosing = False
                    Else
                        RefreshApp()
                    End If
                End If
            Catch
            End Try
        End Sub

#End Region


        'REFRESH  Menu Button
        Public Sub BarButtonRefresh_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_Refresh.ItemClick
            NeedResizeRefresh = True
            RefreshApp()
        End Sub
        'Code
        Public Sub RefreshApp()
            If FormClosingval Then Return : If AppOpenFinished = False Then Return : If AddingFiles Then Return : If xcarform.DontSetParents Then Return : If My.Settings.DriveMode Then Return

            Meta_and_Artwork_Timer2.Start() : Timer_Meta_and_Artwork.Start()

            Try
                Dim SongArrangePanelIsAdded As Boolean = False
                For Each c As Control In PlaylistTabs.SelectedTabPage.Controls
                    If c.Name = SongArrangePanel.Name Then
                        SongArrangePanelIsAdded = True
                    End If
                Next
                If SongArrangePanelIsAdded = False Then
                    PlaylistTabs.SelectedTabPage.Controls.Add(SongArrangePanel)
                    PlaylistTabs.SelectedTabPage.Controls.Add(OpenProgressBar)
                    OpenProgressBar.BringToFront()
                End If
            Catch ex As Exception : End Try

            If My.Settings.SkinChanged Then
                BarCheckBox_UseCustomImage.Checked = My.Settings.CustomImageCheckState
                Setup_Enhanced_Skins()
            End If
            If NeedResizeRefresh Then
                Timer_PlaylistsSizes.Start()
                If My.Settings.TouchFriendly = True Then
                    SkinChange_Playlists_Touch()
                Else
                    SkinChange_Playlists_Standard()
                End If


            End If

            Try
                If Not PlaylistTabs.SelectedTabPage.PageVisible Then
                    TryCast(PlaylistTabs, XtraTabControl).MakePageVisible(PlaylistTabs.SelectedTabPage)
                End If
            Catch ex As Exception : End Try


            If My.Settings.MiniModeOn Then
                PictureBoxSpec.Location = New Point(169, 179)
            End If


            ctrlpress = False
            shiftpress = False


        End Sub
        ' Timer: 2.5 secs
        Public Sub RefreshTimer_Tick(sender As Object, e As EventArgs) Handles Timer_Refresh.Tick
            ' 2.5 Secs

            If AppOpenFinished = False Then Return
            RefreshApp()
            Timer_Refresh.Stop()
        End Sub
        ' Timer: 10 secs
        Public Sub RefreshTimer2_Tick(sender As Object, e As EventArgs) Handles Timer_Refresh2.Tick
            ' 10 Secs

            If AppOpenFinished = False Then Return
            NeedResizeRefresh = True
            RefreshApp()
            Timer_Refresh2.Stop()
        End Sub



        'Thumbnail
        Public Sub Artwork_SizeChanged(sender As Object, e As EventArgs) Handles Artwork.SizeChanged
            ResetThumbnail()
        End Sub
        Public Sub ResetThumbnail()
            Try
                If My.Settings.FirstTimeSetup Then Return
                If AppOpenFinished = False Then Return
                '  CheckIfVideo()
                '  If IsVideo Then
                '       TaskbarManager.Instance.TabbedThumbnail.SetThumbnailClip(Me.Handle, New Rectangle(VlcPlayer.Location, VlcPlayer.Size))
                '  Else
                If My.Settings.DriveMode Then
                    Try
                        TaskbarManager.Instance.TabbedThumbnail.SetThumbnailClip(CarForm.Handle, New Rectangle(Artwork.Location, Artwork.Size))
                    Catch ex As Exception
                    End Try
                Else
                    TaskbarManager.Instance.TabbedThumbnail.SetThumbnailClip(Me.Handle, New Rectangle(Artwork.Location, Artwork.Size))
                End If

                '  End If

                If FirstSetupTaskbar Then

                    Dim array(3) As ThumbnailToolBarButton
                    array(0) = TaskbarStopBut
                    array(1) = TaskbarPreviousBut
                    array(2) = TaskbarPlayPauseBut
                    array(3) = TaskbarNextBut
                    TaskbarManager.Instance.ThumbnailToolBars.AddButtons(Me.Handle, array) 'TaskbarStopBut, TaskbarPreviousBut, TaskbarPlayPauseBut, TaskbarNextBut)

                    AddHandler TaskbarStopBut.Click, AddressOf StopSub
                    AddHandler TaskbarPreviousBut.Click, AddressOf PrevItem
                    AddHandler TaskbarPlayPauseBut.Click, AddressOf PlayALL
                    AddHandler TaskbarNextBut.Click, AddressOf NextItem
                    FirstSetupTaskbar = False
                    Timer_SetupTaskbar.Stop()
                End If
            Catch
            End Try
        End Sub


        Public Sub TimerSetupTaskbar_Tick(sender As Object, e As EventArgs) Handles Timer_SetupTaskbar.Tick
            FirstSetupTaskbar = True
            ResetThumbnail()
        End Sub

        'Auto Update: 20 secs
        Public Sub AutoUpdaterTimer_Tick(sender As Object, e As EventArgs) Handles Timer_AutoUpdater.Tick
            If AppOpenFinished = False Then Return
            If AppOpenFinished Then
                If UpdateCanceled = False Then
                    If AppOpen = False Then
                        If Web_Update.UpdateAvailable(True) Then
                            ' Web_Update.AutoUpdate()
                            If My.Settings.MiniModeOn = False Then
                                Label_Update.Visible = True
                                Label_Update.BringToFront()
                                Label_Update.Location = New Point(168, 20)
                            Else
                                Label_Update.Visible = True
                                Label_Update.Left = PicBox_Window_AppName.Left - 3
                                Label_Update.Top = PicBox_Window_AppName.Top + 20
                                '  Label_Update.Text = "Update" & Environment.NewLine & "Available!"
                                Label_Update.BringToFront()
                            End If

                        Else
                            Label_Update.Visible = False
                        End If

                    End If
                End If

                '   Timer_AutoUpdater.Stop()
            End If   
        End Sub
    
#Region " Auto Save"
        Public IsAppFirstOpening As Boolean = False
        Public IsAutoSaving As Boolean = False

        Public Sub AutoSaveTimer_Tick()
            If AppOpenFinished = False Then Return
            If My.Settings.AutoSave = True Then
                IsAutoSaving = True
                SaveALLBut = True
                FormClosing()
                SaveALLBut = False
                IsAutoSaving = False
            End If
        End Sub

#End Region


#End Region

#Region " Global HotKeys"
#Region " Send Clicks"

#Region " Declare Send Clicks"
        Dim Ctrl1 = WindowsInput.Native.VirtualKeyCode.CONTROL
        Dim Alt1 = WindowsInput.Native.VirtualKeyCode.MENU
        Dim Shift1 = WindowsInput.Native.VirtualKeyCode.SHIFT



#End Region

        Public Function SendClick(m As System.Collections.Generic.IEnumerable(Of WindowsInput.Native.VirtualKeyCode), k As WindowsInput.Native.VirtualKeyCode)

            Dim keyboard As New WindowsInput.InputSimulator
            keyboard.Keyboard.ModifiedKeyStroke(m, k)
        End Function

#End Region
#Region " Declarations"

        Public hkr As New HotKeyRegistryClass(Me.Handle)
        'Public Const WM_APPCOMMAND As UInteger = &H319
        Private Const WM_APPCOMMAND As Integer = &H319
        Private Const APPCOMMAND_MEDIA_PLAY_PAUSE As Integer = 14
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
        Dim ctrl = HotKeyRegistryClass.Modifiers.MOD_CTRL
        Dim alt = HotKeyRegistryClass.Modifiers.MOD_ALT
        Dim shift = HotKeyRegistryClass.Modifiers.MOD_SHIFT
#End Region
#Region " Setup"
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

                'Toggle Fullscreen
                hkr.Register(My.Settings.KeyFullscreenCtrl Or My.Settings.KeyFullscreenAlt, My.Settings.KeyFullscreen).ToString()


                ' RegisterHamburgerMenu()

            Catch
                MyMsgBox.Show("Error 0x32: Error loading Global Hotkeys. To fix, reapply your desired Hotkeys from the Edit menu.", "", True)
            End Try
        End Sub
        Public Sub RegisterBlankHotkeys()
            Try
                For i As Integer = 0 To 15
                    hkr.Register(Nothing, Nothing)
                Next
            Catch ex As Exception

            End Try
        End Sub
        Public Sub RegisterHamburgerMenu()

            'Hamburger Menu
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.F1)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT, Keys.V)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.S)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.I)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT, Keys.M)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT, Keys.O)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.O)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.A)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.P)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.Y)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.R)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.F22)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.F2)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT, Keys.F2)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.Q)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.C)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT, Keys.A)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.E)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT, Keys.S)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.S)
            '36 
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.Y)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.C)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.L)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.R)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.E)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT, Keys.E)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.W)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.O)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL, Keys.E)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL, Keys.C)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL, Keys.W)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL, Keys.T)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL, Keys.D)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL, Keys.P)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_ALT, Keys.R)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_ALT, Keys.A)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.R)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.O)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.T)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.U)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.F24)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.F23)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.F21)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.F20)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.F19)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.F18)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.F17)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.F16)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.F15)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.F14)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.F13)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.F12)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.F11)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT Or HotKeyRegistryClass.Modifiers.MOD_SHIFT, Keys.F10)
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_ALT, Keys.T)
        End Sub
        Public Sub UnRegisterHamburgerMenu()
            For i As Integer = 16 To 71
                hkr.Unregister(i)
            Next
        End Sub
        Public Sub RegisterStartupForm()
            hkr.Register(HotKeyRegistryClass.Modifiers.MOD_CTRL Or HotKeyRegistryClass.Modifiers.MOD_SHIFT Or HotKeyRegistryClass.Modifiers.MOD_ALT, Keys.F12)
        End Sub
        Public Sub UnRegisterStartupForm()
            hkr.Unregister(72)
        End Sub
        Public Sub UnRegisterAll()
            Try
                For i As Integer = 0 To 75
                    hkr.Unregister(i)
                Next
            Catch ex As Exception
            End Try
        End Sub
#End Region

        'Hotkey Pressed
        Protected Overrides Sub WndProc(ByRef m As Message)

            Try


                If m.Msg = HotKeyRegistryClass.Messages.WM_HOTKEY Then 'NOT THE ACTUAL WINDOWS NAMESPACE
                    Dim ID As String = m.WParam.ToString()

                    Select Case ID
                        'F9
                        Case 0
                            If My.Settings.YouTubeHotkeys Then
                                If Application.OpenForms().OfType(Of YouTubeBrowser).Any Then
                                    Try : Dim w As Integer : Dim processRunning As Process() = Process.GetProcesses() : For Each pr As Process In processRunning : Select Case pr.ProcessName : Case "Rich Player" : w = pr.MainWindowHandle.ToInt32() : SetForegroundWindow(w) : Case "Rich Player.vshost" : w = pr.MainWindowHandle.ToInt32() : SetForegroundWindow(w) : End Select : Next : Catch : End Try
                                    Dim xform As Form = Application.OpenForms.OfType(Of YouTubeBrowser)().Where(Function(frm) frm.Name = "YouTubeBrowser").SingleOrDefault
                                    xform.Focus()
                                    Dim web As Gecko.GeckoWebBrowser : For Each c As Control In xform.Controls : If c.GetType Is GetType(Gecko.GeckoWebBrowser) Then : web = c : End If : Next : web.Focus()
                                    MenuForm.SendClick(Nothing, WindowsInput.Native.VirtualKeyCode.VK_K)
                                Else
                                    '......SAME.......
                                    If My.Settings.UseMediaKeys Then
                                        MenuForm.SendClick(Nothing, WindowsInput.Native.VirtualKeyCode.MEDIA_PLAY_PAUSE)
                                    Else
                                        PlayHotkeySub()
                                    End If
                                End If
                            Else
                                '......SAME.......
                                If My.Settings.UseMediaKeys Then
                                    MenuForm.SendClick(Nothing, WindowsInput.Native.VirtualKeyCode.MEDIA_PLAY_PAUSE)
                                Else
                                    PlayHotkeySub()
                                End If
                            End If


                        Case 1
                            If My.Settings.YouTubeHotkeys Then
                                If Application.OpenForms().OfType(Of YouTubeBrowser).Any Then
                                    Try : Dim w As Integer : Dim processRunning As Process() = Process.GetProcesses() : For Each pr As Process In processRunning : Select Case pr.ProcessName : Case "Rich Player" : w = pr.MainWindowHandle.ToInt32() : SetForegroundWindow(w) : Case "Rich Player.vshost" : w = pr.MainWindowHandle.ToInt32() : SetForegroundWindow(w) : End Select : Next : Catch : End Try
                                    Dim xform As Form = Application.OpenForms.OfType(Of YouTubeBrowser)().Where(Function(frm) frm.Name = "YouTubeBrowser").SingleOrDefault
                                    xform.Focus() : Dim web As Gecko.GeckoWebBrowser : For Each c As Control In xform.Controls
                                        If c.GetType Is GetType(Gecko.GeckoWebBrowser) Then : web = c : End If : Next : web.Focus()
                                    MenuForm.SendClick(Nothing, WindowsInput.Native.VirtualKeyCode.VK_0)
                                Else
                                    '......SAME.......
                                    If My.Settings.UseMediaKeys Then
                                        MenuForm.SendClick(Nothing, WindowsInput.Native.VirtualKeyCode.MEDIA_PREV_TRACK)
                                    Else
                                        PrevHotkeySub()
                                    End If
                                End If
                            Else
                                '......SAME.......
                                If My.Settings.UseMediaKeys Then
                                    MenuForm.SendClick(Nothing, WindowsInput.Native.VirtualKeyCode.MEDIA_PREV_TRACK)
                                Else
                                    PrevHotkeySub()
                                End If
                            End If


                            'F11
                        Case 2
                            If My.Settings.YouTubeHotkeys Then
                                If Application.OpenForms().OfType(Of YouTubeBrowser).Any Then
                                    Try : Dim w As Integer : Dim processRunning As Process() = Process.GetProcesses() : For Each pr As Process In processRunning : Select Case pr.ProcessName : Case "Rich Player" : w = pr.MainWindowHandle.ToInt32() : SetForegroundWindow(w) : Case "Rich Player.vshost" : w = pr.MainWindowHandle.ToInt32() : SetForegroundWindow(w) : End Select : Next : Catch : End Try
                                    Dim xform As Form = Application.OpenForms.OfType(Of YouTubeBrowser)().Where(Function(frm) frm.Name = "YouTubeBrowser").SingleOrDefault
                                    xform.Focus() : Dim web As Gecko.GeckoWebBrowser : For Each c As Control In xform.Controls
                                        If c.GetType Is GetType(Gecko.GeckoWebBrowser) Then : web = c : End If : Next : web.Focus()
                                    MenuForm.SendClick({WindowsInput.Native.VirtualKeyCode.SHIFT}, WindowsInput.Native.VirtualKeyCode.VK_N)
                                Else
                                    '......SAME.......
                                    If My.Settings.UseMediaKeys Then
                                        MenuForm.SendClick(Nothing, WindowsInput.Native.VirtualKeyCode.MEDIA_NEXT_TRACK)
                                    Else
                                        NextHotkeySub()
                                    End If
                                End If
                            Else
                                '......SAME.......
                                If My.Settings.UseMediaKeys Then
                                    MenuForm.SendClick(Nothing, WindowsInput.Native.VirtualKeyCode.MEDIA_NEXT_TRACK)
                                Else
                                    NextHotkeySub()
                                End If
                            End If


                            'Ctrl + F9
                        Case 3
                            If My.Settings.YouTubeHotkeys Then
                                If Application.OpenForms().OfType(Of YouTubeBrowser).Any Then
                                    Try : Dim w As Integer : Dim processRunning As Process() = Process.GetProcesses() : For Each pr As Process In processRunning : Select Case pr.ProcessName : Case "Rich Player" : w = pr.MainWindowHandle.ToInt32() : SetForegroundWindow(w) : Case "Rich Player.vshost" : w = pr.MainWindowHandle.ToInt32() : SetForegroundWindow(w) : End Select : Next : Catch : End Try
                                    Dim xform As Form = Application.OpenForms.OfType(Of YouTubeBrowser)().Where(Function(frm) frm.Name = "YouTubeBrowser").SingleOrDefault
                                    xform.Focus() : Dim web As Gecko.GeckoWebBrowser : For Each c As Control In xform.Controls
                                        If c.GetType Is GetType(Gecko.GeckoWebBrowser) Then : web = c : End If : Next : web.Focus()
                                    Dim ctrl As WindowsInput.Native.VirtualKeyCode
                                    If My.Settings.PreviousKeyCtrl = 0 Then
                                        ctrl = Nothing
                                    ElseIf My.Settings.PreviousKeyCtrl = 2 Then
                                        ctrl = WindowsInput.Native.VirtualKeyCode.CONTROL
                                    End If
                                    Dim ctrl2 As WindowsInput.Native.VirtualKeyCode
                                    If My.Settings.PlayPauseKeyCtrl = 0 Then
                                        ctrl2 = Nothing
                                    ElseIf My.Settings.PlayPauseKeyCtrl = 2 Then
                                        ctrl2 = WindowsInput.Native.VirtualKeyCode.CONTROL
                                    End If
                                    MenuForm.SendClick({ctrl}, My.Settings.PreviousKey)
                                    MenuForm.SendClick({ctrl2}, My.Settings.PlayPauseKey)
                                Else
                                    If My.Settings.UseMediaKeys Then
                                        MenuForm.SendClick(Nothing, WindowsInput.Native.VirtualKeyCode.MEDIA_STOP)
                                    Else
                                        '......SAME.......
                                        If My.Settings.YouTubeOpened Then
                                            AddYTBrowserURL()
                                        ElseIf My.Settings.RadioOpened Then
                                            AddRadioURL()
                                        Else
                                            StopSub()
                                        End If
                                    End If
                                End If
                            Else
                                If My.Settings.UseMediaKeys Then
                                    MenuForm.SendClick(Nothing, WindowsInput.Native.VirtualKeyCode.MEDIA_STOP)
                                Else
                                    '......SAME.......
                                    If My.Settings.YouTubeOpened Then
                                        AddYTBrowserURL()
                                    ElseIf My.Settings.RadioOpened Then
                                        AddRadioURL()
                                    Else
                                        StopSub()
                                    End If
                                End If
                            End If



                            'Ctrl + F10
                        Case 4
                            If My.Settings.YouTubeHotkeys Then
                                If Application.OpenForms().OfType(Of YouTubeBrowser).Any Then
                                    Try : Dim w As Integer : Dim processRunning As Process() = Process.GetProcesses() : For Each pr As Process In processRunning : Select Case pr.ProcessName : Case "Rich Player" : w = pr.MainWindowHandle.ToInt32() : SetForegroundWindow(w) : Case "Rich Player.vshost" : w = pr.MainWindowHandle.ToInt32() : SetForegroundWindow(w) : End Select : Next : Catch : End Try
                                    Dim xform As Form = Application.OpenForms.OfType(Of YouTubeBrowser)().Where(Function(frm) frm.Name = "YouTubeBrowser").SingleOrDefault
                                    xform.Focus() : Dim web As Gecko.GeckoWebBrowser : For Each c As Control In xform.Controls
                                        If c.GetType Is GetType(Gecko.GeckoWebBrowser) Then : web = c : End If : Next : web.Focus()
                                    MenuForm.SendClick({WindowsInput.Native.VirtualKeyCode.SHIFT}, WindowsInput.Native.VirtualKeyCode.LEFT)
                                Else
                                    Prevrwhover = True
                                    SkipBackwards5Secs()
                                    Prevrwhover = False
                                End If
                            Else
                                Prevrwhover = True
                                SkipBackwards5Secs()
                                Prevrwhover = False

                            End If


                            'Ctrl + f11
                        Case 5
                            If My.Settings.YouTubeHotkeys Then
                                If Application.OpenForms().OfType(Of YouTubeBrowser).Any Then
                                    Try : Dim w As Integer : Dim processRunning As Process() = Process.GetProcesses() : For Each pr As Process In processRunning : Select Case pr.ProcessName : Case "Rich Player" : w = pr.MainWindowHandle.ToInt32() : SetForegroundWindow(w) : Case "Rich Player.vshost" : w = pr.MainWindowHandle.ToInt32() : SetForegroundWindow(w) : End Select : Next : Catch : End Try
                                    Dim xform As Form = Application.OpenForms.OfType(Of YouTubeBrowser)().Where(Function(frm) frm.Name = "YouTubeBrowser").SingleOrDefault
                                    xform.Focus() : Dim web As Gecko.GeckoWebBrowser : For Each c As Control In xform.Controls
                                        If c.GetType Is GetType(Gecko.GeckoWebBrowser) Then : web = c : End If : Next : web.Focus()
                                    MenuForm.SendClick({WindowsInput.Native.VirtualKeyCode.SHIFT}, WindowsInput.Native.VirtualKeyCode.RIGHT)
                                Else
                                    NextButhover = True
                                    SkipForward5Secs()
                                    NextButhover = False
                                End If
                            Else
                                NextButhover = True
                                SkipForward5Secs()
                                NextButhover = False

                            End If
                            If Application.OpenForms().OfType(Of YouTubeBrowser).Any Then
                                If My.Settings.YouTubeHotkeys Then Return
                            End If
                            NextButhover = True
                            SkipForward5Secs()
                            NextButhover = False

                        Case 6      'A Repeast
                            A_Repeat()
                        Case 7      'B Repeat
                            B_Repeat()
                        Case 8      'AB Repeat Reset
                            Reset_AB_Repeat()
                        Case 9      'Slow Down
                            Speed_Slow()
                        Case 10     'Speed Up
                            Speed_Fast()
                        Case 11     'Speed Reset
                            Speed_Norm()

                        Case 12     'Pitch Down
                            If IsVideo = False Then
                                If Not trackbar_Pitch2.Value = -12 Then
                                    trackbar_Pitch2.Value -= 1
                                End If
                            End If
                        Case 13     'Pitch Up
                            If IsVideo = False Then
                                If Not trackbar_Pitch2.Value = 12 Then
                                    trackbar_Pitch2.Value += 1
                                End If
                            End If
                        Case 14     'Pitch Reset
                            If IsVideo = False Then
                                trackbar_Pitch2.Value = 0
                            End If
                        Case 15
                            MySpotify.ToggleHide()

                        Case 16
                            FullScreenVLC()






                            '--------- Hamburger Menu ---------
                        Case 17 ' Open
                            If My.Settings.YouTubeURL = "FirstOpened" Then
                                BringMeToFocus()
                                Dim xform As New YouTubeBrowser
                                xform.StartPosition = FormStartPosition.Manual
                                xform.Location = New System.Drawing.Point(Me.Location.X + Me.Width / 2 - xform.Width / 2, Me.Location.Y + Me.Height / 2 - xform.Height / 2)
                                xform.Show()
                                My.Settings.YouTubeURL = ""
                                My.Settings.Save()
                                xform.BringToFront()
                            ElseIf My.Settings.RadioURL = "FirstOpened" Then

                                BringMeToFocus()
                                Dim xform As New RadioBrowser
                                xform.StartPosition = FormStartPosition.Manual
                                xform.Location = New System.Drawing.Point(Me.Location.X + Me.Width / 2 - xform.Width / 2, Me.Location.Y + Me.Height / 2 - xform.Height / 2)
                                xform.Show()
                                My.Settings.RadioURL = ""
                                My.Settings.Save()
                                xform.BringToFront()
                            Else
                                BringMeToFocus()
                                OpenButtonClick()
                            End If

                        Case 18 ' Player Vol Hide/Show
                            BringMeToFocus()
                            ChangeValuePlayerVol()
                        Case 19 ' Search Box Hide/Shpow
                            BringMeToFocus()
                            ChangeStatePlaylistSearchBox()
                        Case 20 ' Use Shadows
                            BringMeToFocus()
                            BarCheckbox_UseShadows.Checked = Not BarCheckbox_UseShadows.Checked
                        Case 21 ' Mini Mode
                            BringMeToFocus()
                            BarCheckBox_MiniMode.Checked = Not BarCheckBox_MiniMode.Checked
                        Case 22 'Open Folder
                            BringMeToFocus()
                            OpenFolderSub()
                        Case 23 'Open and sub Folder
                            BringMeToFocus()
                            OpenFolderSubSub()
                        Case 24 ' Add Folders
                            BringMeToFocus()
                            AddFolderandSub()
                        Case 25
                            BringMeToFocus()
                            ShufflePlay()
                        Case 26
                            BringMeToFocus()
                            AddYTURL()
                        Case 27
                            BringMeToFocus()
                            AddRadio()
                        Case 28
                            BringMeToFocus()
                            SavePlaylist()
                        Case 29
                            BringMeToFocus()
                            Rename_Playlist()
                        Case 30
                            BringMeToFocus()
                            RenameCurrentItem()
                        Case 31
                            BringMeToFocus()
                            MenuForm.SendClick({WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.SHIFT}, WindowsInput.Native.VirtualKeyCode.VK_Q)
                            'QuickOpenMenu.ShowPopup(But_SettingsPic.PointToScreen(But_SettingsPic.Location))
                        Case 32
                            BringMeToFocus()
                            ChooseFavoriteColor()
                        Case 33
                            BringMeToFocus()
                            Adjustfont()
                        Case 34
                            BringMeToFocus()
                            OpenEqualizer()
                        Case 35
                            BringMeToFocus()
                            OpenSpotify()
                        Case 36
                            BringMeToFocus()
                            If MenuForm.RefreshSpotify Then
                                UsingSpotify = True
                                UseSpotify()
                                Meta_and_Artwork_Timer2.Start()
                                Timer_Spotify.Start()
                            Else
                                Dim SpotifyLocalFixed As Boolean = False
                                If SpotifyLocalFixed Then
                                    If BarCheckBox_UseSpotifyold.Checked Then
                                        BarCheckBox_UseSpotifyold.Checked = False
                                    Else
                                        BarCheckBox_UseSpotifyLocal.Checked = Not BarCheckBox_UseSpotifyLocal.Checked
                                    End If
                                Else
                                    BarCheckBox_UseSpotifyold.Checked = Not BarCheckBox_UseSpotifyold.Checked
                                End If
                            End If



                        Case 37
                            BringMeToFocus()
                            Dim xform As New Tutorial
                            xform.StartPosition = FormStartPosition.Manual
                            xform.Location = New System.Drawing.Point(Me.Location.X + Me.Width / 2 - xform.Width / 2, Me.Location.Y + Me.Height / 2 - xform.Height / 2)
                            xform.Show()
                            xform.BringToFront()
                            xform.TopMost = True
                        Case 38
                            BringMeToFocus()
                            EditHotkeys()
                        Case 39
                            BringMeToFocus()
                            SaveLyrics()
                        Case 40
                            If HotkeysReset Then
                                BringMeToFocus()
                                BarCheckbox_DisableHotkeys.Checked = Not BarCheckbox_DisableHotkeys.Checked

                                HotkeysReset = False

                            Else
                                BringMeToFocus()
                                ResetLyrics()
                            End If

                        Case 41
                            BringMeToFocus()
                            ArtworkFilename()
                        Case 42
                            BringMeToFocus()
                            ArtworkAlbum()
                        Case 43
                            BringMeToFocus()
                            ViewLyrics()
                            Return
                        Case 44
                            BringMeToFocus()
                            ChangeArtworkOpacity()

                        Case 45
                            BringMeToFocus()
                            EnahancedSkins()
                        Case 46
                            BringMeToFocus()
                            ControlsBG()
                        Case 47
                            BringMeToFocus()
                            PlaylistTabsChangeWidth()
                        Case 48
                            BringMeToFocus()
                            CustomizeSliders()
                        Case 49
                            BringMeToFocus()
                            InitDriveMode()
                        Case 50
                            BringMeToFocus()
                            Options()
                        Case 51
                            BringMeToFocus()
                            NeedResizeRefresh = True
                            RefreshApp()
                        Case 52
                            BringMeToFocus()
                            SaveAll()
                        Case 53
                            BringMeToFocus()
                            ResetSettings()
                        Case 54
                            BringMeToFocus()
                            OpenAppLocation()
                        Case 55
                            BringMeToFocus()
                            BarCheckbox_DisableHotkeys.Checked = Not BarCheckbox_DisableHotkeys.Checked
                        Case 56
                            UnRegisterHamburgerMenu()
                        Case 57
                            BringMeToFocus()
                            EnableLyricEditing()
                        Case 58
                            BringMeToFocus()
                            EditSpectrumColors()
                        Case 59
                            BringMeToFocus()
                            WindowsColors.ShowDialog()
                            If WindowsColors.DialogResult = System.Windows.Forms.DialogResult.OK Then
                                ChangeHue()
                            End If
                        Case 60
                            BringMeToFocus()
                            SwitchOrientation()
                        Case 61
                            BringMeToFocus()
                            If PlayerBounds.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                                Me.Location = My.Settings.PlayerLocation
                                Me.Size = My.Settings.FormSize
                            End If
                        Case 62
                            BringMeToFocus()
                            Dim xform As New AboutForm
                            xform.ShowDialog()
                        Case 63
                            BringMeToFocus()
                            Me.Close()
                        Case 64
                            BringMeToFocus()
                            Dim result As Integer = MyFullMsgBox.Show("This will force the app to close. Settings will NOT be saved!", _
                                                                      "Are you sure you want to force exit?", True, MyFullMsgBox.CustomButtons.YesNo)
                            If result = DialogResult.Yes Then
                                Process.GetCurrentProcess.Kill()
                            End If
                        Case 65
                            BringMeToFocus()
                            MenuForm.SendClick({WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.SHIFT}, WindowsInput.Native.VirtualKeyCode.F19)
                        Case 66
                            BringMeToFocus()
                            MenuForm.SendClick({WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.SHIFT}, WindowsInput.Native.VirtualKeyCode.F20)
                        Case 67
                            BringMeToFocus()
                            MenuForm.SendClick({WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.SHIFT}, WindowsInput.Native.VirtualKeyCode.F18)

                        Case 68
                            Spotify_ViewAccount()


                        Case 69
                            BringMeToFocus()
                            BackupSettings()
                        Case 70
                            BringMeToFocus()
                            RestoreSettings()

                        Case 71
                            BringMeToFocus()
                            Simplistic_Mode_On()
                          

                        Case 72
                            SetupForm.Dispose()
                            BringMeToFocus()
                            My.Settings.SkinChanged = True
                            RefreshApp()
                            UnRegisterStartupForm()


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

                        Case WM_APPCOMMAND
                            Dim cmd As Integer = m.LParam
                            Select Case cmd
                                Case APPCOMMAND_MEDIA_PLAY_PAUSE
                                    If My.Settings.YouTubeHotkeys Then
                                        If Application.OpenForms().OfType(Of YouTubeBrowser).Any Then
                                            Try : Dim w As Integer : Dim processRunning As Process() = Process.GetProcesses() : For Each pr As Process In processRunning : Select Case pr.ProcessName : Case "Rich Player" : w = pr.MainWindowHandle.ToInt32() : SetForegroundWindow(w) : Case "Rich Player.vshost" : w = pr.MainWindowHandle.ToInt32() : SetForegroundWindow(w) : End Select : Next : Catch : End Try
                                            Dim xform As Form = Application.OpenForms.OfType(Of YouTubeBrowser)().Where(Function(frm) frm.Name = "YouTubeBrowser").SingleOrDefault
                                            xform.Focus()
                                            Dim web As Gecko.GeckoWebBrowser : For Each c As Control In xform.Controls : If c.GetType Is GetType(Gecko.GeckoWebBrowser) Then : web = c : End If : Next : web.Focus()
                                            MenuForm.SendClick(Nothing, WindowsInput.Native.VirtualKeyCode.VK_K)
                                        Else
                                            '......SAME.......
                                            If My.Settings.UseMediaKeys Then
                                                MenuForm.SendClick(Nothing, WindowsInput.Native.VirtualKeyCode.MEDIA_PLAY_PAUSE)
                                            Else
                                                PlayHotkeySub()
                                            End If
                                        End If
                                    Else
                                        '......SAME.......
                                        If My.Settings.UseMediaKeys Then
                                            MenuForm.SendClick(Nothing, WindowsInput.Native.VirtualKeyCode.MEDIA_PLAY_PAUSE)
                                        Else
                                            PlayHotkeySub()
                                        End If
                                    End If
                                Case Else

                            End Select

                        Case Else
                            Try
                                MyBase.WndProc(m)
                            Catch
                            End Try

                    End Select
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

        'Edit >> Global Hotkeys
        Public Sub BarGlobal_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_EditHotkeys.ItemClick
            EditHotkeys()
        End Sub
        Public Sub EditHotkeys()
            Dim xform As New hotkeysfrm
            xform.ShowDialog()
            If xform.DialogResult = DialogResult.OK Then

                'Play / Pause
                If xform.PlayPauseCombo.Text = "None" Then
                    My.Settings.PlayPauseKey = Keys.None
                    My.Settings.PlayPauseKeyCtrl = 0
                Else
                    If Not xform.PlayPauseCombo.Text = "" Then
                        Dim newkey As Integer = [Enum].Parse(GetType(Keys), (xform.PlayPauseCombo.Text.Split(" ").Last), False)
                        My.Settings.PlayPauseKey = newkey

                        If xform.PlayPauseCombo.Text.StartsWith("Ctrl") Then
                            My.Settings.PlayPauseKeyCtrl = 2
                        Else
                            My.Settings.PlayPauseKeyCtrl = 0
                        End If
                    End If
                End If
                hkr.Unregister(0)
                hkr.Register(My.Settings.PlayPauseKeyCtrl, My.Settings.PlayPauseKey).ToString()

                'Previous
                If xform.PreviousCombo.Text = "None" Then
                    My.Settings.PreviousKey = Keys.None
                    My.Settings.PreviousKeyCtrl = 0
                Else
                    If Not xform.PreviousCombo.Text = "" Then
                        Dim newkey As Integer = [Enum].Parse(GetType(Keys), (xform.PreviousCombo.Text.Split(" ").Last), False)

                        My.Settings.PreviousKey = newkey

                        If xform.PreviousCombo.Text.StartsWith("Ctrl") Then
                            My.Settings.PreviousKeyCtrl = 2
                        Else
                            My.Settings.PreviousKeyCtrl = 0
                        End If
                    End If
                End If
                hkr.Unregister(1)
                hkr.Register(My.Settings.PreviousKeyCtrl, My.Settings.PreviousKey).ToString()

                'Next
                If xform.NextCombo.Text = "None" Then
                    My.Settings.NextKey = Keys.None
                    My.Settings.NextKeyCtrl = 0
                Else
                    If Not xform.NextCombo.Text = "" Then
                        Dim newkey As Integer = [Enum].Parse(GetType(Keys), (xform.NextCombo.Text.Split(" ").Last), False)
                        My.Settings.NextKey = newkey

                        If xform.NextCombo.Text.StartsWith("Ctrl") Then
                            My.Settings.NextKeyCtrl = 2
                        Else
                            My.Settings.NextKeyCtrl = 0
                        End If
                    End If
                End If
                hkr.Unregister(2)
                hkr.Register(My.Settings.NextKeyCtrl, My.Settings.NextKey).ToString()

                'Stop
                If xform.StopCombo.Text = "None" Then
                    My.Settings.StopKey = Keys.None
                    My.Settings.StopKeyCtrl = 0
                Else
                    If Not xform.StopCombo.Text = "" Then
                        Dim newkey As Integer = [Enum].Parse(GetType(Keys), (xform.StopCombo.Text.Split(" ").Last), False)
                        My.Settings.StopKey = newkey

                        If xform.StopCombo.Text.StartsWith("Ctrl") Then
                            My.Settings.StopKeyCtrl = 2
                        Else
                            My.Settings.StopKeyCtrl = 0
                        End If
                    End If
                End If
                hkr.Unregister(3)
                hkr.Register(My.Settings.StopKeyCtrl, My.Settings.StopKey).ToString()

                'Skip Backwarsds
                If xform.SkipBackwardsCombo.Text = "None" Then
                    My.Settings.SkipBackwardsKey = Keys.None
                    My.Settings.SkipBackwardsKeyCtrl = 0
                Else
                    If Not xform.SkipBackwardsCombo.Text = "" Then
                        Dim newkey As Integer = [Enum].Parse(GetType(Keys), (xform.SkipBackwardsCombo.Text.Split(" ").Last), False)
                        My.Settings.SkipBackwardsKey = newkey

                        If xform.SkipBackwardsCombo.Text.StartsWith("Ctrl") Then
                            My.Settings.SkipBackwardsKeyCtrl = 2
                        Else
                            My.Settings.SkipBackwardsKeyCtrl = 0
                        End If
                    End If
                End If
                hkr.Unregister(4)
                hkr.Register(My.Settings.SkipBackwardsKeyCtrl, My.Settings.SkipBackwardsKey).ToString()

                'Skip Forward
                If xform.SkipForwardCombo.Text = "None" Then
                    My.Settings.SkipForwardKey = Keys.None
                    My.Settings.SkipForwardKeyCtrl = 0
                Else
                    If Not xform.SkipForwardCombo.Text = "" Then
                        Dim newkey As Integer = [Enum].Parse(GetType(Keys), (xform.SkipForwardCombo.Text.Split(" ").Last), False)
                        My.Settings.SkipForwardKey = newkey

                        If xform.SkipForwardCombo.Text.StartsWith("Ctrl") Then
                            My.Settings.SkipForwardKeyCtrl = 2
                        Else
                            My.Settings.SkipForwardKeyCtrl = 0
                        End If
                    End If
                End If
                hkr.Unregister(5)
                hkr.Register(My.Settings.SkipForwardKeyCtrl, My.Settings.SkipForwardKey).ToString()


                'AB Repeat
                'A
                If xform.ArCombo.Text = "None" Then
                    My.Settings.Arkey = Keys.None
                    My.Settings.ArKeyCtrl = 0
                    My.Settings.ArKeyAlt = 0
                Else
                    If Not xform.ArCombo.Text = "" Then
                        Dim newkey As Integer = [Enum].Parse(GetType(Keys), (xform.ArCombo.Text.Split(" ").Last), False)
                        My.Settings.Arkey = newkey

                        If xform.ArCombo.Text.StartsWith("Ctrl") Then
                            My.Settings.ArKeyCtrl = 2
                            If xform.ArCombo.Text.Contains("Alt") Then
                                My.Settings.ArKeyAlt = 1
                            End If
                        Else
                            My.Settings.ArKeyCtrl = 0
                        End If
                    End If
                End If
                hkr.Unregister(6)
                If xform.ArCombo.Text.Contains("Alt") Then
                    hkr.Register(My.Settings.ArKeyCtrl Or HotKeyRegistryClass.Modifiers.MOD_ALT, My.Settings.Arkey).ToString()
                Else
                    hkr.Register(My.Settings.ArKeyCtrl, My.Settings.Arkey).ToString()
                End If


                'B
                If xform.BrCombo.Text = "None" Then
                    My.Settings.Brkey = Keys.None
                    My.Settings.BrKeyCtrl = 0
                    My.Settings.BrKeyAlt = 0
                Else
                    If Not xform.BrCombo.Text = "" Then
                        Dim newkey As Integer = [Enum].Parse(GetType(Keys), (xform.BrCombo.Text.Split(" ").Last), False)
                        My.Settings.Brkey = newkey

                        If xform.BrCombo.Text.StartsWith("Ctrl") Then
                            My.Settings.BrKeyCtrl = 2
                            If xform.BrCombo.Text.Contains("Alt") Then
                                My.Settings.BrKeyAlt = 1
                            End If

                        Else
                            My.Settings.BrKeyCtrl = 0
                        End If
                    End If
                End If
                hkr.Unregister(7)
                If xform.BrCombo.Text.Contains("Alt") Then
                    hkr.Register(My.Settings.BrKeyCtrl Or HotKeyRegistryClass.Modifiers.MOD_ALT, My.Settings.Brkey).ToString()
                Else
                    hkr.Register(My.Settings.BrKeyCtrl, My.Settings.Brkey).ToString()
                End If

                'AB Reset
                If xform.ABrCombo.Text = "None" Then
                    My.Settings.ABrkey = Keys.None
                    My.Settings.ABrKeyCtrl = 0
                    My.Settings.ABrKeyAlt = 0

                Else
                    If Not xform.ABrCombo.Text = "" Then
                        Dim newkey As Integer = [Enum].Parse(GetType(Keys), (xform.ABrCombo.Text.Split(" ").Last), False)
                        My.Settings.ABrkey = newkey

                        If xform.ABrCombo.Text.StartsWith("Ctrl") Then
                            My.Settings.ABrKeyCtrl = 2
                            If xform.ABrCombo.Text.Contains("Alt") Then
                                My.Settings.ABrKeyAlt = 1
                            End If

                        Else
                            My.Settings.ABrKeyCtrl = 0
                        End If
                    End If
                End If
                hkr.Unregister(8)
                If xform.ABrCombo.Text.Contains("Alt") Then
                    hkr.Register(My.Settings.ABrKeyCtrl Or HotKeyRegistryClass.Modifiers.MOD_ALT, My.Settings.ABrkey).ToString()
                Else
                    hkr.Register(My.Settings.ABrKeyCtrl, My.Settings.ABrkey).ToString()
                End If

                'speed
                'Slow
                If xform.SlowCombo.Text = "None" Then
                    My.Settings.Slowkey = Keys.None
                    My.Settings.SlowKeyCtrl = 0
                Else
                    If Not xform.SlowCombo.Text = "" Then
                        Dim newkey As Integer = [Enum].Parse(GetType(Keys), (xform.SlowCombo.Text.Split(" ").Last), False)
                        My.Settings.Slowkey = newkey

                        If xform.SlowCombo.Text.StartsWith("Ctrl") Then
                            My.Settings.SlowKeyCtrl = 2
                            If xform.SlowCombo.Text.Contains("Alt") Then
                                My.Settings.SlowKeyAlt = 1
                            End If
                        Else
                            My.Settings.SlowKeyCtrl = 0
                        End If
                    End If
                End If
                hkr.Unregister(9)
                If xform.SlowCombo.Text.Contains("Alt") Then
                    hkr.Register(My.Settings.SlowKeyCtrl Or HotKeyRegistryClass.Modifiers.MOD_ALT, My.Settings.Slowkey).ToString()
                Else
                    hkr.Register(My.Settings.SlowKeyCtrl, My.Settings.Slowkey).ToString()
                End If

                'Fast
                If xform.FastCombo.Text = "None" Then
                    My.Settings.FastKey = Keys.None
                    My.Settings.FastKeyCtrl = 0
                Else
                    If Not xform.FastCombo.Text = "" Then
                        Dim newkey As Integer = [Enum].Parse(GetType(Keys), (xform.FastCombo.Text.Split(" ").Last), False)
                        My.Settings.FastKey = newkey

                        If xform.FastCombo.Text.StartsWith("Ctrl") Then
                            My.Settings.FastKeyCtrl = 2
                            If xform.FastCombo.Text.Contains("Alt") Then
                                My.Settings.FastKeyAlt = 1
                            End If
                        Else
                            My.Settings.FastKeyCtrl = 0
                        End If
                    End If
                End If
                hkr.Unregister(10)
                If xform.FastCombo.Text.Contains("Alt") Then
                    hkr.Register(My.Settings.FastKeyCtrl Or HotKeyRegistryClass.Modifiers.MOD_ALT, My.Settings.FastKey).ToString()
                Else
                    hkr.Register(My.Settings.FastKeyCtrl, My.Settings.FastKey).ToString()
                End If

                'Speed Reset to Normal
                If xform.SpeedNormCombo.Text = "None" Then
                    My.Settings.SpeedNormkey = Keys.None
                    My.Settings.SpeedNormCtrl = 0
                Else
                    If Not xform.SpeedNormCombo.Text = "" Then
                        Dim newkey As Integer = [Enum].Parse(GetType(Keys), (xform.SpeedNormCombo.Text.Split(" ").Last), False)
                        My.Settings.SpeedNormkey = newkey

                        If xform.SpeedNormCombo.Text.StartsWith("Ctrl") Then
                            My.Settings.SpeedNormCtrl = 2
                            If xform.SpeedNormCombo.Text.Contains("Alt") Then
                                My.Settings.SpeedNormAlt = 1
                            End If
                        Else
                            My.Settings.SpeedNormCtrl = 0
                        End If
                    End If
                End If
                hkr.Unregister(11)
                If xform.SpeedNormCombo.Text.Contains("Alt") Then
                    hkr.Register(My.Settings.SpeedNormCtrl Or HotKeyRegistryClass.Modifiers.MOD_ALT, My.Settings.SpeedNormkey).ToString()
                Else
                    hkr.Register(My.Settings.SpeedNormCtrl, My.Settings.SpeedNormkey).ToString()
                End If



                'Pitch
                'Down
                If xform.PitchDownCombo.Text = "None" Then
                    My.Settings.PitchDownKey = Keys.None
                    My.Settings.PitchDownKeyCtrl = 0
                Else
                    If Not xform.PitchDownCombo.Text = "" Then
                        Dim newkey As Integer = [Enum].Parse(GetType(Keys), (xform.PitchDownCombo.Text.Split(" ").Last), False)
                        My.Settings.PitchDownKey = newkey

                        If xform.PitchDownCombo.Text.StartsWith("Ctrl") Then
                            My.Settings.PitchDownKeyCtrl = 2
                            If xform.PitchDownCombo.Text.Contains("Alt") Then
                                My.Settings.PitchDownKeyAlt = 1
                            End If
                        Else
                            My.Settings.PitchDownKeyCtrl = 0
                        End If
                    End If
                End If
                hkr.Unregister(12)
                If xform.PitchDownCombo.Text.Contains("Alt") Then
                    hkr.Register(My.Settings.PitchDownKeyCtrl Or HotKeyRegistryClass.Modifiers.MOD_ALT, My.Settings.PitchDownKey).ToString()
                Else
                    hkr.Register(My.Settings.PitchDownKeyCtrl, My.Settings.PitchDownKey).ToString()
                End If

                'Up
                If xform.PitchUpCombo.Text = "None" Then
                    My.Settings.PitchUpKey = Keys.None
                    My.Settings.PitchUpKeyCtrl = 0
                Else
                    If Not xform.PitchUpCombo.Text = "" Then
                        Dim newkey As Integer = [Enum].Parse(GetType(Keys), (xform.PitchUpCombo.Text.Split(" ").Last), False)
                        My.Settings.PitchUpKey = newkey

                        If xform.PitchUpCombo.Text.StartsWith("Ctrl") Then
                            My.Settings.PitchUpKeyCtrl = 2
                            If xform.PitchUpCombo.Text.Contains("Alt") Then
                                My.Settings.PitchUpKeyAlt = 1
                            End If
                        Else
                            My.Settings.PitchUpKeyCtrl = 0
                        End If
                    End If
                End If
                hkr.Unregister(13)
                If xform.PitchUpCombo.Text.Contains("Alt") Then
                    hkr.Register(My.Settings.PitchUpKeyCtrl Or HotKeyRegistryClass.Modifiers.MOD_ALT, My.Settings.PitchUpKey).ToString()
                Else
                    hkr.Register(My.Settings.PitchUpKeyCtrl, My.Settings.PitchUpKey).ToString()
                End If

                'Pitch Reset
                If xform.PitchResetCombo.Text = "None" Then
                    My.Settings.PitchResetKey = Keys.None
                    My.Settings.PitchResetKeyCtrl = 0
                Else
                    If Not xform.PitchResetCombo.Text = "" Then
                        Dim newkey As Integer = [Enum].Parse(GetType(Keys), (xform.PitchResetCombo.Text.Split(" ").Last), False)
                        My.Settings.PitchResetKey = newkey

                        If xform.PitchResetCombo.Text.StartsWith("Ctrl") Then
                            My.Settings.PitchResetKeyCtrl = 2
                            If xform.PitchResetCombo.Text.Contains("Alt") Then
                                My.Settings.PitchResetKeyAlt = 1
                            End If
                        Else
                            My.Settings.PitchResetKeyCtrl = 0
                        End If
                    End If
                End If
                hkr.Unregister(14)
                If xform.PitchResetCombo.Text.Contains("Alt") Then
                    hkr.Register(My.Settings.PitchResetKeyCtrl Or HotKeyRegistryClass.Modifiers.MOD_ALT, My.Settings.PitchResetKey).ToString()
                Else
                    hkr.Register(My.Settings.PitchResetKeyCtrl, My.Settings.PitchResetKey).ToString()
                End If

                'Toggle Fullscreen
                If xform.FullscreenCombo.Text = "None" Then
                    My.Settings.KeyFullscreen = Keys.None
                    My.Settings.KeyFullscreenCtrl = 0
                Else
                    If Not xform.FullscreenCombo.Text = "" Then
                        Dim newkey As Integer = [Enum].Parse(GetType(Keys), (xform.FullscreenCombo.Text.Split(" ").Last), False)
                        My.Settings.KeyFullscreen = newkey

                        If xform.FullscreenCombo.Text.StartsWith("Ctrl") Then
                            My.Settings.KeyFullscreenCtrl = 2
                            If xform.FullscreenCombo.Text.Contains("Alt") Then
                                My.Settings.KeyFullscreenAlt = 1
                            End If
                        Else
                            My.Settings.KeyFullscreenCtrl = 0
                        End If
                    End If
                End If
                hkr.Unregister(16)
                If xform.FullscreenCombo.Text.Contains("Alt") Then
                    hkr.Register(My.Settings.KeyFullscreenCtrl Or HotKeyRegistryClass.Modifiers.MOD_ALT, My.Settings.KeyFullscreen).ToString()
                Else
                    hkr.Register(My.Settings.KeyFullscreenCtrl, My.Settings.KeyFullscreen).ToString()
                End If




                My.Settings.Save()
                'SaveINISettings 'SaveSettings()
            End If

        End Sub
        'Temp. Disable
        Dim HotkeysReset As Boolean = False
        Public Sub BarCheckItem1_CheckedChanged(sender As Object, e As XtraBars.ItemClickEventArgs) Handles BarCheckbox_DisableHotkeys.CheckedChanged
            If BarCheckbox_DisableHotkeys.Checked = False Then
                For i As Integer = 0 To 69
                    hkr.Unregister(i)
                Next
                '  UnRegisterHamburgerMenu()
                Try
                    hkr.Unregister(68)
                Catch ex As Exception
                End Try

                Setup_Hotkeys()
                RegisterHamburgerMenu()

            Else
                Try

                    For i As Integer = 0 To 69
                        hkr.Unregister(i)
                    Next
                    '  UnRegisterHamburgerMenu()
                    Try
                        hkr.Unregister(68)
                    Catch ex As Exception
                    End Try
                    HotkeysReset = True

                    RegisterBlankHotkeys()
                    RegisterHamburgerMenu()

                Catch
                    MyMsgBox.Show("Error 0x33: Unable to unregister hotkeys", "", True)
                End Try
            End If
        End Sub

#End Region


#Region " PLAYERS"
#Region " Declarations"
        Public PlaylistIsReady As Boolean = False
        Public VLC_Artwork As New PictureBox
#End Region
#Region " Check if Video"
        Public Sub CheckIfVideo()

            Try : If Not UsingSpotify Then

                    Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next
                    Try
                        If Playlist.Rows.Count = Nothing Then Return
                    Catch : End Try
                    Dim RowCount As Integer = Playlist.RowCount

                    If RowCount = Nothing Then Return
                    If RowCount = 0 Then
                        PlaylistIsReady = False
                    Else
                        If Playlist.Item(0, 0).Value = "" Then
                            PlaylistIsReady = False
                        Else
                            PlaylistIsReady = True
                        End If
                    End If
                    If AddingPlaylist Then Return
                    Dim Row As Integer = Playlist.CurrentCell.RowIndex
                    If Row = Nothing Then
                        Row = 0
                        Playlist.CurrentCell = Playlist(0, Row)
                    End If
                    If PlaylistIsReady Then
                        Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                        If RowCount = 0 Then Return
                        If Not SongFilename.EndsWith(".wav") And Not SongFilename.EndsWith(".ogg") And Not SongFilename.EndsWith(".flac") And Not SongFilename.EndsWith(".aiff") And Not SongFilename.EndsWith(".aac") And Not SongFilename.EndsWith(".ape") And Not SongFilename.EndsWith(".m4a") And Not SongFilename.EndsWith(".wma") And Not SongFilename.EndsWith(".mp3") And Not SongFilename.EndsWith(".cda") Then
                            IsVideo = True
                        Else
                            IsVideo = False
                        End If
                    End If
                    If IsVideo Then
                        PictureBoxSpec.Visible = False
                    Else
                        PictureBoxSpec.Visible = True
                    End If
                    If Playlist.Item(2, Row).Value = "Radio Station" Then
                        IsRadioStation = True
                    Else
                        IsRadioStation = False
                    End If
                Else : Return : End If
            Catch : End Try


            Try
                If IsVideo Then
                    Dim Fullscreen_but As New PictureBox
                    Fullscreen_but.Name = "Fullscreen_but"
                    Me.Controls.Add(Fullscreen_but)
                    Fullscreen_but.Size = New Size(25, 25)
                    Fullscreen_but.BackgroundImage = My.Resources.Browser_Fullscreen  '.BackColor = Color.Turquoise
                    Fullscreen_but.BackgroundImageLayout = ImageLayout.Zoom
                    Fullscreen_but.Location = New Point(VlcPlayer.Location.X + VlcPlayer.Width - Fullscreen_but.Width - 2, _
                                                        VlcPlayer.Location.Y + 2)
                    Fullscreen_but.BringToFront()
                    Fullscreen_but.Visible = True
                    '    AddHandler Fullscreen_but.Click, AddressOf FullScreenVLC
                    '    AddHandler Fullscreen_but.MouseClick, AddressOf FullScreenVLC
                    AddHandler Fullscreen_but.MouseUp, AddressOf FullScreenVLC

                Else
                    For Each c As Control In Me.Controls
                        If c.Name = "Fullscreen_but" Then
                            c.Dispose()
                        End If
                    Next
                End If
            Catch ex As Exception

            End Try

            Try
                Return
                VlcPlayer.Controls.Add(VLC_Artwork)
                VLC_Artwork.BackColor = Color.Transparent
                VLC_Artwork.BackgroundImage = ChangeOpacity(My.Resources.Blank, 0.1)
                VLC_Artwork.BackgroundImageLayout = ImageLayout.Zoom
                ' VLC_Artwork.BackColor = Color.FromArgb(1, 0, 0, 0)
                VLC_Artwork.Dock = DockStyle.Fill
                VLC_Artwork.Location = VlcPlayer.Location
                VLC_Artwork.Size = VlcPlayer.Size
                VLC_Artwork.BringToFront()
                VLC_Artwork.Visible = True
                AddHandler VLC_Artwork.MouseUp, AddressOf VLC_PlayPause
                ' AddHandler VLC_Artwork.MouseDown, AddressOf PictureBox1_MouseDown
                ' AddHandler VLC_Artwork.MouseMove, AddressOf PictureBox1_MouseMove
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try

        End Sub
        Public Sub VLC_PlayPause()
            If VlcPlayer.playlist.isPlaying Then
                VlcPlayer.playlist.togglePause()
            Else
                VlcPlayer.playlist.play()
            End If


        End Sub
     

#End Region


#Region " VLC   |   Media Status"

        'Media Playing
        Public Sub VLCplayer_MediaPlayerPlaying(sender As Object, e As EventArgs)
            If VLC_installed = False Then
                Dim f As New Font("Segoe UI", 15, FontStyle.Regular)
                Artwork.BackgroundImage = DrawText("Videos unsupported." + Environment.NewLine + "Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)

                Return
            End If
            If VlcPlayer.playlist.isPlaying Then
                'If VlcPlayer.Playlist.isPlaying Then 'If VlcPlayer.playlist.isPlaying Then
                Timer_Seek.Start()
                VlcPlayer.Visible = True
                If playhover = True Then
                    If Playdown = False Then
                        But_PlayPause.BackgroundImage = PauseHoverImage
                    End If
                Else
                    If Playdown = False Then
                        But_PlayPause.BackgroundImage = PauseImage
                    End If
                End If

                But_Stop.BackgroundImage = StopImage
                But_Stop.BackgroundImage = StopImage
                But_Next.BackgroundImage = NextImage
                Prevrwdisabled = False
                But_Previous.BackgroundImage = PreviousImage
                NextButdisabled = False
                But_Next.BackgroundImage = NextImage
                Timer_Seek.Enabled = True
                VLCplayer_buffering = False
                isVLCplaying = True
                IsVideo = True
                trackbar_Pitch2.Enabled = False
                But_PitchDown.Enabled = False
                But_PitchUp.Enabled = False
                But_PitchDown.Visible = False
                But_PitchUp.Visible = False
                trackBar_Speed2.Enabled = False
                Refresh_VLC_volspeed()
            Else
                isVLCplaying = False
                Timer_Seek.Stop()
            End If
            Refresh_VLC_volspeed()

        End Sub

        'Media Paused
        Public Sub VLCplayer_MediaPlayerPaused(sender As Object, e As EventArgs)
            If VLC_installed = False Then
                Dim f As New Font("Segoe UI", 15, FontStyle.Regular)
                Artwork.BackgroundImage = DrawText("Videos unsupported." + Environment.NewLine + "Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)

                Return
            End If
            If Not VlcPlayer.playlist Is Nothing Then
                'If Not VlcPlayer.playlist.items Is Nothing Then
                IsVideo = True
            End If
            But_PlayPause.BackgroundImage = PlayImage
            But_PlayPause.BackgroundImage = PlayImage

            SetTaskbarState(TaskbarState.Paused)



            Timer_Seek.Stop()

            If playhover = True Then
                If Playdown = False Then
                    But_PlayPause.BackgroundImage = PlayHoverImage
                End If
            Else
                If Playdown = False Then
                    But_PlayPause.BackgroundImage = PlayImage
                End If
            End If

           Refresh_VLC_volspeed
        End Sub

        'Media Finished
        Public Sub VLCplayer_MediaPlayerEndReached(sender As Object, e As EventArgs)
            If VLC_installed = False Then
                Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                Artwork.BackgroundImage = DrawText("Videos unsupported. Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)

                Return
            End If
            MediaFinishedSub()

            ResetThumbnail()
            Reset_AB_Repeat()
            Refresh_VLC_volspeed()
        End Sub

        'Media Buffering
        Public Shared IsBuffering As Boolean = False
        ' Public Sub VLCplayer_MediaPlayerBuffering(sender As Object, e As Vlc.DotNet.Core.VlcMediaPlayerBufferingEventArgs)
        Public Sub VLCplayer_MediaPlayerBuffering(sender As Object, e As AxAXVLC.DVLCEvents_MediaPlayerBufferingEvent)
            If VLC_installed = False Then
                Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                Artwork.BackgroundImage = DrawText("Videos unsupported. Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)

                Return
            End If

            '   LabelVLCBuffering.Text = "Buffering"
            VLCplayer_buffering = True
            If VlcPlayer.playlist.isPlaying Then
                'If VlcPlayer.Playlist.isPlaying Then 'If VlcPlayer.playlist.isPlaying Then
                DoGetMetaInfo = True

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
                Dim SongFilename As String
                Try
                    SongFilename = Playlist.Item(6, Playlist.CurrentRow.Index).Value.ToString
                Catch
                End Try
                Try
                    If _title.Contains("C:\") Then   ' If _title.Length = 3 Or 
                        Label_SongName.Text = Path.GetFileNameWithoutExtension(SongFilename)
                        Setup_Time_Labels()
                        If Label_SongName.Text = "" Then Label_SongName.Text = "Unknown Title"
                    End If
                    If Label_Album.Text = "" Then Label_Album.Text = "Unknown Album"
                    If Label_Artist.Text = "" Then Label_Album.Text = "Unknown Artist"
                Catch
                End Try

            Else
                Timer_Meta_and_Artwork.Stop()
            End If


            If VLCplayer_firstbuffer Then
                VLCplayer_LoadChapters()

                ReApplySubtitles()
                ReApplyAudioTracks()
            End If


          Refresh_VLC_volspeed
        End Sub

        'Media Opening
        Public Sub VLCplayer_MediaPlayerOpening(sender As Object, e As EventArgs)

            If VLC_installed = False Then
                Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                Artwork.BackgroundImage = DrawText("Videos unsupported. Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)

                Return
            End If
            IsVideo = True
            VLCplayer_firstbuffer = True
            If BarCheckBox_AllowSaveItemPosition.Checked Then
                Dim TabIndex As Integer = PlaylistTabs.SelectedTabPageIndex

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
                Dim SongPosition As String = Playlist.Item(5, Row).Value.ToString
                If SongPosition <> 0 Then
                    VlcPlayer.input.position = SongPosition
                End If
            End If


            RenderWaveForm()
            Reset_AB_Repeat()
            A_Repeat()
            If Panellyrics.Visible = True Then
                Panellyrics.AutoScrollPosition = New Point(0, 0)
            End If
            If firsttime_TimeLabel = False Then
                firsttime_TimeLabel = True
                timelabel.Width -= 5
                timelabel.Left += 2
            End If
            VLCmediaOpened = True

            NeedResizeRefresh = True
            RefreshApp()
            IsBuffering = True
            Timer_Meta_and_Artwork.Start()
            DoGetMetaInfo = True
            GetMetaInfo()


        Refresh_VLC_volspeed


        End Sub

        'Media Stopped
        Public Sub VLCplayer_MediaPlayerStopped()
            If VLC_installed = False Then
                Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                Artwork.BackgroundImage = DrawText("Videos unsupported. Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)

                Return
            End If
            If VlcPlayer.Visible = True Then
                VlcPlayer.Visible = False
            End If
            ResetThumbnail()
            VLCmediaOpened = False
            Refresh_VLC_volspeed()
        End Sub

        'VLC Visible Changed
        Public Sub VLCplayer_VisibleChanged()
            If VLC_installed = False Then
                Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                Artwork.BackgroundImage = DrawText("Videos unsupported. Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)

                Return
            End If
            If VlcPlayer.Visible Then

            End If
            Refresh_VLC_volspeed()
        End Sub

        Public Sub Refresh_VLC_volspeed()
            VlcPlayer.volume = TrackBar_PlayerVol2.Value
            '  Timer_Rate.Start()

            Try
                If Label_SpeedTextbox.Text = "0.0" Then Label_SpeedTextbox.Text = "1.0"

                Try
                    VlcPlayer.input.rate = CDbl(Label_SpeedTextbox.Text.Replace("%", "")) ' .Substring(0, Label_SpeedTextbox.Text.IndexOf("."))) 'trackBar_Speed2.Value
                Catch ex As Exception
                    VlcPlayer.input.rate = CDbl(Label_SpeedTextbox.Text.Replace("%", "").Substring(0, Label_SpeedTextbox.Text.IndexOf("." + 1))) 'trackBar_Speed2.Value
                End Try
                'VlcPlayer.input.rate = CDbl(Label_SpeedTextbox.Text.Replace("%", "").Substring(0, Label_SpeedTextbox.Text.IndexOf("."))) 'trackBar_Speed2.Value

            Catch ex As Exception

            End Try
        End Sub
 

#End Region
#Region " VLC   |   Chapter Marks"

        'Load Chapters
        Public Sub VLCplayer_LoadChapters()
            If VLC_installed = False Then
                Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                Artwork.BackgroundImage = DrawText("Videos unsupported. Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)

                Return
            End If
            If VlcPlayer.input.chapter.count > 1 Then
                VLCChapterMarks.Visible = True
                VLCChapterMarks.Items.Clear()
                Dim Title As Integer
                Title = VlcPlayer.input.title.track  '= VlcPlayer.title.track
                'For i As Integer = 0 To Title - 1 'VlcPlayer.Input.Chapter.countForTitle(Title) - 1
                For i As Integer = 0 To VlcPlayer.input.chapter.countForTitle(Title) - 1
                    VLCChapterMarks.Items.Add(VlcPlayer.input.chapter.description(Title, i))
                    'VLCChapterMarks.Items.Add(VlcPlayer.Input.Chapter.Current)
                Next
                VLCplayer_firstbuffer = False
            Else
                VLCplayer_firstbuffer = True
                VLCChapterMarks.Visible = False
            End If
            NeedResizeRefresh = True
            RefreshApp()

            PlaylistTabs.MakePageVisible(PlaylistTabs.SelectedTabPage)
        End Sub
        Public Sub VLCplayer_TitleChanged()
            If VLC_installed = False Then
                Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                Artwork.BackgroundImage = DrawText("Videos unsupported. Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)

                Return
            End If
            'If VlcPlayer.title.count > 1 Then
            VLCplayer_LoadChapters()
            '  End If
            NeedResizeRefresh = True
            RefreshApp()
        End Sub

        'VLC Chapter Marks
        Public Sub VLCChapterMarks_Click()
            If VLC_installed = False Then
                Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                Artwork.BackgroundImage = DrawText("Videos unsupported. Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)

                Return
            End If
            VLCChapterMarks_IsClicking = True
            VlcPlayer.input.chapter.track = VLCChapterMarks.SelectedIndex
        End Sub
        Public Sub VLCChapterMarks_MouseDown()
            VLCChapterMarks_IsClicking = True
        End Sub
        Public Sub VLCChapterMarks_MouseUp()
            VLCChapterMarks_IsClicking = False
        End Sub
        Public Sub VLCChapterMarks_SelectedIndexChanged()

        End Sub
        Public Sub VLCChapterMarks_VisiblityChanged()


            If My.Settings.MiniModeOn Then
                If VLCChapterMarks.Visible Then
                    VLCChapterMarks.Dock = DockStyle.None
                    PlaylistTabs.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Bottom
                    PlaylistTabs.Size = New Size(75, Me.Height - 8)
                    VLCChapterMarks.Location = New Point(404, 4)
                    PlaylistTabs.Dock = DockStyle.None
                    PlaylistTabs.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Bottom
                    PlaylistTabs.Visible = True
                    PlaylistTabs.Size = New Size(256, Me.Height - 8)
                    PlaylistTabs.Location = New Point(479, 4)
                Else
                    PlaylistTabs.Dock = DockStyle.None
                    PlaylistTabs.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Bottom
                    PlaylistTabs.Visible = True
                    PlaylistTabs.Size = New Size(331, Me.Height - 8)
                    PlaylistTabs.Location = New Point(404, 4)
                End If

            Else
                If VLCChapterMarks.Visible Then
                    VLCChapterMarks.Dock = DockStyle.Left
                    PlaylistTabs.Dock = DockStyle.Right
                    PlaylistTabs.Width = Splitter.Panel2.Width - VLCChapterMarks.Width
                Else
                    VLCChapterMarks.Dock = DockStyle.None
                    PlaylistTabs.Dock = DockStyle.Fill
                End If
            End If
            NeedResizeRefresh = True


            PlaylistTabs.MakePageVisible(PlaylistTabs.SelectedTabPage)
        End Sub


#End Region

#Region " VLC   |   Subtitles"
        'Enable built-in subtitles for certain videos
        Public Shared SubtitleMenu As New DevExpress.XtraBars.BarSubItem
        Public Shared Subtitle1But As New DevExpress.XtraBars.BarCheckItem
        Public Shared Subtitle2But As New DevExpress.XtraBars.BarCheckItem
        Public Shared Subtitle3But As New DevExpress.XtraBars.BarCheckItem
        Public Shared Subtitle4But As New DevExpress.XtraBars.BarCheckItem

        Public Shared Sub RefreshSubtitles()
            Try
                Subtitle1But.Caption = Environment.NewLine & VlcPlayer.subtitle.description(0)
                'Subtitle1But.Caption = VlcPlayer.SubTitles.All(0).Name
            Catch
            End Try
            Try
                Subtitle2But.Caption = Environment.NewLine & VlcPlayer.subtitle.description(1)
                ' Subtitle1But.Caption = VlcPlayer.SubTitles.All(1).Name
            Catch
            End Try
            Try
                Subtitle3But.Caption = Environment.NewLine & VlcPlayer.subtitle.description(2)
                'Subtitle1But.Caption = VlcPlayer.SubTitles.All(2).Name
            Catch
            End Try
            Try
                Subtitle4But.Caption = Environment.NewLine & VlcPlayer.subtitle.description(3)
                ' Subtitle1But.Caption = VlcPlayer.SubTitles.All(3).Name
            Catch
            End Try
        End Sub

        Public Shared Sub SubtitleMenu_Clear()
            SubtitlesAdded = False
            Subtitle1But.Caption = "Not Available"
            Subtitle2But.Caption = "Not Available"
            Subtitle3But.Caption = "Not Available"
            Subtitle4But.Caption = "Not Available"
        End Sub
        Public Shared Sub ReApplySubtitles()
            Try
                If Subtitle1But.Checked Then
                    If VlcPlayer.subtitle.count <> 0 Then
                        VlcPlayer.subtitle.track = 0
                        ' VlcPlayer.SubTitles.Current = VlcPlayer.SubTitles.All(0)
                    Else
                        ' VlcPlayer.SubTitles.Current = VlcPlayer.SubTitles.All()
                        VlcPlayer.subtitle.track = -1
                    End If
                End If
            Catch
            End Try
            Try
                If Subtitle2But.Checked Then
                    VlcPlayer.subtitle.track = 1
                    '  VlcPlayer.SubTitles.Current = VlcPlayer.SubTitles.All(1)
                End If
            Catch
            End Try
            Try
                If Subtitle3But.Checked Then
                    VlcPlayer.subtitle.track = 2
                    ' VlcPlayer.SubTitles.Current = VlcPlayer.SubTitles.All(2)
                End If
            Catch
            End Try
            Try
                If Subtitle4But.Checked Then
                    VlcPlayer.subtitle.track = 3
                    ' VlcPlayer.SubTitles.Current = VlcPlayer.SubTitles.All(3)
                End If
            Catch
            End Try
        End Sub


        'Checkbuttons
        Public Shared Sub Subtitle1But_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
            Try
                Subtitle2But.Checked = False
                Subtitle3But.Checked = False
                Subtitle4But.Checked = False
                If Subtitle1But.Checked Then
                    If VlcPlayer.subtitle.count <> 0 Then
                        VlcPlayer.subtitle.track = 0
                        ' VlcPlayer.SubTitles.Current = VlcPlayer.SubTitles.All(0)
                    Else
                        VlcPlayer.subtitle.track = -1
                    End If
                    Subtitle2But.Checked = False
                    Subtitle3But.Checked = False
                    Subtitle4But.Checked = False
                End If
            Catch
                '     VlcPlayer.subtitle.track = 0
            End Try

        End Sub
        Public Shared Sub Subtitle2But_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
            Try
                Subtitle1But.Checked = False
                Subtitle3But.Checked = False
                Subtitle4But.Checked = False
                If Subtitle2But.Checked Then
                    ' VlcPlayer.SubTitles.Current = VlcPlayer.SubTitles.All(1)
                    VlcPlayer.subtitle.track = 1
                    Subtitle1But.Checked = False
                    Subtitle3But.Checked = False
                    Subtitle4But.Checked = False
                End If
            Catch

            End Try

        End Sub
        Public Shared Sub Subtitle3But_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
            Try
                Subtitle1But.Checked = False
                Subtitle2But.Checked = False
                Subtitle4But.Checked = False
                If Subtitle3But.Checked Then
                    VlcPlayer.subtitle.track = 2
                    '    VlcPlayer.SubTitles.Current = VlcPlayer.SubTitles.All(2)
                    Subtitle1But.Checked = False
                    Subtitle2But.Checked = False
                    Subtitle4But.Checked = False
                End If
            Catch

            End Try

        End Sub
        Public Shared Sub Subtitle4But_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
            Try
                Subtitle1But.Checked = False
                Subtitle2But.Checked = False
                Subtitle3But.Checked = False
                If Subtitle4But.Checked Then
                    VlcPlayer.subtitle.track = 3
                    '  VlcPlayer.SubTitles.Current = VlcPlayer.SubTitles.All(3)
                    Subtitle1But.Checked = False
                    Subtitle2But.Checked = False
                    Subtitle3But.Checked = False
                End If
            Catch

            End Try

        End Sub


#End Region
#Region " VLC   |   Audio"
        'Enable built-in Audios for certain videos
        Dim AudioMenu As New DevExpress.XtraBars.BarSubItem
        Dim Audio1But As New DevExpress.XtraBars.BarCheckItem
        Dim Audio2But As New DevExpress.XtraBars.BarCheckItem
        Dim Audio3But As New DevExpress.XtraBars.BarCheckItem
        Dim Audio4But As New DevExpress.XtraBars.BarCheckItem

        Public Sub RefreshAudios()
            Try
                ' Audio1But.Caption = VlcPlayer.Audio.description(0)
                Audio1But.Caption = Environment.NewLine & VlcPlayer.audio.Tracks.All(0).Name
            Catch
            End Try
            Try
                ' Audio2But.Caption = VlcPlayer.Audio.description(1)
                Audio2But.Caption = Environment.NewLine & VlcPlayer.audio.Tracks.All(1).Name
            Catch
            End Try
            Try
                'Audio3But.Caption = VlcPlayer.Audio.description(2)
                Audio3But.Caption = Environment.NewLine & VlcPlayer.audio.Tracks.All(2).Name
            Catch
            End Try
            Try
                ' Audio4But.Caption = VlcPlayer.Audio.description(3)
                Audio4But.Caption = Environment.NewLine & VlcPlayer.audio.Tracks.All(3).Name
            Catch
            End Try
        End Sub

        Public Sub AudioMenu_Clear()
            AudiosAdded = False
            Audio1But.Caption = "Not Available"
            Audio2But.Caption = "Not Available"
            Audio3But.Caption = "Not Available"
            Audio4But.Caption = "Not Available"
        End Sub
        Public Sub ReApplyAudioTracks()
            Try
                If Audio1But.Checked Then
                    ' VlcPlayer.Audio.track = 0
                    VlcPlayer.audio.Tracks.Current = VlcPlayer.audio.Tracks.All(0)

                End If
            Catch
            End Try
            Try
                If Audio2But.Checked Then
                    ' VlcPlayer.audio.track = 1
                    VlcPlayer.audio.Tracks.Current = VlcPlayer.audio.Tracks.All(1)
                End If
            Catch
            End Try
            Try
                If Audio3But.Checked Then
                    ' VlcPlayer.Audio.track = 2
                    VlcPlayer.audio.Tracks.Current = VlcPlayer.audio.Tracks.All(2)
                End If
            Catch
            End Try
            Try
                If Audio4But.Checked Then
                    'VlcPlayer.Audio.track = 3
                    VlcPlayer.audio.Tracks.Current = VlcPlayer.audio.Tracks.All(3)
                End If
            Catch
            End Try
        End Sub

        'Checkbuttons
        Public Sub Audio1But_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
            Try
                If Audio1But.Checked Then
                    ' VlcPlayer.Audio.track = 0
                    VlcPlayer.audio.Tracks.Current = VlcPlayer.audio.Tracks.All(0)
                    Audio2But.Checked = False
                    Audio3But.Checked = False
                    Audio4But.Checked = False
                End If
            Catch

            End Try

        End Sub
        Public Sub Audio2But_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
            Try
                If Audio2But.Checked Then
                    ' VlcPlayer.Audio.track = 1
                    VlcPlayer.audio.Tracks.Current = VlcPlayer.audio.Tracks.All(1)
                    Audio1But.Checked = False
                    Audio3But.Checked = False
                    Audio4But.Checked = False
                End If
            Catch

            End Try

        End Sub
        Public Sub Audio3But_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
            Try
                If Audio3But.Checked Then
                    ' VlcPlayer.Audio.track = 2
                    VlcPlayer.audio.Tracks.Current = VlcPlayer.audio.Tracks.All(2)
                    Audio1But.Checked = False
                    Audio2But.Checked = False
                    Audio4But.Checked = False
                End If
            Catch

            End Try

        End Sub
        Public Sub Audio4But_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
            Try
                If Audio4But.Checked Then
                    '  VlcPlayer.audio.track = 3
                    VlcPlayer.audio.Tracks.Current = VlcPlayer.audio.Tracks.All(3)
                    Audio1But.Checked = False
                    Audio2But.Checked = False
                    Audio3But.Checked = False
                End If
            Catch

            End Try

        End Sub


#End Region
#Region " VLC   |   Fullscreen"
        Public Shared IsFullscreen As Boolean = False

        Public Sub FullScreenVLC()
            Try
                If IsVideo Then
                    VlcPlayer.video.toggleFullscreen()
                End If

            Catch ex As Exception

            End Try
        End Sub
        Private Sub BarBut_FullScreen_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_FullScreen.ItemClick
            FullScreenVLC()
        End Sub

        Private Sub Timer_Fullscreen_Tick(sender As Object, e As EventArgs) Handles Timer_Fullscreen.Tick
            For Each c As Control In Me.Controls
                If c.Name = "Fullscreen_but" Then
                    c.BringToFront()
                End If
            Next
        End Sub

#End Region



#Region " All Players    |   Media Finished"

        'Media Finished
        Public Sub AudioPlayer_EndReached()
            MediaFinishedSub()
            Reset_AB_Repeat()
            Refresh_VLC_volspeed()
        End Sub

        'Media Finished Code
        Public Sub MediaFinishedSub()
            Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next : Dim Row As Integer = Playlist.CurrentCell.RowIndex : Dim RowCount As Integer = Playlist.RowCount

            If Playlist.Item(2, Row).Value = "YouTube Video" Then Return
            If Playlist.Item(2, Row).Value = "Radio Station" Then Return

            If BarCheckBox_ViewLyrics.Checked Then
                If BarCheckBox_CustomizeLyrics.Checked Then
                    IO.File.WriteAllText(TempLyrics, LabelLyrics.Text)
                    Dim Lyrics As String = LabelLyrics.Text
                    Dim TempLyricsSaved As Integer = IO.File.ReadAllText(TempLyrics).Length
                    Try
                        Dim CurrentLyricsSaved As Integer = IO.File.ReadAllText(SelectedSong_path_).Length
                        If Not TempLyricsSaved = CurrentLyricsSaved Then
                            IO.File.WriteAllText(SelectedSong_path_, Lyrics)

                        End If
                    Catch ex As Exception
                        IO.File.WriteAllText(SelectedSong_path_, Lyrics)

                    End Try

                End If
            End If

            If Panellyrics.Visible = True Then
                Panellyrics.AutoScrollPosition = New Point(0, 0)
            End If

            Select Case IsShuffle
                Case True                                       'Shuffle On
                    MediaFinishedShuffleOn()
                Case Else                                       'Shuffle Off
                    Select Case repeat
                        Case True
                            Select Case repeatOne
                                Case True                       'Repeat One
                                    MediaFinishedRepeatOne()
                                Case Else                       'Repeat All
                                    MediaFinishedRepeatAll()
                            End Select
                        Case Else                               'Repeat None
                            MediaFinishedRepeatNone()
                    End Select
            End Select

            If IsVideo Then
                DoGetMetaInfo = True
                GetMetaInfo()
                If VLC_installed Then
                    VlcPlayer.Visible = True
                End If
                Artwork.Visible = False

                Dim SongFilename As String
                Try
                    SongFilename = Playlist.Item(6, Playlist.CurrentRow.Index).Value.ToString
                Catch
                End Try
                Try
                    If _title.Contains("C:\") Then '_title.Length = 3 Or
                        Label_SongName.Text = Path.GetFileNameWithoutExtension(SongFilename)
                        Setup_Time_Labels()
                        If Label_SongName.Text = "" Then Label_SongName.Text = "Unknown Title"
                    End If
                    If Label_Album.Text = "" Then Label_Album.Text = "Unknown Album"
                    If Label_Artist.Text = "" Then Label_Album.Text = "Unknown Artist"
                Catch
                End Try
            Else
                Timer_Meta_and_Artwork.Start()
            End If
            If AudioPlayer.Instance.GetStreamStatus = BASSActive.BASS_ACTIVE_PLAYING Then
                But_PlayPause.BackgroundImage = PauseImage
                Timer_Seek.Start()
            Else
                But_PlayPause.BackgroundImage = PlayImage
            End If
            RefreshSkin()
            SubtitleMenu_Clear()
            AudioMenu_Clear()


        End Sub

        Public Sub MediaFinishedShuffleOn()


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
            Dim random As New System.Random
            Dim Index As System.Int32 = random.Next(0, RowCount)
            Row = Index
            Playlist.CurrentCell = Playlist(0, Row)
            If My.Settings.PlayFavorites Then
                Dim PassedOnce As Boolean = False
                Do
                    If Row = RowCount - 1 Then
                        If PassedOnce Then
                            If Playlist.CurrentCell.RowIndex = Playlist.RowCount - 1 Then
                                Row = 0
                            Else
                                Row = Playlist.CurrentCell.RowIndex + 1
                            End If
                            Exit Do
                        End If

                        If repeat Then
                            Row = 0
                            ' Playlist.CurrentCell = Playlist(0, Row)

                        End If
                        PassedOnce = True
                    Else
                        Row += 1
                        ' Playlist.CurrentCell = Playlist(0, Row)
                    End If

                Loop Until Playlist.Rows(Row).Cells(4).Value = "True"
                Playlist.CurrentCell = Playlist(0, Row)

            End If
            Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
            CheckIfVideo()
            If IsVideo Then
                If VLC_installed Then

                    VLCclearPlaylists()
                    ' VlcPlayer.SetMedia(New IO.FileInfo (SongFilename)) 
                    'VlcPlayer.Playlist.Play() 
                    VlcPlayer.playlist.play()
                    DoGetMetaInfo = True
                    GetMetaInfo()
                    VlcPlayer.Visible = True
                    Artwork.Visible = False
                    Try
                        SongFilename = Playlist.Item(6, Playlist.CurrentRow.Index).Value.ToString
                    Catch
                    End Try
                    Try
                        If _title.Contains("C:\") Then '_title.Length = 3 Or 
                            Label_SongName.Text = Path.GetFileNameWithoutExtension(SongFilename)
                            Setup_Time_Labels()
                            If Label_SongName.Text = "" Then Label_SongName.Text = "Unknown Title"
                        End If
                        If Label_Album.Text = "" Then Label_Album.Text = "Unknown Album"
                        If Label_Artist.Text = "" Then Label_Album.Text = "Unknown Artist"
                    Catch
                    End Try
                Else
                    Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                    Artwork.BackgroundImage = DrawText("Videos unsupported. Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)
                End If
            Else
                AudioPlayer.Instance.ResetTrackList()
                Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                AudioPlayer.Instance.TrackList.Tracks.Add(track)
                New_Play()
                Timer_Meta_and_Artwork.Start()
            End If
        End Sub
        Public Sub MediaFinishedRepeatOne()


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
            'Playlist.CurrentCell = Playlist(0, Row)

            CheckIfVideo()
            Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
            If IsVideo Then
                If VLC_installed Then
                    VLCclearPlaylists()
                    VlcPlayer.playlist.add("file:///" & SongFilename)
                    '  VlcPlayer.SetMedia(New IO.FileInfo (SongFilename))
                    'VlcPlayer.Playlist.Play() 
                    ' VlcPlayer.Playlist.Stop() 

                    VlcPlayer.playlist.play()
                    VlcPlayer.playlist.stop()
                    VlcPlayer.playlist.play()
                    DoGetMetaInfo = True
                    GetMetaInfo()
                    VlcPlayer.Visible = True
                    Artwork.Visible = False


                    Try
                        SongFilename = Playlist.Item(6, Playlist.CurrentRow.Index).Value.ToString
                    Catch
                    End Try


                    Try
                        If _title.Contains("C:\") Then ' _title.Length = 3 Or 
                            Label_SongName.Text = Path.GetFileNameWithoutExtension(SongFilename)
                            Setup_Time_Labels()
                            If Label_SongName.Text = "" Then Label_SongName.Text = "Unknown Title"
                        End If
                        If Label_Album.Text = "" Then Label_Album.Text = "Unknown Album"
                        If Label_Artist.Text = "" Then Label_Album.Text = "Unknown Artist"
                    Catch
                    End Try
                Else
                    Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                    Artwork.BackgroundImage = DrawText("Videos unsupported. Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)
                End If

            Else
                AudioPlayer.Instance.ResetTrackList()
                Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                AudioPlayer.Instance.TrackList.Tracks.Add(track)
                New_Play()

            End If
        End Sub
        Public Sub MediaFinishedRepeatAll()


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

            If RowCount > 1 Then
                If My.Settings.PlayFavorites Then
                    Dim PassedOnce As Boolean = False
                    Do
                        If Row = RowCount - 1 Then
                            If PassedOnce Then
                                If Playlist.CurrentCell.RowIndex = Playlist.RowCount - 1 Then
                                    Row = 0
                                Else
                                    Row = Playlist.CurrentCell.RowIndex + 1
                                End If
                                Exit Do
                            End If
                            If repeat Then
                                Row = 0
                                '  Playlist.CurrentCell = Playlist(0, Row)

                            End If
                            PassedOnce = True
                        Else
                            Row += 1
                            'Playlist.CurrentCell = Playlist(0, Row)
                        End If

                    Loop Until Playlist.Rows(Row).Cells(4).Value = "True"
                    Playlist.CurrentCell = Playlist(0, Row)
                Else
                    If Row = RowCount - 1 Then
                        Row = 0
                    Else
                        Row += 1
                    End If
                    Playlist.CurrentCell = Playlist(0, Row)
                End If

                Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                CheckIfVideo()
                If IsVideo Then
                    If VLC_installed Then
                        VLCclearPlaylists()
                        VlcPlayer.playlist.add("file:///" & SongFilename)
                        ' VlcPlayer.playlist.add("file:///" & SongFilename)
                        'VlcPlayer.Playlist.Play() 
                        ' VlcPlayer.Playlist.Stop() 

                        VlcPlayer.playlist.play()
                        VlcPlayer.playlist.stop()
                        VlcPlayer.playlist.play()
                    Else
                        Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                        Artwork.BackgroundImage = DrawText("Videos unsupported. Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)
                    End If



                Else
                    AudioPlayer.Instance.ResetTrackList()
                    Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                    AudioPlayer.Instance.TrackList.Tracks.Add(track)
                    New_Play()

                End If

            ElseIf RowCount = 1 Then

                If My.Settings.PlayFavorites Then
                    Dim PassedOnce As Boolean = False
                    Do
                        If Row = RowCount - 1 Then
                            If PassedOnce Then
                                If Playlist.CurrentCell.RowIndex = Playlist.RowCount - 1 Then
                                    Row = 0
                                Else
                                    Row = Playlist.CurrentCell.RowIndex + 1
                                End If
                                Exit Do
                            End If
                            If repeat Then
                                Row = 0
                                ' Playlist.CurrentCell = Playlist(0, Row)

                            End If
                            PassedOnce = True
                        Else
                            Row += 1
                            'Playlist.CurrentCell = Playlist(0, Row)
                        End If

                    Loop Until Playlist.Rows(Row).Cells(4).Value = "True"
                    Playlist.CurrentCell = Playlist(0, Row)
                Else
                    Row = 0
                    Playlist.CurrentCell = Playlist(0, Row)
                End If
                Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                CheckIfVideo()
                If IsVideo Then
                    If VLC_installed Then
                        VLCclearPlaylists()
                        VlcPlayer.playlist.add("file:///" & SongFilename)

                        'VlcPlayer.Playlist.Play() 
                        ' VlcPlayer.Playlist.Stop() 

                        VlcPlayer.playlist.play()
                        VlcPlayer.playlist.stop()
                        VlcPlayer.playlist.play()
                    Else
                        Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                        Artwork.BackgroundImage = DrawText("Videos unsupported. Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)
                    End If


                Else
                    AudioPlayer.Instance.ResetTrackList()
                    Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                    AudioPlayer.Instance.TrackList.Tracks.Add(track)
                    New_Play()

                End If

            End If
        End Sub
        Public Sub MediaFinishedRepeatNone()


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
            If RowCount > 1 Then
                If Row = RowCount - 1 Then

                    If My.Settings.PlayFavorites Then
                        Dim PassedOnce As Boolean = False
                        Do
                            If Row = RowCount - 1 Then
                                If PassedOnce Then
                                    If Playlist.CurrentCell.RowIndex = Playlist.RowCount - 1 Then
                                        Row = 0
                                    Else
                                        Row = Playlist.CurrentCell.RowIndex + 1
                                    End If
                                    Exit Do
                                End If
                                ' If repeat Then
                                Row = 0
                                ' Playlist.CurrentCell = Playlist(0, Row)
                                PassedOnce = True
                                ' End If
                            Else
                                Row += 1
                                'Playlist.CurrentCell = Playlist(0, Row)
                            End If

                        Loop Until Playlist.Rows(Row).Cells(4).Value = "True"
                        Playlist.CurrentCell = Playlist(0, Row)
                    Else
                        Row = 0
                        Playlist.CurrentCell = Playlist(0, Row)
                    End If
                    Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                    CheckIfVideo()
                    If IsVideo Then
                        If VLC_installed Then
                            VLCclearPlaylists()
                            VlcPlayer.playlist.add("file:///" & SongFilename)
                            ' VlcPlayer.Playlist.Stop() 
                            ' VlcPlayer.playlist.add("file:///" & SongFilename)
                            ' VlcPlayer.Playlist.Stop() 
                            VlcPlayer.playlist.stop()
                        Else
                            Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                            Artwork.BackgroundImage = DrawText("Videos unsupported. Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)
                        End If

                    Else
                        AudioPlayer.Instance.ResetTrackList()
                        Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                        AudioPlayer.Instance.TrackList.Tracks.Add(track)
                        AudioPlayer.Instance.Stop()
                    End If
                Else

                    Dim DoPlay As Boolean = True
                    If My.Settings.PlayFavorites Then
                        Dim PassedOnce As Boolean = False
                        Do
                            If Row = RowCount - 1 Then
                                If PassedOnce Then
                                    If Playlist.CurrentCell.RowIndex = Playlist.RowCount - 1 Then
                                        Row = 0
                                    Else
                                        Row = Playlist.CurrentCell.RowIndex + 1
                                    End If
                                    Exit Do
                                End If
                                '  If repeat Then
                                Row = 0
                                DoPlay = False
                                ' Playlist.CurrentCell = Playlist(0, Row)
                                PassedOnce = True
                                ' End If
                            Else
                                Row += 1
                                DoPlay = True
                                'Playlist.CurrentCell = Playlist(0, Row)
                            End If

                        Loop Until Playlist.Rows(Row).Cells(4).Value = "True"
                        Playlist.CurrentCell = Playlist(0, Row)
                        If DoPlay Then
                            Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                            CheckIfVideo()
                            If IsVideo Then
                                If IsRadioStation Then
                                    New_Play()
                                Else
                                    If VLC_installed Then
                                        VLCclearPlaylists()
                                        VlcPlayer.playlist.add("file:///" & SongFilename)
                                        VlcPlayer.playlist.play()
                                        New_Play()
                                        Timer_Meta_and_Artwork.Start()
                                    Else
                                        Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                                        Artwork.BackgroundImage = DrawText("Videos unsupported. Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)
                                    End If

                                End If
                            Else
                                AudioPlayer.Instance.ResetTrackList()
                                Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                                AudioPlayer.Instance.TrackList.Tracks.Add(track)
                                New_Play()
                                Timer_Meta_and_Artwork.Start()
                            End If
                        Else
                            Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                            CheckIfVideo()
                            If IsVideo Then
                                If VLC_installed Then
                                    VLCclearPlaylists()
                                    VlcPlayer.playlist.add("file:///" & SongFilename)
                                    ' VlcPlayer.Playlist.Stop() 
                                    ' VlcPlayer.playlist.add("file:///" & SongFilename)
                                    ' VlcPlayer.Playlist.Stop() 
                                    VlcPlayer.playlist.stop()
                                Else
                                    Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                                    Artwork.BackgroundImage = DrawText("Videos unsupported. Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)
                                End If


                            Else
                                AudioPlayer.Instance.ResetTrackList()
                                Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                                AudioPlayer.Instance.TrackList.Tracks.Add(track)
                                AudioPlayer.Instance.Stop()
                            End If
                        End If
                    Else
                        Dim index As Integer = Row + 1
                        Row = index
                        Playlist.CurrentCell = Playlist(0, Row)
                        Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                        CheckIfVideo()
                        If IsVideo Then
                            If VLC_installed Then
                                VLCclearPlaylists()
                                VlcPlayer.playlist.add("file:///" & SongFilename)
                                ' VlcPlayer.playlist.add("file:///" & SongFilename)
                                'VlcPlayer.Playlist.Play() 
                                ' VlcPlayer.Playlist.Stop() 

                                VlcPlayer.playlist.play()
                                VlcPlayer.playlist.stop()
                                VlcPlayer.playlist.play()
                            Else
                                Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                                Artwork.BackgroundImage = DrawText("Videos unsupported. Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)
                            End If

                        Else
                            AudioPlayer.Instance.ResetTrackList()
                            Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                            AudioPlayer.Instance.TrackList.Tracks.Add(track)
                            New_Play()
                        End If
                    End If

                End If
            ElseIf RowCount = 1 Then

                If My.Settings.PlayFavorites Then
                    Dim PassedOnce As Boolean = False
                    Do
                        If Row = RowCount - 1 Then
                            If PassedOnce Then
                                If Playlist.CurrentCell.RowIndex = Playlist.RowCount - 1 Then
                                    Row = 0
                                Else
                                    Row = Playlist.CurrentCell.RowIndex + 1
                                End If
                                Exit Do
                            End If
                            ' If repeat Then
                            Row = 0
                            ' Playlist.CurrentCell = Playlist(0, Row)
                            PassedOnce = True
                            'End If
                        Else
                            Row += 1
                            '  Playlist.CurrentCell = Playlist(0, Row)
                        End If

                    Loop Until Playlist.Rows(Row).Cells(4).Value = "True"
                    Playlist.CurrentCell = Playlist(0, Row)
                Else
                    Row = 0
                    Playlist.CurrentCell = Playlist(0, Row)
                End If
                Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                CheckIfVideo()
                If IsVideo Then
                    If VLC_installed Then
                        VLCclearPlaylists()
                        ' VlcPlayer.SetMedia(New IO.FileInfo (SongFilename)) 
                        ' VlcPlayer.Playlist.Stop() 
                        VlcPlayer.playlist.add("file:///" & SongFilename)
                        VlcPlayer.playlist.stop()
                    Else
                        Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                        Artwork.BackgroundImage = DrawText("Videos unsupported. Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)
                    End If

                Else
                    AudioPlayer.Instance.ResetTrackList()
                    Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                    AudioPlayer.Instance.TrackList.Tracks.Add(track)
                    AudioPlayer.Instance.Stop()
                End If
            End If
        End Sub


#End Region



#Region " BASS.NET  |   Media Status"

        'Update
        Public Sub timerUpdate_Tick(sender As Object, e As EventArgs)
            Dim But_PlayPause As PictureBox
            Dim TrackBar_Seek2 As RichTrackBar

            If My.Settings.DriveMode Then
                But_PlayPause = xcarform.But_PlayPause
                TrackBar_Seek2 = xcarform.TrackBar_Seek2
            Else
                But_PlayPause = Me.But_PlayPause
                TrackBar_Seek2 = Me.TrackBar_Seek2
            End If

            If IsVideo Then
                VLCTimerSeekCode()
                Return
            End If

            Dim bassActive__1 As BASSActive = AudioPlayer.Instance.GetStreamStatus()

            Select Case bassActive__1
                Case BASSActive.BASS_ACTIVE_PLAYING
                    Dim len As Long = AudioPlayer.Instance.CurrentAudioHandle.LengthInBytes
                    Dim pos As Long = AudioPlayer.Instance.CurrentAudioHandle.Position
                    Dim LenSecs As Integer = CInt(AudioPlayer.Instance.CurrentAudioHandle.LengthInSeconds)
                    Dim PosDouble As Integer = CInt(AudioPlayer.Instance.CurrentAudioHandle.GetElapsedTime)

                    If PosDouble < LenSecs Then

                        TrackBar_Seek2.Value = (PosDouble / LenSecs) * SeekBarMaxVal
                        'DrawWavePosition(pos, len)
                    Else
                        AudioPlayer_EndReached()
                    End If
                    Try
                        If My.Settings.DriveMode Then
                            Dim hWnd As IntPtr = FindWindow(Nothing, CarForm.Text)
                            Taskbar.TaskbarManager.Instance.SetProgressValue(PosDouble / LenSecs * 100, 100, hWnd)
                        Else
                            Taskbar.TaskbarManager.Instance.SetProgressValue(PosDouble / LenSecs * 100, 100)
                        End If

                        SetTaskbarState(TaskbarState.Playing)
                    Catch
                    End Try

                    TimerSeekCode()

                    If playhover = True Then
                        If Playdown = False Then
                            But_PlayPause.BackgroundImage = PauseHoverImage
                        End If
                    Else
                        If Playdown = False Then
                            But_PlayPause.BackgroundImage = PauseImage
                        End If
                    End If

                    Exit Select
                Case BASSActive.BASS_ACTIVE_STOPPED, BASSActive.BASS_ACTIVE_PAUSED, BASSActive.BASS_ACTIVE_STALLED
                    If playhover = True Then
                        If Playdown = False Then
                            But_PlayPause.BackgroundImage = PlayHoverImage
                        End If
                    Else
                        If Playdown = False Then
                            But_PlayPause.BackgroundImage = PlayImage
                        End If
                    End If
                    Try
                        SetTaskbarState(TaskbarState.Paused)
                    Catch
                    End Try
            End Select

        End Sub
        'Update Spectrum
        Public Sub SpectrumTimer_Tick()
            If AppOpenFinished = False Then Return


            If UsingSpotify Then
                Try

                    RefreshSpectrumLine(True, Bass.BASS_GetDevice) ' My.Settings.DefaultDevice)
                Catch ex As Exception
                End Try
                Return
            End If
            If IsVideo Then
                If IsRadioStation Then Return
                Try
                    RefreshSpectrumLine(True, Bass.BASS_GetDevice) ' My.Settings.DefaultDevice)
                Catch ex As Exception
                End Try
                Return
            End If

            If BASSActive.BASS_ACTIVE_STOPPED Then
                RefreshSpectrumLine(False, AudioPlayer.Instance.CurrentAudioHandle.CurrentHandle)
            Else
                RefreshSpectrumLine(True, AudioPlayer.Instance.CurrentAudioHandle.CurrentHandle)
            End If
        End Sub


        'Media Opening
        Public Sub SeekBar2_ValueChanged() Handles TrackBar_Seek2.ValueChanged
            Dim But_PlayPause As PictureBox
            Dim TrackBar_Seek2 As RichTrackBar

            If My.Settings.DriveMode Then
                But_PlayPause = xcarform.But_PlayPause
                TrackBar_Seek2 = xcarform.TrackBar_Seek2
            Else
                But_PlayPause = Me.But_PlayPause
                TrackBar_Seek2 = Me.TrackBar_Seek2
            End If
            If UsingSpotify Then Return
            If SongStartOver = True Then
                If IsVideo = True Then
                    AudioPlayer.Instance.ResetTrackList()
                Else

                    ' VLCclearPlaylists()
                    If VLC_installed Then
                        VlcPlayer.playlist.stop()
                    End If
                    AudioPlayer_Song_Opening()
                End If

                SongStartOver = False
            End If

            Try
                If My.Settings.DriveMode Then
                    Dim hWnd As IntPtr = FindWindow(Nothing, CarForm.Text)
                    Taskbar.TaskbarManager.Instance.SetProgressValue(TrackBar_Seek2.Value, TrackBar_Seek2.Maximum, hWnd)
                Else
                    Taskbar.TaskbarManager.Instance.SetProgressValue(TrackBar_Seek2.Value, TrackBar_Seek2.Maximum)
                End If

            Catch

            End Try
        End Sub

        Public Sub AudioPlayer_Song_Opening()


            If BarCheckBox_AllowSaveItemPosition.Checked Then
                Dim TabIndex As Integer = PlaylistTabs.SelectedTabPageIndex
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
                Dim SongPosition As String = Playlist.Item(5, Row).Value.ToString
                If SongPosition <> 0 Then
                    If SongPosition <> "" Then
                        AudioPlayer.Instance.Position = SongPosition
                    Else
                        Playlist.Item(5, Row).Value = 0
                    End If
                End If
            End If

            RenderWaveForm()
            Reset_AB_Repeat()
            A_Repeat()

            If Panellyrics.Visible = True Then
                Panellyrics.AutoScrollPosition = New Point(0, 0)
            End If
            If firsttime_TimeLabel = False Then
                firsttime_TimeLabel = True
                timelabel.Width -= 5
                timelabel.Left += 2
            End If
            If IsAppFirstOpening Then
                LoadLastPlayedPos()
            End If

            GetArtwork()
            DoGetMetaInfo = True
            GetMetaInfo()
            GetOtherMetaInfo()
            Bass_InitEQ()
            Try
                SetPlayerVolume()
            Catch ex As Exception

            End Try

        End Sub



#End Region

#Region " BASS.NET  |   Spectrum"
        Private vis As New Visuals()

        Public Sub RefreshSpectrumLine(show As Boolean, stream As Integer)
            Dim PictureBoxSpec As PictureBox
            If My.Settings.DriveMode Then
                PictureBoxSpec = xcarform.PictureBoxSpec
            Else
                PictureBoxSpec = Me.PictureBoxSpec
            End If


            If IsVideo Then Return

            Select Case My.Settings.SpectrumType
                Case "Line"
                    PictureBoxSpec.Image = _
                        If(show, vis.CreateSpectrumLine(stream, _
                        PictureBoxSpec.Width, PictureBoxSpec.Height, _
                        Color.FromArgb(175, SpectrumColor2), Color.FromArgb(175, SpectrumColor1), Color.Transparent, _
                        My.Settings.SpectrumLineWidth, 3, False, False, False), Nothing)

                Case "Dot"
                    PictureBoxSpec.Image = _
                        If(show, vis.CreateSpectrumDot(stream, _
                        PictureBoxSpec.Width, PictureBoxSpec.Height, _
                        Color.FromArgb(175, SpectrumColor2), Color.FromArgb(175, SpectrumColor1), Color.Transparent, _
                        My.Settings.SpectrumLineWidth, 3, False, False, False), Nothing)

                Case "Line Peak"
                    PictureBoxSpec.Image = _
                        If(show, vis.CreateSpectrumLinePeak(stream, _
                        PictureBoxSpec.Width, PictureBoxSpec.Height, _
                        Color.FromArgb(175, SpectrumColor2), Color.FromArgb(175, SpectrumColor1), Color.FromArgb(175, SpectrumColor1), Color.Transparent, _
                        My.Settings.SpectrumLineWidth, 2, 3, 4, False, False, False), Nothing)

                Case "Bean"
                    PictureBoxSpec.Image = _
                        If(show, vis.CreateSpectrumBean(stream, _
                        PictureBoxSpec.Width, PictureBoxSpec.Height, _
                        Color.FromArgb(175, SpectrumColor2), Color.FromArgb(175, SpectrumColor1), Color.Transparent, _
                        My.Settings.SpectrumLineWidth, True, False, False), Nothing)

                Case "Ellipse"
                    PictureBoxSpec.Image = _
                        If(show, vis.CreateSpectrumEllipse(stream, _
                        PictureBoxSpec.Width, PictureBoxSpec.Height, _
                        Color.FromArgb(175, SpectrumColor2), Color.FromArgb(175, SpectrumColor1), Color.Transparent, _
                        My.Settings.SpectrumLineWidth, 3, True, True, False), Nothing)

                Case "Text"
                    PictureBoxSpec.Image = _
                        If(show, vis.CreateSpectrumText(stream, _
                        PictureBoxSpec.Width, PictureBoxSpec.Height, _
                        Color.FromArgb(175, SpectrumColor2), Color.FromArgb(175, SpectrumColor1), Color.Transparent, _
                        Label_SongName.Text, False, False, False), Nothing)

                Case "Wave"
                    PictureBoxSpec.Image = _
                        If(show, vis.CreateSpectrumWave(stream, _
                        PictureBoxSpec.Width, PictureBoxSpec.Height, _
                        Color.FromArgb(175, SpectrumColor2), Color.FromArgb(175, SpectrumColor1), Color.Transparent, _
                        My.Settings.SpectrumLineWidth, False, False, False), Nothing)
                Case "Full"
                    PictureBoxSpec.Image = _
                        If(show, vis.CreateSpectrum(stream, _
                        PictureBoxSpec.Width, PictureBoxSpec.Height, _
                        Color.FromArgb(175, SpectrumColor2), Color.FromArgb(175, SpectrumColor1), Color.Transparent, _
                        False, False, False), Nothing)


                Case Else

                    PictureBoxSpec.Image = _
                        If(show, vis.CreateSpectrumLine(stream, _
                        PictureBoxSpec.Width, PictureBoxSpec.Height, _
                        Color.FromArgb(175, SpectrumColor2), Color.FromArgb(175, SpectrumColor1), Color.Transparent, _
                        My.Settings.SpectrumLineWidth, 3, False, False, False), Nothing)


            End Select

        End Sub

        'Change Spectrum Visual
        Public Sub PictureBoxSpec_Click(sender As Object, e As MouseEventArgs) Handles PictureBoxSpec.MouseClick
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

#Region " BASS.NET  |   Settings"
#Region " Declarations"

        Private streamWave As Integer = 0
        Public Shared ghostCursorPosition As Long = -1

        <DllImport("user32.dll")> _
        Public Shared Function ShowScrollBar(hWnd As System.IntPtr, wBar As Integer, bShow As Boolean) As Boolean
        End Function

#End Region

        Public Sub LoadAudioSettings()
            trackBar_Speed2.Value = My.Settings.BASS_Speed
            trackbar_Pitch2.Value = My.Settings.BASS_Pitch
            TrackBar_PlayerVol2.Value = My.Settings.Player_Volume
            Bass_InitEQ()

            Dim audioSettings As AudioSettings = If(Settings.[Default].AudioSettings, New AudioSettings())
            AudioPlayer.Instance.TrackList = audioSettings.TrackList
            Dim trackList As TrackList = AudioPlayer.Instance.TrackList
            AudioPlayer.Instance.CurrentTrackIndex = audioSettings.LastTrack


            Dim files As String() = Environment.GetCommandLineArgs()

            If files.Length > 1 Then
                Dim composedFile As String = [String].Join(" ", files, 1, files.Length - 1)
                AudioPlayer.Instance.ResetTrackList()
                AudioPlayer.Instance.TrackList.Tracks.Add(Track.GetTrack(files(1)))
                AudioPlayer.Instance.CurrentTrackIndex = 0

                '  isplaying = AudioPlayer.Instance.Play(True)
                SongStartOver = True
            Else
                If trackList.Tracks.Count > 0 Then
                    Try

                        Dim filename As String = trackList.Tracks(AudioPlayer.Instance.CurrentTrackIndex).Location
                        If IO.File.Exists(filename) Then
                            LoadFile(filename, False)
                        End If
                    Catch generatedExceptionName As Exception
                    End Try
                End If
            End If

        End Sub

        Public Sub LoadFile(filename As String, play As Boolean)

            If IO.File.Exists(filename) Then
                If play Then
                    AudioPlayer.Instance.Play(True)
                End If
            End If

        End Sub

#End Region

#Region " BASS.NET  |   Equalizer"

        'Setup
        Dim _fxEQ As Integer() = {0, 0, 0}
        Public Sub Bass_InitEQ()
            If AppOpen Then Return
            Try
                If EQ_FirstTimeOpen Then
                    If Not Application.OpenForms().OfType(Of EqualizerForm).Any Then
                        Dim f2 As New EqualizerForm
                        EqualizerForm.Show()
                        EqualizerForm.Hide()
                    End If


                    EqualizerForm.BASS_TableEQ()
                End If
            Catch
            End Try
        End Sub
        Public Sub ShowEqualizerBut_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_ShowEqualizer.ItemClick
            OpenEqualizer()
        End Sub
        Public Sub OpenEqualizer()
            EqualizerForm.Show()
            Dim x As Integer = ((Me.Location.X) + (Me.Width / 2)) - (EqualizerForm.Width / 2)
            Dim y As Integer = ((Me.Location.Y) + (Me.Height / 2)) - (EqualizerForm.Height / 2)
            EqualizerForm.Location = New Point(x, y)
        End Sub










#End Region
#Region " BASS.NET  |   Wavelength  (Not used right now)"

        Public Shared waveForm As WaveForm = Nothing
        Private _syncer As Integer = 0
        Private _sync As SYNCPROC = Nothing

        Public Sub pictureBoxWF_Resize(sender As Object, e As EventArgs) Handles PictureBoxWF.Resize
            DrawWave()
        End Sub

        'Change Position
        Public Sub pictureBoxWF_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBoxWF.MouseUp

            ToolTip1.Hide(Me)
            If waveForm Is Nothing Then
                Return
            End If
            ghostCursorPosition = -1

            If e.Button = MouseButtons.Left Then
                Dim pos As Long = waveForm.GetBytePositionFromX(e.X, Me.PictureBoxWF.Width, -1, -1)
                Bass.BASS_ChannelSetPosition(AudioPlayer.Instance.CurrentAudioHandle.CurrentHandle, pos)
            End If
        End Sub
        Public Sub pictureBoxWF_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBoxWF.MouseMove

            If waveForm Is Nothing Then
                Return
            End If
            If e.Button = MouseButtons.Left Then
                Dim pos As Long = waveForm.GetBytePositionFromX(e.X, Me.PictureBoxWF.Width, -1, -1)
                ghostCursorPosition = pos
            End If
        End Sub

        'Current Position
        Public Sub DrawWavePosition(pos As Long, len As Long)

            If len = 0 OrElse pos < 0 Then
                Me.PictureBoxWF.Image = Nothing
                Return
            End If

            Dim bitmap As Bitmap = Nothing
            Dim g As Graphics = Nothing
            Dim p As Pen = Nothing
            Dim bpp As Double = 0
            Try

                bpp = len / CDbl(Me.PictureBoxWF.Width)
                p = New Pen(Color.Gray)
                bitmap = New Bitmap(Me.PictureBoxWF.Width, Me.PictureBoxWF.Height)
                g = Graphics.FromImage(bitmap)
                Dim x As Integer = CInt(Math.Round(pos / bpp))
                g.DrawLine(p, x, 0, x, Me.PictureBoxWF.Height - 1)
                DrawGhostCursorPosition(g, ghostCursorPosition, len)
            Catch generatedExceptionName As Exception
                bitmap = Nothing
            Finally
                ' clean up graphics resources
                If p IsNot Nothing Then
                    p.Dispose()
                End If
                If g IsNot Nothing Then
                    g.Dispose()
                End If
            End Try

            Me.PictureBoxWF.Image = bitmap
        End Sub
        'Mouse Down/Move Position
        Public Sub DrawGhostCursorPosition(g As Graphics, pos As Long, len As Long)

            If ghostCursorPosition < 0 Then
                Return
            End If

            Dim p As Pen = Nothing
            Dim bpp As Double = 0

            Try
                p = New Pen(Color.Gray)
                bpp = len / CDbl(Me.PictureBoxWF.Width)
                Dim x As Integer = CInt(Math.Round(pos / bpp))
                g.DrawLine(p, x, 0, x, Me.PictureBoxWF.Height - 1)
                ToolTip1.Show(Utils.FixTimespan(Bass.BASS_ChannelBytes2Seconds(AudioPlayer.Instance.CurrentAudioHandle.CurrentHandle, pos), "MMSS"), Me, x + Me.PictureBoxWF.Left, Me.Height - Me.PictureBoxWF.Height)
            Catch generatedExceptionName As Exception
            Finally
                ' clean up graphics resources
                If p IsNot Nothing Then
                    p.Dispose()
                End If
                If g IsNot Nothing Then
                    g.Dispose()
                End If
            End Try
        End Sub


        'Set Wave to Picturebox
        Public Sub DrawWave()

            Me.PictureBoxWF.BackgroundImage = Nothing
            PictureBoxWF.Visible = False
            Return
            If waveForm IsNot Nothing Then
                If AudioPlayer.Instance.CurrentAudioHandle.LengthInSeconds > 20000000 Then
                    Me.PictureBoxWF.BackgroundImage = Nothing

                Else
                    Me.PictureBoxWF.BackgroundImage = waveForm.CreateBitmap(Me.PictureBoxWF.Width, Me.PictureBoxWF.Height, -1, -1, True)
                End If



            Else
                Me.PictureBoxWF.BackgroundImage = Nothing

            End If
        End Sub

        'Create Wave
        Public Sub RenderWaveForm()

            Dim filename As String
            If IsVideo = False Then
                PictureBoxWF.Visible = True
                Try
                    filename = AudioPlayer.Instance.CurrentTrack.Location
                Catch
                End Try

            Else
                PictureBoxWF.Visible = False
            End If
            If AudioPlayer.Instance.CurrentAudioHandle.LengthInSeconds > 2000000 Then
                If waveForm IsNot Nothing Then
                    waveForm.Reset()

                    If AudioPlayer.Instance.CurrentAudioHandle.IsModule() Then
                        streamWave = Bass.BASS_MusicLoad(filename, 0, 0, BASSFlag.BASS_MUSIC_DECODE Or BASSFlag.BASS_MUSIC_FLOAT Or BASSFlag.BASS_MUSIC_PRESCAN, 0)
                    Else
                        streamWave = Bass.BASS_StreamCreateFile(filename, 0L, 0L, BASSFlag.BASS_STREAM_DECODE Or BASSFlag.BASS_SAMPLE_FLOAT Or BASSFlag.BASS_STREAM_PRESCAN)
                    End If
                    waveForm.NotifyHandler = New WAVEFORMPROC(AddressOf MyWaveFormCallback)
                    waveForm.WinControl = Me
                    Dim isRendering As Boolean = waveForm.RenderStart(streamWave, False)
                End If


            Else
                waveForm = New WaveForm()
                waveForm.FrameResolution = 0.01F
                ' 10ms are nice
                waveForm.CallbackFrequency = 2000
                ' every 30 seconds rendered (3000*10ms=30sec)
                waveForm.ColorBackground = Color.Transparent
                waveForm.ColorLeft = Color.FromArgb(110, 110, 110)
                waveForm.ColorLeftEnvelope = Color.FromArgb(80, 80, 80)
                waveForm.ColorRight = Color.FromArgb(95, 95, 95)
                waveForm.ColorRightEnvelope = Color.FromArgb(65, 65, 65)
                waveForm.ColorMarker = Color.DarkBlue
                waveForm.DrawWaveForm = waveForm.WAVEFORMDRAWTYPE.Stereo
                waveForm.DrawMarker = waveForm.MARKERDRAWTYPE.Line Or waveForm.MARKERDRAWTYPE.Name Or waveForm.MARKERDRAWTYPE.NamePositionAlternate
                waveForm.MarkerLength = 0.75F

                If AudioPlayer.Instance.CurrentAudioHandle.IsModule() Then
                    streamWave = Bass.BASS_MusicLoad(filename, 0, 0, BASSFlag.BASS_MUSIC_DECODE Or BASSFlag.BASS_MUSIC_FLOAT Or BASSFlag.BASS_MUSIC_PRESCAN, 0)
                Else
                    streamWave = Bass.BASS_StreamCreateFile(filename, 0L, 0L, BASSFlag.BASS_STREAM_DECODE Or BASSFlag.BASS_SAMPLE_FLOAT Or BASSFlag.BASS_STREAM_PRESCAN)
                End If
                waveForm.NotifyHandler = New WAVEFORMPROC(AddressOf MyWaveFormCallback)
                waveForm.WinControl = Me
                Dim isRendering As Boolean = waveForm.RenderStart(streamWave, True)



            End If

        End Sub
        Public Sub MyWaveFormCallback(framesDone As Integer, framesTotal As Integer, elapsedTime As TimeSpan, finished As Boolean)

            If finished Then
            End If
            ' will be called during rendering...
            DrawWave()
        End Sub


#End Region



        Dim SoundCloud1 As Ewk.SoundCloud.ApiLibrary.SoundCloud
        Public Sub Setup_SoundCloud()



        End Sub


#End Region


#Region " MEDIA"


#Region " Spotify "
#Region " Declartions"
        Public Shared MySpotify As New Spotify()
        Public Shared _SpotifyNew As New SpotifyWebAPI '= _SpotifyNew. 'SpotifyWebClientUserControl._Spotify

        Public Shared UsingSpotify As Boolean = False
        Public WasVideo As Boolean = False
        Public WasVideo_v2 As Boolean = False
        Public FirstUseSpotify As Boolean = True
#End Region

#Region " Main"
        Dim WasShuffled As Boolean
        Dim VolumeWas As Integer
        Dim SpotifyLogo As New PictureBox


        'Open Spotify
        Public Sub BarButtonSpotify_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_OpenSpotify.ItemClick
            OpenSpotify()
        End Sub
        Public Sub OpenSpotify()
            SpotifyAPI.Local.SpotifyLocalAPI.RunSpotify()

            Me.Focus()
        End Sub

        'Using Spotify
        Public Sub BarCheckSpotify_CheckedChanged(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarCheckBox_UseSpotifyold.CheckedChanged
            If BarCheckBox_UseSpotifyold.Checked Then
                '  If FirstUseSpotify Then
                UseSpotify()
                '  Else
                '      UseSpotifyContinue()
                '  End If
            Else
                'Disable use of Spotify
                IsShuffle = WasShuffled
                Select Case IsShuffle
                    Case True
                        But_Shuffle.BackgroundImage = ShuffleImage
                    Case False
                        But_Shuffle.BackgroundImage = ShuffleDisabledImage
                End Select
                Select Case My.Settings.Repeat
                    Case 0
                        repeatAll = False
                        repeat = False
                        repeatOne = False
                        But_Repeat.BackgroundImage = RepeatOffImage
                    Case 1
                        repeat = True
                        repeatOne = True
                        repeatAll = False
                        But_Repeat.BackgroundImage = RepeatOneImage
                    Case 2
                        repeat = True
                        repeatOne = False
                        repeatAll = True
                        But_Repeat.BackgroundImage = RepeatAllImage
                End Select
                UsingSpotify = False
                Timer_Spotify.Stop()

                'Re-enable Controls
                SpotifyLogo.Visible = False
                TrackBar_PlayerVol2.Value = VolumeWas
                PlaylistTabs.Enabled = True
                TrackBar_Seek2.Enabled = True
                But_Stop.Enabled = True
                But_A.Visible = True
                But_AB_Reset.Visible = True
                But_B.Visible = True
                Timer_AB.Interval = 1
                trackBar_Speed2.Visible = True
                But_SpeedDown.Visible = True
                SpeedResetBut.Visible = True
                But_SpeedUp.Visible = True
                But_PitchDown.Visible = True
                But_PitchUp.Visible = True
                trackbar_Pitch2.Visible = True
                PicBox_PitchTextBox.Visible = True
                PicBox_PitchText.Visible = True
                Label_SpeedTextbox.Visible = True
                PicBox_SpeedText.Visible = True

                'Refresh Artwork
                Timer_Meta_and_Artwork.Start()

                If WasVideo Then
                    If VLC_installed Then
                        VlcPlayer.Visible = True
                    Else
                        Dim f As New Font("Segoe UI", 10, FontStyle.Regular)
                        Artwork.BackgroundImage = DrawText("Videos unsupported. Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)
                    End If

                    Artwork.Visible = False
                End If
                TrackBar_Seek2.Maximum = SeekBarMaxVal
                timerUpdate.Start()


            End If
        End Sub
        Public Sub UseSpotify()
            Task.Run(Function() RunAuthentication())
            '   RunAuthentication()
        End Sub
        Public Sub UseSpotifyContinue()
            If InvokeRequired Then
                Invoke(New Action(AddressOf UseSpotifyContinue))
                Return
            End If
            FirstUseSpotify = False
            WasShuffled = IsShuffle

            UsingSpotify = True
            SpotifyTimeSet = DateTime.Now.AddMinutes(15)
            Artwork.BackgroundImage = ChangeOpacity(My.Resources.spotify, My.Settings.ArtworkTransparency)
            If My.Settings.MiniModeOn Then
                Label_SongName.Parent.Controls.Add(SpotifyLogo)
                SpotifyLogo.BringToFront()
                SpotifyLogo.BackgroundImage = My.Resources.Spotify_Logo_RGB_Green
                SpotifyLogo.BackgroundImageLayout = ImageLayout.Zoom
                Dim y As Integer = Label_SongName.Location.Y - Artwork.Location.Y
                SpotifyLogo.Location = New System.Drawing.Point(Label_SongName.Location.X, Artwork.Location.Y)
                SpotifyLogo.Size = New Size(78, y - 5)
                SpotifyLogo.Visible = True
            End If
            Timer_Meta_and_Artwork.Start()

            GrabSpotifyMetadata()
            GrabSpotifyArtwork()
            GrabSpotifyPlayingState()
            GrabSpotifyTime()
            GrabSpotifyStates()
            GrabSpotifyVolume()


            ' _title = _SpotifyNew.GetPlayingTrack.Item.Name.Replace(Chr(38), "&&")
            ' _Artist = _SpotifyNew.GetPlayingTrack.Item.Artists(0).Name.Replace(Chr(38), "&&")
            ' _Album = _SpotifyNew.GetPlayingTrack.Item.Album.Name.Replace(Chr(38), "&&")
            '
            ' Label_SongName.Text = _title
            ' Label_Artist.Text = _Artist
            ' Label_Album.Text = _Album
            '
            ' IsShuffle = _SpotifyNew.GetPlayback.ShuffleState

            'Pause default player music
            SetTaskbarState(TaskbarState.Paused)
            Try
                If VLC_installed Then
                    VlcPlayer.playlist.togglePause()
                    'VlcPlayer.Playlist.Play() 
                End If

                If AudioPlayer.Instance.GetStreamStatus = BASSActive.BASS_ACTIVE_PLAYING Then
                    AudioPlayer.Instance.Pause()
                End If
                timerUpdate.Stop()
            Catch
            End Try

            '----------Disable Normal Controls------------
            VolumeWas = TrackBar_PlayerVol2.Value
            PlaylistTabs.Enabled = False
            ' TrackBar_Seek2.Enabled = False
            ' But_A.Visible = False
            ' But_AB_Reset.Visible = False
            ' But_B.Visible = False
            '
            Timer_AB.Interval = 1000
            trackBar_Speed2.Visible = False
            But_SpeedDown.Visible = False
            SpeedResetBut.Visible = False
            But_SpeedUp.Visible = False
            But_PitchDown.Visible = False
            But_PitchUp.Visible = False
            trackbar_Pitch2.Visible = False
            PicBox_PitchTextBox.Visible = False
            PicBox_PitchText.Visible = False
            Label_SpeedTextbox.Visible = False
            PicBox_SpeedText.Visible = False

            '-----------------------------------------------

            'Spotify Artwork

            If VLC_installed Then
                If VlcPlayer.Visible Then
                    VlcPlayer.Visible = False
                    VLCChapterMarks.Visible = False
                    WasVideo = True
                Else
                    WasVideo = False
                End If
           End If

            Artwork.Visible = True
            RadioPlayer.Visible = False
            Timer_Meta_and_Artwork.Start()
        End Sub

        Dim auth As SpotifyAPI.Web.Auth.AutorizationCodeAuth

        Dim _clientId As String = "79f9d83ae2be40f6af9a10fea21355b4"
        Dim _secretId As String = "94c4dbe622c240faa355139a2bfde8ce"
        Dim _URL As String = "http://localhost:8000"
        Dim _scopes As SpotifyAPI.Web.Enums.Scope = Scope.UserReadPrivate Or _
Scope.UserModifyPlaybackState Or Scope.Streaming Or _
        Scope.UserReadPlaybackState
        Dim _state As String = "spotify_auth_state"
        Private Async Function RunAuthentication() As Task
            Dim ifWorks As Boolean = False
            If ifWorks Then
                auth = New SpotifyAPI.Web.Auth.AutorizationCodeAuth()
                auth.ClientId = _clientId
                auth.RedirectUri = _URL
                auth.Scope = _scopes
                auth.State = _state

                AddHandler auth.OnResponseReceivedEvent, AddressOf auth_OnAuthRecieved

                Try
                    auth.ShowDialog = True
                    auth.DoAuth()
                    auth.StartHttpServer()

                    ' auth.OpenBrowser()
                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try



            Else

                Dim webApiFactory As New WebAPIFactory("http://localhost", 8000, _clientId, _scopes)


                Try
                    _SpotifyNew = Await webApiFactory.GetWebApi()
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try



                UseSpotifyContinue()


            End If



        End Function
        Async Function auth_OnAuthRecieved(payload) As Task
            auth.StopHttpServer()
            Dim token As Token = auth.ExchangeAuthCode(payload.Code, _secretId)
            Dim api As SpotifyWebAPI = New SpotifyWebAPI() With {
                .TokenType = token.TokenType,
                .AccessToken = token.AccessToken
            }
            _SpotifyNew = api

            UseSpotifyContinue()
        End Function


        'Show / Hide Spotify
        Public Sub BarButtonToggleHideSpotify_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_ToggleHideSpotify.ItemClick
            MySpotify.ToggleHide()
        End Sub

        'Search
        Public Sub BarSpotifySearchBox_EditValueChanged(sender As Object, e As EventArgs) Handles BarEdit_SpotifySearchBox.EditValueChanged
            If Not BarEdit_SpotifySearchBox.EditValue = "" Then
                MySpotify.Search(BarEdit_SpotifySearchBox.EditValue, False)
                BarEdit_SpotifySearchBox.EditValue = ""
            End If
        End Sub

#End Region


#Region " Spotify   |   View Account"


        Public Sub barbut_Initialize_ItemClick(sender As Object, e As ItemClickEventArgs) Handles barbut_Initialize.ItemClick
            Spotify_ViewAccount()
        End Sub



        Public Sub Spotify_ViewAccount()
            Dim xform As New SpotifyWebClientForm
            xform.StartPosition = FormStartPosition.Manual
            xform.Location = New Point(Me.Location.X + Me.Width / 2 - xform.Width / 2, Me.Location.Y + Me.Height / 2 - xform.Height / 2)

            xform.Show()



        End Sub

#End Region


#End Region '                                              <<<<<<<<<<<<                                     Spotify                                              >>>>>>>>>>>>


#Region " Plex "

        Public Sub BarCheck_UsePlex_CheckedChanged(sender As Object, e As ItemClickEventArgs) Handles BarCheck_UsePlex.CheckedChanged
            If BarCheck_UsePlex.Checked Then

            Else

            End If
        End Sub



#End Region




#End Region

#Region " Play on Start     |   App Open & Current"
        Public InitiatePlayOnStart As Boolean = False
        Public InitiatePlayOnStartCurrent As Boolean = False


        'Current
        Public Sub PlayOnStartCurrent()
            Try
                CheckIfVideo()
                RefreshApp()
                If AddingPlaylist Then Return
                If IsVideo Then
                    If IsRadioStation Then
                        If My.Settings.PlayWhenTabActive.Item(PlaylistTabs.SelectedTabPageIndex) = True Then
                            Try
                                VideoPlay()
                            Catch : End Try
                        End If
                    Else
                        PlayOnStartCurrentVLC()
                    End If
                Else
                    PlayonStartCurrentBASS_NET()

                End If
                Timer_Meta_and_Artwork.Start()

            Catch
                '  MyMsgBox.Show("Error: 0x17: Error loading last played position" + Environment.NewLine + "Maybe playlist has no items added?") '& vbNewLine & vbNewLine & ex.ToString)
            End Try
        End Sub
        Public Sub PlayOnStartCurrentVLC()
            If VLC_installed = False Then Return
            VLCclearPlaylists()
            AudioPlayer.Instance.Stop()
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
            If Playlist Is Nothing Then
                VlcPlayer.playlist.stop()
                Return
            End If
            If RowCount = 0 Then
                VlcPlayer.playlist.stop()
                Return
            End If
            Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString

            If My.Settings.PlayWhenTabActive.Item(PlaylistTabs.SelectedTabPageIndex) = True Then
                VlcPlayer.playlist.add("file:///" & SongFilename)
                InitiatePlayOnStartCurrent = True
                VlcPlayer.playlist.play()
                ' VlcPlayer.Input.Position = My.Settings.LastPlayedPositions.Item(PlaylistTabs.SelectedTabPageIndex)
                VlcPlayer.input.position = My.Settings.LastPlayedPositions.Item(PlaylistTabs.SelectedTabPageIndex)
            Else
                VlcPlayer.playlist.add("file:///" & SongFilename)
                InitiatePlayOnStartCurrent = False
            End If
        End Sub
        Public Sub PlayonStartCurrentBASS_NET()


            '   Dim PlaylistIsReady As Boolean = False
            AudioPlayer.Instance.ResetTrackList()
            ' VlcPlayer.playlist.play()
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
            Row = My.Settings.LastPlayedSongs.Item(PlaylistTabs.SelectedTabPageIndex)
            If Playlist.RowCount <> 0 Then
                Playlist.Rows(Row).Selected = True
            End If
            If Playlist Is Nothing Then
                AudioPlayer.Instance.Stop()
                Return
            End If
            If RowCount = 0 Then
                AudioPlayer.Instance.Stop()
                Return
            End If
            Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString

            Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
            AudioPlayer.Instance.TrackList.Tracks.Add(track)

            If My.Settings.PlayWhenTabActive.Item(PlaylistTabs.SelectedTabPageIndex) = "" Then _
                My.Settings.PlayWhenTabActive.Item(PlaylistTabs.SelectedTabPageIndex) = False
            If My.Settings.PlayWhenTabActive.Item(PlaylistTabs.SelectedTabPageIndex) = True Then
                InitiatePlayOnStartCurrent = True
                New_Play()
                AudioPlayer.Instance.Position = My.Settings.LastPlayedPositions.Item(PlaylistTabs.SelectedTabPageIndex)
            Else
                InitiatePlayOnStartCurrent = False
                Return
            End If

        End Sub

        'App Open
        Public Sub PlayOnStart()


            '   '  Threading.Thread.Sleep(10)
            If PlaylistTabs.SelectedTabPage.Text = "Spotify" Then Return
            Try
                CheckIfVideo()
                If IsVideo Then
                    PlayOnStartVLC()
                Else
                    PlayOnStartBASS_NET()
                End If
            Catch ex As Exception
                Dim text As String = "Error: 0x18: Error Loading Setting: ""Play song on App Start"". To fix, please reset setting by turning if off, then on again."
                If isOldPlaylist = False Then
                    ' MyMsgBox.Show(text, "", True)
                End If
            End Try
        End Sub
        Public Sub PlayOnStartVLC()

            If VLC_installed = False Then Return

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

            If My.Settings.PlayOnStart = True Then
                VLCclearPlaylists()
                PlaylistTabs.SelectedTabPageIndex = My.Settings.PlaylistsSelected
                InitiatePlayOnStart = True
                VideoPlay()
                VlcPlayer.input.position = My.Settings.LastPlayedPositions.Item(PlaylistTabs.SelectedTabPageIndex)
            Else
                InitiatePlayOnStart = False
            End If
        End Sub
        Public Sub PlayOnStartBASS_NET()


            'Play On Start on/off
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
            If Playlist.Rows.Count = 0 Then Return
            Dim Row As Integer = Playlist.CurrentCell.RowIndex
            Dim RowCount As Integer = Playlist.RowCount
            Row = My.Settings.LastPlayedSongs.Item(PlaylistTabs.SelectedTabPageIndex)
            Playlist.Rows(Row).Selected = True
            Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
            AudioPlayer.Instance.ResetTrackList()
            Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
            AudioPlayer.Instance.TrackList.Tracks.Add(track)

            If My.Settings.PlayOnStart = True Then
                InitiatePlayOnStart = True
                New_Play()
                AudioPlayer.Instance.Position = My.Settings.LastPlayedPositions.Item(PlaylistTabs.SelectedTabPageIndex)
            Else
                InitiatePlayOnStart = False
            End If
        End Sub


#End Region
#Region " PLAYBACK"


#Region " PLAY | PAUSE | STOP | PREV | NEXT"
#Region " Varibles for Playback Controls & Seek"
        Dim TotalTime As Long
        Dim CurTime As Long
        Dim currentIndex As Integer = 0
        Dim NewPlay As Boolean = False
        Dim radioplaybutton As Gecko.GeckoHtmlElement
        Dim RadioPlayState As String
        Dim RadioTimer As New Timer
#End Region
        Public Enum TaskbarState
            Playing
            Paused
        End Enum

        Public Function SetTaskbarState(TaskbarState As TaskbarState)
            Select Case TaskbarState
                Case TaskbarState.Paused
                    If My.Settings.DriveMode Then
                        Dim hWnd As IntPtr = FindWindow(Nothing, CarForm.Text)
                        Taskbar.TaskbarManager.Instance.SetProgressState(Taskbar.TaskbarProgressBarState.Paused, hWnd)
                    Else
                        Taskbar.TaskbarManager.Instance.SetProgressState(Taskbar.TaskbarProgressBarState.Paused)
                    End If
                Case TaskbarState.Playing
                    If My.Settings.DriveMode Then
                        Dim hWnd As IntPtr = FindWindow(Nothing, CarForm.Text)
                        Taskbar.TaskbarManager.Instance.SetProgressState(Taskbar.TaskbarProgressBarState.Normal, hWnd)
                    Else
                        Taskbar.TaskbarManager.Instance.SetProgressState(Taskbar.TaskbarProgressBarState.Normal)
                    End If
            End Select
        End Function



        'All Play
        Public Sub PlayALL()
            CheckIfVideo()
            If IsVideo = False Then
                If AudioPlayer.Instance.TrackList.Tracks.Count > 0 Then
                    New_Play()
                End If
            Else
                VideoPlay()
            End If
        End Sub
        Public Sub PlayHotkeySub()
            If IsVideo = False Then
                If AudioPlayer.Instance.TrackList.Tracks.Count > 0 Then
                    New_Play()
                End If
            Else
                VideoPlay()
            End If
        End Sub
        'Double Click to PLAY
        Public Sub DoubleClickPlay()
            NewPlay = True
            InitiatePlayOnStart = True
            InitiatePlayOnStartCurrent = True
            Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next

            If Playlist.CurrentCell.RowIndex >= 0 AndAlso Playlist.CurrentCell.ColumnIndex >= 0 Then
                Dim row As Integer
                For i As Integer = 0 To Playlist.SelectedCells.Count - 1
                    row = Playlist.SelectedCells.Item(i).RowIndex
                Next
                Dim SongFilename As String = Playlist.Item(6, row).Value.ToString

                Timer_Seek.Stop() : Timer_Meta_and_Artwork.Stop()
                AudioPlayer.Instance.ResetTrackList()
                Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                AudioPlayer.Instance.TrackList.Tracks.Add(track)
                If AudioPlayer.Instance.TrackList.Tracks.Count > 0 Then
                    If AudioPlayer.Instance.CurrentTrack Is Nothing Then AudioPlayer.Instance.CurrentTrackIndex = 0
                    SongStartOver = True
                    New_Play()

                End If
                Timer_Meta_and_Artwork.Start() : Timer_Seek.Start()
            End If
        End Sub

        'Audio Play
        Public Sub New_Play()
            Timer_Meta_and_Artwork.Start()
            CheckIfVideo()

            If UsingSpotify Then
                '  MySpotify.RefreshConnection()
                '  MySpotify.PlayPause()
                If SpotifyPlaying Then
                    _SpotifyNew.PausePlayback()
                    SetTaskbarState(TaskbarState.Paused)
                Else
                    _SpotifyNew.ResumePlayback()

                    SetTaskbarState(TaskbarState.Playing)
                End If

            Else
                If IsVideo = False Then
                    If VLC_installed Then
                        VlcPlayer.playlist.stop()
                    End If


                    NewAudioPlay()
                Else
                    If VLC_installed Then
                        VlcPlayer.playlist.stop() : VLCclearPlaylists()
                    End If

                    isVLCplaying = False : If InitiatePlayOnStart = True Then VideoPlay()
                End If
            End If
        End Sub
        Public Sub NewAudioPlay()
            Dim currentAction As BASSActive = AudioPlayer.Instance.GetStreamStatus()
            Select Case currentAction
                Case BASSActive.BASS_ACTIVE_PLAYING     'Was Playing
                    AudioPlayer.Instance.Pause()
                    Exit Select

                Case BASSActive.BASS_ACTIVE_PAUSED      'Was Paused
                    UpdatePitchByCurrentValue()
                    SetPlayerVolume()
                    AudioPlayer.Instance.Play(False)
                    UpdatePitchByCurrentValue()
                    SetPlayerVolume()
                    Exit Select

                Case Else                               'Was Stopped
                    If AudioPlayer.Instance.TrackList.Tracks.Count > 0 Then
                        If AudioPlayer.Instance.CurrentTrack Is Nothing Then AudioPlayer.Instance.CurrentTrackIndex = 0
                        AudioPlayer.Instance.Play(True) : UpdatePitchByCurrentValue() : UpdateSpeedByCurrentValue() : SetPlayerVolume()
                        AudioPlayer.Instance.Pause() : AudioPlayer.Instance.Play(False)
                        trackbar_Pitch2.Enabled = True : But_PitchDown.Enabled = True : But_PitchUp.Enabled = True : But_PitchDown.Visible = True : But_PitchUp.Visible = True : trackBar_Speed2.Enabled = True : SongStartOver = True
                    End If
                    If InitiatePlayOnStart = False Then
                        Try : AudioPlayer.Instance.Position = My.Settings.LastPlayedPositions.Item(PlaylistTabs.SelectedTabPageIndex)
                        Catch ex As Exception : End Try
                        InitiatePlayOnStart = True
                    End If
                    If InitiatePlayOnStartCurrent = False Then
                        Try : AudioPlayer.Instance.Position = My.Settings.LastPlayedPositions.Item(PlaylistTabs.SelectedTabPageIndex)
                        Catch ex As Exception : End Try
                        InitiatePlayOnStartCurrent = True
                    End If : Exit Select : End Select
        End Sub



        'Video Play
        Public Sub VideoPlay()
            Try

          
            Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next
            Dim row As Integer : For i As Integer = 0 To Playlist.SelectedCells.Count - 1 : row = Playlist.SelectedCells.Item(i).RowIndex : Next

            Timer_Meta_and_Artwork.Start()
            AudioPlayer.Instance.Stop() : AudioPlayer.Instance.ResetTrackList()

            If UsingSpotify Then
                MySpotify.RefreshConnection() : MySpotify.PlayPause()

            ElseIf Playlist.Item(2, row).Value = "Radio Station" Then
                If RadioPlayer.Visible = False Then
                    NewVideoPlay()
                    But_PlayPause.BackgroundImage = PauseImage
                    DoGetMetaInfo = True
                Else
                    If NewPlay Then
                        NewVideoPlay()
                        But_PlayPause.BackgroundImage = PauseImage
                        DoGetMetaInfo = True
                        NewPlay = False
                        Return
                    End If
                    If RadioPlayer.Document Is Nothing Then
                        Dim SongFilename As String = Playlist.Item(6, row).Value.ToString

                        Artwork.Parent.Controls.Add(RadioPlayer)
                        RadioPlayer.Focus()
                        RadioPlayer.Size = Artwork.Size
                        RadioPlayer.Location = Artwork.Location
                        RadioPlayer.Visible = True
                        RadioPlayer.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
                        RadioPlayer.Navigate(SongFilename)
                        Timer_Meta_and_Artwork.Start()
                        radioplaybutton = CType(RadioPlayer.DomDocument.GetElementById("b_top_play"), Gecko.GeckoHtmlElement)
                    End If

                    RadioPlayer.BringToFront()
                    ' VlcPlayer.Visible = False


                    Try
                        radioplaybutton.Click()
                        If radioplaybutton.ClassName = "b-play" Then
                            But_PlayPause.BackgroundImage = PlayImage
                            RadioPlayState = "Stopped"
                            RadioTimer.Start()
                            SetTaskbarState(TaskbarState.Paused)
                        Else
                            But_PlayPause.BackgroundImage = PauseImage
                            RadioPlayState = "Playing"
                            RadioTimer.Start()
                            SetTaskbarState(TaskbarState.Playing)
                        End If
                    Catch ex As Exception
                        '  MyMsgBox.Show()
                    End Try
                    DoGetMetaInfo = True



                End If

            Else
                If VLC_installed Then
                    If VlcPlayer.playlist.isPlaying = True Then ' Pause Video
                        VlcPlayer.playlist.togglePause()
                        isVLCplaying = True
                        Try : SetTaskbarState(TaskbarState.Paused) : Catch : End Try
                        But_PlayPause.BackgroundImage = PlayImage

                    Else
                        If isVLCplaying Then                    ' Play Video
                            VlcPlayer.playlist.play()
                            isVLCplaying = True
                            Try : SetTaskbarState(TaskbarState.Playing) : Catch : End Try
                            But_PlayPause.BackgroundImage = PauseImage
                        Else                                    ' Play Video from start
                            isVLCplaying = False
                            NewVideoPlay()
                            VlcPlayer.playlist.play()
                            isVLCplaying = True
                            Try : SetTaskbarState(TaskbarState.Playing) : Catch : End Try
                            But_PlayPause.BackgroundImage = PauseImage
                        End If
                        If InitiatePlayOnStart = False Then
                            VlcPlayer.input.position = My.Settings.LastPlayedPositions.Item(PlaylistTabs.SelectedTabPageIndex)
                            InitiatePlayOnStart = True
                        End If
                        If InitiatePlayOnStartCurrent = False Then
                            VlcPlayer.input.position = My.Settings.LastPlayedPositions.Item(PlaylistTabs.SelectedTabPageIndex)
                            InitiatePlayOnStartCurrent = True
                        End If

                    End If
                End If

                ReApplySubtitles()
            End If
            Catch ex As Exception

            End Try
        End Sub
        Public Sub NewVideoPlay()
            If VLC_installed = False Then Return
            BarCheckBox_UseSpotifyold.Checked = False : BarCheckBox_YoutubeUse.Checked = False
            Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next

            Timer_Seek.Stop() : Timer_Meta_and_Artwork.Stop()
            VLCclearPlaylists()

            Dim row As Integer
            For i As Integer = 0 To Playlist.SelectedCells.Count - 1
                row = Playlist.SelectedCells.Item(i).RowIndex
            Next
            Dim SongFilename As String = Playlist.Item(6, row).Value.ToString
            If Playlist.Item(2, row).Value = "YouTube Video" Then
                VlcPlayer.playlist.stop()
                Timer_Seek.Stop() : Timer_Meta_and_Artwork.Stop()
                Dim videoID As String = "http://www.youtube.com/watch?v=" & SongFilename
                VlcPlayer.playlist.add(videoID)
                VlcPlayer.playlist.play()
            ElseIf Playlist.Item(2, row).Value = "Radio Station" Then
                VlcPlayer.playlist.stop()
                Timer_Seek.Stop() : Timer_Meta_and_Artwork.Stop()
                Artwork.Parent.Controls.Add(RadioPlayer)
                RadioPlayer.Size = Artwork.Size
                RadioPlayer.Location = Artwork.Location
                RadioPlayer.Visible = True
                RadioPlayer.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
                RadioPlayer.Navigate(SongFilename)
                DoGetMetaInfo = True
                RadioTimer.Start()
                Timer_Meta_and_Artwork.Start()
                Try
                    radioplaybutton = CType(RadioPlayer.DomDocument.GetElementById("b_top_play"), Gecko.GeckoHtmlElement)
                Catch ex As Exception
                End Try

                RadioPlayer.BringToFront()

                '  VlcPlayer.Visible = False

                'VlcPlayer.playlist.add(SongFilename)
                'VlcPlayer.playlist.play()
            Else
                '  Label_SpeedTextbox.Text = VlcPlayer.input.rate & ".0"

                VlcPlayer.playlist.add("file:///" & SongFilename)
                VlcPlayer.playlist.play()
                Refresh_VLC_volspeed()
            End If
            Timer_Seek.Start() : Timer_Meta_and_Artwork.Start() ' Set Artwork

        End Sub

        'Youtube Play
        Public Sub PlayYoutube()
            If VLC_installed = False Then Return
            If VlcPlayer.playlist.isPlaying = True Then
                VlcPlayer.playlist.togglePause()
                isVLCplaying = True
                Try
                    SetTaskbarState(TaskbarState.Paused)
                Catch
                End Try

                But_PlayPause.BackgroundImage = PlayImage

            Else
                If isVLCplaying Then
                    VlcPlayer.playlist.play()
                    isVLCplaying = True
                    Try
                        SetTaskbarState(TaskbarState.Playing)
                    Catch
                    End Try
                    But_PlayPause.BackgroundImage = PauseImage
                Else
                    isVLCplaying = False
                    Timer_Seek.Stop()
                    Timer_Meta_and_Artwork.Stop()
                    VlcPlayer.playlist.stop()
                    VLCclearPlaylists()

                    Try
                        SetTaskbarState(TaskbarState.Playing)
                    Catch
                    End Try
                    But_PlayPause.BackgroundImage = PauseImage
                End If
                isVLCplaying = False
            End If

        End Sub

        Public Sub RadioPlayer_Navigated(sender As Object, e As Gecko.GeckoNavigatedEventArgs)
            Try

                RadioPlayer.GetDocShellAttribute.GetContentViewerAttribute.SetFullZoomAttribute(0.4)
                wait(1)
                radioplaybutton = CType(RadioPlayer.DomDocument.GetElementById("b_top_play"), Gecko.GeckoHtmlElement)

                If radioplaybutton.ClassName = "b-play" Then
                    But_PlayPause.BackgroundImage = PlayImage
                    RadioPlayState = "Stopped"
                Else
                    But_PlayPause.BackgroundImage = PauseImage
                    RadioPlayState = "Playing"
                End If
            Catch ex As Exception
                ' MyMsgBox.Show("Error with Radio, please wait, or try again.")
            End Try
            DoGetMetaInfo = True
            RadioTimer.Start()
        End Sub
        Public Sub RadioTimer_Tick()
            DoGetMetaInfo = True
            Timer_Meta_and_Artwork.Start()
            RadioTimer.Start()
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
                        But_PlayPause.BackgroundImage = PlayImage
                    Else
                        But_PlayPause.BackgroundImage = PauseImage
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
                            But_PlayPause.BackgroundImage = PlayImage
                        Else
                            But_PlayPause.BackgroundImage = PauseImage
                        End If
                    End If

                Else
                    If VLC_installed Then
                        If VlcPlayer.playlist() IsNot Nothing Then 'If VlcPlayer.playlist IsNot Nothing Then
                            VlcPlayer.playlist.stop()
                            isVLCplaying = False
                            If VlcPlayer.playlist.isPlaying = False Then ' If VlcPlayer.playlist.isPlaying = False Then
                                But_PlayPause.BackgroundImage = PlayImage
                            Else
                                But_PlayPause.BackgroundImage = PauseImage
                            End If
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
                        '  CheckIfVideo()
                        If IsVideo Then
                            If VLC_installed Then

                                If VlcPlayer.input.chapter.count > 1 Then
                                    If VlcPlayer.input.chapter.track = VlcPlayer.input.chapter.count - 1 Then
                                        NextItem()
                                    Else
                                        VlcPlayer.input.chapter.next()


                                        VLCChapterMarks.SelectedIndex = VlcPlayer.input.chapter.track
                                    End If
                                Else
                                    NextItem()
                                End If
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

            NewPlay = True
            If UsingSpotify Then
                ' MySpotify.RefreshConnection()
                '  MySpotify.PlayNext()
                _SpotifyNew.SkipPlaybackToNext()
                Exit Sub


                'Playlists
            Else
                Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next
                Dim Row As Integer = Playlist.CurrentCell.RowIndex : Dim RowCount As Integer = Playlist.RowCount
                'Dim SongFilename As String = Playlist.Item(6, row).Value.ToString
                If Playlist.RowCount > 0 Then
                    If IsShuffle Then
                        Dim random As New System.Random
                        Dim Index As System.Int32 = random.Next(0, Playlist.RowCount)
                        Row = Index
                        '     Playlist.CurrentCell = Playlist(0, Row)
                        Playlist.CurrentCell = Playlist(0, Row)
                        If My.Settings.PlayFavorites Then
                            Dim PassedOnce As Boolean = False
                            Do
                                If Row = RowCount - 1 Then
                                    If PassedOnce Then
                                        If Playlist.CurrentCell.RowIndex = Playlist.RowCount - 1 Then
                                            Row = 0
                                        Else
                                            Row = Playlist.CurrentCell.RowIndex + 1
                                        End If
                                        Exit Do
                                    End If
                                    '  If repeat Then
                                    Row = 0
                                    '  Playlist.CurrentCell = Playlist(0, Row)
                                    PassedOnce = True
                                    'End If
                                Else
                                    Row += 1
                                    '  Playlist.CurrentCell = Playlist(0, Row)
                                End If
                            Loop Until Playlist.Rows(Row).Cells(4).Value = "True"
                            Playlist.CurrentCell = Playlist(0, Row)
                        End If

                        Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString

                        CheckIfVideo()
                        If IsVideo Then
                            If IsRadioStation Then
                                New_Play()
                            Else
                                If VLC_installed Then
                                    VLCclearPlaylists()
                                    VlcPlayer.playlist.add("file:///" & SongFilename)
                                    VlcPlayer.playlist.play()

                                End If
                                New_Play()
                                Timer_Meta_and_Artwork.Start()
                            End If

                        Else
                            AudioPlayer.Instance.ResetTrackList()
                            Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                            AudioPlayer.Instance.TrackList.Tracks.Add(track)
                            New_Play()
                            Timer_Meta_and_Artwork.Start()
                        End If


                    Else
                        Timer_Seek.Stop()
                        Dim DoPlay As Boolean = True
                        If My.Settings.PlayFavorites Then
                            Dim PassedOnce As Boolean = False
                            Do
                                If Row = RowCount - 1 Then
                                    If PassedOnce Then
                                        If Playlist.CurrentCell.RowIndex = Playlist.RowCount - 1 Then
                                            Row = 0
                                        Else
                                            Row = Playlist.CurrentCell.RowIndex + 1
                                        End If
                                        Exit Do
                                    End If
                                    '  If repeat Then
                                    Row = 0
                                    '  Playlist.CurrentCell = Playlist(0, Row)
                                    DoPlay = False
                                    'End If
                                    PassedOnce = True
                                Else
                                    Row += 1
                                    DoPlay = True
                                    ' Playlist.CurrentCell = Playlist(0, Row)
                                End If
                            Loop Until Playlist.Rows(Row).Cells(4).Value = "True"
                            Playlist.CurrentCell = Playlist(0, Row)
                        Else

                            If Row = RowCount - 1 Then
                                If repeat Then
                                    Row = 0
                                    Playlist.CurrentCell = Playlist(0, Row)
                                End If
                            Else
                                Row += 1
                                Playlist.CurrentCell = Playlist(0, Row)
                            End If
                        End If

                        Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                        CheckIfVideo()
                        If IsVideo Then
                            If IsRadioStation Then
                                New_Play()
                            Else
                                If VLC_installed Then
                                    VLCclearPlaylists()
                                    VlcPlayer.playlist.add("file:///" & SongFilename)
                                    VlcPlayer.playlist.play()

                                End If
                                New_Play()
                                Timer_Meta_and_Artwork.Start()
                            End If

                        Else
                            AudioPlayer.Instance.ResetTrackList()
                            Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                            AudioPlayer.Instance.TrackList.Tracks.Add(track)
                            New_Play()
                            Timer_Meta_and_Artwork.Start()

                        End If

                    End If
                    Timer_Seek.Start()
                    Reset_AB_Repeat()
                    Return
                End If
            End If
            Reset_AB_Repeat()
        End Sub
        Public Sub SkipForward5Secs()
            If UsingSpotify Then
                'MySpotify.PlayForward()
                For i As Integer = 0 To _SpotifyNew.GetDevices.Devices.Count - 1
                    If _SpotifyNew.GetDevices.Devices(i).IsActive Then
                        _SpotifyNew.SeekPlayback(_SpotifyNew.GetPlayback.ProgressMs + 5000, _SpotifyNew.GetDevices.Devices(i).Id)
                    End If
                Next

            Else
                If NextButhover Then
                    If IsVideo = False Then
                        AudioPlayer.Instance.Position += CLng(CDbl(AudioPlayer.Instance.CurrentAudioHandle.GetElapsedTime) + 1600000)  'CLng(35000 * (AudioPlayer.Instance.CurrentAudioHandle.LengthInSeconds / 5))
                    Else
                        If VLC_installed Then
                            VlcPlayer.input.time = VlcPlayer.input.time + 5000
                        End If

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
                    If IsRadioStation Then
                        NextItem()
                    Else
                        If VLC_installed Then
                            If VlcPlayer.input.chapter.count > 1 Then
                                If VlcPlayer.input.chapter.track = VlcPlayer.input.chapter.count - 1 Then
                                    NextItem()
                                Else
                                    VlcPlayer.input.chapter.next()
                                    VLCChapterMarks.SelectedIndex = VlcPlayer.input.chapter.track
                                End If
                            Else
                                NextItem()
                            End If
                        End If

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
                        ' CheckIfVideo()
                        If IsVideo Then
                            If IsRadioStation Then
                                PrevItem()
                            Else
                                If VLC_installed Then
                                    If VlcPlayer.input.chapter.count > 1 Then
                                        If VlcPlayer.input.chapter.track = 0 Then
                                            PrevItem()
                                        Else
                                            VlcPlayer.input.chapter.prev()
                                            VLCChapterMarks.SelectedIndex = VlcPlayer.input.chapter.track
                                        End If
                                    Else
                                        PrevItem()
                                    End If
                                End If

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

            NewPlay = True
            If UsingSpotify Then
                '   MySpotify.RefreshConnection()
                '  MySpotify.PlayPrev()
                _SpotifyNew.SkipPlaybackToPrevious()
                Exit Sub
            Else



                Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next
                Dim Row As Integer = Playlist.CurrentCell.RowIndex : Dim RowCount As Integer = Playlist.RowCount


                If Playlist.RowCount > 0 Then
                    If IsShuffle Then
                        Dim random As New System.Random
                        Dim Index As System.Int32 = random.Next(0, Playlist.RowCount)
                        Row = Index
                        Playlist.CurrentCell = Playlist(0, Row)
                        If My.Settings.PlayFavorites Then
                            Dim PassedOnce As Boolean = False
                            Do
                                If Row = RowCount - 1 Then
                                    If PassedOnce Then
                                        If Playlist.CurrentCell.RowIndex = Playlist.RowCount - 1 Then
                                            Row = 0
                                        Else
                                            Row = Playlist.CurrentCell.RowIndex + 1
                                        End If
                                        Exit Do
                                    End If
                                    Row = 0
                                    PassedOnce = True
                                Else
                                    Row += 1
                                End If
                            Loop Until Playlist.Rows(Row).Cells(4).Value = "True"
                            Playlist.CurrentCell = Playlist(0, Row)
                        End If
                        Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                        AudioPlayer.Instance.ResetTrackList()
                        Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                        AudioPlayer.Instance.TrackList.Tracks.Add(track)
                        New_Play()
                        Timer_Meta_and_Artwork.Start()
                    Else
                        Dim DoPlay As Boolean = True
                        If My.Settings.PlayFavorites Then
                            Dim PassedOnce As Boolean = False
                            Do
                                If Row < 0 Then
                                    '   New_Play()
                                    '   Timer_Meta_and_Artwork.Start()
                                ElseIf Row <> 0 Then
                                    Row -= 1
                                    DoPlay = False

                                ElseIf Row = 0 Then
                                    ' 
                                    If PassedOnce Then
                                        If Playlist.CurrentCell.RowIndex = 0 Then
                                            Row = Playlist.RowCount - 1
                                        Else
                                            Row = Playlist.CurrentCell.RowIndex - 1
                                        End If
                                        Exit Do
                                    Else
                                        Row = Playlist.RowCount - 1
                                    End If

                                    PassedOnce = True

                                End If

                            Loop Until Playlist.Rows(Row).Cells(4).Value = "True"
                            Playlist.CurrentCell = Playlist(0, Row)

                            Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                            AudioPlayer.Instance.ResetTrackList()
                            Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                            AudioPlayer.Instance.TrackList.Tracks.Add(track)
                            New_Play()
                            Timer_Meta_and_Artwork.Start()

                        Else
                            If Row < 0 Then
                                New_Play()
                                Timer_Meta_and_Artwork.Start()
                            ElseIf Row <> 0 Then
                                Row -= 1
                                Playlist.CurrentCell = Playlist(0, Row)
                                Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                                AudioPlayer.Instance.ResetTrackList()
                                Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                                AudioPlayer.Instance.TrackList.Tracks.Add(track)
                                New_Play()
                                Timer_Meta_and_Artwork.Start()
                            ElseIf repeatAll = True Or repeatOne = True Then
                                If Row = 0 Then
                                    Row = RowCount - 1
                                    Playlist.CurrentCell = Playlist(0, Row)
                                    Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                                    AudioPlayer.Instance.ResetTrackList()
                                    Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                                    AudioPlayer.Instance.TrackList.Tracks.Add(track)
                                    New_Play()
                                    Timer_Meta_and_Artwork.Start()
                                End If
                            End If
                        End If


                    End If
                    Return
                End If
            End If

        End Sub
        Public Sub SkipBackwards5Secs()
            If UsingSpotify Then
                '  MySpotify.PlayBackwards()
                For i As Integer = 0 To _SpotifyNew.GetDevices.Devices.Count - 1
                    If _SpotifyNew.GetDevices.Devices(i).IsActive Then
                        If _SpotifyNew.GetPlayback.ProgressMs > 5000 Then
                            _SpotifyNew.SeekPlayback(_SpotifyNew.GetPlayback.ProgressMs - 5000, _SpotifyNew.GetDevices.Devices(i).Id)
                        Else
                            _SpotifyNew.SeekPlayback(0, _SpotifyNew.GetDevices.Devices(i).Id)
                        End If

                    End If
                Next


            Else
                If Prevrwhover Then
                    If IsVideo = False Then
                        If AudioPlayer.Instance.Position >= 1600000 Then
                            AudioPlayer.Instance.Position += CLng(CDbl(AudioPlayer.Instance.CurrentAudioHandle.GetElapsedTime) - 1600000) 'CLng(35000 * (AudioPlayer.Instance.CurrentAudioHandle.LengthInSeconds / 5))
                        Else
                            AudioPlayer.Instance.Position = 0
                        End If

                    Else
                        If VLC_installed Then
                            If VlcPlayer.input.time > 5000 Then
                                VlcPlayer.input.time = VlcPlayer.input.time - 5000
                            Else
                                VlcPlayer.input.time = 0
                            End If
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
                    If IsRadioStation Then
                        PrevItem()
                    Else
                        If VLC_installed Then
                            If VlcPlayer.input.chapter.count > 1 Then
                                VlcPlayer.input.chapter.prev()
                                VLCChapterMarks.SelectedIndex = VlcPlayer.input.chapter.track
                            Else
                                PrevItem()
                            End If
                        End If

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
                            VideoPlay()
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
                                If IsRadioStation Then
                                    PrevItem()
                                Else
                                    If VLC_installed Then
                                        If VlcPlayer.input.chapter.count > 1 Then
                                            VlcPlayer.input.chapter.prev()
                                            VLCChapterMarks.SelectedIndex = VlcPlayer.input.chapter.track
                                        Else
                                            PrevItem()
                                        End If
                                    End If

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
                                If IsRadioStation Then
                                    NextItem()
                                Else
                                    If VLC_installed Then
                                        If VlcPlayer.input.chapter.count > 1 Then
                                            If VlcPlayer.input.chapter.track = VlcPlayer.input.chapter.count - 1 Then
                                                NextItem()
                                            Else
                                                VlcPlayer.input.chapter.next()
                                                VLCChapterMarks.SelectedIndex = VlcPlayer.input.chapter.track
                                            End If
                                        Else
                                            NextItem()
                                        End If
                                    End If

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
                Case Keys.M
                    If e.Control AndAlso e.Shift Then
                        HamburgerMenu.ShowPopup(But_SettingsPic.PointToScreen(But_SettingsPic.Location))
                    End If
                Case Keys.Q
                    If e.Control AndAlso e.Shift Then
                        QuickOpenMenu.ShowPopup(But_SettingsPic.PointToScreen(But_SettingsPic.Location))
                    End If
                Case Keys.A
                    If e.Control AndAlso e.Shift Then
                        ' AudioMenu.ShowPopup(But_SettingsPic.PointToScreen(But_SettingsPic.Location))
                    End If
                Case Keys.S
                    If e.Control AndAlso e.Shift Then
                        ' SubtitlesnMenu.ShowPopup(But_SettingsPic.PointToScreen(But_SettingsPic.Location))
                    End If
                Case Keys.F19
                    If e.Control AndAlso e.Shift Then
                        BringMeToFocus()
                        RefreshAudios()
                        MENUNEW_Audio.ShowPopup(But_SettingsPic.PointToScreen(But_SettingsPic.Location))
                    End If
                Case Keys.F20
                    If e.Control AndAlso e.Shift Then
                        BringMeToFocus()
                        MENUNEW_Subtitle.ShowPopup(But_SettingsPic.PointToScreen(But_SettingsPic.Location))
                    End If
                Case Keys.F18
                    If e.Control AndAlso e.Shift Then
                        BringMeToFocus()
                        Playlist_Rowheight = My.Settings.PlaylistRowHeight
                        My.Settings.SkinChanged = True
                        My.Settings.Save()
                        Timer_PlaylistsSizes.Start()
                    End If


            End Select
        End Sub


#End Region '                     <<<<<<<<<<<<                          CONTROLS                                                        >>>>>>>>>>>>


#Region " SEEK"
        Dim firsttime_TimeLabel As Boolean = True


        'VLC
        'Public Sub VLCplayer_MediaPlayerPositionChanged(sender As Object, e As Vlc.DotNet.Core.VlcMediaPlayerPositionChangedEventArgs)
        Public Sub VLCplayer_MediaPlayerPositionChanged(sender As Object, e As DVLCEvents_MediaPlayerPositionChangedEvent) ' Handles VlcPlayer.MediaPlayerPositionChanged
            If VLCChapterMarks.Visible = True Then
                If VLCChapterMarks_IsClicking = False Then
                    VLCChapterMarks.SelectedIndex = VlcPlayer.input.chapter.track
                End If

            End If

            If VLCmediaOpened Then
                VLCTimerSeekCode()
            End If
            Try
                '//////////////Taskbar///////////////
                'ProgressBar
                Dim pos As Double = VlcPlayer.input.position
                If My.Settings.DriveMode Then
                    Dim hWnd As IntPtr = FindWindow(Nothing, CarForm.Text)
                    Taskbar.TaskbarManager.Instance.SetProgressValue(pos * 100, 100, hWnd)
                Else
                    Taskbar.TaskbarManager.Instance.SetProgressValue(pos * 100, 100)
                End If

                If Not VlcPlayer.playlist.isPlaying = True Then ' If Not VlcPlayer.playlist.isPlaying = True Then
                    SetTaskbarState(TaskbarState.Paused)
                Else
                    SetTaskbarState(TaskbarState.Playing)
                End If
                '/////////////End Taskbar///////////
            Catch
            End Try
        End Sub
        Public Sub VLCTimerSeekCode()
            If VLC_installed = False Then Return
            Dim But_PlayPause As PictureBox
            Dim TrackBar_Seek2 As RichTrackBar

            If My.Settings.DriveMode Then
                But_PlayPause = xcarform.But_PlayPause
                TrackBar_Seek2 = xcarform.TrackBar_Seek2
            Else
                But_PlayPause = Me.But_PlayPause
                TrackBar_Seek2 = Me.TrackBar_Seek2
            End If

            Try
                If SeekBarDown Then Return
                If VlcPlayer.playlist.isPlaying = False Then Return ' If VlcPlayer.playlist.isPlaying = False Then Return
                If isVLCplaying = False Then Return

                Dim total As TimeSpan = TimeSpan.FromMilliseconds(VlcPlayer.input.length)
                Dim pos As Double = VlcPlayer.input.position

                Setup_Time_LabelsVLC()
                Try
                    TrackBar_Seek2.Maximum = SeekBarMaxVal
                    TrackBar_Seek2.Value = pos * SeekBarMaxVal
                Catch
                End Try

                Try
                    If VlcPlayer.input.position > 0.999999 Then
                        SubtitleMenu_Clear()
                        AudioMenu_Clear()
                    End If
                    If VlcPlayer.input.position = 0 Then
                        SubtitleMenu_Clear()
                        AudioMenu_Clear()
                    End If
                    If VlcPlayer.input.position > 0 Then
                        If SubtitlesAdded Then Return

                        RefreshSubtitles()
                        RefreshAudios()

                        SubtitlesAdded = True
                        AudiosAdded = True
                    End If
                Catch

                End Try


                Try
                    '//////////////Taskbar///////////////
                    'ProgressBar
                    If My.Settings.DriveMode Then
                        Dim hWnd As IntPtr = FindWindow(Nothing, CarForm.Text)
                        Taskbar.TaskbarManager.Instance.SetProgressValue(pos * 100, 100, hWnd)
                    Else
                        Taskbar.TaskbarManager.Instance.SetProgressValue(pos * 100, 100)
                    End If

                    If Not VlcPlayer.playlist.isPlaying = True Then ' If Not VlcPlayer.playlist.isPlaying = True Then
                        SetTaskbarState(TaskbarState.Paused)
                    Else
                        SetTaskbarState(TaskbarState.Playing)
                    End If
                    '/////////////End Taskbar///////////
                Catch
                End Try
            Catch

            End Try
        End Sub
        Public Sub Setup_Time_LabelsVLC()
            If VLC_installed = False Then Return
            'Time Labels
            Dim total As TimeSpan = TimeSpan.FromMilliseconds(VlcPlayer.input.length)
            Dim artwork As PictureBox : Dim Label_Album As Label : Dim Label_Artist As Label : Dim Label_SongName As Label
            Dim But_PlayPause As PictureBox : Dim TrackBar_Seek2 As RichTrackBar : Dim timelabel As Label : Dim totaltimelabel As Label
            If My.Settings.DriveMode Then
                artwork = xcarform.Artwork : Label_SongName = xcarform.Label_SongName : Label_Artist = xcarform.Label_Artist : Label_Album = xcarform.Label_Album
                But_PlayPause = xcarform.But_PlayPause : TrackBar_Seek2 = xcarform.TrackBar_Seek2 : timelabel = xcarform.timelabel : totaltimelabel = xcarform.totaltimelabel
            Else
                artwork = Me.Artwork : Label_SongName = Me.Label_SongName : Label_Artist = Me.Label_Artist : Label_Album = Me.Label_Album
                But_PlayPause = Me.But_PlayPause : TrackBar_Seek2 = Me.TrackBar_Seek2 : timelabel = Me.timelabel : totaltimelabel = Me.totaltimelabel
            End If
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
                    AudioMenu_Clear()
                End If
                If AudioPlayer.Instance.Position = 0 Then
                    SubtitleMenu_Clear()
                    AudioMenu_Clear()
                End If
                If AudioPlayer.Instance.Position > 0 Then
                    If SubtitlesAdded Then Return
                    RefreshSubtitles()
                    RefreshAudios()
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
            Dim artwork As System.Windows.Forms.PictureBox : Dim Label_Album As Label : Dim Label_Artist As Label : Dim Label_SongName As Label
            Dim But_PlayPause As PictureBox : Dim TrackBar_Seek2 As RichTrackBar : Dim timelabel As Label : Dim totaltimelabel As Label
            If My.Settings.DriveMode Then
                artwork = xcarform.Artwork : Label_SongName = xcarform.Label_SongName : Label_Artist = xcarform.Label_Artist : Label_Album = xcarform.Label_Album
                But_PlayPause = xcarform.But_PlayPause : TrackBar_Seek2 = xcarform.TrackBar_Seek2 : timelabel = xcarform.timelabel : totaltimelabel = xcarform.totaltimelabel
            Else
                artwork = Me.Artwork : Label_SongName = Me.Label_SongName : Label_Artist = Me.Label_Artist : Label_Album = Me.Label_Album
                But_PlayPause = Me.But_PlayPause : TrackBar_Seek2 = Me.TrackBar_Seek2 : timelabel = Me.timelabel : totaltimelabel = Me.totaltimelabel
            End If

            If UsingSpotify Then
                '  timelabel.Text = _SpotifyNew.GetPlayback.Timestamp
            Else
                timelabel.Text = current
                totaltimelabel.Text = total
            End If




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
            Try

                If IsVideo = False Then
                    If e.Button = MouseButtons.Left Then
                        timerUpdate.Stop()
                        Timer_Meta_and_Artwork.Stop()
                        'Move to mouse position
                        Dim dblValue As Double

                        If TrackBar_Seek2.Value > (SeekBarMaxVal / 2) Then
                            ' dblValue = (Convert.ToDouble(e.X) / Convert.ToDouble(TrackBar_Seek2.Width - 10)) * (TrackBar_Seek2.Maximum)
                            dblValue = (Convert.ToDouble(e.X) / Convert.ToDouble(TrackBar_Seek2.Width - 10)) * (TrackBar_Seek2.Maximum - TrackBar_Seek2.Minimum)

                        Else
                            dblValue = (Convert.ToDouble(e.X) / Convert.ToDouble(TrackBar_Seek2.Width)) * (TrackBar_Seek2.Maximum - TrackBar_Seek2.Minimum)
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

                        Timer_Seek.Stop()
                        Timer_Meta_and_Artwork.Stop()
                        'Move to mouse position
                        Dim dblValue As Double
                        Dim use As Boolean = False
                        If use Then
                            If TrackBar_Seek2.Value > (SeekBarMaxVal / 2) Then
                                dblValue = (Convert.ToDouble(e.X) / Convert.ToDouble(TrackBar_Seek2.Width - 10)) * (TrackBar_Seek2.Maximum - TrackBar_Seek2.Minimum)
                            Else
                                dblValue = (Convert.ToDouble(e.X) / Convert.ToDouble(TrackBar_Seek2.Width)) * (TrackBar_Seek2.Maximum - TrackBar_Seek2.Minimum)
                            End If

                            Try
                                TrackBar_Seek2.Value = Convert.ToInt32(dblValue)
                            Catch ex As Exception
                                If ex.ToString.Contains("System.OverflowException") Then
                                    TrackBar_Seek2.Value = SeekBarMaxVal
                                End If
                            End Try
                        End If


                        SeekBarDown = True
                    End If
                End If
            Catch
            End Try
            TrackBar_Seek2.Text = TrackBar_Seek2.Value
        End Sub
        Public Sub seekbar_MouseUp(sender As Object, e As MouseEventArgs) Handles TrackBar_Seek2.MouseUp
            If e.Button = MouseButtons.Right Then
                CustomizeSliders()

                Return
            End If
            If UsingSpotify Then
                Try
                    For i As Integer = 0 To _SpotifyNew.GetDevices.Devices.Count - 1
                        If _SpotifyNew.GetDevices.Devices(i).IsActive Then
                            _SpotifyNew.SeekPlayback(TrackBar_Seek2.Value, _SpotifyNew.GetDevices.Devices(i).Id)
                        End If
                    Next
                    Meta_and_Artwork_Timer2.Start()
                Catch ex As Exception

                End Try


                Return
            End If
            If IsVideo = False Then
                If SeekBarDown = True Then
                    If waveForm Is Nothing Then
                        Return
                    End If
                    ghostCursorPosition = -1

                    If e.Button = MouseButtons.Left Then
                        Dim pos As Long = (TrackBar_Seek2.Value / (SeekBarMaxVal)) * AudioPlayer.Instance.CurrentAudioHandle.LengthInBytes
                        Bass.BASS_ChannelSetPosition(AudioPlayer.Instance.CurrentAudioHandle.CurrentHandle, pos)

                    End If
                    timerUpdate.Start()
                End If
                SeekBarDown = False

            Else
                If SeekBarDown = True Then
                    If VLC_installed Then
                        VlcPlayer.input.position = TrackBar_Seek2.Value / (SeekBarMaxVal)
                    End If

                    Timer_Seek.Start()
                End If
                SeekBarDown = False
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
            If UsingSpotify Then

                AddHandler Spotify_AB_BGW.DoWork, AddressOf Spotify_AB_BGW_DoWork
                If Not Spotify_AB_BGW.IsBusy Then
                    SpotifyABclick = "A"
                    Spotify_AB_BGW.RunWorkerAsync()
                End If


                Return
            End If

            If IsVideo Then
                If VLC_installed Then
                    Dim current As TimeSpan = TimeSpan.FromMilliseconds(VlcPlayer.input.time)
                    CurTime = VlcPlayer.input.position
                    If VlcPlayer.playlist.isPlaying Then 'If VlcPlayer.playlist.isPlaying Then
                        Alabel = String.Format("{0}:{1:D2}", current.Minutes, current.Seconds)
                        Alabel2 = VlcPlayer.input.position
                    End If
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
            If UsingSpotify Then
                AddHandler Spotify_AB_BGW.DoWork, AddressOf Spotify_AB_BGW_DoWork
                If Not Spotify_AB_BGW.IsBusy Then
                    SpotifyABclick = "B"
                    Spotify_AB_BGW.RunWorkerAsync()
                End If
                Return
            End If


            If IsVideo Then
                If VLC_installed Then
                    Dim current As TimeSpan = TimeSpan.FromMilliseconds(VlcPlayer.input.time)
                    CurTime = VlcPlayer.input.position
                    If VlcPlayer.playlist.isPlaying Then 'If VlcPlayer.playlist.isPlaying Then
                        Blabel = String.Format("{0}:{1:D2}", current.Minutes, current.Seconds)
                        If Alabel2 = "00:00" Then
                            Blabel2 = "00:00"
                        Else
                            Blabel2 = VlcPlayer.input.position
                        End If
                        If Not Alabel2 = "00:00" Then
                            VlcPlayer.input.position = Alabel2
                        End If
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
        Public Sub abtimer_Tick(sender As Object, e As EventArgs) Handles Timer_AB.Tick
            If FormClosingval Then Return
            If AppOpenFinished = False Then Return

            If UsingSpotify Then

                If Not B_Postion = CLng(0) Then
                    If Not SpotifyPlaying Then Return
                    If B_Postion > A_Postion Then
                        AddHandler Spotify_AB_BGW.DoWork, AddressOf Spotify_AB_BGW_DoWork
                        If Not Spotify_AB_BGW.IsBusy Then
                            SpotifyABclick = "Timer"
                            Spotify_AB_BGW.RunWorkerAsync()
                        End If
                    End If
                End If


                Return
            End If


            If IsVideo Then
                Return
                If VlcPlayer.playlist.isPlaying = False Then Return ' If VlcPlayer.playlist.isPlaying = False Then Return
                If Not Blabel2 = "00:00" Then 'Not Label1.Text = "00:00" And 
                    If Blabel2 > Alabel2 Then
                        If VlcPlayer.input.position >= CDbl(Blabel2) Then
                            VlcPlayer.input.position = CDbl(Alabel2)
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

        Dim SpotifyABclick As String
        Public Sub Spotify_AB_BGW_DoWork(sender As Object, e As DoWorkEventArgs)
            If SpotifyABclick = "Reset" Then Return
            Select Case SpotifyABclick
                Case "Timer"
                    If _SpotifyNew.GetPlayback.ProgressMs >= B_Postion Then
                        For i As Integer = 0 To _SpotifyNew.GetDevices.Devices.Count - 1
                            If _SpotifyNew.GetDevices.Devices(i).IsActive Then
                                _SpotifyNew.SeekPlayback(A_Postion, _SpotifyNew.GetDevices.Devices(i).Id)
                            End If
                        Next
                        ABLength = (B_Postion) - (A_Postion)
                    End If

                Case "A"
                    CurTime = _SpotifyNew.GetPlayback.ProgressMs
                    Dim ts As TimeSpan = TimeSpan.FromMilliseconds(CurTime)
                    If SpotifyPlaying Then
                        A_Postion = CurTime
                        Alabel = String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds)
                        Alabel2 = TrackBar_Seek2.Value
                    End If
                Case "B"
                    CurTime = _SpotifyNew.GetPlayback.ProgressMs
                    Dim ts As TimeSpan = TimeSpan.FromMilliseconds(CurTime)

                    If A_Postion = CLng(0) Then
                        B_Postion = CLng(0)
                        Blabel2 = 0
                    Else
                        B_Postion = CurTime
                        Blabel = String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds)
                        Blabel2 = TrackBar_Seek2.Value
                    End If

                    If Not A_Postion = CLng(0) Then
                        For i As Integer = 0 To _SpotifyNew.GetDevices.Devices.Count - 1
                            If _SpotifyNew.GetDevices.Devices(i).IsActive Then
                                _SpotifyNew.SeekPlayback(A_Postion, _SpotifyNew.GetDevices.Devices(i).Id)
                            End If
                        Next
                    End If


                    Timer_AB.Start()


            End Select

        End Sub
        'Reset all A B
        Public Sub abresetbut2_Click(sender As Object, e As EventArgs) Handles But_AB_Reset.Click
            Reset_AB_Repeat()
        End Sub
        Public Sub Reset_AB_Repeat()
            SpotifyABclick = "Reset"
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
            trackBar_Speed2.Text = trackBar_Speed2.Value
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
                CustomizeSliders()

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
                If VLC_installed Then
                    VlcPlayer.input.rate = speed / 50
                End If


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
                    If VLC_installed Then
                        If VlcPlayer.input.rate = 1 Then
                            VlcPlayer.input.rate = 0.9
                        Else
                            VlcPlayer.input.rate -= 0.1
                        End If
                    End If

                Else
                    trackBar_Speed2.Value -= 1
                    Timer_Rate.Start()
                End If
            Catch
            End Try
        End Sub
        'Normal
        Public Sub normbut2_Click(sender As Object, e As EventArgs) Handles SpeedResetBut.Click, Label_SpeedTextbox.Click
            Speed_Norm()
        End Sub
        Public Sub Speed_Norm()
            Try
                If IsVideo Then
                    If VLC_installed Then
                        VlcPlayer.input.rate = 1
                    End If

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
                    If VLC_installed Then
                        VlcPlayer.input.rate += 0.1
                    End If

                Else
                    trackBar_Speed2.Value += 1
                    Timer_Rate.Start()
                End If
            Catch
            End Try
        End Sub
        'Speed Label
        Public Sub Timerrate_Tick(sender As Object, e As EventArgs) Handles Timer_Rate.Tick
            If AppOpenFinished = False Then Return
            Try
                If IsVideo Then
                    If VLC_installed Then
                        If VlcPlayer.input.rate = 1 Or 2 Or 3 Or 4 Or 5 Then
                            Label_SpeedTextbox.Text = VlcPlayer.input.rate & ".0"
                        Else
                            Label_SpeedTextbox.Text = VlcPlayer.input.rate
                        End If
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
            trackbar_Pitch2.Text = trackbar_Pitch2.Value
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
                CustomizeSliders()

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

        Public Sub TimerLabelConfirmSpeed_Tick(sender As Object, e As EventArgs) Handles Timer_LabelConfirmSpeedFadeIn.Tick
            If AppOpenFinished = False Then Return
            LabelConfirmSpeed.Visible = False
        End Sub

        Public Sub TimerLabelConfirmSpeedFadeOut_Tick(sender As Object, e As EventArgs) Handles Timer_LabelConfirmSpeedFadeOut.Tick
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
                CustomizeSliders()

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
                            If VLC_installed Then
                                VlcPlayer.audio.volume = TrackBar_PlayerVol2.Value
                            End If

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
            TrackBar_PlayerVol2.Text = TrackBar_PlayerVol2.Value
        End Sub
        'Volume
        Public Sub SetPlayerVolume()
            Try


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

            Catch ex As Exception

            End Try
        End Sub


#Region " Hide Player Volume"
        Public Sub HidePlayerVolumeCheckBox_CheckedChanged(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarCheckBox_Player_Volume.CheckedChanged
            HidePlayerVolCB()
        End Sub
        Public Sub HidePlayerVolCB()
            If BarCheckBox_Player_Volume.Checked = False Then
                TrackBar_PlayerVol2.Visible = False
            Else
                TrackBar_PlayerVol2.Visible = True
            End If
            My.Settings.EnablePlayerVolumeCheckState = BarCheckBox_Player_Volume.Checked
            My.Settings.Save()
        End Sub
        Public Sub ChangeValuePlayerVol()
            BarCheckBox_Player_Volume.Checked = Not BarCheckBox_Player_Volume.Checked
        End Sub

#End Region

#End Region


#End Region '


#End Region

#Region " ADD / REMOVE MEDIA    |   Shuffle Play"
#Region " Declarations & GetFiles"
        Dim OpeningFilesOrFolders As Integer = 0
        Dim AddingFiles As Boolean = False
        Public Shared Function GetFiles(path As String, searchPattern As String, searchOption As SearchOption) As String()
            Dim searchPatterns As String() = searchPattern.Split("|"c)
            Dim files As New List(Of String)()
            For Each sp As String In searchPatterns
                files.AddRange(System.IO.Directory.GetFiles(path, sp, searchOption))
            Next
            files.Sort()
            Return files.ToArray()
        End Function
#End Region


#Region " Open  |   Media / Rich Playlists / M3U"

        'Open / Add Songs
        Public Sub OpenBut_Click(sender As Object, e As EventArgs) Handles BarBut_FileOpen.ItemClick, But_Add.Click

            OpenButtonClick()

        End Sub
        Public Sub OpenButtonClick()
           
                If OpenFile.ShowDialog = DialogResult.OK Then
                    AddingFile = True
                    Timer_AddingFile.Start()
                    OpeningFilesOrFolders = 1

                    bgw.WorkerReportsProgress = True
                    bgw.WorkerSupportsCancellation = True

                    If bgw.IsBusy = False Then

                        OpenProgressBarBackgroundWorker.RunWorkerAsync()
                        bgw.RunWorkerAsync()
                    End If
                    Dim x As Integer = Splitter.Panel2.Location.X
                    Dim y As Integer = PlaylistTabs.Height
                    PlaylistTabs.SelectedTabPage.Controls.Add(OpenProgressBar)
                    OpenProgressBar.Location = SongArrangePanel.Location
                    OpenProgressBar.Size = SongArrangePanel.Size
                End If

        End Sub

        'Adding File Timer
        Public Sub AddingFileTimer_Tick(sender As Object, e As EventArgs) Handles Timer_AddingFile.Tick
            If AppOpenFinished = False Then Return
            AddingFile = False
            Timer_AddingFile.Stop()
        End Sub


        Public Sub Open()


            AddingFiles = True
            Dim File As String = OpenFile.FileName
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
            Try
                If Not File.EndsWith(".rpl") Then
                    If File.EndsWith(".m3u") Then           ' Add .m3u playlist
                        TabPlaylists_AddM3UPlaylist()
                    ElseIf File.Contains(".vob") Or File.Contains("VIDEO_TS") Or File.Contains("VTS_") Then
                        If VLC_installed Then
                            VlcPlayer.playlist.stop()
                            VLCclearPlaylists()
                            VlcPlayer.playlist.add("dvd:///" & Path.GetPathRoot(File))
                            '  VlcPlayer.SetMedia("dvd:///" & Path.GetPathRoot(File))
                            VlcPlayer.playlist.play()
                        End If

                    ElseIf File.EndsWith(".richplaylist") Then           ' Add .richplaylist Playlist
                        Playlist.Rows.Clear()
                        TabPlaylists_AddRichPlaylists_old()
                        DoubleClickPlay()
                    Else                                            ' Add media files
                        TabPlaylists_AddMediaFiles()
                    End If
                Else                                                   ' Add .rpl Playlist
                    Playlist.Rows.Clear()
                    TabPlaylists_AddRichPlaylists()
                    DoubleClickPlay()
                End If

                If AudioPlayer.Instance.GetStreamStatus = BASSActive.BASS_ACTIVE_PLAYING Then
                ElseIf AudioPlayer.Instance.TrackList.Tracks.Count = 0 Then
                End If
                GC.Collect()
                RefreshApp()
            Catch
            End Try

            AddingFiles = False
            timerUpdate.Start()
            Timer_Meta_and_Artwork.Start()
            Meta_and_Artwork_Timer2.Start()
            If My.Settings.FirstTimeSetup Then
                AppOpenFinished = True
                DoGetMetaInfo = True
                GetMetaInfo()
                GetOtherMetaInfo()
                GetArtwork()
            End If
            My.Settings.SkinChanged = True
            NeedResizeRefresh = True
            Timer_PlaylistsSizes.Start()
        End Sub

        'Adding Rich Playlists
        Public Sub TabPlaylists_AddRichPlaylists()


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
            Timer_Meta_and_Artwork.Stop()
            If OpenFile.FileName.Contains(".richplaylist") Then Return
            LoadGridData(Playlist, OpenFile.FileName)

            Dim Row As Integer = Playlist.CurrentCell.RowIndex
            Dim RowCount As Integer = Playlist.RowCount
            Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString

            Playlist.Rows(0).Selected = True
            CheckIfVideo()
            If IsVideo Then
                If VLC_installed Then
                    VLCclearPlaylists()
                    VlcPlayer.playlist.add("file:///" & SongFilename)
                    VlcPlayer.playlist.play()
                End If

            Else
                Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                AudioPlayer.Instance.TrackList.Tracks.Add(track)
                AudioPlayer.Instance.Play(True)
                SongStartOver = True
            End If

            'Playlist Names
            My.Settings.PlaylistNames.Item(TabIndex) = Path.GetFileNameWithoutExtension(OpenFile.FileName)
            PlaylistTabs.SelectedTabPage.Text = My.Settings.PlaylistNames.Item(TabIndex)

            My.Settings.Save()
            Me.UseWaitCursor = False
            Timer_Meta_and_Artwork.Start()
            GetOtherMetaInfo()
        End Sub
        Public Function TabPlaylists_AddRichPlaylists_old()


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
            Dim ItemNum As Integer = 0
            Dim num As Integer = PlaylistTabs.SelectedTabPageIndex
            Dim ini As String = OpenFile.FileName
            Dim iniCount As Integer = 0
            Dim HasFavorites As Boolean = False

            Using r As System.IO.StreamReader = New System.IO.StreamReader(ini)
                Do While r.EndOfStream = False
                    If r.ReadLine = "[Playlist Item Locations]" Then
                        Do While r.EndOfStream = False
                            If r.ReadLine.StartsWith("Item") Then
                                iniCount += 1
                            Else
                            End If
                        Loop
                    End If
                Loop
            End Using
            Using r As System.IO.StreamReader = New System.IO.StreamReader(ini)
                Do While r.EndOfStream = False
                    If r.ReadLine.Contains("Playlist Item Favorites") Then
                        HasFavorites = True
                    End If
                Loop
            End Using

            If HasFavorites Then
                iniCount = (iniCount / 4) - 1
            Else
                iniCount = (iniCount / 3) - 1
            End If

            For i As Integer = 0 To iniCount
                Playlist.Rows.Add()
                Playlist.Item(0, i).Value = (ReadPlaylist(ini, "Playlist Item Names", "Item" & i, ""))
                Playlist.Item(4, i).Value = (ReadPlaylist(ini, "Playlist Item Favorites", "Item" & i, ""))
                Playlist.Item(5, i).Value = (ReadPlaylist(ini, "Playlist Item Positions", "Item" & i, ""))
                Playlist.Item(6, i).Value = ReadPlaylist(ini, "Playlist Item Locations", "Item" & i, "")
            Next

            AudioPlayer.Instance.ResetTrackList()
            Dim track As Track = AudioController.Track.GetTrack(Playlist.Item(6, Playlist.CurrentRow.Index).Value.ToString, True)
            AudioPlayer.Instance.TrackList.Tracks.Add(track)
            SongStartOver = True
            Timer_Meta_and_Artwork.Start()
            PlaylistTabs.SelectedTabPage.Text = Path.GetFileNameWithoutExtension(OpenFile.FileName)

            My.Settings.Save()
            Me.UseWaitCursor = False
        End Function

        'Adding Media Files
        Public Sub TabPlaylists_AddMediaFiles()


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
            Dim row As Integer '= playlist.currentrow.index
            Dim RowCount As Integer
            Try
                row = Playlist.CurrentCell.RowIndex
            Catch
            End Try
            Try
                RowCount = Playlist.RowCount
            Catch
            End Try
            If RowCount = 0 Then
                firstopen2 = True
                row = 0
            Else
                row = Playlist.CurrentCell.RowIndex
            End If
            Dim n As Integer = 1
            Dim numToDo As Integer = OpenFile.FileNames.Count

            For Each i As String In OpenFile.FileNames
                Playlist.Rows.Add()
                Playlist.Item(6, Playlist.Rows.Count - 1).Value = i
                Dim AudioFile As MediaFile = New MediaFile(i)
                Try
                    Try
                        Playlist.Item(0, Playlist.Rows.Count - 1).Value = AudioFile.General.Properties.Item("Track name")
                    Catch
                        Playlist.Item(0, Playlist.Rows.Count - 1).Value = AudioFile.General.Properties.Item("Track")
                    End Try

                Catch
                    Playlist.Item(0, Playlist.Rows.Count - 1).Value = Path.GetFileNameWithoutExtension(i)
                End Try

                Try
                    Playlist.Item(1, Playlist.Rows.Count - 1).Value = AudioFile.General.Properties.Item("Performer")
                Catch
                    Playlist.Item(1, Playlist.Rows.Count - 1).Value = ""
                End Try

                Try
                    Playlist.Item(2, Playlist.Rows.Count - 1).Value = AudioFile.General.Properties.Item("Album")
                Catch
                    Playlist.Item(2, Playlist.Rows.Count - 1).Value = ""
                End Try

                Playlist.Item(3, Playlist.Rows.Count - 1).Value = ""
                Playlist.Item(4, Playlist.Rows.Count - 1).Value = "False"
                Playlist.Item(5, Playlist.Rows.Count - 1).Value = 0
                row = Playlist.CurrentCell.RowIndex
                bgw.ReportProgress(Convert.ToInt32((n / numToDo) * 100))
                n += 1
            Next
            If firstopen2 Then
                Playlist.Rows(0).Selected = True
                row = 0
            End If
            Dim SongFilename As String = Playlist.Item(6, row).Value.ToString
            Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
            Timer_Meta_and_Artwork.Start()

            If firstopen2 Then
                Timer_Seek.Stop()

                AudioPlayer.Instance.Play(True)
                SongStartOver = True
                Timer_Seek.Start()
                Timer_Meta_and_Artwork.Start() ' Set Artwork
                firstopen2 = False
            End If
        End Sub
        'Add M3U Playlist
        Public Sub TabPlaylists_AddM3UPlaylist()


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
            Dim RowCount As Integer = Playlist.RowCount
            Dim row As Integer
            Dim SongFilename As String
            If RowCount = 0 Then firstopen2 = True

            Try
                Using sr As New StreamReader(OpenFile.FileName)
                    Dim line As String

                    While (AudioPlayer.InlineAssignHelper(line, sr.ReadLine())) IsNot Nothing
                        If line.Substring(0, 1).Equals("#") = False Then
                            Dim tracks As Track = AudioController.Track.GetTrack(line, False)
                            Playlist.Rows.Add()
                            Playlist.Item(6, Playlist.Rows.Count - 1).Value = Path.GetDirectoryName(OpenFile.FileName) & "\" & line
                            Dim AudioFile As MediaFile = New MediaFile(Path.GetDirectoryName(OpenFile.FileName) & "\" & line)
                            Try
                                Try
                                    Playlist.Item(0, Playlist.Rows.Count - 1).Value = AudioFile.General.Properties.Item("Track name")
                                Catch
                                    Playlist.Item(0, Playlist.Rows.Count - 1).Value = AudioFile.General.Properties.Item("Track")
                                End Try

                            Catch
                                Playlist.Item(0, Playlist.Rows.Count - 1).Value = (Path.GetFileNameWithoutExtension(OpenFile.FileName) & "\" & line)
                            End Try

                            Try
                                Playlist.Item(1, Playlist.Rows.Count - 1).Value = AudioFile.General.Properties.Item("Performer")
                            Catch
                                Playlist.Item(1, Playlist.Rows.Count - 1).Value = ""
                            End Try

                            Try
                                Playlist.Item(2, Playlist.Rows.Count - 1).Value = AudioFile.General.Properties.Item("Album")
                            Catch
                                Playlist.Item(2, Playlist.Rows.Count - 1).Value = ""
                            End Try
                            Playlist.Item(3, Playlist.Rows.Count - 1).Value = ""
                            Playlist.Item(4, Playlist.Rows.Count - 1).Value = "False"
                            Playlist.Item(5, Playlist.Rows.Count - 1).Value = 0

                        End If
                    End While
                End Using
            Catch e As Exception
                ' Let the user know what went wrong.
                MyMsgBox.Show("Error 0x36: " & e.ToString, "", True)
            End Try
            If firstopen2 Then
                Playlist.Rows(0).Selected = True

            End If
            SongFilename = Playlist.Item(6, row).Value.ToString
            Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
            AudioPlayer.Instance.TrackList.Tracks.Add(track)
            Timer_Meta_and_Artwork.Start()
            If firstopen2 Then
                Timer_Seek.Stop()
                AudioPlayer.Instance.Play(True)
                SongStartOver = True
                Timer_Seek.Start()
                Timer_Meta_and_Artwork.Start() ' Set Artwork
                firstopen2 = False
            End If
            GetOtherMetaInfo()
        End Sub





#End Region
#Region " Open  |   Background Worker"
        Dim WithEvents bgw As New BackgroundWorker
        Dim frmProg As New ProgressBarForm
        Dim bgwDone As Boolean = False


        Public Sub bgw_DoWork(sender As Object, e As DoWorkEventArgs)


            Try
                bgwDone = False
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

                Dim files As String() '= GetFiles(OpenFolder.FileName, "*.wav|*.ogg|*.flac|*.aiff|*.aac|*.ape|*.m4a|*.wma|*.mp3|*.cda", SearchOption.AllDirectories)
                Select Case OpeningFilesOrFolders
                    Case 1
                        Open()
                    Case 2
                        files = GetFiles(OpenFolder.FileName, "*.wav|*.ogg|*.flac|*.aiff|*.aac|*.ape|*.m4a|*.wma|*.mp3|*.cda", SearchOption.TopDirectoryOnly)
                    Case 3
                        files = GetFiles(OpenFolder.FileName, "*.wav|*.ogg|*.flac|*.aiff|*.aac|*.ape|*.m4a|*.wma|*.mp3|*.cda", SearchOption.AllDirectories)

                End Select

                If files.Count > 0 Then
                    Playlist.Rows.Clear()
                    AudioPlayer.Instance.ResetTrackList()
                End If

                If OpeningFilesOrFolders <> 1 Then
                    Dim n As Integer = 1
                    Dim numToDo As Integer = files.Count
                    For Each file As String In files
                        Dim fileName As String = Path.GetFileNameWithoutExtension(file)
                        Dim item As New ListViewItem(Convert.ToString("  ") & fileName, 0)
                        item.Tag = file
                        Playlist.Rows.Add()
                        Playlist.Item(6, Playlist.Rows.Count - 1).Value = file
                        Playlist.Item(0, Playlist.Rows.Count - 1).Value = fileName
                        Playlist.Item(1, Playlist.Rows.Count - 1).Value = ""
                        Playlist.Item(2, Playlist.Rows.Count - 1).Value = ""
                        Playlist.Item(3, Playlist.Rows.Count - 1).Value = ""
                        Playlist.Item(4, Playlist.Rows.Count - 1).Value = "False"
                        Playlist.Item(5, Playlist.Rows.Count - 1).Value = 0
                        '  OpenProgressBarBackgroundWorker.ReportProgress(Convert.ToInt32((n / numToDo) * 100))
                        '  n += 1
                    Next
                End If

                Select Case OpeningFilesOrFolders
                    Case 1

                    Case 2
                        Timer_Seek.Stop()
                        AudioPlayer.Instance.ResetTrackList()
                        Playlist.Rows(0).Selected = True
                        Dim row As Integer = 0
                        Dim SongFilename As String = Playlist.Item(6, row).Value.ToString
                        Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                        AudioPlayer.Instance.TrackList.Tracks.Add(track)
                        firstopen2 = False
                        SongStartOver = True
                        Timer_Seek.Start()
                        Timer_Meta_and_Artwork.Start() ' Set Artwork
                        Cursor.Current = Cursors.Default

                    Case 3
                        Dim Row As Integer = Playlist.CurrentCell.RowIndex
                        Dim RowCount As Integer = Playlist.RowCount
                        Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                        Timer_Seek.Stop()
                        AudioPlayer.Instance.ResetTrackList()
                        Timer_Seek.Start()
                        Timer_Meta_and_Artwork.Start() ' Set Artwork
                        Cursor.Current = Cursors.Default
                        If Playlist.Rows(vbNull).Selected = True Then
                            Playlist.Rows(0).Selected = True
                        End If
                        If IsAddFolder = False Then Return
                        Playlist.Rows(0).Selected = True
                        Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                        AudioPlayer.Instance.TrackList.Tracks.Add(track)
                        firstopen2 = False
                        SongStartOver = True
                        Timer_Seek.Start()
                        Timer_Meta_and_Artwork.Start() ' Set Artwork
                        Cursor.Current = Cursors.Default
                End Select

                Scroller.LookAndFeel.UseDefaultLookAndFeel = False
                Scroller.LookAndFeel.SkinName = "DevExpress Style"
                Scroller.LookAndFeel.SkinName = "DevExpress Dark Style"
                Scroller.Refresh()
            Catch ex As Exception
                '  MsgBox(ex.ToString)
            End Try
        End Sub
        Public Sub bgw_ProgressChanged(sender As Object, e As ProgressChangedEventArgs)
            UpdateOpenProgress(e.ProgressPercentage)
        End Sub

        Public Sub bgw_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) 'Handles bgw.RunWorkerCompleted


            OpenProgressBar.Visible = False
            bgwDone = True
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

            Select Case OpeningFilesOrFolders
                Case 1

                Case 2
                    Timer_Seek.Stop()
                    AudioPlayer.Instance.ResetTrackList()


                    Playlist.Rows(0).Selected = True
                    Dim row As Integer = 0
                    Dim SongFilename As String = Playlist.Item(6, row).Value.ToString
                    Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                    AudioPlayer.Instance.TrackList.Tracks.Add(track)
                    firstopen2 = False


                    SongStartOver = True
                    Timer_Seek.Start()
                    Timer_Meta_and_Artwork.Start() ' Set Artwork

                    Cursor.Current = Cursors.Default

                Case 3
                    Dim Row As Integer = Playlist.CurrentCell.RowIndex
                    Dim RowCount As Integer = Playlist.RowCount
                    Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString


                    Timer_Seek.Stop()
                    AudioPlayer.Instance.ResetTrackList()
                    Timer_Seek.Start()
                    Timer_Meta_and_Artwork.Start() ' Set Artwork

                    Cursor.Current = Cursors.Default


                    If Playlist.Rows(vbNull).Selected = True Then
                        Playlist.Rows(0).Selected = True
                    End If



                    If IsAddFolder = False Then Return

                    Playlist.Rows(0).Selected = True
                    Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                    AudioPlayer.Instance.TrackList.Tracks.Add(track)
                    firstopen2 = False

                    SongStartOver = True
                    Timer_Seek.Start()
                    Timer_Meta_and_Artwork.Start() ' Set Artwork
                    Cursor.Current = Cursors.Default

                    Timer_Meta_and_Artwork.Start()

            End Select
            Meta_and_Artwork_Timer2.Start()
            Scroller.LookAndFeel.UseDefaultLookAndFeel = False
            Scroller.LookAndFeel.SkinName = "DevExpress Style"
            Scroller.LookAndFeel.SkinName = "DevExpress Dark Style"
            Scroller.Refresh()
        End Sub

        'Progress Bar BG_Worker
        Public Sub OpenProgressBarBackgroundWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) ' Handles OpenProgressBarBackgroundWorker.DoWork
            Try
                OpenProgressBar.Visible = True
                For i = 0 To OpenProgressBar.Properties.Maximum
                    OpenProgressBarBackgroundWorker.ReportProgress(i / 100)
                Next
            Catch
            End Try
        End Sub
        Public Sub OpenProgressBarBackgroundWorker_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) 'Handles OpenProgressBarBackgroundWorker.ProgressChanged
            OpenProgressBar.EditValue = e.ProgressPercentage
            If OpenProgressBar.EditValue = 100 Then
                OpenProgressBar.Visible = False
            Else
                OpenProgressBar.Visible = True
            End If
        End Sub
        Public Sub OpenProgressBarBackgroundWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) 'Handles OpenProgressBarBackgroundWorker.RunWorkerCompleted


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
            Scroller.LookAndFeel.UseDefaultLookAndFeel = False
            Scroller.LookAndFeel.SkinName = "DevExpress Style"
            Scroller.LookAndFeel.SkinName = "DevExpress Dark Style"
            Scroller.Refresh()
        End Sub

        Public Sub UpdateOpenProgress(pct As Integer)



            OpenProgressBar.EditValue = pct
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
            Scroller.LookAndFeel.UseDefaultLookAndFeel = False
            Scroller.LookAndFeel.SkinName = "DevExpress Style"
            Scroller.LookAndFeel.SkinName = "DevExpress Dark Style"
            Scroller.Refresh()
        End Sub

#End Region

#Region " Open  |   by Folder"

        'Open Parent Folder Only
        Public Sub BarButtonOpenFolder_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_OpenFolder.ItemClick
            OpenFolderSub()
        End Sub
        Public Sub OpenFolderSub()
            Try
                OpenFolder.Title = "Open Media Files from Selected Folder"
                BringMeToFocus()
                If OpenFolder.ShowDialog = CommonFileDialogResult.Ok Then ' If ShufflePlayFileOpener.ShowDialog() = DialogResult.OK Then
                    'PlayFolder()
                    OpeningFilesOrFolders = 2
                    PlayParentFolderOnly()
                    Timer_Meta_and_Artwork.Start()
                End If
            Catch ex As Exception
                MyMsgBox.Show("Error 0x:40 " & ex.Message + vbLf & vbLf + ex.StackTrace, "", True)
            End Try
        End Sub
        'Open Folder
        Public Sub BarButtonOpenFolderandSub_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_OpenFolderandSub.ItemClick
            OpenFolderSubSub()
        End Sub

        Public Sub OpenFolderSubSub()
            Try
                OpenFolder.Title = "Open Media Files from Selected Folder and its SubFolders"
                BringMeToFocus()
                If OpenFolder.ShowDialog() = DialogResult.OK Then
                    ClearPlaylists()
                    IsAddFolder = True
                    OpeningFilesOrFolders = 3
                    PlayFolder()
                    IsAddFolder = False
                    Timer_Meta_and_Artwork.Start()
                End If
            Catch ex As Exception
                MyMsgBox.Show("Error 0x:41 " & ex.Message + vbLf & vbLf + ex.StackTrace, "", True)
            End Try

        End Sub

        'Add Folder and Subfolders
        Dim IsAddFolder As Boolean = False
        Public Sub AddFolderAndSubBut_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_AddFolderAndSub.ItemClick
            AddFolderandSub()
        End Sub
        Public Sub AddFolderandSub()
            Try
                OpenFolder.Title = "Add Media Files from Selected Folder and its SubFolders"
                BringMeToFocus()
                If OpenFolder.ShowDialog() = DialogResult.OK Then
                    OpeningFilesOrFolders = 3
                    PlayFolder()
                    Timer_Meta_and_Artwork.Start()
                End If
            Catch ex As Exception
                MyMsgBox.Show("Error 0x42: " & ex.Message + vbLf & vbLf + ex.StackTrace, "", True)
            End Try
        End Sub


        ''Open Folder Code
        Public Sub ClearPlaylists()


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
            Playlist.Rows.Clear()
        End Sub
        Public Sub PlayFolder()


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
            Cursor.Current = Cursors.WaitCursor
            AudioPlayer.Instance.ResetTrackList()
            bgw.WorkerReportsProgress = True
            bgw.WorkerSupportsCancellation = True

            If bgw.IsBusy = False Then

                OpenProgressBarBackgroundWorker.RunWorkerAsync()
                bgw.RunWorkerAsync()
            End If
            Dim x As Integer = Splitter.Panel2.Location.X '(Me.Width / 2) - (frmProg.Width / 2)
            Dim y As Integer = PlaylistTabs.Height '- SongArrangePanel.Height '(Me.Height / 2) - (frmProg.Height / 2) '/// allow extra for the toolbox ( hence 2.2 )

            PlaylistTabs.SelectedTabPage.Controls.Add(OpenProgressBar)
            OpenProgressBar.Location = SongArrangePanel.Location
            OpenProgressBar.Size = SongArrangePanel.Size

        End Sub
        Public Sub PlayParentFolderOnly()


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
            Cursor.Current = Cursors.WaitCursor
           

            bgw.WorkerReportsProgress = True
            bgw.WorkerSupportsCancellation = True

            If bgw.IsBusy = False Then
                OpenProgressBarBackgroundWorker.RunWorkerAsync()
                bgw.RunWorkerAsync()         ' do some important work x10
            End If
            Dim x As Integer = Splitter.Panel2.Location.X '(Me.Width / 2) - (frmProg.Width / 2)
            Dim y As Integer = PlaylistTabs.Height '- SongArrangePanel.Height '(Me.Height / 2) - (frmProg.Height / 2) '/// allow extra for the toolbox ( hence 2.2 )
            PlaylistTabs.SelectedTabPage.Controls.Add(OpenProgressBar)
            OpenProgressBar.Location = SongArrangePanel.Location
            OpenProgressBar.Size = SongArrangePanel.Size
        End Sub



#End Region

#Region " Drag & Drop files"

        ' Setup
        Public Sub SetupArtworkDragandDrop()
            Artwork.AllowDrop = False
            Me.AllowDrop = False
            Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next
            ' 'Playlist.AllowDrop = False

            Utilities.ElevatedDragDropManager.Instance.EnableDragDrop(Playlist.Handle)

            Utilities.ElevatedDragDropManager.Instance.EnableDragDrop(Artwork.Handle)
            AddHandler Utilities.ElevatedDragDropManager.Instance.ElevatedDragDrop, AddressOf Artwork_DragDrop

        End Sub

        ' Drag Drop
        Public Sub Artwork_DragDrop(sender As Object, e As Utilities.ElevatedDragDropArgs) 'Handles Artwork.DragDrop
            Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next

            If e.HWnd = Artwork.Handle Then
                For Each file In e.Files
                    OpenFile.FileName = file
                    Open()

                Next
            ElseIf e.HWnd = Playlist.Handle Then
                For Each file In e.Files
                    OpenFile.FileName = file
                    Open()

                Next
            End If

        End Sub



#End Region


#Region " Shuffle Play"
        Dim ShufflePlayFileOpener As New FolderBrowserDialog

        'Get Files from Shuffle Play

        'Shuffle Play
        Public Sub BarButtonShufflePlay_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_ShufflePlay.ItemClick

            ShufflePlay()
        End Sub
        Public Sub ShufflePlay()

            Try
                ShufflePlayFileOpener.Description = "Randomly Play Media From Selected Folder and its SubFolders"
                BringMeToFocus()
                If ShufflePlayFileOpener.ShowDialog() = DialogResult.OK Then
                    'Set to Shuffle
                    IsShuffle = True
                    But_Shuffle.BackgroundImage = ShuffleImage
                    My.Settings.Shuffle = 1
                    PlayFolder()
                    MediaFinishedSub()
                    Dim TabIndex As Integer = PlaylistTabs.SelectedTabPageIndex
                    Dim myvalue As String = "Shuffle Play"
                    PlaylistTabs.SelectedTabPage.Text = myvalue
                    My.Settings.Save()
                End If
            Catch ex As Exception
                MyMsgBox.Show("Error 0x39: " & ex.Message + vbLf & vbLf + ex.StackTrace, "", True)
            End Try
        End Sub


#End Region

#Region " Remove"

        'Ctrl Key Press
        Public Sub playlistboxedit_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown, Me.KeyDown, But_Next.KeyDown
            If e.KeyCode = Keys.ControlKey Then
                ctrlpress = True
            End If
            If e.KeyCode = Keys.ShiftKey Then
                shiftpress = True
            End If
        End Sub
        Public Sub playlistboxedit_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp, Me.KeyUp
            If e.KeyCode = Keys.Escape Then
                If VLC_installed Then
                    If VlcPlayer.video.fullscreen = True Then
                        VlcPlayer.video.fullscreen = False
                    End If
                End If

            End If
            ctrlpress = False
            shiftpress = False
            If e.KeyCode = Keys.Delete Then
                If Not UsingSpotify Then
                    DeleteSong()
                End If
            End If
        End Sub

        'Minus Button
        Public Sub MinusBut1_Click(sender As Object, e As EventArgs) Handles But_Minus.Click
            DeleteSong()
        End Sub
        Public Sub DeleteSong()


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

                If ctrlpress Or shiftpress Then
                    Playlist.Rows.Clear()
                Else
                    Try
                        Dim Row As Integer = Playlist.CurrentCell.RowIndex
                        Dim RowCount As Integer = Playlist.RowCount
                        If RowCount <> 0 Then
                            temp_index = Row
                            Try
                                Dim index As Integer = Row

                                Playlist.Rows.RemoveAt(index)

                                Playlist.Rows(temp_index).Selected = True
                            Catch
                            End Try
                        End If
                    Catch
                    End Try
                End If

        End Sub


#End Region


#Region "Extra      | YouTube || Radio Stations"

        'YouTube
        Private Sub BarBut_AddYT_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_AddYT.ItemClick
            AddYTURL()
        End Sub
        Public Sub AddYTURL()
            Dim ytURL As String
            Dim message, title, defaultValue As String : Dim myValue As Object
            message = "Enter a Youtube URL to add to playlist"
            title = "Add video from URL"
            defaultValue = "YouTube URL"

            Dim xform As New MyInputBox : xform.Text = title : xform.Label1.Text = message : xform.TextEdit1.Text = defaultValue
            AddHandler xform.Label1.Click, Sub(sender, e)
                                               Process.Start("www.youtube.com")
                                           End Sub

            Try : If xform.ShowDialog = DialogResult.OK Then
                    myValue = xform.TextEdit1.Text
                    If myValue Is "" Then myValue = defaultValue
                    ytURL = myValue
                End If
            Catch : End Try
            If ytURL = "" Then Return

            Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next
            Dim row As Integer : Dim RowCount As Integer : Try : row = Playlist.CurrentCell.RowIndex : Catch : End Try : Try : RowCount = Playlist.RowCount : Catch : End Try : If RowCount = 0 Then : firstopen2 = True : row = 0 : Else : row = Playlist.CurrentCell.RowIndex : End If

            Try
                Playlist.Rows.Add()
                Playlist.Item(6, Playlist.Rows.Count - 1).Value = GetVideoId(ytURL)

                Dim youtubeService = New YouTubeService(New BaseClientService.Initializer() With {
                  .ApiKey = "AIzaSyAxNfdTGeu5cbvTJliIQ1yWJb0JD46XWQY",
                  .ApplicationName = Me.[GetType]().ToString()
              })

                Dim searchListRequest = youtubeService.Videos.List("snippet")
                searchListRequest.Id = GetVideoId(ytURL)
                Dim searchListResponse = searchListRequest.Execute

                Dim webClient As New System.Net.WebClient
                Dim bytes() As Byte = webClient.DownloadData(searchListResponse.Items(0).Snippet.Thumbnails.Default__.Url)
                Dim stream As New IO.MemoryStream(bytes)
                Dim img As New System.Drawing.Bitmap(stream)
                Dim imgBox As New PictureBox
                imgBox.Image = img

                Playlist.Item(0, Playlist.Rows.Count - 1).Value = searchListResponse.Items(0).Snippet.Title
                Playlist.Item(1, Playlist.Rows.Count - 1).Value = searchListResponse.Items(0).Snippet.ChannelTitle
                Playlist.Item(2, Playlist.Rows.Count - 1).Value = "YouTube Video"
                Playlist.Item(3, Playlist.Rows.Count - 1).Value = ""
                Playlist.Item(4, Playlist.Rows.Count - 1).Value = "False"
                Playlist.Item(5, Playlist.Rows.Count - 1).Value = 0

            Catch ex As Exception
                Playlist.Rows.RemoveAt(Playlist.RowCount - 1)
                ' MsgBox(ex.ToString)
                MyMsgBox.Show("Invalid Youtube URL!", "", True)
            End Try

        End Sub
        Public Sub AddYTBrowserURL()
            Dim ytURL As String = My.Settings.YouTubeURL

            Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next
            Dim row As Integer : Dim RowCount As Integer : Try : row = Playlist.CurrentCell.RowIndex : Catch : End Try : Try : RowCount = Playlist.RowCount : Catch : End Try : If RowCount = 0 Then : firstopen2 = True : row = 0 : Else : row = Playlist.CurrentCell.RowIndex : End If

            Try
                Playlist.Rows.Add()
                Playlist.Item(6, Playlist.Rows.Count - 1).Value = GetVideoId(ytURL)

                Dim youtubeService = New YouTubeService(New BaseClientService.Initializer() With {
                  .ApiKey = "AIzaSyAxNfdTGeu5cbvTJliIQ1yWJb0JD46XWQY",
                  .ApplicationName = Me.[GetType]().ToString()
              })

                Dim searchListRequest = youtubeService.Videos.List("snippet")
                searchListRequest.Id = GetVideoId(ytURL)
                Dim searchListResponse = searchListRequest.Execute

                Dim webClient As New System.Net.WebClient
                Dim bytes() As Byte = webClient.DownloadData(searchListResponse.Items(0).Snippet.Thumbnails.Default__.Url)
                Dim stream As New IO.MemoryStream(bytes)
                Dim img As New System.Drawing.Bitmap(stream)
                Dim imgBox As New PictureBox
                imgBox.Image = img

                Playlist.Item(0, Playlist.Rows.Count - 1).Value = searchListResponse.Items(0).Snippet.Title
                Playlist.Item(1, Playlist.Rows.Count - 1).Value = searchListResponse.Items(0).Snippet.ChannelTitle
                Playlist.Item(2, Playlist.Rows.Count - 1).Value = "YouTube Video"
                Playlist.Item(3, Playlist.Rows.Count - 1).Value = ""
                Playlist.Item(4, Playlist.Rows.Count - 1).Value = "False"
                Playlist.Item(5, Playlist.Rows.Count - 1).Value = 0

            Catch ex As Exception
                Playlist.Rows.RemoveAt(Playlist.RowCount - 1)
                '   MsgBox(ex.ToString)
                MyMsgBox.Show("Invalid Youtube URL!", "", True)
            End Try

        End Sub
        Public Const YoutubeLinkRegex As String = "(?:.+?)?(?:\/v\/|watch\/|\?v=|\&v=|youtu\.be\/|\/v=|^youtu\.be\/)([a-zA-Z0-9_-]{11})+"
        Friend Shared Function GetVideoId(input As String) As String
            Dim regex = New Regex(YoutubeLinkRegex, RegexOptions.Compiled)
            For Each match As Match In regex.Matches(input)
                For Each data As Group In match.Groups.Cast(Of Group)().Where(Function(groupdata) Not groupdata.ToString().StartsWith("http://") AndAlso Not groupdata.ToString().StartsWith("https://") AndAlso Not groupdata.ToString().StartsWith("youtu") AndAlso Not groupdata.ToString().StartsWith("www."))
                    Return data.ToString()
                Next
            Next
            Return String.Empty
        End Function

        'Radio Stations
        Private Sub BarBut_AddRadio_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_AddRadio.ItemClick
            AddRadio()
        End Sub
        Public Sub AddRadio()
            Dim RadioURL As String
            Dim RadioTitle As String
            Dim message, title, defaultValue, default2Value As String : Dim myValue As Object : Dim my2Value As Object
            message = "Enter the URL of the Radio Station to add to playlist." & Environment.NewLine & Environment.NewLine & _
                "Please use onlineradiobox.com for best results"
            title = "Add Radio Station from URL"
            defaultValue = "Radio Staion URL"
            default2Value = "Radio Station Name"

            Dim xform As New MyInputBox2 : xform.Text = title : xform.Label1.Text = message : xform.TextEdit1.Text = defaultValue
            xform.RichLabel1.Text = "Give your Radio Station a name" : xform.TextEdit2.Text = default2Value

            AddHandler xform.Label1.Click, Sub(sender, e)
                                               Process.Start("www.onlineradiobox.com")
                                           End Sub
            Try : If xform.ShowDialog = DialogResult.OK Then
                    myValue = xform.TextEdit1.Text
                    my2Value = xform.TextEdit2.Text
                    If myValue Is "" Then myValue = defaultValue
                    If my2Value Is "" Then my2Value = default2Value
                    RadioURL = myValue
                    RadioTitle = my2Value

                End If
            Catch : End Try
            If RadioURL = "" Then Return

            Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next
            Dim row As Integer : Dim RowCount As Integer : Try : row = Playlist.CurrentCell.RowIndex : Catch : End Try : Try : RowCount = Playlist.RowCount : Catch : End Try : If RowCount = 0 Then : firstopen2 = True : row = 0 : Else : row = Playlist.CurrentCell.RowIndex : End If

            Try
                Playlist.Rows.Add()
                Playlist.Item(6, Playlist.Rows.Count - 1).Value = RadioURL

                Playlist.Item(0, Playlist.Rows.Count - 1).Value = RadioTitle
                Playlist.Item(1, Playlist.Rows.Count - 1).Value = ""
                Playlist.Item(2, Playlist.Rows.Count - 1).Value = "Radio Station"
                Playlist.Item(3, Playlist.Rows.Count - 1).Value = ""
                Playlist.Item(4, Playlist.Rows.Count - 1).Value = "False"
                Playlist.Item(5, Playlist.Rows.Count - 1).Value = 0

            Catch
                Playlist.Rows.RemoveAt(Playlist.RowCount - 1)
                MyMsgBox.Show("Invalid Radio Station URL!", "", True)
            End Try
        End Sub


        Public Sub AddRadioURL()
            Dim RadioURL As String = My.Settings.RadioURL
            Dim RadioTitle As String = My.Settings.RadioStation
           

            Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next
            Dim row As Integer : Dim RowCount As Integer : Try : row = Playlist.CurrentCell.RowIndex : Catch : End Try : Try : RowCount = Playlist.RowCount : Catch : End Try : If RowCount = 0 Then : firstopen2 = True : row = 0 : Else : row = Playlist.CurrentCell.RowIndex : End If

            Try
                Playlist.Rows.Add()
                Playlist.Item(6, Playlist.Rows.Count - 1).Value = RadioURL

                Playlist.Item(0, Playlist.Rows.Count - 1).Value = RadioTitle
                Playlist.Item(1, Playlist.Rows.Count - 1).Value = ""
                Playlist.Item(2, Playlist.Rows.Count - 1).Value = "Radio Station"
                Playlist.Item(3, Playlist.Rows.Count - 1).Value = ""
                Playlist.Item(4, Playlist.Rows.Count - 1).Value = "False"
                Playlist.Item(5, Playlist.Rows.Count - 1).Value = 0

            Catch
                Playlist.Rows.RemoveAt(Playlist.RowCount - 1)
                MyMsgBox.Show("Invalid Radio Station URL!", "", True)
            End Try
        End Sub


#End Region

#End Region
#Region " Save / Load"

        'Save
        Public Sub BarSave_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_SavePlaylist.ItemClick
            SavePlaylist()
        End Sub
        Public Sub SaveSub()


            Dim SavePlaylist As String = SaveFile.FileName
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

            UseWaitCursor = True

            System.IO.File.WriteAllText(SavePlaylist, "")
            Dim temp As Integer = Playlist.CurrentCell.RowIndex
            SaveGridData(Playlist, SavePlaylist)
            Playlist.CurrentCell = Nothing
            Playlist.CurrentCell = Playlist(0, temp)
            Playlist.Refresh()

            UseWaitCursor = False

            My.Settings.PlaylistNames.Item(TabIndex) = Path.GetFileNameWithoutExtension(SaveFile.FileName)
            PlaylistTabs.SelectedTabPage.Text = Path.GetFileNameWithoutExtension(SaveFile.FileName)

            My.Settings.Save()

        End Sub
        Public Sub SavePlaylist()
            If SaveFile.ShowDialog = DialogResult.OK Then
                SaveSub()
            End If
        End Sub


        'Save / Load Grid Playlists
        Public Sub LoadGridData(ByRef ThisGrid As DataGridView, ByVal Filename As String)
            ThisGrid.Rows.Clear()
            Try
                Dim rows As New List(Of DataGridViewRow)()
                For Each ThisLine In IO.File.ReadAllLines(Filename)
                    Dim row As Object() = ThisLine.Split(ControlChars.Tab)
                    Dim row2 As New DataGridViewRow()
                    Dim i As Integer = 0
                    Dim item1 As String = (ThisLine.Split(ControlChars.Tab)(0))
                    i = item1.Length
                    Dim item2 As String = (ThisLine.Split(ControlChars.Tab)(1))
                    i = item2.Length
                    Dim item3 As String = (ThisLine.Split(ControlChars.Tab)(2))
                    i = item3.Length
                    Dim item4 As String = (ThisLine.Split(ControlChars.Tab)(3))
                    i = item4.Length
                    Dim item5 As String = (ThisLine.Split(ControlChars.Tab)(4))
                    i = item5.Length
                    Dim item6 As String = (ThisLine.Split(ControlChars.Tab)(5))
                    i = item6.Length
                    Dim item7 As String = (ThisLine.Split(ControlChars.Tab)(6))

                    row2.CreateCells(ThisGrid)
                    row2.Cells(0).Value = item1
                    row2.Cells(1).Value = item2
                    row2.Cells(2).Value = item3
                    row2.Cells(3).Value = item4
                    row2.Cells(4).Value = item5
                    row2.Cells(5).Value = item6
                    row2.Cells(6).Value = item7

                    Dim c As Color = Color.Gainsboro
                    If item5 = "True" Then
                        c = FavoriteColorChooser.Color
                    Else
                        c = Color.Gainsboro
                    End If

                    row2.Height = Playlist_Rowheight

                    If item6 <> 0 Then
                        row2.DefaultCellStyle.Font = New System.Drawing.Font(FontDialog1.Font.FontFamily, FontDialog1.Font.Size, System.Drawing.FontStyle.Bold, _
                                                        System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                    Else
                        row2.DefaultCellStyle.Font = New System.Drawing.Font(FontDialog1.Font.FontFamily, FontDialog1.Font.Size, System.Drawing.FontStyle.Regular, _
                                                      System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                    End If

                    row2.DefaultCellStyle.ForeColor = c
                    row2.DefaultCellStyle.SelectionForeColor = c
                    rows.Add(row2)
                Next
                ThisGrid.Rows.AddRange(rows.ToArray())
            Catch
            End Try
        End Sub
        Public Sub SaveGridData(ByRef ThisGrid As DataGridView, ByVal Filename As String)
            Dim temp As Integer = ThisGrid.CurrentCell.RowIndex
            ThisGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
            ThisGrid.MultiSelect = True
            ThisGrid.SelectAll()
            IO.File.WriteAllText(Filename, ThisGrid.GetClipboardContent().GetText.TrimEnd)
            ThisGrid.ClearSelection()
            ThisGrid.MultiSelect = False
            ThisGrid.CurrentCell = Nothing
            ThisGrid.CurrentCell = ThisGrid(0, temp)
            ThisGrid.Refresh()
        End Sub


#End Region

#Region " PLAYLISTS"
#Region " Declartions"
        Dim PlaylistClosing As Boolean = False
        Dim AddingPlaylist As Boolean = False
        Dim currentToolTipText As String = ""

        ' Temp Playlists
        Dim playlist0temp As ListBox
        Dim playlistfull0temp As ListBox

        'Re-ordering
        Dim ChangingPlaylistPostions0ItemOrder As Boolean = False
        Dim ChangingPlaylistOrder As Boolean = False

        'Properties
        Public Shared filelocation As String

        'Search
        Dim occurance As New ListBox

        'Row Height
        Dim Playlist_Rowheight As Integer = My.Settings.PlaylistRowHeight


        Dim AllPlaylistsListNumClicked As Integer

#End Region

#Region " Quick Select Playlists"

        Public Sub QuickOpenMENU_Popup(sender As Object, e As EventArgs) Handles MENU_QuickOpen.Popup, QuickOpenMenu.Popup
            Select Case sender.name
                Case "MENU_QuickOpen"
                    SetupQuickPlaylists()
                Case "QuickOpenMenu"
                    SetupQuickPlaylistsNew()
            End Select

        End Sub
        Public Sub SetupQuickPlaylists()
            If My.Settings.QuickOpen <> "" Then
                MENU_QuickOpen.ItemLinks.Clear()
                MENU_QuickOpen.AddItem(BarBut_ChooseDirectory)
                Dim files As String() = GetFiles(Path.GetFullPath(My.Settings.QuickOpen), "*.rpl", SearchOption.TopDirectoryOnly)
                Dim filenum As Integer = 0

                Dim ns As New Global.ns.NumericComparer()
                Array.Sort(files, ns)


                For Each file As String In files
                    Dim fileName As String = Path.GetFileNameWithoutExtension(file)
                    Dim baritm As New BarButtonItem
                    baritm.Caption = fileName
                    baritm.Name = filenum
                    baritm.Tag = file
                    MENU_QuickOpen.AddItem(baritm)
                    AddHandler baritm.ItemClick, AddressOf QuickPlaylistSelector
                    filenum += 1
                Next



                MENU_QuickOpen.ItemAppearance.SetFont(New Font("Segoe UI", 12))
                MENU_QuickOpen.MenuAppearance.AppearanceMenu.SetFont(New Font("Segoe UI", 12))

                Try
                    MENU_QuickOpen.ItemLinks(1).BeginGroup = True
                Catch
                End Try
            End If
        End Sub

        Public Sub SetupQuickPlaylistsNew()
            If My.Settings.QuickOpen <> "" Then
                QuickOpenMenu.ItemLinks.Clear()
                QuickOpenMenu.AddItem(BarBut_ChooseDirectory)
                Dim files As String() = GetFiles(Path.GetFullPath(My.Settings.QuickOpen), "*.rpl", SearchOption.TopDirectoryOnly)
                Dim filenum As Integer = 0

                Dim ns As New Global.ns.NumericComparer()
                Array.Sort(files, ns)
              

                For Each file As String In files
                    Dim fileName As String = Path.GetFileNameWithoutExtension(file)
                    Dim baritm As New BarButtonItem
                    baritm.Caption = Environment.NewLine & fileName
                    baritm.Name = filenum
                    baritm.Tag = file
                    QuickOpenMenu.AddItem(baritm)
                    AddHandler baritm.ItemClick, AddressOf QuickPlaylistSelector
                    filenum += 1
                Next

                QuickOpenMenu.MenuAppearance.AppearanceMenu.SetFont(New Font("Tahoma", 9.5, FontStyle.Bold))
                QuickOpenMenu.MenuAppearance.Menu.ForeColor = Color.Silver

                Try
                    QuickOpenMenu.ItemLinks(1).BeginGroup = True
                Catch
                End Try
            End If
        End Sub
        Public Sub ChooseDirectoryBut_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_ChooseDirectory.ItemClick, barbut_ChooseDirectoryNew.ItemClick
            If OpenFolder.ShowDialog = CommonFileDialogResult.Ok Then
                Try
                    My.Settings.QuickOpen = Path.GetFullPath(OpenFolder.FileName)
                Catch ex As Exception
                    MyMsgBox.Show("Error adding playlists from folder. (Playlist Menu >> Quick Open) Error Info: " _
                                  & Environment.NewLine & Environment.NewLine & ex.ToString, "", True)
                End Try
            End If
        End Sub
        Public Sub QuickPlaylistSelector(sender As Object, e As ItemClickEventArgs)
            Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next

            AddingFile = True
            OpenFile.FileName = Path.GetFullPath(CStr(e.Item.Tag))
            Playlist.Rows.Clear()
            Timer_Meta_and_Artwork.Stop()
            If OpenFile.FileName.Contains(".richplaylist") Then Return
            LoadGridData(Playlist, Path.GetFullPath(CStr(e.Item.Tag)))

            Dim Row As Integer = Playlist.CurrentCell.RowIndex
            Dim RowCount As Integer = Playlist.RowCount
            Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString

            Playlist.Rows(0).Selected = True
            CheckIfVideo()
            If IsVideo Then
                If VLC_installed Then
                    VLCclearPlaylists()
                    VlcPlayer.playlist.add("file:///" & SongFilename)
                    VlcPlayer.playlist.play()
                End If

            Else
                Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                AudioPlayer.Instance.TrackList.Tracks.Add(track)
                AudioPlayer.Instance.Play(True)
                SongStartOver = True
            End If


            'Playlist Names
            My.Settings.PlaylistNames.Item(TabIndex) = Path.GetFileNameWithoutExtension(OpenFile.FileName)
            PlaylistTabs.SelectedTabPage.Text = My.Settings.PlaylistNames.Item(TabIndex)

            My.Settings.Save()

            DoubleClickPlay()
            Timer_Meta_and_Artwork.Start()
            GetOtherMetaInfo()
            AddingFile = False
        End Sub


#End Region

        ' Save All  /   Add    /   Remove
#Region " Playlist  |   Custom Header Buttons"

        Public Sub XtraTabControl1_CustomHeaderButtonClick(sender As Object, e As DevExpress.XtraTab.ViewInfo.CustomHeaderButtonEventArgs) 'Handles PlaylistTabs.CustomHeaderButtonClick
            Dim TabIndex As Integer = PlaylistTabs.SelectedTabPageIndex
            Dim TabCount As Integer = PlaylistTabs.TabPages.Count

            'Add
            If e.Button.Kind = ButtonPredefines.Plus Then
                AddingPlaylist = True
                AddNewPlaylist()

                'Remove
            ElseIf e.Button.Kind = ButtonPredefines.Minus Then
                CloseTab()
                My.Settings.PlaylistsCount = TabCount

            ElseIf e.Button.ToolTip = "Save All Playlists" Then
                '...................Save Playlist Items and Positions...............

                'Grid Playlists
                Try
                    My.Settings.PlaylistsCount = TabCount
                    '...................Save Playlist Items and Positions...............
                    Try

                        For num As Integer = 0 To My.Settings.PlaylistsCount - 1
                            Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.TabPages(num).Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next


                            Dim Row As Integer = Playlist.CurrentCell.RowIndex
                            Dim RowCount As Integer = Playlist.RowCount
                            Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                            If RowCount > 0 Then
                                System.IO.File.WriteAllText(TempPlaylistNew & num & ".rpl", "")

                                Dim temp As Integer = Playlist.CurrentCell.RowIndex
                                SaveGridData(Playlist, TempPlaylistNew & num & ".rpl")
                                Playlist.CurrentCell = Nothing
                                Playlist.CurrentCell = Playlist(0, temp)
                                Playlist.Refresh()


                                My.Settings.LastPlayedSongs.Insert(num, Row)
                                My.Settings.LastPlayedPositions(PlaylistTabs.SelectedTabPageIndex) = AudioPlayer.Instance.Position
                            Else

                            End If
                        Next
                    Catch
                        ClosingErrors = ClosingErrors + "Error 0x21: Unable to save Playlists items! (Playlists cannot be blank when saving/closing app!)" + Environment.NewLine
                    End Try







                    '.......................Last Played Item...................
                    Try

                        For num As Integer = 0 To My.Settings.PlaylistsCount - 1

                            Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.TabPages(num).Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next


                            Dim Row As Integer = Playlist.CurrentCell.RowIndex
                            Dim RowCount As Integer = Playlist.RowCount
                            Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString

                            If RowCount > 0 Then
                                My.Settings.LastPlayedSongs.Item(num) = Row

                            Else


                            End If
                        Next

                    Catch
                        ClosingErrors = ClosingErrors + "Error 0x22: Unable to save other Playlists' Last Played item! (Playlists cannot be blank when saving/closing app!)" + Environment.NewLine
                        ' MyMsgBox.Show("Error 0x22: Unable to save other Playlists' Last Played item!")
                    End Try








                    '..............Playlist Selected................
                    Try
                        My.Settings.PlaylistsSelected = PlaylistTabs.SelectedTabPageIndex
                    Catch
                        ClosingErrors = ClosingErrors + "Error 0x25: Unable to save Selected Playlist status" + Environment.NewLine
                        '  MyMsgBox.Show("Error 0x25: Unable to save Selected Playlist status")
                    End Try






                    '.......................Playlist Names..............
                    My.Settings.PlaylistNames.Clear()
                    For num As Integer = 0 To PlaylistTabs.TabPages.Count - 1
                        My.Settings.PlaylistNames.Insert(num, "")
                        My.Settings.PlaylistNames.Item(num) = PlaylistTabs.TabPages(num).Text.ToString
                    Next
                    MyMsgBox.Show("Saved all Playlists!", "", True)
                Catch ex As Exception
                    MyMsgBox.Show("Error 0x52: Unable to save Playlists items! (Playlist(s) cannot be blank!)" & Environment.NewLine & ex.ToString, "", True)
                End Try

                RefreshPlaylistFavorites()
            ElseIf e.Button.ToolTip = "Quick Select" Then
                Popup_AllPlaylistsList.ItemLinks.Clear()
                For i As Integer = 0 To PlaylistTabs.TabPages.Count - 1
                    Dim baritm As New BarButtonItem
                    baritm.Caption = PlaylistTabs.TabPages(i).Text
                    baritm.Name = i
                    Popup_AllPlaylistsList.AddItem(baritm)
                    AddHandler baritm.ItemClick, AddressOf PlaylistSelector

                Next

                Popup_AllPlaylistsList.ShowPopup(MousePosition)


            End If

        End Sub

#End Region


#Region " Playlists |   Adding"

        Public Sub AddNewPlaylist()


            Dim TabIndex As Integer = PlaylistTabs.SelectedTabPageIndex
            Dim TabCount As Integer = PlaylistTabs.TabPages.Count
            PlaylistTabs.TabPages.Add("Playlist" & (TabCount + 1))
            TabCount = PlaylistTabs.TabPages.Count
            Dim i As Integer = TabCount - 1
            PlaylistTabs.SelectedTabPageIndex = PlaylistTabs.TabPages.Count - 1
            PlaylistTabs.TabPages(PlaylistTabs.TabPages.Count - 1).AllowTouchScroll = True
            My.Settings.PlaylistNames.Add("Playlist" & PlaylistTabs.TabPages.Count)
            'Add Playlist and Picturebox
            Dim Scroller As New Scroller
            Dim Playlist As New GridPlaylist
            Scroller.Controls.Add(Playlist)
            PlaylistTabs.TabPages(PlaylistTabs.TabPages.Count - 1).Controls.Add(SongArrangePanel)
            PlaylistTabs.TabPages(PlaylistTabs.TabPages.Count - 1).Controls.Add(OpenProgressBar)
            PlaylistTabs.TabPages(PlaylistTabs.TabPages.Count - 1).Controls.Add(Scroller)
            '  'Playlist.AllowDrop = False

            AddHandler Playlist.CellMouseDoubleClick, AddressOf DoubleClickPlay
            AddHandler Playlist.SelectionChanged, AddressOf GridPlaylist_SelectionChanged
            AddHandler Playlist.CellMouseUp, AddressOf GridPlaylist_CellMouseDown
            AddHandler Playlist.KeyUp, AddressOf playlistboxedit_KeyUp
            AddHandler Playlist.KeyDown, AddressOf playlistboxedit_KeyDown
            Utilities.ElevatedDragDropManager.Instance.EnableDragDrop(Playlist.Handle)

            Try
                AddHandler PlaylistTabs.TabPages(TabIndex).MouseClick, AddressOf TabPage1_MouseClick
            Catch
            End Try
            RefreshApp()
            Scroller.BringToFront()
            SongArrangePanel.BringToFront()
            OpenProgressBar.BringToFront()
            TabCount = PlaylistTabs.TabPages.Count
            My.Settings.PlaylistsCount = PlaylistTabs.TabPages.Count
            Timer_PlaylistsSizes.Start()
            RefreshApp()
        End Sub
        Public Sub AddSpotifyBut_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_AddSpotify.ItemClick


            PlaylistTabs.TabPages.Add("Spotify")
            OpenSpotify()
            '  Threading.Thread.Sleep(100)
            BarCheckBox_UseSpotifyLocal.Checked = True
            PlaylistTabs.SelectedTabPageIndex = PlaylistTabs.TabPages.Count - 1
            Dim Playlist As New SpotifyGridPlaylist
            Playlist.Name = "SpotifyPlaylist"
            PlaylistTabs.SelectedTabPage.Controls.Add(Playlist)

        End Sub

#End Region
#Region " Playlists |   Removing"

        'Close
        Public Sub CloseTab()


            If PlaylistTabs.TabPages.Count <> 1 Then
                PlaylistClosing = True
                Dim TabCount As Integer = PlaylistTabs.TabPages.Count - 1
                Dim index As Integer = PlaylistTabs.SelectedTabPageIndex
                Try
                    My.Settings.LastPlayedSongs.RemoveAt(index)

                    My.Settings.LastPlayedPositions.RemoveAt(index)
                Catch ex As Exception

                End Try

                My.Settings.PlayWhenTabActive.RemoveAt(index)
                PlaylistTabs.TabPages.RemoveAt(index)
                index -= 1
                PlaylistTabs.SelectedTabPageIndex = (index)
                GC.Collect()
            End If
        End Sub
        'Close with Middle Click
        Public Sub XtraTabControl1_TabMiddleClick(sender As Object, e As DevExpress.XtraTab.ViewInfo.PageEventArgs) ' Handles PlaylistTabs.TabMiddleClick
            CloseTab()
        End Sub


#End Region


        ' Features
#Region " ToolTip   |   song"

        Public Sub GridPlaylist_MouseMove(ByVal sender As Object, ByVal e As DataGridViewCellMouseEventArgs)


            Dim itemIndex As Integer
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
            Dim cellColumnIndex As Integer = -1, cellRowIndex As Integer = -1
            Dim Row As Integer = Playlist.CurrentCell.RowIndex
            Dim col As Integer = Playlist.CurrentCell.ColumnIndex
            Dim RowCount As Integer = Playlist.RowCount
            If e.ColumnIndex <> cellColumnIndex OrElse e.RowIndex <> cellRowIndex Then
                cellColumnIndex = e.ColumnIndex
                cellRowIndex = e.RowIndex
                Dim SongFilename As String = Playlist.Item(6, cellRowIndex).Value.ToString
                If currentToolTipText <> SongFilename Then
                    currentToolTipText = SongFilename.Replace(Chr(38), "&&")
                    Timer_GetItemLocation.Start()
                End If
            End If
        End Sub
        Public Sub Timerplaylistbox_Tick(sender As Object, e As EventArgs) Handles Timer_GetItemLocation.Tick


            If AppOpenFinished = False Then Return
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
                Dim Row As Integer = Playlist.CurrentCell.RowIndex
                Dim RowCount As Integer = Playlist.RowCount
                If RowCount < 1 Then Return
                ToolTip1.SetToolTip(Playlist, currentToolTipText)
                Timer_GetItemLocation.Stop()
            Catch
            End Try
        End Sub


#End Region

#Region " Playlist  |   Re-order Items"

        Public Sub ReOrderButtons() Handles But_MoveUp.Click, But_MoveDown.Click, But_MoveTop.Click, But_MoveBottom.Click
            ScrollPlaylistIntoView()
        End Sub

        Public Sub UpBut_Click(sender As Object, e As EventArgs) Handles But_MoveUp.Click

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

                Dim row As Integer = Playlist.CurrentCell.RowIndex
                Dim RowCount As Integer = Playlist.RowCount - 1


                ChangingPlaylistOrder = True

                If RowCount > 0 Then

                    If row > 0 Then

                        MoveRow(Playlist, -1)


                        ChangingPlaylistOrder = False

                    End If
                End If
            Catch
            End Try



        End Sub
        Public Sub DownBut_Click(sender As Object, e As EventArgs) Handles But_MoveDown.Click
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
                    Dim Row As Integer = Playlist.CurrentCell.RowIndex
                    Dim RowCount As Integer = Playlist.RowCount
                    If RowCount > 0 Then
                        If Row < RowCount - 1 Then
                            MoveRow(Playlist, 1)
                            ChangingPlaylistOrder = False
                        End If
                    End If
                Catch
                End Try
        End Sub
        Public Sub MoveTopBut_Click(sender As Object, e As EventArgs) Handles But_MoveTop.Click

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
                    Dim Row As Integer = Playlist.CurrentCell.RowIndex
                    Dim RowCount As Integer = Playlist.RowCount
                    If RowCount > 0 Then
                        If Row > 0 Then
                            MoveRowTop(Playlist)
                            ChangingPlaylistOrder = False
                        End If
                    End If
                Catch
                End Try
        End Sub
        Public Sub MoveBottomBut_Click(sender As Object, e As EventArgs) Handles But_MoveBottom.Click
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
                    Dim Row As Integer = Playlist.CurrentCell.RowIndex
                    Dim RowCount As Integer = Playlist.RowCount
                    If RowCount > 0 Then
                        If Row < RowCount - 1 Then
                            MoveRowBottom(Playlist)

                            ChangingPlaylistOrder = False
                        End If
                    End If
                Catch
                End Try
        End Sub

        Public Sub MoveRow(ByVal DGV As DataGridView, ByVal i As Integer)
            Try
                If (DGV.SelectedCells.Count > 0) Then
                    Dim curr_index As Integer = DGV.CurrentCell.RowIndex
                    Dim curr_col_index As Integer = DGV.CurrentCell.ColumnIndex
                    Dim curr_row As DataGridViewRow = DGV.CurrentRow
                    DGV.Rows.Remove(curr_row)
                    DGV.Rows.Insert(curr_index + i, curr_row)
                    DGV.CurrentCell = DGV(curr_col_index, curr_index + i)
                End If
            Catch
            End Try
        End Sub
        Public Sub MoveRowTop(ByVal DGV As DataGridView)
            Try
                If (DGV.SelectedCells.Count > 0) Then
                    Dim curr_index As Integer = DGV.CurrentCell.RowIndex
                    Dim curr_col_index As Integer = DGV.CurrentCell.ColumnIndex
                    Dim curr_row As DataGridViewRow = DGV.CurrentRow
                    DGV.Rows.Remove(curr_row)
                    DGV.Rows.Insert(0, curr_row)
                    DGV.CurrentCell = DGV(curr_col_index, 0)
                    ScrollPlaylistIntoView()
                End If
            Catch
            End Try

        End Sub
        Public Sub MoveRowBottom(ByVal DGV As DataGridView)
            Try
                If (DGV.SelectedCells.Count > 0) Then
                    Dim curr_index As Integer = DGV.CurrentCell.RowIndex
                    Dim curr_col_index As Integer = DGV.CurrentCell.ColumnIndex
                    Dim curr_row As DataGridViewRow = DGV.CurrentRow
                    DGV.Rows.Remove(curr_row)
                    DGV.Rows.Insert(DGV.Rows.Count, curr_row)
                    DGV.CurrentCell = DGV(curr_col_index, DGV.Rows.Count - 1)
                    ScrollPlaylistIntoView()
                End If
            Catch
            End Try
        End Sub

#End Region
        Public Sub ScrollPlaylistIntoView()


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

            Dim inter As Integer = ((Playlist.SelectedRows.Item(0).Index) / (Playlist.RowCount - 1)) * (Scroller.VerticalScroll.Maximum - 100)
            Scroller.VerticalScroll.Value = inter
        End Sub

#Region " Playlist  |   Right Click item"


        'Right Click on Playlist

        Public Sub GridPlaylist_CellMouseDown(ByVal sender As Object, ByVal e As DataGridViewCellMouseEventArgs)


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
                Dim Row As Integer = Playlist.CurrentCell.RowIndex ' (DataGridViewElementStates.Selected) - 1
                Dim RowCount As Integer = Playlist.RowCount

                If e.Button = MouseButtons.Right AndAlso e.RowIndex >= 0 Then
                    Playlist.Rows(e.RowIndex).Selected = True
                    Playlist.CurrentCell = Playlist(e.ColumnIndex, e.RowIndex)
                    Popup_Playlist.ShowPopup(MousePosition)

                End If
            Catch
            End Try
        End Sub

        Private Sub Popup_Playlist_Popup(sender As Object, e As EventArgs) Handles Popup_Playlist.Popup
            Try
                Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next : Dim Row As Integer = Playlist.CurrentCell.RowIndex : Dim RowCount As Integer = Playlist.RowCount

                If Playlist.Item(2, Row).Value = "YouTube Video" Then
                    but_OpenWithYouTube.Visibility = BarItemVisibility.Always
                Else
                    but_OpenWithYouTube.Visibility = BarItemVisibility.Never
                End If
            Catch ex As Exception

            End Try


        End Sub


        'Show Properties Window
        Public Sub BarButtonItemProperties_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles ItemPropertiesRtClkBut.ItemClick
            Dim xfrom As New itemproperties
            xfrom.ShowDialog()
            xfrom.artistlabel.Text = Label_Artist.Text
            xfrom.locationlabel.Text = filelocation

        End Sub
        'Open file location
        Public Sub BarButtonFileLocation_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles OpenFileLocationRtClkBut.ItemClick



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
            Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
            Try
                Process.Start("explorer.exe", "/select," & Path.GetFullPath(SongFilename))
            Catch ex As Exception
                MyMsgBox.Show(ex.ToString, "", True)
            End Try

        End Sub

        'Open With YouTube
        Private Sub but_OpenWithYouTube_ItemClick(sender As Object, e As ItemClickEventArgs) Handles but_OpenWithYouTube.ItemClick
            Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next : Dim Row As Integer = Playlist.CurrentCell.RowIndex : Dim RowCount As Integer = Playlist.RowCount

            If Playlist.Item(2, Row).Value = "YouTube Video" Then
                Process.Start("http://www.youtube.com/watch?v=" & Playlist.Item(6, Row).Value.ToString)

            End If

        End Sub

        'Search on YouTube
        Private Sub but_SearchonYouTube_ItemClick(sender As Object, e As ItemClickEventArgs) Handles but_SearchonYouTube.ItemClick
            Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next : Dim Row As Integer = Playlist.CurrentCell.RowIndex : Dim RowCount As Integer = Playlist.RowCount
            Dim s As String = Playlist.Item(1, Row).Value.ToString

            If Not s = "" Then
                Process.Start("https://www.youtube.com/results?search_query=" & HTML_String(s))
            End If
        End Sub
        'Search on YouTube Music
        Private Sub but_SearchonYouTubeMusic_ItemClick(sender As Object, e As ItemClickEventArgs) Handles but_SearchonYouTubeMusic.ItemClick
            Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next : Dim Row As Integer = Playlist.CurrentCell.RowIndex : Dim RowCount As Integer = Playlist.RowCount
            Dim s As String = Playlist.Item(1, Row).Value.ToString

            If Not s = "" Then
                Process.Start("https://music.youtube.com/search?q=" & HTML_String(s))
            End If
        End Sub

        'More
        Public Sub MoreArtistsBut_ItemClick(sender As Object, e As ItemClickEventArgs) Handles MoreArtistsBut.ItemClick
            Try


                CheckIfVideo()

                Dim Artist As String
                Dim Browser As String

                If IsVideo Then
                    If VLC_installed Then
                        Artist = VlcPlayer.playlist.Artist ' 'VlcPlayer.playlist.artist
                    End If

                Else
                    Artist = AudioPlayer.Instance.CurrentTrack.Artist
                End If



                If IO.File.Exists("C:\Program Files (x86)\Google\Chrome\Application\chrome.exe") Then
                    Browser = "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"
                ElseIf IO.File.Exists("C:\Program Files\Google\Chrome\Application\chrome.exe") Then
                    Browser = "C:\Program Files\Google\Chrome\Application\chrome.exe"
                ElseIf IO.File.Exists(Environment.SpecialFolder.LocalApplicationData + "\Google\Chrome\Application\chrome.exe") Then
                    Browser = Environment.SpecialFolder.LocalApplicationData + "\Google\Chrome\Application\chrome.exe"
                Else
                    Browser = ""
                End If

                ' AllMusic.com 
                ' Artist = Artist.Replace(Chr(39), "%20").Replace(" ", "%20").Trim()
                Artist = Artist.Replace(Chr(39), "+").Replace(" ", "+").Trim()
                Try
                    If Browser = "" Then
                        'Process.Start("http://www.allmusic.com/search/all/" + Artist)
                        Process.Start("https://www.last.fm/music/" + Artist)
                    Else
                        ' Process.Start(Browser, "http://www.allmusic.com/search/all/" + Artist)
                        Process.Start(Browser, "https://www.last.fm/music/" + Artist)
                    End If
                Catch
                    MyMsgBox.Show("Error loading internet browser! Check to see that your internet browser is set to your default browser.", "", True)
                End Try
            Catch ex As Exception
            End Try

        End Sub

#End Region
#Region " Playlists |   Rename Playlist & Items     |   Quick Select Playlist from right clicking"


        'Rename Playlist
        Public Sub BarButtonRename_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_RenamePlaylist.ItemClick
            Rename_Playlist()
        End Sub
        'Right Click on tab to rename
        Public Sub XtraTabControl1_MouseUp(sender As Object, e As MouseEventArgs) 'Handles PlaylistTabs.MouseUp


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

            Dim info As BaseTabHitInfo = PlaylistTabs.CalcHitInfo(e.Location)
            If info.HitTest = XtraTabHitTest.PageHeader Then
                If e.Button = MouseButtons.Right Then
                    Popup_PlaylistTab.ShowPopup(MousePosition)
                    Try
                        TEXT_SongCount.Caption = "Song Count: " & Playlist.Rows.Count
                        Dim time As New TimeSpan
                        Dim timeText As String
                        Dim time2 As TimeSpan
                        For i As Integer = 0 To Playlist.Rows.Count - 1
                            timeText = Playlist.Item(3, i).Value.ToString
                            If timeText.Count = 5 Then
                                timeText = "00:" & timeText
                            End If
                            time2 = TimeSpan.ParseExact(timeText, "g", Globalization.CultureInfo.InvariantCulture)
                            time = time.Add(time2)
                        Next
                        TEXT_TotalDurations.Caption = "Total Play Time: " & time.ToString
                    Catch
                        TEXT_SongCount.Caption = ""
                        TEXT_TotalDurations.Caption = ""
                    End Try
                End If


                'Quick Select

            Else 'If info.HitTest = XtraTabHitTest.PageClient Then
                If e.Button = MouseButtons.Right Then
                    Popup_AllPlaylistsList.ItemLinks.Clear()
                    For i As Integer = 0 To PlaylistTabs.TabPages.Count - 1
                        Dim baritm As New BarButtonItem
                        baritm.Caption = PlaylistTabs.TabPages(i).Text
                        baritm.Name = i
                        Popup_AllPlaylistsList.AddItem(baritm)
                        AddHandler baritm.ItemClick, AddressOf PlaylistSelector
                    Next
                    Popup_AllPlaylistsList.ShowPopup(MousePosition)
                End If
            End If
        End Sub
        Public Sub BarButtonItem3_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_Rename.ItemClick
            Rename_Playlist()

        End Sub
        Public Sub Rename_Playlist()


            If PlaylistTabs.SelectedTabPage.Text = "Spotify" Then Return
            Dim TabIndex As Integer = PlaylistTabs.SelectedTabPageIndex
            Dim TempName As String = PlaylistTabs.SelectedTabPage.Text
            Dim message, title, defaultValue As String
            Dim myValue As Object
            message = "Enter a new name for the current playlist"
            title = "Rename Playlist"
            defaultValue = TempName

            Dim xform As New MyInputBox
            xform.Text = title
            xform.Label1.Text = message
            xform.TextEdit1.Text = defaultValue
            Try
                If xform.ShowDialog = DialogResult.OK Then
                    myValue = xform.TextEdit1.Text
                    If myValue Is "" Then myValue = defaultValue
                    PlaylistTabs.SelectedTabPage.Text = myValue
                    My.Settings.Save()
                End If
            Catch
            End Try

        End Sub

        'Select Playlists
        Public Sub PlaylistSelector(sender As Object, e As ItemClickEventArgs)


            PlaylistTabs.SelectedTabPageIndex = CInt(e.Item.Name)

        End Sub

        'Label Item
        Public Sub PlaylistItemLabel_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_RenameCurrentItem.ItemClick, PlaylistItemRtClickMenuBUt.ItemClick

            RenameCurrentItem()
        End Sub

        Public Sub RenameCurrentItem()

            Dim TabPageIndex As Integer = PlaylistTabs.SelectedTabPageIndex
            Dim TempName As String
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
            TempName = Playlist.Item(0, Row).Value.ToString
            Dim message, title, defaultValue As String
            Dim myValue As Object
            message = "Enter a new name for the current item"
            title = "Rename Playlist" & TabPageIndex & " item"
            defaultValue = TempName

            Dim xform As New MyInputBox
            xform.Text = title
            xform.Label1.Text = message
            xform.TextEdit1.Text = defaultValue
            Try
                If xform.ShowDialog = DialogResult.OK Then
                    myValue = xform.TextEdit1.Text
                    If myValue Is "" Then myValue = defaultValue
                    Playlist.Item(0, Row).Value = myValue
                End If
            Catch
            End Try


            Timer_Meta_and_Artwork.Start()
        End Sub

#End Region


#Region " Playlist  |   Color Chooser"

        Public Sub FavoriteColorChooserBut_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_FavoriteColorChooser.ItemClick
            ChooseFavoriteColor()
        End Sub
        Public Sub ChooseFavoriteColor()

            FavoriteColorChooser.ShowDialog()
            My.Settings.FavColor = FavoriteColorChooser.Color
            RefreshFavoriteColors()
            My.Settings.Save()
        End Sub
        Public Sub RefreshFavoriteColors()


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

            Dim col As Color = Color.Gainsboro
            For i As Integer = 0 To Playlist.Rows.Count - 1
                If Playlist.Rows(i).Cells(4).Value = "True" Then
                    col = FavoriteColorChooser.Color
                Else
                    col = Color.Gainsboro
                End If
                Playlist.Rows(i).DefaultCellStyle.ForeColor = col
                Playlist.Rows(i).DefaultCellStyle.SelectionForeColor = col
            Next


        End Sub

#End Region
#Region " Playlist  |   Favorites"

        'Click Favorite Button
        Public Sub FavBut_Click(sender As Object, e As EventArgs) Handles But_Fav.Click
            SuspendDrawing()

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
            Dim ScrollPos As Integer = Scroller.VerticalScroll.Value
            ' Try
            Try


                Dim Row As Integer = Playlist.CurrentCell.RowIndex ' (DataGridViewElementStates.Selected) - 1
                Dim RowCount As Integer = Playlist.RowCount
                Dim SongFavorite As String = Playlist.Item(4, Row).Value.ToString
                Dim curText As String = SongFavorite
                If curText = "False" Then
                    Playlist.Item(4, Row).Value = "True"

                ElseIf curText = "True" Then
                    Playlist.Item(4, Row).Value = "False"

                ElseIf curText = "" Then
                    Playlist.Item(4, Row).Value = "True"

                End If
                Dim b As Color = Color.Gainsboro
                If Playlist.Item(4, Row).Value = "True" Then
                    b = FavoriteColorChooser.Color
                Else
                    b = Color.Gainsboro
                End If

                Playlist.Rows(Row).DefaultCellStyle.ForeColor = b
                Playlist.Rows(Row).DefaultCellStyle.SelectionForeColor = b

                Dim inter As Integer = ((Playlist.Rows(Row).Index) / (Playlist.RowCount - 1)) * (Scroller.VerticalScroll.Maximum - 100)
                Scroller.VerticalScroll.Value = inter
            Catch
            End Try


            My.Settings.FavColor = FavoriteColorChooser.Color
            ScrollPlaylistIntoView()

            Scroller.VerticalScroll.Value = ScrollPos


            ResumeDrawing()
        End Sub

#End Region
#Region " Playlist  |   Positions   /   Seekbar Paint (also mouse enter/leave)"


        'Allow Save
        Public Sub CheckBox_AllowSaveItemPosition_CheckedChanged(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarCheckBox_AllowSaveItemPosition.CheckedChanged


            If AppOpenFinished = False Then Return
            If PlaylistTabs.SelectedTabPage.Text = "Spotify" Then Return
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
            If PlaylistIsReady = False Then Return
            If Playlist.Rows.Count = 0 Then Return
            Dim Row As Integer = Playlist.CurrentCell.RowIndex
            Dim RowCount As Integer = Playlist.RowCount
            If BarCheckBox_AllowSaveItemPosition.Checked Then
                BarBut_PlaylistItemPosition.Enabled = True
                BarBut_PlaylistItemPositionRemove.Enabled = True
            Else
                BarBut_PlaylistItemPosition.Enabled = False
                BarBut_PlaylistItemPositionRemove.Enabled = False
                If AppOpen = False Then
                    My.Settings.SaveItemPosition = 0
                End If
            End If
            PlaylistPositions0_SIC()
        End Sub

        'Save
        Public Sub PlaylistItemPosition_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_PlaylistItemPosition.ItemClick
            SaveItemPositionSub()
        End Sub
        Public Sub SavePositionBut_SAP_Click(sender As Object, e As EventArgs) Handles But_SavePosition_SAP.Click
            SaveItemPositionSub()
        End Sub
        Public Sub SaveItemPositionSub()

            If IsVideo Then
                SaveItemPositionVLCSub()
            Else
                Try
                    If BarCheckBox_AllowSaveItemPosition.Checked Then
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
                        My.Settings.SaveItemPosition = CDbl(AudioPlayer.Instance.Position) ' SeekBar.Value  'CDbl(AudioPlayer.Instance.Position)
                        Playlist.Item(5, Row).Value = My.Settings.SaveItemPosition
                        My.Settings.SaveItemPositionItemName = Playlist.Item(0, Row).Value.ToString
                        My.Settings.SaveItemPositionIndex = Row

                        If BarCheckBox_AllowSaveItemPosition.Checked Then
                            If Playlist.Item(5, Row).Value <> 0 Then
                                Playlist.Rows(Row).DefaultCellStyle.Font = New System.Drawing.Font(FontDialog1.Font.FontFamily, FontDialog1.Font.Size, System.Drawing.FontStyle.Bold, _
                                                                System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                            Else
                                Playlist.Rows(Row).DefaultCellStyle.Font = New System.Drawing.Font(FontDialog1.Font.FontFamily, FontDialog1.Font.Size, System.Drawing.FontStyle.Regular, _
                                                              System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                            End If
                        End If
                    End If
                    PlaylistPositions0_SIC()
                    SeekBarPaint_ErrorShown = False
                Catch
                End Try
            End If
        End Sub
        Public Sub SaveItemPositionVLCSub()


            If BarCheckBox_AllowSaveItemPosition.Checked Then
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
                    Dim Row As Integer = Playlist.CurrentCell.RowIndex
                    Dim RowCount As Integer = Playlist.RowCount
                    Dim SongTitle As String = Playlist.Item(0, Row).Value.ToString
                    If VLC_installed Then
                        My.Settings.SaveItemPosition = VlcPlayer.input.position
                    Else
                        My.Settings.SaveItemPosition = 0
                    End If

                    Playlist.Item(5, Row).Value = My.Settings.SaveItemPosition
                    My.Settings.SaveItemPositionItemName = SongTitle
                    My.Settings.SaveItemPositionIndex = Row

                    If BarCheckBox_AllowSaveItemPosition.Checked Then
                        If Playlist.Item(5, Row).Value <> 0 Then
                            Playlist.Rows(Row).DefaultCellStyle.Font = New System.Drawing.Font(FontDialog1.Font.FontFamily, FontDialog1.Font.Size, System.Drawing.FontStyle.Bold, _
                                                            System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                        Else
                            Playlist.Rows(Row).DefaultCellStyle.Font = New System.Drawing.Font(FontDialog1.Font.FontFamily, FontDialog1.Font.Size, System.Drawing.FontStyle.Regular, _
                                                          System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                        End If
                    End If

                Catch
                End Try
            End If
            PlaylistPositions0_SIC()
        End Sub
        'Right Click Menu on Time Label to Save
        Public Sub SavePositionBut_ItemClick(sender As Object, e As XtraBars.ItemClickEventArgs) Handles SavePositionBut.ItemClick
            If IsVideo Then
                SaveItemPositionVLCSub()
            Else
                SaveItemPositionSub()
            End If
        End Sub

        'Remove
        Public Sub PlaylistItemPositionRemove_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_PlaylistItemPositionRemove.ItemClick
            RemoveItemPositionSub()
        End Sub
        Public Sub RemovePositionBut_SAP_Click(sender As Object, e As EventArgs) Handles But_RemovePosition_SAP.Click
            RemoveItemPositionSub()
        End Sub
        Public Sub RemoveItemPositionSub()


                If BarCheckBox_AllowSaveItemPosition.Checked Then
                    Try
                        If BarCheckBox_AllowSaveItemPosition.Checked Then
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
                            Playlist.Item(5, Row).Value = 0
                            If Playlist.Item(5, Row).Value <> 0 Then
                                Playlist.Rows(Row).DefaultCellStyle.Font = New System.Drawing.Font(FontDialog1.Font.FontFamily, FontDialog1.Font.Size, System.Drawing.FontStyle.Bold, _
                                                                System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                            Else
                                Playlist.Rows(Row).DefaultCellStyle.Font = New System.Drawing.Font(FontDialog1.Font.FontFamily, FontDialog1.Font.Size, System.Drawing.FontStyle.Regular, _
                                                              System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                            End If
                        End If
                    Catch
                    End Try
                End If

            PlaylistPositions0_SIC()
            My.Settings.SaveItemPosition = 0
        End Sub

        'Selected Index Changed
        Public Sub PlaylistPositions0_SIC()



            If BarCheckBox_AllowSaveItemPosition.Checked Then
              
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
                    Try
                        Dim Row As Integer = Playlist.CurrentCell.RowIndex
                        If Playlist.Item(5, Row).Value = 0 Then
                            BarBut_PlaylistItemPositionRemove.Enabled = False
                        Else
                            BarBut_PlaylistItemPositionRemove.Enabled = True
                        End If
                    Catch
                    End Try
                End If

        End Sub


        Public Sub TrackBar_Seek2_MouseEnter() Handles TrackBar_Seek2.MouseEnter
            TrackBar_Seek2.IsMouseEnter = True
        End Sub
        Public Sub TrackBar_Seek2_MouseLeave() Handles TrackBar_Seek2.MouseLeave
            TrackBar_Seek2.IsMouseEnter = False
        End Sub

        'Paint SeekBar Saved Position and A/B Repeat
        Dim SeekBarPaint_ErrorShown As Boolean = False

        Public Sub SeekBar2_Paint(sender As Object, e As PaintEventArgs) Handles TrackBar_Seek2.Paint
            Try
                'VLC
                If IsVideo Then
                    If Not Blabel2 = "00:00" Then
                        Dim val1 As Integer
                        If Alabel2 = "00:00" Or 0 Then
                            val1 = 0
                        Else
                            val1 = Alabel2 * (TrackBar_Seek2.Width + 8)
                        End If
                        ABLength = (Blabel2 - Alabel2) * (TrackBar_Seek2.Width)
                        If My.Settings.MiniModeOn Then
                            e.Graphics.FillRectangle(System.Drawing.Brushes.DeepSkyBlue, val1, 22, (ABLength), 2)
                        Else
                            e.Graphics.FillRectangle(System.Drawing.Brushes.DeepSkyBlue, val1, 27, (ABLength), 2)
                        End If
                    End If
                    If Not (My.Settings.SaveItemPosition = 0) Then
                        Dim val As Integer
                        val = (My.Settings.SaveItemPosition) * (TrackBar_Seek2.Width - 15)
                        If My.Settings.MiniModeOn Then
                            e.Graphics.FillRectangle(System.Drawing.Brushes.Gray, val, 23, 20, 2)
                        Else
                            e.Graphics.FillRectangle(System.Drawing.Brushes.Gray, val, 29, 20, 2)
                        End If

                    End If

                    'BASS.NET
                Else
                    If Not Blabel = "00:00" Then
                        Dim val1 As Integer
                        Dim val2 As Integer
                        If Alabel2 = "00:00" Then
                            val1 = 0
                        Else
                            val1 = Alabel2 / SeekBarMaxVal * (TrackBar_Seek2.Width + 2)
                        End If
                        Try
                            ABLength = (Blabel2 - Alabel2) / SeekBarMaxVal * (TrackBar_Seek2.Width)
                            If My.Settings.MiniModeOn Then
                                e.Graphics.FillRectangle(System.Drawing.Brushes.DeepSkyBlue, val1, 22, (ABLength), 2)
                            Else
                                e.Graphics.FillRectangle(System.Drawing.Brushes.DeepSkyBlue, val1, 27, (ABLength), 2)
                            End If
                        Catch
                        End Try

                    End If
                    If Not (My.Settings.SaveItemPosition = 0) Then


                        Dim val As Integer
                        Dim pos As Double = My.Settings.SaveItemPosition
                        Dim Len As Double = (AudioPlayer.Instance.CurrentAudioHandle.LengthInBytes)
                        val = ((pos / Len)) * (TrackBar_Seek2.Width - 15)

                        Try
                            If SeekBarPaint_ErrorShown = False Then
                                If My.Settings.MiniModeOn Then
                                    e.Graphics.FillRectangle(System.Drawing.Brushes.Gray, val, 23, 15, 2)
                                Else
                                    e.Graphics.FillRectangle(System.Drawing.Brushes.Gray, val, 29, 15, 2)
                                End If
                            End If
                        Catch
                            If SeekBarPaint_ErrorShown = False Then
                                SeekBarPaint_ErrorShown = True
                                MyMsgBox.Show("Error 0x43: Error! Positions not saved properly, please re-save item position.", "", True)
                            End If
                        End Try
                    End If
                End If
            Catch
            End Try
        End Sub



#End Region
#Region " Playlists |   Search Box"


        ' Type Song in Search box
        Public Sub TextBox1_KeyUp_1(sender As Object, e As KeyEventArgs) Handles TextBox_PlaylistSearch.KeyUp
            If e.KeyCode = Keys.Enter Then

                FindMyString(TextBox_PlaylistSearch.Text)

            End If
        End Sub
        ' Press Search Button
        Public Sub SearchBut_Click(sender As Object, e As EventArgs) Handles But_Search.Click
            FindMyString(TextBox_PlaylistSearch.Text)

        End Sub
        Public Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox_PlaylistSearch.KeyDown
            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
            End If
        End Sub
        Public Sub PlaylistSearchBox_TextChanged(sender As Object, e As EventArgs) Handles TextBox_PlaylistSearch.TextChanged
            occurance.Items.Clear()
        End Sub
        ' Search
        Public Sub FindMyString(ByVal searchString As String)


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
            Dim TabIndex As Integer = PlaylistTabs.SelectedTabPageIndex
            If searchString <> String.Empty Then

                Dim Row As Integer = Playlist.CurrentCell.RowIndex
                Dim RowCount As Integer = Playlist.RowCount
                Dim count As Integer = (RowCount - 1)
                Dim words As String
                If occurance.Items.Count > 0 Then
                    If occurance.SelectedIndex = occurance.Items.Count - 1 Then
                        occurance.SelectedIndex = 0
                    Else
                        occurance.SelectedIndex += 1
                    End If
                    Playlist.Rows(CInt(occurance.SelectedItem)).Selected = True

                    '  Scroller.VerticalScroll.Value = Playlist.CurrentRow.Index

                Else
                    For a = 0 To count
                        words = Playlist.Item(0, a).Value
                        If InStr(words.ToLower, searchString.ToLower) Then
                            Playlist.Rows(a).Selected = True 'words
                            occurance.Items.Add(a)
                        End If
                    Next
                    If occurance.Items.Count = 0 Then Return
                    occurance.SelectedIndex = 0
                    Playlist.Rows(CInt(occurance.SelectedItem)).Selected = True
                    '  Scroller.VerticalScroll.Value = Playlist.CurrentRow.Index
                End If
            End If
            ScrollPlaylistIntoView()

        End Sub


        'Show / Hide
        Public Sub BarCheckPlaylistSearchbox_CheckedChanged(sender As Object, e As ItemClickEventArgs) Handles BarCheckBox_PlaylistSearchbox.CheckedChanged
            If BarCheckBox_PlaylistSearchbox.Checked Then
                TextBox_PlaylistSearch.Visible = True
            Else
                TextBox_PlaylistSearch.Visible = False
            End If
            My.Settings.PlaylistSearchBoxCheckState = BarCheckBox_PlaylistSearchbox.Checked
        End Sub
        Public Sub ChangeStatePlaylistSearchBox()
            BarCheckBox_PlaylistSearchbox.Checked = Not BarCheckBox_PlaylistSearchbox.Checked
        End Sub

        'Clear
        Public Sub PlaylistSearchBox_MouseUp(sender As Object, e As MouseEventArgs) Handles TextBox_PlaylistSearch.MouseUp
            TextBox_PlaylistSearch.Text = ""
        End Sub


        'Allow using space bar without changing playback status
        Public Shared UseSpaceBar As Boolean = True
        Public Sub PlaylistSearchBox_Enter(sender As Object, e As EventArgs) Handles TextBox_PlaylistSearch.Enter
            UseSpaceBar = False
        End Sub
        Public Sub PlaylistSearchBox_Leave(sender As Object, e As EventArgs) Handles TextBox_PlaylistSearch.Leave
            UseSpaceBar = True
        End Sub



#End Region


#Region " Playlist   |   Resizing Row Height"

        Public Sub PlaylistRowHeightChanger_EditValueChanged(sender As Object, e As EventArgs) Handles BarEdit_PlaylistRowHeightChanger.EditValueChanged, BarEdit_PlaylistRowHeightChanger.ItemClick
            Playlist_Rowheight = BarEdit_PlaylistRowHeightChanger.EditValue
            My.Settings.PlaylistRowHeight = Playlist_Rowheight
            My.Settings.Save()
            My.Settings.SkinChanged = True
            Timer_PlaylistsSizes.Start()

        End Sub

#End Region

#Region " Playlist  |   Font"


        'Adjust Font
        Public Sub AdjustFontBut_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_AdjustFont.ItemClick

            Adjustfont()

        End Sub
        Public Sub Adjustfont()
            Dim Playlist1 As GridPlaylist
            For Each c As Control In PlaylistTabs.SelectedTabPage.Controls
                If c.GetType Is GetType(GridPlaylist) Then
                    Playlist1 = c
                End If
            Next
            FontDialog1.Font = My.Settings.PlaylistsFont

            If FontDialog1.ShowDialog = DialogResult.OK Then
                For i As Integer = 0 To PlaylistTabs.TabPages.Count - 1
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
                    Playlist.DefaultCellStyle.Font = FontDialog1.Font
                    Playlist.RowsDefaultCellStyle.Font = FontDialog1.Font
                    My.Settings.PlaylistsFont = FontDialog1.Font
                    My.Settings.Save()
                Next
            End If
        End Sub


#End Region


        ' Tab Playlist Item Selection
#Region " Playlist  |   Selection Changed"

        Dim ArtworkError As Boolean = False
        Public Sub GridPlaylist_SelectionChanged()


            InitiatePlayOnStart = True
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

                Dim row As Integer = Playlist.CurrentRow.Index
                Dim RowCount As Integer
                Dim SongFilename As String
                Dim SongPosition As String

                Try
                    Playlist.CurrentCell = Playlist.Rows(row).Cells(0)
                    row = Playlist.CurrentCell.RowIndex
                Catch
                End Try
                Try
                    RowCount = Playlist.RowCount
                Catch
                End Try
                Try
                    SongFilename = Playlist.Item(6, row).Value.ToString
                    filelocation = SongFilename
                Catch
                End Try
                Try
                    SongPosition = Playlist.Item(5, row).Value.ToString
                    My.Settings.SaveItemPosition = CDbl(SongPosition)
                Catch
                End Try
                Playlist.CurrentCell = Playlist.Rows(row).Cells(0)
            Catch
            End Try
        End Sub



#End Region
#Region " Playlists |   Tab Changed"


        'Change Selected Playlist
        Public Sub XtraTabControl1_SelectedPageChanging(sender As Object, e As TabPageChangingEventArgs) ' Handles PlaylistTabs.SelectedPageChanging


            If AppOpenFinished = False Then Return

            Dim TabIndex As Integer = PlaylistTabs.SelectedTabPageIndex
            If PlaylistClosing Then
                GC.Collect()
            Else
                BarCheckBox_UseSpotifyLocal.Checked = False
                If AppOpen = True Then Return
                If IsVideo Then
                    If IsRadioStation Then
                        Try

                            If radioplaybutton.ClassName = "b-play" Then
                                My.Settings.PlayWhenTabActive.Item(PlaylistTabs.SelectedTabPageIndex) = False
                            Else
                                radioplaybutton.Click()
                                My.Settings.PlayWhenTabActive.Item(PlaylistTabs.SelectedTabPageIndex) = True
                                RadioTimer.Stop()
                            End If

                        Catch ex As Exception
                        End Try
                    Else
                        If firstopen = False Then
                            Dim Playlist As GridPlaylist
                            Dim Scroller As Scroller
                            For Each c As Control In PlaylistTabs.SelectedTabPage.Controls
                                If c.GetType Is GetType(Scroller) Then
                                    Scroller = c
                                    For Each c2 As Control In Scroller.Controls
                                        If c2.GetType Is GetType(GridPlaylist) Then
                                            Playlist = c2
                                            Try
                                                Dim Row As Integer = Playlist.CurrentCell.RowIndex
                                                Dim RowCount As Integer = Playlist.RowCount
                                                Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                                                If RowCount > 0 Then
                                                    My.Settings.LastPlayedSongs.Item(TabIndex) = Row
                                                    If VLC_installed Then
                                                        My.Settings.LastPlayedPositions.Item(TabIndex) = VlcPlayer.input.position

                                                        If VlcPlayer.playlist.isPlaying Then 'If VlcPlayer.playlist.isPlaying Then
                                                            My.Settings.PlayWhenTabActive.Item(PlaylistTabs.SelectedTabPageIndex) = True
                                                        Else
                                                            My.Settings.PlayWhenTabActive.Item(PlaylistTabs.SelectedTabPageIndex) = False
                                                        End If
                                                    Else
                                                        My.Settings.LastPlayedPositions.Item(TabIndex) = 0
                                                    End If

                                                End If
                                                My.Settings.Save()
                                                If VLC_installed Then
                                                    VlcPlayer.playlist.stop()
                                                    VLCclearPlaylists()
                                                End If

                                            Catch
                                            End Try
                                        End If
                                    Next
                                End If
                            Next
                        End If
                    End If

                Else
                    If firstopen = False Then
                        Dim Playlist As GridPlaylist
                        Dim Scroller As Scroller
                        For Each c As Control In PlaylistTabs.SelectedTabPage.Controls
                            If c.GetType Is GetType(Scroller) Then
                                Scroller = c
                                For Each c2 As Control In Scroller.Controls
                                    If c2.GetType Is GetType(GridPlaylist) Then
                                        Playlist = c2
                                        Try
                                            Dim Row As Integer = Playlist.CurrentRow.Index
                                            Dim RowCount As Integer = Playlist.RowCount
                                            Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                                            If RowCount > 0 Then
                                                My.Settings.LastPlayedSongs.Item(TabIndex) = Row
                                                My.Settings.LastPlayedPositions.Item(TabIndex) = AudioPlayer.Instance.Position
                                                If AudioPlayer.Instance.GetStreamStatus = BASSActive.BASS_ACTIVE_PLAYING Then
                                                    My.Settings.PlayWhenTabActive.Item(PlaylistTabs.SelectedTabPageIndex) = True
                                                Else
                                                    My.Settings.PlayWhenTabActive.Item(PlaylistTabs.SelectedTabPageIndex) = False
                                                End If
                                            End If
                                        Catch
                                        End Try
                                        My.Settings.Save()
                                    End If
                                Next
                            End If
                        Next
                        AudioPlayer.Instance.Pause()
                    End If
                End If
            End If
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Sub
        Public Sub XtraTabControl1_SelectedPageChanged(sender As Object, e As TabPageChangedEventArgs) 'Handles PlaylistTabs.SelectedPageChanged


            If PlaylistTabs.SelectedTabPage.Text = "Spotify" Then
                BarCheckBox_UseSpotifyLocal.Checked = True
                Return
            Else
                BarCheckBox_UseSpotifyLocal.Checked = False
            End If

            If AddingPlaylist Then
                AddingPlaylist = False
                If AppOpenFinished Then
                    RefreshApp()
                End If
                Return
            End If

            Dim TabIndex As Integer = PlaylistTabs.SelectedTabPageIndex
            If AppOpen = True Then Return
            If My.Settings.PlayOnStart = True Then My.Settings.TempPlay = True
            If AppOpenFinished Then
                PlayOnStartCurrent()
                Timer_Meta_and_Artwork.Start()
            End If
            If Not e.Page.PageVisible Then
                TryCast(sender, XtraTabControl).MakePageVisible(e.Page)
            End If
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
            Dim i As Integer = TabIndex
            PlaylistTabs.TabPages(i).Controls.Add(SongArrangePanel)
            PlaylistTabs.TabPages(i).Controls.Add(OpenProgressBar)
            SongArrangePanel.BringToFront()
            'PlaylistTabs.TabPages(i).Controls.Add(TextBox_PlaylistSearch)
            PlaylistTabs.TabPages(i).Controls.Add(SearchBox_Panel)
            'TextBox_PlaylistSearch.Dock = DockStyle.Top
            'TextBox_PlaylistSearch.Controls.Add(But_Search)
            'But_Search.BringToFront()
            'But_Search.Top = 1

            OpenProgressBar.BringToFront()
            If Not Playlist Is Nothing Then
                If PlaylistClosing Then
                    GC.Collect()
                    PlaylistClosing = False
                Else
                    If AppOpenFinished Then
                        RefreshApp()
                    End If
                End If
            End If
            occurance.Items.Clear()
            If AddingPlaylist Then Return
            If AppOpenFinished Then
                If My.Settings.TouchFriendly = True Then
                    SkinChange_Playlists_Touch()
                Else
                    SkinChange_Playlists_Standard()
                End If
            End If
            If AppOpenFinished Then
                RefreshApp()
                FontDialog1.Font = My.Settings.PlaylistsFont
                Try
                    Playlist.DefaultCellStyle.Font = FontDialog1.Font
                    Playlist.RowsDefaultCellStyle.Font = FontDialog1.Font
                Catch ex As Exception
                End Try

            End If
            NeedResizeRefresh = True
            GC.Collect()

            Try
                Scroller.Focus()
                Playlist.Focus()
            Catch ex As Exception
            End Try

            DoGetMetaInfo = True
            GetMetaInfo()
            GetOtherMetaInfo()
            GetArtwork()
            If Not e.Page.PageVisible Then
                TryCast(sender, XtraTabControl).MakePageVisible(e.Page)
            End If
        End Sub


#End Region


#Region " Drag Tabs"

#Region " Declarations"

        Private Delegate Sub tabPageMovedd(sender As Object, e As TabDragEventArgs)

        <Description("Fires when a tabPage has been moved to a new location.")> _
        Private Event TabPageMoved As tabPageMovedd

        Private m_allowTabPageMove As Boolean = True
        Private tabMoveCursor As System.Windows.Forms.Cursor = Cursors.SizeWE

        Private mRectDragBoxFromMouseDown As Rectangle

        Private originalTP As DevExpress.XtraTab.XtraTabPage
        Private originalTPLoc As Integer
        Private mIsDragging As Boolean = False

#End Region

        <DefaultValue(False)> _
        <Category("Behavior")> _
        <Description("Enables the tabPages to be re-ordered.")> _
        Public Property AllowTabPageMove() As Boolean


            Get
                Return m_allowTabPageMove
            End Get
            Set(value As Boolean)


                m_allowTabPageMove = value
                If value AndAlso Not PlaylistTabs.AllowDrop Then
                    PlaylistTabs.AllowDrop = value
                End If
            End Set
        End Property

        Public Sub OnMouseDown(sender As Object, e As MouseEventArgs) 'Handles PlaylistTabs.MouseDown


            MyBase.OnMouseDown(e)
            CalcRectDragBox(e.X, e.Y)

            Dim hinfo As DevExpress.XtraTab.ViewInfo.XtraTabHitInfo = PlaylistTabs.CalcHitInfo(New Point(e.X, e.Y))
            If hinfo.Page IsNot Nothing Then
                'Log the original index and tabPage to restore if the user drags it outside the control////
                originalTP = hinfo.Page
                originalTPLoc = FindTabIndex(originalTP)
            End If
        End Sub

        Public Sub OnMouseMove(sender As Object, e As MouseEventArgs) ' Handles PlaylistTabs.MouseMove


            Try
                MyBase.OnMouseMove(e)
                If e.Button = MouseButtons.Left And m_allowTabPageMove Then
                    If Not mRectDragBoxFromMouseDown.Equals(Rectangle.Empty) AndAlso Not mRectDragBoxFromMouseDown.Contains(e.X, e.Y) Then
                        mIsDragging = True
                        DoDragDrop(PlaylistTabs.SelectedTabPage, DragDropEffects.Move)
                        CalcRectDragBox(e.X, e.Y)
                    End If
                End If
            Catch
            End Try
        End Sub

        Public Sub OnDragDrop(sender As Object, drgevent As DragEventArgs) ' Handles PlaylistTabs.DragDrop


            Try
                MyBase.OnDragDrop(drgevent)
                Dim dragTab As DevExpress.XtraTab.XtraTabPage = DirectCast(drgevent.Data.GetData(GetType(DevExpress.XtraTab.XtraTabPage)), DevExpress.XtraTab.XtraTabPage)
                Dim dropLocationIndex As Integer = FindTabIndex(dragTab)

                If dropLocationIndex <> originalTPLoc Then
                    RaiseEvent TabPageMoved(PlaylistTabs, New TabDragEventArgs(dragTab, dropLocationIndex))
                    Dim originalTabSong As Integer = CInt(My.Settings.LastPlayedSongs.Item(originalTPLoc))
                    Dim originalTabPosition As String = My.Settings.LastPlayedPositions.Item(originalTPLoc)
                    Dim originalPlayState As Boolean = My.Settings.PlayWhenTabActive.Item(originalTPLoc)
                    My.Settings.LastPlayedSongs.RemoveAt(originalTPLoc)
                    My.Settings.LastPlayedSongs.Insert(dropLocationIndex, originalTabSong)
                    My.Settings.LastPlayedPositions.RemoveAt(originalTPLoc)
                    My.Settings.PlayWhenTabActive.RemoveAt(originalTPLoc)
                    If IsVideo Then
                        If VLC_installed Then
                            My.Settings.LastPlayedPositions.Insert(dropLocationIndex, VlcPlayer.input.position)
                            If VlcPlayer.playlist.isPlaying Then 'If VlcPlayer.playlist.isPlaying Then
                                My.Settings.PlayWhenTabActive.Insert(dropLocationIndex, True)
                            Else
                                My.Settings.PlayWhenTabActive.Insert(dropLocationIndex, False)
                            End If
                        End If

                    Else
                        My.Settings.LastPlayedPositions.Insert(dropLocationIndex, AudioPlayer.Instance.Position)
                        If AudioPlayer.Instance.GetStreamStatus = BASSActive.BASS_ACTIVE_PLAYING Then
                            My.Settings.PlayWhenTabActive.Insert(dropLocationIndex, True)
                        Else
                            My.Settings.PlayWhenTabActive.Insert(dropLocationIndex, False)
                        End If
                    End If
                End If
            Catch
            End Try
        End Sub

        Public Sub OnDragOver(sender As Object, drgevent As DragEventArgs) 'Handles PlaylistTabs.DragOver


            Try
                MyBase.OnDragOver(drgevent)
                Dim hinfo As DevExpress.XtraTab.ViewInfo.XtraTabHitInfo = PlaylistTabs.CalcHitInfo(PlaylistTabs.PointToClient(New Point(drgevent.X, drgevent.Y)))
                If hinfo.Page IsNot Nothing And mIsDragging And m_allowTabPageMove Then
                    Dim hoverTab As DevExpress.XtraTab.XtraTabPage = hinfo.Page
                    If drgevent.Data.GetDataPresent(GetType(DevExpress.XtraTab.XtraTabPage)) <> False Then
                        drgevent.Effect = DragDropEffects.Move
                        Dim dragTab As DevExpress.XtraTab.XtraTabPage = DirectCast(drgevent.Data.GetData(GetType(DevExpress.XtraTab.XtraTabPage)), DevExpress.XtraTab.XtraTabPage)
                        Dim itemDragIndex As Integer = FindTabIndex(dragTab)
                        Dim dropLocationIndex As Integer = FindTabIndex(hoverTab)
                        If itemDragIndex <> dropLocationIndex AndAlso drgevent.AllowedEffect = DragDropEffects.Move Then
                            MoveTabPages(dropLocationIndex, itemDragIndex, hinfo.HitPoint.X)
                        End If
                    Else
                        drgevent.Effect = DragDropEffects.None
                    End If
                Else
                    drgevent.Effect = DragDropEffects.None
                End If
            Catch
            End Try
        End Sub

        Public Sub MoveTabPages(dropLocationIndex As Integer, itemDragIndex As Integer, ptX As Integer)


            Dim viewInfo As BaseTabControlViewInfo = DirectCast(PlaylistTabs, IXtraTab).ViewInfo
            Dim selRect As Rectangle = viewInfo.HeaderInfo.AllPages(itemDragIndex).Bounds
            Dim dropRect As Rectangle = viewInfo.HeaderInfo.AllPages(dropLocationIndex).Bounds
            If itemDragIndex < dropLocationIndex Then
                If ptX < (selRect.Left + dropRect.Width) Then
                    Return
                End If
            ElseIf ptX > (selRect.Right - dropRect.Width) Then
                Return
            End If
            Dim selTab As DevExpress.XtraTab.XtraTabPage = PlaylistTabs.TabPages(itemDragIndex)
            Dim repTab As DevExpress.XtraTab.XtraTabPage = PlaylistTabs.TabPages(dropLocationIndex)
            PlaylistTabs.TabPages.Move(itemDragIndex, repTab)
            PlaylistTabs.TabPages.Move(dropLocationIndex, selTab)
            PlaylistTabs.SelectedTabPage = selTab
        End Sub


        Public Sub OnDragLeave(sender As Object, e As DragEventArgs) 'Handles PlaylistTabs.DragLeave


            MyBase.OnDragLeave(e)

            Dim newIndex As Integer = FindTabIndex(originalTP)
            If newIndex <> originalTPLoc Then
                If newIndex < originalTPLoc Then
                    PlaylistTabs.TabPages.Move(originalTPLoc + 1, originalTP)
                Else
                    PlaylistTabs.TabPages.Move(originalTPLoc, originalTP)
                End If
            End If
            mIsDragging = False
        End Sub

        Public Sub OnGiveFeedback(sender As Object, gfbevent As GiveFeedbackEventArgs) ' Handles PlaylistTabs.GiveFeedback


            MyBase.OnGiveFeedback(gfbevent)
            If tabMoveCursor Is Nothing Then
                Return
            End If
            gfbevent.UseDefaultCursors = False
            If gfbevent.Effect = DragDropEffects.Move Then
                Cursor.Current = tabMoveCursor
            Else
                Cursor.Current = Cursors.No
            End If
        End Sub

        Public Sub CalcRectDragBox(x As Integer, y As Integer)
            Dim dragSize As Size = SystemInformation.DragSize
            mRectDragBoxFromMouseDown = New Rectangle(New Point(CInt(x - (dragSize.Width / 2)), CInt(y - dragSize.Height / 2)), dragSize)
        End Sub

        Private Function FindTabIndex(tPage As DevExpress.XtraTab.XtraTabPage) As Integer


            Return PlaylistTabs.TabPages.IndexOf(tPage)
        End Function

#End Region





#End Region


#Region " METADATA    &    ARTWORK"


#Region " Metadata Get     |   Artwork Get"
#Region " Declarations"
        Dim Is_Title_Long As Boolean = False
        Public Shared IsVideo As Boolean = False
        Public Shared IsRadioStation As Boolean = False
        Public Shared _title As String = "Title"
        Public Shared _Artist As String = "Artist"
        Public Shared _Album As String = "Album"
        Public Shared _description As String = ""
        Public Shared _genre As String = ""
        Public Shared _tracknum As String = ""
        Public Shared _Duration As String = ""
        Public Shared _format As String = ""
        Public Shared _date As String = ""
        Public Shared _encode As String = ""
        Public Shared _publisher As String = ""
        Public Shared _nowplaying As String = ""
        Public Shared _rating As String = ""
        Public Shared _setting As String = ""
        Public Shared _trackid As String = ""
        Public Shared _filesize As String = ""
#End Region

        'Setup Meta Info and Artwork
        Public Sub Meta_and_Artwork_Timer_Tick(sender As Object, e As EventArgs) Handles Timer_Meta_and_Artwork.Tick
            Try
                If IsVideo Then
                    For Each c As Control In Me.Controls
                        If c.Name = "Fullscreen_but" Then
                            c.BringToFront()
                        End If
                    Next
                End If

            Catch ex As Exception

            End Try
            Try
                If FormClosingNow Then Return
                '  If NeedResizeRefresh = False Then Return
                Dim artwork As System.Windows.Forms.PictureBox : Dim Label_Album As Label : Dim Label_Artist As Label : Dim Label_SongName As Label

                Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next


                Try
                    If Playlist.Rows.Count = 0 Then Return
                Catch ex As Exception
                    Return
                End Try

                If My.Settings.DriveMode Then
                    artwork = xcarform.Artwork : Label_SongName = xcarform.Label_SongName : Label_Artist = xcarform.Label_Artist : Label_Album = xcarform.Label_Album
                Else
                    artwork = Me.Artwork : Label_SongName = Me.Label_SongName : Label_Artist = Me.Label_Artist : Label_Album = Me.Label_Album
                End If

                Dim TextColor As Color
                If UsingSpotify Then
                    TextColor = Color.FromArgb(30, 208, 98)
                Else
                    TextColor = Color.Silver
                End If
                If UsingSpotify = False Then
                    If Playlist.Rows(Playlist.CurrentCell.RowIndex).Cells(4).Value = "True" Then
                        TextColor = My.Settings.FavColor
                    End If
                End If

                Label_SongName.ForeColor = TextColor ' : Label_Album.ForeColor = TextColor : Label_Artist.ForeColor = TextColor


                If BarCheckBox_ViewLyrics.Checked Then Return
                Try
                    If AppOpenFinished = False Then Return
                    If PlaylistClosing Then Return
                    If FormClosingval Then Return

                    Dim row As Integer
                    Dim RowCount As Integer
                    Dim SongFilename As String
                    Dim SongPosition As String
                    Try
                        row = Playlist.CurrentCell.RowIndex
                    Catch
                    End Try
                    Try
                        RowCount = Playlist.RowCount
                    Catch
                    End Try
                    Try
                        SongFilename = Playlist.Item(6, row).Value.ToString
                        filelocation = SongFilename
                    Catch
                    End Try
                    If UsingSpotify Then
                        AddHandler Timer_Spotify.Tick, AddressOf Timer_Spotify_Tick
                        Timer_Spotify.Start()
                        AddHandler SpotifyBGWorker.DoWork, AddressOf SpotifyBGWorker_DoWork
                        If Not SpotifyBGWorker.IsBusy Then
                            SpotifyBGWorker.RunWorkerAsync()
                        End If

                        Label_SongName.Text = _title
                        Label_Artist.Text = _Artist
                        Label_Album.Text = _Album

                        Return
                    Else
                        GetMetaInfo()
                        GetArtwork()
                        Try
                            If _title.Contains("C:\") Then ' _title.Length = 3 Or 
                                If Label_SongName.Text <> Path.GetFileNameWithoutExtension(SongFilename) Then
                                    Label_SongName.Text = Path.GetFileNameWithoutExtension(SongFilename)
                                End If
                                Setup_Time_Labels()
                                If Label_SongName.Text = "" Then Label_SongName.Text = "Unknown Title"
                            End If
                            If Label_Album.Text = "" Then Label_Album.Text = "Unknown Album"
                            If Label_Artist.Text = "" Then Label_Album.Text = "Unknown Artist"
                        Catch
                            Timer_Meta_and_Artwork.Stop()
                            Return

                        End Try

                        '..................LYRICS...................
                        If BarCheckBox_ViewLyrics.Checked Then
                            If My.Settings.DriveMode Then
                                If artwork.BackgroundImage.Size.Width >= 512 Then
                                    artwork.BackgroundImage = ChangeOpacity(artwork.BackgroundImage.GetThumbnailImage(artwork.BackgroundImage.PhysicalDimension.Width / 40, artwork.BackgroundImage.PhysicalDimension.Height / 40, Nothing, System.IntPtr.Zero), 0.25)
                                Else
                                    artwork.BackgroundImage = ChangeOpacity(artwork.BackgroundImage.GetThumbnailImage(artwork.BackgroundImage.PhysicalDimension.Width / 10, artwork.BackgroundImage.PhysicalDimension.Height / 10, Nothing, System.IntPtr.Zero), 0.25)
                                End If
                            Else
                                If artwork.BackgroundImage.Size.Width >= 512 Then
                                    artwork.BackgroundImage = ChangeOpacity(artwork.BackgroundImage.GetThumbnailImage(artwork.BackgroundImage.PhysicalDimension.Width / 40, artwork.BackgroundImage.PhysicalDimension.Height / 40, Nothing, System.IntPtr.Zero), My.Settings.ArtworkTransparency)
                                Else
                                    artwork.BackgroundImage = ChangeOpacity(artwork.BackgroundImage.GetThumbnailImage(artwork.BackgroundImage.PhysicalDimension.Width / 10, artwork.BackgroundImage.PhysicalDimension.Height / 10, Nothing, System.IntPtr.Zero), My.Settings.ArtworkTransparency)
                                End If
                            End If

                        End If
                        ImageOriginal = artwork.BackgroundImage

                    End If
                    ResetThumbnail()
                    Is_Title_Long = False
                Catch
                End Try


                Timer_Meta_and_Artwork.Stop()
                Meta_and_Artwork_Timer2.Stop()
            Catch ex As Exception

            End Try
        End Sub

        'Get Metadata
        Public DoGetMetaInfo As Boolean = False
        Public Sub GetMetaInfo()


            Dim artwork As System.Windows.Forms.PictureBox : Dim Label_Album As Label : Dim Label_Artist As Label : Dim Label_SongName As Label
            Dim But_PlayPause As PictureBox : Dim TrackBar_Seek2 As RichTrackBar : Dim timelabel As Label : Dim totaltimelabel As Label
            If My.Settings.DriveMode Then
                artwork = xcarform.Artwork : Label_SongName = xcarform.Label_SongName : Label_Artist = xcarform.Label_Artist : Label_Album = xcarform.Label_Album
                But_PlayPause = xcarform.But_PlayPause : TrackBar_Seek2 = xcarform.TrackBar_Seek2 : timelabel = xcarform.timelabel : totaltimelabel = xcarform.totaltimelabel
            Else
                artwork = Me.Artwork : Label_SongName = Me.Label_SongName : Label_Artist = Me.Label_Artist : Label_Album = Me.Label_Album
                But_PlayPause = Me.But_PlayPause : TrackBar_Seek2 = Me.TrackBar_Seek2 : timelabel = Me.timelabel : totaltimelabel = Me.totaltimelabel
            End If



            If DoGetMetaInfo = False Then Return
            If AppOpenFinished = False Then Return
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
            CheckIfVideo()

            'Reset names
            _title = ""
            _Artist = ""
            _Album = ""
            _description = ""
            _genre = ""
            _tracknum = ""
            _Duration = ""
            _format = ""
            _date = ""
            _encode = ""
            _publisher = ""
            _nowplaying = ""
            _rating = ""
            _setting = ""
            _trackid = ""
            _filesize = ""

            Dim SelectedItem As String
            Try
                SelectedItem = Path.GetFullPath(Playlist.Item(6, Playlist.CurrentRow.Index).Value.ToString)
            Catch
            End Try
            Dim Audiofile As New MediaFile(SelectedItem)

            'Title
            Try
                If IsVideo Then
                    If IsRadioStation Then
                        _title = Playlist.Item(0, Playlist.CurrentRow.Index).Value.ToString.Replace(Chr(38), "&&").TrimEnd
                    Else
                        If VLC_installed Then
                            _title = VlcPlayer.mediaDescription.title.Replace(Chr(38), "&&")
                        End If

                    End If

                Else
                    _title = AudioPlayer.Instance.TrackList.Tracks(0).Title.Replace(Chr(38), "&&")
                End If
                If _title.ToString.Contains("A:\") Or _title.ToString.Contains("B:\") Or _title.ToString.Contains("C:\") Or _title.ToString.Contains("D:\") Or _title.ToString.Contains("E:\") Or _title.ToString.Contains("F:\") Or _title.ToString.Contains("G:\") Or _title.ToString.Contains("H:\") Or _title.ToString.Contains("I:\") Or _title.ToString.Contains("J:\") Or _title.ToString.Contains("K:\") Or _title.ToString.Contains("L:\") Or _title.ToString.Contains("M:\") Or _title.ToString.Contains("N:\") Or _title.ToString.Contains("O:\") Or _title.ToString.Contains("P:\") Or _title.ToString.Contains("Q:\") Or _title.ToString.Contains("R:\") Or _title.ToString.Contains("S:\") Or _title.ToString.Contains("T:\") Or _title.ToString.Contains("U:\") Or _title.ToString.Contains("V:\") Or _title.ToString.Contains("W:\") Or _title.ToString.Contains("X:\") Or _title.ToString.Contains("Y:\") Or _title.ToString.Contains("Z:\") Then
                    Dim tempString As String = _title
                    _title = Path.GetFileNameWithoutExtension(tempString)
                    Is_Title_Long = True
                End If
            Catch
                _title = "Unknown Title"
            End Try


            'Artist
            Try
                If IsVideo Then
                    If IsRadioStation Then

                        Try
                            Try
                                Dim s As String = _Artist
                                Try
                                    For Each ele As Gecko.GeckoHtmlElement In RadioPlayer.Document.GetElementsByTagName("top_player_track")
                                        If ele.GetAttribute("class").Contains("ajax") Then
                                            _Artist = ele.InnerHtml
                                        End If
                                    Next
                                Catch ex As Exception
                                End Try

                                If s = _Artist Then
                                    _Artist = CType(RadioPlayer.Document.GetElementById("top_player_track").ChildNodes(0), Gecko.GeckoHtmlElement).InnerHtml
                                End If
                                '_Artist = CType(RadioPlayer.Document.GetElementById("top_player_track").GetElementsByTagName("a"), Gecko.GeckoHtmlElement).InnerHtml
                            Catch ex As Exception
                                _Artist = CType(RadioPlayer.Document.GetElementById("top_player_track").ChildNodes(0), Gecko.GeckoHtmlElement).InnerHtml
                            End Try

                        Catch ex As Exception
                            _Artist = "Playing Radio"
                        End Try
                    Else
                        RadioTimer.Stop()
                        If VLC_installed Then

                            If VlcPlayer.mediaDescription.artist.ToString.Count <> 0 Then
                                _Artist = VlcPlayer.mediaDescription.artist ' 'VlcPlayer.playlist.artist.Replace(Chr(38), "&&")
                            Else
                                _Artist = "Unknown Artist"
                            End If
                        End If

                    End If

                Else
                    If AudioPlayer.Instance.TrackList.Tracks(0).Artist.ToString.Count <> 0 Then
                        _Artist = AudioPlayer.Instance.TrackList.Tracks(0).Artist.Replace(Chr(38), "&&")
                    Else
                        _Artist = "Unknown Artist"
                    End If
                End If

            Catch
                _Artist = "Unknown Artist"
            End Try


            'Album
            Try
                If IsVideo Then
                    If IsRadioStation Then
                        _Album = "Radio Station"
                    Else
                        If VLC_installed Then
                            If VlcPlayer.mediaDescription.album.ToString.Count <> 0 Then
                                _Album = VlcPlayer.mediaDescription.album.Replace(Chr(38), "&&")
                            Else
                                _Album = "Unknown Album"
                            End If
                        End If

                    End If


                Else
                    If AudioPlayer.Instance.TrackList.Tracks(0).Album.ToString.Count <> 0 Then
                        _Album = AudioPlayer.Instance.TrackList.Tracks(0).Album.Replace(Chr(38), "&&")
                    Else
                        _Album = "Unknown Album"
                    End If
                End If
            Catch
                _Album = "Unknown Album"
            End Try

            'Track Number
            Try
                If Not IsVideo Then
                    Try
                        _tracknum = Audiofile.General.Properties.Item("Track/Position")
                    Catch
                        Try
                            _tracknum = Audiofile.General.Properties.Item("Position")
                        Catch
                            _tracknum = Audiofile.General.Properties.Item("Track name/Position")
                        End Try

                    End Try


                End If
            Catch

            End Try

            'Duration / Length
            Try
                If IsVideo Then
                    If IsRadioStation Then
                        _Duration = "0:00"
                    Else
                        _Duration = totaltimelabel.Text
                    End If
                Else
                    _Duration = AudioPlayer.Instance.GetTotalTime
                End If
            Catch
                _Duration = "0:00"
            End Try


            'Genre
            Try
                If IsVideo Then
                    If IsRadioStation Then
                        _genre = ""
                    Else
                        If VLC_installed Then
                            _genre = VlcPlayer.mediaDescription.genre
                        End If

                    End If
                Else
                    _genre = Audiofile.General.Properties.Item("Genre")
                End If
            Catch
            End Try


            'Date
            Try
                If IsVideo Then
                    If IsRadioStation Then
                        _date = ""
                    Else
                        If VLC_installed Then
                            _date = VlcPlayer.mediaDescription.date
                        End If

                    End If
                Else

                    Try
                        _date = Audiofile.General.Properties.Item("Recorded_Date")
                    Catch
                        Try
                            _date = Audiofile.General.Properties.Item("Recorded date")
                        Catch
                            _date = Audiofile.General.Properties.Item("Date")
                        End Try
                    End Try
                End If
            Catch
            End Try


            'Rating
            Try
                If IsVideo Then
                    If IsRadioStation Then
                        _rating = ""
                    Else
                        If VLC_installed Then
                            _rating = VlcPlayer.mediaDescription.rating
                        End If

                    End If


                Else
                    _rating = Audiofile.General.Properties.Item("Rating")
                End If
            Catch
            End Try


            'Publisher
            Try
                If IsVideo Then
                    If IsRadioStation Then
                        _publisher = ""
                    Else
                        If VLC_installed Then
                            _publisher = VlcPlayer.mediaDescription.publisher
                        End If

                    End If


                Else
                    _publisher = Audiofile.General.Properties.Item("Publisher")

                End If
            Catch
            End Try


            'Encoded By
            Try
                If IsVideo Then
                    If IsRadioStation Then
                        _encode = ""
                    Else
                        If VLC_installed Then
                            _encode = VlcPlayer.mediaDescription.encodedBy 'VlcPlayer.playlist.encodedBy
                        End If

                    End If

                Else
                    _encode = Audiofile.General.Properties.Item("EncodedBy")
                End If
            Catch
            End Try


            'File Size
            Try
                If IsVideo Then
                    If IsRadioStation Then
                        _filesize = ""
                    Else
                        If VLC_installed Then
                            _filesize = (FileIO.FileSystem.GetFileInfo(VlcPlayer.mediaDescription.nowPlaying).Length / 1048576).ToString.Remove(5) & " MB"

                        End If
                    End If

                Else
                    _filesize = (FileIO.FileSystem.GetFileInfo(AudioPlayer.Instance.CurrentTrack.Location).Length / 1048576).ToString.Remove(5) & " MB"
                End If
            Catch
                _filesize = ""
            End Try

            Try
                'Set Labels and Tooltips
                If Label_SongName.Text <> _title Then

                    Label_SongName.Text = _title
                    '                    ToolTip1.SetToolTip(SongName, _title)
                End If
                If Label_Artist.Text <> _Artist Then
                    Label_Artist.Text = _Artist
                    '                   ToolTip1.SetToolTip(artistlabel, _Artist)
                End If
                If Label_Album.Text <> _Album Then
                    Label_Album.Text = _Album
                    '                    ToolTip1.SetToolTip(albumlabel, _Album)
                End If

            Catch
            End Try

            Refresh_VLC_volspeed()


            DoGetMetaInfo = False
        End Sub
        'Get Artwork
        Public Sub GetArtwork()


            Dim artwork As System.Windows.Forms.PictureBox
            Dim Label_Album As Label
            Dim Label_Artist As Label
            Dim Label_SongName As Label

            If My.Settings.DriveMode Then
                artwork = xcarform.Artwork
                Label_SongName = xcarform.Label_SongName
                Label_Artist = xcarform.Label_Artist
                Label_Album = xcarform.Label_Album
            Else
                artwork = Me.Artwork
                Label_SongName = Me.Label_SongName
                Label_Artist = Me.Label_Artist
                Label_Album = Me.Label_Album
            End If
            If AppOpenFinished = False Then Return
            CheckIfVideo()
            'Artwork
            If IsRadioStation Then
                If VLC_installed Then
                    VlcPlayer.Visible = False
                End If

                artwork.Visible = False
                If RadioPlayer.Visible = False Then
                    artwork.Parent.Controls.Add(RadioPlayer)
                    RadioPlayer.Visible = True
                    RadioPlayer.Size = artwork.Size
                    RadioPlayer.Location = artwork.Location
                    RadioPlayer.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
                    RadioPlayer.BringToFront()
                End If
            ElseIf IsVideo Then
                If UsingSpotify Then Return




                If VLC_installed Then
                    VlcPlayer.Visible = True
                    artwork.Visible = False

                Else
                    artwork.Visible = True
                    Dim f As New Font("Century Gothic", 15, FontStyle.Bold)
                    artwork.BackgroundImage = DrawText("Videos unsupported." + Environment.NewLine + "Install VLC", f, Color.FromArgb(200, 200, 200), Color.Transparent)
                End If


            Else
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
                    Dim Row As Integer = Playlist.CurrentCell.RowIndex
                    Dim RowCount As Integer = Playlist.RowCount
                    Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                    filelocation = SongFilename
                    Dim file = TagLib.File.Create(SongFilename)
                    If VLC_installed Then
                        VlcPlayer.Visible = False
                    End If

                    VLCChapterMarks.Visible = False
                    IsVideo = False
                    artwork.Visible = True
                    RadioPlayer.Visible = False
                    If IO.File.Exists(Path.GetDirectoryName(filelocation) + "\" + Path.GetFileNameWithoutExtension(filelocation) + ".jpg") Then
                        Dim image As System.Drawing.Image = System.Drawing.Bitmap.FromFile(Path.GetDirectoryName(filelocation) + "\" + Path.GetFileNameWithoutExtension(filelocation) + ".jpg")
                        artwork.BackgroundImage = ChangeOpacity(image, My.Settings.ArtworkTransparency)
                    ElseIf IO.File.Exists(Path.GetDirectoryName(filelocation) + "\" + Path.GetFileNameWithoutExtension(filelocation) + ".png") Then
                        Dim image As System.Drawing.Image = System.Drawing.Bitmap.FromFile(Path.GetDirectoryName(filelocation) + "\" + Path.GetFileNameWithoutExtension(filelocation) + ".png")
                        artwork.BackgroundImage = ChangeOpacity(image, My.Settings.ArtworkTransparency)
                    Else
                        If file.Tag.Pictures.Length = 0 Then
                            If My.Computer.FileSystem.FileExists(Path.GetDirectoryName(SongFilename) + "\folder.gif") Then
                                file.Tag.Pictures = New TagLib.IPicture() {TagLib.Picture.CreateFromPath(Path.GetDirectoryName(SongFilename) + "\folder.gif")}
                                Dim bin = DirectCast(file.Tag.Pictures(0).Data.Data, Byte())
                                artwork.BackgroundImage = ChangeOpacity(System.Drawing.Image.FromStream(New IO.MemoryStream(bin)), My.Settings.ArtworkTransparency)
                            ElseIf My.Computer.FileSystem.FileExists(Path.GetDirectoryName(SongFilename) + "\folder.jpg") Then
                                file.Tag.Pictures = New TagLib.IPicture() {TagLib.Picture.CreateFromPath(Path.GetDirectoryName(SongFilename) + "\folder.jpg")}
                                Dim bin = DirectCast(file.Tag.Pictures(0).Data.Data, Byte())
                                artwork.BackgroundImage = ChangeOpacity(System.Drawing.Image.FromStream(New IO.MemoryStream(bin)), My.Settings.ArtworkTransparency)
                            ElseIf My.Computer.FileSystem.FileExists(Path.GetDirectoryName(SongFilename) + "\folder.png") Then
                                file.Tag.Pictures = New TagLib.IPicture() {TagLib.Picture.CreateFromPath(Path.GetDirectoryName(SongFilename) + "\folder.png")}
                                Dim bin = DirectCast(file.Tag.Pictures(0).Data.Data, Byte())
                                artwork.BackgroundImage = ChangeOpacity(System.Drawing.Image.FromStream(New IO.MemoryStream(bin)), My.Settings.ArtworkTransparency)
                            ElseIf My.Computer.FileSystem.FileExists(Path.GetDirectoryName(SongFilename) + "\cover.jpg") Then
                                file.Tag.Pictures = New TagLib.IPicture() {TagLib.Picture.CreateFromPath(Path.GetDirectoryName(SongFilename) + "\cover.jpg")}
                                Dim bin = DirectCast(file.Tag.Pictures(0).Data.Data, Byte())
                                artwork.BackgroundImage = ChangeOpacity(System.Drawing.Image.FromStream(New IO.MemoryStream(bin)), My.Settings.ArtworkTransparency)
                            Else
                                artwork.BackgroundImage = ChangeOpacity(My.Resources.not_available1, My.Settings.ArtworkTransparency)
                            End If
                        ElseIf file.Tag.Pictures.Length >= 1 Then
                            If VLC_installed Then
                                VlcPlayer.Visible = False
                            End If

                            VLCChapterMarks.Visible = False
                            IsVideo = False
                            artwork.Visible = True
                            RadioPlayer.Visible = False
                            Dim bin = DirectCast(file.Tag.Pictures(0).Data.Data, Byte())
                            artwork.BackgroundImage = ChangeOpacity(System.Drawing.Image.FromStream(New IO.MemoryStream(bin)), My.Settings.ArtworkTransparency)
                        Else
                            If SongFilename.EndsWith(".jpg") Or SongFilename.EndsWith(".png") Or SongFilename.EndsWith(".gif") Or
                                SongFilename.EndsWith(".bmp") Then
                                file.Tag.Pictures = New TagLib.IPicture() {TagLib.Picture.CreateFromPath(Path.GetDirectoryName(SongFilename))}
                                Dim bin = DirectCast(file.Tag.Pictures(0).Data.Data, Byte())
                                artwork.BackgroundImage = ChangeOpacity(System.Drawing.Image.FromStream(New IO.MemoryStream(bin)), My.Settings.ArtworkTransparency)
                            Else
                                If VLC_installed Then
                                    VlcPlayer.Visible = True
                                End If

                                IsVideo = True
                            End If
                        End If
                    End If

                Catch
                    Timer_Meta_and_Artwork.Stop()
                End Try
            End If
            Timer_AB.Start()
        End Sub


        'Get All other metadata
        Public Sub GetOtherMetaInfo()


            Dim artwork As System.Windows.Forms.PictureBox
            Dim Label_Album As Label
            Dim Label_Artist As Label
            Dim Label_SongName As Label

            If My.Settings.DriveMode Then
                artwork = xcarform.Artwork
                Label_SongName = xcarform.Label_SongName
                Label_Artist = xcarform.Label_Artist
                Label_Album = xcarform.Label_Album
            Else
                artwork = Me.Artwork
                Label_SongName = Me.Label_SongName
                Label_Artist = Me.Label_Artist
                Label_Album = Me.Label_Album
            End If
            If AppOpenFinished = False Then Return
            Try
                Dim Artist As String, Album As String, Duration As String
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
                For row As Integer = 0 To Playlist.Rows.Count - 1
                    Dim SongFilename As String = Playlist.Item(6, row).Value.ToString
                    CheckIfVideo()
                    If Not IsVideo Then
                        Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                        AudioPlayer.Instance.TrackList.Tracks.Add(track)
                        Artist = AudioPlayer.Instance.TrackList.Tracks(AudioPlayer.Instance.TrackList.Tracks.Count - 1).Artist
                        Album = AudioPlayer.Instance.TrackList.Tracks(AudioPlayer.Instance.TrackList.Tracks.Count - 1).Album
                        Duration = AudioPlayer.Instance.TrackList.Tracks(AudioPlayer.Instance.TrackList.Tracks.Count - 1).Length

                        Dim total As TimeSpan = TimeSpan.FromSeconds(Duration)
                        If total.Hours >= 1 Then
                            Duration = total.ToString.Substring(0, 8)
                        Else
                            Duration = total.ToString.Substring(3, 5)
                        End If

                        If Playlist.Item(1, row).Value = "" Then
                            Playlist.Item(1, row).Value = Artist
                        End If
                        If Playlist.Item(2, row).Value = "" Then
                            Playlist.Item(2, row).Value = Album
                        End If
                        If Playlist.Item(3, row).Value = "" Then
                            Playlist.Item(3, row).Value = Duration
                        End If

                    End If
                Next
            Catch
            End Try
        End Sub

        'Spotify
        Dim SpotifyTime As Integer = 0
        Dim SpotifyTimeSet As Date
        Dim SpotifyTimeText As String = "00:00"
        Private Async Sub Timer_Spotify_Tick(sender As Object, e As EventArgs) Handles Timer_Spotify.Tick
            If UsingSpotify = False Then
                Timer_Spotify.Stop()
                Return

            End If
            Timer_Spotify.Interval = 1
            AddHandler SpotifyBGWorker.DoWork, AddressOf SpotifyBGWorker_DoWork
            If Not SpotifyBGWorker.IsBusy Then
                SpotifyBGWorker.RunWorkerAsync()
            End If



        End Sub
        Dim SpotifyBGWorker As New BackgroundWorker
        Dim Spotify_AB_BGW As New BackgroundWorker
        Public Sub SpotifyBGWorker_DoWork(sender As Object, e As DoWorkEventArgs)
            If UsingSpotify = False Then Return
            Dim useSpotifyTime As Boolean = False
            If useSpotifyTime Then
                SpotifyTime += 1
                If SpotifyTime >= 180 Then
                    UseSpotify()
                    SpotifyTime = 0
                End If
            End If

            Dim ts As TimeSpan = SpotifyTimeSet - DateTime.Now.AddSeconds(-1)
            SpotifyTimeText = ts.Minutes.ToString("00") & ":" & ts.Seconds.ToString("00")
            If SpotifyTimeText = "00:00" Then
                UseSpotify()
                SpotifyTimeSet = DateTime.Now.AddMinutes(15)
            End If



            If Not _title = _SpotifyNew.GetPlayingTrack.Item.Name.Replace(Chr(38), "&&") Then
                GrabSpotifyMetadata()
                GrabSpotifyArtwork()
            End If
            GrabSpotifyPlayingState()
            GrabSpotifyTime()
            GrabSpotifyStates()
            GrabSpotifyVolume()
            TextBox_PlaylistSearch.Text = SpotifyTimeText
        End Sub


        Public Async Function GrabSpotifyMetadata() As Task
            Dim _SpotifyFast As SpotifyWebAPI = _SpotifyNew
            Try
                _title = _SpotifyFast.GetPlayingTrack.Item.Name.Replace(Chr(38), "&&") '_SpotifyNew. 'SpotifyWebClientUserControl._Title
                _Artist = _SpotifyFast.GetPlayingTrack.Item.Artists(0).Name.Replace(Chr(38), "&&") 'SpotifyWebClientUserControl._Artist
                _Album = _SpotifyFast.GetPlayingTrack.Item.Album.Name.Replace(Chr(38), "&&") 'SpotifyWebClientUserControl._Album

                Label_SongName.Text = _title
                Label_Artist.Text = _Artist
                Label_Album.Text = _Album


            Catch ex As Exception
            End Try
        End Function
        Public Shared SpotifyPlaying As Boolean
        Public Sub GrabSpotifyPlayingState()
            If _SpotifyNew.GetPlayback.IsPlaying Then
                SetTaskbarState(TaskbarState.Playing)
                But_PlayPause.BackgroundImage = PauseImage
                SpotifyPlaying = True
            Else
                SetTaskbarState(TaskbarState.Paused)
                But_PlayPause.BackgroundImage = PlayImage
                SpotifyPlaying = False
            End If
        End Sub
        Public Async Function GrabSpotifyArtwork() As Task
            Dim _SpotifyFast As SpotifyWebAPI = _SpotifyNew
            If _SpotifyFast.GetPlayback.Item.Album.Images IsNot Nothing AndAlso _SpotifyFast.GetPlayback.Item.Album.Images.Count > 0 Then
                Using wc As New System.Net.WebClient()
                    Dim imageBytes As Byte() = Await wc.DownloadDataTaskAsync(New Uri(_SpotifyFast.GetPlayback.Item.Album.Images(0).Url))
                    Using stream As New MemoryStream(imageBytes)
                        Artwork.BackgroundImage = ChangeOpacity(System.Drawing.Image.FromStream(stream), My.Settings.ArtworkTransparency)

                    End Using
                End Using
            End If
        End Function

        Public Sub GrabSpotifyTime()
            Dim _SpotifyFast As SpotifyWebAPI = _SpotifyNew
            Try
                Dim ts As TimeSpan = TimeSpan.FromMilliseconds(_SpotifyFast.GetPlayback.ProgressMs)
                timelabel.Text = String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds)

                Dim ts2 As TimeSpan = TimeSpan.FromMilliseconds(_SpotifyFast.GetPlayback.Item.DurationMs)
                totaltimelabel.Text = String.Format("{0:00}:{1:00}", ts2.Minutes, ts2.Seconds)

                TrackBar_Seek2.Maximum = _SpotifyFast.GetPlayback.Item.DurationMs
                TrackBar_Seek2.Value = _SpotifyFast.GetPlayback.ProgressMs
            Catch ex As Exception
            End Try
        End Sub
        Public Sub GrabSpotifyStates()
            Dim _SpotifyFast As SpotifyWebAPI = _SpotifyNew
            IsShuffle = _SpotifyFast.GetPlayback.ShuffleState
            If IsShuffle Then
                But_Shuffle.BackgroundImage = ShuffleImage
            Else
                But_Shuffle.BackgroundImage = ShuffleDisabledImage
            End If

            Select Case _SpotifyFast.GetPlayback.RepeatState
                Case RepeatState.Off
                    repeatAll = False
                    repeatOne = False
                    repeat = False
                    But_Repeat.BackgroundImage = RepeatOffImage

                Case RepeatState.Track
                    repeat = True
                    repeatOne = True
                    repeatAll = False
                    But_Repeat.BackgroundImage = RepeatOneImage

                Case RepeatState.Context
                    repeat = True
                    repeatOne = False
                    repeatAll = True
                    But_Repeat.BackgroundImage = RepeatAllImage

            End Select
        End Sub
        Public Sub GrabSpotifyVolume()
            Dim _SpotifyFast As SpotifyWebAPI = _SpotifyNew
            For i As Integer = 0 To _SpotifyFast.GetDevices.Devices.Count - 1
                Try
                    If _SpotifyFast.GetDevices.Devices(i).IsActive Then
                        TrackBar_PlayerVol2.Value = _SpotifyFast.GetDevices.Devices(i).VolumePercent * 2
                    End If
                Catch ex As Exception
                End Try
            Next

        End Sub

#End Region
#Region " Metadata  |   Edit"
        'Title
        Public Sub BarButtonSongTitle_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_SongTitle.ItemClick


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
            Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString

            If Not SongFilename.ToString.EndsWith(".mp3") Then Return

            MP3Tag.Read(SongFilename)

            Dim TempName As String = MP3Tag.ID3v2Tag.Title
            Dim message, title, defaultValue As String
            Dim myValue As Object
            message = "Enter a new name for the current song"
            title = "Edit Song Title"
            defaultValue = TempName


            Dim xform As New MyInputBox
            xform.Text = title
            xform.Label1.Text = message
            xform.TextEdit1.Text = defaultValue
            Try
                If xform.ShowDialog = DialogResult.OK Then
                    myValue = xform.TextEdit1.Text

                    If myValue Is "" Then Return 'myValue = defaultValue

                    'Collect info from current song


                    With MP3Tag.ID3v2Tag
                        .Title = myValue
                    End With
                    MP3Tag.Write()
                    Label_SongName.Text = myValue

                End If
            Catch

            End Try

        End Sub

        'Artist
        Public Sub BarButtonArtist_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_Artist.ItemClick


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
            Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
            If Not SongFilename.ToString.EndsWith(".mp3") Then Return

            MP3Tag.Read(SongFilename)

            Dim TempName As String = MP3Tag.ID3v2Tag.Artist
            Dim message, title, defaultValue As String
            Dim myValue As Object
            message = "Enter a new name for the artist of current song"
            title = "Edit Artist Name"
            defaultValue = TempName

            Dim xform As New MyInputBox
            xform.Text = title
            xform.Label1.Text = message
            xform.TextEdit1.Text = defaultValue
            Try
                If xform.ShowDialog = DialogResult.OK Then
                    myValue = xform.TextEdit1.Text


                    If myValue Is "" Then Return 'myValue = defaultValue

                    'Collect info from current song

                    '  MP3Tag.Read(SongFilename)

                    With MP3Tag.ID3v2Tag
                        .Artist = myValue
                    End With
                    MP3Tag.Write()
                    Label_Artist.Text = myValue
                End If
            Catch

            End Try
        End Sub

        'Album
        Public Sub BarButtonAlbum_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_Album.ItemClick


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
            Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
            If Not SongFilename.ToString.EndsWith(".mp3") Then Return

            MP3Tag.Read(SongFilename)

            Dim TempName As String = MP3Tag.ID3v2Tag.Album
            Dim message, title, defaultValue As String
            Dim myValue As Object
            message = "Enter a new name for the album of current song"
            title = "Edit Album Name"
            defaultValue = TempName



            Dim xform As New MyInputBox
            xform.Text = title
            xform.Label1.Text = message
            xform.TextEdit1.Text = defaultValue
            Try
                If xform.ShowDialog = DialogResult.OK Then
                    myValue = xform.TextEdit1.Text


                    If myValue Is "" Then Return

                    'Collect info from current song
                    '  MP3Tag.Read(SongFilename)
                    With MP3Tag.ID3v2Tag
                        .Album = myValue
                    End With
                    MP3Tag.Write()
                    Label_Album.Text = myValue
                End If
            Catch

            End Try

        End Sub

        'Artwork
        Public Sub BarButtonArtworkEdit_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_ArtworkEdit.ItemClick
            ArtworkFilename()
        End Sub
        Public Sub barbut_AlbumArt_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles barbut_AlbumArt.ItemClick
            ArtworkAlbum()
        End Sub


#End Region

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
            If LyricsMoved2 Then
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
            LyricsDown = False
            If e.Button = MouseButtons.Right Then
                AudioPlayer.Instance.Stop()
                But_PlayPause.BackgroundImage = PlayImage
                But_PlayPause.BackgroundImage = PlayImage
            End If
            If Not e.Button = MouseButtons.Left Then Return
            But_Previous.BackgroundImage = PreviousImage
            But_Next.BackgroundImage = NextImage

            If LyricsMoved2 Then
                LyricsMoved2 = False
                LyricsMoved = False
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
            If LyricsMoved Then
                LyricsMoved = False
                LyricsMoved2 = False
                Return
            End If
            'Control Speed
            If MaximumSizeChanged = False Then
                If e.Location.Y >= (YTemp + 75) Then

                    Speed_Slow()
                ElseIf e.Location.Y <= (YTemp - 75) Then

                    Speed_Fast()
                Else
                    New_Play()
                End If
            End If
            MaximumSizeChanged = False
        End Sub


        Public Sub Artwork_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles Artwork.MouseDoubleClick
            Zoom()
        End Sub
#End Region
#Region " Artwork   |   Edit"
        'Declarations
        Dim ArtworkFinder As New OpenFileDialog

        'Artwork OpenFileDialog
        Public Sub Setup_ArtworkFinder()
            ArtworkFinder.Filter = "JPG|*.jpg|PNG|*.png"
            ArtworkFinder.Title = "Choose Artwork Image"
        End Sub

        'Album Artwork
        Public Sub EditAlbumArtwork_ItemClick(sender As Object, e As XtraBars.ItemClickEventArgs) Handles BarBut_EditAlbumArtwork.ItemClick
            ArtworkAlbum()
        End Sub
        Public Sub ArtworkAlbum()
            GridPlaylist_SelectionChanged()
            If ArtworkFinder.ShowDialog = DialogResult.OK Then
                AddingFile = True
                Dim imagefile As String = ArtworkFinder.FileName
                Dim image As System.Drawing.Image = System.Drawing.Bitmap.FromFile(ArtworkFinder.FileName)
                Dim copyimage As String = IO.Path.GetDirectoryName(filelocation) + "\" + "folder" + Path.GetExtension(imagefile)

                My.Computer.FileSystem.CopyFile(imagefile, copyimage, Microsoft.VisualBasic.FileIO.UIOption.AllDialogs, FileIO.UICancelOption.DoNothing)
                Timer_Meta_and_Artwork.Start()
                Timer_AddingFile.Start()
            End If

        End Sub
        'Filename Artwork
        Public Sub EditArtwork_ItemClick(sender As Object, e As XtraBars.ItemClickEventArgs) Handles BarBut_EditArtwork.ItemClick
            ArtworkFilename()
        End Sub
        Public Sub ArtworkFilename()
            GridPlaylist_SelectionChanged()
            If ArtworkFinder.ShowDialog = DialogResult.OK Then
                AddingFile = True
                Dim imagefile As String = ArtworkFinder.FileName
                Dim image As System.Drawing.Image = System.Drawing.Bitmap.FromFile(ArtworkFinder.FileName)
                Dim copyimage As String = IO.Path.GetDirectoryName(filelocation) + "\" + Path.GetFileNameWithoutExtension(filelocation) + IO.Path.GetExtension(ArtworkFinder.FileName)

                My.Computer.FileSystem.CopyFile(imagefile, copyimage)

                Dim imageFinal As System.Drawing.Image = System.Drawing.Bitmap.FromFile(copyimage)
                Artwork.BackgroundImage = ChangeOpacity(imageFinal, My.Settings.ArtworkTransparency)


                Timer_Meta_and_Artwork.Start()
                Timer_AddingFile.Start()
            End If
        End Sub


#End Region


#End Region


#Region " HAMBURGER MENU"
        Dim UseNewHamMenu As Boolean = True
#Region " Menu Graphics"

        ' Hamburger Menu
        Public Shared PopupOpen As Boolean = False
        Dim MenuFormX As Form
        Public Shared FormLeft As Integer
        Public Shared FormTop As Integer
        Public Shared FormBottom As Integer
        Public Sub SettingsPic_MouseUp(sender As Object, e As MouseEventArgs) Handles But_SettingsPic.MouseUp
            If e.Button = MouseButtons.Left Then
                If SettingsPicHover Then
                    If PopupOpen = True Then
                        HamburgerMenu.HidePopup()
                        PopupOpen = False
                        Try : If UseNewHamMenu Then MenuFormX.Close()
                        Catch ex As Exception : End Try
                    Else
                        PopupOpen = True
                        Select Case UseNewHamMenu
                            Case True
                                UnRegisterAll()
                                Setup_Hotkeys()

                                RegisterHamburgerMenu()

                                FormLeft = Me.Left
                                FormTop = Me.Top
                                FormBottom = Me.Bottom

                                MenuFormX = New MenuForm
                                MenuFormX.Width = 1
                                MenuFormX.Show()

                            Case False
                                HamburgerMenu.ShowPopup(New Point(MousePosition.X + 5, MousePosition.Y + 5)) 'New Point((Me.Location.X + 10), (Me.Location.Y + SettingsPic.Location.Y + Window_Titlebar.Height)))
                        End Select
                    End If : But_SettingsPic.BackgroundImage = My.Resources.Hamburger_1_Hover

                Else : But_SettingsPic.BackgroundImage = My.Resources.Hamburger_1
                End If
            End If
        End Sub
        Public Sub SettingsPic_MouseDown(sender As Object, e As MouseEventArgs) Handles But_SettingsPic.MouseDown
            But_SettingsPic.BackgroundImage = My.Resources.Hamburger_1_Press
        End Sub

        ' Hamburger Hover
        Dim SettingsPicHover As Boolean = False
        Public Sub SettingsPic_MouseEnter(sender As Object, e As EventArgs) Handles But_SettingsPic.MouseEnter
            SettingsPicHover = True
			But_SettingsPic.BackgroundImage = My.Resources.Hamburger_1_Hover
			TimerSettingsBack.Start()
		End Sub
        Public Sub SettingsPic_MouseLeave(sender As Object, e As EventArgs) Handles But_SettingsPic.MouseLeave
            SettingsPicHover = False
			But_SettingsPic.BackgroundImage = My.Resources.Hamburger_1
			TimerSettingsBack.Stop()
		End Sub

        'Hamburger Menu Closed
        Public Sub PopupMenu1_CloseUp(sender As Object, e As EventArgs) Handles HamburgerMenu.CloseUp
            If SettingsPicHover = False Then
                PopupOpen = False
            End If
        End Sub



#End Region

#Region " View"

#Region " Change Artwork Opacity"
        Public Sub ArtworkTransparency1_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_ArtworkTransparency1.ItemClick
            BarItem_ArtworkTransparencyCustom.EditValue = 75
        End Sub

        Public Sub ArtworkTransparency2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_ArtworkTransparency2.ItemClick
            BarItem_ArtworkTransparencyCustom.EditValue = 50
        End Sub

        Public Sub ArtworkTransparency3_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_ArtworkTransparency3.ItemClick
            BarItem_ArtworkTransparencyCustom.EditValue = 35
        End Sub
        Public Sub ArtworkTransparencyCustom_EditValueChanged(sender As Object, e As EventArgs) Handles BarItem_ArtworkTransparencyCustom.EditValueChanged
            Try
                If BarItem_ArtworkTransparencyCustom.EditValue.ToString.Count = 2 Then
                    My.Settings.ArtworkTransparency = BarItem_ArtworkTransparencyCustom.EditValue / 100
                    Timer_Meta_and_Artwork.Start()
                ElseIf BarItem_ArtworkTransparencyCustom.EditValue.ToString.Count = 3 Then
                    If BarItem_ArtworkTransparencyCustom.EditValue = 100 Then
                        My.Settings.ArtworkTransparency = BarItem_ArtworkTransparencyCustom.EditValue / 100
                        Timer_Meta_and_Artwork.Start()
                    Else
                        ArtworkTransparencyChangeError()
                    End If
                Else
                    ArtworkTransparencyChangeError()
                End If
            Catch
                ArtworkTransparencyChangeError()
            End Try
        End Sub
        Public Sub ArtworkTransparencyChangeError()
            MyMsgBox.Show("Invalid value! Please use only 2-3 digits", "", True)
            BarItem_ArtworkTransparencyCustom.EditValue = My.Settings.ArtworkTransparency * 100
        End Sub

        Public Sub ChangeArtworkOpacity()
            If ArtworkOpacity.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                GetArtwork()
                Timer_Meta_and_Artwork.Start()
            End If
        End Sub
#End Region


#Region " Lyrics"

#Region "Fetch Lyrics & Checkbox State"
        Dim ImageOriginal As System.Drawing.Image
        Dim SelectedSong_path_ As String
        'Lyrics Checkbox
        Public Sub ViewLyricsCheckbox_CheckedChanged(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarCheckBox_ViewLyrics.CheckedChanged
            Panellyrics.BackColor = Color.FromArgb(100, 128, 128, 128)
            If BarCheckBox_ViewLyrics.Checked Then
                My.Settings.CheckStateLyrics = True
                Panellyrics.Visible = True
                If Artwork.BackgroundImage.Size.Width >= 512 Then
                    Artwork.BackgroundImage = Artwork.BackgroundImage.GetThumbnailImage(Artwork.BackgroundImage.PhysicalDimension.Width / 40, Artwork.BackgroundImage.PhysicalDimension.Height / 40, Nothing, System.IntPtr.Zero)
                Else
                    Artwork.BackgroundImage = Artwork.BackgroundImage.GetThumbnailImage(Artwork.BackgroundImage.PhysicalDimension.Width / 10, Artwork.BackgroundImage.PhysicalDimension.Height / 10, Nothing, System.IntPtr.Zero)
                End If
            Else
                Panellyrics.Visible = False
                My.Settings.CheckStateLyrics = False
                Artwork.BackgroundImage = ImageOriginal
                RefreshApp()
            End If

            My.Settings.Save()
            FindLyrics()
        End Sub
        Public Sub ViewLyrics()
            BarCheckBox_ViewLyrics.Checked = Not BarCheckBox_ViewLyrics.Checked
        End Sub


        'Fetch Lyrics
        Public Sub FindLyrics()


            Try
                Meta_and_Artwork_Timer2.Stop()
                Timer_Meta_and_Artwork.Stop()

                Dim SelectedSong As String
                Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next


                If BarCheckBox_ViewLyrics.Checked Then

                    If Artwork.BackgroundImage.Size.Width >= 512 Then
                        Artwork.BackgroundImage = Artwork.BackgroundImage.GetThumbnailImage(Artwork.BackgroundImage.PhysicalDimension.Width / 40, Artwork.BackgroundImage.PhysicalDimension.Height / 40, Nothing, System.IntPtr.Zero)
                    Else
                        Artwork.BackgroundImage = Artwork.BackgroundImage.GetThumbnailImage(Artwork.BackgroundImage.PhysicalDimension.Width / 10, Artwork.BackgroundImage.PhysicalDimension.Height / 10, Nothing, System.IntPtr.Zero)
                    End If

                    If Not String.IsNullOrWhiteSpace(Label_Artist.Text) AndAlso Not String.IsNullOrWhiteSpace(Label_SongName.Text) Then

                        SelectedSong = Playlist.Item(6, Playlist.CurrentRow.Index).Value
                        Dim SelectedSong_txt As String = Path.GetFileNameWithoutExtension(SelectedSong) & ".txt"
                        Dim SelectedSong_path As String = IO.Path.Combine(Application.StartupPath & "\Lyrics\", SelectedSong_txt)

                        If Not UsingSpotify Then


                            If IO.File.Exists(SelectedSong_path) Then
                                LabelLyrics.Text = IO.File.ReadAllText(SelectedSong_path)
                            Else
                                SongLyrics(SelectedSong, SelectedSong_path)
                            End If

                        Else
                            SongLyrics(SelectedSong, SelectedSong_path)


                        End If
                        SelectedSong_path_ = SelectedSong_path
                    End If
                    LabelLyrics.Left = (Panellyrics.Width / 2) - (LabelLyrics.Width / 2)
                    If TextboxLyrics.Visible = True Then
                        TextboxLyrics.Left = (Panellyrics.Width / 2) - (TextboxLyrics.Width / 2)
                    End If
                Else
                    Artwork.BackgroundImage = ImageOriginal

                End If

                CurrentSong = Label_SongName.Text
            Catch
            End Try
        End Sub

        'Fetch from ...
        Public Sub SongLyrics(SelectedSong As String, SelectedSong_path As String)


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

            Try
                Dim src As String = New System.Net.WebClient().DownloadString(New Uri(String.Format("http://www.songlyrics.com/{0}/{1}-lyrics/", Label_Artist.Text.Replace(" ", "-"), Label_SongName.Text.Replace(" ", "-"))))
                Dim prselyrics As String = Parse(src, "class=""songLyricsV14 iComment-text"">", "</p>", "class=""songLyricsV14 iComment-text"">".Length) _
                                             .Replace(vbCrLf, "") _
                                             .Replace("<br />", Environment.NewLine) _
                                             .Replace("<span class=""iComment"" data-chunk-id=""", "") _
                                             .Replace("href=""", "") _
                                             .Replace("""", "") _
                                             .Replace("http://www.songlyrics.com/annot-", "") _
                                             .Replace("</span>", "") _
                                             .Replace(">", "") _
                                             .Replace("0", "") _
                                             .Replace("1", "") _
                                             .Replace("2", "") _
                                             .Replace("3", "") _
                                             .Replace("4", "") _
                                             .Replace("5", "") _
                                             .Replace("6", "") _
                                             .Replace("7", "") _
                                             .Replace("8", "") _
                                             .Replace("9", "")

                LabelLyrics.Text = HtmlDecode(prselyrics)
                If Not UsingSpotify Then
                    IO.File.AppendAllText(SelectedSong_path, LabelLyrics.Text)
                End If


            Catch ex As Exception
                If Not UsingSpotify Then
                    SelectedSong = Playlist.Item(6, Playlist.CurrentRow.Index).Value
                    MP3Tag.Read(SelectedSong)


                    If ex.Message.Contains("404") Then
                        If MP3Tag.ID3v2Tag.Comments.ToString.Length >= 1 Then
                            LabelLyrics.Text = MP3Tag.ID3v2Tag.Comments _
                            .Replace(";", Environment.NewLine + Environment.NewLine)
                        Else
                            LabelLyrics.Text = "Unable to find lyrics for " & Chr(13) & Chr(13) & Label_SongName.Text + " by " + Label_Artist.Text
                        End If
                    End If
                End If
            End Try
        End Sub

#End Region

#Region "Lyrics Label & Panel Physics"
        Const SB_HORZ As Integer = 0
        Dim m_PanStartPoint As Point
        Public LyricsScrollPosition As Integer
        Public LyricsMoved As Boolean = False
        Public LyricsMoved2 As Boolean = False
        Public LyricsDown As Boolean = False

        'Scroll Lyrics
        Public Sub LabelLyrics_MouseDown_1(sender As Object, e As MouseEventArgs) 'Handles LabelLyrics.MouseDown
            Try
                'Capture the initial point 
                m_PanStartPoint = New Point(0, e.Y)
                LyricsDown = True
                LyricsScrollPosition = Panellyrics.AutoScrollPosition.Y
            Catch ex As Exception

            End Try

        End Sub
        Public Sub LabelLyrics_MouseMove(sender As Object, e As MouseEventArgs) 'Handles LabelLyrics.MouseMove
            Try
                If e.Button = MouseButtons.Left Then

                    'Get the change in coordinates.
                    Dim DeltaX As Integer = 0
                    Dim DeltaY As Integer = (m_PanStartPoint.Y - e.Y)

                    Panellyrics.AutoScrollPosition =
                    New Point((DeltaX - Panellyrics.AutoScrollPosition.X),
                              (DeltaY - Panellyrics.AutoScrollPosition.Y))
                    LyricsMoved = True
                    If LyricsDown = True Then
                        Timer_Meta_and_Artwork.Start()
                    End If

                    If Panellyrics.AutoScrollPosition.Y >= (LyricsScrollPosition + 50) Then
                        LyricsMoved2 = True
                    ElseIf Panellyrics.AutoScrollPosition.Y <= (LyricsScrollPosition - 50) Then
                        LyricsMoved2 = True
                    Else
                        LyricsMoved2 = False
                    End If
                End If
            Catch ex As Exception

            End Try

        End Sub
        Public Sub PanelLyrics_Scroll(sender As Object, e As ScrollEventArgs) ' Handles Panellyrics.Scroll

            Try

            Catch ex As Exception
                Timer_Meta_and_Artwork.Start()
            End Try
        End Sub

        Public Sub TimerLyrics_Tick(sender As Object, e As EventArgs) Handles Timer_Lyrics.Tick

            Try
                If AppOpenFinished = False Then Return
                Timer_Lyrics.Interval = 100
                If firstopen Then Return
                If Not BarCheckBox_ViewLyrics.Checked Then Return
                If NeedResizeRefresh = False Then Return
                If Panellyrics.Visible Then
                    LabelLyrics.Left = (Panellyrics.Width / 2) - (LabelLyrics.Width / 2)
                    Panellyrics.HorizontalScroll.Visible = False
                    Panellyrics.HorizontalScroll.Enabled = False
                    ShowScrollBar(Panellyrics.Handle, SB_HORZ, False)
                    LabelLyrics.Left = (Panellyrics.Width / 2) - (LabelLyrics.Width / 2)
                    TextboxLyrics.Left = (Panellyrics.Width / 2) - (LabelLyrics.Width / 2)
                End If
                Timer_Lyrics.Start()
            Catch ex As Exception

            End Try
        End Sub

#End Region
#Region "Edit Lyrics"
        'Customize CheckBox -- Allow for editing lyrics
        Public Sub CustomizeLyricsCheckbox_CheckedChanged(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarCheckBox_CustomizeLyrics.CheckedChanged
            If BarCheckBox_CustomizeLyrics.Checked Then
                My.Settings.CheckStateLyricsCustomize = True
            Else
                My.Settings.CheckStateLyricsCustomize = False
            End If
            My.Settings.Save()
        End Sub
        Public Sub EnableLyricEditing()
            BarCheckBox_CustomizeLyrics.Checked = Not BarCheckBox_CustomizeLyrics.Checked
        End Sub
        'Customize Lyrics
        Public Sub labels_MouseDoubleClick(ByVal sender As System.Object, ByVal e As MouseEventArgs) ' Handles LabelLyrics.MouseDoubleClick
            If Not BarCheckBox_CustomizeLyrics.Checked Then Return
            LabelLyrics = sender
            Me.TextboxLyrics.Text = LabelLyrics.Text
            Me.TextboxLyrics.Bounds = LabelLyrics.Bounds
            LabelLyrics.Visible = False
            Me.TextboxLyrics.Visible = True
            TextboxLyrics.Multiline = True
            Timer_Lyrics.Stop()
            TextboxLyrics.Dock = DockStyle.Top
            Me.TextboxLyrics.BringToFront()
            Me.TextboxLyrics.Select()
        End Sub
        'Edit Lyrics (Textbox)
        Public Sub Textbox1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As MouseEventArgs) 'Handles TextboxLyrics.MouseDoubleClick
            LabelLyrics.Text = Me.TextboxLyrics.Text
            LabelLyrics.Visible = True
            Me.TextboxLyrics.Visible = False
        End Sub
        Public Sub TextBox1_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs) ' Handles TextboxLyrics.KeyUp
            Return
            If e.KeyCode = Keys.Enter Then
                LabelLyrics.Text = Me.TextboxLyrics.Text
                LabelLyrics.Visible = True
                Me.TextboxLyrics.Visible = False
            End If
        End Sub
        'Manually Save Lyrics
        Public Sub BarButtonSaveLyrics_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_LyricsSave.ItemClick
            SaveLyrics()
        End Sub
        Public Sub SaveLyrics()
            IO.File.AppendAllText(SelectedSong_path_, LabelLyrics.Text)
        End Sub

#End Region

        Dim CurrentSong As String
        Public Sub LyricsTimer_Tick()

            Try
                If AppOpenFinished = False Then Return
                Dim tempSong As String
                tempSong = Label_SongName.Text

                If tempSong <> CurrentSong Then
                    If BarCheckBox_ViewLyrics.Checked Then
                        FindLyrics()
                    End If

                End If
            Catch ex As Exception

            End Try

        End Sub

        ' Reset Lyrics
        Public Sub BarBut_LyricsReset_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_LyricsReset.ItemClick
            ResetLyrics()
        End Sub
        Public Sub ResetLyrics()
            Dim SelectedSong As String
            Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next
            SelectedSong = Playlist.Item(6, Playlist.CurrentRow.Index).Value
            Dim SelectedSong_txt As String = Path.GetFileNameWithoutExtension(SelectedSong) & ".txt"
            Dim SelectedSong_path As String = IO.Path.Combine(Application.StartupPath & "\Lyrics\", SelectedSong_txt)

            SongLyrics(SelectedSong, SelectedSong_path)
        End Sub

#End Region

#Region " Enhanced Skins"
        Dim DontUseAgain As Boolean = False
        Public Sub BarCheckEnhancedSkins_CheckedChanged(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles EnhancedSkin1_Checkbox.CheckedChanged, _
            EnhancedSkin2_Checkbox.CheckedChanged, EnhancedSkin3_Checkbox.CheckedChanged, EnhancedSkin4_Checkbox.CheckedChanged, EnhancedSkin5_Checkbox.CheckedChanged, _
            EnhancedSkin6_Checkbox.CheckedChanged, EnhancedSkin7_Checkbox.CheckedChanged, EnhancedSkin8_Checkbox.CheckedChanged, EnhancedSkin9_Checkbox.CheckedChanged, _
            EnhancedSkin10_Checkbox.CheckedChanged, EnhancedSkin11_Checkbox.CheckedChanged, EnhancedSkin12_Checkbox.CheckedChanged, EnhancedSkin13_Checkbox.CheckedChanged, _
            EnhancedSkin14_Checkbox.CheckedChanged, EnhancedSkin15_Checkbox.CheckedChanged

            EnhancedSkinsCheckedChanged(sender, e)

        End Sub
        Public Sub EnhancedSkinsCheckedChanged(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
            If DontUseAgain = True Then Return

            Me.SuspendDrawing()

            Dim senderChecked As Boolean = sender.checked
            EnhancedSkin1_Checkbox.Checked = False
            EnhancedSkin2_Checkbox.Checked = False
            EnhancedSkin3_Checkbox.Checked = False
            EnhancedSkin4_Checkbox.Checked = False
            EnhancedSkin5_Checkbox.Checked = False
            EnhancedSkin6_Checkbox.Checked = False
            EnhancedSkin7_Checkbox.Checked = False
            EnhancedSkin8_Checkbox.Checked = False
            EnhancedSkin9_Checkbox.Checked = False
            EnhancedSkin10_Checkbox.Checked = False
            EnhancedSkin11_Checkbox.Checked = False
            EnhancedSkin12_Checkbox.Checked = False
            EnhancedSkin13_Checkbox.Checked = False
            EnhancedSkin14_Checkbox.Checked = False
            EnhancedSkin15_Checkbox.Checked = False
            If senderChecked = True Then
                DontUseAgain = True
                sender.checked = senderChecked
                My.Settings.EnhancedSkin = sender.tag
                My.Settings.hasbackground = True
            Else
                My.Settings.hasbackground = False
            End If
            If My.Settings.hasbackground = False Then
                Me.BackgroundImage = Nothing
            Else
                Select Case My.Settings.EnhancedSkin
                    Case 1
                        Me.BackgroundImage = My.Resources.form_bg1
                    Case 2
                        Me.BackgroundImage = My.Resources.form_bg_21
                    Case 3
                        Me.BackgroundImage = My.Resources.form_bg_31
                    Case 4
                        Me.BackgroundImage = My.Resources.form_bg_41
                    Case 5
                        Me.BackgroundImage = My.Resources.form_bg_51
                    Case 6
                        Me.BackgroundImage = My.Resources.form_bg_6
                    Case 7
                        Me.BackgroundImage = My.Resources.form_bg_71
                    Case 8
                        Me.BackgroundImage = My.Resources.form_bg_81
                    Case 9
                        Me.BackgroundImage = My.Resources.form_bg_9
                    Case 10
                        Me.BackgroundImage = My.Resources.form_bg_10
                    Case 11
                        Me.BackgroundImage = My.Resources.form_bg_11png
                    Case 12
                        Me.BackgroundImage = My.Resources.form_bg_12
                    Case 13
                        Me.BackgroundImage = My.Resources.form_bg_13
                    Case 14
                        Me.BackgroundImage = My.Resources.form_bg_14
                    Case 15
                        Me.BackgroundImage = My.Resources.form_bg_15
                End Select

            End If

            Me.ResumeDrawing()

            DontUseAgain = False
        End Sub

        'Scaling
        Public Sub barbut_ScaleNone_ItemClick(sender As Object, e As ItemClickEventArgs) Handles barbut_ScaleNone.ItemClick, barbut_ScaleStretch.ItemClick, _
      barbut_ScaleCenter.ItemClick, barbut_ScaleZoom.ItemClick, barbut_ScaleTile.ItemClick

            Select Case e.Item.Tag
                Case 0
                    Me.BackgroundImageLayout = ImageLayout.None
                Case 1
                    Me.BackgroundImageLayout = ImageLayout.Tile
                Case 2
                    Me.BackgroundImageLayout = ImageLayout.Center
                Case 3
                    Me.BackgroundImageLayout = ImageLayout.Stretch
                Case 4
                    Me.BackgroundImageLayout = ImageLayout.Zoom
            End Select
            My.Settings.BackgroundImageLayout = e.Item.Tag
        End Sub



        'Custom
        Dim CurrentImage As System.Drawing.Image
        Dim HasEnahancedBackground As Boolean = False
        Public Sub UseCustomImage_Checkbox_CheckedChanged(sender As Object, e As XtraBars.ItemClickEventArgs) Handles BarCheckBox_UseCustomImage.CheckedChanged

            SuspendDrawing()
            If BarCheckBox_UseCustomImage.Checked Then
                BarBut_ChangeCustomImage.Enabled = True
                If My.Settings.CustomImageFile = Nothing Then
                    If OpenPicture.ShowDialog = DialogResult.OK Then
                        My.Settings.CustomImageFile = OpenPicture.FileName
                    End If
                End If
                If AppOpen = False Then
                    Try
                        Me.BackgroundImage = ChangeOpacity(System.Drawing.Image.FromFile(My.Settings.CustomImageFile), 0.45)
                    Catch ex As Exception
                        MyMsgBox.Show("Error loading custom background image! Please check that image is not corrupted.", "Error!", True)
                    End Try

                Else

                    If AppOpenFinished Then
                        CurrentImage = Me.BackgroundImage
                        AddingFile = True
                        If OpenPicture.ShowDialog = DialogResult.OK Then
                            My.Settings.CustomImageFile = OpenPicture.FileName
                            Me.BackgroundImage = ChangeOpacity(System.Drawing.Image.FromFile(My.Settings.CustomImageFile), 0.45)
                        ElseIf DialogResult.Cancel Then
                            BarCheckBox_UseCustomImage.Checked = False
                        End If
                    End If

                End If


                EnhancedSkin1_Checkbox.Enabled = False
                EnhancedSkin2_Checkbox.Enabled = False
                EnhancedSkin3_Checkbox.Enabled = False
                EnhancedSkin4_Checkbox.Enabled = False
                EnhancedSkin5_Checkbox.Enabled = False
                EnhancedSkin6_Checkbox.Enabled = False
                EnhancedSkin7_Checkbox.Enabled = False
                EnhancedSkin8_Checkbox.Enabled = False
                EnhancedSkin9_Checkbox.Enabled = False
                EnhancedSkin10_Checkbox.Enabled = False
                EnhancedSkin11_Checkbox.Enabled = False
                EnhancedSkin12_Checkbox.Enabled = False
                EnhancedSkin13_Checkbox.Enabled = False
                EnhancedSkin14_Checkbox.Enabled = False
                EnhancedSkin15_Checkbox.Enabled = False


            Else
                BarBut_ChangeCustomImage.Enabled = False
                EnhancedSkin1_Checkbox.Enabled = True
                EnhancedSkin2_Checkbox.Enabled = True
                EnhancedSkin3_Checkbox.Enabled = True
                EnhancedSkin4_Checkbox.Enabled = True
                EnhancedSkin5_Checkbox.Enabled = True
                EnhancedSkin6_Checkbox.Enabled = True
                EnhancedSkin7_Checkbox.Enabled = True
                EnhancedSkin8_Checkbox.Enabled = True
                EnhancedSkin9_Checkbox.Enabled = True
                EnhancedSkin10_Checkbox.Enabled = True
                EnhancedSkin11_Checkbox.Enabled = True
                EnhancedSkin12_Checkbox.Enabled = True
                EnhancedSkin13_Checkbox.Enabled = True
                EnhancedSkin14_Checkbox.Enabled = True
                EnhancedSkin15_Checkbox.Enabled = True



                If HasEnahancedBackground Then
                    EnhancedSkin1_Checkbox.Checked = False
                    EnhancedSkin2_Checkbox.Checked = False
                    EnhancedSkin3_Checkbox.Checked = False
                    EnhancedSkin4_Checkbox.Checked = False
                    EnhancedSkin5_Checkbox.Checked = False
                    EnhancedSkin6_Checkbox.Checked = False
                    EnhancedSkin7_Checkbox.Checked = False
                    EnhancedSkin8_Checkbox.Checked = False
                    EnhancedSkin9_Checkbox.Checked = False
                    EnhancedSkin10_Checkbox.Checked = False
                    EnhancedSkin11_Checkbox.Checked = False
                    EnhancedSkin12_Checkbox.Checked = False
                    EnhancedSkin13_Checkbox.Checked = False
                    EnhancedSkin14_Checkbox.Checked = False
                    EnhancedSkin15_Checkbox.Checked = False
                    Setup_Enhanced_Skins()
                Else
                    If CurrentImage Is Nothing Then
                        Setup_Enhanced_Skins()
                    Else
                        Me.BackgroundImage = CurrentImage
                    End If
                End If
            End If
            My.Settings.CustomImageCheckState = BarCheckBox_UseCustomImage.Checked
            If My.Settings.SkinChanged Then
                Setup_Enhanced_Skins()
            End If
            ResumeDrawing()
        End Sub
        Public Sub ChangeCustomImageBut_ItemClick(sender As Object, e As XtraBars.ItemClickEventArgs) Handles BarBut_ChangeCustomImage.ItemClick
            If BarCheckBox_UseCustomImage.Checked Then
                CurrentImage = Me.BackgroundImage
                AddingFile = True
                If OpenPicture.ShowDialog = DialogResult.OK Then
                    My.Settings.CustomImageFile = OpenPicture.FileName
                    Me.BackgroundImage = ChangeOpacity(System.Drawing.Image.FromFile(My.Settings.CustomImageFile), 0.45)
                End If
            End If
        End Sub


        Public Shared timerRefreshWasEnabled As Boolean
        Public Shared timerRefresh2WasEnabled As Boolean

        'Wizard
        Public Sub EnhancedSkinRunWizard_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_EnhancedSkinRunWizard.ItemClick
            EnahancedSkins()
        End Sub
        Public Sub EnahancedSkins()
            RegisterStartupForm()
            Timer_Refresh2.Stop()
            SetupForm.Show()
            SetupForm.SuspendDrawing()
            SetupForm.Panel_Step1.Visible = False
            SetupForm.Panel_Step2.Visible = True
            SetupForm.Label_Main.Text = "Apply Enhanced Skin"
            SetupForm.Label_SelectedSkin.Visible = True
            SetupForm.But_Previous.Enabled = True
            SetupForm.But_Next.Enabled = True
            SetupForm.But_Next.Text = "Finish"
            SetupForm.ResumeDrawing()




        End Sub

#End Region


#Region " Spectrum Colors"

        Public Sub SpectrumColor1But_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_SpectrumColor1.ItemClick
            Dim ColorDialog As New ColorDialog
            ColorDialog.Color = SpectrumColor1
            ColorDialog.FullOpen = True
            If ColorDialog.ShowDialog = DialogResult.OK Then
                SpectrumColor1 = ColorDialog.Color
                My.Settings.SpectrumColor1 = SpectrumColor1
            End If
        End Sub
        Public Sub SpectrumColor2But_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_SpectrumColor2.ItemClick
            Dim ColorDialog As New ColorDialog
            ColorDialog.Color = SpectrumColor2
            ColorDialog.FullOpen = True
            If ColorDialog.ShowDialog = DialogResult.OK Then
                SpectrumColor2 = ColorDialog.Color
                My.Settings.SpectrumColor2 = SpectrumColor2
            End If
        End Sub
        Public Sub SpectrumColorDefaultBut_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_SpectrumColorDefault.ItemClick
            SpectrumColor1 = System.Drawing.ColorTranslator.FromHtml("#84C8FF")
            SpectrumColor2 = System.Drawing.ColorTranslator.FromHtml("#AE91FB")
            My.Settings.SpectrumColor1 = SpectrumColor1
            My.Settings.SpectrumColor2 = SpectrumColor2
        End Sub
        Public Sub EditSpectrumColors()
            If SpectrumColors.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                SpectrumColor1 = My.Settings.SpectrumColor1
                SpectrumColor2 = My.Settings.SpectrumColor2
            End If
        End Sub


#End Region

#Region " Playlist Tabs |   Orientation / Width"

        ' Width
        Public Sub PlaylistTabsChangeWidth_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_PlaylistTabsChangeWidth.ItemClick
            PlaylistTabsChangeWidth()
        End Sub
        Public Sub PlaylistTabsChangeWidth()
            Dim xform As New MyInputBox
            xform.Label1.Text = "Type in desire Playlist Tabs width/height, then click Okay."
            Try
                If xform.ShowDialog = DialogResult.OK Then
                    PlaylistTabs.TabPageWidth = CInt(xform.TextEdit1.Text)
                    My.Settings.TabWidth = CInt(xform.TextEdit1.Text)
                End If
            Catch
            End Try
        End Sub

        ' Orientation
        Public Sub PlaylistTabsHorBut_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_PlaylistTabsHor.ItemClick, BarBut_PlaylistTabsVert.ItemClick


            If e.Item.Name = "PlaylistTabsHorBut" Then
                PlaylistTabs.HeaderOrientation = TabOrientation.Horizontal
                My.Settings.TabOrientation = 0

            ElseIf e.Item.Name = "PlaylistTabsVertBut" Then
                PlaylistTabs.HeaderOrientation = TabOrientation.Vertical
                My.Settings.TabOrientation = 1
            End If
            My.Settings.Save()


        End Sub
        Public Sub SwitchOrientation()
            Select Case My.Settings.TabOrientation
                Case 0
                    PlaylistTabs.HeaderOrientation = TabOrientation.Vertical
                    My.Settings.TabOrientation = 1
                    My.Settings.Save()
                    Exit Select
                Case 1
                    PlaylistTabs.HeaderOrientation = TabOrientation.Horizontal
                    My.Settings.TabOrientation = 0
                    My.Settings.Save()
                    Exit Select
            End Select

        End Sub

#End Region



#End Region
#Region " Options"
        'Show Options Window
        Public Sub BarButtonItem1_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_Options.ItemClick
            Options()
        End Sub
        Public Sub Options()
            If Not Me.BackgroundImage Is Nothing Then
                My.Settings.hasbackground = True
            Else
                My.Settings.hasbackground = False
            End If
            Dim xform As New OptionsForm1
            xform.ShowDialog()
            If xform.DialogResult = DialogResult.OK Then
                Dim CurrentPos As Long = AudioPlayer.Instance.CurrentAudioHandle.Position
                Dim WasPlaying As Boolean = False
                If IsVideo Then
                    If VLC_installed Then
                        If VlcPlayer.playlist.isPlaying Then 'If VlcPlayer.playlist.isPlaying Then
                            WasPlaying = True
                        End If
                    End If

                Else
                    If AudioPlayer.Instance.GetStreamStatus = BASSActive.BASS_ACTIVE_PLAYING Then
                        WasPlaying = True
                    End If
                End If


                Bass.BASS_Free()
                Dim AP As New AudioPlayer
                AudioPlayer.Instance.LoadAudio()


                Try
                    Dim array = TryCast(xform.DeviceBox.Properties.Items(xform.DeviceBox.SelectedIndex), String).Split(" "c)
                    My.Settings.DefaultDevice = Convert.ToInt32(Array(0))
                Catch ex As Exception
                    My.Settings.DefaultDevice = 0
                End Try


                Dim result As Boolean = Bass.BASS_Init(My.Settings.DefaultDevice, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero)
                If Not result Then
                    Dim [error] = Bass.BASS_ErrorGetCode()
                    MyMsgBox.Show([error].ToString(), "", True)
                End If


                '  Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATETHREADS, False)
                '  result = Bass.BASS_Init(OptionsForm1.DeviceBox.SelectedIndex, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero)
                If Not result Then
                    ' Throw New Exception("Init Error")
                End If
                Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.SelectedTabPage.Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next
                If Not Playlist.RowCount = 0 Then
                    If IsVideo Then
                        If VLC_installed Then
                            If WasPlaying Then
                                VlcPlayer.playlist.play()
                            Else
                                VlcPlayer.playlist.play()
                            End If
                        End If

                    Else
                        DoubleClickPlay()
                        AudioPlayer.Instance.CurrentAudioHandle.Position = CurrentPos
                        If WasPlaying Then
                            AudioPlayer.Instance.Play(False)
                        Else
                            AudioPlayer.Instance.Pause()
                        End If

                    End If
                End If






                'Set Checkboxes
                My.Settings.StartupCheckBoxState = My.Settings.StartupCheckBoxStateCurrent
                '   My.Settings.PlayOnStart = My.Settings.PlayWhenTabActive(PlaylistTabs.SelectedTabPageIndex)

                Checkbox_Startup()

                Checkbox_TouchFriendly()

                'Save
                My.Settings.Save()
            End If
        End Sub
        'Startup Checkbox
        Public Sub Checkbox_Startup()
            If My.Settings.StartupCheckBoxStateCurrent = True Then         'ADD

                Dim WshShell As IWshRuntimeLibrary.WshShell = New IWshRuntimeLibrary.WshShell()
                Dim ShortcutPath As String = Environment.GetFolderPath(Environment.SpecialFolder.Startup)
                Dim Shortcut As IWshRuntimeLibrary.IWshShortcut = CType(WshShell.CreateShortcut(System.IO.Path.Combine(ShortcutPath, Application.ProductName) & ".lnk"), IWshRuntimeLibrary.IWshShortcut)
                Shortcut.TargetPath = Application.ExecutablePath
                Shortcut.WorkingDirectory = Application.StartupPath

                Shortcut.Save()

            Else                              'REMOVE
                Try
                    Dim startUpFolderPath As String = Environment.GetFolderPath(Environment.SpecialFolder.Startup)
                    Dim di As New DirectoryInfo(startUpFolderPath)
                    Dim files As FileInfo() = di.GetFiles()

                    For Each fi As FileInfo In files

                        If fi.ToString.Contains("Rich_Player") Then ', StringComparison.InvariantCultureIgnoreCase) Then
                            System.IO.File.Delete(fi.FullName)
                        End If
                    Next
                Catch
                End Try
            End If
        End Sub
        Public Function GetShortcutTargetFile(shortcutFilename As String) As String
            Dim pathOnly As String = Path.GetDirectoryName(shortcutFilename)
            Dim filenameOnly As String = Path.GetFileName(shortcutFilename)

            Dim shell As Shell32.Shell = New Shell32.ShellClass()
            Dim folder As Shell32.Folder = shell.[NameSpace](pathOnly)
            Dim folderItem As Shell32.FolderItem = folder.ParseName(filenameOnly)
            If folderItem IsNot Nothing Then
                Dim link As Shell32.ShellLinkObject = DirectCast(folderItem.GetLink, Shell32.ShellLinkObject)
                Return link.Path
            End If

            Return [String].Empty
            ' Not found
        End Function



        'Enable Touch Friendly Mode
        Public Sub Checkbox_TouchFriendly()
            Return
            PlaybackControlsTouchChanged()
            'Skin
            If My.Settings.TouchFriendly = True Then
                SkinChange_Playlists_Touch()
                ViewOtherControlsCheckbox.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
                If My.Settings.WasChecked Then
                    '  PanelTouch.Visible = True
                End If
            Else
                SkinChange_Playlists_Standard()
                ViewOtherControlsCheckbox.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
                '  PanelTouch.Visible = False
            End If
            My.Settings.WasChecked = ViewOtherControlsCheckbox.Checked
        End Sub
        Public Sub PlaybackControlsTouchChanged()
            ''SpeedCoverupBox.Visible = False
            Select Case AppOpen
                Case True
                    Select Case My.Settings.TouchFriendly
                        Case True
                            Dim PlayY As Integer = 32
                            Dim StopY As Integer = 37
                            Dim PreviousY As Integer = 37
                            Dim NextY As Integer = 37
                            Dim SepVal As Integer = 27
                            Dim OtherVal As Integer = 34

                            But_PlayPause.Size = New Size(30, 30)
                            But_PlayPause.Location = New Point(2, PlayY - 3)

                            But_Stop.Size = New Size(23, 23)
                            But_Stop.Location = New Point(30, StopY - 3)

                            But_Previous.Size = New Size(23, 23)
                            But_Previous.Location = New Point(58, PreviousY - 3)

                            But_Next.Size = New Size(23, 23)
                            But_Next.Location = New Point(86, NextY - 3)

                            'ABrepeatBG.Visible = False
                            ' SpeedControlBG.Visible = False

                            ControlsSeperator1.Size = New Size(8, 42)
                            ControlsSeperator1.Location = New Point(111, SepVal - 3)

                            But_Repeat.Size = New Size(28, 30)
                            But_Repeat.Location = New Point(122, OtherVal - 4)

                            But_Shuffle.Size = New Size(30, 28)
                            But_Shuffle.Location = New Point(148, OtherVal - 4)

                            But_A.Size = New Size(22, 22)
                            But_A.Location = New Point(189, PlayY + 1)

                            But_AB_Reset.Size = New Size(21, 24)
                            But_AB_Reset.Location = New Point(216, OtherVal - 2)

                            But_B.Size = New Size(22, 22)
                            But_B.Location = New Point(245, PlayY + 2)

                            But_SpeedDown.Size = New Size(22, 22)
                            But_SpeedDown.Location = New Point(283, PlayY + 1)

                            SpeedResetBut.Size = New Size(21, 24)
                            SpeedResetBut.Location = New Point(319, OtherVal - 2)

                            trackBar_Speed2.Location = New Point(300, 31)


                            But_SpeedUp.Size = New Size(28, 28)
                            But_SpeedUp.Location = New Point(373, PlayY + 1)

                            Label_SpeedTextbox.Size = New Size(32, 24)
                            Label_SpeedTextbox.Location = New Point(400, PlayY + 5)
                            '   SpeedCoverupBox.Location = New Point(385, PlayY + 17)

                            But_PitchDown.Size = New Size(28, 28)
                            But_PitchDown.Location = New Point(440, PlayY + 1)

                            trackbar_Pitch2.Location = New Point(457, 31)

                            But_PitchUp.Size = New Size(28, 28)
                            But_PitchUp.Location = New Point(530, OtherVal - 2)

                            PicBox_PitchTextBox.Size = New Size(32, 24)
                            PicBox_PitchTextBox.Location = New Point(557, PlayY + 5)
                        Case False
                            'ABrepeatBG.Visible = True
                            ' SpeedControlBG.Visible = True

                            But_PlayPause.Size = New Size(22, 22)
                            But_PlayPause.Location = New Point(6, 34)

                            But_Stop.Size = New Size(15, 15)
                            But_Stop.Location = New Point(30, 38)

                            But_Previous.Size = New Size(15, 15)
                            But_Previous.Location = New Point(48, 38)

                            But_Next.Size = New Size(15, 15)
                            But_Next.Location = New Point(66, 38)

                            ControlsSeperator1.Size = New Size(8, 34)
                            ControlsSeperator1.Location = New Point(83, 28)

                            But_Repeat.Size = New Size(18, 20)
                            But_Repeat.Location = New Point(92, 35)

                            But_Shuffle.Size = New Size(20, 18)
                            But_Shuffle.Location = New Point(116, 36)

                            But_A.Size = New Size(14, 14)
                            But_A.Location = New Point(156, 38)

                            But_AB_Reset.Size = New Size(13, 16)
                            But_AB_Reset.Location = New Point(179, 37)

                            But_B.Size = New Size(14, 14)
                            But_B.Location = New Point(203, 38)

                            But_SpeedDown.Size = New Size(15, 15)
                            But_SpeedDown.Location = New Point(236, 38)

                            SpeedResetBut.Size = New Size(13, 16)
                            SpeedResetBut.Location = New Point(266, 37)

                            trackBar_Speed2.Location = New Point(252, 31)

                            But_SpeedUp.Size = New Size(15, 15)
                            But_SpeedUp.Location = New Point(333, 38)

                            Label_SpeedTextbox.Size = New Size(24, 16)
                            Label_SpeedTextbox.Location = New Point(354, 38)

                            But_PitchDown.Size = New Size(15, 15)
                            But_PitchDown.Location = New Point(395, 39)

                            trackbar_Pitch2.Location = New Point(411, 31)

                            But_PitchUp.Size = New Size(15, 15)
                            But_PitchUp.Location = New Point(492, 39)

                            PicBox_PitchTextBox.Size = New Size(24, 16)
                            PicBox_PitchTextBox.Location = New Point(508, 38)
                    End Select

                Case False

                    Dim PlayY As Integer = 32
                    Dim StopY As Integer = 37
                    Dim PreviousY As Integer = 37
                    Dim NextY As Integer = 37
                    Dim SepVal As Integer = 27
                    Dim OtherVal As Integer = 34

                    Select Case My.Settings.TouchFriendly
                        Case True


                            'ABrepeatBG.Visible = False
                            ' SpeedControlBG.Visible = False

                            But_PlayPause.Size = New Size(30, 30)
                            But_PlayPause.Location = New Point(2, PlayY - 3)

                            But_Stop.Size = New Size(23, 23)
                            But_Stop.Location = New Point(30, StopY - 3)

                            But_Previous.Size = New Size(23, 23)
                            But_Previous.Location = New Point(58, PreviousY - 3)

                            But_Next.Size = New Size(23, 23)
                            But_Next.Location = New Point(86, NextY - 3)

                            'ABrepeatBG.Visible = False
                            ' SpeedControlBG.Visible = False

                            ControlsSeperator1.Size = New Size(8, 42)
                            ControlsSeperator1.Location = New Point(111, SepVal - 3)

                            But_Repeat.Size = New Size(28, 30)
                            But_Repeat.Location = New Point(122, OtherVal - 4)

                            But_Shuffle.Size = New Size(30, 28)
                            But_Shuffle.Location = New Point(148, OtherVal - 4)

                            But_A.Size = New Size(22, 22)
                            But_A.Location = New Point(189, PlayY + 1)

                            But_AB_Reset.Size = New Size(21, 24)
                            But_AB_Reset.Location = New Point(216, OtherVal - 2)

                            But_B.Size = New Size(22, 22)
                            But_B.Location = New Point(245, PlayY + 2)

                            But_SpeedDown.Size = New Size(28, 28)
                            But_SpeedDown.Location = New Point(283, PlayY + 1)

                            SpeedResetBut.Size = New Size(21, 24)
                            SpeedResetBut.Location = New Point(319, OtherVal - 2)

                            trackBar_Speed2.Location = New Point(300, 31)

                            But_SpeedUp.Size = New Size(28, 28)
                            But_SpeedUp.Location = New Point(373, PlayY + 1)

                            Label_SpeedTextbox.Size = New Size(32, 24)
                            Label_SpeedTextbox.Location = New Point(400, PlayY + 5)
                            '   SpeedCoverupBox.Location = New Point(385, PlayY + 17)

                            But_PitchDown.Size = New Size(28, 28)
                            But_PitchDown.Location = New Point(440, PlayY + 1)

                            trackbar_Pitch2.Location = New Point(457, 31)

                            But_PitchUp.Size = New Size(28, 28)
                            But_PitchUp.Location = New Point(530, OtherVal - 2)

                            PicBox_PitchTextBox.Size = New Size(32, 24)
                            PicBox_PitchTextBox.Location = New Point(557, PlayY + 5)
                        Case False

                            'ABrepeatBG.Visible = True
                            ' SpeedControlBG.Visible = True

                            But_PlayPause.Size = New Size(22, 22)
                            But_PlayPause.Location = New Point(6, PlayY)

                            But_Stop.Size = New Size(15, 15)
                            But_Stop.Location = New Point(30, StopY)

                            But_Previous.Size = New Size(15, 15)
                            But_Previous.Location = New Point(48, PreviousY)

                            But_Next.Size = New Size(15, 15)
                            But_Next.Location = New Point(66, NextY)

                            ControlsSeperator1.Size = New Size(8, 34)
                            ControlsSeperator1.Location = New Point(83, SepVal)

                            But_Repeat.Size = New Size(18, 20)
                            But_Repeat.Location = New Point(92, OtherVal)

                            But_Shuffle.Size = New Size(20, 18)
                            But_Shuffle.Location = New Point(116, OtherVal + 1)

                            But_A.Size = New Size(14, 14)
                            But_A.Location = New Point(156, OtherVal + 4)

                            But_AB_Reset.Size = New Size(13, 16)
                            But_AB_Reset.Location = New Point(179, OtherVal + 2)

                            But_B.Size = New Size(14, 14)
                            But_B.Location = New Point(203, OtherVal + 4)

                            But_SpeedDown.Size = New Size(15, 15)
                            But_SpeedDown.Location = New Point(236, 38)

                            SpeedResetBut.Size = New Size(13, 16)
                            SpeedResetBut.Location = New Point(266, 37)

                            trackBar_Speed2.Location = New Point(252, 31)

                            But_SpeedUp.Size = New Size(15, 15)
                            But_SpeedUp.Location = New Point(333, 38)

                            Label_SpeedTextbox.Size = New Size(24, 16)

                            Label_SpeedTextbox.Location = New Point(354, 38)
                            '   SpeedCoverupBox.Location = New Point(309, 49)

                            But_PitchDown.Size = New Size(15, 15)
                            But_PitchDown.Location = New Point(395, 39)

                            trackbar_Pitch2.Location = New Point(411, 31)

                            But_PitchUp.Size = New Size(15, 15)
                            But_PitchUp.Location = New Point(492, 39)

                            PicBox_PitchTextBox.Size = New Size(24, 16)
                            PicBox_PitchTextBox.Location = New Point(508, 38)
                    End Select
            End Select
        End Sub

        Public Sub SkinChange_Playlists_Touch()


            Try

                Dim TabIndex As Integer = PlaylistTabs.SelectedTabPageIndex
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

                If My.Settings.SkinChanged = True Then
                    For i As Integer = 0 To Playlist.RowCount - 1
                        Select Case Playlist_Rowheight
                            Case Nothing
                                Playlist.Rows(i).Height = 35
                                '  Case Is < 30
                                '     Playlist.Rows(i).Height = 35
                            Case Else
                                Playlist.Rows(i).Height = Playlist_Rowheight
                        End Select
                    Next
                End If


                Try
                    Playlist.RowHeadersWidth = 60
                Catch
                End Try



                SongArrangePanel.MaximumSize = New Size(PlaylistTabs.TabPages(TabIndex).Width, 36)
                SongArrangePanel.MinimumSize = New Size(PlaylistTabs.TabPages(TabIndex).Width, 36)
                SongArrangePanel.Size = New Size(PlaylistTabs.TabPages(TabIndex).Width, 36)

                OpenProgressBar.MaximumSize = New Size(PlaylistTabs.TabPages(TabIndex).Width, 36)
                OpenProgressBar.MinimumSize = New Size(PlaylistTabs.TabPages(TabIndex).Width, 36)
                OpenProgressBar.Size = New Size(PlaylistTabs.TabPages(TabIndex).Width, 36)



                Playlist.Font = New Font("Segoe UI", 11)
                PlaylistTabs.AppearancePage.Header.Font = New Font("Segoe UI", 9.75)



                If VLCChapterMarks.Visible Then
                    But_MoveTop.Size = New Size(22, 22)
                    But_MoveTop.Location = New Point(0, 2)
                    But_MoveUp.Size = New Size(22, 22)
                    But_MoveUp.Location = New Point(26, 2)
                    But_MoveDown.Size = New Size(22, 22)
                    But_MoveDown.Location = New Point(52, 2)
                    But_MoveBottom.Size = New Size(22, 22)
                    But_MoveBottom.Location = New Point(78, 2)

                    But_Add.Size = New Size(22, 22)
                    But_Add.Location = New Point(SongArrangePanel.Width - 60, 2)
                    But_Minus.Size = New Size(22, 22)
                    But_Minus.Location = New Point(SongArrangePanel.Width - 30, 2)

                    But_SavePosition_SAP.Size = New Size(22, 22)
                    But_SavePosition_SAP.Location = New Point(110, 2)
                    But_RemovePosition_SAP.Size = New Size(22, 22)
                    But_RemovePosition_SAP.Location = New Point(136, 2)

                    But_Fav.Size = New Size(14, 22)
                    But_Fav.Location = New Point(168, 2)
                Else
                    But_MoveTop.Size = New Size(28, 28)
                    But_MoveTop.Location = New Point(2, 2)
                    But_MoveUp.Size = New Size(28, 28)
                    But_MoveUp.Location = New Point(34, 2)
                    But_MoveDown.Size = New Size(28, 28)
                    But_MoveDown.Location = New Point(66, 2)
                    But_MoveBottom.Size = New Size(28, 28)
                    But_MoveBottom.Location = New Point(98, 2)

                    But_Add.Size = New Size(28, 28)
                    But_Add.Location = New Point(SongArrangePanel.Width - 64, 2)
                    But_Minus.Size = New Size(28, 28)
                    But_Minus.Location = New Point(SongArrangePanel.Width - 30, 2)

                    But_SavePosition_SAP.Size = New Size(28, 28)
                    But_SavePosition_SAP.Location = New Point(140, 2)
                    But_RemovePosition_SAP.Size = New Size(28, 28)
                    But_RemovePosition_SAP.Location = New Point(172, 2)

                    But_Fav.Size = New Size(20, 28)
                    But_Fav.Location = New Point(208, 2)
                End If





                But_Search.Size = New Size(20, 28)
                ' SearchBut.Location = New Point(296, 2)

                TextBox_PlaylistSearch.MaximumSize = New Size(0, 30)
                TextBox_PlaylistSearch.MinimumSize = New Size(0, 30)
                TextBox_PlaylistSearch.Size = New Size(SongArrangePanel.Width - 232 - 64 - 6, 30)
                TextBox_PlaylistSearch.Location = New Point(232, 2)

                But_Search.Location = New Point(TextBox_PlaylistSearch.Location.X + TextBox_PlaylistSearch.Width - 21, But_Search.Location.Y)


                Dim y As Integer = PlaylistTabs.TabPages(TabIndex).Height - SongArrangePanel.Height - TextBox_PlaylistSearch.Height
                Try
                    Scroller.Dock = DockStyle.Top
                    Scroller.Size = New Size(PlaylistTabs.TabPages(TabIndex).Width, y)
                Catch
                End Try
                'Song Arrangement Buttons
                If My.Settings.SkinChanged = True Then
                    Try
                        Scroller.Dock = DockStyle.Top
                        Scroller.Size = New Size(PlaylistTabs.TabPages(TabIndex).Width, y)
                        Playlist.Size = New Size(PlaylistTabs.TabPages(TabIndex).Width, y)
                    Catch
                    End Try
                End If

                OpenProgressBar.BringToFront()
                My.Settings.SkinChanged = False
            Catch
            End Try
        End Sub
        Public Sub SkinChange_Playlists_Standard()


            Try

                Dim tabindex As Integer = PlaylistTabs.SelectedTabPageIndex

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

                If My.Settings.SkinChanged = True Then
                    For i As Integer = 0 To Playlist.RowCount - 1
                        Select Case Playlist_Rowheight
                            Case Nothing
                                Playlist.Rows(i).Height = 22
                                '  Case Is < 30
                                '     Playlist.Rows(i).Height = 35
                            Case Else
                                Playlist.Rows(i).Height = Playlist_Rowheight
                        End Select
                    Next
                End If


                Dim w As Integer = (PlaylistTabs.TabPages(tabindex).Width)
                SongArrangePanel.MaximumSize = New Size(w, 23)
                SongArrangePanel.MinimumSize = New Size(w, 23)
                SongArrangePanel.Size = New Size(w, 23)


                OpenProgressBar.MaximumSize = New Size(PlaylistTabs.TabPages(tabindex).Width, 23)
                OpenProgressBar.MinimumSize = New Size(PlaylistTabs.TabPages(tabindex).Width, 23)
                OpenProgressBar.Size = New Size(PlaylistTabs.TabPages(tabindex).Width, 23)






                But_MoveTop.Size = New Size(18, 16)
                But_MoveTop.Location = New Point(2, 2)
                But_MoveUp.Size = New Size(18, 16)
                But_MoveUp.Location = New Point(26, 2)
                But_MoveDown.Size = New Size(18, 16)
                But_MoveDown.Location = New Point(50, 2)
                But_MoveBottom.Size = New Size(18, 16)
                But_MoveBottom.Location = New Point(74, 2)

                But_Add.Size = New Size(18, 16)
                But_Add.Location = New Point(SongArrangePanel.Width - 46, 2) '331
                But_Minus.Size = New Size(18, 16)
                But_Minus.Location = New Point(SongArrangePanel.Width - 22, 2)

                But_SavePosition_SAP.Size = New Size(18, 16)
                But_SavePosition_SAP.Location = New Point(108, 2)
                But_RemovePosition_SAP.Size = New Size(18, 16)
                But_RemovePosition_SAP.Location = New Point(132, 2)

                But_Fav.Size = New Size(18, 16)
                But_Fav.Location = New Point(156, 2)

                But_Search.Size = New Size(18, 16)
                But_Search.Top = 2
                ' SearchBut.Location = New Point(298, 3)


                TextBox_PlaylistSearch.MaximumSize = New Size(0, 22)
                TextBox_PlaylistSearch.MinimumSize = New Size(0, 22)
                TextBox_PlaylistSearch.Size = New Size(SongArrangePanel.Width - 180 - 46 - 6, 20)
                TextBox_PlaylistSearch.Location = New Point(180, 2)

                But_Search.Location = New Point(TextBox_PlaylistSearch.Location.X + TextBox_PlaylistSearch.Width - 19, But_Search.Location.Y)


                Dim y As Integer = PlaylistTabs.TabPages(tabindex).Height - SongArrangePanel.Height - TextBox_PlaylistSearch.Height
                Try
                    Scroller.Dock = DockStyle.Top
                    Scroller.Size = New Size(PlaylistTabs.TabPages(tabindex).Width, y)
                Catch

                End Try

                'Song Arrangement Buttons
                If My.Settings.SkinChanged = True Then
                    Try
                        Scroller.Dock = DockStyle.Top
                        Scroller.Size = New Size(PlaylistTabs.TabPages(tabindex).Width, y)
                        Playlist.Size = New Size(PlaylistTabs.TabPages(tabindex).Width, y)
                    Catch
                    End Try

                End If

                OpenProgressBar.BringToFront()
                My.Settings.SkinChanged = False
            Catch
            End Try
        End Sub
        Public Sub TimerPlaylistsSizes_Tick(sender As Object, e As EventArgs) Handles Timer_PlaylistsSizes.Tick


            If AddingFiles Then Return
            If AppOpenFinished = False Then Return
            If AppOpen = True Then
                SuspendDrawing()
            End If

            Try
                If NeedResizeRefresh = True Then

                    Dim tabindex As Integer = PlaylistTabs.SelectedTabPageIndex
                    Dim FoundOnce As Boolean = False
                    SetupControlLocations()

                    Dim Playlist As GridPlaylist
                    Dim Scroller As Scroller
                    For Each c As Control In PlaylistTabs.SelectedTabPage.Controls
                        If c.GetType Is GetType(Scroller) Then
                            Scroller = c
                            For Each c2 As Control In Scroller.Controls
                                If c2.GetType Is GetType(GridPlaylist) Then
                                    Playlist = c2
                                    PlaylistIsReady = True
                                    FoundOnce = True
                                Else
                                    If FoundOnce = False Then
                                        PlaylistIsReady = False
                                    End If
                                End If
                            Next
                        End If
                    Next


                    If AddingPlaylist Then Return
                    If PlaylistIsReady = False Then Return
                    If Playlist.Rows.Count = 0 Then Return

                    If My.Settings.TouchFriendly = True Then
                        SkinChange_Playlists_Touch()
                    Else
                        SkinChange_Playlists_Standard()
                    End If

                    Timer_PlaylistsSizes.Start()
                    NeedResizeRefresh = False

                Else
                    Timer_PlaylistsSizes.Stop()
                End If


            Catch
            End Try
            If AppOpen = True Then
                ResumeDrawing()
            End If
        End Sub

        Public Sub TabPage1_MouseClick(sender As Object, e As MouseEventArgs) Handles TabPage1.MouseClick, SongArrangePanel.MouseClick
            If My.Settings.TouchFriendly = True Then
                SkinChange_Playlists_Touch()
            Else
                SkinChange_Playlists_Standard()
            End If
        End Sub

        Public Sub SongArrangePanel_MouseEnter(sender As Object, e As EventArgs) Handles SongArrangePanel.MouseEnter
            If My.Settings.TouchFriendly = True Then
                SkinChange_Playlists_Touch()
            Else
                SkinChange_Playlists_Standard()
            End If
        End Sub


#End Region
#Region " About & Release Notes"

        'About
        Public Sub BarButtonAbout_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_About.ItemClick
            Dim xform As New AboutForm
            xform.ShowDialog()

        End Sub

        'Release Notes
        Public Sub BarButtonReleaseNotes_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_ReleaseNotes.ItemClick
            Dim xform As New ReleaseNotes
            xform.ShowDialog()
            If xform.DialogResult = DialogResult.OK Then

            End If
        End Sub

#End Region
#Region " Open App Location"

        Public Sub OpenAppLocationBut_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_OpenAppLocation.ItemClick
            OpenAppLocation()
        End Sub
        Public Sub OpenAppLocation()
            Process.Start("Explorer.exe", IO.Path.GetDirectoryName(Application.ExecutablePath))
        End Sub

#End Region '
#Region " Close"

        Public Sub BarClose_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarBut_Exit.ItemClick
            Me.Close()
            'Application.ExitThread()
        End Sub

#End Region


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
                If IsRadioStation Then
                    If RadioPlayState = "Playing" Then
                        But_PlayPause.BackgroundImage = PauseHoverImage
                    Else
                        But_PlayPause.BackgroundImage = PlayHoverImage
                    End If
                Else
                    If VLC_installed Then
                        If VlcPlayer.playlist.isPlaying = True Then
                            But_PlayPause.BackgroundImage = PauseHoverImage
                        ElseIf VlcPlayer.playlist.isPlaying = False Then ' If VlcPlayer.playlist.isPlaying = False Then

                            But_PlayPause.BackgroundImage = PlayHoverImage
                        End If
                    End If

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
                If IsRadioStation Then
                    If RadioPlayState = "Playing" Then
                        But_PlayPause.BackgroundImage = PauseImage
                    Else
                        But_PlayPause.BackgroundImage = PlayImage
                    End If
                Else
                    If VLC_installed Then
                        If VlcPlayer.playlist.isPlaying = True Then
                            But_PlayPause.BackgroundImage = PauseImage
                        ElseIf VlcPlayer.playlist.isPlaying = False Then ' If VlcPlayer.playlist.isPlaying = False Then

                            But_PlayPause.BackgroundImage = PlayImage
                        End If
                    End If

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
                If IsRadioStation Then
                    If RadioPlayState = "Playing" Then
                        But_PlayPause.BackgroundImage = PausePressImage
                    Else
                        But_PlayPause.BackgroundImage = PlayPressImage
                    End If
                Else
                    If VLC_installed Then
                        If VlcPlayer.playlist.isPlaying = True Then
                            But_PlayPause.BackgroundImage = PausePressImage
                        ElseIf VlcPlayer.playlist.isPlaying = False Then ' If VlcPlayer.playlist.isPlaying = False Then
                            But_PlayPause.BackgroundImage = PlayPressImage
                        End If
                    End If

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
                        But_PlayPause.BackgroundImage = PauseHoverImage : Else
                        But_PlayPause.BackgroundImage = PlayHoverImage : End If
                Else
                    If AudioPlayer.Instance.GetStreamStatus = BASSActive.BASS_ACTIVE_PLAYING Then
                        But_PlayPause.BackgroundImage = PauseImage : Else
                        But_PlayPause.BackgroundImage = PlayImage : End If
                End If
            Else
                If IsRadioStation Then
                    If playhover = True Then
                        If RadioPlayState = "Playing" Then
                            But_PlayPause.BackgroundImage = PauseHoverImage : Else
                            But_PlayPause.BackgroundImage = PlayHoverImage : End If
                    Else
                        If RadioPlayState = "Playing" Then
                            But_PlayPause.BackgroundImage = PauseImage : Else
                            But_PlayPause.BackgroundImage = PlayImage : End If
                    End If
                Else
                    If VLC_installed Then
                        If playhover = True Then
                            If VlcPlayer.playlist.isPlaying = True Then
                                But_PlayPause.BackgroundImage = PauseHoverImage : ElseIf VlcPlayer.playlist.isPlaying = False Then
                                But_PlayPause.BackgroundImage = PlayHoverImage : End If
                        Else
                            If VlcPlayer.playlist.isPlaying = True Then
                                But_PlayPause.BackgroundImage = PauseImage : ElseIf VlcPlayer.playlist.isPlaying = False Then
                                But_PlayPause.BackgroundImage = PlayImage : End If
                        End If
                    End If

                End If

            End If
            CheckIfVideo()
            If IsVideo = False Then
                If AudioPlayer.Instance.TrackList.Tracks.Count > 0 Then : If playhover Then
                        New_Play() : End If
                End If
            Else
                VideoPlay()
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
            ElseIf repeat = False Then
                But_Repeat.BackgroundImage = RepeatOffHoverImage
            End If
            repeatbuthover = True
        End Sub
        Public Sub repeatbut2_MouseLeave(sender As Object, e As EventArgs) Handles But_Repeat.MouseLeave
            If repeatAll Then
                But_Repeat.BackgroundImage = RepeatAllImage
            ElseIf repeatOne Then
                But_Repeat.BackgroundImage = RepeatOneImage
            ElseIf repeat = False Then
                But_Repeat.BackgroundImage = RepeatOffImage
            End If
            repeatbuthover = False
        End Sub
        Public Sub repeatbut2_MouseDown(sender As Object, e As MouseEventArgs) Handles But_Repeat.MouseDown
            If repeatAll Then
                But_Repeat.BackgroundImage = RepeatAllPressImage
            ElseIf repeatOne Then
                But_Repeat.BackgroundImage = RepeatOnePressImage
            ElseIf repeat = False Then
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
                ElseIf repeat = False Then
                    But_Repeat.BackgroundImage = RepeatOffHoverImage
                End If
            Else
                '' Standard Version
                If repeatAll Then
                    But_Repeat.BackgroundImage = RepeatAllImage
                ElseIf repeatOne Then
                    But_Repeat.BackgroundImage = RepeatOneImage
                ElseIf repeat = False Then
                    But_Repeat.BackgroundImage = RepeatOffImage
                End If
            End If

            '' Standard Version
            If repeatOne = False And repeatAll = False Then
                repeat = True
                repeatOne = True
                If UsingSpotify Then
                    _SpotifyNew.SetRepeatMode(RepeatState.Track)
                Else
                    My.Settings.Repeat = 1
                End If
                repeatAll = False
                But_Repeat.BackgroundImage = RepeatOneImage

            ElseIf repeatOne = True And repeatAll = False Then
                repeat = True
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
                repeat = False
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
        'Norm
        Public Shared normbut2hover As Boolean = False
        Public Shared SpeedResetButhover As Boolean = False
        Public Sub normbut2_MouseEnter(sender As Object, e As EventArgs) Handles SpeedResetBut.MouseEnter
            SpeedResetBut.BackgroundImage = ResetHoverImage
            normbut2hover = True
        End Sub
        Public Sub normbut2_MouseLeave(sender As Object, e As EventArgs) Handles SpeedResetBut.MouseLeave
            SpeedResetBut.BackgroundImage = ResetImage
            normbut2hover = False
        End Sub
        Public Sub normbut2_MouseDown(sender As Object, e As MouseEventArgs) Handles SpeedResetBut.MouseDown
            SpeedResetBut.BackgroundImage = ResetPressImage
        End Sub
        Public Sub normbut2_MouseUp(sender As Object, e As MouseEventArgs) Handles SpeedResetBut.MouseUp
            If normbut2hover = True Then
                SpeedResetBut.BackgroundImage = ResetHoverImage
            Else
                SpeedResetBut.BackgroundImage = ResetImage
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



        'Caption Buttons
        'Close
        Public Shared TitleBar_Closehover As Boolean = False
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
        Public Sub TitleBar_CLose_Click() Handles But_TitleBar_Close.Click
            '  Application.ExitThread()
            Me.Close()
        End Sub

        'Max
        Public Shared TitleBarMaxhover As Boolean = False
        Public Sub TitleBarMax2_MouseEnter(sender As Object, e As EventArgs) Handles But_TitleBarMax.MouseEnter
            But_TitleBarMax.BackgroundImage = TitleBarMaxHoverImage
            TitleBarMaxhover = True
            Me.Cursor = Cursors.Default
        End Sub
        Public Sub TitleBarMax2_MouseLeave(sender As Object, e As EventArgs) Handles But_TitleBarMax.MouseLeave
            But_TitleBarMax.BackgroundImage = TitleBarMaxImage
            TitleBarMaxhover = False
        End Sub
        Public Sub TitleBarMax2_MouseDown(sender As Object, e As MouseEventArgs) Handles But_TitleBarMax.MouseDown
            But_TitleBarMax.BackgroundImage = TitleBarMaxPressImage
        End Sub
        Public Sub TitleBarMax2_MouseUp(sender As Object, e As MouseEventArgs) Handles But_TitleBarMax.MouseUp
            If TitleBarMaxhover = True Then
                But_TitleBarMax.BackgroundImage = TitleBarMaxHoverImage
            Else
                But_TitleBarMax.BackgroundImage = TitleBarMaxImage
            End If
            MyBase.Focus()
        End Sub
        Public Sub TitleBarMax_Click() Handles But_TitleBarMax.Click
            If Me.WindowState = FormWindowState.Maximized Then
                Me.WindowState = FormWindowState.Normal
            Else
                If My.Settings.MiniModeOn Then
                    Dim result As Integer = MyFullMsgBox.Show("This will switch to normal mode from mini mode. Continue?", "Turn Mini Mode Off?", True, MyFullMsgBox.CustomButtons.YesNo)
                    'MyFullMsgBox.Show("This will switch to normal mode from mini mode. Continue?", "Turn Mini Mode Off?", MessageBoxButtons.YesNo)

                    If result = DialogResult.No Then

                    ElseIf result = DialogResult.Yes Then
                        MiniModeOff()
                    End If
                Else
                    Me.WindowState = FormWindowState.Maximized
                End If

            End If
        End Sub
        'Min
        Public Shared TitleBarMinhover As Boolean = False
        Public Sub TitleBarMin2_MouseEnter(sender As Object, e As EventArgs) Handles But_TitleBarMin.MouseEnter
            But_TitleBarMin.BackgroundImage = TitleBarMinHoverImage
            TitleBarMinhover = True
            Me.Cursor = Cursors.Default
        End Sub
        Public Sub TitleBarMin2_MouseLeave(sender As Object, e As EventArgs) Handles But_TitleBarMin.MouseLeave
            But_TitleBarMin.BackgroundImage = TitleBarMinImage
            TitleBarMinhover = False
        End Sub
        Public Sub TitleBarMin2_MouseDown(sender As Object, e As MouseEventArgs) Handles But_TitleBarMin.MouseDown
            But_TitleBarMin.BackgroundImage = TitleBarMinPressImage
        End Sub
        Public Sub TitleBarMin2_MouseUp(sender As Object, e As MouseEventArgs) Handles But_TitleBarMin.MouseUp
            If TitleBarMinhover = True Then
                But_TitleBarMin.BackgroundImage = TitleBarMinHoverImage

            Else
                But_TitleBarMin.BackgroundImage = TitleBarMinImage
            End If
            MyBase.Focus()
        End Sub
        Public Sub TitleBarMin_Click() Handles But_TitleBarMin.Click
            Me.WindowState = FormWindowState.Minimized
        End Sub
        'Song Arrange Panel
        'Move Top
        Public Shared MoveTopButhover As Boolean = False
        Public Sub MoveTopBut2_MouseEnter(sender As Object, e As EventArgs) Handles But_MoveTop.MouseEnter
            But_MoveTop.BackgroundImage = My.Resources.New_Design_MoveTop_Hover
            MoveTopButhover = True
        End Sub
        Public Sub MoveTopBut2_MouseLeave(sender As Object, e As EventArgs) Handles But_MoveTop.MouseLeave
            But_MoveTop.BackgroundImage = My.Resources.New_Design_MoveTop
            MoveTopButhover = False
        End Sub
        Public Sub MoveTopBut2_MouseDown(sender As Object, e As MouseEventArgs) Handles But_MoveTop.MouseDown
            But_MoveTop.BackgroundImage = My.Resources.New_Design_MoveTop_Press
        End Sub
        Public Sub MoveTopBut2_MouseUp(sender As Object, e As MouseEventArgs) Handles But_MoveTop.MouseUp
            If MoveTopButhover = True Then
                But_MoveTop.BackgroundImage = My.Resources.New_Design_MoveTop_Hover
            Else
                But_MoveTop.BackgroundImage = My.Resources.New_Design_MoveTop
            End If
            MyBase.Focus()
        End Sub
        'Move Up
        Public Shared UpButhover As Boolean = False
        Public Sub UpBut2_MouseEnter(sender As Object, e As EventArgs) Handles But_MoveUp.MouseEnter
            But_MoveUp.BackgroundImage = My.Resources.New_Design_MoveUp_Hover
            UpButhover = True
        End Sub
        Public Sub UpBut2_MouseLeave(sender As Object, e As EventArgs) Handles But_MoveUp.MouseLeave
            But_MoveUp.BackgroundImage = My.Resources.New_Design_MoveUp
            UpButhover = False
        End Sub
        Public Sub UpBut2_MouseDown(sender As Object, e As MouseEventArgs) Handles But_MoveUp.MouseDown
            But_MoveUp.BackgroundImage = My.Resources.New_Design_MoveUp_Press
        End Sub
        Public Sub UpBut2_MouseUp(sender As Object, e As MouseEventArgs) Handles But_MoveUp.MouseUp
            If UpButhover = True Then
                But_MoveUp.BackgroundImage = My.Resources.New_Design_MoveUp_Hover
            Else
                But_MoveUp.BackgroundImage = My.Resources.New_Design_MoveUp
            End If
            MyBase.Focus()
        End Sub
        'Move Down
        Public Shared DownButhover As Boolean = False
        Public Sub DownBut2_MouseEnter(sender As Object, e As EventArgs) Handles But_MoveDown.MouseEnter
            But_MoveDown.BackgroundImage = My.Resources.New_Design_MoveDown_Hover
            DownButhover = True
        End Sub
        Public Sub DownBut2_MouseLeave(sender As Object, e As EventArgs) Handles But_MoveDown.MouseLeave
            But_MoveDown.BackgroundImage = My.Resources.New_Design_MoveDown
            DownButhover = False
        End Sub
        Public Sub DownBut2_MouseDown(sender As Object, e As MouseEventArgs) Handles But_MoveDown.MouseDown
            But_MoveDown.BackgroundImage = My.Resources.New_Design_MoveDown_PRess
        End Sub
        Public Sub DownBut2_MouseUp(sender As Object, e As MouseEventArgs) Handles But_MoveDown.MouseUp
            If DownButhover = True Then
                But_MoveDown.BackgroundImage = My.Resources.New_Design_MoveDown_Hover
            Else
                But_MoveDown.BackgroundImage = My.Resources.New_Design_MoveDown
            End If
            MyBase.Focus()
        End Sub
        'Move Bottom
        Public Shared MoveBottomButhover As Boolean = False
        Public Sub MoveBottomBut2_MouseEnter(sender As Object, e As EventArgs) Handles But_MoveBottom.MouseEnter
            But_MoveBottom.BackgroundImage = My.Resources.New_Design_MoveBottom_Hover
            MoveBottomButhover = True
        End Sub
        Public Sub MoveBottomBut2_MouseLeave(sender As Object, e As EventArgs) Handles But_MoveBottom.MouseLeave
            But_MoveBottom.BackgroundImage = My.Resources.New_Design_MoveBottom
            MoveBottomButhover = False
        End Sub
        Public Sub MoveBottomBut2_MouseDown(sender As Object, e As MouseEventArgs) Handles But_MoveBottom.MouseDown
            But_MoveBottom.BackgroundImage = My.Resources.New_Design_MoveBottom_Press
        End Sub
        Public Sub MoveBottomBut2_MouseUp(sender As Object, e As MouseEventArgs) Handles But_MoveBottom.MouseUp
            If MoveBottomButhover = True Then
                But_MoveBottom.BackgroundImage = My.Resources.New_Design_MoveBottom_Hover
            Else
                But_MoveBottom.BackgroundImage = My.Resources.New_Design_MoveBottom
            End If
            MyBase.Focus()
        End Sub
        'Save Position
        Public Shared SavePositionBut_SAPhover As Boolean = False
        Public Sub SavePositionBut_SAP2_MouseEnter(sender As Object, e As EventArgs) Handles But_SavePosition_SAP.MouseEnter
            ' But_SavePosition_SAP.BackgroundImageLayout = ImageLayout.Center
            But_SavePosition_SAP.BackgroundImage = My.Resources.SavePosition_Hover 'My.Resources.New_Design_Save_Hover
            SavePositionBut_SAPhover = True
        End Sub
        Public Sub SavePositionBut_SAP2_MouseLeave(sender As Object, e As EventArgs) Handles But_SavePosition_SAP.MouseLeave
            '  But_SavePosition_SAP.BackgroundImageLayout = ImageLayout.Zoom
            But_SavePosition_SAP.BackgroundImage = My.Resources.SavePosition ' My.Resources.New_Design_Save

            SavePositionBut_SAPhover = False
        End Sub
        Public Sub SavePositionBut_SAP2_MouseDown(sender As Object, e As MouseEventArgs) Handles But_SavePosition_SAP.MouseDown
            But_SavePosition_SAP.BackgroundImage = My.Resources.SavePosition_Pressed ' My.Resources.New_Design_Save_Press
        End Sub
        Public Sub SavePositionBut_SAP2_MouseUp(sender As Object, e As MouseEventArgs) Handles But_SavePosition_SAP.MouseUp
            If SavePositionBut_SAPhover = True Then
                But_SavePosition_SAP.BackgroundImage = My.Resources.SavePosition_Hover 'My.Resources.New_Design_Save_Hover
            Else
                '    But_SavePosition_SAP.BackgroundImageLayout = ImageLayout.Zoom
                But_SavePosition_SAP.BackgroundImage = My.Resources.SavePosition ' My.Resources.New_Design_Save
            End If
            MyBase.Focus()
        End Sub
        'Remove Position
        Public Shared RemovePositionBut_SAPhover As Boolean = False
        Public Sub RemovePositionBut_SAP2_MouseEnter(sender As Object, e As EventArgs) Handles But_RemovePosition_SAP.MouseEnter
            'But_RemovePosition_SAP.BackgroundImageLayout = ImageLayout.Center
            But_RemovePosition_SAP.BackgroundImage = My.Resources.RemovePosition_Hover ' My.Resources.New_Design_Remove_Hover
            RemovePositionBut_SAPhover = True
        End Sub
        Public Sub RemovePositionBut_SAP2_MouseLeave(sender As Object, e As EventArgs) Handles But_RemovePosition_SAP.MouseLeave
            ' But_RemovePosition_SAP.BackgroundImageLayout = ImageLayout.Zoom
            But_RemovePosition_SAP.BackgroundImage = My.Resources.RemovePosition ' My.Resources.New_Design_Remove
            RemovePositionBut_SAPhover = False
        End Sub
        Public Sub RemovePositionBut_SAP2_MouseDown(sender As Object, e As MouseEventArgs) Handles But_RemovePosition_SAP.MouseDown
            But_RemovePosition_SAP.BackgroundImage = My.Resources.RemovePosition_Pressed ' My.Resources.New_Design_Remove_Press
        End Sub
        Public Sub RemovePositionBut_SAP2_MouseUp(sender As Object, e As MouseEventArgs) Handles But_RemovePosition_SAP.MouseUp
            If RemovePositionBut_SAPhover = True Then
                But_RemovePosition_SAP.BackgroundImage = My.Resources.RemovePosition_Hover ' My.Resources.New_Design_Remove_Hover
            Else
                '   But_RemovePosition_SAP.BackgroundImageLayout = ImageLayout.Zoom
                But_RemovePosition_SAP.BackgroundImage = My.Resources.RemovePosition ' My.Resources.New_Design_Remove
            End If
            MyBase.Focus()
        End Sub
        'Addbut
        Public Shared AddButhover As Boolean = False
        Public Sub AddBut2_MouseEnter(sender As Object, e As EventArgs) Handles But_Add.MouseEnter
            But_Add.BackgroundImage = My.Resources.New_Design_Add_Hover
            AddButhover = True
        End Sub
        Public Sub AddBut2_MouseLeave(sender As Object, e As EventArgs) Handles But_Add.MouseLeave
            But_Add.BackgroundImage = My.Resources.New_Design_Add
            AddButhover = False
        End Sub
        Public Sub AddBut2_MouseDown(sender As Object, e As MouseEventArgs) Handles But_Add.MouseDown
            But_Add.BackgroundImage = My.Resources.New_Design_Add_Press
        End Sub
        Public Sub AddBut2_MouseUp(sender As Object, e As MouseEventArgs) Handles But_Add.MouseUp
            If AddButhover = True Then
                But_Add.BackgroundImage = My.Resources.New_Design_Add_Hover
            Else
                But_Add.BackgroundImage = My.Resources.New_Design_Add
            End If
            MyBase.Focus()
        End Sub
        'MinusBut
        Public Shared MinusButhover As Boolean = False
        Public Sub MinusBut2_MouseEnter(sender As Object, e As EventArgs) Handles But_Minus.MouseEnter
            But_Minus.BackgroundImage = My.Resources.New_Design_Minus_Hover
            MinusButhover = True
        End Sub
        Public Sub MinusBut2_MouseLeave(sender As Object, e As EventArgs) Handles But_Minus.MouseLeave
            But_Minus.BackgroundImage = My.Resources.New_Design_Minus
            MinusButhover = False
        End Sub
        Public Sub MinusBut2_MouseDown(sender As Object, e As MouseEventArgs) Handles But_Minus.MouseDown
            But_Minus.BackgroundImage = My.Resources.New_Design_Minus_Press
        End Sub
        Public Sub MinusBut2_MouseUp(sender As Object, e As MouseEventArgs) Handles But_Minus.MouseUp
            If MinusButhover = True Then
                But_Minus.BackgroundImage = My.Resources.New_Design_Minus_Hover
            Else
                But_Minus.BackgroundImage = My.Resources.New_Design_Minus
            End If
            MyBase.Focus()
        End Sub
        'FavBut
        Public Shared FavButhover As Boolean = False
        Public Sub FavBut2_MouseEnter(sender As Object, e As EventArgs) Handles But_Fav.MouseEnter
            But_Fav.BackgroundImage = My.Resources.Favorite_Hover
            FavButhover = True
        End Sub
        Public Sub FavBut2_MouseLeave(sender As Object, e As EventArgs) Handles But_Fav.MouseLeave
            But_Fav.BackgroundImage = My.Resources.Favorite
            FavButhover = False
        End Sub
        Public Sub FavBut2_MouseDown(sender As Object, e As MouseEventArgs) Handles But_Fav.MouseDown
            But_Fav.BackgroundImage = My.Resources.Favorite_Press
        End Sub
        Public Sub FavBut2_MouseUp(sender As Object, e As MouseEventArgs) Handles But_Fav.MouseUp
            If FavButhover = True Then
                But_Fav.BackgroundImage = My.Resources.Favorite_Hover
            Else
                But_Fav.BackgroundImage = My.Resources.Favorite
            End If
            '  MyBase.Focus()
        End Sub





#End Region
        ' Use Shadows or not
        Public Sub BarCheckbox_UseShadows_CheckedChanged(sender As Object, e As ItemClickEventArgs) Handles BarCheckbox_UseShadows.CheckedChanged
            Refresh_ButtonGraphics()
        End Sub
        Public Sub Refresh_ButtonGraphics()
            If BarCheckbox_UseShadows.Checked Then
                PlayImage = My.Resources.Play_3
                PlayHoverImage = My.Resources.Play_3_Hover
                PlayPressImage = My.Resources.Play_3_Press

                PauseImage = My.Resources.Pause_3
                PauseHoverImage = My.Resources.Pause_3_Hover
                PausePressImage = My.Resources.Pause_3_Press

                StopImage = My.Resources.Stop_3
                StopHoverImage = My.Resources.Stop_3_Hover
                StopPressImage = My.Resources.Stop_3_Press

                NextImage = My.Resources.Next_3
                NextHoverImage = My.Resources.Next_3_Hover
                NextPressImage = My.Resources.Next_3_Press

                PreviousImage = My.Resources.Previous_3
                PreviousHoverImage = My.Resources.Previous_3_Hover
                PreviousPressImage = My.Resources.Previous_3_Press

                SlowImage = My.Resources.Prev_3
                SlowHoverImage = My.Resources.Prev_3_Hover
                SlowPressImage = My.Resources.Prev_3_Press

                FastImage = My.Resources.Next_4
                FastHoverImage = My.Resources.Next_4_Hover
                FastPressImage = My.Resources.Next_4_Press

                AImage = My.Resources.New_Design_A
                AHoverImage = My.Resources.New_Design_A_Hover
                APressImage = My.Resources.New_Design_A_Press

                BImage = My.Resources.New_Design_B
                BHoverImage = My.Resources.New_Design_B_Hover
                BPressImage = My.Resources.New_Design_B_Press

                ResetImage = My.Resources.New_Design_reset
                ResetHoverImage = My.Resources.New_Design_reset_Hover
                ResetPressImage = My.Resources.New_Design_reset_Press

                RepeatAllImage = My.Resources.New_Design_RepeatAll_Active
                RepeatAllHoverImage = My.Resources.New_Design_RepeatAll_Hover
                RepeatAllPressImage = My.Resources.New_Design_RepeatAll_Press

                RepeatOneImage = My.Resources.New_Design_RepeatOne_Active
                RepeatOneHoverImage = My.Resources.New_Design_RepeatOne_Hover
                RepeatOnePressImage = My.Resources.New_Design_RepeatOne_Press

                RepeatOffImage = My.Resources.New_Design_RepeatOff
                RepeatOffHoverImage = My.Resources.New_Design_RepeatOff_Hover
                RepeatOffPressImage = My.Resources.New_Design_RepeatOff_Press

                ShuffleImage = My.Resources.New_Design_Shuffle_Active
                ShuffleDisabledImage = My.Resources.New_Design_Shuffle
                ShuffleHoverImage = My.Resources.New_Design_Shuffle_Hover
                ShufflePressImage = My.Resources.New_Design_Shuffle_Press

            Else
                PlayImage = My.Resources.Play_4
                PlayHoverImage = My.Resources.Play_3_Hover
                PlayPressImage = My.Resources.Play_4_Press

                PauseImage = My.Resources.Pause_4
                PauseHoverImage = My.Resources.Pause_3_Hover
                PausePressImage = My.Resources.Pause_4_Press

                StopImage = My.Resources.Stop_4
                StopHoverImage = My.Resources.Stop_3_Hover
                StopPressImage = My.Resources.Stop_4_Press

                NextImage = My.Resources.Next_5
                NextHoverImage = My.Resources.Next_3_Hover
                NextPressImage = My.Resources.Next_5_Press

                PreviousImage = My.Resources.Previous_4
                PreviousHoverImage = My.Resources.Previous_3_Hover
                PreviousPressImage = My.Resources.Previous_4_Press

                SlowImage = My.Resources.Prev_4
                SlowHoverImage = My.Resources.Prev_4_Hover
                SlowPressImage = My.Resources.Prev_4_Press

                FastImage = My.Resources.Next_6
                FastHoverImage = My.Resources.Next_6_Hover
                FastPressImage = My.Resources.Next_6_Press

                AImage = My.Resources.New_Design_A_2
                AHoverImage = My.Resources.New_Design_A_Hover
                APressImage = My.Resources.New_Design_A_Press_2

                BImage = My.Resources.New_Design_B_2
                BHoverImage = My.Resources.New_Design_B_Hover
                BPressImage = My.Resources.New_Design_B_Press_2

                ResetImage = My.Resources.New_Design_reset_2
                ResetHoverImage = My.Resources.New_Design_reset_Hover
                ResetPressImage = My.Resources.New_Design_reset_Press_2

                RepeatAllImage = My.Resources.New_Design_RepeatAll_Active_2
                RepeatAllHoverImage = My.Resources.New_Design_RepeatAll_Hover_2
                RepeatAllPressImage = My.Resources.New_Design_RepeatAll_Press_2

                RepeatOneImage = My.Resources.New_Design_RepeatOne_Active_2
                RepeatOneHoverImage = My.Resources.New_Design_RepeatOne_Hover_2
                RepeatOnePressImage = My.Resources.New_Design_RepeatOne_Press_2

                RepeatOffImage = My.Resources.New_Design_RepeatOff_2
                RepeatOffHoverImage = My.Resources.New_Design_RepeatOff_Hover_2
                RepeatOffPressImage = My.Resources.New_Design_RepeatOff_Press_2

                ShuffleImage = My.Resources.New_Design_Shuffle_Active_2
                ShuffleDisabledImage = My.Resources.New_Design_Shuffle_2
                ShuffleHoverImage = My.Resources.New_Design_Shuffle_Hover_2
                ShufflePressImage = My.Resources.New_Design_Shuffle_Press_2

            End If
            My.Settings.UseIconShadows = BarCheckbox_UseShadows.Checked

            If IsVideo = False Then
                If AudioPlayer.Instance.GetStreamStatus = BASSActive.BASS_ACTIVE_PLAYING Then
                    But_PlayPause.BackgroundImage = PauseImage
                Else
                    But_PlayPause.BackgroundImage = PlayImage
                End If
            Else
                If VLC_installed Then
                    If VlcPlayer.playlist.isPlaying = True Then
                        But_PlayPause.BackgroundImage = PauseImage
                    ElseIf VlcPlayer.playlist.isPlaying = False Then ' If VlcPlayer.playlist.isPlaying = False Then

                        But_PlayPause.BackgroundImage = PlayImage
                    End If
                End If

            End If
            But_Stop.BackgroundImage = StopImage
            But_Previous.BackgroundImage = PreviousImage
            But_Next.BackgroundImage = NextImage
            But_SpeedDown.BackgroundImage = SlowImage
            But_SpeedUp.BackgroundImage = FastImage
            But_PitchDown.BackgroundImage = SlowImage
            But_PitchUp.BackgroundImage = FastImage
            But_A.BackgroundImage = AImage
            But_B.BackgroundImage = BImage
            But_AB_Reset.BackgroundImage = ResetImage


            If ShuffleButdisabled = False Then
                If IsShuffle Then
                    But_Shuffle.BackgroundImage = ShuffleImage
                Else
                    But_Shuffle.BackgroundImage = ShuffleDisabledImage
                End If
            End If

            If repeatAll Then
                But_Repeat.BackgroundImage = RepeatAllImage
            ElseIf repeatOne Then
                But_Repeat.BackgroundImage = RepeatOneImage
            ElseIf repeat = False Then
                But_Repeat.BackgroundImage = RepeatOffImage
            End If

        End Sub

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
                ResizeBut2.BringToFront()
            Catch

            End Try

        End Sub
        Public Sub SongName_MouseLeave(sender As Object, e As EventArgs) Handles Label_SongName.MouseLeave
            Try
                If My.Settings.MiniModeOn = False Then Return
                Label_SongName.AutoSize = False
                Label_SongName.Parent.Controls.SetChildIndex(Label_SongName, SongnameZOrder)
                ResizeBut2.BringToFront()
            Catch

            End Try

        End Sub


        'Artist
        Dim ArtistZOrder As Integer
        Public Sub Artist_MouseEnter(sender As Object, e As EventArgs) Handles Label_Artist.MouseEnter
            Try
                If My.Settings.MiniModeOn = False Then Return
                Label_Artist.Parent.Controls.GetChildIndex(Label_Artist, ArtistZOrder)
                Label_Artist.AutoSize = True
                ResizeBut2.BringToFront()
            Catch

            End Try

        End Sub
        Public Sub Artist_MouseLeave(sender As Object, e As EventArgs) Handles Label_Artist.MouseLeave
            Try
                If My.Settings.MiniModeOn = False Then Return
                Label_Artist.AutoSize = False
                Label_Artist.Parent.Controls.SetChildIndex(Label_Artist, ArtistZOrder)
                ResizeBut2.BringToFront()
            Catch

            End Try

        End Sub


        'Album
        Dim AlbumZOrder As Integer
        Public Sub Album_MouseEnter(sender As Object, e As EventArgs) Handles Label_Album.MouseEnter
            Try
                If My.Settings.MiniModeOn = False Then Return
                Label_Album.Parent.Controls.GetChildIndex(Label_Album, AlbumZOrder)
                Label_Album.AutoSize = True
                ResizeBut2.BringToFront()
            Catch

            End Try

        End Sub
        Public Sub Album_MouseLeave(sender As Object, e As EventArgs) Handles Label_Album.MouseLeave
            Try
                If My.Settings.MiniModeOn = False Then Return
                Label_Album.AutoSize = False
                Label_Album.Parent.Controls.SetChildIndex(Label_Album, AlbumZOrder)
                ResizeBut2.BringToFront()
            Catch

            End Try

        End Sub

#End Region


#End Region

#Region " Customize Trackbars"

        Public Sub BarBut_SeekBar_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_SeekBar.ItemClick
            CustomizeSliders()

        End Sub
        Public Sub CustomizeSliders()
            Dim SeekbarLoadedSliderShape As Integer = My.Settings.seekbarShape
            Dim SeekbarSliderShape As RichTrackBar.SliderShapes
            Dim _SliderFillSeek As Boolean = My.Settings.SlidersFilledSeek
            Dim _BorderThicknessSeek As Integer = My.Settings.BorderThicknessSeek

            Dim Seekbar_Color_BarLeft As Color = My.Settings.seekbarBarLeft
            Dim Seekbar_Color_BarLeftInactive As Color = My.Settings.seekbarBarLeftInactive
            Dim Seekbar_Color_BarRight As Color = My.Settings.seekbarBarRight
            Dim Seekbar_Color_Border As Color = My.Settings.seekbarBorder
            Dim Seekbar_Color_Slider As Color = My.Settings.seekbarSlider
            Dim Seekbar_Text_Color As Color = My.Settings.seekbarTextColor
            Dim Seekbar_ShowText As RichTrackBar.ShowText

            Dim speedbarLoadedSliderShape As Integer = My.Settings.speedbarShape
            Dim speedbarSliderShape As RichTrackBar.SliderShapes
            Dim _SliderFillSpeed As Boolean = My.Settings.SlidersFilledSpeed
            Dim _BorderThicknessSpeed As Integer = My.Settings.BorderThicknessSpeed

            Dim speedbar_Color_BarLeft As Color = My.Settings.speedbarBarLeft
            Dim speedbar_Color_BarLeftInactive As Color = My.Settings.speedbarBarLeftInactive
            Dim speedbar_Color_BarRight As Color = My.Settings.speedbarBarRight
            Dim speedbar_Color_Border As Color = My.Settings.speedbarBorder
            Dim speedbar_Color_Slider As Color = My.Settings.speedbarSlider
            Dim speedbar_Text_Color As Color = My.Settings.speedbarTextColor
            Dim speedbar_ShowText As RichTrackBar.ShowText

            Dim pitchbarLoadedSliderShape As Integer = My.Settings.pitchbarShape
            Dim pitchbarSliderShape As RichTrackBar.SliderShapes
            Dim _SliderFillPitch As Boolean = My.Settings.SlidersFilledPitch
            Dim _BorderThicknessPitch As Integer = My.Settings.BorderThicknessPitch

            Dim pitchbar_Color_BarLeft As Color = My.Settings.pitchbarBarLeft
            Dim pitchbar_Color_BarLeftInactive As Color = My.Settings.pitchbarBarLeftInactive
            Dim pitchbar_Color_BarRight As Color = My.Settings.pitchbarBarRight
            Dim pitchbar_Color_Border As Color = My.Settings.pitchbarBorder
            Dim pitchbar_Color_Slider As Color = My.Settings.pitchbarSlider
            Dim pitchbar_Text_Color As Color = My.Settings.pitchbarTextColor
            Dim pitchbar_ShowText As RichTrackBar.ShowText

            Dim volumebarLoadedSliderShape As Integer = My.Settings.volumebarShape
            Dim volumebarSliderShape As RichTrackBar.SliderShapes
            Dim _SliderFillVolume As Boolean = My.Settings.SlidersFilledVolume
            Dim _BorderThicknessVolume As Integer = My.Settings.BorderThicknessVolume

            Dim volumebar_Color_BarLeft As Color = My.Settings.volumebarBarLeft
            Dim volumebar_Color_BarLeftInactive As Color = My.Settings.volumebarBarLeftInactive
            Dim volumebar_Color_BarRight As Color = My.Settings.volumebarBarRight
            Dim volumebar_Color_Border As Color = My.Settings.volumebarBorder
            Dim volumebar_Color_Slider As Color = My.Settings.volumebarSlider
            Dim volumebar_Text_Color As Color = My.Settings.volumebarTextColor
            Dim volumebar_ShowText As RichTrackBar.ShowText



            Sliders.ShowDialog()
            If Sliders.DialogResult = DialogResult.OK Then
                Sliders.Close()
                If My.Settings.DriveMode Then
                    xcarform.SkinTrackbars()
                Else
                    SetupTrackBars()
                End If


            ElseIf Sliders.DialogResult = DialogResult.Cancel Then
                Sliders.Close()
                My.Settings.seekbarShape = SeekbarLoadedSliderShape
                My.Settings.seekbarBarLeft = Seekbar_Color_BarLeft
                My.Settings.seekbarBarLeftInactive = Seekbar_Color_BarLeftInactive
                My.Settings.seekbarBarRight = Seekbar_Color_BarRight
                My.Settings.seekbarBorder = Seekbar_Color_Border
                My.Settings.seekbarSlider = Seekbar_Color_Slider
                My.Settings.BorderThicknessSeek = _BorderThicknessSeek

                My.Settings.volumebarShape = volumebarLoadedSliderShape
                My.Settings.volumebarBarLeft = volumebar_Color_BarLeft
                My.Settings.volumebarBarLeftInactive = volumebar_Color_BarLeftInactive
                My.Settings.volumebarBarRight = volumebar_Color_BarRight
                My.Settings.volumebarBorder = volumebar_Color_Border
                My.Settings.volumebarSlider = volumebar_Color_Slider
                My.Settings.BorderThicknessVolume = _BorderThicknessVolume

                My.Settings.speedbarShape = speedbarLoadedSliderShape
                My.Settings.speedbarBarLeft = speedbar_Color_BarLeft
                My.Settings.speedbarBarLeftInactive = speedbar_Color_BarLeftInactive
                My.Settings.speedbarBarRight = speedbar_Color_BarRight
                My.Settings.speedbarBorder = speedbar_Color_Border
                My.Settings.speedbarSlider = speedbar_Color_Slider
                My.Settings.BorderThicknessSpeed = _BorderThicknessSpeed

                My.Settings.pitchbarShape = pitchbarLoadedSliderShape
                My.Settings.pitchbarBarLeft = pitchbar_Color_BarLeft
                My.Settings.pitchbarBarLeftInactive = pitchbar_Color_BarLeftInactive
                My.Settings.pitchbarBarRight = pitchbar_Color_BarRight
                My.Settings.pitchbarBorder = pitchbar_Color_Border
                My.Settings.pitchbarSlider = pitchbar_Color_Slider
                My.Settings.BorderThicknessPitch = _BorderThicknessPitch

                My.Settings.seekbarText = Seekbar_ShowText
                My.Settings.seekbarTextColor = Seekbar_Text_Color
                My.Settings.volumebarText = volumebar_ShowText
                My.Settings.volumebarTextColor = volumebar_Text_Color
                My.Settings.speedbarText = speedbar_ShowText
                My.Settings.speedbarTextColor = speedbar_Text_Color
                My.Settings.pitchbarText = pitchbar_ShowText
                My.Settings.pitchbarTextColor = pitchbar_Text_Color

            End If
        End Sub

        Public Sub SetupTrackBars()
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


#Region " Settings"

        Public Shared Sub BackupSettings()
            Dim SettingsFile As String = _
             System.Configuration.ConfigurationManager.OpenExeConfiguration( _
                 System.Configuration.ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath()

            Dim sDialog As New SaveFileDialog()
            sDialog.DefaultExt = ".config"
            sDialog.Filter = "Rich Player Settings|*.config"
            sDialog.Title = "Backup Rich Player Settings"

            If sDialog.ShowDialog() = DialogResult.OK Then
                My.Settings.Save()
                IO.File.Copy(SettingsFile, sDialog.FileName, True)
                My.Settings.Save()

                MyMsgBox.Show("Please make separate backups for your playlists! These will not be backed up.", "Backup Your Playlists!", True)


            End If

        End Sub
        Public Shared Sub RestoreSettings()
            Dim SettingsFile As String = _
        System.Configuration.ConfigurationManager.OpenExeConfiguration( _
            System.Configuration.ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath()

            Dim sDialog As New OpenFileDialog()
            sDialog.DefaultExt = ".config"
            sDialog.Filter = "Rich Player Settings|*.config"
            sDialog.Title = "Restore Rich Player Settings"

            If sDialog.ShowDialog() = DialogResult.OK Then
                Dim result As Integer = MyFullMsgBox.Show("This will close the app and the settings will be restored upon manual restart.", _
                                                                      "Are you sure you want to restore settings?", True, MyFullMsgBox.CustomButtons.YesNo)
                If result = DialogResult.Yes Then
                    IO.File.Copy(sDialog.FileName, SettingsFile, True)
                    Process.GetCurrentProcess.Kill()
                End If

            End If

        End Sub




        Public Sub BarBut_BackupSettings_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_BackupSettings.ItemClick
            MyMsgBox.Show("This is a beta feature. Use at your own risk! (This is currently unused. New settings have not be added to this feature.)", "", True)
            Dim sDialog As New SaveFileDialog()
            sDialog.DefaultExt = ".rpSettings"
            sDialog.Filter = "Rich Player Settings (*.rpSettings)|*.rpSettings"

            If sDialog.ShowDialog() = DialogResult.OK Then
                Dim use As Boolean = False
                If use Then
                    Dim exclusions() As String = {"PlayWhenTabActive", "LastPlayedSongs", "PlaylistNames", "LastPlayedPositions", "chk4"}
                    Using sWriter As New StreamWriter(sDialog.FileName)
                        For Each setting As System.Configuration.SettingsPropertyValue In My.Settings.PropertyValues
                            If setting.Property.GetType() Is GetType(Point) Then
                                MyMsgBox.Show(setting.Name, "", True)
                            End If
                            Try
                                If Not exclusions.Any(Function(s) s = setting.Name) Then
                                    sWriter.WriteLine(setting.Name & ":" & setting.SerializedValue.ToString)
                                End If
                            Catch ex As Exception
                                MyMsgBox.Show("Unable to backup setting for: " & setting.Name, "", True)
                            End Try
                        Next
                    End Using
                End If




                My.Settings.Save()
                fBackupSettings(sDialog.FileName)
                If MyFullMsgBox.Show("Backup playlists as well?" & Environment.NewLine & Environment.NewLine & _
                                   "You will be asked to save it to a folder. It is recommended that you backup the playlists to a folder by itself.", "Playlists?", _
                                  True, MessageBoxButtons.YesNo) = DialogResult.Yes Then
                    Dim FolderDialog As New FolderBrowserDialog
                    If FolderDialog.ShowDialog = DialogResult.OK Then


                        For num As Integer = 0 To My.Settings.PlaylistsCount - 1
                            Dim Playlist As GridPlaylist : Dim Scroller As Scroller : For Each c As Control In PlaylistTabs.TabPages(num).Controls : If c.GetType Is GetType(Scroller) Then : Scroller = c : For Each c2 As Control In Scroller.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist = c2 : End If : Next : End If : Next
                            If num = 0 Then : If Playlist.RowCount <> 0 Then : My.Settings.Playlist0HasFiles = True : My.Settings.FirstTimeSetup = False
                                Else : My.Settings.Playlist0HasFiles = False : End If : End If

                            Try : Dim Row As Integer = Playlist.CurrentCell.RowIndex
                                Dim RowCount As Integer = Playlist.RowCount : Dim SongFilename As String = Playlist.Item(6, Row).Value.ToString
                                If RowCount > 0 Then
                                    SaveGridData(Playlist, FolderDialog.SelectedPath & "\Temp_Playlist" & num & ".rpl")
                                    My.Settings.LastPlayedSongs.Insert(num, Row)
                                End If
                            Catch : ClosingErrors = ClosingErrors + "Unable to save playlist(s) items! Possibly no items in playlists?" + Environment.NewLine : End Try
                        Next

                    End If

                End If


                MyMsgBox.Show("Settings backed up", "", True)

            End If



        End Sub

        Public PlaylistsImported As Boolean = False
        Public Sub BarBut_ImportSettings_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_ImportSettings.ItemClick
            Dim oDialog As New OpenFileDialog
            oDialog.Filter = "Rich Player Settings (*.rpSettings)|*.rpSettings"

            If oDialog.ShowDialog() = DialogResult.OK Then
                fRestoreSettings(oDialog.FileName)
                If fRestoreSettings(oDialog.FileName) Then
                    For i As Integer = 1 To PlaylistTabs.TabPages.Count - 1
                        PlaylistTabs.TabPages.RemoveAt(i)
                    Next

                    My.Settings.Reload()
                    My.Settings.Save()
                    ForceClose = True
                    ' Application.Exit()
                    ' Application.ExitThread()
                    ' Process.Start(Application.StartupPath & "\Rich Player.exe")
                    ' Return



                    If MyFullMsgBox.Show("Load playlists from backup?", "Load Playlists?", True, MessageBoxButtons.YesNo) = DialogResult.Yes Then
                        Dim FolderDialog As New FolderBrowserDialog
                        If FolderDialog.ShowDialog = DialogResult.OK Then
                            Dim Playlist0 As GridPlaylist : Dim Scroller0 As Scroller : For Each c As Control In PlaylistTabs.TabPages(0).Controls : If c.GetType Is GetType(Scroller) Then : Scroller0 = c : For Each c2 As Control In Scroller0.Controls : If c2.GetType Is GetType(GridPlaylist) Then : Playlist0 = c2 : End If : Next : End If : Next
                            Scroller0.Dispose()

                            Dim Number As Integer = My.Settings.PlaylistsCount
                            If My.Settings.PlaylistsCount = 0 Then Number = 1
                            For num As Integer = 0 To Number - 1
                                If num <> 0 Then
                                    PlaylistTabs.TabPages.Add(My.Settings.PlaylistNames.Item(num).ToString)
                                Else
                                    If My.Settings.PlaylistNames.Count <> 0 Then
                                        If My.Settings.PlaylistNames.Item(num).ToString <> "" Then
                                            PlaylistTabs.TabPages(num).Text = ((My.Settings.PlaylistNames.Item(num).ToString))
                                        Else
                                            PlaylistTabs.TabPages(num).Text = "Default Playlist"
                                        End If
                                    End If
                                End If

                                PlaylistTabs.TabPages(num).AllowTouchScroll = True

                                If My.Settings.PlaylistNames.Item(num).ToString <> "Spotify" Then
                                    Dim Scroller As New Scroller
                                    Dim Playlist As New GridPlaylist
                                    Scroller.Controls.Add(Playlist)
                                    Playlist.CreateControl()
                                    SongArrangePanel.CreateControl()
                                    PlaylistTabs.TabPages(num).Controls.Add(Scroller)

                                    AddHandler Playlist.CellMouseDoubleClick, AddressOf DoubleClickPlay
                                    AddHandler Playlist.SelectionChanged, AddressOf GridPlaylist_SelectionChanged
                                    AddHandler Playlist.CellMouseUp, AddressOf GridPlaylist_CellMouseDown
                                    Utilities.ElevatedDragDropManager.Instance.EnableDragDrop(Playlist.Handle)

                                    '    'Playlist.AllowDrop = False
                                    Dim row As Integer
                                    Dim RowCount As Integer = Playlist.Rows.Count

                                    Scroller.BringToFront()

                                    Dim playlistFile As String = FolderDialog.SelectedPath & "\Temp_Playlist" & num & ".rpl"
                                    If IO.File.Exists(playlistFile) Then
                                        OpenFile.FileName = playlistFile
                                        LoadGridData(Playlist, playlistFile)
                                    End If
                                    Timer_Meta_and_Artwork.Stop()
                                    If Playlist.Rows.Count = 0 Then Return
                                    'Start Fresh
                                    If Playlist.RowCount <> 0 Then
                                        Playlist.Rows(0).Selected = True
                                        row = 0
                                        Playlist.CurrentCell = Playlist(0, row)
                                    End If
                                    Try
                                        row = My.Settings.LastPlayedSongs.Item(num)
                                    Catch
                                        row = 0
                                    End Try
                                    If Playlist.RowCount <> 0 Then
                                        Playlist.Rows(row).Selected = True
                                        Playlist.CurrentCell = Playlist.Rows(row).Cells(0)
                                    End If
                                    Scroller.BringToFront()
                                    Scroller.Refresh()
                                    Scroller.FireScrollEventOnMouseWheel = True
                                End If

                            Next
                            PlaylistsImported = True

                        End If


                    End If


                    LoadApp()

                    PlaylistsImported = False
                    MyMsgBox.Show("Settings Imported", "Success!", True)
                    NeedResizeRefresh = True
                    RefreshApp()
                    Dim Tabpage As Integer = PlaylistTabs.SelectedTabPageIndex
                    PlaylistTabs.TabPages(Tabpage).Controls.Add(SongArrangePanel)
                    PlaylistTabs.TabPages(Tabpage).Controls.Add(OpenProgressBar)
                    SongArrangePanel.BringToFront()
                    ' PlaylistTabs.TabPages(Tabpage).Controls.Add(TextBox_PlaylistSearch)
                    PlaylistTabs.TabPages(Tabpage).Controls.Add(SearchBox_Panel)
                    ' TextBox_PlaylistSearch.Dock = DockStyle.Top
                    ' TextBox_PlaylistSearch.Controls.Add(But_Search)
                    ' But_Search.BringToFront()
                    ' But_Search.Top = 1


                End If

            End If
        End Sub


        Public Sub BarBut_ResetSettings_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarBut_ResetSettings.ItemClick
            ResetSettings()
        End Sub
        Public Sub ResetSettings()

            Dim result As Integer = MyFullMsgBox.Show("Are you sure you want to reset settings? This will reset app to the state it was originally. This cannot be undone!", _
                                                      "Reset Settings?", True, MyFullMsgBox.CustomButtons.YesNo)
            If result = DialogResult.Yes Then
                My.Settings.Reset()
                My.Settings.Reload()
                For i As Integer = 1 To PlaylistTabs.TabPages.Count - 1
                    PlaylistTabs.TabPages.RemoveAt(i)
                Next
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
                Playlist.Rows.Clear()
                My.Settings.Reload()
                My.Settings.Save()
                ForceClose = True
                Application.Exit()
                Application.ExitThread()
                Process.Start(Application.StartupPath & "\Rich Player.exe")
            End If
        End Sub








        Sub FrmValidatorFormClosed(sender As Object, e As FormClosedEventArgs)
            Return
            fBackupSettings(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "iTSfv.xml")

        End Sub

        Private Function fBackupSettings(filePath As String) As Boolean

            ' filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "iTSfv.xml"

            My.Settings.Save()

            Dim config As System.Configuration.Configuration = _
                System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.PerUserRoamingAndLocal)

            config.SaveAs(filePath)


        End Function

        Private Function fRestoreSettings(filePath As String) As Boolean

            ' filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "iTSfv.xml"

            Dim success As Boolean = False

            Dim config As System.Configuration.Configuration = _
                 System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.PerUserRoamingAndLocal)

            Dim use As Boolean = True
            If use Then
                Using sr As New StreamReader(filePath)
                    Dim r As String = sr.ReadToEnd
                    Using sw As New StreamWriter(config.FilePath)
                        sw.WriteLine(r)
                        success = True
                    End Using
                End Using
            Else
                IO.File.Copy(filePath, config.FilePath)
                success = True
            End If



            Return success
        End Function



#End Region

#Region " Report Problem    |   Send Email"

        Public Sub ReportProblem()
            Try
                Dim xform As New ReportProblemForm
                xform.ShowDialog()

                If xform.DialogResult = DialogResult.OK Then
                    Dim Smtp_Server As New SmtpClient
                    Dim e_mail As New MailMessage()
                    Smtp_Server.UseDefaultCredentials = False
                    Smtp_Server.Credentials = New System.Net.NetworkCredential("rexfordrich@gmail.com", "Superman_911")
                    Smtp_Server.Port = 587
                    Smtp_Server.EnableSsl = True
                    Smtp_Server.Host = "smtp.gmail.com"

                    e_mail = New MailMessage()
                    e_mail.From = New MailAddress("rexfordrich@gmail.com")
                    e_mail.To.Add("rexfordrich@gmail.com")
                    e_mail.Subject = "Rich Player Problem Reported"
                    e_mail.IsBodyHtml = False
                    e_mail.Body = xform.txtMessage.Text
                    Smtp_Server.Send(e_mail)

                    MyMsgBox.Show("Message Sent", "", True)
                End If
            Catch error_t As Exception
                MyMsgBox.Show(error_t.ToString, "", True)
            End Try
        End Sub



#End Region

#Region " Click on Update Available! label to update"

        Public Sub Label_Update_Click(sender As Object, e As EventArgs) Handles Label_Update.Click
            UpdateDialog.ShowDialog()
            ' Dialog1.ShowDialog()
        End Sub



#End Region

        Private Function DrawText(ByVal text As String, ByVal font As Font, ByVal textColor As Color, ByVal backColor As Color) As System.Drawing.Image
            Dim img As System.Drawing.Image = New Bitmap(1, 1)
            Dim drawing As Graphics = Graphics.FromImage(img)
            Dim textSize As SizeF = drawing.MeasureString(text, font)
            img.Dispose()
            drawing.Dispose()
            img = New Bitmap(CInt(textSize.Width), CInt(textSize.Height))
            drawing = Graphics.FromImage(img)
            drawing.Clear(backColor)
            Dim textBrush As Brush = New SolidBrush(textColor)
            drawing.DrawString(text, font, textBrush, 0, 0)
            drawing.Save()
            textBrush.Dispose()
            drawing.Dispose()
            Return img
        End Function

        Public Function HTML_String(s As String) As String
            Dim str As String = s.Replace("+", "%2B").Replace(" ", "+").Replace("&", "%26").Replace("!", "%21").Replace("""", "%22").Replace("#", "%23").Replace("$", "%24") _
                .Replace("%", "%25").Replace("'", "%27").Replace("(", "%28").Replace(")", "%29").Replace("*", "%2A") _
                .Replace(",", "%2C").Replace("-", "%2D").Replace(".", "%2E").Replace("/", "%2F").Replace(":", "%3A").Replace(";", "3B").Replace("<", "%3C") _
                .Replace("=", "%3D").Replace(">", "%3E").Replace("?", "%3F").Replace("@", "%40").Replace("[", "%5B").Replace("\", "%5C").Replace("]", "%5D") _
                .Replace("^", "%5E").Replace("_", "%5F").Replace("`", "%60").Replace("{", "%7B").Replace("|", "%7C").Replace("}", "%7D").Replace("~", "%7E")

            Return str
        End Function


	End Class


#Region " Drag Tabs Class"

	Public Class TabDragEventArgs
        Private m_tabPage As DevExpress.XtraTab.XtraTabPage

        Public Property TabPage() As DevExpress.XtraTab.XtraTabPage
            Get
                Return m_tabPage
            End Get
            Set(value As DevExpress.XtraTab.XtraTabPage)
                m_tabPage = value
            End Set
        End Property
        Private m_newIndex As Integer = -1

        Public Property NewIndex() As Integer
            Get
                Return m_newIndex
            End Get
            Set(value As Integer)
                m_newIndex = value
            End Set
        End Property
        Public Sub New(tabPage__1 As DevExpress.XtraTab.XtraTabPage, newIndex__2 As Integer)
            TabPage = tabPage__1
            NewIndex = newIndex__2
        End Sub
    End Class

#End Region

#Region " Drag Window Class"
    'Drag
    Public Class DragInfo
        Public Property InitialMouseCoords As Point
        Public Property InitialLocation As Point

        Public Sub New(ByVal MouseCoords As Point, ByVal Location As Point)
            InitialMouseCoords = MouseCoords
            InitialLocation = Location
        End Sub
        Public Function NewLocation(ByVal MouseCoords As Point) As Point
            Dim loc As New Point(InitialLocation.X + (MouseCoords.X - InitialMouseCoords.X), InitialLocation.Y + (MouseCoords.Y - InitialMouseCoords.Y))
            Return loc
        End Function
    End Class
#End Region



End Namespace



Namespace Utilities

#Region " Drag and Drop on Administrator Apps"


    Public Class ElevatedDragDropManager
        Implements IMessageFilter

#Region " P/Invoke"
        <DllImport("user32.dll")> _
        Private Shared Function ChangeWindowMessageFilterEx(hWnd As IntPtr, msg As UInteger, action As ChangeWindowMessageFilterExAction, ByRef changeInfo As CHANGEFILTERSTRUCT) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        <DllImport("user32.dll")> _
        Private Shared Function ChangeWindowMessageFilter(msg As UInteger, flags As ChangeWindowMessageFilterFlags) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        <DllImport("shell32.dll")> _
        Private Shared Sub DragAcceptFiles(hwnd As IntPtr, fAccept As Boolean)
        End Sub

        <DllImport("shell32.dll")> _
        Private Shared Function DragQueryFile(hDrop As IntPtr, iFile As UInteger, <Out()> lpszFile As StringBuilder, cch As UInteger) As UInteger
        End Function

        <DllImport("shell32.dll")> _
        Private Shared Function DragQueryPoint(hDrop As IntPtr, ByRef lppt As POINT) As Boolean
        End Function

        <DllImport("shell32.dll")> _
        Private Shared Sub DragFinish(hDrop As IntPtr)
        End Sub

        <StructLayout(LayoutKind.Sequential)> _
        Private Structure POINT
            Public X As Integer
            Public Y As Integer

            Public Sub New(newX As Integer, newY As Integer)
                X = newX
                Y = newY
            End Sub

            Public Shared Widening Operator CType(p As POINT) As System.Drawing.Point
                Return New System.Drawing.Point(p.X, p.Y)
            End Operator

            Public Shared Widening Operator CType(p As System.Drawing.Point) As POINT
                Return New POINT(p.X, p.Y)
            End Operator
        End Structure

        Private Enum MessageFilterInfo As UInteger
            None
            AlreadyAllowed
            AlreadyDisAllowed
            AllowedHigher
        End Enum

        Private Enum ChangeWindowMessageFilterExAction As UInteger
            Reset
            Allow
            Disallow
        End Enum

        Private Enum ChangeWindowMessageFilterFlags As UInteger
            Add = 1
            Remove = 2
        End Enum

        <StructLayout(LayoutKind.Sequential)> _
        Private Structure CHANGEFILTERSTRUCT
            Public cbSize As UInteger
            Public ExtStatus As MessageFilterInfo
        End Structure
#End Region
#Region " Declarations"
        Public Shared Instance As New ElevatedDragDropManager()
        Public Event ElevatedDragDrop As EventHandler(Of ElevatedDragDropArgs)

        Private Const WM_DROPFILES As UInteger = &H233
        Private Const WM_COPYDATA As UInteger = &H4A
        Private Const WM_COPYGLOBALDATA As UInteger = &H49

        Private ReadOnly IsVistaOrHigher As Boolean = Environment.OSVersion.Version.Major >= 6
        Private ReadOnly Is7OrHigher As Boolean = (Environment.OSVersion.Version.Major = 6 AndAlso Environment.OSVersion.Version.Minor >= 1) OrElse Environment.OSVersion.Version.Major > 6

#End Region

        Protected Sub New()
            Application.AddMessageFilter(Me)
        End Sub

        Public Sub EnableDragDrop(hWnd As IntPtr)
            If Is7OrHigher Then
                Dim changeStruct As New CHANGEFILTERSTRUCT()
                changeStruct.cbSize = CUInt(Marshal.SizeOf(GetType(CHANGEFILTERSTRUCT)))
                ChangeWindowMessageFilterEx(hWnd, WM_DROPFILES, ChangeWindowMessageFilterExAction.Allow, changeStruct)
                ChangeWindowMessageFilterEx(hWnd, WM_COPYDATA, ChangeWindowMessageFilterExAction.Allow, changeStruct)
                ChangeWindowMessageFilterEx(hWnd, WM_COPYGLOBALDATA, ChangeWindowMessageFilterExAction.Allow, changeStruct)
            ElseIf IsVistaOrHigher Then
                ChangeWindowMessageFilter(WM_DROPFILES, ChangeWindowMessageFilterFlags.Add)
                ChangeWindowMessageFilter(WM_COPYDATA, ChangeWindowMessageFilterFlags.Add)
                ChangeWindowMessageFilter(WM_COPYGLOBALDATA, ChangeWindowMessageFilterFlags.Add)
            End If

            DragAcceptFiles(hWnd, True)
        End Sub

        Public Function PreFilterMessage(ByRef m As Message) As Boolean Implements IMessageFilter.PreFilterMessage
            If m.Msg = WM_DROPFILES Then
                HandleDragDropMessage(m)
                Return True
            End If

            Return False
        End Function

        Private Sub HandleDragDropMessage(m As Message)
            Dim sb = New StringBuilder(260)
            Dim numFiles As UInteger = DragQueryFile(m.WParam, &HFFFFFFFFUI, sb, 0)
            Dim list = New List(Of String)()

            For i As UInteger = 0 To numFiles - 1
                If DragQueryFile(m.WParam, i, sb, CUInt(sb.Capacity) * 2) > 0 Then
                    list.Add(sb.ToString())
                End If
            Next

            Dim p As POINT
            DragQueryPoint(m.WParam, p)
            DragFinish(m.WParam)

            Dim args = New ElevatedDragDropArgs()
            args.HWnd = m.HWnd
            args.Files = list
            args.X = p.X
            args.Y = p.Y

            RaiseEvent ElevatedDragDrop(Me, args)
        End Sub
    End Class

    Public Class ElevatedDragDropArgs
        Inherits EventArgs
        Public Property HWnd() As IntPtr
            Get
                Return m_HWnd
            End Get
            Set(value As IntPtr)
                m_HWnd = value
            End Set
        End Property
        Private m_HWnd As IntPtr
        Public Property Files() As List(Of String)
            Get
                Return m_Files
            End Get
            Set(value As List(Of String))
                m_Files = value
            End Set
        End Property
        Private m_Files As List(Of String)
        Public Property X() As Integer
            Get
                Return m_X
            End Get
            Set(value As Integer)
                m_X = value
            End Set
        End Property
        Private m_X As Integer
        Public Property Y() As Integer
            Get
                Return m_Y
            End Get
            Set(value As Integer)
                m_Y = value
            End Set
        End Property
        Private m_Y As Integer

        Public Sub New()
            Files = New List(Of String)()
        End Sub
    End Class

#End Region

End Namespace