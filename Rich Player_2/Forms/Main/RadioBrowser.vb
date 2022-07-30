Imports WindowsInput.Native
Imports Rich_Player.CsWinFormsBlackApp

Public Class RadioBrowser
#Region " Send Clicks"

#Region " Declare Send Clicks"
    Dim Ctrl = VirtualKeyCode.CONTROL
    Dim Alt = VirtualKeyCode.MENU
    Dim Shift = VirtualKeyCode.SHIFT



#End Region

    Public Function SendClick(m As System.Collections.Generic.IEnumerable(Of VirtualKeyCode), k As VirtualKeyCode)
        Dim keyboard As New WindowsInput.InputSimulator
        keyboard.Keyboard.ModifiedKeyStroke(m, k)
    End Function

#End Region
    Dim ChromeWeb As New Gecko.GeckoWebBrowser

#Region " Load  |   Close"

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

    Private Sub RadioBrowser_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Do Until IsOnScreen(Me)
            Dim i As Integer = Me.Top - 1
            Me.Top = i
        Loop


        Me.Controls.Add(ChromeWeb)
        ChromeWeb.BringToFront()
        ChromeWeb.Dock = DockStyle.Fill
        ChromeWeb.Navigate("https://onlineradiobox.com/")

        AddHandler ChromeWeb.DocumentCompleted, AddressOf GeckoFX_DocumentCompleted
    End Sub
    Private Sub RadioBrowser_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        My.Settings.YouTubeOpened = False
        My.Settings.RadioOpened = True

        My.Settings.Save()
    End Sub
    Private Sub RadioBrowser_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        My.Settings.RadioOpened = False
        My.Settings.Save()
    End Sub

#End Region

    Public Sub GeckoFX_DocumentCompleted(sender As Object, e As Gecko.Events.GeckoDocumentCompletedEventArgs)
        Try
            ChromeWeb.GetDocShellAttribute.GetContentViewerAttribute.SetFullZoomAttribute(0.75)
            Me.BringToFront()
        Catch ex As Exception
        End Try
    End Sub


#Region " Navigation"

    Private Sub but_Back_Click(sender As Object, e As EventArgs) Handles but_Back.Click
        '  YTBrowser.GoBack()
        ChromeWeb.GoBack()
    End Sub

    Private Sub but_Forward_Click(sender As Object, e As EventArgs) Handles but_Forward.Click
        ChromeWeb.GoForward()
    End Sub

    Private Sub but_Refresh_Click(sender As Object, e As EventArgs) Handles but_Refresh.Click
        ChromeWeb.Reload()
    End Sub


#End Region

#Region " Add to Playlist"

    Private Sub but_Add_Click(sender As Object, e As EventArgs) Handles but_Add.Click
        My.Settings.RadioURL = ChromeWeb.Url.ToString
        My.Settings.RadioStation = tb_StationName.Text
        My.Settings.Save()
        If My.Settings.StopKeyCtrl = 2 Then
            SendClick({Ctrl}, My.Settings.StopKey)
        Else
            SendClick(Nothing, My.Settings.StopKey)
        End If


    End Sub

#End Region



End Class