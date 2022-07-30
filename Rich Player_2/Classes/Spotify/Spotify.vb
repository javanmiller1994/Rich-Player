#Region " Instructions "
' Spotify API V0.01 beta - a very quick First draft
' © august 3 2009 by Steffest
' This code is free to use in any way you want and comes with NO WARRANTIES
' tested with Spotify 0.3.18
' Usage:
' 
' Dim Spotify As New Spotify()
' 
' Spotify.PlayPause()
' Spotify.PlayPrev()
' Spotify.PlayNext()
' Spotify.Mute()
' Spotify.VolumeUp()
' Spotify.VolumeDown()
' Spotify.Nowplaying() (Gets the name of the current playing track)
' Spotify.Search("Artist",False) (Searches for "Artist")
' Spotify.Search("Artist",True) (Searches for "Artist" and starts playing the results)
#End Region
#Region " Imports "

Imports System.Environment
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

#End Region

Public Class Spotify

#Region " win32 "
    Private Declare Auto Function FindWindow Lib "user32" (ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    '   Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    Private Declare Auto Function SetForegroundWindow Lib "user32" (ByVal hWnd As IntPtr) As Boolean
    Private Declare Auto Function keybd_event Lib "user32" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo As Integer) As Boolean
    Private Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Integer)
    Private Declare Auto Function GetWindowText Lib "user32" (ByVal hwnd As IntPtr, ByVal lpString As String, ByVal cch As IntPtr) As IntPtr
    Private Declare Auto Function SetWindowText Lib "user32" (ByVal hwnd As IntPtr, ByVal lpString As String) As Boolean
    <DllImport("User32")> Private Shared Function ShowWindow(ByVal hwnd As Integer, ByVal nCmdShow As Integer) As Integer
    End Function
    Private Declare Auto Function EnumChildWindows Lib "user32" (ByVal hWndParent As Long, ByVal lpEnumFunc As Long, ByVal lParam As Long) As Long

    'Get Text
    Private Const WM_GETTEXT As Integer = &HD
    Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal msg As Integer, _
    ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function FindWindowEx(ByVal parentHandle As IntPtr, _
                                     ByVal childAfter As IntPtr, _
                                     ByVal lclassName As String, _
                                     ByVal windowTitle As String) As IntPtr
    End Function
    <DllImport("USER32.DLL", EntryPoint:="GetActiveWindow", SetLastError:=True,
    CharSet:=CharSet.Unicode, ExactSpelling:=False,
    CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function GetActiveWindowHandle() As System.IntPtr
    End Function

    <DllImport("USER32.DLL", EntryPoint:="GetWindowText", SetLastError:=True,
        CharSet:=CharSet.Unicode, ExactSpelling:=False,
        CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function GetActiveWindowText(ByVal hWnd As System.IntPtr, _
                                            ByVal lpString As System.Text.StringBuilder, _
                                            ByVal cch As Integer) As Integer
    End Function

    '  Declare Sub keybd_event Lib "user32.dll" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Int32, ByVal dwExtraInfo As Int32)

    Public Shared Sub SendKey(ByVal key As Keys)
        keybd_event(CByte(key), 0, 0, 0)
    End Sub

  

#End Region

#Region " Constants "
    Private Const WM_KEYDOWN = &H100
    Private Const WM_KEYUP = &H101
    Private Const WM_MOUSEACTIVATE = &H21
    Private Const KEYEVENTF_EXTENDEDKEY As Integer = &H1S
    Private Const KEYEVENTF_KEYUP As Integer = &H2S
#End Region

    Private w As Integer
#Region " Set Foreground Window "

    'Spotify
    Public Sub New()
        w = FindWindow("SpotifyMainWindow", vbNullString)
    End Sub
    Dim TempActiveWindow As String
    Public Function GetActiveWindow()
        Dim caption As New System.Text.StringBuilder(256)
        Dim hWnd As IntPtr = GetActiveWindowHandle()
        GetActiveWindowText(hWnd, caption, caption.Capacity)

        TempActiveWindow = caption.ToString
        ' MsgBox(caption.ToString)
        '  w = FindWindow(TempActiveWindow, vbNullString)

        Dim processRunning As Process() = Process.GetProcesses()
        For Each pr As Process In processRunning
            If pr.ProcessName = TempActiveWindow Then
                w = pr.MainWindowHandle.ToInt32()
                SetForegroundWindow(w)
            End If
        Next

        ' SetForegroundWindow(w)

    End Function
    Public Function RefreshConnection() As Boolean
        w = FindWindow("SpotifyMainWindow", vbNullString)
    End Function
    Public Function SetForegroundWindow_Spotify()
        GetActiveWindow()

        Sleep(200)

        RefreshConnection()
        SetForegroundWindow(w)
    End Function

    'Rich Player
    Public Function SetForegroundWindow_RichPlayer()
        If IsShown Then Exit Function
        Sleep(100)

        Dim processRunning As Process() = Process.GetProcesses()
        For Each pr As Process In processRunning
            If pr.ProcessName = "Rich Player.vshost" Then
                w = pr.MainWindowHandle.ToInt32()
                SetForegroundWindow(w)
            ElseIf pr.ProcessName = "Rich Player" Then
                w = pr.MainWindowHandle.ToInt32()
                SetForegroundWindow(w)
            End If
        Next

    End Function

#End Region

    Public Function IsSpotifyRunning() As Integer
        Return w
    End Function

    Public Function ShowNowPlaying() As Boolean
        SetForegroundWindow_Spotify()

        keybd_event(Keys.ControlKey, &H1D, 0, 0)
        keybd_event(Keys.N, &H45S, 0, 0)
        keybd_event(Keys.N, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        '  Sleep(50)
        keybd_event(Keys.ControlKey, &H1D, KEYEVENTF_KEYUP, 0)

        keybd_event(Keys.Escape, &H45S, 0, 0)
        keybd_event(Keys.Escape, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)

        keybd_event(Keys.Up, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Up, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.PageUp, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.PageUp, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.PageUp, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.PageUp, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)

        keybd_event(Keys.Down, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Down, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.Down, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Down, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        'Shell("C:/Program Files/Spotify/spotify.exe") FIXME - Spotify now saved in user folder

    End Function

    Public Function ShowSpotify() As Boolean
        If IO.File.Exists(GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\Spotify\Spotify.exe") Then

            Process.Start(GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\Spotify\Spotify.exe")
            '   Rich_Player.CsWinFormsBlackApp.Form1.BarCheckBox_UseSpotifyold.Checked = True

        Else
            MessageBox.Show("Spotify not found! May not be installed." & Environment.NewLine & "Please locate spotify after clicking OK.") '_

            Dim opener As New OpenFileDialog
            opener.Filter = "Spotify |spotify.exe"

            If opener.ShowDialog = DialogResult.OK Then
                Process.Start(opener.FileName)
                '   Rich_Player.CsWinFormsBlackApp.Form1.BarCheckBox_UseSpotifyold.Checked = True
            End If

        End If

        SetForegroundWindow_Spotify()
        keybd_event(Keys.Enter, 0, 0, 0)
    End Function

#Region " Playback "

    'Play / Pause
    Public Function PlayPause() As Boolean
        SendKey(Keys.MediaPlayPause)


        Exit Function
        '........Original Coding..........

        SetForegroundWindow_Spotify()
        keybd_event(Keys.Space, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Space, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)

        'SendMessage(w, WM_KEYDOWN, Keys.Space, 0)
        'SendMessage(w, WM_KEYUP, Keys.Space, 0)

        SetForegroundWindow_RichPlayer()

    End Function

    'Previous Song
    Public Function PlayPrev() As Boolean
        SendKey(Keys.MediaPreviousTrack)


        Exit Function
        '........Original Coding..........



        ' for some reason the PostMessage(w, WM_KEYDOWN, Keys.MediaNextTrack, 0) doesn't work
        ' sending ctrl+ commands to a windows still is a PITA ...
        SetForegroundWindow_Spotify()
        keybd_event(Keys.ControlKey, &H1D, 0, 0)
        keybd_event(Keys.Left, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Left, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        Try
            'Sleep(100) ' wait until spotify has trapped the control key before releasing it
        Catch ex As Exception

        End Try
        keybd_event(Keys.ControlKey, &H1D, KEYEVENTF_KEYUP, 0)
        SetForegroundWindow_RichPlayer()
    End Function

    'Skip Backwards
    Public Function PlayBackwards() As Boolean
        ' for some reason the PostMessage(w, WM_KEYDOWN, Keys.MediaNextTrack, 0) doesn't work
        ' sending ctrl+ commands to a windows still is a PITA ...
        SetForegroundWindow_Spotify()
        keybd_event(Keys.ShiftKey, &H1D, 0, 0)
        keybd_event(Keys.Left, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Left, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        Try
            'Sleep(100) ' wait until spotify has trapped the control key before releasing it
        Catch ex As Exception

        End Try
        keybd_event(Keys.ShiftKey, &H1D, KEYEVENTF_KEYUP, 0)
        SetForegroundWindow_RichPlayer()
    End Function

    'Next Song
    Public Function PlayNext() As Boolean
        SendKey(Keys.MediaNextTrack)


        Exit Function
        '........Original Coding..........

        SetForegroundWindow_Spotify()
        keybd_event(Keys.ControlKey, &H1D, 0, 0)
        keybd_event(Keys.Right, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Right, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        Try
            'Sleep(100) ' wait until spotify has trapped the control key before releasing it
        Catch ex As Exception

        End Try
        keybd_event(Keys.ControlKey, &H1D, KEYEVENTF_KEYUP, 0)
        SetForegroundWindow_RichPlayer()
    End Function

    'Skip Forward
    Public Function PlayForward() As Boolean
        SetForegroundWindow_Spotify()
        keybd_event(Keys.ShiftKey, &H1D, 0, 0)
        keybd_event(Keys.Right, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Right, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        Try
            'Sleep(100) ' wait until spotify has trapped the control key before releasing it
        Catch ex As Exception

        End Try
        keybd_event(Keys.ShiftKey, &H1D, KEYEVENTF_KEYUP, 0)
        SetForegroundWindow_RichPlayer()
    End Function

#End Region

#Region " Repeat & Shuffle"
    Public Function Repeat() As Boolean
        SetForegroundWindow_Spotify()
        keybd_event(Keys.ControlKey, &H1D, 0, 0)
        keybd_event(Keys.R, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.R, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        Try
            'Sleep(100) ' wait until spotify has trapped the control key before releasing it
        Catch ex As Exception

        End Try
        keybd_event(Keys.ControlKey, &H1D, KEYEVENTF_KEYUP, 0)
        SetForegroundWindow_RichPlayer()
    End Function
    Public Function Shuffle() As Boolean
        SetForegroundWindow_Spotify()
        keybd_event(Keys.ControlKey, &H1D, 0, 0)
        keybd_event(Keys.S, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.S, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        Try
            'Sleep(100) ' wait until spotify has trapped the control key before releasing it
        Catch ex As Exception

        End Try
        keybd_event(Keys.ControlKey, &H1D, KEYEVENTF_KEYUP, 0)
        SetForegroundWindow_RichPlayer()
    End Function

#End Region

#Region " Volume "
    'Up
    Public Function VolumeUp() As Boolean
        SetForegroundWindow_Spotify()
        keybd_event(Keys.ControlKey, &H1D, 0, 0)
        keybd_event(Keys.Up, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Up, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        Try
            'Sleep(100) ' wait until spotify has trapped the control key before releasing it
        Catch ex As Exception

        End Try
        keybd_event(Keys.ControlKey, &H1D, KEYEVENTF_KEYUP, 0)
        SetForegroundWindow_RichPlayer()
    End Function

    'Down
    Public Function VolumeDown() As Boolean
        SetForegroundWindow_Spotify()
        keybd_event(Keys.ControlKey, &H1D, 0, 0)
        keybd_event(Keys.Down, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Down, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        Try
            ' 'Sleep(100) ' wait until spotify has trapped the control key before releasing it
        Catch ex As Exception

        End Try
        keybd_event(Keys.ControlKey, &H1D, KEYEVENTF_KEYUP, 0)
        SetForegroundWindow_RichPlayer()
    End Function

    'Mute
    Public Function Mute() As Boolean
        SetForegroundWindow_Spotify()
        keybd_event(Keys.ControlKey, &H1D, 0, 0)
        keybd_event(Keys.ShiftKey, &H1D, 0, 0)
        keybd_event(Keys.Down, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Down, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        Try
            'Sleep(100) ' wait until spotify has trapped the control key before releasing it
        Catch ex As Exception

        End Try

        keybd_event(Keys.ShiftKey, &H1D, KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.ControlKey, &H1D, KEYEVENTF_KEYUP, 0)
        SetForegroundWindow_RichPlayer()
    End Function

#End Region

    Public Function Get_NowPlaying() As String
        SetForegroundWindow_Spotify()
        RefreshConnection

        'Find the running notepad window
        Dim Hwnd As IntPtr = w 'FindWindow("Chrome_RenderWidgetHostHWND", "ChromeLegacyWindow")  'Nothing, "Chrome_RenderWidgetHostHWND")


        'Alloc memory for the buffer that recieves the text
        Dim Handle As IntPtr = Marshal.AllocHGlobal(100)

        'send WM_GWTTEXT message to the notepad window
        Dim NumText As Integer = SendMessage(Hwnd, WM_GETTEXT, 50, Handle)

        'copy the characters from the unmanaged memory to a managed string
        Dim Text As String = Marshal.PtrToStringUni(Handle)



       
        'Find the Edit control of the Running Notepad
        Dim ChildHandle As IntPtr = FindWindowEx(Hwnd, IntPtr.Zero, "Chrome_RenderWidgetHostHWND", "ChromeLegacyWindow")

        'Alloc memory for the buffer that recieves the text
        Dim Hndl As IntPtr = Marshal.AllocHGlobal(100)

        'Send The WM_GETTEXT Message
        NumText = SendMessage(ChildHandle, WM_GETTEXT, 200, Hndl)

        'copy the characters from the unmanaged memory to a managed string
        Text = Marshal.PtrToStringUni(Hndl)

        'Display the string using a label
        ''  Label2.Text = Text
        SetForegroundWindow_RichPlayer()
        Return Text

    End Function

    Public Function Nowplaying() As String
        keybd_event(Keys.ShiftKey, &H1D, 0, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Tab, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)

        keybd_event(Keys.ShiftKey, &H1D, KEYEVENTF_KEYUP, 0)


        Dim lpText As String
        lpText = New String(Chr(0), 100)
        Dim intLength As Integer = GetWindowText(w, lpText, lpText.Length)



        '''

        'Dim lpText As String
        '    lpText = New String(Chr(0), 100)
        '  Dim intLength As Integer = GetWindowText(w, lpText, lpText.Length)

        '''   Dim TrackName As String = 

        '   If (intLength <= 0) OrElse (intLength > lpText.Length) Then Return "Unknown"
        '     Dim strTitle As String = lpText.Substring(0, intLength)
        '      strTitle = Mid(strTitle, 1)
        '     Return strTitle
    End Function

    Public Function Search(ByVal s As String, ByVal AndPlay As Boolean) As Boolean
        SetForegroundWindow_Spotify()


        keybd_event(Keys.ControlKey, &H1D, 0, 0)
        keybd_event(Keys.L, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.L, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        Try
            'Sleep(100) ' wait until spotify has trapped the control key before releasing it
        Catch ex As Exception

        End Try
        keybd_event(Keys.ControlKey, &H1D, KEYEVENTF_KEYUP, 0)

        Sleep(100)

        keybd_event(Keys.ControlKey, &H1D, 0, 0)
        keybd_event(Keys.A, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.A, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)
        keybd_event(Keys.ControlKey, &H1D, KEYEVENTF_KEYUP, 0)

        keybd_event(Keys.Delete, &H45S, KEYEVENTF_EXTENDEDKEY Or 0, 0)
        keybd_event(Keys.Delete, &H45S, KEYEVENTF_EXTENDEDKEY Or KEYEVENTF_KEYUP, 0)

        Sleep(100)


        SendKeys.SendWait(s & Chr(13))

        If AndPlay Then
            ' this is a bit stupid but works in this version: press tab twice, then enter
            Try
                'Sleep(100) ' wait until spotify has trapped the control key before releasing it
            Catch ex As Exception

            End Try
            SendKeys.SendWait(Chr(9) & Chr(9) & Chr(13))
        End If

    End Function

#Region " Hide / Show Spotify "
    Dim IsShown As Boolean = False
    Public Function ToggleHide() As Boolean
        SetForegroundWindow_Spotify()

        'Check Hidden Status
        If IsShown Then
            ShowWindow(w, 9)
            IsShown = False
        Else
            ShowWindow(w, 0)
            IsShown = True
        End If

        SetForegroundWindow_RichPlayer()

    End Function
#End Region

End Class
